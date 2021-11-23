using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.ImController.EventLogging;
using Lenovo.ImController.EventLogging.Model;
using Lenovo.ImController.EventLogging.Services;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.Shared.Telemetry
{
	// Token: 0x02000007 RID: 7
	public class EventLogger
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000025BC File Offset: 0x000007BC
		public static EventLogger GetInstance()
		{
			EventLogger result;
			if ((result = EventLogger._instance) == null)
			{
				result = (EventLogger._instance = new EventLogger());
			}
			return result;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000025D4 File Offset: 0x000007D4
		private EventLogger()
		{
			this._logInSerialSemaphore = new SemaphoreSlim(1);
			this._systemTelemetryLogger = new TelemetryLogger(KnownConstants.AppChannel.Core, Constants.ImControllerMetricsProductName, Constants.ImControllerVersion);
			this._subscriptionSettingsAgent = SubscriptionSettingsAgent.GetInstance();
			this._eventAblenessDictionary = new ConcurrentDictionary<string, bool>();
			this._eventQueue = new BlockingCollection<ImcEvent>(new ConcurrentQueue<ImcEvent>());
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002630 File Offset: 0x00000830
		public void StartOrResume(CancellationToken cancelToken)
		{
			Logger.Log(Logger.LogSeverity.Information, "EventLogger: Worker requested to start");
			try
			{
				this._shouldBeWorking = true;
				this._cancellationToken = cancelToken;
				this._workerThread = new Thread(delegate()
				{
					this.LogThingsOnDifferentThread(cancelToken);
				})
				{
					Name = "ImcEventLoggerThread"
				};
				this._workerThread.Start();
				Logger.Log(Logger.LogSeverity.Information, "EventLogger: Worker start complete");
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "EventLogger: Exception while starting or resuming");
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000026C8 File Offset: 0x000008C8
		public void Stop()
		{
			Logger.Log(Logger.LogSeverity.Information, "EventLogger: Worker requested to stop");
			try
			{
				this._shouldBeWorking = false;
				if (this._workerThread != null)
				{
					this._workerThread.Join();
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "EventLogger: Exception while handling stop");
			}
			Logger.Log(Logger.LogSeverity.Information, "EventLogger: Worker thread stopped");
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002724 File Offset: 0x00000924
		private void LogThingsOnDifferentThread(CancellationToken cancelToken)
		{
			try
			{
				while (this._shouldBeWorking && !cancelToken.IsCancellationRequested)
				{
					try
					{
						this._logInSerialSemaphore.Wait(cancelToken);
						if (!cancelToken.IsCancellationRequested)
						{
							ImcEvent imcEvent = this._eventQueue.Take(cancelToken);
							if (imcEvent != null && imcEvent.Event != null)
							{
								Logger.Log(Logger.LogSeverity.Information, "EventLogger: Event " + (imcEvent.Event.Name ?? "null") + " has been dequeued");
								if (!ImcPolicy.IsDeviceImprovementDisabled())
								{
									if (!cancelToken.IsCancellationRequested)
									{
										if (this.IsEventEnabledViaSubscription(imcEvent.Event, cancelToken))
										{
											this.InjectGlobalVariables(imcEvent.Event);
											bool flag = this._systemTelemetryLogger.LogEvent(imcEvent.Event);
											if (!flag)
											{
												Logger.LogSeverity severity = Logger.LogSeverity.Information;
												string format = "EventLogger: Logged event {0} ? {1})";
												object arg;
												if (imcEvent == null)
												{
													arg = null;
												}
												else
												{
													UserEvent @event = imcEvent.Event;
													arg = ((@event != null) ? @event.Name : null);
												}
												Logger.Log(severity, string.Format(format, arg, flag));
											}
										}
										else
										{
											Logger.Log(Logger.LogSeverity.Information, "EventLogger: Will not log event due to subscription policy");
										}
									}
								}
								else
								{
									Logger.Log(Logger.LogSeverity.Information, "EventLogger: Will not log event due to local policy");
								}
							}
						}
					}
					catch (OperationCanceledException)
					{
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "EventLogger: Exception while handling particular event");
					}
					finally
					{
						this._logInSerialSemaphore.Release();
					}
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "EventLogger: Exception while handling thread");
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000028CC File Offset: 0x00000ACC
		public void HandleUserChange()
		{
			Logger.Log(Logger.LogSeverity.Information, "EventLogger: Handling user change");
			this._systemTelemetryLogger.ClearUserDataCache();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000028E4 File Offset: 0x00000AE4
		public void CleanupData()
		{
			this._eventAblenessDictionary.Clear();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000028F4 File Offset: 0x00000AF4
		public void LogEvent(ImcEvent userEvent)
		{
			try
			{
				if (userEvent == null || string.IsNullOrWhiteSpace(userEvent.Event.Name))
				{
					throw new ArgumentNullException("UserEvent must not be null and have an event name");
				}
				this._eventQueue.Add(userEvent);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "EventLogger: Exception while logging telemetry event");
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000294C File Offset: 0x00000B4C
		private bool IsEventEnabledViaSubscription(UserEvent userEvent, CancellationToken cancelToken)
		{
			Task<bool> task = this.IsEventEnabledViaSubscriptionAsync(userEvent, cancelToken);
			task.Wait();
			return task.Result;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002964 File Offset: 0x00000B64
		private async Task<bool> IsEventEnabledViaSubscriptionAsync(UserEvent userEvent, CancellationToken cancelToken)
		{
			bool result;
			try
			{
				if (userEvent == null || string.IsNullOrWhiteSpace(userEvent.Name))
				{
					Logger.Log(Logger.LogSeverity.Error, "EventLogger: User event is null");
					result = false;
				}
				else if (this._eventAblenessDictionary.ContainsKey(userEvent.Name))
				{
					result = this._eventAblenessDictionary[userEvent.Name];
				}
				else
				{
					string settingName = "Imc.Telemetry.IsEnabled";
					Setting setting = await this._subscriptionSettingsAgent.GetApplicableSettingAsync(settingName, cancelToken);
					if (setting != null)
					{
						bool flag = false;
						bool? valueAsBool = setting.GetValueAsBool();
						if (valueAsBool != null && valueAsBool.GetValueOrDefault(false))
						{
							flag = true;
						}
						if (!flag)
						{
							this._eventAblenessDictionary.TryAdd(userEvent.Name, false);
							Logger.Log(Logger.LogSeverity.Warning, "EventLogger: Subscription Global setting is for '" + userEvent.Name + "' is disabled");
							return false;
						}
					}
					if (cancelToken.IsCancellationRequested)
					{
						result = false;
					}
					else
					{
						string settingName2 = "Imc.Telemetry.Event." + userEvent.Name + ".IsEnabled";
						Setting setting2 = await this._subscriptionSettingsAgent.GetApplicableSettingAsync(settingName2, cancelToken);
						if (setting2 != null && setting2.GetValueAsBool() != null)
						{
							bool flag2 = false;
							bool? valueAsBool2 = setting2.GetValueAsBool();
							if (valueAsBool2 != null && valueAsBool2.GetValueOrDefault(false))
							{
								flag2 = true;
							}
							this._eventAblenessDictionary.TryAdd(userEvent.Name, flag2);
							Logger.Log(Logger.LogSeverity.Information, string.Format("EventLogger: Subscription setting found for '{0}' is '{1}' with value '{2}' ", userEvent.Name, flag2, setting2.Value));
							result = flag2;
						}
						else
						{
							this._eventAblenessDictionary.TryAdd(userEvent.Name, false);
							Logger.Log(Logger.LogSeverity.Warning, "EventLogger: Subscription setting not found for for '" + userEvent.Name + "' is disabled by default");
							result = false;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "EventLogger: Exception while detecting if telemetry is enabled for event");
				result = false;
			}
			return result;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000029BC File Offset: 0x00000BBC
		private void InjectGlobalVariables(UserEvent userEvent)
		{
			SessionTracker instance = SessionTracker.GetInstance();
			userEvent.Variables.Add(new UserEventVariable("ImcVersion", Constants.ImControllerVersion));
			userEvent.Variables.Add(new UserEventVariable("DriverVersion", Constants.ImDriverVersion));
			userEvent.Variables.Add(new UserEventVariable("MinutesSinceServiceStart", instance.GetMinutesSinceProcessStart().ToString()));
			userEvent.Variables.Add(new UserEventVariable("MinutesSinceSessionStart", instance.GetMinutesSinceSessionStart().ToString()));
			userEvent.Variables.Add(new UserEventVariable("WindowsSessionID", instance.GetSessionId().ToString()));
		}

		// Token: 0x04000042 RID: 66
		private static EventLogger _instance;

		// Token: 0x04000043 RID: 67
		private readonly TelemetryLogger _systemTelemetryLogger;

		// Token: 0x04000044 RID: 68
		private readonly SubscriptionSettingsAgent _subscriptionSettingsAgent;

		// Token: 0x04000045 RID: 69
		private readonly ConcurrentDictionary<string, bool> _eventAblenessDictionary;

		// Token: 0x04000046 RID: 70
		private readonly BlockingCollection<ImcEvent> _eventQueue;

		// Token: 0x04000047 RID: 71
		private readonly SemaphoreSlim _logInSerialSemaphore;

		// Token: 0x04000048 RID: 72
		private CancellationToken _cancellationToken;

		// Token: 0x04000049 RID: 73
		private Thread _workerThread;

		// Token: 0x0400004A RID: 74
		private bool _shouldBeWorking;
	}
}
