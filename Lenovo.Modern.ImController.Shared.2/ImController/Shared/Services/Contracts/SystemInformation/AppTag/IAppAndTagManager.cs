using System;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;

namespace Lenovo.Modern.ImController.Shared.Services.Contracts.SystemInformation.AppTag
{
	// Token: 0x02000023 RID: 35
	public interface IAppAndTagManager : IDataCleanup
	{
		// Token: 0x060000CD RID: 205
		Task<AppAndTagCollection> GetAppAndTagsAsync(CancellationToken cancellationToken);

		// Token: 0x060000CE RID: 206
		void UpdateCacheWithDelay(CancellationToken cancellationToken);
	}
}
