using System;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Services;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors
{
	// Token: 0x0200000D RID: 13
	internal abstract class EventMonitorBase : IEventMonitor
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000023 RID: 35
		public abstract string Version { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000024 RID: 36
		public abstract string Name { get; }

		// Token: 0x06000025 RID: 37
		public abstract void RegisterSubscribedEvent(SubscribedEvent subscribedEvent);

		// Token: 0x06000026 RID: 38
		public abstract void Unregister(EventHandlerReason reason);

		// Token: 0x06000027 RID: 39
		public abstract Task<bool> InitializeAsync(EventHandlerReason reason);

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000028 RID: 40 RVA: 0x000027E0 File Offset: 0x000009E0
		// (remove) Token: 0x06000029 RID: 41 RVA: 0x00002818 File Offset: 0x00000A18
		public event EventGeneratorDelegate EventGenerator;

		// Token: 0x0600002A RID: 42 RVA: 0x0000284D File Offset: 0x00000A4D
		protected void NotifyObservers(EventReaction eventReaction, SubscribedEvent subscribedEvent)
		{
			if (this.EventGenerator != null)
			{
				this.EventGenerator(eventReaction, subscribedEvent);
			}
		}
	}
}
