using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.Shared.Utilities.Ipc
{
	// Token: 0x0200003E RID: 62
	public class NamedPipeClient : IDisposable
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060001B5 RID: 437 RVA: 0x00008D74 File Offset: 0x00006F74
		// (remove) Token: 0x060001B6 RID: 438 RVA: 0x00008DAC File Offset: 0x00006FAC
		public event CriticalErrorDelegate CriticalErrorHandler;

		// Token: 0x060001B7 RID: 439 RVA: 0x00008DE4 File Offset: 0x00006FE4
		public NamedPipeClient(string guidName)
		{
			this._pipeName = NamedPipeClient._pipePrefix + guidName;
			this._liveEventName = "Global\\_liveevent" + guidName;
			EventWaitHandleAccessRule rule = new EventWaitHandleAccessRule(new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null), EventWaitHandleRights.FullControl, AccessControlType.Allow);
			EventWaitHandleSecurity eventWaitHandleSecurity = new EventWaitHandleSecurity();
			eventWaitHandleSecurity.AddAccessRule(rule);
			bool flag = false;
			this._liveEvent = new EventWaitHandle(false, EventResetMode.ManualReset, this._liveEventName, ref flag, eventWaitHandleSecurity);
			this._cancellationToken = this._cancellationTokenSource.Token;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00008E84 File Offset: 0x00007084
		protected void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing)
			{
				this._liveEvent.Dispose();
				this._liveEvent = null;
				if (this._pipeStream != null)
				{
					try
					{
						this._pipeStream.Close();
					}
					catch (Exception)
					{
					}
					this._pipeStream.Dispose();
					this._pipeStream = null;
				}
				if (this._cancellationTokenSource != null)
				{
					try
					{
						this._cancellationTokenSource.Cancel();
					}
					catch (Exception)
					{
					}
				}
				this._disposed = true;
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00008F14 File Offset: 0x00007114
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00008F20 File Offset: 0x00007120
		public void Connect(CancellationToken connectCancelToken)
		{
			if (this._pipeStream != null)
			{
				throw new NamedPipeException("Already connected");
			}
			if (!this.WaitForServerLive(connectCancelToken))
			{
				throw new NamedPipeException("Server is not running");
			}
			this._pipeStream = new NamedPipeClientStream(".", this._pipeName, PipeDirection.Out, PipeOptions.None);
			bool flag = false;
			while (!flag)
			{
				try
				{
					this._pipeStream.Connect(0);
					this._connectRetries = 0;
					flag = true;
				}
				catch (IOException ex)
				{
					Logger.Log(ex, "IOException in NamedPipeClient");
					this._connectRetries++;
					if (this._connectRetries >= this._maxRetries)
					{
						throw new NamedPipeException("Pipe is broken");
					}
				}
				catch (TimeoutException ex2)
				{
					Logger.Log(ex2, "TimeoutException in NamedPipeClient");
					this._connectRetries++;
					if (this._connectRetries >= this._maxRetries)
					{
						throw new NamedPipeException("Server connection timeout occured when there should be no timeout");
					}
				}
				if (!flag)
				{
					Thread.Sleep(100);
				}
			}
			Task.Factory.StartNew(new Action(this.DoSend), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00009040 File Offset: 0x00007240
		public void Send(string sendStr)
		{
			if (this._pipeStream == null)
			{
				Logger.Log(Logger.LogSeverity.Error, "Pipe client error: Not connected to server");
				throw new NamedPipeException("Not connected to server");
			}
			if (NamedPipeClient._BUF_SIZE > Encoding.UTF8.GetBytes(sendStr).GetLength(0))
			{
				this._requestQueue.Add(sendStr);
				return;
			}
			throw new NamedPipeException("Buffer too big");
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000909C File Offset: 0x0000729C
		public void Close()
		{
			if (this._pipeStream != null)
			{
				try
				{
					this._pipeStream.Close();
				}
				catch (Exception)
				{
				}
				this._pipeStream.Dispose();
				this._pipeStream = null;
			}
			if (this._cancellationTokenSource != null)
			{
				try
				{
					this._cancellationTokenSource.Cancel();
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00009108 File Offset: 0x00007308
		public bool WaitForServerLive(CancellationToken waitCancelToken)
		{
			bool result;
			try
			{
				if (WaitHandle.WaitAny(new WaitHandle[] { this._liveEvent, waitCancelToken.WaitHandle }, NamedPipeClient._waitLiveTimeoutMS) == 0)
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception in NamedPipeClient.WaitForServerLive");
				result = false;
			}
			return result;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00009164 File Offset: 0x00007364
		private void DoSend()
		{
			while (this._pipeStream != null)
			{
				try
				{
					string text = this._requestQueue.Take(this._cancellationToken);
					if (text != null)
					{
						byte[] bytes = Encoding.UTF8.GetBytes(text);
						this._pipeStream.Write(bytes, 0, bytes.Length);
						this._pipeStream.Flush();
					}
				}
				catch (OperationCanceledException)
				{
					Logger.Log(Logger.LogSeverity.Information, "NamedPipeClient is being closed. Ending DoSend thread.");
					break;
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "NamedPipeClient: Critical error happened, will restart NamedPipeClient with the same pipe name: {0}", new object[] { this._pipeName });
					this.ErrorHandler();
					break;
				}
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00009208 File Offset: 0x00007408
		private void ErrorHandler()
		{
			if (this.CriticalErrorHandler != null)
			{
				this.CriticalErrorHandler();
			}
		}

		// Token: 0x040000E8 RID: 232
		private static readonly int _waitLiveTimeoutMS = 30000;

		// Token: 0x040000E9 RID: 233
		private readonly int _maxRetries = 50;

		// Token: 0x040000EA RID: 234
		private int _connectRetries;

		// Token: 0x040000EB RID: 235
		private NamedPipeClientStream _pipeStream;

		// Token: 0x040000EC RID: 236
		private string _pipeName;

		// Token: 0x040000ED RID: 237
		protected static string _pipePrefix = "pipe_";

		// Token: 0x040000EE RID: 238
		private bool _disposed;

		// Token: 0x040000EF RID: 239
		private EventWaitHandle _liveEvent;

		// Token: 0x040000F0 RID: 240
		private string _liveEventName;

		// Token: 0x040000F1 RID: 241
		private BlockingCollection<string> _requestQueue = new BlockingCollection<string>();

		// Token: 0x040000F2 RID: 242
		protected static int _BUF_SIZE = 16777216;

		// Token: 0x040000F3 RID: 243
		private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		// Token: 0x040000F4 RID: 244
		private CancellationToken _cancellationToken;
	}
}
