using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Lenovo.Modern.ImController
{
	// Token: 0x0200000D RID: 13
	[SuppressUnmanagedCodeSecurity]
	internal class Win32
	{
		// Token: 0x06000018 RID: 24
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern SafeServiceHandle OpenSCManager(string lpMachineName, string lpDatabaseName, int dwDesiredAccess);

		// Token: 0x06000019 RID: 25
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern SafeServiceHandle OpenService(SafeServiceHandle hSCManager, string lpServiceName, int dwDesiredAccess);

		// Token: 0x0600001A RID: 26
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseServiceHandle(IntPtr hSCObject);

		// Token: 0x0600001B RID: 27
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, EntryPoint = "ChangeServiceConfig2", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ChangeServiceFailureActions(SafeServiceHandle hService, int dwInfoLevel, [MarshalAs(UnmanagedType.Struct)] ref SERVICE_FAILURE_ACTIONS lpInfo);

		// Token: 0x0600001C RID: 28
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, EntryPoint = "ChangeServiceConfig2", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FailureActionsOnNonCrashFailures(SafeServiceHandle hService, int dwInfoLevel, [MarshalAs(UnmanagedType.Struct)] ref SERVICE_FAILURE_ACTIONS_FLAG lpInfo);

		// Token: 0x0600001D RID: 29
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AdjustTokenPrivileges(SafeTokenHandle TokenHandle, bool DisableAllPrivileges, [MarshalAs(UnmanagedType.Struct)] ref TOKEN_PRIVILEGES NewState, int BufferLength, IntPtr PreviousState, ref int ReturnLength);

		// Token: 0x0600001E RID: 30
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref long lpLuid);

		// Token: 0x0600001F RID: 31
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, out SafeTokenHandle TokenHandle);

		// Token: 0x06000020 RID: 32
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool CloseHandle(IntPtr handle);

		// Token: 0x06000021 RID: 33
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr RegisterServiceCtrlHandlerEx(string lpServiceName, Win32.ServiceControlHandlerEx cbex, IntPtr context);

		// Token: 0x0400001F RID: 31
		public const int NO_ERROR = 0;

		// Token: 0x04000020 RID: 32
		public const int SERVICE_ALL_ACCESS = 983551;

		// Token: 0x04000021 RID: 33
		public const int SERVICE_QUERY_CONFIG = 1;

		// Token: 0x04000022 RID: 34
		public const int SERVICE_CONFIG_FAILURE_ACTIONS = 2;

		// Token: 0x04000023 RID: 35
		public const int ERROR_ACCESS_DENIED = 5;

		// Token: 0x04000024 RID: 36
		public const int SERVICE_CONFIG_FAILURE_ACTIONS_FLAG = 4;

		// Token: 0x04000025 RID: 37
		public const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

		// Token: 0x04000026 RID: 38
		public const int SE_PRIVILEGE_ENABLED = 2;

		// Token: 0x04000027 RID: 39
		public const int TOKEN_ADJUST_PRIVILEGES = 32;

		// Token: 0x04000028 RID: 40
		public const int TOKEN_QUERY = 8;

		// Token: 0x04000029 RID: 41
		public const int DEVICE_NOTIFY_SERVICE_HANDLE = 1;

		// Token: 0x0400002A RID: 42
		public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;

		// Token: 0x0400002B RID: 43
		public const int DBT_DEVTYP_DEVICEINTERFACE = 5;

		// Token: 0x0400002C RID: 44
		public const int DBT_DEVTYP_HANDLE = 6;

		// Token: 0x0400002D RID: 45
		public const int DBT_DEVICEARRIVAL = 32768;

		// Token: 0x0400002E RID: 46
		public const int DBT_DEVICEQUERYREMOVE = 32769;

		// Token: 0x0400002F RID: 47
		public const int DBT_DEVICEREMOVECOMPLETE = 32772;

		// Token: 0x04000030 RID: 48
		public const int SERVICE_CONTROL_STOP = 1;

		// Token: 0x04000031 RID: 49
		public const int SERVICE_CONTROL_DEVICEEVENT = 11;

		// Token: 0x04000032 RID: 50
		public const int SERVICE_CONTROL_SHUTDOWN = 5;

		// Token: 0x04000033 RID: 51
		public const int SERVICE_CONTROL_POWEREVENT = 13;

		// Token: 0x04000034 RID: 52
		public const int SERVICE_CONTROL_SESSIONCHANGE = 14;

		// Token: 0x04000035 RID: 53
		public const int POWER_EVENT_SUSPEND = 4;

		// Token: 0x04000036 RID: 54
		public const int POWER_EVENT_RESUME = 18;

		// Token: 0x04000037 RID: 55
		public const int POWER_EVENT_POWERSETTINGCHANGE = 32787;

		// Token: 0x0200001C RID: 28
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct WTSSESSION_NOTIFICATION
		{
			// Token: 0x04000069 RID: 105
			public int cbSize;

			// Token: 0x0400006A RID: 106
			public uint dwSessionId;
		}

		// Token: 0x0200001D RID: 29
		// (Invoke) Token: 0x0600006B RID: 107
		public delegate int ServiceControlHandlerEx(int control, int eventType, IntPtr eventData, IntPtr context);
	}
}
