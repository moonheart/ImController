using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Events.WindowMessage;
using Lenovo.Modern.ImController.EventManager.Services.EventMonitors.SystemEvent;
using Lenovo.Modern.ImController.EventManager.Services.EventMonitors.UsageMonitor.AppUsageMonitor;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.WindowMessage
{
	// Token: 0x0200000F RID: 15
	internal class WindowMessageMonitor : EventMonitorBase
	{
		// Token: 0x06000031 RID: 49 RVA: 0x0000289C File Offset: 0x00000A9C
		public WindowMessageMonitor(IntPtr serviceHandle, ref ServiceControlHandlerDelegate svcCtrlHandlerEx)
		{
			this._serviceHandle = serviceHandle;
			this._pwrSubscriptionMappingList = new ConcurrentBag<EventSubscriptionMapping<PowerBroadcastEventSubscription>>();
			this._sessionSubscriptionMappingList = new ConcurrentBag<EventSubscriptionMapping<SessionChangeEventSubscription>>();
			this._devchangeSubscriptionMappingList = new ConcurrentBag<EventSubscriptionMapping<DeviceChangeEventSubscription>>();
			this._pwrEventPbtSubscriptionList = new List<int>();
			this._sessionChangeEventWtsSubscriptionList = new List<int>();
			this._deviceChangeEventSubscriptionList = new List<Guid>();
			svcCtrlHandlerEx = new ServiceControlHandlerDelegate(this.ServiceControlHandlerDelegate);
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002906 File Offset: 0x00000B06
		public override string Version
		{
			get
			{
				return WindowMessageMonitorConstants.Version;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000033 RID: 51 RVA: 0x0000290D File Offset: 0x00000B0D
		public override string Name
		{
			get
			{
				return WindowMessageMonitorConstants.MonitorName;
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002914 File Offset: 0x00000B14
		public override void RegisterSubscribedEvent(SubscribedEvent subscribedEvent)
		{
			string parameter = subscribedEvent.Parameter;
			PowerBroadcastEventSubscription powerBroadcastEventSubscription = null;
			if (!string.IsNullOrEmpty(parameter) && parameter.Contains("PowerBroadcastEventSubscription"))
			{
				powerBroadcastEventSubscription = Serializer.Deserialize<PowerBroadcastEventSubscription>(subscribedEvent.Parameter);
			}
			if (powerBroadcastEventSubscription != null)
			{
				bool flag = false;
				if (powerBroadcastEventSubscription.PbtValueList.Any<int>() && powerBroadcastEventSubscription.PbtValueList.Contains(32787) && powerBroadcastEventSubscription.PowerSettingGuidList != null && powerBroadcastEventSubscription.PowerSettingGuidList.Any<string>())
				{
					foreach (string g in powerBroadcastEventSubscription.PowerSettingGuidList)
					{
						flag = true;
						this.RegisterPowerEventGuidforMonitoring(new Guid(g));
					}
				}
				if (powerBroadcastEventSubscription.PbtValueList != null && powerBroadcastEventSubscription.PbtValueList.Any<int>())
				{
					flag = true;
					foreach (int item in powerBroadcastEventSubscription.PbtValueList)
					{
						if (!this._pwrEventPbtSubscriptionList.Contains(item))
						{
							this._pwrEventPbtSubscriptionList.Add(item);
						}
					}
				}
				if (flag)
				{
					this._pwrSubscriptionMappingList.Add(new EventSubscriptionMapping<PowerBroadcastEventSubscription>(powerBroadcastEventSubscription, subscribedEvent));
				}
			}
			SessionChangeEventSubscription sessionChangeEventSubscription = null;
			if (!string.IsNullOrEmpty(subscribedEvent.Parameter) && subscribedEvent.Parameter.Contains("SessionChangeEventSubscription"))
			{
				sessionChangeEventSubscription = Serializer.Deserialize<SessionChangeEventSubscription>(subscribedEvent.Parameter);
			}
			if (sessionChangeEventSubscription != null && sessionChangeEventSubscription.WtsSessionValueList != null && sessionChangeEventSubscription.WtsSessionValueList.Any<int>())
			{
				foreach (int item2 in sessionChangeEventSubscription.WtsSessionValueList)
				{
					if (!this._sessionChangeEventWtsSubscriptionList.Contains(item2))
					{
						this._sessionChangeEventWtsSubscriptionList.Add(item2);
					}
				}
				this._sessionSubscriptionMappingList.Add(new EventSubscriptionMapping<SessionChangeEventSubscription>(sessionChangeEventSubscription, subscribedEvent));
			}
			DeviceChangeEventSubscription deviceChangeEventSubscription = null;
			if (!string.IsNullOrEmpty(subscribedEvent.Parameter) && subscribedEvent.Parameter.Contains("DeviceChangeEventSubscription"))
			{
				deviceChangeEventSubscription = Serializer.Deserialize<DeviceChangeEventSubscription>(subscribedEvent.Parameter);
			}
			if (deviceChangeEventSubscription != null && deviceChangeEventSubscription.DevInterfaceClassGuidList != null && deviceChangeEventSubscription.DevInterfaceClassGuidList.Any<string>())
			{
				foreach (string g2 in deviceChangeEventSubscription.DevInterfaceClassGuidList)
				{
					Guid guid = new Guid(g2);
					if (!this._deviceChangeEventSubscriptionList.Contains(guid))
					{
						this.RegisterDeviceInterfaceGuidForMonitoring(guid);
						this._deviceChangeEventSubscriptionList.Add(guid);
					}
				}
				this._devchangeSubscriptionMappingList.Add(new EventSubscriptionMapping<DeviceChangeEventSubscription>(deviceChangeEventSubscription, subscribedEvent));
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002B62 File Offset: 0x00000D62
		public override Task<bool> InitializeAsync(EventHandlerReason reason)
		{
			if (!WindowMessageMonitor._systemStartAlreadyHandled)
			{
				Task.Run(async delegate()
				{
					try
					{
						await Task.Delay(TimeSpan.FromSeconds(1.0));
						SystemEventMonitor.SystemEventCallback(10001, IntPtr.Zero, 1U);
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "Exception in WindowMessageMonitor.InitializeAsync");
					}
				});
			}
			return Task.FromResult<bool>(true);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002B98 File Offset: 0x00000D98
		public override void Unregister(EventHandlerReason reason)
		{
			this._eventsUnregistered = true;
			WindowMessageMonitor._systemStartAlreadyHandled = true;
			this._pwrEventPbtSubscriptionList.Clear();
			this._sessionChangeEventWtsSubscriptionList.Clear();
			this._deviceChangeEventSubscriptionList.Clear();
			while (!this._pwrSubscriptionMappingList.IsEmpty)
			{
				EventSubscriptionMapping<PowerBroadcastEventSubscription> eventSubscriptionMapping = null;
				this._pwrSubscriptionMappingList.TryTake(out eventSubscriptionMapping);
			}
			while (!this._sessionSubscriptionMappingList.IsEmpty)
			{
				EventSubscriptionMapping<SessionChangeEventSubscription> eventSubscriptionMapping2 = null;
				this._sessionSubscriptionMappingList.TryTake(out eventSubscriptionMapping2);
			}
			while (!this._devchangeSubscriptionMappingList.IsEmpty)
			{
				EventSubscriptionMapping<DeviceChangeEventSubscription> eventSubscriptionMapping3 = null;
				this._devchangeSubscriptionMappingList.TryTake(out eventSubscriptionMapping3);
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002C30 File Offset: 0x00000E30
		public int ServiceControlHandlerDelegate(int control, int eventType, IntPtr eventData, IntPtr context)
		{
			Logger.Log(Logger.LogSeverity.Information, "ServiceControlHandler: Entry");
			if (this._eventsUnregistered)
			{
				return 0;
			}
			if (control == 13)
			{
				Logger.Log(Logger.LogSeverity.Information, "ServiceControlHandler: WM_POWEREVENT. PBTType={0}", new object[] { eventType });
				this.PowerEventHandler(eventType, eventData);
			}
			if (control == 14)
			{
				Logger.Log(Logger.LogSeverity.Information, "ServiceControlHandler: SERVICE_CONTROL_SESSIONCHANGE. PBTType={0}", new object[] { eventType });
				uint sessionId = this.SessionEventHandler(eventType, eventData);
				SystemEventMonitor.SystemEventCallback(eventType, eventData, sessionId);
				if (eventType == 7)
				{
					AppUsageMonitor.GetInstance().OnWindowsEvent(EventHandlerReason.SessionLock);
				}
				if (eventType == 8)
				{
					AppUsageMonitor.GetInstance().OnWindowsEvent(EventHandlerReason.SessionUnlock);
				}
			}
			if (control == 5)
			{
				SystemEventMonitor.SystemEventCallback(1001, eventData, 1U);
			}
			if (control == 11)
			{
				Logger.Log(Logger.LogSeverity.Information, "ServiceControlHandler: SERVICE_CONTROL_DEVICEEVENT. DbtType={0}", new object[] { eventType });
				this.DeviceChangeHandler(eventType, eventData);
			}
			return 0;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002D00 File Offset: 0x00000F00
		private void PowerEventHandler(int eventType, IntPtr eventData)
		{
			if (this._pwrEventPbtSubscriptionList.Contains(eventType))
			{
				PowerBroadcastEventReaction powerBroadcastEventReaction = new PowerBroadcastEventReaction();
				powerBroadcastEventReaction.PbtValue = eventType;
				if (eventType == 32787)
				{
					WindowMessageMonitorConstants.Win32.POWERBROADCAST_SETTING powerbroadcast_SETTING = WindowMessageMonitor.ConvertUnmanagedPtrtoPwrBroadcastSetting(eventData);
					powerBroadcastEventReaction.PowerSettingGuid = powerbroadcast_SETTING.PowerSetting.ToString();
					powerBroadcastEventReaction.DataLength = powerbroadcast_SETTING.DataLength;
					powerBroadcastEventReaction.Data = powerbroadcast_SETTING.Data;
				}
				string parameter = Serializer.Serialize<PowerBroadcastEventReaction>(powerBroadcastEventReaction);
				EventReaction eventReaction = new EventReaction
				{
					Monitor = WindowMessageMonitorConstants.MonitorName,
					DataType = WindowMessageMonitorConstants.DataType,
					Trigger = WindowMessageMonitorConstants.PowerTrigger,
					Parameter = parameter
				};
				foreach (SubscribedEvent subscribedEvent in this.GetRecepientsForEvent(powerBroadcastEventReaction))
				{
					base.NotifyObservers(eventReaction, subscribedEvent);
				}
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002DE8 File Offset: 0x00000FE8
		private uint SessionEventHandler(int eventType, IntPtr eventData)
		{
			uint result = 0U;
			WindowMessageMonitorConstants.Win32.WTSSESSION_NOTIFICATION wtssession_NOTIFICATION = (WindowMessageMonitorConstants.Win32.WTSSESSION_NOTIFICATION)Marshal.PtrToStructure(eventData, typeof(WindowMessageMonitorConstants.Win32.WTSSESSION_NOTIFICATION));
			result = wtssession_NOTIFICATION.dwSessionId;
			if (this._sessionChangeEventWtsSubscriptionList.Contains(eventType))
			{
				SessionChangeEventReaction sessionChangeEventReaction = new SessionChangeEventReaction();
				sessionChangeEventReaction.WtsSessionValue = eventType;
				sessionChangeEventReaction.SessionId = wtssession_NOTIFICATION.dwSessionId;
				string parameter = Serializer.Serialize<SessionChangeEventReaction>(sessionChangeEventReaction);
				EventReaction eventReaction = new EventReaction
				{
					Monitor = WindowMessageMonitorConstants.MonitorName,
					DataType = WindowMessageMonitorConstants.DataType,
					Trigger = WindowMessageMonitorConstants.SessionTrigger,
					Parameter = parameter
				};
				foreach (SubscribedEvent subscribedEvent in this.GetRecepientsForEvent(sessionChangeEventReaction))
				{
					base.NotifyObservers(eventReaction, subscribedEvent);
				}
			}
			return result;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002EC0 File Offset: 0x000010C0
		private void DeviceChangeHandler(int eventType, IntPtr eventData)
		{
			Guid item = Guid.Empty;
			if (((WindowMessageMonitorConstants.Win32.DEV_BROADCAST_HDR)Marshal.PtrToStructure(eventData, typeof(WindowMessageMonitorConstants.Win32.DEV_BROADCAST_HDR))).dbch_DeviceType == 5U)
			{
				WindowMessageMonitorConstants.Win32.DEV_BROADCAST_DEVICEINTERFACE dev_BROADCAST_DEVICEINTERFACE = (WindowMessageMonitorConstants.Win32.DEV_BROADCAST_DEVICEINTERFACE)Marshal.PtrToStructure(eventData, typeof(WindowMessageMonitorConstants.Win32.DEV_BROADCAST_DEVICEINTERFACE));
				item = dev_BROADCAST_DEVICEINTERFACE.dbcc_classguid;
				if (this._deviceChangeEventSubscriptionList.Contains(item) && !item.Equals(Guid.Empty))
				{
					DeviceChangeEventReaction deviceChangeEventReaction = new DeviceChangeEventReaction();
					deviceChangeEventReaction.DbtEventValue = eventType;
					deviceChangeEventReaction.DevInterfaceClassGuid = item.ToString();
					deviceChangeEventReaction.DbccSize = dev_BROADCAST_DEVICEINTERFACE.dbcc_size;
					byte[] array = new byte[deviceChangeEventReaction.DbccSize];
					if (deviceChangeEventReaction.DbccSize > 0)
					{
						Marshal.Copy(eventData, array, 0, dev_BROADCAST_DEVICEINTERFACE.dbcc_size);
					}
					deviceChangeEventReaction.Data = array;
					string parameter = Serializer.Serialize<DeviceChangeEventReaction>(deviceChangeEventReaction);
					EventReaction eventReaction = new EventReaction
					{
						Monitor = WindowMessageMonitorConstants.MonitorName,
						DataType = WindowMessageMonitorConstants.DataType,
						Trigger = WindowMessageMonitorConstants.DeviceTrigger,
						Parameter = parameter
					};
					foreach (SubscribedEvent subscribedEvent in this.GetRecepientsForEvent(deviceChangeEventReaction))
					{
						base.NotifyObservers(eventReaction, subscribedEvent);
					}
				}
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000300C File Offset: 0x0000120C
		private void RegisterPowerEventGuidforMonitoring(Guid guid)
		{
			try
			{
				WindowMessageMonitorConstants.Win32.RegisterPowerSettingNotification(this._serviceHandle, ref guid, 1);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to Register Power event Guid for monitoring");
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003048 File Offset: 0x00001248
		private IntPtr RegisterDeviceInterfaceGuidForMonitoring(Guid guid)
		{
			IntPtr result = IntPtr.Zero;
			try
			{
				WindowMessageMonitorConstants.Win32.DEV_BROADCAST_DEVICEINTERFACE dev_BROADCAST_DEVICEINTERFACE = default(WindowMessageMonitorConstants.Win32.DEV_BROADCAST_DEVICEINTERFACE);
				dev_BROADCAST_DEVICEINTERFACE.dbcc_size = Marshal.SizeOf(dev_BROADCAST_DEVICEINTERFACE);
				dev_BROADCAST_DEVICEINTERFACE.dbcc_devicetype = 5;
				dev_BROADCAST_DEVICEINTERFACE.dbcc_classguid = guid;
				IntPtr intPtr = 0;
				intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(dev_BROADCAST_DEVICEINTERFACE));
				Marshal.StructureToPtr(dev_BROADCAST_DEVICEINTERFACE, intPtr, true);
				result = WindowMessageMonitorConstants.Win32.RegisterDeviceNotificationW(this._serviceHandle, intPtr, 1U);
				Marshal.FreeHGlobal(intPtr);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to Register Power event Guid for monitoring");
			}
			return result;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000030E0 File Offset: 0x000012E0
		private static WindowMessageMonitorConstants.Win32.POWERBROADCAST_SETTING ConvertUnmanagedPtrtoPwrBroadcastSetting(IntPtr unmanagedPtr)
		{
			WindowMessageMonitorConstants.Win32.POWERBROADCAST_SETTING result = default(WindowMessageMonitorConstants.Win32.POWERBROADCAST_SETTING);
			WindowMessageMonitorConstants.Win32.POWERBROADCAST_SETTING_PARTIAL powerbroadcast_SETTING_PARTIAL = (WindowMessageMonitorConstants.Win32.POWERBROADCAST_SETTING_PARTIAL)Marshal.PtrToStructure(unmanagedPtr, typeof(WindowMessageMonitorConstants.Win32.POWERBROADCAST_SETTING_PARTIAL));
			result.DataLength = powerbroadcast_SETTING_PARTIAL.DataLength;
			result.PowerSetting = powerbroadcast_SETTING_PARTIAL.PowerSetting;
			byte[] array = new byte[powerbroadcast_SETTING_PARTIAL.DataLength + Marshal.SizeOf(powerbroadcast_SETTING_PARTIAL)];
			Marshal.Copy(unmanagedPtr, array, 0, powerbroadcast_SETTING_PARTIAL.DataLength + Marshal.SizeOf(powerbroadcast_SETTING_PARTIAL));
			byte[] array2 = new byte[powerbroadcast_SETTING_PARTIAL.DataLength];
			Array.Copy(array, Marshal.SizeOf(powerbroadcast_SETTING_PARTIAL), array2, 0, powerbroadcast_SETTING_PARTIAL.DataLength);
			result.Data = array2;
			return result;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003188 File Offset: 0x00001388
		private IEnumerable<SubscribedEvent> GetRecepientsForEvent(PowerBroadcastEventReaction pwrEvent)
		{
			List<SubscribedEvent> list = new List<SubscribedEvent>();
			foreach (EventSubscriptionMapping<PowerBroadcastEventSubscription> eventSubscriptionMapping in this._pwrSubscriptionMappingList)
			{
				if (eventSubscriptionMapping.EventSubscriptionData.PbtValueList.Contains(pwrEvent.PbtValue))
				{
					if (!string.IsNullOrEmpty(pwrEvent.PowerSettingGuid))
					{
						if (eventSubscriptionMapping.EventSubscriptionData.PowerSettingGuidList.Contains(pwrEvent.PowerSettingGuid, StringComparer.OrdinalIgnoreCase))
						{
							list.Add(eventSubscriptionMapping.SubscribedEvent);
						}
					}
					else
					{
						list.Add(eventSubscriptionMapping.SubscribedEvent);
					}
				}
			}
			return list;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003234 File Offset: 0x00001434
		private IEnumerable<SubscribedEvent> GetRecepientsForEvent(SessionChangeEventReaction sessionEvent)
		{
			List<SubscribedEvent> list = new List<SubscribedEvent>();
			foreach (EventSubscriptionMapping<SessionChangeEventSubscription> eventSubscriptionMapping in this._sessionSubscriptionMappingList)
			{
				if (eventSubscriptionMapping.EventSubscriptionData.WtsSessionValueList.Contains(sessionEvent.WtsSessionValue))
				{
					list.Add(eventSubscriptionMapping.SubscribedEvent);
				}
			}
			return list;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000032A8 File Offset: 0x000014A8
		private IEnumerable<SubscribedEvent> GetRecepientsForEvent(DeviceChangeEventReaction devchangeEvent)
		{
			List<SubscribedEvent> list = new List<SubscribedEvent>();
			foreach (EventSubscriptionMapping<DeviceChangeEventSubscription> eventSubscriptionMapping in this._devchangeSubscriptionMappingList)
			{
				if (!string.IsNullOrEmpty(devchangeEvent.DevInterfaceClassGuid) && eventSubscriptionMapping.EventSubscriptionData.DevInterfaceClassGuidList.Contains(devchangeEvent.DevInterfaceClassGuid, StringComparer.OrdinalIgnoreCase) && !devchangeEvent.DevInterfaceClassGuid.Equals(Guid.Empty))
				{
					list.Add(eventSubscriptionMapping.SubscribedEvent);
				}
			}
			return list;
		}

		// Token: 0x0400001C RID: 28
		private readonly IntPtr _serviceHandle;

		// Token: 0x0400001D RID: 29
		private ConcurrentBag<EventSubscriptionMapping<PowerBroadcastEventSubscription>> _pwrSubscriptionMappingList;

		// Token: 0x0400001E RID: 30
		private ConcurrentBag<EventSubscriptionMapping<SessionChangeEventSubscription>> _sessionSubscriptionMappingList;

		// Token: 0x0400001F RID: 31
		private ConcurrentBag<EventSubscriptionMapping<DeviceChangeEventSubscription>> _devchangeSubscriptionMappingList;

		// Token: 0x04000020 RID: 32
		private readonly List<int> _pwrEventPbtSubscriptionList;

		// Token: 0x04000021 RID: 33
		private readonly List<int> _sessionChangeEventWtsSubscriptionList;

		// Token: 0x04000022 RID: 34
		private readonly List<Guid> _deviceChangeEventSubscriptionList;

		// Token: 0x04000023 RID: 35
		private bool _eventsUnregistered;

		// Token: 0x04000024 RID: 36
		private static bool _systemStartAlreadyHandled;
	}
}
