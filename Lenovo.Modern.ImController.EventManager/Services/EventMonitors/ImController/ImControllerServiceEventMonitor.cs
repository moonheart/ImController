using System;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Services;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.ImController
{
	// Token: 0x0200001C RID: 28
	internal class ImControllerServiceEventMonitor : EventMonitorBase
	{
		// Token: 0x06000087 RID: 135 RVA: 0x00005A4F File Offset: 0x00003C4F
		public ImControllerServiceEventMonitor()
		{
			this._eventReaction = new EventReaction
			{
				Monitor = ImControllerEventConstants.MonitorName,
				Trigger = "imc-startup",
				DataType = ImControllerEventConstants.ImControllerEventDataType
			};
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00005A83 File Offset: 0x00003C83
		public override string Name
		{
			get
			{
				return ImControllerEventConstants.MonitorName;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00005A8A File Offset: 0x00003C8A
		public override string Version
		{
			get
			{
				return ImControllerEventConstants.Version;
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000344B File Offset: 0x0000164B
		public override Task<bool> InitializeAsync(EventHandlerReason reason)
		{
			return Task.FromResult<bool>(true);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00005A91 File Offset: 0x00003C91
		public override void RegisterSubscribedEvent(SubscribedEvent subscribedEvent)
		{
			if (!ImControllerServiceEventMonitor._imcStartAlreadyHandled && subscribedEvent.Trigger.Equals("imc-startup"))
			{
				base.NotifyObservers(this._eventReaction, subscribedEvent);
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00005AB9 File Offset: 0x00003CB9
		public override void Unregister(EventHandlerReason reason)
		{
			ImControllerServiceEventMonitor._imcStartAlreadyHandled = true;
		}

		// Token: 0x04000065 RID: 101
		private EventReaction _eventReaction;

		// Token: 0x04000066 RID: 102
		private static bool _imcStartAlreadyHandled;
	}
}
