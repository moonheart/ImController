using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Events.AppMonitor;
using Lenovo.Modern.CoreTypes.Models.Subscription;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.PluginManager.Services;
using Lenovo.Modern.ImController.Shared;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Services.Contracts.SystemInformation.AppTag;
using Lenovo.Modern.Utilities.Services.Logging;
using Microsoft.Win32;

namespace Lenovo.Modern.ImController.EventManager.Services
{
	// Token: 0x02000007 RID: 7
	public class EventManager
	{
		// Token: 0x0600000B RID: 11 RVA: 0x0000218C File Offset: 0x0000038C
		public EventManager(ISubscriptionManager subscriptionManager, IMachineInformationManager machineInformationManager, IAppAndTagManager appTagManager, IEnumerable<IEventMonitor> eventMonitors, IPluginManager pluginManager, IEventPrioritizer eventPrioritizer)
		{
			this._subscriptionManager = subscriptionManager;
			this._machineInformationManager = machineInformationManager;
			this._appTagManager = appTagManager;
			this._listOfEventMonitors = eventMonitors;
			this._pluginManager = pluginManager;
			this._pluginRepository = new PluginRepository();
			this._eventReactionSender = new EventReactionSender(this._pluginManager, this._pluginRepository);
			this._eventPrioritizer = eventPrioritizer;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021FC File Offset: 0x000003FC
		public async Task<bool> InitializeAsync(CancellationToken cancelToken, ManualResetEventSlim firstContractBrokerRequestEvent, EventHandlerReason reason)
		{
			Logger.Log(Logger.LogSeverity.Information, "EventManager: Started");
			bool success = false;
			PackageSubscription subscription = await this._subscriptionManager.GetSubscriptionAsync(cancelToken);
			this._subscription = subscription;
			if (this._subscription != null && this._subscription.PackageList != null && this._subscription.PackageList.Any<Lenovo.Modern.ImController.Shared.Model.Packages.Package>())
			{
				this._pluginSettingsManager = new PluginSettingsAgent(this._subscription);
				IEnumerable<Lenovo.Modern.ImController.Shared.Model.Packages.Package> applicablePackages = (from package in this._subscription.PackageList
					where SubscribedPackageManager.IsPackageApplicable(this._subscription, package, cancelToken)
					select package).ToList<Lenovo.Modern.ImController.Shared.Model.Packages.Package>();
				using (IEnumerator<IEventMonitor> enumerator = this._listOfEventMonitors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventManager.<>c__DisplayClass18_1 CS$<>8__locals2 = new EventManager.<>c__DisplayClass18_1();
						CS$<>8__locals2.eventMonitor = enumerator.Current;
						if (CS$<>8__locals2.eventMonitor != null)
						{
							try
							{
								await CS$<>8__locals2.eventMonitor.InitializeAsync(reason);
								IEnumerable<SubscribedEvent> enumerable = (from se in (from p in applicablePackages
										where p.SubscribedEventList != null
										select p).SelectMany((Lenovo.Modern.ImController.Shared.Model.Packages.Package p) => p.SubscribedEventList)
									where se != null && se.Monitor != null && se.Monitor.Equals(CS$<>8__locals2.eventMonitor.Name, StringComparison.OrdinalIgnoreCase)
									select se).ToList<SubscribedEvent>();
								if (enumerable != null && enumerable.Any<SubscribedEvent>())
								{
									CS$<>8__locals2.eventMonitor.EventGenerator += this.EventHandler;
									foreach (SubscribedEvent subscribedEvent in enumerable)
									{
										try
										{
											CS$<>8__locals2.eventMonitor.RegisterSubscribedEvent(subscribedEvent);
										}
										catch (Exception ex)
										{
											Logger.Log(ex, "EventManager: Unable to subscribe event monitor {0} to an event", new object[] { CS$<>8__locals2.eventMonitor.Name });
										}
									}
								}
							}
							catch (Exception ex2)
							{
								Logger.Log(ex2, "EventManager: Unable to initialize event monitor name: {0}", new object[] { CS$<>8__locals2.eventMonitor.Name });
							}
						}
						if (cancelToken.IsCancellationRequested)
						{
							break;
						}
						CS$<>8__locals2 = null;
					}
				}
				IEnumerator<IEventMonitor> enumerator = null;
				this.SubscribeProtocolEvents(applicablePackages);
				if (!cancelToken.IsCancellationRequested)
				{
					this._normalEventHandlerThread = new Thread(delegate()
					{
						this.EventHandlerThread(cancelToken, firstContractBrokerRequestEvent, EventManager._eventQueueNormal, this._startupEventDelayMS);
					});
					this._normalEventHandlerThread.Start();
					this._immediateEventHandlerThread = new Thread(delegate()
					{
						this.EventHandlerThread(cancelToken, firstContractBrokerRequestEvent, EventManager._eventQueueImmediate, 0);
					});
					this._immediateEventHandlerThread.Start();
					success = true;
				}
				applicablePackages = null;
			}
			return success;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000225C File Offset: 0x0000045C
		public void Stop(EventHandlerReason reason)
		{
			new List<Task>();
			foreach (IEventMonitor eventMonitor in this._listOfEventMonitors)
			{
				try
				{
					Logger.Log(Logger.LogSeverity.Information, "EventManager: Stopping event monitor with name: '{0}' reason:{1}", new object[]
					{
						(eventMonitor != null) ? eventMonitor.Name : "NULL",
						reason.ToString()
					});
					if (eventMonitor != null)
					{
						eventMonitor.Unregister(reason);
					}
					Logger.Log(Logger.LogSeverity.Information, "EventManager: Event monitor was stopped: '{0}'", new object[] { (eventMonitor != null) ? eventMonitor.Name : "NULL" });
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "EventManager: Unable to stop event monitor with name: '{0}'", new object[] { (eventMonitor != null) ? eventMonitor.Name : "NULL" });
				}
			}
			Logger.Log(Logger.LogSeverity.Information, "EventManager: Stopped all event Monitors");
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002350 File Offset: 0x00000550
		private void EventHandler(EventReaction eventReaction, SubscribedEvent subscribedEvent)
		{
			if (this._stopping)
			{
				return;
			}
			bool flag = true;
			bool flag2 = false;
			if (this._pluginSettingsManager != null)
			{
				PluginSettingsAgent.Setting prioritizedSetting = this._pluginSettingsManager.GetPrioritizedSetting(subscribedEvent.Plugin, "ImController.Privilege.ReceiveEvents", PluginSettingsAgent.SettingLocation.SetDynamically);
				if (prioritizedSetting == null || string.IsNullOrWhiteSpace(prioritizedSetting.ValueAsString))
				{
					flag2 = true;
					flag = true;
				}
				else
				{
					int? valueAsInt = prioritizedSetting.ValueAsInt;
					int num = 1;
					flag2 = (valueAsInt.GetValueOrDefault() == num) & (valueAsInt != null);
					flag = false;
				}
			}
			if (flag2)
			{
				if (eventReaction.Monitor == AppMonitorEventConstants.Get.AppMonitorEventMonitorName)
				{
					Logger.Log(Logger.LogSeverity.Information, "EventManager: Received event for AppMonitor. Not checking UAP install state but forwarding the event");
				}
				else
				{
					flag2 = SubscribedPackageManager.ValidateDeviceUapAssociation(this._subscription, subscribedEvent.Plugin, false);
				}
			}
			Logger.Log(Logger.LogSeverity.Information, "EventManager: Received event for '{0}'.  Will forward? '{1}'. IsLegacy? '{2}'.", new object[] { subscribedEvent.Plugin, flag2, flag });
			try
			{
				if (flag2)
				{
					if (!flag)
					{
						EventManager._eventQueueImmediate.Add(new Tuple<EventReaction, SubscribedEvent>(eventReaction, subscribedEvent));
						Logger.Log(Logger.LogSeverity.Information, "EventManager: Non-Legacy Event from monitor '{0}' was enqueued with immediate priority: '{1}'", new object[] { eventReaction.Monitor, eventReaction.Parameter });
					}
					else
					{
						PluginPrivilegeReader pluginPrivilegeReader = new PluginPrivilegeReader(this._subscription);
						EventPriority eventPrioprity = this._eventPrioritizer.GetEventPrioprity(eventReaction, pluginPrivilegeReader.GetPluginPrivileges(subscribedEvent.Plugin));
						if (eventPrioprity != EventPriority.PriorityNormal)
						{
							if (eventPrioprity == EventPriority.PriorityImmediate)
							{
								EventManager._eventQueueImmediate.Add(new Tuple<EventReaction, SubscribedEvent>(eventReaction, subscribedEvent));
								Logger.Log(Logger.LogSeverity.Information, "EventManager: Legacy Event from monitor '{0}' was enqueued with immediate priority: '{1}'", new object[] { eventReaction.Monitor, eventReaction.Parameter });
							}
						}
						else
						{
							EventManager._eventQueueNormal.Add(new Tuple<EventReaction, SubscribedEvent>(eventReaction, subscribedEvent));
							Logger.Log(Logger.LogSeverity.Information, "EventManager: Legacy Event from monitor '{0}' was enqueued with normal priority: '{1}'", new object[] { eventReaction.Monitor, eventReaction.Parameter });
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "EventManager: Exception while enqueing event");
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002524 File Offset: 0x00000724
		private void EventHandlerThread(CancellationToken cancelToken, ManualResetEventSlim firstContractBrokerRequestEvent, BlockingCollection<Tuple<EventReaction, SubscribedEvent>> eventQueue, int startDelayMs)
		{
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "EventManager: EventHandlerThread started and is delaying events for {0} ms", new object[] { startDelayMs });
				Logger.Log(Logger.LogSeverity.Information, firstContractBrokerRequestEvent.Wait(startDelayMs, cancelToken) ? "EventManager: EventHandlerThread: A contract request was received. Stop waiting and start handling events" : "EventManager: EventHandlerThread: Initial delay has ended and event handling will start now");
				while (!cancelToken.IsCancellationRequested)
				{
					Tuple<EventReaction, SubscribedEvent> e = eventQueue.Take(cancelToken);
					if (e != null && !cancelToken.IsCancellationRequested)
					{
						Logger.Log(Logger.LogSeverity.Information, "EventManager: Event from monitor {0} was dequeued. Plugin name: {1}. Trigger: {2}", new object[]
						{
							e.Item1.Monitor,
							e.Item2.Plugin,
							e.Item1.Trigger
						});
						Task.Run(delegate()
						{
							try
							{
								this._eventReactionSender.SendEventReaction(e.Item1, e.Item2, cancelToken);
							}
							catch (Exception ex2)
							{
								Logger.Log(ex2, "EventManager: Exception while handing event");
							}
						});
					}
					else
					{
						Logger.Log(Logger.LogSeverity.Warning, "Event was dequeued, but it was null and was discarded.");
					}
				}
			}
			catch (OperationCanceledException)
			{
				Logger.Log(Logger.LogSeverity.Information, "EventManager: EventHandlerThread was canceled");
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "EventManager: Exception in EventHandlerThread");
			}
			Logger.Log(Logger.LogSeverity.Information, "EventManager: EventHandlerThread exited");
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002684 File Offset: 0x00000884
		private void SubscribeProtocolEvents(IEnumerable<Lenovo.Modern.ImController.Shared.Model.Packages.Package> applicablePackages)
		{
			try
			{
				applicablePackages.Where(delegate(Lenovo.Modern.ImController.Shared.Model.Packages.Package x)
				{
					if (x != null && x.PackageInformation != null && !string.IsNullOrWhiteSpace(x.PackageInformation.Name) && x.SettingList != null)
					{
						AppSetting appSetting = x.SettingList.FirstOrDefault((AppSetting s) => !string.IsNullOrWhiteSpace(s.Key) && s.Key.Equals("ImController.Privilege.Protocol") && !string.IsNullOrWhiteSpace(s.Value));
						if (appSetting != null)
						{
							try
							{
								string value = appSetting.Value;
								Logger.Log(Logger.LogSeverity.Information, "SubscribeProtocolEvents: Registering {0} Protocol ", new object[] { value });
								RegistryKey registryKey = Registry.ClassesRoot.CreateSubKey(value + "\\" + Constants.ProtocolRegistrationKey);
								string[] array = new string[5];
								array[0] = "\"";
								int num = 1;
								Assembly entryAssembly = Assembly.GetEntryAssembly();
								array[num] = ((entryAssembly != null) ? entryAssembly.Location : null);
								array[2] = "\" /protocolEventPackage ";
								array[3] = x.PackageInformation.Name;
								array[4] = " /protocolEventValue \"%1\"";
								string value2 = string.Concat(array);
								if (registryKey != null)
								{
									registryKey.SetValue("", value2);
								}
								registryKey = Registry.ClassesRoot.CreateSubKey(value);
								if (registryKey != null)
								{
									registryKey.SetValue("URL Protocol", "");
									registryKey.SetValue("", "URL:" + value + " Protocol");
								}
								registryKey = Registry.ClassesRoot.CreateSubKey(value + "\\DefaultIcon");
								if (registryKey != null)
								{
									RegistryKey registryKey2 = registryKey;
									string name = "";
									string str = "\"";
									Assembly entryAssembly2 = Assembly.GetEntryAssembly();
									registryKey2.SetValue(name, str + ((entryAssembly2 != null) ? entryAssembly2.Location : null) + "\"");
								}
							}
							catch (Exception ex2)
							{
								Logger.Log(ex2, "SubscribeProtocolEvents: Exception occured");
							}
						}
						return true;
					}
					return false;
				}).Count<Lenovo.Modern.ImController.Shared.Model.Packages.Package>();
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception occured in SubscribeProtocolEvents");
			}
		}

		// Token: 0x04000005 RID: 5
		private PackageSubscription _subscription;

		// Token: 0x04000006 RID: 6
		private static BlockingCollection<Tuple<EventReaction, SubscribedEvent>> _eventQueueNormal = new BlockingCollection<Tuple<EventReaction, SubscribedEvent>>();

		// Token: 0x04000007 RID: 7
		private static BlockingCollection<Tuple<EventReaction, SubscribedEvent>> _eventQueueImmediate = new BlockingCollection<Tuple<EventReaction, SubscribedEvent>>();

		// Token: 0x04000008 RID: 8
		private readonly int _startupEventDelayMS = 180000;

		// Token: 0x04000009 RID: 9
		private readonly ISubscriptionManager _subscriptionManager;

		// Token: 0x0400000A RID: 10
		private readonly IMachineInformationManager _machineInformationManager;

		// Token: 0x0400000B RID: 11
		private readonly IAppAndTagManager _appTagManager;

		// Token: 0x0400000C RID: 12
		private readonly EventReactionSender _eventReactionSender;

		// Token: 0x0400000D RID: 13
		private readonly IEnumerable<IEventMonitor> _listOfEventMonitors;

		// Token: 0x0400000E RID: 14
		private readonly IPluginManager _pluginManager;

		// Token: 0x0400000F RID: 15
		private static DateTime _lastBootOrResumeTime = DateTime.Now;

		// Token: 0x04000010 RID: 16
		private bool _stopping;

		// Token: 0x04000011 RID: 17
		private PluginRepository _pluginRepository;

		// Token: 0x04000012 RID: 18
		private IEventPrioritizer _eventPrioritizer;

		// Token: 0x04000013 RID: 19
		private Thread _normalEventHandlerThread;

		// Token: 0x04000014 RID: 20
		private Thread _immediateEventHandlerThread;

		// Token: 0x04000015 RID: 21
		private PluginSettingsAgent _pluginSettingsManager;
	}
}
