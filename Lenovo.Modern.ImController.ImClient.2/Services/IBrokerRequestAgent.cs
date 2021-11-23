using System;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.ImClient.Models;

namespace Lenovo.Modern.ImController.ImClient.Services
{
	// Token: 0x02000012 RID: 18
	public interface IBrokerRequestAgent
	{
		// Token: 0x06000059 RID: 89
		Task<ImClientRequestTask> MakeRequestAsync(ContractRequest request, Func<BrokerResponseTask, bool> responseReceived, CancellationToken cancelToken);

		// Token: 0x0600005A RID: 90
		Task<bool> CancelRequestAsync(Guid taskId);
	}
}
