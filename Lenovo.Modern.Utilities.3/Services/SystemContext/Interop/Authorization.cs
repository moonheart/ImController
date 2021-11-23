using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Utilities.Services.SystemContext.Interop
{
	// Token: 0x02000031 RID: 49
	public static class Authorization
	{
		// Token: 0x06000127 RID: 295
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern bool CreateProcessAsUser(IntPtr hToken, string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandle, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref Authorization.STARTUPINFO lpStartupInfo, out Authorization.PROCESS_INFORMATION lpProcessInformation);

		// Token: 0x06000128 RID: 296
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("advapi32.dll")]
		public static extern bool DuplicateTokenEx(IntPtr ExistingTokenHandle, uint dwDesiredAccess, IntPtr lpThreadAttributes, int TokenType, int ImpersonationLevel, ref IntPtr DuplicateTokenHandle);

		// Token: 0x06000129 RID: 297
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool SetTokenInformation(IntPtr TokenHandle, Authorization.TOKEN_INFORMATION_CLASS TokenInformationClass, ref uint TokenInformation, uint TokenInformationLength);

		// Token: 0x0600012A RID: 298
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, out IntPtr TokenHandle);

		// Token: 0x0600012B RID: 299
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenProcess(Authorization.ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

		// Token: 0x0600012C RID: 300
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("userenv.dll", SetLastError = true)]
		public static extern bool CreateEnvironmentBlock(ref IntPtr lpEnvironment, IntPtr hToken, bool bInherit);

		// Token: 0x0600012D RID: 301
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("userenv.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DestroyEnvironmentBlock(IntPtr lpEnvironment);

		// Token: 0x0600012E RID: 302
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hSnapshot);

		// Token: 0x0600012F RID: 303
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

		// Token: 0x06000130 RID: 304
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

		// Token: 0x06000131 RID: 305
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll")]
		public static extern uint WTSGetActiveConsoleSessionId();

		// Token: 0x06000132 RID: 306
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("Wtsapi32.dll", SetLastError = true)]
		public static extern uint WTSQueryUserToken(uint SessionId, ref IntPtr phToken);

		// Token: 0x06000133 RID: 307
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("wtsapi32.dll", SetLastError = true)]
		public static extern int WTSEnumerateSessions(IntPtr hServer, int Reserved, int Version, ref IntPtr ppSessionInfo, ref int pCount);

		// Token: 0x06000134 RID: 308
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("wtsapi32.dll")]
		private static extern IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] string pServerName);

		// Token: 0x06000135 RID: 309
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("wtsapi32.dll")]
		private static extern void WTSCloseServer(IntPtr hServer);

		// Token: 0x06000136 RID: 310
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("wtsapi32.dll")]
		private static extern void WTSFreeMemory(IntPtr pMemory);

		// Token: 0x06000137 RID: 311
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("Wtsapi32.dll")]
		private static extern bool WTSQuerySessionInformation(IntPtr hServer, int sessionId, Authorization.WTS_INFO_CLASS wtsInfoClass, out IntPtr ppBuffer, out uint pBytesReturned);

		// Token: 0x06000138 RID: 312 RVA: 0x00006730 File Offset: 0x00004930
		public static IntPtr OpenServer(string Name)
		{
			return Authorization.WTSOpenServer(Name);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006738 File Offset: 0x00004938
		public static void CloseServer(IntPtr ServerHandle)
		{
			Authorization.WTSCloseServer(ServerHandle);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006740 File Offset: 0x00004940
		public static uint GetLoggedInUserSessionId()
		{
			uint num = uint.MaxValue;
			IntPtr zero = IntPtr.Zero;
			int num2 = 0;
			if (Authorization.WTSEnumerateSessions(Authorization.WTS_CURRENT_SERVER_HANDLE, 0, 1, ref zero, ref num2) != 0)
			{
				int offset = Marshal.SizeOf(typeof(Authorization.WTS_SESSION_INFO));
				IntPtr intPtr = zero;
				for (int i = 0; i < num2; i++)
				{
					Authorization.WTS_SESSION_INFO wts_SESSION_INFO = (Authorization.WTS_SESSION_INFO)Marshal.PtrToStructure(intPtr, typeof(Authorization.WTS_SESSION_INFO));
					intPtr += offset;
					if (wts_SESSION_INFO.State == Authorization.WTS_CONNECTSTATE_CLASS.WTSActive)
					{
						num = wts_SESSION_INFO.SessionID;
					}
				}
			}
			if (num == 4294967295U)
			{
				num = Authorization.WTSGetActiveConsoleSessionId();
			}
			return num;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000067CC File Offset: 0x000049CC
		public static bool GetSessionUserToken(ref IntPtr phUserToken)
		{
			bool result = false;
			IntPtr zero = IntPtr.Zero;
			if (Authorization.WTSQueryUserToken(Authorization.GetLoggedInUserSessionId(), ref zero) != 0U)
			{
				result = Authorization.DuplicateTokenEx(zero, 0U, IntPtr.Zero, 2, 1, ref phUserToken);
				Authorization.CloseHandle(zero);
			}
			else
			{
				Logger.Log(Logger.LogSeverity.Information, "Error in Authorization.WTSQueryUserToken. GetLastError = {0}", new object[] { Marshal.GetLastWin32Error() });
			}
			return result;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00006828 File Offset: 0x00004A28
		public static bool GetUserSessionTokenSystemContext(ref IntPtr phToken)
		{
			IntPtr zero = IntPtr.Zero;
			uint loggedInUserSessionId = Authorization.GetLoggedInUserSessionId();
			IntPtr intPtr = Authorization.OpenProcess(Authorization.ProcessAccessFlags.QueryInformation, false, Process.GetCurrentProcess().Id);
			bool flag = Authorization.OpenProcessToken(intPtr, 983551, out zero);
			if (flag)
			{
				flag = Authorization.DuplicateTokenEx(zero, 0U, IntPtr.Zero, 2, 1, ref phToken);
				if (flag)
				{
					flag = Authorization.SetTokenInformation(phToken, Authorization.TOKEN_INFORMATION_CLASS.TokenSessionId, ref loggedInUserSessionId, (uint)IntPtr.Size);
				}
				Authorization.CloseHandle(zero);
			}
			else
			{
				Logger.Log(Logger.LogSeverity.Information, "Error in Authorization.WTSQueryUserToken. GetLastError = {0}", new object[] { Marshal.GetLastWin32Error() });
			}
			Authorization.CloseHandle(intPtr);
			return flag;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000068C0 File Offset: 0x00004AC0
		public static string GetLoggedInUsernameBySessionId(bool prependDomain = false)
		{
			string text = "SYSTEM";
			uint loggedInUserSessionId = Authorization.GetLoggedInUserSessionId();
			IntPtr intPtr;
			uint num;
			if (Authorization.WTSQuerySessionInformation(IntPtr.Zero, (int)loggedInUserSessionId, Authorization.WTS_INFO_CLASS.WTSUserName, out intPtr, out num) && num > 1U)
			{
				text = Marshal.PtrToStringAnsi(intPtr);
				Authorization.WTSFreeMemory(intPtr);
				if (prependDomain && Authorization.WTSQuerySessionInformation(IntPtr.Zero, (int)loggedInUserSessionId, Authorization.WTS_INFO_CLASS.WTSDomainName, out intPtr, out num) && num > 1U)
				{
					text = Marshal.PtrToStringAnsi(intPtr) + "\\" + text;
					Authorization.WTSFreeMemory(intPtr);
				}
			}
			return text;
		}

		// Token: 0x04000056 RID: 86
		public const int CREATE_UNICODE_ENVIRONMENT = 1024;

		// Token: 0x04000057 RID: 87
		public const int CREATE_NO_WINDOW = 134217728;

		// Token: 0x04000058 RID: 88
		public const int CREATE_NEW_CONSOLE = 16;

		// Token: 0x04000059 RID: 89
		public const uint INVALID_SESSION_ID = 4294967295U;

		// Token: 0x0400005A RID: 90
		public static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

		// Token: 0x0400005B RID: 91
		public const int TOKEN_ALL_ACCESS = 983551;

		// Token: 0x02000080 RID: 128
		public enum WTS_INFO_CLASS
		{
			// Token: 0x040001AC RID: 428
			WTSInitialProgram,
			// Token: 0x040001AD RID: 429
			WTSApplicationName,
			// Token: 0x040001AE RID: 430
			WTSWorkingDirectory,
			// Token: 0x040001AF RID: 431
			WTSOEMId,
			// Token: 0x040001B0 RID: 432
			WTSSessionId,
			// Token: 0x040001B1 RID: 433
			WTSUserName,
			// Token: 0x040001B2 RID: 434
			WTSWinStationName,
			// Token: 0x040001B3 RID: 435
			WTSDomainName,
			// Token: 0x040001B4 RID: 436
			WTSConnectState,
			// Token: 0x040001B5 RID: 437
			WTSClientBuildNumber,
			// Token: 0x040001B6 RID: 438
			WTSClientName,
			// Token: 0x040001B7 RID: 439
			WTSClientDirectory,
			// Token: 0x040001B8 RID: 440
			WTSClientProductId,
			// Token: 0x040001B9 RID: 441
			WTSClientHardwareId,
			// Token: 0x040001BA RID: 442
			WTSClientAddress,
			// Token: 0x040001BB RID: 443
			WTSClientDisplay,
			// Token: 0x040001BC RID: 444
			WTSClientProtocolType
		}

		// Token: 0x02000081 RID: 129
		public enum SW
		{
			// Token: 0x040001BE RID: 446
			SW_HIDE,
			// Token: 0x040001BF RID: 447
			SW_SHOWNORMAL,
			// Token: 0x040001C0 RID: 448
			SW_NORMAL = 1,
			// Token: 0x040001C1 RID: 449
			SW_SHOWMINIMIZED,
			// Token: 0x040001C2 RID: 450
			SW_SHOWMAXIMIZED,
			// Token: 0x040001C3 RID: 451
			SW_MAXIMIZE = 3,
			// Token: 0x040001C4 RID: 452
			SW_SHOWNOACTIVATE,
			// Token: 0x040001C5 RID: 453
			SW_SHOW,
			// Token: 0x040001C6 RID: 454
			SW_MINIMIZE,
			// Token: 0x040001C7 RID: 455
			SW_SHOWMINNOACTIVE,
			// Token: 0x040001C8 RID: 456
			SW_SHOWNA,
			// Token: 0x040001C9 RID: 457
			SW_RESTORE,
			// Token: 0x040001CA RID: 458
			SW_SHOWDEFAULT,
			// Token: 0x040001CB RID: 459
			SW_MAX = 10
		}

		// Token: 0x02000082 RID: 130
		public enum WTS_CONNECTSTATE_CLASS
		{
			// Token: 0x040001CD RID: 461
			WTSActive,
			// Token: 0x040001CE RID: 462
			WTSConnected,
			// Token: 0x040001CF RID: 463
			WTSConnectQuery,
			// Token: 0x040001D0 RID: 464
			WTSShadow,
			// Token: 0x040001D1 RID: 465
			WTSDisconnected,
			// Token: 0x040001D2 RID: 466
			WTSIdle,
			// Token: 0x040001D3 RID: 467
			WTSListen,
			// Token: 0x040001D4 RID: 468
			WTSReset,
			// Token: 0x040001D5 RID: 469
			WTSDown,
			// Token: 0x040001D6 RID: 470
			WTSInit
		}

		// Token: 0x02000083 RID: 131
		public struct PROCESS_INFORMATION
		{
			// Token: 0x040001D7 RID: 471
			public IntPtr hProcess;

			// Token: 0x040001D8 RID: 472
			public IntPtr hThread;

			// Token: 0x040001D9 RID: 473
			public uint dwProcessId;

			// Token: 0x040001DA RID: 474
			public uint dwThreadId;
		}

		// Token: 0x02000084 RID: 132
		public enum SECURITY_IMPERSONATION_LEVEL
		{
			// Token: 0x040001DC RID: 476
			SecurityAnonymous,
			// Token: 0x040001DD RID: 477
			SecurityIdentification,
			// Token: 0x040001DE RID: 478
			SecurityImpersonation,
			// Token: 0x040001DF RID: 479
			SecurityDelegation
		}

		// Token: 0x02000085 RID: 133
		public enum STARTF : uint
		{
			// Token: 0x040001E1 RID: 481
			STARTF_USESHOWWINDOW = 1U,
			// Token: 0x040001E2 RID: 482
			STARTF_USESIZE,
			// Token: 0x040001E3 RID: 483
			STARTF_USEPOSITION = 4U,
			// Token: 0x040001E4 RID: 484
			STARTF_USECOUNTCHARS = 8U,
			// Token: 0x040001E5 RID: 485
			STARTF_USEFILLATTRIBUTE = 16U,
			// Token: 0x040001E6 RID: 486
			STARTF_RUNFULLSCREEN = 32U,
			// Token: 0x040001E7 RID: 487
			STARTF_FORCEONFEEDBACK = 64U,
			// Token: 0x040001E8 RID: 488
			STARTF_FORCEOFFFEEDBACK = 128U,
			// Token: 0x040001E9 RID: 489
			STARTF_USESTDHANDLES = 256U
		}

		// Token: 0x02000086 RID: 134
		public struct STARTUPINFO
		{
			// Token: 0x040001EA RID: 490
			public int cb;

			// Token: 0x040001EB RID: 491
			public string lpReserved;

			// Token: 0x040001EC RID: 492
			public string lpDesktop;

			// Token: 0x040001ED RID: 493
			public string lpTitle;

			// Token: 0x040001EE RID: 494
			public uint dwX;

			// Token: 0x040001EF RID: 495
			public uint dwY;

			// Token: 0x040001F0 RID: 496
			public uint dwXSize;

			// Token: 0x040001F1 RID: 497
			public uint dwYSize;

			// Token: 0x040001F2 RID: 498
			public uint dwXCountChars;

			// Token: 0x040001F3 RID: 499
			public uint dwYCountChars;

			// Token: 0x040001F4 RID: 500
			public uint dwFillAttribute;

			// Token: 0x040001F5 RID: 501
			public uint dwFlags;

			// Token: 0x040001F6 RID: 502
			public short wShowWindow;

			// Token: 0x040001F7 RID: 503
			public short cbReserved2;

			// Token: 0x040001F8 RID: 504
			public IntPtr lpReserved2;

			// Token: 0x040001F9 RID: 505
			public IntPtr hStdInput;

			// Token: 0x040001FA RID: 506
			public IntPtr hStdOutput;

			// Token: 0x040001FB RID: 507
			public IntPtr hStdError;
		}

		// Token: 0x02000087 RID: 135
		public enum TOKEN_TYPE
		{
			// Token: 0x040001FD RID: 509
			TokenPrimary = 1,
			// Token: 0x040001FE RID: 510
			TokenImpersonation
		}

		// Token: 0x02000088 RID: 136
		public enum TOKEN_INFORMATION_CLASS
		{
			// Token: 0x04000200 RID: 512
			TokenSessionId = 12
		}

		// Token: 0x02000089 RID: 137
		[Flags]
		public enum ProcessAccessFlags : uint
		{
			// Token: 0x04000202 RID: 514
			All = 2035711U,
			// Token: 0x04000203 RID: 515
			Terminate = 1U,
			// Token: 0x04000204 RID: 516
			CreateThread = 2U,
			// Token: 0x04000205 RID: 517
			VirtualMemoryOperation = 8U,
			// Token: 0x04000206 RID: 518
			VirtualMemoryRead = 16U,
			// Token: 0x04000207 RID: 519
			VirtualMemoryWrite = 32U,
			// Token: 0x04000208 RID: 520
			DuplicateHandle = 64U,
			// Token: 0x04000209 RID: 521
			CreateProcess = 128U,
			// Token: 0x0400020A RID: 522
			SetQuota = 256U,
			// Token: 0x0400020B RID: 523
			SetInformation = 512U,
			// Token: 0x0400020C RID: 524
			QueryInformation = 1024U,
			// Token: 0x0400020D RID: 525
			QueryLimitedInformation = 4096U,
			// Token: 0x0400020E RID: 526
			Synchronize = 1048576U
		}

		// Token: 0x0200008A RID: 138
		public struct WTS_SESSION_INFO
		{
			// Token: 0x0400020F RID: 527
			public readonly uint SessionID;

			// Token: 0x04000210 RID: 528
			[MarshalAs(UnmanagedType.LPStr)]
			public readonly string pWinStationName;

			// Token: 0x04000211 RID: 529
			public readonly Authorization.WTS_CONNECTSTATE_CLASS State;
		}
	}
}
