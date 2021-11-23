using System;
using System.Security.Principal;

namespace Lenovo.Modern.ImController.ImClient.Utilities
{
	// Token: 0x02000004 RID: 4
	internal class ProcessPrivilegeDetector
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002294 File Offset: 0x00000494
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

		// Token: 0x06000012 RID: 18 RVA: 0x000022BC File Offset: 0x000004BC
		private static bool IsAdministrator()
		{
			bool result = false;
			using (WindowsIdentity current = WindowsIdentity.GetCurrent())
			{
				result = new WindowsPrincipal(current).IsInRole(WindowsBuiltInRole.Administrator);
			}
			return result;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002300 File Offset: 0x00000500
		private static bool IsSystem()
		{
			bool result;
			using (WindowsIdentity current = WindowsIdentity.GetCurrent())
			{
				result = new WindowsPrincipal(current).IsInRole("SYSTEM");
			}
			return result;
		}

		// Token: 0x02000039 RID: 57
		public enum RunAsPrivilege
		{
			// Token: 0x040000BB RID: 187
			Unknown,
			// Token: 0x040000BC RID: 188
			User,
			// Token: 0x040000BD RID: 189
			Admin,
			// Token: 0x040000BE RID: 190
			System
		}
	}
}
