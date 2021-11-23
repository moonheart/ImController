using System;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Services;

namespace Lenovo.Modern.ImController.EventManager.Services
{
	// Token: 0x0200000B RID: 11
	public interface IEventMonitor
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001B RID: 27
		string Version { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001C RID: 28
		string Name { get; }

		// Token: 0x0600001D RID: 29
		void RegisterSubscribedEvent(SubscribedEvent subscribedEvent);

		// Token: 0x0600001E RID: 30
		void Unregister(EventHandlerReason reason);

		// Token: 0x0600001F RID: 31
		Task<bool> InitializeAsync(EventHandlerReason reason);

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000020 RID: 32
		// (remove) Token: 0x06000021 RID: 33
		event EventGeneratorDelegate EventGenerator;
	}
}
