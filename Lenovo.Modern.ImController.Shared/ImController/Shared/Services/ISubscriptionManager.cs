using System;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.Shared.Model.Subscription;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x02000011 RID: 17
	public interface ISubscriptionManager : IDataCleanup
	{
		// Token: 0x06000040 RID: 64
		Task<PackageSubscription> GetSubscriptionAsync(CancellationToken ct);

		// Token: 0x06000041 RID: 65
		Task<PackageSubscription> GetLatestSubscriptionAsync(CancellationToken ct);

		// Token: 0x06000042 RID: 66
		Task<bool> UpdateSubscriptionFile(string newSubscriptionFilePath);
	}
}
