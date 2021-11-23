using System;
using System.Collections.Generic;
using Lenovo.Modern.ImController.ImClient.Models;

namespace Lenovo.Modern.ImController.EventManager.Services
{
	// Token: 0x02000003 RID: 3
	internal class HardcodedEventPrioritizer : IEventPrioritizer
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002173 File Offset: 0x00000373
		public EventPriority GetEventPrioprity(EventReaction eventReaction, List<PluginPrivilege> pluginPrivilegeList)
		{
			if (!pluginPrivilegeList.Contains(PluginPrivilege.ImmediateEventNotification))
			{
				return EventPriority.PriorityNormal;
			}
			return EventPriority.PriorityImmediate;
		}
	}
}
