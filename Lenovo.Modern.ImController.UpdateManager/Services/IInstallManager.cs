using System;

namespace Lenovo.Modern.ImController.UpdateManager.Services
{
	// Token: 0x02000004 RID: 4
	public interface IInstallManager
	{
		// Token: 0x06000004 RID: 4
		int GetNumberOfInstallAttempts(string packageCacheName, string versionNumber);

		// Token: 0x06000005 RID: 5
		bool IsInstallerValid(string installerFilePath);

		// Token: 0x06000006 RID: 6
		void RunMsInstaller(string cabFilePath);

		// Token: 0x06000007 RID: 7
		void RunInstaller(string installerFilePath, bool fistAttempt);
	}
}
