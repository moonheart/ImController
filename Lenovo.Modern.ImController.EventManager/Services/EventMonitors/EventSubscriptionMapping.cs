using System;
using Lenovo.Modern.ImController.Shared.Model.Packages;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors
{
	// Token: 0x0200000E RID: 14
	public class EventSubscriptionMapping<T>
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002864 File Offset: 0x00000A64
		// (set) Token: 0x0600002D RID: 45 RVA: 0x0000286C File Offset: 0x00000A6C
		public T EventSubscriptionData { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002875 File Offset: 0x00000A75
		// (set) Token: 0x0600002F RID: 47 RVA: 0x0000287D File Offset: 0x00000A7D
		public SubscribedEvent SubscribedEvent { get; set; }

		// Token: 0x06000030 RID: 48 RVA: 0x00002886 File Offset: 0x00000A86
		public EventSubscriptionMapping(T subscriptionData, SubscribedEvent subscribedEvent)
		{
			this.EventSubscriptionData = subscriptionData;
			this.SubscribedEvent = subscribedEvent;
		}
	}
}
