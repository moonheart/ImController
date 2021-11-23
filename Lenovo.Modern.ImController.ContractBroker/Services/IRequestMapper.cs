using System;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model.Packages;

namespace Lenovo.Modern.ImController.ContractBroker.Services
{
	// Token: 0x02000010 RID: 16
	public interface IRequestMapper
	{
		// Token: 0x0600009A RID: 154
		Task<bool> InitializeAsync();

		// Token: 0x0600009B RID: 155
		Task<Tuple<PackageInformation, ContractMapping>> GetPluginMappingAsync(ContractRequest contractRequest, CancellationToken cancelToken);
	}
}
