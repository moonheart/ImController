using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Lenovo.Modern.CoreTypes.Events.Network;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.Network
{
	// Token: 0x02000017 RID: 23
	internal class NetworkMonitor : EventMonitorBase
	{
		// Token: 0x06000069 RID: 105 RVA: 0x000051C8 File Offset: 0x000033C8
		public NetworkMonitor()
		{
			this._subscriptionMappingList = new ConcurrentBag<EventSubscriptionMapping<NetworkEventSubscription>>();
			this._mutex = new Mutex();
			NetworkMonitor._addrChangeTimer = new System.Timers.Timer((double)this._addrChangeDelayMS);
			NetworkMonitor._addrChangeTimer.Elapsed += delegate(object s, ElapsedEventArgs e)
			{
				this.OnTimerEvent();
			};
			NetworkMonitor._addrChangeTimer.AutoReset = false;
			NetworkMonitor._addrChangeTimer.Enabled = false;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00005239 File Offset: 0x00003439
		public override string Version
		{
			get
			{
				return NetworkMonitorConstants.Version;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00005240 File Offset: 0x00003440
		public override string Name
		{
			get
			{
				return NetworkMonitorConstants.MonitorName;
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00005248 File Offset: 0x00003448
		public override void RegisterSubscribedEvent(SubscribedEvent subscribedEvent)
		{
			string parameter = subscribedEvent.Parameter;
			NetworkEventSubscription networkEventSubscription = null;
			if (!string.IsNullOrEmpty(parameter) && parameter.Contains("NetworkEventSubscription"))
			{
				networkEventSubscription = Serializer.Deserialize<NetworkEventSubscription>(subscribedEvent.Parameter);
			}
			if (networkEventSubscription != null)
			{
				this.SubscribeToNetworkEvent(networkEventSubscription, subscribedEvent);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000344B File Offset: 0x0000164B
		public override Task<bool> InitializeAsync(EventHandlerReason reason)
		{
			return Task.FromResult<bool>(true);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000528C File Offset: 0x0000348C
		public override void Unregister(EventHandlerReason reason)
		{
			NetworkMonitor._addrChangeTimer.Stop();
			if (this._isNetworkAddressChangedRegistered)
			{
				NetworkMonitor.DetachWatcher();
				this._isNetworkAddressChangedRegistered = false;
			}
			while (!this._subscriptionMappingList.IsEmpty)
			{
				EventSubscriptionMapping<NetworkEventSubscription> eventSubscriptionMapping = null;
				this._subscriptionMappingList.TryTake(out eventSubscriptionMapping);
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000052D8 File Offset: 0x000034D8
		private static void AttachWatcher()
		{
			Logger.Log(Logger.LogSeverity.Information, "AttachWatcher: Entry");
			EventLogQuery eventQuery = new EventLogQuery("Microsoft-Windows-NetworkProfile/Operational", PathType.LogName, "*[System[(EventID = 10000)] or System[(EventID = 10001)]] or System[(EventID = 10002)]");
			if (NetworkMonitor._logWatcher == null)
			{
				NetworkMonitor._logWatcher = new EventLogWatcher(eventQuery);
				NetworkMonitor._logWatcher.EventRecordWritten += NetworkMonitor.OnEventLogWritten;
				NetworkMonitor._logWatcher.Enabled = true;
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00005334 File Offset: 0x00003534
		private static void DetachWatcher()
		{
			Logger.Log(Logger.LogSeverity.Information, "DetachWatcher: Entry");
			if (NetworkMonitor._logWatcher != null)
			{
				NetworkMonitor._logWatcher.Enabled = false;
				NetworkMonitor._logWatcher.EventRecordWritten -= NetworkMonitor.OnEventLogWritten;
				NetworkMonitor._logWatcher = null;
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000536F File Offset: 0x0000356F
		private bool SubscribeToNetworkEvent(NetworkEventSubscription subscriptionData, SubscribedEvent subscribedEvent)
		{
			if (subscriptionData.NetworkAddressChange && !this._isNetworkAddressChangedRegistered)
			{
				NetworkMonitor.AttachWatcher();
				this._isNetworkAddressChangedRegistered = true;
			}
			this._subscriptionMappingList.Add(new EventSubscriptionMapping<NetworkEventSubscription>(subscriptionData, subscribedEvent));
			return true;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000053A0 File Offset: 0x000035A0
		private static void OnEventLogWritten(object obj, EventRecordWrittenEventArgs arg)
		{
			Logger.Log(Logger.LogSeverity.Information, "OnEventLogWritten: Entry");
			NetworkMonitor._addrChangeTimer.Stop();
			NetworkMonitor._addrChangeTimer.Start();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000053C1 File Offset: 0x000035C1
		private void AddressChangedCallback(object sender, EventArgs e)
		{
			Logger.Log(Logger.LogSeverity.Information, "AddressChangedCallback");
			NetworkMonitor._addrChangeTimer.Stop();
			NetworkMonitor._addrChangeTimer.Start();
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000053E4 File Offset: 0x000035E4
		private async Task OnTimerEvent()
		{
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "AddressChangedCallbackTimer.Entry");
				NetworkEventReaction networkEventReaction = new NetworkEventReaction();
				networkEventReaction.NetworkAddressChange = true;
				bool internetConnectionOn = false;
				networkEventReaction.InternetConnectivityChanged = false;
				networkEventReaction.InternetConnectionOn = internetConnectionOn;
				string parameter = Serializer.Serialize<NetworkEventReaction>(networkEventReaction);
				EventReaction eventReaction = new EventReaction
				{
					Monitor = NetworkMonitorConstants.MonitorName,
					DataType = NetworkMonitorConstants.DataType,
					Trigger = NetworkMonitorConstants.TriggerAddressChanged,
					Parameter = parameter
				};
				foreach (SubscribedEvent subscribedEvent in this.GetRecepientsForEvent(networkEventReaction))
				{
					base.NotifyObservers(eventReaction, subscribedEvent);
				}
				Logger.Log(Logger.LogSeverity.Information, "AddressChangedCallbackTimer.Exit");
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception in NetworkMonitor.OnTimerEvent");
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000542C File Offset: 0x0000362C
		private bool GetInternetConnectionStatus()
		{
			bool result = false;
			try
			{
				int num = 0;
				result = NetworkMonitorConstants.InternetGetConnectedState(out num, 0);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "GetInternetConnectionStatus: Exception occured while getting internet connection status");
			}
			Logger.Log(Logger.LogSeverity.Information, "GetInternetConnectionStatus returned {0}", new object[] { result.ToString() });
			return result;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00005480 File Offset: 0x00003680
		private bool IsInternetConnectionStatusChanged(out bool internetConnectionStatus)
		{
			bool result = false;
			this._mutex.WaitOne();
			internetConnectionStatus = this.GetInternetConnectionStatus();
			if (this._cachedInternetStatus != internetConnectionStatus)
			{
				this._cachedInternetStatus = internetConnectionStatus;
				result = true;
			}
			this._mutex.ReleaseMutex();
			return result;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000054C4 File Offset: 0x000036C4
		private IEnumerable<SubscribedEvent> GetRecepientsForEvent(NetworkEventReaction networkEvent)
		{
			List<SubscribedEvent> list = new List<SubscribedEvent>();
			foreach (EventSubscriptionMapping<NetworkEventSubscription> eventSubscriptionMapping in this._subscriptionMappingList)
			{
				if (networkEvent.InternetConnectivityChanged && eventSubscriptionMapping.EventSubscriptionData.InternetConnectivityChanged)
				{
					list.Add(eventSubscriptionMapping.SubscribedEvent);
				}
				else if (networkEvent.NetworkAddressChange && eventSubscriptionMapping.EventSubscriptionData.NetworkAddressChange)
				{
					list.Add(eventSubscriptionMapping.SubscribedEvent);
				}
			}
			return list;
		}

		// Token: 0x04000052 RID: 82
		private ConcurrentBag<EventSubscriptionMapping<NetworkEventSubscription>> _subscriptionMappingList;

		// Token: 0x04000053 RID: 83
		private Mutex _mutex;

		// Token: 0x04000054 RID: 84
		private bool _cachedInternetStatus;

		// Token: 0x04000055 RID: 85
		private bool _isNetworkAddressChangedRegistered;

		// Token: 0x04000056 RID: 86
		private static System.Timers.Timer _addrChangeTimer;

		// Token: 0x04000057 RID: 87
		private static EventLogWatcher _logWatcher;

		// Token: 0x04000058 RID: 88
		private readonly int _addrChangeDelayMS = 200;
	}
}
