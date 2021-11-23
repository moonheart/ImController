using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Events.Network;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.Network
{
	// Token: 0x02000019 RID: 25
	internal class WlanMonitor : EventMonitorBase
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00005595 File Offset: 0x00003795
		public WlanMonitor()
		{
			this._wlanSubscriptionList = new BlockingCollection<SubscribedEvent>();
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000055B3 File Offset: 0x000037B3
		public override string Version
		{
			get
			{
				return WlanEventConstants.Get.WlanEventVersion;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000055BF File Offset: 0x000037BF
		public override string Name
		{
			get
			{
				return WlanEventConstants.Get.WlanEventMonitorName;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000055CC File Offset: 0x000037CC
		public override void RegisterSubscribedEvent(SubscribedEvent subscribedEvent)
		{
			if (subscribedEvent.Monitor == this.Name)
			{
				int wlanMonitorFlagFromSubscribedEvent = this.GetWlanMonitorFlagFromSubscribedEvent(subscribedEvent);
				if (this.RegisterWlanNotification(wlanMonitorFlagFromSubscribedEvent))
				{
					this._wlanNotifyRegistered = true;
					this._wlanSubscriptionList.Add(subscribedEvent);
				}
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000344B File Offset: 0x0000164B
		public override Task<bool> InitializeAsync(EventHandlerReason reason)
		{
			return Task.FromResult<bool>(true);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005610 File Offset: 0x00003810
		public override void Unregister(EventHandlerReason reason)
		{
			try
			{
				if (this._wlanNotifyRegistered)
				{
					if (!this._hClient.Equals(IntPtr.Zero))
					{
						WlanMonitorConstants.Win32.WlanCloseHandle(this._hClient, IntPtr.Zero);
					}
					this._hClient = IntPtr.Zero;
					this._wlanNotifyRegistered = false;
				}
				goto IL_60;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "WLANEventMonitor.Unregister: Exception ");
				goto IL_60;
			}
			IL_50:
			SubscribedEvent subscribedEvent = null;
			this._wlanSubscriptionList.TryTake(out subscribedEvent);
			IL_60:
			if (!this._wlanSubscriptionList.Any<SubscribedEvent>())
			{
				return;
			}
			goto IL_50;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000569C File Offset: 0x0000389C
		public bool RegisterWlanNotification(int flag)
		{
			int num = 0;
			uint num2 = 0U;
			try
			{
				if (this._hClient.Equals(IntPtr.Zero))
				{
					num = WlanMonitorConstants.Win32.WlanOpenHandle(2U, IntPtr.Zero, ref num2, ref this._hClient);
				}
				if (num != 0)
				{
					return false;
				}
				this._wlanNotificationCallbackDelegate = new WlanMonitorConstants.Win32.WLAN_NOTIFICATION_CALLBACK(this.WlanNotification);
				WlanEventConstants.Win32.WLAN_NOTIFICATION_SOURCE wlan_NOTIFICATION_SOURCE = WlanEventConstants.Win32.WLAN_NOTIFICATION_SOURCE.None;
				if (WlanMonitorConstants.Win32.WlanRegisterNotification(this._hClient, flag, false, this._wlanNotificationCallbackDelegate, IntPtr.Zero, IntPtr.Zero, ref wlan_NOTIFICATION_SOURCE) != 0)
				{
					return false;
				}
				if (wlan_NOTIFICATION_SOURCE != WlanEventConstants.Win32.WLAN_NOTIFICATION_SOURCE.None && WlanMonitorConstants.Win32.WlanRegisterNotification(this._hClient, flag | (int)wlan_NOTIFICATION_SOURCE, false, this._wlanNotificationCallbackDelegate, IntPtr.Zero, IntPtr.Zero, ref wlan_NOTIFICATION_SOURCE) != 0)
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "WLANEventMonitor.RegisterWlanNotification: Exception ");
			}
			return true;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00005768 File Offset: 0x00003968
		private void WlanNotification(ref WlanMonitorConstants.Win32.WLAN_NOTIFICATION_DATA notificationData, IntPtr context)
		{
			try
			{
				byte[] array = new byte[notificationData.dataSize];
				if (notificationData.dataSize > 0)
				{
					Marshal.Copy(notificationData.data, array, 0, notificationData.dataSize);
					string text = "";
					if (notificationData.notificationSource == 8)
					{
						text = WlanEventConstants.Get.WlanAcmEventTriggerName;
					}
					if (notificationData.notificationSource == 16)
					{
						if (notificationData.notificationCode == 1)
						{
							text = WlanEventConstants.Get.WlanMsmAssociatingTriggerName;
						}
						if (notificationData.notificationCode == 2)
						{
							text = WlanEventConstants.Get.WlanMsmAssociatedTriggerName;
						}
						if (notificationData.notificationCode == 3)
						{
							text = WlanEventConstants.Get.WlanMsmAuthenticatingTriggerName;
						}
						if (notificationData.notificationCode == 4)
						{
							text = WlanEventConstants.Get.WlanMsmConnectedTriggerName;
						}
						if (notificationData.notificationCode == 10)
						{
							text = WlanEventConstants.Get.WlanMsmDisconnectedTriggerName;
						}
					}
					if (!string.IsNullOrWhiteSpace(text))
					{
						string parameter = Serializer.Serialize<WlanEventReaction>(new WlanEventReaction
						{
							InterfaceGuid = notificationData.InterfaceGuid.ToString(),
							NotificationCode = notificationData.notificationCode,
							NotificationSource = notificationData.notificationSource,
							DwDataSize = notificationData.dataSize,
							Data = array
						});
						EventReaction eventReaction = new EventReaction
						{
							Monitor = WlanEventConstants.Get.WlanEventMonitorName,
							Trigger = text,
							Parameter = parameter
						};
						foreach (SubscribedEvent subscribedEvent in this.GetRecepientsForEvent(text))
						{
							base.NotifyObservers(eventReaction, subscribedEvent);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception occured in WlanNotification callback");
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000592C File Offset: 0x00003B2C
		private IEnumerable<SubscribedEvent> GetRecepientsForEvent(string trigger)
		{
			List<SubscribedEvent> list = new List<SubscribedEvent>();
			foreach (SubscribedEvent subscribedEvent in ((IEnumerable<SubscribedEvent>)this._wlanSubscriptionList))
			{
				if (string.Compare(subscribedEvent.Trigger, trigger, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					list.Add(subscribedEvent);
				}
			}
			return list;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00005990 File Offset: 0x00003B90
		private int GetWlanMonitorFlagFromSubscribedEvent(SubscribedEvent subscribedEvent)
		{
			int result = 0;
			if (subscribedEvent.Trigger == WlanEventConstants.Get.WlanAcmEventTriggerName)
			{
				result = 8;
			}
			if (subscribedEvent.Trigger == WlanEventConstants.Get.WlanMsmAssociatedTriggerName || subscribedEvent.Trigger == WlanEventConstants.Get.WlanMsmConnectedTriggerName || subscribedEvent.Trigger == WlanEventConstants.Get.WlanMsmDisconnectedTriggerName || subscribedEvent.Trigger == WlanEventConstants.Get.WlanMsmAssociatingTriggerName || subscribedEvent.Trigger == WlanEventConstants.Get.WlanMsmAuthenticatingTriggerName)
			{
				result = 16;
			}
			return result;
		}

		// Token: 0x0400005E RID: 94
		private WlanMonitorConstants.Win32.WLAN_NOTIFICATION_CALLBACK _wlanNotificationCallbackDelegate;

		// Token: 0x0400005F RID: 95
		private BlockingCollection<SubscribedEvent> _wlanSubscriptionList;

		// Token: 0x04000060 RID: 96
		private bool _wlanNotifyRegistered;

		// Token: 0x04000061 RID: 97
		private IntPtr _hClient = IntPtr.Zero;
	}
}
