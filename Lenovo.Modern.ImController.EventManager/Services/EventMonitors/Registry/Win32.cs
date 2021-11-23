using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.Registry
{
	// Token: 0x02000016 RID: 22
	[SuppressUnmanagedCodeSecurity]
	public static class Win32
	{
		// Token: 0x06000064 RID: 100
		[DllImport("Advapi32.dll", SetLastError = true)]
		public static extern int RegNotifyChangeKeyValue(IntPtr hKey, bool watchSubtree, Win32.REG_NOTIFY_CHANGE notifyFilter, IntPtr hEvent, bool asynchronous);

		// Token: 0x06000065 RID: 101
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern int RegOpenKeyEx(IntPtr hKey, string subKey, uint options, int samDesired, out IntPtr phkResult);

		// Token: 0x06000066 RID: 102
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern int RegCreateKeyEx(IntPtr hKey, string lpSubKey, int Reserved, string lpClass, uint dwOptions, int samDesired, IntPtr lpSecurityAttributes, out IntPtr phkResult, out uint lpdwDisposition);

		// Token: 0x06000067 RID: 103
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern int RegCloseKey(IntPtr hKey);

		// Token: 0x04000044 RID: 68
		public const int KEY_QUERY_VALUE = 1;

		// Token: 0x04000045 RID: 69
		public const int KEY_NOTIFY = 16;

		// Token: 0x04000046 RID: 70
		public const int KEY_ALLACCESS = 983103;

		// Token: 0x04000047 RID: 71
		public const int KEY_WOW64_32KEY = 512;

		// Token: 0x04000048 RID: 72
		public const int STANDARD_RIGHTS_READ = 131072;

		// Token: 0x04000049 RID: 73
		public const int REG_OPTION_NON_VOLATILE = 0;

		// Token: 0x0400004A RID: 74
		public const int ERROR_KEY_DELETED = 1018;

		// Token: 0x0400004B RID: 75
		public static readonly IntPtr HKEY_CLASSES_ROOT = new IntPtr(int.MinValue);

		// Token: 0x0400004C RID: 76
		public static readonly IntPtr HKEY_CURRENT_USER = new IntPtr(-2147483647);

		// Token: 0x0400004D RID: 77
		public static readonly IntPtr HKEY_LOCAL_MACHINE = new IntPtr(-2147483646);

		// Token: 0x0400004E RID: 78
		public static readonly IntPtr HKEY_USERS = new IntPtr(-2147483645);

		// Token: 0x0400004F RID: 79
		public static readonly IntPtr HKEY_PERFORMANCE_DATA = new IntPtr(-2147483644);

		// Token: 0x04000050 RID: 80
		public static readonly IntPtr HKEY_CURRENT_CONFIG = new IntPtr(-2147483643);

		// Token: 0x04000051 RID: 81
		public static readonly IntPtr HKEY_DYN_DATA = new IntPtr(-2147483642);

		// Token: 0x02000035 RID: 53
		[Flags]
		public enum REG_NOTIFY_CHANGE : uint
		{
			// Token: 0x040000F1 RID: 241
			NAME = 1U,
			// Token: 0x040000F2 RID: 242
			ATTRIBUTES = 2U,
			// Token: 0x040000F3 RID: 243
			LAST_SET = 4U,
			// Token: 0x040000F4 RID: 244
			SECURITY = 8U
		}
	}
}
