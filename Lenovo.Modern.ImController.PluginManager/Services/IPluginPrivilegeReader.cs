using System;
using System.Collections.Generic;
using Lenovo.Modern.ImController.ImClient.Models;

namespace Lenovo.Modern.ImController.PluginManager.Services
{
	// Token: 0x02000003 RID: 3
	public interface IPluginPrivilegeReader
	{
		// Token: 0x06000002 RID: 2
		List<PluginPrivilege> GetPluginPrivileges(string pluginName);
	}
}
