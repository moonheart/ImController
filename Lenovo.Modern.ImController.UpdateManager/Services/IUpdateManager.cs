using System;
using System.Threading;

namespace Lenovo.Modern.ImController.UpdateManager.Services
{
	// Token: 0x02000006 RID: 6
	public interface IUpdateManager
	{
		// Token: 0x0600000D RID: 13
		void Start(CancellationToken cancelToken);

		// Token: 0x0600000E RID: 14
		void StopAndWait();

		// Token: 0x0600000F RID: 15
		void ApplyPendingUpdates();
	}
}
