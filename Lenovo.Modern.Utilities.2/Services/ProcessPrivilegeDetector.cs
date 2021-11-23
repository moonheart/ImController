using System;
using System.Security.Principal;

namespace Lenovo.Modern.Utilities.Services
{
	// Token: 0x02000002 RID: 2
	public class ProcessPrivilegeDetector : IProcessPrivilegeDetector
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public ProcessPrivilegeDetector.RunAsPrivilege GetCurrentProcessPrivilege()
		{
			ProcessPrivilegeDetector.RunAsPrivilege result;
			if (ProcessPrivilegeDetector.IsSystem())
			{
				result = ProcessPrivilegeDetector.RunAsPrivilege.System;
			}
			else if (ProcessPrivilegeDetector.IsAdministrator())
			{
				result = ProcessPrivilegeDetector.RunAsPrivilege.Admin;
			}
			else
			{
				result = ProcessPrivilegeDetector.RunAsPrivilege.User;
			}
			return result;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002078 File Offset: 0x00000278
		private static bool IsAdministrator()
		{
			bool result = false;
			using (WindowsIdentity current = WindowsIdentity.GetCurrent())
			{
				result = new WindowsPrincipal(current).IsInRole(WindowsBuiltInRole.Administrator);
			}
			return result;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020BC File Offset: 0x000002BC
		private static bool IsSystem()
		{
			bool result;
			using (WindowsIdentity current = WindowsIdentity.GetCurrent())
			{
				result = new WindowsPrincipal(current).IsInRole("SYSTEM");
			}
			return result;
		}

		// Token: 0x02000041 RID: 65
		public enum RunAsPrivilege
		{
			// Token: 0x0400007F RID: 127
			Unknown,
			// Token: 0x04000080 RID: 128
			User,
			// Token: 0x04000081 RID: 129
			Admin,
			// Token: 0x04000082 RID: 130
			System
		}
	}
}
