using System;
using System.Runtime.InteropServices;

namespace Lenovo.Modern.Utilities.Services.SystemContext.Interop
{
	// Token: 0x02000033 RID: 51
	internal static class WindowsTerminalServer
	{
		// Token: 0x06000145 RID: 325
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll")]
		internal static extern uint WTSGetActiveConsoleSessionId();

		// Token: 0x06000146 RID: 326
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("wtsapi32.dll", SetLastError = true)]
		internal static extern bool WTSQueryUserToken(uint sessionId, out IntPtr Token);

		// Token: 0x06000147 RID: 327
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("wtsapi32.dll", SetLastError = true)]
		internal static extern IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] string pServerName);

		// Token: 0x06000148 RID: 328
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("wtsapi32.dll")]
		internal static extern void WTSCloseServer(IntPtr hServer);

		// Token: 0x06000149 RID: 329
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("wtsapi32.dll", SetLastError = true)]
		internal static extern int WTSEnumerateSessions(IntPtr hServer, [MarshalAs(UnmanagedType.U4)] int Reserved, [MarshalAs(UnmanagedType.U4)] int Version, ref IntPtr ppSessionInfo, [MarshalAs(UnmanagedType.U4)] ref int pCount);

		// Token: 0x0600014A RID: 330
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("wtsapi32.dll")]
		internal static extern void WTSFreeMemory(IntPtr pMemory);

		// Token: 0x0200008E RID: 142
		public enum WTS_CONNECTSTATE_CLASS
		{
			// Token: 0x04000226 RID: 550
			WTSActive,
			// Token: 0x04000227 RID: 551
			WTSConnected,
			// Token: 0x04000228 RID: 552
			WTSConnectQuery,
			// Token: 0x04000229 RID: 553
			WTSShadow,
			// Token: 0x0400022A RID: 554
			WTSDisconnected,
			// Token: 0x0400022B RID: 555
			WTSIdle,
			// Token: 0x0400022C RID: 556
			WTSListen,
			// Token: 0x0400022D RID: 557
			WTSReset,
			// Token: 0x0400022E RID: 558
			WTSDown,
			// Token: 0x0400022F RID: 559
			WTSInit
		}

		// Token: 0x0200008F RID: 143
		public struct WTS_SESSION_INFO
		{
			// Token: 0x04000230 RID: 560
			public int SessionID;

			// Token: 0x04000231 RID: 561
			[MarshalAs(UnmanagedType.LPStr)]
			public string pWinStationName;

			// Token: 0x04000232 RID: 562
			public WindowsTerminalServer.WTS_CONNECTSTATE_CLASS State;
		}
	}
}
