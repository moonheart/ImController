using System;
using System.Collections.Generic;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.UpdateManager.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Validation;
using Lenovo.Modern.Utilities.Services.Wrappers.Network;

namespace Lenovo.Modern.ImController.UpdateManager
{
	// Token: 0x02000003 RID: 3
	public static class UpdateManagerFactory
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002118 File Offset: 0x00000318
		public static IUpdateManager GetUpdateManagerInstance()
		{
			try
			{
				if (UpdateManagerFactory._updateManagerInstance == null)
				{
					IPackageHistory packageHistory = new PackageHistory();
					IImcCertificateValidator certValidator = new ImcCertificateValidator();
					IInstallManager installManager = new InstallManager(packageHistory, certValidator);
					INetworkAgent networkAgent = new NetworkAgent();
					ISubscriptionManager instance = SubscriptionManager.GetInstance(networkAgent);
					UpdateManagerFactory._updateManagerInstance = new UpdateManager(new List<IUpdater>
					{
						new SelfUpdater(networkAgent, packageHistory, instance, installManager),
						new PackageUpdater(networkAgent, packageHistory, instance, installManager)
					});
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to initialize Update manager instance");
			}
			return UpdateManagerFactory._updateManagerInstance;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000021A0 File Offset: 0x000003A0
		public static void DisposeUpdateManager()
		{
			UpdateManagerFactory._updateManagerInstance = null;
		}

		// Token: 0x04000014 RID: 20
		private static IUpdateManager _updateManagerInstance;
	}
}
