using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lenovo.Modern.ImController.ImClient.Services
{
	// Token: 0x02000014 RID: 20
	internal interface IInterProcessRequester
	{
		// Token: 0x0600005F RID: 95
		Task<Guid> PutRequestAsync(string brokerRequest);

		// Token: 0x06000060 RID: 96
		Task<Tuple<Guid, string>> WaitForResponseAsync(Guid taskId, CancellationToken cancellationToken);

		// Token: 0x06000061 RID: 97
		Task<Tuple<Guid, string>> WaitForResponseAsync(Guid taskId);

		// Token: 0x06000062 RID: 98
		Task<bool> CloseTaskAsync(Guid taskId);

		// Token: 0x06000063 RID: 99
		Task<bool> PutResponse(Guid taskId, string response);

		// Token: 0x06000064 RID: 100
		void CloseDriver();
	}
}
