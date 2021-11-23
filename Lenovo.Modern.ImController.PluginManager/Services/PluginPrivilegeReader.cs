using System;
using System.Collections.Generic;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model.Subscription;

namespace Lenovo.Modern.ImController.PluginManager.Services
{
	// Token: 0x02000008 RID: 8
	public class PluginPrivilegeReader : IPluginPrivilegeReader
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00003A7A File Offset: 0x00001C7A
		public PluginPrivilegeReader(PackageSubscription ps)
		{
			this._subscription = ps;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003A89 File Offset: 0x00001C89
		public List<PluginPrivilege> GetPluginPrivileges(string pluginName)
		{
			return new List<PluginPrivilege> { PluginPrivilege.KeepAlive };
		}

		// Token: 0x04000027 RID: 39
		private PackageSubscription _subscription;
	}
}
