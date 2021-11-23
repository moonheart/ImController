using System;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model.Subscription;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x0200000E RID: 14
	public interface IPluginManager : IDataCleanup
	{
		// Token: 0x06000038 RID: 56
		void StopPluginHosts(bool systemSuspending);

		// Token: 0x06000039 RID: 57
		void SetPackageSubscription(PackageSubscription ps);

		// Token: 0x0600003A RID: 58
		Task<BrokerResponseTask> MakePluginRequest(PluginRequestInformation pluginInfo, CancellationToken cancelToken);

		// Token: 0x0600003B RID: 59
		void Stop(bool systemSuspending, bool systemShuttingdown);
	}
}
