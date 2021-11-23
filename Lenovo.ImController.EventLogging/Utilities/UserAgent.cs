using System;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Lenovo.ImController.EventLogging.Utilities
{
	// Token: 0x02000008 RID: 8
	internal class UserAgent
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002310 File Offset: 0x00000510
		public static string GetLoggedInUserSID()
		{
			string text = string.Empty;
			string activeUserUsername = UserAgent.GetActiveUserUsername();
			if (!string.IsNullOrEmpty(activeUserUsername))
			{
				try
				{
					text = ((SecurityIdentifier)new NTAccount(activeUserUsername).Translate(typeof(SecurityIdentifier))).ToString();
				}
				catch (Exception)
				{
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				try
				{
					using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_Process where Description = 'explorer.exe'"))
					{
						if (managementObjectSearcher != null)
						{
							using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
							{
								if (managementObjectCollection.Count != 0)
								{
									string[] array = new string[1];
									try
									{
										ManagementObject managementObject = managementObjectCollection.Cast<ManagementObject>().First<ManagementObject>();
										string methodName = "GetOwnerSid";
										object[] args = array;
										managementObject.InvokeMethod(methodName, args);
									}
									catch (Exception)
									{
									}
									text = array[0];
								}
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}
			return text;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002408 File Offset: 0x00000608
		private static string GetActiveUserUsername()
		{
			string result = null;
			try
			{
				uint num = 0U;
				result = "SYSTEM";
				uint loggedInUserSessionId = UserAgent.GetLoggedInUserSessionId();
				IntPtr intPtr;
				if (UserAgent.WTSQuerySessionInformationW(IntPtr.Zero, (int)loggedInUserSessionId, UserAgent.WTS_INFO_CLASS.WTSUserName, out intPtr, out num) && num > 1U)
				{
					result = Marshal.PtrToStringUni(intPtr);
					UserAgent.WTSFreeMemory(intPtr);
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002460 File Offset: 0x00000660
		private static uint GetLoggedInUserSessionId()
		{
			uint num = uint.MaxValue;
			try
			{
				IntPtr zero = IntPtr.Zero;
				int num2 = 0;
				if (UserAgent.WTSEnumerateSessionsW(UserAgent.WTS_CURRENT_SERVER_HANDLE, 0, 1, ref zero, ref num2) != 0)
				{
					int offset = Marshal.SizeOf(typeof(UserAgent.WTS_SESSION_INFO));
					IntPtr intPtr = zero;
					for (int i = 0; i < num2; i++)
					{
						UserAgent.WTS_SESSION_INFO wts_SESSION_INFO = (UserAgent.WTS_SESSION_INFO)Marshal.PtrToStructure(intPtr, typeof(UserAgent.WTS_SESSION_INFO));
						intPtr += offset;
						if (wts_SESSION_INFO.State == UserAgent.WTS_CONNECTSTATE_CLASS.WTSActive)
						{
							num = wts_SESSION_INFO.SessionID;
						}
					}
				}
				if (num == 4294967295U)
				{
					num = UserAgent.WTSGetActiveConsoleSessionId();
				}
			}
			catch (Exception)
			{
			}
			return num;
		}

		// Token: 0x0600000B RID: 11
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("wtsapi32.dll")]
		private static extern void WTSFreeMemory(IntPtr pMemory);

		// Token: 0x0600000C RID: 12
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("Wtsapi32.dll")]
		private static extern bool WTSQuerySessionInformationW(IntPtr hServer, int sessionId, UserAgent.WTS_INFO_CLASS wtsInfoClass, out IntPtr ppBuffer, out uint pBytesReturned);

		// Token: 0x0600000D RID: 13
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("wtsapi32.dll", SetLastError = true)]
		private static extern int WTSEnumerateSessionsW(IntPtr hServer, int Reserved, int Version, ref IntPtr ppSessionInfo, ref int pCount);

		// Token: 0x0600000E RID: 14
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll")]
		private static extern uint WTSGetActiveConsoleSessionId();

		// Token: 0x04000005 RID: 5
		private const uint INVALID_SESSION_ID = 4294967295U;

		// Token: 0x04000006 RID: 6
		private static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

		// Token: 0x0200001A RID: 26
		public class User
		{
			// Token: 0x0600006B RID: 107 RVA: 0x00002F47 File Offset: 0x00001147
			public User(string sid, string name)
			{
				this.Sid = sid;
				this.Name = name;
			}

			// Token: 0x1700001E RID: 30
			// (get) Token: 0x0600006C RID: 108 RVA: 0x00002F5D File Offset: 0x0000115D
			public string Name { get; }

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x0600006D RID: 109 RVA: 0x00002F65 File Offset: 0x00001165
			public string Sid { get; }
		}

		// Token: 0x0200001B RID: 27
		private struct WTS_SESSION_INFO
		{
			// Token: 0x04000036 RID: 54
			public readonly uint SessionID;

			// Token: 0x04000037 RID: 55
			[MarshalAs(UnmanagedType.LPStr)]
			public readonly string pWinStationName;

			// Token: 0x04000038 RID: 56
			public readonly UserAgent.WTS_CONNECTSTATE_CLASS State;
		}

		// Token: 0x0200001C RID: 28
		private enum WTS_CONNECTSTATE_CLASS
		{
			// Token: 0x0400003A RID: 58
			WTSActive,
			// Token: 0x0400003B RID: 59
			WTSConnected,
			// Token: 0x0400003C RID: 60
			WTSConnectQuery,
			// Token: 0x0400003D RID: 61
			WTSShadow,
			// Token: 0x0400003E RID: 62
			WTSDisconnected,
			// Token: 0x0400003F RID: 63
			WTSIdle,
			// Token: 0x04000040 RID: 64
			WTSListen,
			// Token: 0x04000041 RID: 65
			WTSReset,
			// Token: 0x04000042 RID: 66
			WTSDown,
			// Token: 0x04000043 RID: 67
			WTSInit
		}

		// Token: 0x0200001D RID: 29
		private enum WTS_INFO_CLASS
		{
			// Token: 0x04000045 RID: 69
			WTSInitialProgram,
			// Token: 0x04000046 RID: 70
			WTSApplicationName,
			// Token: 0x04000047 RID: 71
			WTSWorkingDirectory,
			// Token: 0x04000048 RID: 72
			WTSOEMId,
			// Token: 0x04000049 RID: 73
			WTSSessionId,
			// Token: 0x0400004A RID: 74
			WTSUserName,
			// Token: 0x0400004B RID: 75
			WTSWinStationName,
			// Token: 0x0400004C RID: 76
			WTSDomainName,
			// Token: 0x0400004D RID: 77
			WTSConnectState,
			// Token: 0x0400004E RID: 78
			WTSClientBuildNumber,
			// Token: 0x0400004F RID: 79
			WTSClientName,
			// Token: 0x04000050 RID: 80
			WTSClientDirectory,
			// Token: 0x04000051 RID: 81
			WTSClientProductId,
			// Token: 0x04000052 RID: 82
			WTSClientHardwareId,
			// Token: 0x04000053 RID: 83
			WTSClientAddress,
			// Token: 0x04000054 RID: 84
			WTSClientDisplay,
			// Token: 0x04000055 RID: 85
			WTSClientProtocolType
		}
	}
}
