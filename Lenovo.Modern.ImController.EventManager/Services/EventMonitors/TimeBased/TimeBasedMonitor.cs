using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;
using Lenovo.Modern.CoreTypes.Events.TimeBased;
using Lenovo.Modern.ImController.EventManager.Services.EventMonitors.Registry;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.TimeBased
{
	// Token: 0x02000011 RID: 17
	internal class TimeBasedMonitor : EventMonitorBase
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00003384 File Offset: 0x00001584
		public TimeBasedMonitor(IMachineInformationManager machineInformationAgent)
		{
			if (machineInformationAgent == null)
			{
				throw new ArgumentNullException("machineInformationAgent");
			}
			this._macheininformationManager = machineInformationAgent;
			this._eventTerminate = new ManualResetEvent(false);
			this._monitorTaskList = new ConcurrentBag<System.Threading.Tasks.Task>();
			this._listOfTimebasedEvents = new ConcurrentBag<SubscribedEvent>();
			this._cleanupTaskSemaphore = new SemaphoreSlim(1);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000044 RID: 68 RVA: 0x000033DA File Offset: 0x000015DA
		public override string Version
		{
			get
			{
				return TimeBasedEventConstants.Get.TimeBasedEventVersion;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000033E6 File Offset: 0x000015E6
		public override string Name
		{
			get
			{
				return TimeBasedEventConstants.Get.TimeBasedEventMonitorName;
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000033F4 File Offset: 0x000015F4
		public override void RegisterSubscribedEvent(SubscribedEvent subscribedEvent)
		{
			if (!this._listOfTimebasedEvents.Contains(subscribedEvent))
			{
				this._listOfTimebasedEvents.Add(subscribedEvent);
			}
			System.Threading.Tasks.Task.Run(delegate()
			{
				try
				{
					System.Threading.Tasks.Task.Delay(200000).Wait();
					this.DeleteUnusedSchTasks();
					if (this._machineInformation == null)
					{
						Task<MachineInformation> machineInformationAsync = this._macheininformationManager.GetMachineInformationAsync(CancellationToken.None);
						machineInformationAsync.Wait();
						this._machineInformation = machineInformationAsync.Result;
					}
					string parameter = subscribedEvent.Parameter;
					TimeBasedEventSubscription timeBasedEventSubscription = null;
					if (!string.IsNullOrEmpty(parameter) && parameter.Contains("TimeBasedEventSubscription"))
					{
						timeBasedEventSubscription = Serializer.Deserialize<TimeBasedEventSubscription>(subscribedEvent.Parameter);
					}
					if (timeBasedEventSubscription != null)
					{
						try
						{
							this.SubscribeToTimeBasedEvent(timeBasedEventSubscription, subscribedEvent, this._machineInformation);
						}
						catch (Exception ex)
						{
							Logger.Log(ex, "TimeBasedMonitor: TimeBase.RegisterSubscribedEvent: Exception while SubscribeToTimeBasedEvent");
						}
					}
				}
				catch (Exception ex2)
				{
					Logger.Log(ex2, "TimeBasedMonitor: Unable to register subscribed event");
				}
			});
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000344B File Offset: 0x0000164B
		public override Task<bool> InitializeAsync(EventHandlerReason reason)
		{
			return System.Threading.Tasks.Task.FromResult<bool>(true);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003454 File Offset: 0x00001654
		public override void Unregister(EventHandlerReason reason)
		{
			this._listOfTimebasedEvents = new ConcurrentBag<SubscribedEvent>();
			this._eventTerminate.Set();
			while (!this._monitorTaskList.IsEmpty)
			{
				System.Threading.Tasks.Task task = null;
				this._monitorTaskList.TryTake(out task);
				task.Wait();
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000034A0 File Offset: 0x000016A0
		private void DeleteUnusedSchTasks()
		{
			this._cleanupTaskSemaphore.Wait();
			if (!this._isCleanupDone)
			{
				this._isCleanupDone = true;
				using (TaskService taskService = new TaskService())
				{
					try
					{
						if (taskService.RootFolder.SubFolders.Exists("Lenovo") && taskService.RootFolder.SubFolders["Lenovo"].SubFolders.Exists("ImController") && taskService.RootFolder.SubFolders["Lenovo"].SubFolders["ImController"].SubFolders.Exists("TimeBasedEvents") && taskService.RootFolder.SubFolders["Lenovo"].SubFolders["ImController"].SubFolders["TimeBasedEvents"].Tasks != null)
						{
							List<Microsoft.Win32.TaskScheduler.Task> list = new List<Microsoft.Win32.TaskScheduler.Task>();
							foreach (Microsoft.Win32.TaskScheduler.Task task in taskService.RootFolder.SubFolders["Lenovo"].SubFolders["ImController"].SubFolders["TimeBasedEvents"].Tasks)
							{
								bool flag = false;
								string text = task.Definition.RegistrationInfo.Description.Replace("\r", "");
								text = text.Replace("\n", "");
								foreach (SubscribedEvent instance in this._listOfTimebasedEvents)
								{
									string text2 = (Serializer.Serialize<SubscribedEvent>(instance) + this.GetVersionTag()).Replace("\r", "");
									text2 = text2.Replace("\n", "");
									if (text == text2)
									{
										flag = true;
									}
								}
								if (!flag)
								{
									list.Add(task);
								}
							}
							foreach (Microsoft.Win32.TaskScheduler.Task task2 in list)
							{
								taskService.RootFolder.SubFolders["Lenovo"].SubFolders["ImController"].SubFolders["TimeBasedEvents"].DeleteTask(task2.Name, false);
							}
						}
					}
					catch (Exception)
					{
					}
				}
			}
			this._cleanupTaskSemaphore.Release();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000037A4 File Offset: 0x000019A4
		private bool SubscribeToTimeBasedEvent(TimeBasedEventSubscription subscriptionData, SubscribedEvent subscribedEvent, MachineInformation machineInformation)
		{
			TimeBasedMonitor.<>c__DisplayClass16_0 CS$<>8__locals1 = new TimeBasedMonitor.<>c__DisplayClass16_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.subscriptionData = subscriptionData;
			CS$<>8__locals1.subscribedEvent = subscribedEvent;
			string text = Serializer.Serialize<SubscribedEvent>(CS$<>8__locals1.subscribedEvent) + this.GetVersionTag();
			using (TaskService taskService = new TaskService())
			{
				TimeBasedMonitor.<>c__DisplayClass16_1 CS$<>8__locals2 = new TimeBasedMonitor.<>c__DisplayClass16_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.taskName = Guid.NewGuid().ToString();
				bool flag = false;
				try
				{
					if (taskService.RootFolder.SubFolders.Exists("Lenovo") && taskService.RootFolder.SubFolders["Lenovo"].SubFolders.Exists("ImController") && taskService.RootFolder.SubFolders["Lenovo"].SubFolders["ImController"].SubFolders.Exists("TimeBasedEvents") && taskService.RootFolder.SubFolders["Lenovo"].SubFolders["ImController"].SubFolders["TimeBasedEvents"].Tasks != null)
					{
						foreach (Microsoft.Win32.TaskScheduler.Task task in taskService.RootFolder.SubFolders["Lenovo"].SubFolders["ImController"].SubFolders["TimeBasedEvents"].Tasks)
						{
							string a = task.Definition.RegistrationInfo.Description.Replace("\r", "").Replace("\n", "");
							string text2 = text.Replace("\r", "");
							text2 = text2.Replace("\n", "");
							if (a == text2)
							{
								CS$<>8__locals2.taskName = task.Name;
								flag = true;
								break;
							}
						}
					}
				}
				catch
				{
				}
				if (!flag)
				{
					using (TaskDefinition taskDefinition = taskService.NewTask())
					{
						taskDefinition.Principal.UserId = "NT AUTHORITY\\LocalService";
						taskDefinition.Settings.DisallowStartIfOnBatteries = false;
						taskDefinition.Settings.StopIfGoingOnBatteries = false;
						taskDefinition.Settings.StartWhenAvailable = true;
						taskDefinition.RegistrationInfo.Description = text;
						taskDefinition.Settings.RunOnlyIfNetworkAvailable = CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RequireNetworkConnection;
						DateTimeOffset dateTimeOffset = (CS$<>8__locals2.CS$<>8__locals1.subscriptionData.OOBEProximity ? machineInformation.FirstRunDate : DateTime.Now);
						DateTimeOffset startDateTime = CS$<>8__locals2.CS$<>8__locals1.subscriptionData.StartDateTime;
						if (!CS$<>8__locals2.CS$<>8__locals1.subscriptionData.OOBEProximity && CS$<>8__locals2.CS$<>8__locals1.subscriptionData.StartDateTime.Ticks != 0L)
						{
							dateTimeOffset = CS$<>8__locals2.CS$<>8__locals1.subscriptionData.StartDateTime;
						}
						short repeatInterval = CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatInterval;
						int num = 0;
						if (CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatIntervalUnit == RepeatIntervalUnitEnum.Hourly)
						{
							num = (int)CS$<>8__locals2.CS$<>8__locals1.subscriptionData.OffsetForFirstEvent;
						}
						else if (RepeatIntervalUnitEnum.Daily == CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatIntervalUnit)
						{
							num = (int)(24 * CS$<>8__locals2.CS$<>8__locals1.subscriptionData.OffsetForFirstEvent);
						}
						else if (RepeatIntervalUnitEnum.Weekly == CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatIntervalUnit)
						{
							num = (int)(168 * CS$<>8__locals2.CS$<>8__locals1.subscriptionData.OffsetForFirstEvent);
						}
						else if (RepeatIntervalUnitEnum.Monthly == CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatIntervalUnit)
						{
							num = (int)(720 * CS$<>8__locals2.CS$<>8__locals1.subscriptionData.OffsetForFirstEvent);
						}
						if (CS$<>8__locals2.CS$<>8__locals1.subscriptionData.OffsetForFirstEvent != 0)
						{
							dateTimeOffset += TimeSpan.FromHours((double)num);
						}
						TimeSpan timeSpan = default(TimeSpan);
						if (CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatIntervalUnit == RepeatIntervalUnitEnum.Hourly || repeatInterval == 0)
						{
							using (TimeTrigger timeTrigger = new TimeTrigger())
							{
								timeTrigger.StartBoundary = dateTimeOffset.DateTime;
								if (this.GetTimeSpanFromString(ref timeSpan, CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RandomDelay))
								{
									timeTrigger.RandomDelay = timeSpan;
								}
								if (this.GetTimeSpanFromString(ref timeSpan, CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatTaskInterval))
								{
									timeTrigger.Repetition.Interval = timeSpan;
									if (this.GetTimeSpanFromString(ref timeSpan, CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatTaskDuration))
									{
										timeTrigger.Repetition.Duration = timeSpan;
									}
									timeTrigger.Repetition.StopAtDurationEnd = CS$<>8__locals2.CS$<>8__locals1.subscriptionData.StopTaskAtEndDuration;
								}
								taskDefinition.Triggers.Add(timeTrigger);
								goto IL_811;
							}
						}
						if (RepeatIntervalUnitEnum.Daily == CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatIntervalUnit)
						{
							using (DailyTrigger dailyTrigger = new DailyTrigger(1)
							{
								DaysInterval = repeatInterval
							})
							{
								dailyTrigger.StartBoundary = dateTimeOffset.DateTime;
								if (this.GetTimeSpanFromString(ref timeSpan, CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RandomDelay))
								{
									dailyTrigger.RandomDelay = timeSpan;
								}
								if (this.GetTimeSpanFromString(ref timeSpan, CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatTaskInterval))
								{
									dailyTrigger.Repetition.Interval = timeSpan;
									if (this.GetTimeSpanFromString(ref timeSpan, CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatTaskDuration))
									{
										dailyTrigger.Repetition.Duration = timeSpan;
									}
									dailyTrigger.Repetition.StopAtDurationEnd = CS$<>8__locals2.CS$<>8__locals1.subscriptionData.StopTaskAtEndDuration;
								}
								taskDefinition.Triggers.Add(dailyTrigger);
								goto IL_811;
							}
						}
						if (RepeatIntervalUnitEnum.Weekly == CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatIntervalUnit)
						{
							using (WeeklyTrigger weeklyTrigger = new WeeklyTrigger(DaysOfTheWeek.Sunday, 1)
							{
								WeeksInterval = repeatInterval
							})
							{
								weeklyTrigger.StartBoundary = dateTimeOffset.DateTime;
								DayOfTheWeekEnum dayForWeeklyEvent = CS$<>8__locals2.CS$<>8__locals1.subscriptionData.DayForWeeklyEvent;
								if (dayForWeeklyEvent <= DayOfTheWeekEnum.Thursday)
								{
									switch (dayForWeeklyEvent)
									{
									case DayOfTheWeekEnum.AllDays:
										weeklyTrigger.DaysOfWeek = DaysOfTheWeek.AllDays;
										break;
									case DayOfTheWeekEnum.Sunday:
										weeklyTrigger.DaysOfWeek = DaysOfTheWeek.Sunday;
										break;
									case DayOfTheWeekEnum.Monday:
										weeklyTrigger.DaysOfWeek = DaysOfTheWeek.Monday;
										break;
									case (DayOfTheWeekEnum)3:
									case (DayOfTheWeekEnum)5:
									case (DayOfTheWeekEnum)6:
									case (DayOfTheWeekEnum)7:
										break;
									case DayOfTheWeekEnum.Tuesday:
										weeklyTrigger.DaysOfWeek = DaysOfTheWeek.Tuesday;
										break;
									case DayOfTheWeekEnum.Wednesday:
										weeklyTrigger.DaysOfWeek = DaysOfTheWeek.Wednesday;
										break;
									default:
										if (dayForWeeklyEvent == DayOfTheWeekEnum.Thursday)
										{
											weeklyTrigger.DaysOfWeek = DaysOfTheWeek.Thursday;
										}
										break;
									}
								}
								else if (dayForWeeklyEvent != DayOfTheWeekEnum.Friday)
								{
									if (dayForWeeklyEvent == DayOfTheWeekEnum.Saturday)
									{
										weeklyTrigger.DaysOfWeek = DaysOfTheWeek.Saturday;
									}
								}
								else
								{
									weeklyTrigger.DaysOfWeek = DaysOfTheWeek.Friday;
								}
								if (this.GetTimeSpanFromString(ref timeSpan, CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RandomDelay))
								{
									weeklyTrigger.RandomDelay = timeSpan;
								}
								if (this.GetTimeSpanFromString(ref timeSpan, CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatTaskInterval))
								{
									weeklyTrigger.Repetition.Interval = timeSpan;
									if (this.GetTimeSpanFromString(ref timeSpan, CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatTaskDuration))
									{
										weeklyTrigger.Repetition.Duration = timeSpan;
									}
									weeklyTrigger.Repetition.StopAtDurationEnd = CS$<>8__locals2.CS$<>8__locals1.subscriptionData.StopTaskAtEndDuration;
								}
								taskDefinition.Triggers.Add(weeklyTrigger);
								goto IL_811;
							}
						}
						if (RepeatIntervalUnitEnum.Monthly == CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatIntervalUnit)
						{
							using (MonthlyTrigger monthlyTrigger = new MonthlyTrigger(1, MonthsOfTheYear.AllMonths)
							{
								MonthsOfYear = MonthsOfTheYear.AllMonths
							})
							{
								monthlyTrigger.StartBoundary = dateTimeOffset.DateTime;
								monthlyTrigger.DaysOfMonth = CS$<>8__locals2.CS$<>8__locals1.subscriptionData.DatesForMonthlyEvent;
								if (this.GetTimeSpanFromString(ref timeSpan, CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RandomDelay))
								{
									monthlyTrigger.RandomDelay = timeSpan;
								}
								if (this.GetTimeSpanFromString(ref timeSpan, CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatTaskInterval))
								{
									monthlyTrigger.Repetition.Interval = timeSpan;
									if (this.GetTimeSpanFromString(ref timeSpan, CS$<>8__locals2.CS$<>8__locals1.subscriptionData.RepeatTaskDuration))
									{
										monthlyTrigger.Repetition.Duration = timeSpan;
									}
									monthlyTrigger.Repetition.StopAtDurationEnd = CS$<>8__locals2.CS$<>8__locals1.subscriptionData.StopTaskAtEndDuration;
								}
								taskDefinition.Triggers.Add(monthlyTrigger);
							}
						}
						IL_811:
						string arguments = "/timebasedeventtrigger " + CS$<>8__locals2.taskName;
						string str = "\"";
						Assembly entryAssembly = Assembly.GetEntryAssembly();
						using (ExecAction execAction = new ExecAction(str + ((entryAssembly != null) ? entryAssembly.Location : null) + "\"", arguments, null))
						{
							taskDefinition.Actions.Add(execAction);
						}
						try
						{
							TaskFolder taskFolder = taskService.RootFolder;
							if (!taskFolder.SubFolders.Exists("Lenovo"))
							{
								taskFolder = taskFolder.CreateFolder("Lenovo", null);
							}
							else
							{
								taskFolder = taskFolder.SubFolders["Lenovo"];
							}
							if (!taskFolder.SubFolders.Exists("ImController"))
							{
								taskFolder = taskFolder.CreateFolder("ImController", null);
							}
							else
							{
								taskFolder = taskFolder.SubFolders["ImController"];
							}
							if (!taskFolder.SubFolders.Exists("TimeBasedEvents"))
							{
								taskFolder = taskFolder.CreateFolder("TimeBasedEvents", null);
							}
							else
							{
								taskFolder = taskFolder.SubFolders["TimeBasedEvents"];
							}
							taskFolder.RegisterTaskDefinition(CS$<>8__locals2.taskName, taskDefinition);
							flag = true;
						}
						catch (Exception ex)
						{
							Logger.Log(ex, "TimeBasedMonitor: Folder already exists");
						}
					}
				}
				if (flag)
				{
					System.Threading.Tasks.Task item = System.Threading.Tasks.Task.Run(delegate()
					{
						TimeBasedMonitor.<>c__DisplayClass16_1.<<SubscribeToTimeBasedEvent>b__0>d <<SubscribeToTimeBasedEvent>b__0>d;
						<<SubscribeToTimeBasedEvent>b__0>d.<>4__this = CS$<>8__locals2;
						<<SubscribeToTimeBasedEvent>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<SubscribeToTimeBasedEvent>b__0>d.<>1__state = -1;
						AsyncTaskMethodBuilder <>t__builder = <<SubscribeToTimeBasedEvent>b__0>d.<>t__builder;
						<>t__builder.Start<TimeBasedMonitor.<>c__DisplayClass16_1.<<SubscribeToTimeBasedEvent>b__0>d>(ref <<SubscribeToTimeBasedEvent>b__0>d);
						return <<SubscribeToTimeBasedEvent>b__0>d.<>t__builder.Task;
					});
					this._monitorTaskList.Add(item);
				}
			}
			return true;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000421C File Offset: 0x0000241C
		private void RetouchTaskUsingOSUtility(string taskName)
		{
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "RetouchTaskUsingOSUtility: Retouch task with id: {0}", new object[] { taskName });
				if (!Utility.SanitizeString(ref taskName))
				{
					Logger.Log(Logger.LogSeverity.Error, "RetouchTaskUsingOSUtility: Failed to RetouchTaskUsingOSUtility as tasName is invalid. TaskName - {0}", new object[] { taskName });
				}
				else
				{
					string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\system32\\schtasks.exe";
					string arguments = "/change /tn \"Lenovo\\ImController\\TimebasedEvents\\" + taskName + "\" /du 01:00";
					Process process = Process.Start(new ProcessStartInfo
					{
						FileName = fileName,
						Arguments = arguments,
						CreateNoWindow = true,
						WindowStyle = ProcessWindowStyle.Hidden,
						UseShellExecute = false
					});
					if (process != null)
					{
						process.WaitForExit(60000);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "RetouchTaskUsingOSUtility: Exception");
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000042E0 File Offset: 0x000024E0
		private bool GetTimeSpanFromString(ref TimeSpan randomDelayTimeSpan, string timeSpanPthFormatString)
		{
			bool result = false;
			if (!string.IsNullOrWhiteSpace(timeSpanPthFormatString))
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				try
				{
					string[] array = timeSpanPthFormatString.Split(new char[] { 'P', 'D', 'T', 'H', 'M', 'S' });
					int num5 = 0;
					if (timeSpanPthFormatString.IndexOf('P') >= 0)
					{
						num5++;
						if (timeSpanPthFormatString.IndexOf('D') >= 0)
						{
							num = int.Parse(array[num5]);
							num5++;
						}
						if (timeSpanPthFormatString.IndexOf('T') >= 0)
						{
							num5++;
							if (timeSpanPthFormatString.IndexOf('H') >= 0)
							{
								num2 = int.Parse(array[num5]);
								num5++;
							}
							if (timeSpanPthFormatString.IndexOf('M') >= 0)
							{
								num3 = int.Parse(array[num5]);
								num5++;
							}
							if (timeSpanPthFormatString.IndexOf('S') >= 0)
							{
								num4 = int.Parse(array[num5]);
								num5++;
							}
						}
					}
					if (num > 0 || num2 > 0 || num3 > 0 || num4 > 0)
					{
						randomDelayTimeSpan = TimeSpan.Parse(string.Concat(new string[]
						{
							num.ToString(),
							":",
							num2.ToString(),
							":",
							num3.ToString(),
							":",
							num4.ToString()
						}));
						result = true;
					}
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "TimeBasedMonitor: InvalidFormat Exception occured while adding randomdelay.");
				}
			}
			return result;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00004450 File Offset: 0x00002650
		private async System.Threading.Tasks.Task StartScheduledTaskregMonitoring(string regKeyPath, TimeBasedEventSubscription subscriptionData, SubscribedEvent subscribedEvent)
		{
			try
			{
				bool missedNotification = false;
				try
				{
					using (RegistryKey registryKey = Registry.Users.CreateSubKey("S-1-5-19\\\\Software\\\\Lenovo\\\\ImController\\\\ScheduledTasks\\\\" + regKeyPath))
					{
						if (registryKey != null)
						{
							if (registryKey.GetValue("ExecutionTime") == null)
							{
								registryKey.SetValue("ExecutionTime", DateTime.Now.ToUniversalTime().ToString());
								registryKey.SetValue("LastNotificationTime", DateTime.Now.ToUniversalTime().ToString());
							}
							else if (registryKey.GetValue("LastNotificationTime") == null)
							{
								missedNotification = true;
							}
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "TimeBasedMonitor: Exception occured while checking last notification logic");
				}
				for (;;)
				{
					bool flag = missedNotification || this.WaitForRegistryValueChange("S-1-5-19\\Software\\Lenovo\\ImController\\ScheduledTasks\\" + regKeyPath, "ExecutionTime");
					if (!flag)
					{
						break;
					}
					missedNotification = false;
					string parameter = Serializer.Serialize<TimeBasedEventReaction>(new TimeBasedEventReaction
					{
						FriendlyName = subscriptionData.FriendlyName,
						RepeatIntervalUnit = subscriptionData.RepeatIntervalUnit,
						OOBEProximity = subscriptionData.OOBEProximity,
						RepeatInterval = subscriptionData.RepeatInterval,
						StartDateTime = subscriptionData.StartDateTime,
						DatesForMonthlyEvent = subscriptionData.DatesForMonthlyEvent,
						DayForWeeklyEvent = subscriptionData.DayForWeeklyEvent,
						OffsetForFirstEvent = subscriptionData.OffsetForFirstEvent,
						RequireNetworkConnection = subscriptionData.RequireNetworkConnection,
						RandomDelay = subscriptionData.RandomDelay,
						RepeatTaskDuration = subscriptionData.RepeatTaskDuration,
						RepeatTaskInterval = subscriptionData.RepeatTaskInterval,
						StopTaskAtEndDuration = subscriptionData.StopTaskAtEndDuration
					});
					EventReaction eventReaction = new EventReaction
					{
						Monitor = TimeBasedEventConstants.Get.TimeBasedEventMonitorName,
						DataType = TimeBasedEventConstants.Get.TimeBasedEventDataType,
						Trigger = TimeBasedEventConstants.Get.TimeBasedEventTriggerName,
						Parameter = parameter
					};
					base.NotifyObservers(eventReaction, subscribedEvent);
					try
					{
						using (RegistryKey registryKey2 = Registry.Users.CreateSubKey("S-1-5-19\\\\Software\\\\Lenovo\\\\ImController\\\\ScheduledTasks\\\\" + regKeyPath))
						{
							if (registryKey2 != null)
							{
								registryKey2.SetValue("LastNotificationTime", DateTime.Now.ToUniversalTime().ToString());
							}
						}
					}
					catch (Exception ex2)
					{
						Logger.Log(ex2, "TimeBasedMonitor: Exception occured while updating notification time");
					}
					await System.Threading.Tasks.Task.Delay(10000);
				}
			}
			catch (Exception ex3)
			{
				Logger.Log(ex3, "TimeBasedMonitor: Exception occured while monitoring registry in time based monitoring");
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000044B0 File Offset: 0x000026B0
		private bool WaitForRegistryValueChange(string regKeyPath, string regValue)
		{
			bool flag = false;
			WaitHandle[] array = new WaitHandle[2];
			AutoResetEvent autoResetEvent = new AutoResetEvent(false);
			array[0] = autoResetEvent;
			array[1] = this._eventTerminate;
			IntPtr intPtr;
			if (Win32.RegOpenKeyEx(Win32.HKEY_USERS, regKeyPath, 0U, 131089, out intPtr) == 0)
			{
				while (!array[1].WaitOne(0, true))
				{
					try
					{
						Win32.REG_NOTIFY_CHANGE notifyFilter = Win32.REG_NOTIFY_CHANGE.LAST_SET;
						if (Win32.RegNotifyChangeKeyValue(intPtr, false, notifyFilter, autoResetEvent.SafeWaitHandle.DangerousGetHandle(), true) == 0)
						{
							if (WaitHandle.WaitAny(array) == 0)
							{
								flag = true;
							}
						}
						else
						{
							System.Threading.Tasks.Task.Delay(1000).Wait();
						}
					}
					catch (Exception)
					{
					}
					finally
					{
						if (intPtr != IntPtr.Zero)
						{
							Win32.RegCloseKey(intPtr);
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004574 File Offset: 0x00002774
		private string GetVersionTag()
		{
			return "<!-- " + this.Version + ">";
		}

		// Token: 0x0400002B RID: 43
		private readonly IMachineInformationManager _macheininformationManager;

		// Token: 0x0400002C RID: 44
		private MachineInformation _machineInformation;

		// Token: 0x0400002D RID: 45
		private ManualResetEvent _eventTerminate;

		// Token: 0x0400002E RID: 46
		private ConcurrentBag<System.Threading.Tasks.Task> _monitorTaskList;

		// Token: 0x0400002F RID: 47
		private ConcurrentBag<SubscribedEvent> _listOfTimebasedEvents;

		// Token: 0x04000030 RID: 48
		private readonly SemaphoreSlim _cleanupTaskSemaphore;

		// Token: 0x04000031 RID: 49
		private bool _isCleanupDone;
	}
}
