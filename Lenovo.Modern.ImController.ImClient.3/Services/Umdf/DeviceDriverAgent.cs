using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.ImClient.Interop;
using Lenovo.Modern.ImController.ImClient.Utilities;

namespace Lenovo.Modern.ImController.ImClient.Services.Umdf
{
	// Token: 0x0200001D RID: 29
	public class DeviceDriverAgent : IInterProcessRequester, IInterProcessResponder
	{
		// Token: 0x06000087 RID: 135 RVA: 0x00003B20 File Offset: 0x00001D20
		public static DeviceDriverAgent GetInstance()
		{
			if (DeviceDriverAgent._instance == null)
			{
				object syncRoot = DeviceDriverAgent.SyncRoot;
				lock (syncRoot)
				{
					if (DeviceDriverAgent._instance == null)
					{
						DeviceDriverAgent._instance = new DeviceDriverAgent();
					}
				}
			}
			return DeviceDriverAgent._instance;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003B78 File Offset: 0x00001D78
		private DeviceDriverAgent()
		{
			if (string.IsNullOrEmpty(DeviceDriver.DriverPath))
			{
				throw new DeviceDriverMissingException("Unable to initialize Im DeviceDriverAgent - path is null");
			}
			DeviceDriverAgent._timer = new Stopwatch();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003BB4 File Offset: 0x00001DB4
		~DeviceDriverAgent()
		{
			this.CloseDriver();
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003BE0 File Offset: 0x00001DE0
		private void LazyOpenDriver()
		{
			if (IntPtr.Zero == this._deviceHandle || new IntPtr(-1L) == this._deviceHandle)
			{
				SemaphoreSlim deviceSemaphore = DeviceDriverAgent.DeviceSemaphore;
				if (deviceSemaphore != null)
				{
					deviceSemaphore.Wait();
				}
				try
				{
					int num = 0;
					while ((IntPtr.Zero == this._deviceHandle || new IntPtr(-1L) == this._deviceHandle) && num < 10)
					{
						ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "Trying to open UMDF driver");
						if (DeviceDriverAgent._timer.IsRunning && DeviceDriverAgent._timer.Elapsed.TotalSeconds > 5.0)
						{
							this._deviceHandle = DeviceDriver.GetDeviceDriverHandle();
						}
						else if (!DeviceDriverAgent._timer.IsRunning)
						{
							this._deviceHandle = DeviceDriver.GetDeviceDriverHandle();
						}
						if (IntPtr.Zero == this._deviceHandle || new IntPtr(-1L) == this._deviceHandle)
						{
							ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Warning, "Failed to open UMDF driver, retrying...");
							Task.Delay(1000).Wait();
						}
						else
						{
							ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "UMDF driver opened successfully");
							if (this._deviceWatcher == null)
							{
								this._deviceWatcher = new DeviceWatcher(new OnDeviceRemovalDelegate(this.CloseDriverOnDeviceRemoval));
							}
							if (this._deviceWatcher != null)
							{
								this._deviceWatcher.StartMessagePump(this._deviceHandle);
							}
						}
						num++;
					}
				}
				catch (Exception ex)
				{
					ExternalLogger.Instance.Log(ex, "Exception occured while calling GetDeviceDriverHandle");
				}
				SemaphoreSlim deviceSemaphore2 = DeviceDriverAgent.DeviceSemaphore;
				if (deviceSemaphore2 != null)
				{
					deviceSemaphore2.Release();
				}
				if (IntPtr.Zero == this._deviceHandle || new IntPtr(-1L) == this._deviceHandle)
				{
					throw new DeviceDriverMissingException("Unable to initialize Im DeviceDriverAgent - handle is null");
				}
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003DC0 File Offset: 0x00001FC0
		public void CloseDriverOnDeviceRemoval()
		{
			SemaphoreSlim deviceSemaphore = DeviceDriverAgent.DeviceSemaphore;
			if (deviceSemaphore != null)
			{
				deviceSemaphore.Wait();
			}
			if (DeviceDriverAgent._timer.IsRunning)
			{
				DeviceDriverAgent._timer.Restart();
			}
			else
			{
				DeviceDriverAgent._timer.Start();
			}
			SemaphoreSlim deviceSemaphore2 = DeviceDriverAgent.DeviceSemaphore;
			if (deviceSemaphore2 != null)
			{
				deviceSemaphore2.Release();
			}
			this.CloseDriver();
			if (this._deviceWatcher != null)
			{
				try
				{
					this._deviceWatcher.StopMessagePump();
				}
				catch (Exception ex)
				{
					ExternalLogger.Instance.Log(ex, "Exception occured while calling deviceWatcher.StopMessagePump");
				}
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003E50 File Offset: 0x00002050
		public void CloseDriver()
		{
			SemaphoreSlim deviceSemaphore = DeviceDriverAgent.DeviceSemaphore;
			if (deviceSemaphore != null)
			{
				deviceSemaphore.Wait();
			}
			try
			{
				if (this._deviceHandle != IntPtr.Zero)
				{
					DeviceDriver.CloseDeviceDriverHandle(this._deviceHandle);
				}
			}
			catch (Exception ex)
			{
				ExternalLogger.Instance.Log(ex, "Exception occured while calling CloseDeviceDriverHandle");
			}
			this._deviceHandle = IntPtr.Zero;
			SemaphoreSlim deviceSemaphore2 = DeviceDriverAgent.DeviceSemaphore;
			if (deviceSemaphore2 == null)
			{
				return;
			}
			deviceSemaphore2.Release();
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003ECC File Offset: 0x000020CC
		public async Task<Tuple<Guid, string>> WaitForNextRequestAsync(CancellationToken cancelToken)
		{
			Tuple<Guid, string> resultPair = null;
			bool messageReceived = false;
			uint nSizeOutBuffer = 8192U;
			IntPtr outBuffer = Marshal.AllocHGlobal((int)nSizeOutBuffer);
			try
			{
				while (!messageReceived && !cancelToken.IsCancellationRequested)
				{
					uint bytesReturned = 0U;
					int num = 0;
					TaskAwaiter<bool> taskAwaiter = this.MakeIoControlCall(UmdfControlCodes.WaitForNextRequest, IntPtr.Zero, 0U, outBuffer, nSizeOutBuffer, ref bytesReturned, IntPtr.Zero, cancelToken, ref num).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						UmdfDriverData umdfDriverData = this.ConvertUnmanagedPtrtoUmdfDriverData(outBuffer, (int)bytesReturned);
						resultPair = Tuple.Create<Guid, string>(umdfDriverData.TaskId, umdfDriverData.XmlString);
						if (!umdfDriverData.TaskId.Equals(Guid.Empty))
						{
							messageReceived = true;
							this._requestCounter += 1U;
							ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, string.Format("{0} requests were received from UMDF driver", this._requestCounter));
						}
					}
					else
					{
						nSizeOutBuffer *= 2U;
						if (nSizeOutBuffer >= 262144U)
						{
							ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Error, string.Format("WaitForNextRequestAsync: Buffer requirement is too large. Failed size = {0}", nSizeOutBuffer));
							break;
						}
						ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, string.Format("WaitForNextRequestAsync: Reallocating buffer. New size = {0}", nSizeOutBuffer));
						Marshal.FreeHGlobal(outBuffer);
						outBuffer = Marshal.AllocHGlobal((int)nSizeOutBuffer);
					}
					if (!messageReceived && !cancelToken.IsCancellationRequested)
					{
						await Task.Delay(500, cancelToken);
					}
				}
			}
			catch (TaskCanceledException)
			{
				ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "DeviceDriverAgent.WaitForNextRequestAsync was canceled");
			}
			Marshal.FreeHGlobal(outBuffer);
			return resultPair;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003F1C File Offset: 0x0000211C
		public async Task<bool> PutResponse(Guid taskId, string response)
		{
			uint bytesReturned = 0U;
			int nSizeInBuffer = 0;
			bool result;
			if (string.IsNullOrEmpty(response))
			{
				ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "DeviceAgent: PutResponseAsync: Task {0}: UMDF said FAILED since response string is null", new object[] { taskId });
				result = false;
			}
			else
			{
				IntPtr inBuffer = this.ConvertResponseTupleToUnmanagedPtr(taskId, response, out nSizeInBuffer);
				int nSizeOutBuffer = Marshal.SizeOf(typeof(bool));
				IntPtr outBuffer = Marshal.AllocHGlobal(nSizeOutBuffer);
				ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "DeviceAgent: PutResponseAsync: Task {0}: Sending {1} bytes to UMDF", new object[] { taskId, nSizeInBuffer });
				bool ioctlResult = false;
				bool isAdded = false;
				using (CancellationTokenSource tknSource = new CancellationTokenSource(60000))
				{
					CancellationToken cancelToken = tknSource.Token;
					int error = 0;
					try
					{
						int retryCounter = 0;
						do
						{
							bool flag = await this.MakeIoControlCall(UmdfControlCodes.PutBrokerResponse, inBuffer, (uint)nSizeInBuffer, outBuffer, (uint)nSizeOutBuffer, ref bytesReturned, IntPtr.Zero, cancelToken, ref error);
							ioctlResult = flag;
							if (ioctlResult)
							{
								break;
							}
							ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, string.Format("DeviceAgent: PutResponseAsyncMakeIoCtl returned error: {0}", error));
							if (error != 1 && error != 997)
							{
								break;
							}
							await Task.Delay(500);
							retryCounter++;
						}
						while ((error == 1 || error == 997) && retryCounter <= 5);
					}
					catch (TaskCanceledException ex)
					{
						ExternalLogger.Instance.Log(ex, string.Format("DeviceAgent: PutResponseAsync: Task {0}. Exception in PutRequestAsync", taskId));
					}
					if (ioctlResult)
					{
						isAdded = true;
					}
					ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, isAdded ? "DeviceAgent: PutResponseAsync: Task {0}. UMDF said OK" : "DeviceAgent: PutResponseAsync: Task {0}: UMDF said FAILED with IOCTL error = {1}", new object[] { taskId, error });
					this._responseCounter += 1U;
					ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, string.Format("DeviceAgent: {0} responses were sent to UMDF driver", this._responseCounter));
					Marshal.FreeHGlobal(inBuffer);
					Marshal.FreeHGlobal(outBuffer);
					cancelToken = default(CancellationToken);
				}
				CancellationTokenSource tknSource = null;
				result = isAdded;
			}
			return result;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003F74 File Offset: 0x00002174
		public async Task<Guid> PutRequestAsync(string brokerRequest)
		{
			ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "DeviceAgent: Entry: ProcessId({0}) ThreadId({1}) Request={2} ", new object[]
			{
				Thread.CurrentThread.ManagedThreadId,
				Process.GetCurrentProcess().Id,
				brokerRequest
			});
			Guid empty = Guid.Empty;
			uint num = 0U;
			IntPtr inBuffer = IntPtr.Zero;
			try
			{
				inBuffer = Marshal.StringToHGlobalUni(brokerRequest);
			}
			catch (Exception ex)
			{
				ExternalLogger.Instance.Log(ex, "DeviceAgent: Exception occured in Marshalling operations");
			}
			uint nInBufferSize = (uint)(brokerRequest.Length * 2);
			int num2 = Marshal.SizeOf(empty.GetType());
			IntPtr outBuffer = Marshal.AllocHGlobal(num2);
			bool ioctlResult = false;
			CancellationTokenSource tknSource = new CancellationTokenSource(30000);
			CancellationToken token = tknSource.Token;
			try
			{
				int num3 = 0;
				bool flag = await this.MakeIoControlCall(UmdfControlCodes.PutBrokerRequest, inBuffer, nInBufferSize, outBuffer, (uint)num2, ref num, IntPtr.Zero, token, ref num3);
				ioctlResult = flag;
			}
			catch (TaskCanceledException ex2)
			{
				ExternalLogger.Instance.Log(ex2, "DeviceAgent: Exception in PutRequestAsync");
			}
			finally
			{
				tknSource.Dispose();
			}
			byte[] array = new byte[Marshal.SizeOf(typeof(Guid))];
			if (ioctlResult)
			{
				Marshal.Copy(outBuffer, array, 0, Marshal.SizeOf(typeof(Guid)));
			}
			empty = new Guid(array);
			Marshal.FreeHGlobal(outBuffer);
			Marshal.FreeHGlobal(inBuffer);
			return empty;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003FC4 File Offset: 0x000021C4
		public async Task<Tuple<Guid, string>> WaitForResponseAsync(Guid taskId)
		{
			Tuple<Guid, string> result;
			using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds((double)this._responseTimeoutSeconds)))
			{
				result = await this.WaitForResponseAsync(taskId, cts.Token);
			}
			return result;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004014 File Offset: 0x00002214
		public async Task<Tuple<Guid, string>> WaitForResponseAsync(Guid taskId, CancellationToken cancellationToken)
		{
			uint bytesReturned = 0U;
			uint nSizeInBuffer = (uint)Marshal.SizeOf(typeof(Guid));
			IntPtr inBuffer = Marshal.AllocHGlobal((int)nSizeInBuffer);
			try
			{
				Marshal.Copy(taskId.ToByteArray(), 0, inBuffer, (int)nSizeInBuffer);
			}
			catch (Exception ex)
			{
				ExternalLogger.Instance.Log(ex, "DeviceAgent: Task: {0}. Exception occured in Marshalling operations", new object[] { taskId });
			}
			Tuple<Guid, string> responsePair = null;
			try
			{
				int error = 0;
				bool flag = await this.MakeIoControlCall(UmdfControlCodes.WaitForResponseV2, inBuffer, nSizeInBuffer, IntPtr.Zero, 0U, ref bytesReturned, IntPtr.Zero, cancellationToken, ref error);
				ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "DeviceDriverAgent WaitForResponseAsync: TaskId {0}. Receiving {1} bytes from UMDF", new object[] { taskId, bytesReturned });
				if (!flag && bytesReturned != 0U)
				{
					uint nSizeOutBuffer = bytesReturned;
					IntPtr outBuffer = Marshal.AllocHGlobal((int)nSizeOutBuffer);
					bool messageReceived = false;
					while (!messageReceived)
					{
						flag = await this.MakeIoControlCall(UmdfControlCodes.WaitForResponseV2, inBuffer, nSizeInBuffer, outBuffer, nSizeOutBuffer, ref bytesReturned, IntPtr.Zero, cancellationToken, ref error);
						if (flag)
						{
							UmdfDriverData umdfDriverData = this.ConvertUnmanagedPtrtoUmdfDriverData(outBuffer, (int)bytesReturned);
							responsePair = Tuple.Create<Guid, string>(umdfDriverData.TaskId, umdfDriverData.XmlString);
							messageReceived = true;
						}
						else if (bytesReturned != 0U)
						{
							nSizeOutBuffer = bytesReturned;
							Marshal.FreeHGlobal(outBuffer);
							outBuffer = Marshal.AllocHGlobal((int)nSizeOutBuffer);
						}
					}
				}
			}
			catch (TaskCanceledException)
			{
				ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "DeviceAgent.WaitResponseAsync Task:{0} was canceled", new object[] { taskId });
			}
			ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "DeviceAgent.WaitForResponseAsync: Task:{0} Received {1} bytes from UMDF", new object[] { taskId, bytesReturned });
			Marshal.FreeHGlobal(inBuffer);
			return responsePair;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000406C File Offset: 0x0000226C
		public async Task<bool> CloseTaskAsync(Guid taskId)
		{
			bool wasClosed = false;
			uint num = 0U;
			uint num2 = (uint)Marshal.SizeOf(typeof(Guid));
			IntPtr inBuffer = Marshal.AllocHGlobal((int)num2);
			try
			{
				Marshal.Copy(taskId.ToByteArray(), 0, inBuffer, (int)num2);
			}
			catch (Exception ex)
			{
				ExternalLogger.Instance.Log(ex, "DeviceAgent.CloseTaskAsync Task:{0}. Exception occured in Marshalling operations", new object[] { taskId });
			}
			int num3 = 0;
			await this.MakeIoControlCall(UmdfControlCodes.CloseTask, inBuffer, num2, IntPtr.Zero, 0U, ref num, IntPtr.Zero, CancellationToken.None, ref num3);
			if (Marshal.ReadInt32(inBuffer) == 1)
			{
				wasClosed = true;
			}
			Marshal.FreeHGlobal(inBuffer);
			return wasClosed;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000040BC File Offset: 0x000022BC
		private Task<bool> MakeIoControlCall(uint IoControlCode, IntPtr InBuffer, uint nInBufferSize, [Out] IntPtr OutBuffer, uint nOutBufferSize, ref uint pBytesReturned, IntPtr Overlapped, CancellationToken cancelToken, ref int error)
		{
			NativeOverlapped nativeOverlapped = default(NativeOverlapped);
			bool flag = false;
			using (ManualResetEvent manualResetEvent = new ManualResetEvent(true))
			{
				nativeOverlapped.EventHandle = manualResetEvent.SafeWaitHandle.DangerousGetHandle();
				manualResetEvent.Reset();
				try
				{
					this.LazyOpenDriver();
				}
				catch (Exception ex)
				{
					ExternalLogger.Instance.Log(ex, "MakeIoControlCall: Exception occured while calling LazyOpenDriver");
					return Task.FromResult<bool>(flag);
				}
				SemaphoreSlim deviceSemaphore = DeviceDriverAgent.DeviceSemaphore;
				if (deviceSemaphore != null)
				{
					deviceSemaphore.Wait();
				}
				bool flag2 = false;
				try
				{
					manualResetEvent.SafeWaitHandle.DangerousAddRef(ref flag2);
					flag = DeviceIo.DeviceIoControl(this._deviceHandle, IoControlCode, InBuffer, nInBufferSize, OutBuffer, nOutBufferSize, out pBytesReturned, ref nativeOverlapped);
				}
				catch (Exception ex2)
				{
					ExternalLogger.Instance.Log(ex2, "DeviceAgent.MakeIoControlCall: Exception in DeviceIoControl");
				}
				SemaphoreSlim deviceSemaphore2 = DeviceDriverAgent.DeviceSemaphore;
				if (deviceSemaphore2 != null)
				{
					deviceSemaphore2.Release();
				}
				if (!flag)
				{
					error = Marshal.GetLastWin32Error();
					if (997 == error)
					{
						int num = -1;
						try
						{
							if (!CancellationToken.None.Equals(cancelToken))
							{
								num = WaitHandle.WaitAny(new WaitHandle[] { manualResetEvent, cancelToken.WaitHandle });
							}
							else
							{
								num = WaitHandle.WaitAny(new WaitHandle[] { manualResetEvent }, 60000);
							}
						}
						catch (Exception ex3)
						{
							ExternalLogger.Instance.Log(ex3, "DeviceAgent.MakeIoControlCall: Exception while waiting for manualResetEvent");
						}
						SemaphoreSlim deviceSemaphore3 = DeviceDriverAgent.DeviceSemaphore;
						if (deviceSemaphore3 != null)
						{
							deviceSemaphore3.Wait();
						}
						try
						{
							if (num != 0)
							{
								if (IoControlCode == UmdfControlCodes.WaitForNextRequest)
								{
									ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "DeviceAgent.MakeIoControlCall: UMDF driver IOCTL call: Cancelling the IOCTL WaitForNextRequest since Timeout occured");
								}
								else if (IoControlCode == UmdfControlCodes.PutBrokerResponse)
								{
									ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "DeviceAgent.MakeIoControlCall: UMDF driver IOCTL call: Cancelling the IOCTL PutResponse since Timeout occured");
								}
								else if (IoControlCode == UmdfControlCodes.CloseTask)
								{
									ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "DeviceAgent.MakeIoControlCall: UMDF driver IOCTL call: Cancelling the IOCTL CloseTask since Timeout occured");
								}
								else
								{
									ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "DeviceAgent.MakeIoControlCall: UMDF driver IOCTL call: Cancelling the IOCTL since Timeout occured for IOCTL: {0}", new object[] { IoControlCode });
								}
								if (cancelToken.IsCancellationRequested && UmdfControlCodes.WaitForNextRequest == IoControlCode)
								{
									if (this._deviceHandle != IntPtr.Zero)
									{
										DeviceDriver.CloseDeviceDriverHandle(this._deviceHandle);
									}
								}
								else if (!DeviceIo.CancelIo(this._deviceHandle))
								{
									ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Warning, "DeviceAgent.MakeIoControlCall: Interop.DeviceIo.CancelIo failed with error code {0}", new object[] { Marshal.GetLastWin32Error() });
								}
							}
							int num2 = 0;
							flag = DeviceIo.GetOverlappedResult(this._deviceHandle, ref nativeOverlapped, out pBytesReturned, false);
							if (!flag)
							{
								num2 = Marshal.GetLastWin32Error();
								ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Warning, "DeviceAgent.MakeIoControlCall: UMDF driver IOCTL call failed with GetLastError code {0}", new object[] { num2 });
							}
							if (num2 == 6)
							{
								ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Warning, "DeviceAgent.MakeIoControlCall: UMDF driver IOCTL returned error ERROR_INVALID_HANDLE. ");
							}
						}
						catch (Exception ex4)
						{
							ExternalLogger.Instance.Log(ex4, "DeviceAgent.MakeIoControlCall: Exception occured while calling GetOverlappedResult");
						}
						SemaphoreSlim deviceSemaphore4 = DeviceDriverAgent.DeviceSemaphore;
						if (deviceSemaphore4 != null)
						{
							deviceSemaphore4.Release();
						}
					}
					else
					{
						ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Warning, "DeviceAgent.MakeIoControlCall: UMDF driver IOCTL call failed with GetLastError code {0}", new object[] { error });
						if (UmdfControlCodes.WaitForNextRequest == IoControlCode)
						{
							Task.Delay(10).Wait();
						}
					}
				}
				if (flag2)
				{
					manualResetEvent.SafeWaitHandle.DangerousRelease();
				}
			}
			TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
			taskCompletionSource.SetResult(flag);
			return taskCompletionSource.Task;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004438 File Offset: 0x00002638
		private UmdfDriverData ConvertUnmanagedPtrtoUmdfDriverData(IntPtr unmanagedPtr, int nSizeOfBuffer)
		{
			UmdfDriverData umdfDriverData = new UmdfDriverData();
			try
			{
				byte[] array = new byte[nSizeOfBuffer];
				Marshal.Copy(unmanagedPtr, array, 0, nSizeOfBuffer);
				byte[] array2 = new byte[Marshal.SizeOf(typeof(Guid))];
				Array.Copy(array, 0, array2, 0, Marshal.SizeOf(typeof(Guid)));
				umdfDriverData.TaskId = new Guid(array2);
				byte[] array3 = new byte[nSizeOfBuffer - Marshal.SizeOf(typeof(Guid))];
				Array.Copy(array, Marshal.SizeOf(typeof(Guid)), array3, 0, nSizeOfBuffer - Marshal.SizeOf(typeof(Guid)));
				umdfDriverData.XmlString = Encoding.Unicode.GetString(array3);
			}
			catch (Exception ex)
			{
				ExternalLogger.Instance.Log(ex, "DeviceAgent: Exception occured in Marshalling operations");
			}
			return umdfDriverData;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000450C File Offset: 0x0000270C
		private IntPtr ConvertResponseTupleToUnmanagedPtr(Guid taskId, string xmlText, out int nSizeOfBuffer)
		{
			nSizeOfBuffer = Marshal.SizeOf(typeof(Guid)) + xmlText.Length * 2;
			IntPtr intPtr = Marshal.AllocHGlobal(nSizeOfBuffer);
			byte[] array = new byte[nSizeOfBuffer];
			Array.Copy(taskId.ToByteArray(), array, Marshal.SizeOf(typeof(Guid)));
			Array.Copy(Encoding.Unicode.GetBytes(xmlText), 0, array, Marshal.SizeOf(typeof(Guid)), nSizeOfBuffer - Marshal.SizeOf(typeof(Guid)));
			try
			{
				Marshal.Copy(array, 0, intPtr, nSizeOfBuffer);
			}
			catch (Exception ex)
			{
				ExternalLogger.Instance.Log(ex, "DeviceAgent: Exception occured in Marshalling operations");
			}
			return intPtr;
		}

		// Token: 0x0400004C RID: 76
		private static readonly int LengthOfGuid = Guid.Empty.ToString().Length;

		// Token: 0x0400004D RID: 77
		private IntPtr _deviceHandle = IntPtr.Zero;

		// Token: 0x0400004E RID: 78
		private static readonly SemaphoreSlim DeviceSemaphore = new SemaphoreSlim(1);

		// Token: 0x0400004F RID: 79
		private readonly int _responseTimeoutSeconds = 70;

		// Token: 0x04000050 RID: 80
		private uint _requestCounter;

		// Token: 0x04000051 RID: 81
		private uint _responseCounter;

		// Token: 0x04000052 RID: 82
		private static DeviceDriverAgent _instance = null;

		// Token: 0x04000053 RID: 83
		private static readonly object SyncRoot = new object();

		// Token: 0x04000054 RID: 84
		private DeviceWatcher _deviceWatcher;

		// Token: 0x04000055 RID: 85
		private static Stopwatch _timer;

		// Token: 0x04000056 RID: 86
		private const int MILLISECONDS_TO_SLEEP_WAITING_FOR_REQUEST = 10;

		// Token: 0x04000057 RID: 87
		private const int ERROR_INVALID_PARAMETER = 1;

		// Token: 0x04000058 RID: 88
		private const int ERROR_IO_PENDING = 997;

		// Token: 0x04000059 RID: 89
		private const int WAITNG_FOR_CLIENT_DELAY_MS = 500;

		// Token: 0x0400005A RID: 90
		private const int WAITNG_FOR_CLIENT_MAX_RETRIES = 5;

		// Token: 0x0400005B RID: 91
		private const int WAITNG_FOR_DRIVER_DELAY_MS = 1000;

		// Token: 0x0400005C RID: 92
		private const int WAITNG_FOR_DRIVER_MAX_RETRIES = 10;
	}
}
