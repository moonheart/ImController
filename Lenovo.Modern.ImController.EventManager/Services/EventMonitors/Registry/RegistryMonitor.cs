using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Events.Registry;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.SystemContext.Shared;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.Registry
{
	// Token: 0x02000014 RID: 20
	internal class RegistryMonitor : EventMonitorBase
	{
		// Token: 0x0600005A RID: 90 RVA: 0x00004974 File Offset: 0x00002B74
		public RegistryMonitor()
		{
			this._subscriptionMappingList = new ConcurrentBag<EventSubscriptionMapping<RegistryEventSubscription>>();
			this._monitorTaskList = new ConcurrentBag<Task>();
			this._hkcumonitorTaskList = new ConcurrentBag<Task>();
			this._eventTerminate = new ManualResetEvent(false);
			this._eventHkcuTerminate = new ManualResetEvent(false);
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000049C0 File Offset: 0x00002BC0
		public override string Version
		{
			get
			{
				return RegistryMonitorConstants.Version;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000049C7 File Offset: 0x00002BC7
		public override string Name
		{
			get
			{
				return RegistryMonitorConstants.MonitorName;
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000049D0 File Offset: 0x00002BD0
		public override void RegisterSubscribedEvent(SubscribedEvent subscribedEvent)
		{
			this._eventHkcuTerminate.Reset();
			this._eventTerminate.Reset();
			string parameter = subscribedEvent.Parameter;
			RegistryEventSubscription subscriptionData = null;
			if (!string.IsNullOrEmpty(parameter) && parameter.Contains("RegistryEventSubscription"))
			{
				subscriptionData = Serializer.Deserialize<RegistryEventSubscription>(subscribedEvent.Parameter);
			}
			if (subscriptionData != null)
			{
				this._subscriptionMappingList.Add(new EventSubscriptionMapping<RegistryEventSubscription>(subscriptionData, subscribedEvent));
				Task item = Task.Run(delegate()
				{
					try
					{
						this.StartRegistryMonitorAsync(subscriptionData);
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "Exception in StartRegistryMonitorAsync");
					}
				});
				if ("HKEY_CURRENT_USER" == subscriptionData.RegistryHiveName)
				{
					this._hkcumonitorTaskList.Add(item);
					return;
				}
				this._monitorTaskList.Add(item);
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000344B File Offset: 0x0000164B
		public override Task<bool> InitializeAsync(EventHandlerReason reason)
		{
			return Task.FromResult<bool>(true);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004A98 File Offset: 0x00002C98
		public override void Unregister(EventHandlerReason reason)
		{
			this._eventTerminate.Set();
			this._eventHkcuTerminate.Set();
			try
			{
				Task.WaitAll(this._monitorTaskList.ToArray());
				Task.WaitAll(this._hkcumonitorTaskList.ToArray());
				goto IL_56;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "RegistryMonitor: Exception Waiting for all tasks");
				goto IL_56;
			}
			IL_46:
			Task task = null;
			this._monitorTaskList.TryTake(out task);
			IL_56:
			if (this._monitorTaskList.IsEmpty)
			{
				while (!this._hkcumonitorTaskList.IsEmpty)
				{
					Task task2 = null;
					this._hkcumonitorTaskList.TryTake(out task2);
				}
				while (!this._subscriptionMappingList.IsEmpty)
				{
					EventSubscriptionMapping<RegistryEventSubscription> eventSubscriptionMapping = null;
					this._subscriptionMappingList.TryTake(out eventSubscriptionMapping);
				}
				return;
			}
			goto IL_46;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004B58 File Offset: 0x00002D58
		private void StartRegistryMonitorAsync(RegistryEventSubscription regData)
		{
			Logger.Log(Logger.LogSeverity.Information, "StartRegistryMonitorAsync: Started for regkey " + regData.KeyPath + "//" + regData.ValueName);
			WaitHandle[] array = new WaitHandle[2];
			if (!RegistryMonitor.RootKeyDictionary.ContainsKey(regData.RegistryHiveName))
			{
				return;
			}
			IntPtr hKey = RegistryMonitor.RootKeyDictionary[regData.RegistryHiveName];
			string text = regData.KeyPath;
			if ("HKEY_CURRENT_USER" == regData.RegistryHiveName)
			{
				hKey = RegistryMonitor.RootKeyDictionary["HKEY_USERS"];
				text = UserInformationProvider.GetLoggedInUserSID() + "\\" + regData.KeyPath;
			}
			uint num = 0U;
			IntPtr intPtr;
			int num2;
			if (regData.MonitorRegTree)
			{
				num2 = Win32.RegOpenKeyEx(hKey, text, 0U, 983615, out intPtr);
			}
			else
			{
				num2 = Win32.RegCreateKeyEx(hKey, text, 0, null, 0U, 983615, IntPtr.Zero, out intPtr, out num);
			}
			if (num2 == 0)
			{
				try
				{
					AutoResetEvent autoResetEvent = new AutoResetEvent(false);
					array[0] = autoResetEvent;
					array[1] = (("HKEY_CURRENT_USER" == regData.RegistryHiveName) ? this._eventHkcuTerminate : this._eventTerminate);
					Action <>9__0;
					while (!array[1].WaitOne(0, true))
					{
						IContainerValue containerValue = null;
						Win32.REG_NOTIFY_CHANGE notifyFilter;
						if (regData.MonitorRegTree)
						{
							notifyFilter = Win32.REG_NOTIFY_CHANGE.NAME | Win32.REG_NOTIFY_CHANGE.LAST_SET;
						}
						else if (string.IsNullOrEmpty(regData.ValueName))
						{
							notifyFilter = Win32.REG_NOTIFY_CHANGE.NAME;
						}
						else
						{
							IContainer container = new RegistrySystem().LoadContainer((("HKEY_CURRENT_USER" == regData.RegistryHiveName) ? "HKEY_USERS" : regData.RegistryHiveName) + "\\" + text);
							if (container != null)
							{
								containerValue = container.GetValue(regData.ValueName);
							}
							notifyFilter = Win32.REG_NOTIFY_CHANGE.LAST_SET;
						}
						int num3 = Win32.RegNotifyChangeKeyValue(intPtr, regData.MonitorRegTree, notifyFilter, autoResetEvent.SafeWaitHandle.DangerousGetHandle(), true);
						if (num3 == 0)
						{
							if (WaitHandle.WaitAny(array) == 0)
							{
								Logger.Log(Logger.LogSeverity.Information, "regNotify event triggered");
								bool flag = false;
								if (!string.IsNullOrEmpty(regData.ValueName) && !regData.MonitorRegTree)
								{
									IContainer container2 = new RegistrySystem().LoadContainer((("HKEY_CURRENT_USER" == regData.RegistryHiveName) ? "HKEY_USERS" : regData.RegistryHiveName) + "\\" + text);
									IContainerValue containerValue2 = null;
									if (container2 != null)
									{
										containerValue2 = container2.GetValue(regData.ValueName);
									}
									if (containerValue != null && containerValue2 == null)
									{
										flag = true;
									}
									if (containerValue == null && containerValue2 != null)
									{
										flag = true;
									}
									if (containerValue != null && containerValue2 != null && containerValue2.GetValueAsString() != containerValue.GetValueAsString())
									{
										flag = true;
									}
								}
								else
								{
									flag = true;
								}
								if (!flag)
								{
									continue;
								}
								RegistryEventReaction registryEventReaction = new RegistryEventReaction();
								registryEventReaction.RegistryHiveName = regData.RegistryHiveName;
								registryEventReaction.KeyPath = regData.KeyPath;
								registryEventReaction.ValueName = regData.ValueName;
								registryEventReaction.MonitorRegTree = regData.MonitorRegTree;
								string parameter = Serializer.Serialize<RegistryEventReaction>(registryEventReaction);
								EventReaction eventReaction = new EventReaction
								{
									Monitor = RegistryMonitorConstants.MonitorName,
									DataType = RegistryMonitorConstants.DataType,
									Trigger = RegistryMonitorConstants.Trigger,
									Parameter = parameter
								};
								if (regData.MonitorRegTree)
								{
									Task.Delay(1000).Wait();
								}
								using (IEnumerator<SubscribedEvent> enumerator = this.GetRecepientsForEvent(registryEventReaction).GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										SubscribedEvent subscribedEvent = enumerator.Current;
										base.NotifyObservers(eventReaction, subscribedEvent);
									}
									continue;
								}
							}
							Logger.Log(Logger.LogSeverity.Information, "regNotify monitor asked to stop");
						}
						else
						{
							if (num3 == 1018)
							{
								Action action;
								if ((action = <>9__0) == null)
								{
									action = (<>9__0 = delegate()
									{
										try
										{
											this.StartRegistryMonitorAsync(regData);
										}
										catch (Exception ex)
										{
											Logger.Log(ex, "Exception in StartRegistryMonitorAsync");
										}
									});
								}
								Task.Run(action);
								break;
							}
							this._eventTerminate.WaitOne(5000);
						}
					}
					Logger.Log(Logger.LogSeverity.Information, "regNotify monitoring closed");
				}
				catch (Exception)
				{
					Logger.Log(Logger.LogSeverity.Information, "Exception occured while monitoring registry");
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						Win32.RegCloseKey(intPtr);
					}
				}
			}
			Logger.Log(Logger.LogSeverity.Information, "StartRegistryMonitorAsync: Exit");
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00005000 File Offset: 0x00003200
		private IEnumerable<SubscribedEvent> GetRecepientsForEvent(RegistryEventReaction registryEvent)
		{
			List<SubscribedEvent> list = new List<SubscribedEvent>();
			foreach (EventSubscriptionMapping<RegistryEventSubscription> eventSubscriptionMapping in this._subscriptionMappingList)
			{
				if (registryEvent.RegistryHiveName == eventSubscriptionMapping.EventSubscriptionData.RegistryHiveName && registryEvent.KeyPath == eventSubscriptionMapping.EventSubscriptionData.KeyPath)
				{
					if (!string.IsNullOrEmpty(registryEvent.ValueName))
					{
						if (registryEvent.ValueName == eventSubscriptionMapping.EventSubscriptionData.ValueName)
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

		// Token: 0x0400003A RID: 58
		private readonly ManualResetEvent _eventTerminate;

		// Token: 0x0400003B RID: 59
		private readonly ManualResetEvent _eventHkcuTerminate;

		// Token: 0x0400003C RID: 60
		private readonly ConcurrentBag<EventSubscriptionMapping<RegistryEventSubscription>> _subscriptionMappingList;

		// Token: 0x0400003D RID: 61
		private readonly ConcurrentBag<Task> _monitorTaskList;

		// Token: 0x0400003E RID: 62
		private readonly ConcurrentBag<Task> _hkcumonitorTaskList;

		// Token: 0x0400003F RID: 63
		private static readonly IReadOnlyDictionary<string, IntPtr> RootKeyDictionary = new Dictionary<string, IntPtr>
		{
			{
				"HKEY_CLASSES_ROOT",
				Win32.HKEY_CLASSES_ROOT
			},
			{
				"HKEY_CURRENT_USER",
				Win32.HKEY_CURRENT_USER
			},
			{
				"HKEY_LOCAL_MACHINE",
				Win32.HKEY_LOCAL_MACHINE
			},
			{
				"HKEY_USERS",
				Win32.HKEY_USERS
			},
			{
				"HKEY_CURRENT_CONFIG",
				Win32.HKEY_CURRENT_CONFIG
			}
		};
	}
}
