using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Events.SystemEvent;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.SystemContext.Interop;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.SystemEvent
{
	// Token: 0x02000012 RID: 18
	internal class SystemEventMonitor : EventMonitorBase
	{
		// Token: 0x06000050 RID: 80 RVA: 0x0000458B File Offset: 0x0000278B
		public SystemEventMonitor()
		{
			this._subscriptionMappingList = new ConcurrentBag<EventSubscriptionMapping<SystemEventSubscription>>();
			SystemEventMonitor._instance = this;
			this._logonEventMutex = new SemaphoreSlim(1, 1);
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000045B1 File Offset: 0x000027B1
		public override string Version
		{
			get
			{
				return SystemEventMonitorConstants.Version;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000045B8 File Offset: 0x000027B8
		public override string Name
		{
			get
			{
				return SystemEventMonitorConstants.MonitorName;
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000045C0 File Offset: 0x000027C0
		public override void RegisterSubscribedEvent(SubscribedEvent subscribedEvent)
		{
			string parameter = subscribedEvent.Parameter;
			SystemEventSubscription systemEventSubscription = null;
			if (!string.IsNullOrEmpty(parameter) && parameter.Contains("SystemEventSubscription"))
			{
				systemEventSubscription = Serializer.Deserialize<SystemEventSubscription>(subscribedEvent.Parameter);
			}
			else if (subscribedEvent.Trigger.Equals("User-Login"))
			{
				systemEventSubscription = new SystemEventSubscription();
			}
			if (systemEventSubscription != null)
			{
				this._subscriptionMappingList.Add(new EventSubscriptionMapping<SystemEventSubscription>(systemEventSubscription, subscribedEvent));
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000344B File Offset: 0x0000164B
		public override Task<bool> InitializeAsync(EventHandlerReason reason)
		{
			return Task.FromResult<bool>(true);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004628 File Offset: 0x00002828
		public override void Unregister(EventHandlerReason reason)
		{
			while (!this._subscriptionMappingList.IsEmpty)
			{
				EventSubscriptionMapping<SystemEventSubscription> eventSubscriptionMapping = null;
				this._subscriptionMappingList.TryTake(out eventSubscriptionMapping);
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004654 File Offset: 0x00002854
		public static void SystemEventCallback(int eventType, IntPtr eventData, uint sessionId)
		{
			EventManager eventManagerPreviouslyCreatedInstance = EventManagerFactory.GetEventManagerPreviouslyCreatedInstance();
			if (eventManagerPreviouslyCreatedInstance != null)
			{
				if (5 == eventType)
				{
					SystemEventMonitor._instance._logonEventMutex.Wait();
					if (!SystemEventMonitor._instance._IsFirstLogonEventFired)
					{
						SystemEventMonitor._instance._IsFirstLogonEventFired = true;
						SystemEventMonitor._instance.ReceiveUserLoginTrigger();
						Logger.Log(Logger.LogSeverity.Information, "User Logon Event occured");
					}
					SystemEventMonitor._instance._logonEventMutex.Release();
					return;
				}
				if (6 != eventType && (8 == eventType || 7 == eventType || 1 == eventType || 2 == eventType || 1001 == eventType || 10001 == eventType) && sessionId != 0U)
				{
					SystemEventReaction systemEventReaction = new SystemEventReaction();
					systemEventReaction.SystemStart = 10001 == eventType;
					systemEventReaction.SystemShutdown = 1001 == eventType;
					string parameter = Serializer.Serialize<SystemEventReaction>(systemEventReaction);
					EventReaction eventReaction = new EventReaction
					{
						Monitor = SystemEventMonitorConstants.MonitorName,
						DataType = SystemEventMonitorConstants.DataType,
						Trigger = SystemEventMonitorConstants.Trigger,
						Parameter = parameter
					};
					foreach (SubscribedEvent subscribedEvent in SystemEventMonitor._instance.GetRecepientsForEvent(systemEventReaction))
					{
						SystemEventMonitor._instance.NotifyObservers(eventReaction, subscribedEvent);
					}
					SystemEventMonitor._instance._logonEventMutex.Wait();
					if (systemEventReaction.SystemStart && !SystemEventMonitor._instance._IsFirstLogonEventFired)
					{
						IntPtr zero = IntPtr.Zero;
						if (Authorization.GetSessionUserToken(ref zero))
						{
							Logger.Log(Logger.LogSeverity.Information, "We missed Logon Event. Simulating the logon event");
							SystemEventMonitor._instance.ReceiveUserLoginTrigger();
							SystemEventMonitor._instance._IsFirstLogonEventFired = true;
						}
					}
					SystemEventMonitor._instance._logonEventMutex.Release();
				}
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000047FC File Offset: 0x000029FC
		private bool ReceiveUserLoginTrigger()
		{
			List<EventSubscriptionMapping<SystemEventSubscription>> list = (from mapping in this._subscriptionMappingList
				where mapping.SubscribedEvent.Monitor != null && mapping.SubscribedEvent.Trigger != null && mapping.SubscribedEvent.Monitor.Equals(SystemEventMonitorConstants.MonitorName, StringComparison.OrdinalIgnoreCase) && mapping.SubscribedEvent.Trigger.Equals("User-Login", StringComparison.OrdinalIgnoreCase)
				select mapping).ToList<EventSubscriptionMapping<SystemEventSubscription>>();
			if (list != null && list.Any<EventSubscriptionMapping<SystemEventSubscription>>())
			{
				EventReaction eventReaction = new EventReaction
				{
					Monitor = SystemEventMonitorConstants.MonitorName,
					Trigger = "User-Login",
					DataType = SystemEventMonitorConstants.DataType
				};
				foreach (EventSubscriptionMapping<SystemEventSubscription> eventSubscriptionMapping in list)
				{
					base.NotifyObservers(eventReaction, eventSubscriptionMapping.SubscribedEvent);
				}
			}
			return true;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000048B4 File Offset: 0x00002AB4
		private IEnumerable<SubscribedEvent> GetRecepientsForEvent(SystemEventReaction SystemEvent)
		{
			List<SubscribedEvent> list = new List<SubscribedEvent>();
			foreach (EventSubscriptionMapping<SystemEventSubscription> eventSubscriptionMapping in this._subscriptionMappingList)
			{
				if (SystemEvent.SystemShutdown && eventSubscriptionMapping.EventSubscriptionData.SystemShutdown)
				{
					list.Add(eventSubscriptionMapping.SubscribedEvent);
				}
				else if (SystemEvent.SystemStart && eventSubscriptionMapping.EventSubscriptionData.SystemStart)
				{
					list.Add(eventSubscriptionMapping.SubscribedEvent);
				}
			}
			return list;
		}

		// Token: 0x04000032 RID: 50
		private ConcurrentBag<EventSubscriptionMapping<SystemEventSubscription>> _subscriptionMappingList;

		// Token: 0x04000033 RID: 51
		private bool _IsFirstLogonEventFired;

		// Token: 0x04000034 RID: 52
		private static SystemEventMonitor _instance;

		// Token: 0x04000035 RID: 53
		private SemaphoreSlim _logonEventMutex;
	}
}
