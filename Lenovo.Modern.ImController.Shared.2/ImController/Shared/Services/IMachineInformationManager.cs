using System;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x0200000A RID: 10
	public interface IMachineInformationManager : IDataCleanup
	{
		// Token: 0x06000028 RID: 40
		Task<MachineInformation> GetMachineInformationAsync(CancellationToken cancelToken);

		// Token: 0x06000029 RID: 41
		void UpdateCacheWithDelay(CancellationToken cancelToken);
	}
}
