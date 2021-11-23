using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Events.AppMonitor;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.AppMonitor
{
	// Token: 0x02000023 RID: 35
	internal class AppMonitorMonitor : EventMonitorBase
	{
		// Token: 0x060000CC RID: 204 RVA: 0x00006F20 File Offset: 0x00005120
		public AppMonitorMonitor()
		{
			this._subscriptionMappingList = new ConcurrentBag<EventSubscriptionMapping<AppMonitorEventSubscription>>();
			this._pfnMonitorList = new ConcurrentDictionary<string, string>();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006F40 File Offset: 0x00005140
		~AppMonitorMonitor()
		{
			if (this._processWatcher != null)
			{
				try
				{
					this._processWatcher.Stop();
					this._processWatcher.EventArrived -= this.OnProcessStarted;
					this._processWatcher = null;
					this._isProcessWatchStarted = false;
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00006FB0 File Offset: 0x000051B0
		public override string Version
		{
			get
			{
				return AppMonitorEventConstants.Get.AppMonitorEventVersion;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00006FBC File Offset: 0x000051BC
		public override string Name
		{
			get
			{
				return AppMonitorEventConstants.Get.AppMonitorEventMonitorName;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006FC8 File Offset: 0x000051C8
		public override void RegisterSubscribedEvent(SubscribedEvent subscribedEvent)
		{
			string parameter = subscribedEvent.Parameter;
			AppMonitorEventSubscription appMonitorEventSubscription = null;
			if (!string.IsNullOrEmpty(parameter) && parameter.Contains("AppMonitorSubscription"))
			{
				appMonitorEventSubscription = Serializer.Deserialize<AppMonitorEventSubscription>(subscribedEvent.Parameter);
			}
			if (appMonitorEventSubscription != null)
			{
				this.SubscribeToAppMonitorEvent(appMonitorEventSubscription, subscribedEvent);
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000344B File Offset: 0x0000164B
		public override Task<bool> InitializeAsync(EventHandlerReason reason)
		{
			return Task.FromResult<bool>(true);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000700C File Offset: 0x0000520C
		public override void Unregister(EventHandlerReason reason)
		{
			if (this._isProcessWatchStarted && this._processWatcher != null)
			{
				try
				{
					this._processWatcher.Stop();
					this._processWatcher.EventArrived -= this.OnProcessStarted;
					this._processWatcher = null;
					this._isProcessWatchStarted = false;
				}
				catch (Exception)
				{
				}
			}
			this._pfnMonitorList = new ConcurrentDictionary<string, string>();
			UapInstallMonitor.GetInstance().Reset();
			if (this._isInstallUninstallEventHandlerRegistered)
			{
				UapInstallMonitor.GetInstance().AppStatusChangedEvent -= this.AppStatusChanged;
				this._isInstallUninstallEventHandlerRegistered = false;
			}
			while (!this._subscriptionMappingList.IsEmpty)
			{
				EventSubscriptionMapping<AppMonitorEventSubscription> eventSubscriptionMapping = null;
				this._subscriptionMappingList.TryTake(out eventSubscriptionMapping);
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000070C8 File Offset: 0x000052C8
		private bool SubscribeToAppMonitorEvent(AppMonitorEventSubscription subscriptionData, SubscribedEvent subscribedEvent)
		{
			Logger.Log(Logger.LogSeverity.Information, "AppMonitor: Subscribing to the AppMonitor Event. Monitor:{0}, Monitor:{0}, Plugin:{1} and Trigger:{2}", new object[] { subscribedEvent.Monitor, subscribedEvent.Plugin, subscribedEvent.Trigger });
			if (subscriptionData.AppEventList != null && subscriptionData.AppEventList.Any<AppEvent>())
			{
				if (subscribedEvent.Trigger.Equals(AppMonitorEventConstants.Get.AppMonitorAppOpenedEventTrigger) && !this._isProcessWatchStarted)
				{
					try
					{
						this._processWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
						this._processWatcher.EventArrived += this.OnProcessStarted;
						this._processWatcher.Start();
						this._isProcessWatchStarted = true;
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "AppMonitor: Unable to start AppMonitor");
					}
				}
				if (subscribedEvent.Trigger.Equals(AppMonitorEventConstants.Get.AppMonitorInstallUninstallTrigger))
				{
					UapInstallMonitor.GetInstance().ResetTerminateEvent();
					foreach (AppEvent appEvent in subscriptionData.AppEventList)
					{
						string text;
						if (!this._pfnMonitorList.TryGetValue(appEvent.Pfn, out text))
						{
							this._pfnMonitorList.TryAdd(appEvent.Pfn, "started");
							UapInstallMonitor.GetInstance().RegisterMonitor(appEvent.Pfn);
						}
					}
					if (!this._isInstallUninstallEventHandlerRegistered)
					{
						UapInstallMonitor.GetInstance().AppStatusChangedEvent += this.AppStatusChanged;
						UapInstallMonitor.GetInstance().TriggerPendingAppStatusNotificationsAsync();
						this._isInstallUninstallEventHandlerRegistered = true;
					}
				}
			}
			this._subscriptionMappingList.Add(new EventSubscriptionMapping<AppMonitorEventSubscription>(subscriptionData, subscribedEvent));
			return true;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00007254 File Offset: 0x00005454
		private void OnProcessStarted(object sender, EventArrivedEventArgs e)
		{
			try
			{
				int processId = int.Parse(e.NewEvent.Properties["ProcessId"].Value.ToString());
				IntPtr intPtr = AppMonitorMonitor.OpenProcess(AppMonitorMonitor.ProcessAccessFlags.QueryInformation, false, processId);
				uint num = 0U;
				StringBuilder stringBuilder = null;
				long packageFamilyName = AppMonitorMonitor.GetPackageFamilyName(intPtr, out num, stringBuilder);
				if (packageFamilyName > 0L && num > 0U)
				{
					stringBuilder = new StringBuilder((int)num);
					packageFamilyName = AppMonitorMonitor.GetPackageFamilyName(intPtr, out num, stringBuilder);
				}
				if (packageFamilyName == 0L && stringBuilder != null)
				{
					AppMonitorEventReaction appMonitorEventReaction = new AppMonitorEventReaction();
					appMonitorEventReaction.UapAppPfn = stringBuilder.ToString();
					appMonitorEventReaction.Event = AppMonitorEventConstants.Get.AppMonitorEventOpen;
					string parameter = Serializer.Serialize<AppMonitorEventReaction>(appMonitorEventReaction);
					EventReaction eventReaction = new EventReaction
					{
						Monitor = AppMonitorEventConstants.Get.AppMonitorEventMonitorName,
						DataType = AppMonitorEventConstants.Get.AppMonitorEventDataType,
						Trigger = AppMonitorEventConstants.Get.AppMonitorAppOpenedEventTrigger,
						Parameter = parameter
					};
					using (IEnumerator<SubscribedEvent> enumerator = this.GetPfnRecepientsForEvent(appMonitorEventReaction).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SubscribedEvent subscribedEvent = enumerator.Current;
							base.NotifyObservers(eventReaction, subscribedEvent);
						}
						goto IL_1D6;
					}
				}
				AppMonitorEventReaction appMonitorEventReaction2 = new AppMonitorEventReaction();
				appMonitorEventReaction2.DesktopApp = e.NewEvent.Properties["ProcessName"].Value.ToString();
				appMonitorEventReaction2.Event = AppMonitorEventConstants.Get.AppMonitorEventOpen;
				string parameter2 = Serializer.Serialize<AppMonitorEventReaction>(appMonitorEventReaction2);
				EventReaction eventReaction2 = new EventReaction
				{
					Monitor = AppMonitorEventConstants.Get.AppMonitorEventMonitorName,
					DataType = AppMonitorEventConstants.Get.AppMonitorEventDataType,
					Trigger = AppMonitorEventConstants.Get.AppMonitorAppOpenedEventTrigger,
					Parameter = parameter2
				};
				foreach (SubscribedEvent subscribedEvent2 in this.GetDesktopRecepientsForEvent(appMonitorEventReaction2))
				{
					base.NotifyObservers(eventReaction2, subscribedEvent2);
				}
				IL_1D6:
				AppMonitorMonitor.CloseHandle(intPtr);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "AppMonitor: Exception during OnProcessStarted");
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00007498 File Offset: 0x00005698
		private IEnumerable<SubscribedEvent> GetPfnRecepientsForEvent(AppMonitorEventReaction AppMonitorEvent)
		{
			List<SubscribedEvent> list = new List<SubscribedEvent>();
			Func<AppEvent, bool> <>9__0;
			foreach (EventSubscriptionMapping<AppMonitorEventSubscription> eventSubscriptionMapping in this._subscriptionMappingList)
			{
				if (eventSubscriptionMapping.EventSubscriptionData.AppEventList != null)
				{
					IEnumerable<AppEvent> appEventList = eventSubscriptionMapping.EventSubscriptionData.AppEventList;
					Func<AppEvent, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = (AppEvent s) => !string.IsNullOrWhiteSpace(s.Pfn) && string.Compare(s.Pfn, AppMonitorEvent.UapAppPfn, StringComparison.InvariantCultureIgnoreCase) == 0);
					}
					if (appEventList.Any(predicate) && string.Compare(eventSubscriptionMapping.SubscribedEvent.Trigger, AppMonitorEventConstants.Get.AppMonitorAppOpenedEventTrigger, StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						list.Add(eventSubscriptionMapping.SubscribedEvent);
					}
				}
			}
			return list;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000755C File Offset: 0x0000575C
		private IEnumerable<SubscribedEvent> GetDesktopRecepientsForEvent(AppMonitorEventReaction AppMonitorEvent)
		{
			List<SubscribedEvent> list = new List<SubscribedEvent>();
			Func<AppEvent, bool> <>9__0;
			foreach (EventSubscriptionMapping<AppMonitorEventSubscription> eventSubscriptionMapping in this._subscriptionMappingList)
			{
				if (eventSubscriptionMapping.EventSubscriptionData.AppEventList != null)
				{
					IEnumerable<AppEvent> appEventList = eventSubscriptionMapping.EventSubscriptionData.AppEventList;
					Func<AppEvent, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = (AppEvent s) => !string.IsNullOrWhiteSpace(s.DesktopApp) && string.Compare(s.DesktopApp, AppMonitorEvent.DesktopApp, StringComparison.InvariantCultureIgnoreCase) == 0);
					}
					if (appEventList.Any(predicate) && string.Compare(eventSubscriptionMapping.SubscribedEvent.Trigger, AppMonitorEventConstants.Get.AppMonitorAppOpenedEventTrigger, StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						list.Add(eventSubscriptionMapping.SubscribedEvent);
					}
				}
			}
			return list;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00007620 File Offset: 0x00005820
		private IEnumerable<SubscribedEvent> GetInstallUninstallRecepientsForEvent(AppMonitorEventReaction AppMonitorEvent)
		{
			List<SubscribedEvent> list = new List<SubscribedEvent>();
			Func<AppEvent, bool> <>9__0;
			foreach (EventSubscriptionMapping<AppMonitorEventSubscription> eventSubscriptionMapping in this._subscriptionMappingList)
			{
				if (eventSubscriptionMapping.EventSubscriptionData.AppEventList != null)
				{
					IEnumerable<AppEvent> appEventList = eventSubscriptionMapping.EventSubscriptionData.AppEventList;
					Func<AppEvent, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = (AppEvent s) => !string.IsNullOrWhiteSpace(s.Pfn) && string.Compare(s.Pfn, AppMonitorEvent.UapAppPfn, StringComparison.InvariantCultureIgnoreCase) == 0);
					}
					if (appEventList.Any(predicate) && string.Compare(eventSubscriptionMapping.SubscribedEvent.Trigger, AppMonitorEventConstants.Get.AppMonitorInstallUninstallTrigger, StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						list.Add(eventSubscriptionMapping.SubscribedEvent);
					}
				}
			}
			return list;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000076E4 File Offset: 0x000058E4
		private void AppStatusChanged(UapInstallMonitor.AppStatus appStatus)
		{
			Logger.Log(Logger.LogSeverity.Information, "AppStatusChanged: AppStatus changed for pfn:{0} State:{1}", new object[] { appStatus.Pfn, appStatus.InstallState });
			if (SessionTracker.GetInstance().GetSessionId() == Guid.Empty)
			{
				Logger.Log(Logger.LogSeverity.Information, "AppStatusChanged: IMC is suspending. Do nothing.");
				return;
			}
			Logger.Log(Logger.LogSeverity.Information, "AppStatusChanged: Session guid is {0}.", new object[] { SessionTracker.GetInstance().GetSessionId().ToString() });
			AppMonitorEventReaction appMonitorEventReaction = new AppMonitorEventReaction();
			appMonitorEventReaction.UapAppPfn = appStatus.Pfn;
			appMonitorEventReaction.Event = (appStatus.InstallState ? AppMonitorEventConstants.Get.AppMonitorEventInstall : AppMonitorEventConstants.Get.AppMonitorEventUninstall);
			string parameter = Serializer.Serialize<AppMonitorEventReaction>(appMonitorEventReaction);
			EventReaction eventReaction = new EventReaction
			{
				Monitor = AppMonitorEventConstants.Get.AppMonitorEventMonitorName,
				DataType = AppMonitorEventConstants.Get.AppMonitorEventDataType,
				Trigger = AppMonitorEventConstants.Get.AppMonitorInstallUninstallTrigger,
				Parameter = parameter
			};
			foreach (SubscribedEvent subscribedEvent in this.GetInstallUninstallRecepientsForEvent(appMonitorEventReaction))
			{
				base.NotifyObservers(eventReaction, subscribedEvent);
			}
		}

		// Token: 0x060000D9 RID: 217
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenProcess(AppMonitorMonitor.ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

		// Token: 0x060000DA RID: 218
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool CloseHandle(IntPtr hHandle);

		// Token: 0x060000DB RID: 219
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern long GetPackageFamilyName(IntPtr hProcess, out uint buffSize, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder pfnName);

		// Token: 0x04000091 RID: 145
		private ConcurrentBag<EventSubscriptionMapping<AppMonitorEventSubscription>> _subscriptionMappingList;

		// Token: 0x04000092 RID: 146
		private bool _isProcessWatchStarted;

		// Token: 0x04000093 RID: 147
		private bool _isInstallUninstallEventHandlerRegistered;

		// Token: 0x04000094 RID: 148
		private ManagementEventWatcher _processWatcher;

		// Token: 0x04000095 RID: 149
		private ConcurrentDictionary<string, string> _pfnMonitorList;

		// Token: 0x02000040 RID: 64
		[Flags]
		public enum ProcessAccessFlags : uint
		{
			// Token: 0x0400011F RID: 287
			All = 2035711U,
			// Token: 0x04000120 RID: 288
			Terminate = 1U,
			// Token: 0x04000121 RID: 289
			CreateThread = 2U,
			// Token: 0x04000122 RID: 290
			VirtualMemoryOperation = 8U,
			// Token: 0x04000123 RID: 291
			VirtualMemoryRead = 16U,
			// Token: 0x04000124 RID: 292
			VirtualMemoryWrite = 32U,
			// Token: 0x04000125 RID: 293
			DuplicateHandle = 64U,
			// Token: 0x04000126 RID: 294
			CreateProcess = 128U,
			// Token: 0x04000127 RID: 295
			SetQuota = 256U,
			// Token: 0x04000128 RID: 296
			SetInformation = 512U,
			// Token: 0x04000129 RID: 297
			QueryInformation = 1024U,
			// Token: 0x0400012A RID: 298
			QueryLimitedInformation = 4096U,
			// Token: 0x0400012B RID: 299
			Synchronize = 1048576U
		}
	}
}
