using System;
using System.Threading;

namespace Lenovo.Modern.ImController.UpdateManager.Services
{
	// Token: 0x02000007 RID: 7
	public interface IUpdater
	{
		// Token: 0x06000010 RID: 16
		void Start(CancellationToken cancelToken);

		// Token: 0x06000011 RID: 17
		void StopAndWait();

		// Token: 0x06000012 RID: 18
		void ApplyPendingUpdates();
	}
}
