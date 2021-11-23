using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Events.UsageMonitor;
using Lenovo.Modern.ImController.EventManager.Services.EventMonitors.UsageMonitor.AppUsageMonitor;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Services;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.UsageMonitor
{
	// Token: 0x0200001F RID: 31
	internal class UsageMonitor : EventMonitorBase
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00005F78 File Offset: 0x00004178
		public UsageMonitor()
		{
			this._appUsageSubscriptionList = new BlockingCollection<SubscribedEvent>();
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00005F8B File Offset: 0x0000418B
		public override string Version
		{
			get
			{
				return UsageMonitorEventConstants.Get.UsageMonitorEventVersion;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00005F97 File Offset: 0x00004197
		public override string Name
		{
			get
			{
				return UsageMonitorEventConstants.Get.UsageMonitorEventMonitorName;
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005FA3 File Offset: 0x000041A3
		public override void RegisterSubscribedEvent(SubscribedEvent subscribedEvent)
		{
			if (subscribedEvent.Monitor == this.Name && subscribedEvent.Trigger == UsageMonitorEventConstants.Get.UsageMonitorAppTrigger)
			{
				this.SubscribeToAppUsageMonitorEvent(subscribedEvent);
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005FD7 File Offset: 0x000041D7
		public override Task<bool> InitializeAsync(EventHandlerReason reason)
		{
			this._startReason = reason;
			return Task.FromResult<bool>(true);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005FE8 File Offset: 0x000041E8
		public override void Unregister(EventHandlerReason reason)
		{
			if (this._appUsageSubscriptionList.Any<SubscribedEvent>())
			{
				AppUsageMonitor.GetInstance().Stop(reason);
			}
			while (this._appUsageSubscriptionList.Any<SubscribedEvent>())
			{
				SubscribedEvent subscribedEvent = null;
				this._appUsageSubscriptionList.TryTake(out subscribedEvent);
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000602C File Offset: 0x0000422C
		private bool SubscribeToAppUsageMonitorEvent(SubscribedEvent subscribedEvent)
		{
			AppUsageMonitor.GetInstance().Start(this._startReason, subscribedEvent);
			this._appUsageSubscriptionList.Add(subscribedEvent);
			return true;
		}

		// Token: 0x0400006F RID: 111
		private BlockingCollection<SubscribedEvent> _appUsageSubscriptionList;

		// Token: 0x04000070 RID: 112
		private EventHandlerReason _startReason;
	}
}
