using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Lenovo.Modern.ImController.ImClient.Interop;
using Lenovo.Modern.ImController.ImClient.Utilities;

namespace Lenovo.Modern.ImController.ImClient.Services.Umdf
{
	// Token: 0x0200001F RID: 31
	internal class DeviceWatcher
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00004609 File Offset: 0x00002809
		public DeviceWatcher(OnDeviceRemovalDelegate eventHandler)
		{
			this._deviceRemovalDelegate = eventHandler;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000463C File Offset: 0x0000283C
		~DeviceWatcher()
		{
			this.StopMessagePump();
			this._messagePumpRunning.Dispose();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004674 File Offset: 0x00002874
		public void StartMessagePump(IntPtr deviceHandle)
		{
			this._deviceHandle = deviceHandle;
			new Thread(new ThreadStart(this.RunMessagePump))
			{
				Name = "ManualMessagePump",
				IsBackground = true
			}.Start();
			this._messagePumpRunning.WaitOne();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000046B4 File Offset: 0x000028B4
		public void StopMessagePump()
		{
			try
			{
				Application.ExitThread();
				IntPtr registeredDeviceHandle = this._registeredDeviceHandle;
				if (this._registeredDeviceHandle != IntPtr.Zero)
				{
					ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "DeviceWatcher.UnregisterDeviceNotification");
					Win32fileOp.UnregisterDeviceNotification(this._registeredDeviceHandle);
					this._registeredDeviceHandle = IntPtr.Zero;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000471C File Offset: 0x0000291C
		private void RunMessagePump()
		{
			this._messageHandlerWindow = new DeviceWatcher.MessageHandlerWnd();
			this._messageHandlerWindow.OnDeviceRemoval += this._deviceRemovalDelegate;
			this.RegisterDeviceRemovalNotificationWnd(this._messageHandlerWindow.Handle);
			this._messagePumpRunning.Set();
			try
			{
				Application.Run();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000477C File Offset: 0x0000297C
		private void RegisterDeviceRemovalNotificationWnd(IntPtr hwnd)
		{
			try
			{
				Win32fileOp.DEV_BROADCAST_HANDLE dev_BROADCAST_HANDLE = default(Win32fileOp.DEV_BROADCAST_HANDLE);
				dev_BROADCAST_HANDLE.dbch_size = Marshal.SizeOf(dev_BROADCAST_HANDLE);
				dev_BROADCAST_HANDLE.dbch_devicetype = 6;
				dev_BROADCAST_HANDLE.dbch_handle = this._deviceHandle;
				IntPtr intPtr = 0;
				intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(dev_BROADCAST_HANDLE));
				Marshal.StructureToPtr(dev_BROADCAST_HANDLE, intPtr, true);
				this._registeredDeviceHandle = Win32fileOp.RegisterDeviceNotification(hwnd, intPtr, 0U);
				ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "DeviceWatcher.RegisterDeviceRemovalNotificationWnd: After registerDeviceNotification regHandle={0}", new object[] { this._registeredDeviceHandle });
				Marshal.FreeHGlobal(intPtr);
			}
			catch (Exception ex)
			{
				ExternalLogger.Instance.Log(ex, "DeviceWatcher.RegisterDeviceRemovalNotificationWnd: Exception");
			}
		}

		// Token: 0x0400005D RID: 93
		private AutoResetEvent _messagePumpRunning = new AutoResetEvent(false);

		// Token: 0x0400005E RID: 94
		private IntPtr _deviceHandle = IntPtr.Zero;

		// Token: 0x0400005F RID: 95
		private IntPtr _registeredDeviceHandle = IntPtr.Zero;

		// Token: 0x04000060 RID: 96
		private OnDeviceRemovalDelegate _deviceRemovalDelegate;

		// Token: 0x04000061 RID: 97
		private DeviceWatcher.MessageHandlerWnd _messageHandlerWindow;

		// Token: 0x02000067 RID: 103
		internal class MessageHandlerWnd : NativeWindow
		{
			// Token: 0x14000001 RID: 1
			// (add) Token: 0x06000184 RID: 388 RVA: 0x0000717C File Offset: 0x0000537C
			// (remove) Token: 0x06000185 RID: 389 RVA: 0x000071B4 File Offset: 0x000053B4
			public event OnDeviceRemovalDelegate OnDeviceRemoval;

			// Token: 0x06000186 RID: 390 RVA: 0x000071E9 File Offset: 0x000053E9
			public MessageHandlerWnd()
			{
				this.CreateHandle(new CreateParams());
			}

			// Token: 0x06000187 RID: 391 RVA: 0x000071FC File Offset: 0x000053FC
			~MessageHandlerWnd()
			{
				ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "DeviceWatcher.DestroyHandle(): Entry");
				try
				{
					this.DestroyHandle();
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x06000188 RID: 392 RVA: 0x00007248 File Offset: 0x00005448
			protected override void WndProc(ref Message msg)
			{
				if (msg.Msg == 537)
				{
					IntPtr lparam = msg.LParam;
					if (msg.LParam != IntPtr.Zero)
					{
						try
						{
							if (((Win32fileOp.DEV_BROADCAST_HDR)Marshal.PtrToStructure(msg.LParam, typeof(Win32fileOp.DEV_BROADCAST_HDR))).dbch_DeviceType == 6U && ((int)msg.WParam == 32769 || (int)msg.WParam == 32771))
							{
								ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "DeviceWatcher DBT: Query Device Removal");
								this.OnDeviceRemoval();
							}
						}
						catch (Exception)
						{
						}
					}
				}
				base.WndProc(ref msg);
			}
		}
	}
}
