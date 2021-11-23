using System;
using Lenovo.Modern.ImController.Shared.Model;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x0200000D RID: 13
	public interface IPackageHistory
	{
		// Token: 0x06000035 RID: 53
		bool CachePackageInformation(CacheInformation packageInfo, bool updateCurrentInstalledVersion = true);

		// Token: 0x06000036 RID: 54
		CacheInformation GetPackageInformationFromCache(string name, string version);

		// Token: 0x06000037 RID: 55
		Version GetCurrentInstalledVersionOfPackage(string name);
	}
}
