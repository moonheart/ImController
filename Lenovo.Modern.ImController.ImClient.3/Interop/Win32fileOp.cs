using System;
using System.Runtime.InteropServices;

namespace Lenovo.Modern.ImController.ImClient.Interop
{
	// Token: 0x0200000F RID: 15
	public class Win32fileOp
	{
		// Token: 0x06000048 RID: 72
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPTStr)] string filename, uint access, uint share, IntPtr securityAttributes, uint creationDisposition, uint flagsAndAttributes, IntPtr templateFile);

		// Token: 0x06000049 RID: 73
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr hObject);

		// Token: 0x0600004A RID: 74
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr RegisterDeviceNotification(IntPtr IntPtr, IntPtr NotificationFilter, uint Flags);

		// Token: 0x0600004B RID: 75
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool UnregisterDeviceNotification(IntPtr IntPtr);

		// Token: 0x04000025 RID: 37
		public const long INVALID_HANDLE_VALUE = -1L;

		// Token: 0x04000026 RID: 38
		public const int ERROR_INSUFFICIENT_BUFFER = 122;

		// Token: 0x04000027 RID: 39
		public const int ERROR_INVALID_HANDLE = 6;

		// Token: 0x04000028 RID: 40
		public const uint GENERIC_READ = 2147483648U;

		// Token: 0x04000029 RID: 41
		public const uint GENERIC_WRITE = 1073741824U;

		// Token: 0x0400002A RID: 42
		public const uint FILE_SHARE_READ = 1U;

		// Token: 0x0400002B RID: 43
		public const uint FILE_SHARE_WRITE = 2U;

		// Token: 0x0400002C RID: 44
		public const uint OPEN_EXISTING = 3U;

		// Token: 0x0400002D RID: 45
		public const uint FILE_ATTRIBUTE_NORMAL = 128U;

		// Token: 0x0400002E RID: 46
		public const uint FILE_FLAG_OVERLAPPED = 1073741824U;

		// Token: 0x0400002F RID: 47
		public const int DEVICE_NOTIFY_SERVICE_HANDLE = 1;

		// Token: 0x04000030 RID: 48
		public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;

		// Token: 0x04000031 RID: 49
		public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0;

		// Token: 0x04000032 RID: 50
		public const int DBT_DEVTYP_DEVICEINTERFACE = 5;

		// Token: 0x04000033 RID: 51
		public const int DBT_DEVTYP_HANDLE = 6;

		// Token: 0x04000034 RID: 52
		public const int DBT_DEVICEARRIVAL = 32768;

		// Token: 0x04000035 RID: 53
		public const int DBT_DEVICEQUERYREMOVE = 32769;

		// Token: 0x04000036 RID: 54
		public const int DBT_DEVICEREMOVEPENDING = 32771;

		// Token: 0x04000037 RID: 55
		public const int DBT_DEVICEREMOVECOMPLETE = 32772;

		// Token: 0x04000038 RID: 56
		public const int WM_DEVICECHANGE = 537;

		// Token: 0x02000055 RID: 85
		public struct DEV_BROADCAST_HANDLE
		{
			// Token: 0x0400010D RID: 269
			public int dbch_size;

			// Token: 0x0400010E RID: 270
			public int dbch_devicetype;

			// Token: 0x0400010F RID: 271
			public int dbch_reserved;

			// Token: 0x04000110 RID: 272
			public IntPtr dbch_handle;

			// Token: 0x04000111 RID: 273
			public IntPtr dbch_hdevnotify;

			// Token: 0x04000112 RID: 274
			public Guid dbch_eventguid;

			// Token: 0x04000113 RID: 275
			public int dbch_nameoffset;

			// Token: 0x04000114 RID: 276
			public byte dbch_data;
		}

		// Token: 0x02000056 RID: 86
		public struct DEV_BROADCAST_HDR
		{
			// Token: 0x04000115 RID: 277
			public uint dbch_Size;

			// Token: 0x04000116 RID: 278
			public uint dbch_DeviceType;

			// Token: 0x04000117 RID: 279
			public uint dbch_Reserved;
		}
	}
}
