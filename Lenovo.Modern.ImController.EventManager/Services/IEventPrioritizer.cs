using System;
using System.Collections.Generic;
using Lenovo.Modern.ImController.ImClient.Models;

namespace Lenovo.Modern.ImController.EventManager.Services
{
	// Token: 0x02000005 RID: 5
	public interface IEventPrioritizer
	{
		// Token: 0x06000006 RID: 6
		EventPriority GetEventPrioprity(EventReaction eventReaction, List<PluginPrivilege> pluginPrivilegeList);
	}
}
