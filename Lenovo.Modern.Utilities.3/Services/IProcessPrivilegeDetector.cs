using System;

namespace Lenovo.Modern.Utilities.Services
{
	// Token: 0x02000003 RID: 3
	public interface IProcessPrivilegeDetector
	{
		// Token: 0x06000005 RID: 5
		ProcessPrivilegeDetector.RunAsPrivilege GetCurrentProcessPrivilege();
	}
}
