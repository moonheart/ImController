using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Lenovo.Modern.ImController
{
	// Token: 0x0200000E RID: 14
	internal class ServiceRecoveryProperty
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002B18 File Offset: 0x00000D18
		public static void ChangeRecoveryProperty(string scName, List<SC_ACTION> scActions, int resetPeriod, string command, bool fFailureActionsOnNonCrashFailures, string rebootMsg)
		{
			SafeServiceHandle safeServiceHandle = null;
			SafeServiceHandle safeServiceHandle2 = null;
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				safeServiceHandle = Win32.OpenSCManager(null, null, 1);
				if (safeServiceHandle.IsInvalid)
				{
					throw new Win32Exception();
				}
				safeServiceHandle2 = Win32.OpenService(safeServiceHandle, scName, 983551);
				if (safeServiceHandle2.IsInvalid)
				{
					throw new Win32Exception();
				}
				int count = scActions.Count;
				int[] array = new int[count * 2];
				bool flag = false;
				int num = 0;
				foreach (SC_ACTION sc_ACTION in scActions)
				{
					array[num] = sc_ACTION.Type;
					array[++num] = sc_ACTION.Delay;
					num++;
					if (sc_ACTION.Type == 2)
					{
						flag = true;
					}
				}
				if (flag)
				{
					ServiceRecoveryProperty.GrantShutdownPrivilege();
				}
				intPtr = Marshal.AllocHGlobal(array.Length * Marshal.SizeOf(typeof(int)));
				Marshal.Copy(array, 0, intPtr, array.Length);
				SERVICE_FAILURE_ACTIONS service_FAILURE_ACTIONS = default(SERVICE_FAILURE_ACTIONS);
				service_FAILURE_ACTIONS.cActions = count;
				service_FAILURE_ACTIONS.dwResetPeriod = resetPeriod;
				service_FAILURE_ACTIONS.lpCommand = command;
				service_FAILURE_ACTIONS.lpRebootMsg = rebootMsg;
				service_FAILURE_ACTIONS.lpsaActions = intPtr;
				if (!Win32.ChangeServiceFailureActions(safeServiceHandle2, 2, ref service_FAILURE_ACTIONS))
				{
					throw new Win32Exception();
				}
				SERVICE_FAILURE_ACTIONS_FLAG service_FAILURE_ACTIONS_FLAG = default(SERVICE_FAILURE_ACTIONS_FLAG);
				service_FAILURE_ACTIONS_FLAG.fFailureActionsOnNonCrashFailures = fFailureActionsOnNonCrashFailures;
				if (!Win32.FailureActionsOnNonCrashFailures(safeServiceHandle2, 4, ref service_FAILURE_ACTIONS_FLAG))
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				if (safeServiceHandle != null && !safeServiceHandle.IsInvalid)
				{
					safeServiceHandle.Dispose();
				}
				if (safeServiceHandle2 != null && !safeServiceHandle2.IsInvalid)
				{
					safeServiceHandle2.Dispose();
				}
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002CD4 File Offset: 0x00000ED4
		private static void GrantShutdownPrivilege()
		{
			SafeTokenHandle safeTokenHandle = null;
			try
			{
				if (!Win32.OpenProcessToken(Process.GetCurrentProcess().Handle, 40, out safeTokenHandle))
				{
					throw new Win32Exception();
				}
				long luid = 0L;
				if (!Win32.LookupPrivilegeValue(null, "SeShutdownPrivilege", ref luid))
				{
					throw new Win32Exception();
				}
				TOKEN_PRIVILEGES token_PRIVILEGES = default(TOKEN_PRIVILEGES);
				token_PRIVILEGES.PrivilegeCount = 1;
				token_PRIVILEGES.Privileges.Luid = luid;
				token_PRIVILEGES.Privileges.Attributes = 2;
				int num = 0;
				if (!Win32.AdjustTokenPrivileges(safeTokenHandle, false, ref token_PRIVILEGES, 0, IntPtr.Zero, ref num))
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				if (safeTokenHandle != null && !safeTokenHandle.IsInvalid)
				{
					safeTokenHandle.Dispose();
					safeTokenHandle = null;
				}
			}
		}
	}
}
