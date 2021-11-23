using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Lenovo.Modern.Utilities.Services.SystemContext.Interop;
using Lenovo.Modern.Utilities.Services.Wrappers.Process;

namespace Lenovo.Modern.Utilities.Services.SystemContext.ProcessLauncher
{
	// Token: 0x02000030 RID: 48
	public class SystemProcessLauncher : IProcessLauncher
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00002100 File Offset: 0x00000300
		private SystemProcessLauncher()
		{
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000124 RID: 292 RVA: 0x0000641F File Offset: 0x0000461F
		public static IProcessLauncher Instance
		{
			get
			{
				if (SystemProcessLauncher.instance == null)
				{
					SystemProcessLauncher.instance = new SystemProcessLauncher();
				}
				return SystemProcessLauncher.instance;
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006438 File Offset: 0x00004638
		int? IProcessLauncher.LaunchUserProcess(string appPath, string cmdLine, string workDir, bool visible)
		{
			int? result = null;
			IntPtr zero = IntPtr.Zero;
			Authorization.STARTUPINFO startupinfo = default(Authorization.STARTUPINFO);
			Authorization.PROCESS_INFORMATION process_INFORMATION = default(Authorization.PROCESS_INFORMATION);
			IntPtr zero2 = IntPtr.Zero;
			startupinfo.cb = Marshal.SizeOf(typeof(Authorization.STARTUPINFO));
			try
			{
				if (!Authorization.GetSessionUserToken(ref zero))
				{
					bool isSystem;
					using (WindowsIdentity current = WindowsIdentity.GetCurrent())
					{
						isSystem = current.IsSystem;
					}
					if (isSystem)
					{
						throw new Exception("LaunchUserProcess: GetSessionUserToken failed.");
					}
				}
				uint dwCreationFlags = 1024U | (visible ? 16U : 134217728U);
				startupinfo.wShowWindow = (visible ? 5 : 0);
				startupinfo.lpDesktop = "winsta0\\default";
				if (!Authorization.CreateEnvironmentBlock(ref zero2, zero, false))
				{
					throw new Exception("LaunchUserProcess: CreateEnvironmentBlock failed.");
				}
				if (!Authorization.CreateProcessAsUser(zero, appPath, cmdLine, IntPtr.Zero, IntPtr.Zero, false, dwCreationFlags, zero2, workDir, ref startupinfo, out process_INFORMATION))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					throw new InvalidOperationException(string.Format("Unable to launch user process.  Path: {0}, args: {1}, win32Error: {2}", appPath, cmdLine, lastWin32Error));
				}
				result = new int?(Convert.ToInt32(process_INFORMATION.dwProcessId));
			}
			finally
			{
				Authorization.CloseHandle(zero);
				if (zero2 != IntPtr.Zero)
				{
					Authorization.DestroyEnvironmentBlock(zero2);
				}
				Authorization.CloseHandle(process_INFORMATION.hThread);
				Authorization.CloseHandle(process_INFORMATION.hProcess);
			}
			return result;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000065A4 File Offset: 0x000047A4
		int? IProcessLauncher.LaunchSystemProcessInUserSession(string appPath, string cmdLine, string workDir, bool visible)
		{
			int? result = null;
			IntPtr zero = IntPtr.Zero;
			Authorization.STARTUPINFO startupinfo = default(Authorization.STARTUPINFO);
			startupinfo.dwFlags = 1U;
			Authorization.PROCESS_INFORMATION process_INFORMATION = default(Authorization.PROCESS_INFORMATION);
			IntPtr zero2 = IntPtr.Zero;
			startupinfo.cb = Marshal.SizeOf(typeof(Authorization.STARTUPINFO));
			try
			{
				if (!Authorization.GetUserSessionTokenSystemContext(ref zero))
				{
					throw new Exception("LaunchSystemProcessInUserSession: GetSessionUserToken failed.");
				}
				uint dwCreationFlags = 1024U | (visible ? 16U : 134217728U);
				startupinfo.wShowWindow = (visible ? 5 : 0);
				startupinfo.lpDesktop = "winsta0\\default";
				if (!Authorization.CreateEnvironmentBlock(ref zero2, zero, false))
				{
					throw new Exception("LaunchSystemProcessInUserSession: CreateEnvironmentBlock failed.");
				}
				IntPtr ptr = 0;
				bool flag = false;
				if (Environment.Is64BitOperatingSystem)
				{
					flag = Authorization.Wow64DisableWow64FsRedirection(ref ptr);
				}
				if (!Authorization.CreateProcessAsUser(zero, string.IsNullOrEmpty(cmdLine) ? appPath : null, string.IsNullOrEmpty(cmdLine) ? null : (appPath + " " + cmdLine), IntPtr.Zero, IntPtr.Zero, false, dwCreationFlags, zero2, workDir, ref startupinfo, out process_INFORMATION))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					throw new InvalidOperationException(string.Format("Unable to launch system context process.  Path: {0}, args: {1}, win32Error: {2}", appPath, cmdLine, lastWin32Error));
				}
				result = new int?(Convert.ToInt32(process_INFORMATION.dwProcessId));
				if (flag)
				{
					Authorization.Wow64RevertWow64FsRedirection(ptr);
				}
			}
			finally
			{
				Authorization.CloseHandle(zero);
				if (zero2 != IntPtr.Zero)
				{
					Authorization.DestroyEnvironmentBlock(zero2);
				}
				Authorization.CloseHandle(process_INFORMATION.hThread);
				Authorization.CloseHandle(process_INFORMATION.hProcess);
			}
			return result;
		}

		// Token: 0x04000055 RID: 85
		private static IProcessLauncher instance;
	}
}
