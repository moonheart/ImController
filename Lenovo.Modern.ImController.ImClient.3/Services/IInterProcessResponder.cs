using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lenovo.Modern.ImController.ImClient.Services
{
	// Token: 0x02000015 RID: 21
	public interface IInterProcessResponder
	{
		// Token: 0x06000065 RID: 101
		Task<Tuple<Guid, string>> WaitForNextRequestAsync(CancellationToken cancelToken);

		// Token: 0x06000066 RID: 102
		Task<bool> PutResponse(Guid taskId, string response);

		// Token: 0x06000067 RID: 103
		Task<bool> CloseTaskAsync(Guid taskId);

		// Token: 0x06000068 RID: 104
		void CloseDriver();
	}
}
