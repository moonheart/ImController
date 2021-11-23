using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Validation;

namespace Lenovo.Modern.ImController.Shared.Utilities.Ipc
{
	// Token: 0x02000043 RID: 67
	public class NamedPipeServer : IDisposable
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060001D3 RID: 467 RVA: 0x00009274 File Offset: 0x00007474
		// (remove) Token: 0x060001D4 RID: 468 RVA: 0x000092AC File Offset: 0x000074AC
		public event ServerMessageDelegate PipeMessageHandler;

		// Token: 0x060001D5 RID: 469 RVA: 0x000092E4 File Offset: 0x000074E4
		public NamedPipeServer(string guidName, bool verifyParentProcess = false)
		{
			if (string.IsNullOrWhiteSpace(guidName))
			{
				throw new MissingMemberException("Missing dependencies for pipe server");
			}
			this._verifyParentProcess = verifyParentProcess;
			this._pipeName = NamedPipeServer._pipePrefix + guidName;
			this._liveEventName = "Global\\_liveevent" + guidName;
			EventWaitHandleAccessRule rule = new EventWaitHandleAccessRule(new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null), EventWaitHandleRights.FullControl, AccessControlType.Allow);
			EventWaitHandleSecurity eventWaitHandleSecurity = new EventWaitHandleSecurity();
			eventWaitHandleSecurity.AddAccessRule(rule);
			bool flag = false;
			this._liveEvent = new EventWaitHandle(false, EventResetMode.ManualReset, this._liveEventName, ref flag, eventWaitHandleSecurity);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000937C File Offset: 0x0000757C
		protected void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing)
			{
				this._liveEvent.Reset();
				this._liveEvent.Dispose();
				this._liveEvent = null;
				if (this._currentPipeServerStream != null)
				{
					try
					{
						this._currentPipeServerStream.Disconnect();
					}
					catch (Exception)
					{
					}
					try
					{
						this._currentPipeServerStream.Close();
					}
					catch (Exception)
					{
					}
					this._currentPipeServerStream.Dispose();
					this._currentPipeServerStream = null;
				}
				this._disposed = true;
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00009414 File Offset: 0x00007614
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000941D File Offset: 0x0000761D
		public bool IsDisposedOrCancelled()
		{
			return this._cancelRequested || this._disposed;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00009430 File Offset: 0x00007630
		public void Listen()
		{
			Logger.Log(Logger.LogSeverity.Information, "NamedPipeServer: Pipe:{0}. Listening...", new object[] { this._pipeName });
			if (this._currentPipeServerStream != null)
			{
				throw new InvalidOperationException("Existing pipe already started");
			}
			this._cancelRequested = false;
			Task.Factory.StartNew(new Action(this.DoListenAndRespond), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
			Thread.Sleep(10);
			this._liveEvent.Set();
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000094A6 File Offset: 0x000076A6
		public void Cancel()
		{
			this._liveEvent.Reset();
			this._cancelRequested = true;
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001DB RID: 475 RVA: 0x000094BC File Offset: 0x000076BC
		private static bool IsRunningAsAdmin
		{
			get
			{
				bool result = false;
				using (WindowsIdentity current = WindowsIdentity.GetCurrent())
				{
					result = new WindowsPrincipal(current).IsInRole(WindowsBuiltInRole.Administrator);
				}
				return result;
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00009500 File Offset: 0x00007700
		private bool IsAdminProcess(Process process)
		{
			bool result = false;
			SafeTokenHandle safeTokenHandle = null;
			try
			{
				if (Win32.OpenProcessToken(process.Handle, Win32.TOKEN_QUERY | Win32.TOKEN_DUPLICATE, out safeTokenHandle))
				{
					result = new WindowsPrincipal(new WindowsIdentity(safeTokenHandle.DangerousGetHandle())).IsInRole(WindowsBuiltInRole.Administrator);
				}
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Warning, "{0}: exception occured in isAdminProcess", new object[] { ex.Message });
			}
			finally
			{
				if (safeTokenHandle != null)
				{
					safeTokenHandle.Close();
					safeTokenHandle = null;
				}
			}
			return result;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009590 File Offset: 0x00007790
		private string GetPathToApp(Process process)
		{
			string result = string.Empty;
			if (process != null)
			{
				uint capacity = 256U;
				StringBuilder stringBuilder = new StringBuilder((int)capacity);
				uint num = Win32.QueryFullProcessImageName(process.Handle, 0U, stringBuilder, out capacity);
				if (num != 0U)
				{
					result = stringBuilder.ToString();
				}
				else
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					Logger.Log(Logger.LogSeverity.Warning, string.Format("{0}: error when calling GetProcessImageFileName", lastWin32Error));
				}
			}
			return result;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000095F0 File Offset: 0x000077F0
		private bool ValidateConnectedProcess()
		{
			bool flag = false;
			if (NamedPipeServer._currentProcessIsImc)
			{
				return true;
			}
			try
			{
				int processId = 0;
				if (Win32.GetNamedPipeClientProcessId(this._currentPipeServerStream.SafePipeHandle, out processId))
				{
					Process processById = Process.GetProcessById(processId);
					string text = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
					text += "\\lenovo\\imcontroller\\service\\Lenovo.Modern.ImController.exe";
					if (NamedPipeServer.IsRunningAsAdmin)
					{
						if (this.IsAdminProcess(processById))
						{
							string pathToApp = this.GetPathToApp(processById);
							flag = processById.SessionId == 0 && string.Compare(pathToApp, text, StringComparison.InvariantCultureIgnoreCase) == 0;
							if (flag)
							{
								flag = false;
								flag = ((IImcCertificateValidator)new ImcCertificateValidator()).AssertDigitalSignatureIsValid(pathToApp);
								if (!flag)
								{
									Logger.Log(Logger.LogSeverity.Information, "Failed to validate imc signature");
								}
							}
							else
							{
								Logger.Log(Logger.LogSeverity.Information, "connected process path is not that of imc: {0}", new object[] { pathToApp });
							}
						}
						else
						{
							Logger.Log(Logger.LogSeverity.Warning, "connected process is not running as admin");
						}
					}
					else
					{
						flag = processById.SessionId == 0 && string.Compare(processById.ProcessName, "Lenovo.Modern.ImController", StringComparison.InvariantCultureIgnoreCase) == 0;
					}
				}
			}
			catch (Exception)
			{
			}
			return flag;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000096EC File Offset: 0x000078EC
		private void DoListenAndRespond()
		{
			try
			{
				this._currentPipeServerStream = this.CreatePipeServerStream();
				this._currentPipeServerStream.WaitForConnection();
				Logger.Log(Logger.LogSeverity.Information, "NamedPipeServer: Pipe:{0}. Incoming connection..", new object[] { this._pipeName });
				if (this.ValidateConnectedProcess())
				{
					Logger.Log(Logger.LogSeverity.Warning, "NamedPipeServer: Pipe:{0}. NamedPipeServer: Pipe is connected by IMC", new object[] { this._pipeName });
					while (!this._cancelRequested)
					{
						string text = this.ReceiveMessage(this._currentPipeServerStream);
						if (!string.IsNullOrWhiteSpace(text))
						{
							this.MessageHandler(text);
						}
					}
				}
				else
				{
					Logger.Log(Logger.LogSeverity.Warning, "NamedPipeServer: Pipe:{0}. NamedPipeServer: Pipe is not connected by IMC", new object[] { this._pipeName });
				}
			}
			catch (ObjectDisposedException ex)
			{
				Logger.Log(Logger.LogSeverity.Warning, "{0}: NamedPipeServer: Pipe:{1}. NamedPipeServer: Pipe is closed", new object[] { ex.Message, this._pipeName });
			}
			catch (InvalidOperationException ex2)
			{
				Logger.Log(Logger.LogSeverity.Warning, "{0}: NamedPipeServer: Pipe:{1}. Pipe state is off sync", new object[] { ex2.Message, this._pipeName });
			}
			catch (IOException ex3)
			{
				Logger.Log(Logger.LogSeverity.Warning, "{0}: NamedPipeServer: Pipe:{1}. NamedPipeServer: Pipe is broken", new object[] { ex3.Message, this._pipeName });
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000982C File Offset: 0x00007A2C
		private void MessageHandler(string msg)
		{
			if (this.PipeMessageHandler != null)
			{
				this.PipeMessageHandler(msg);
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00009844 File Offset: 0x00007A44
		private string ReceiveMessage(NamedPipeServerStream stream)
		{
			Logger.Log(Logger.LogSeverity.Information, "NamedPipeServer: Pipe:{0}.  Pipe server: Waiting for new message", new object[] { this._pipeName });
			int num = 0;
			while (num == 0 && !this._cancelRequested)
			{
				num = stream.Read(this._messageBuffer, 0, NamedPipeServer._BUF_SIZE);
				if (num == 0)
				{
					Thread.Sleep(100);
				}
			}
			Logger.Log(Logger.LogSeverity.Information, "NamedPipeServer: Pipe:{0}. Read {1} bytes...", new object[] { this._pipeName, num });
			string @string = Encoding.UTF8.GetString(this._messageBuffer, 0, num);
			if (Logger.IsLoggingEnabledForSeverity(Logger.LogSeverity.Information))
			{
				Logger.Log(Logger.LogSeverity.Information, "NamedPipeServer: Pipe:{0}. Message received (WHOLE): {1} ", new object[] { this._pipeName, @string });
			}
			return @string;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000098F4 File Offset: 0x00007AF4
		private NamedPipeServerStream CreatePipeServerStream()
		{
			Logger.Log(Logger.LogSeverity.Information, "NamedPipeServer: Pipe:{0}. Creating new NamedPipeServerStream", new object[] { this._pipeName });
			PipeSecurity pipeSecurity = new PipeSecurity();
			PipeAccessRule rule = new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), PipeAccessRights.ReadWrite, AccessControlType.Allow);
			pipeSecurity.AddAccessRule(rule);
			return new NamedPipeServerStream(this._pipeName, PipeDirection.In, 128, PipeTransmissionMode.Message, PipeOptions.Asynchronous, NamedPipeServer._BUF_SIZE, NamedPipeServer._BUF_SIZE, pipeSecurity);
		}

		// Token: 0x040000F8 RID: 248
		private bool _verifyParentProcess;

		// Token: 0x040000F9 RID: 249
		protected static string _pipePrefix = "pipe_";

		// Token: 0x040000FA RID: 250
		private EventWaitHandle _liveEvent;

		// Token: 0x040000FB RID: 251
		protected static int _BUF_SIZE = 1048576;

		// Token: 0x040000FC RID: 252
		protected string _pipeName;

		// Token: 0x040000FD RID: 253
		protected string _liveEventName;

		// Token: 0x040000FE RID: 254
		private NamedPipeServerStream _currentPipeServerStream;

		// Token: 0x040000FF RID: 255
		private static bool _currentProcessIsImc = string.Compare(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\lenovo\\imcontroller\\service\\Lenovo.Modern.ImController.exe", Process.GetCurrentProcess().MainModule.FileName, StringComparison.InvariantCultureIgnoreCase) == 0;

		// Token: 0x04000100 RID: 256
		private bool _cancelRequested;

		// Token: 0x04000101 RID: 257
		private bool _disposed;

		// Token: 0x04000102 RID: 258
		private byte[] _messageBuffer = new byte[NamedPipeServer._BUF_SIZE];
	}
}
