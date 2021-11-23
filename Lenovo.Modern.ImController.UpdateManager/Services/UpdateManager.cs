using System;
using System.Collections.Generic;
using System.Threading;

namespace Lenovo.Modern.ImController.UpdateManager.Services
{
	// Token: 0x02000009 RID: 9
	public class UpdateManager : IUpdateManager
	{
		// Token: 0x06000020 RID: 32 RVA: 0x00002D59 File Offset: 0x00000F59
		public UpdateManager(IList<IUpdater> updaters)
		{
			this._updaterList = updaters;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002D68 File Offset: 0x00000F68
		public void Start(CancellationToken cancelToken)
		{
			foreach (IUpdater updater in this._updaterList)
			{
				updater.Start(cancelToken);
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002DB4 File Offset: 0x00000FB4
		public void StopAndWait()
		{
			foreach (IUpdater updater in this._updaterList)
			{
				updater.StopAndWait();
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002E00 File Offset: 0x00001000
		public void ApplyPendingUpdates()
		{
			foreach (IUpdater updater in this._updaterList)
			{
				updater.ApplyPendingUpdates();
			}
		}

		// Token: 0x0400001D RID: 29
		private readonly IList<IUpdater> _updaterList;
	}
}
