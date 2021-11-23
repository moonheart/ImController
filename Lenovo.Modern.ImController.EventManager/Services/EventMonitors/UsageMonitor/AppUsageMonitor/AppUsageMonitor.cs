using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.SystemContext.Interop;
using Lenovo.Modern.Utilities.Services.SystemContext.Settings;
using Lenovo.Modern.Utilities.Services.SystemContext.Shared;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.UsageMonitor.AppUsageMonitor
{
	// Token: 0x02000020 RID: 32
	public sealed class AppUsageMonitor
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x0000604C File Offset: 0x0000424C
		public static AppUsageMonitor GetInstance()
		{
			AppUsageMonitor result;
			if ((result = AppUsageMonitor._instance) == null)
			{
				result = (AppUsageMonitor._instance = new AppUsageMonitor());
			}
			return result;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00006064 File Offset: 0x00004264
		private AppUsageMonitor()
		{
			this._sidAppUsageQ = new ConcurrentDictionary<string, ConcurrentQueue<AppUsageRecord>>();
			this._pidNames = new ConcurrentDictionary<string, string>();
			this._pluginList = new BlockingCollection<string>();
			this._sidUserProfileDir = new ConcurrentDictionary<string, string>();
			this._sidUserTokens = new ConcurrentDictionary<string, IntPtr>();
			this._syncSempahore = new SemaphoreSlim(1);
			this._wmiSyncSelaphore = new SemaphoreSlim(1);
			this._subscriptionSettingsAgent = SubscriptionSettingsAgent.GetInstance();
			this._windowsSessionGuid = Guid.NewGuid();
			try
			{
				this._userInformation = UserInformationProvider.Instance.GetUserInformation();
				this._sidUserProfileDir.TryAdd(this._userInformation.SID, this._userInformation.UserProfileFolder);
				this.AddUserTokenToDictionary(this._userInformation.SID);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "AppUsageMonitor.Start: Unable to get user information");
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00006140 File Offset: 0x00004340
		protected override void Finalize()
		{
			try
			{
				try
				{
					foreach (KeyValuePair<string, IntPtr> keyValuePair in this._sidUserTokens)
					{
						if (keyValuePair.Value != IntPtr.Zero)
						{
							Impersonateuser.CloseHandle(keyValuePair.Value);
						}
					}
				}
				catch (Exception)
				{
				}
				if (this._processStartWatcher != null)
				{
					try
					{
						this._processStartWatcher.Stop();
						this._processStartWatcher.EventArrived -= this.OnProcessStarted;
						this._processStartWatcher.Dispose();
						this._processStartWatcher = null;
						this._isProcessStartWatcherStarted = false;
					}
					catch (Exception)
					{
					}
				}
				if (this._processStopWatcher != null)
				{
					try
					{
						this._processStopWatcher.Stop();
						this._processStopWatcher.EventArrived -= this.OnProcessStopped;
						this._processStopWatcher.Dispose();
						this._processStopWatcher = null;
						this._isProcessStopWatcherStarted = false;
					}
					catch (Exception)
					{
					}
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00006274 File Offset: 0x00004474
		public void Start(EventHandlerReason reason, SubscribedEvent subscribedEvent)
		{
			Logger.Log(Logger.LogSeverity.Information, "AppUsageMonitor: Start.entry: reason={0}", new object[] { reason.ToString() });
			if (!this.IsAppUsageMonitorEnabled())
			{
				Logger.Log(Logger.LogSeverity.Warning, "AppUsageMonitor.Start: Not starting app usage monior since ts disabled by IMC setting");
				return;
			}
			this._wmiSyncSelaphore.Wait();
			try
			{
				try
				{
					this._userInformation = UserInformationProvider.Instance.GetUserInformation();
					this._sidUserProfileDir.TryAdd(this._userInformation.SID, this._userInformation.UserProfileFolder);
					this.AddUserTokenToDictionary(this._userInformation.SID);
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "AppUsageMonitor.Start: Unable to get user information");
				}
				if (!this._isProcessStartWatcherStarted)
				{
					try
					{
						this._processStartWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
						this._processStartWatcher.EventArrived += this.OnProcessStarted;
						this._isProcessStartWatcherStarted = true;
						this._processStartWatcher.Start();
					}
					catch (Exception ex2)
					{
						Logger.Log(ex2, "AppUsageMonitor: Unable to start mgmt watcher for Win32_ProcessStartTrace");
					}
					this.AddSystemEventData(reason, this._userInformation.SID, true);
					if (reason == EventHandlerReason.SystemStart)
					{
						IntPtr zero = IntPtr.Zero;
						if (Authorization.GetSessionUserToken(ref zero))
						{
							this.AddSystemEventData(EventHandlerReason.UserLogon, this._userInformation.SID, true);
						}
					}
				}
				if (this._isProcessStartWatcherStarted && !this._isProcessStopWatcherStarted)
				{
					try
					{
						this._processStopWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
						this._processStopWatcher.EventArrived += this.OnProcessStopped;
						this._isProcessStopWatcherStarted = true;
						this._processStopWatcher.Start();
					}
					catch (Exception ex3)
					{
						Logger.Log(ex3, "AppUsageMonitor: Unable to start mgmt watcher for Win32_ProcessStopTrace");
					}
				}
				if (!this._pluginList.Contains(subscribedEvent.Plugin))
				{
					this._pluginList.TryAdd(subscribedEvent.Plugin);
				}
				if (this.GetFlushIntervalMinutes() > 0)
				{
					this._flushTimer = new System.Timers.Timer(TimeSpan.FromMinutes((double)this._flushIntervalMinutes.Value).TotalMilliseconds);
					this._flushTimer.Elapsed += delegate(object s, ElapsedEventArgs e)
					{
						this.OnFlushTimerEvent();
					};
					this._flushTimer.AutoReset = true;
					this._flushTimer.Enabled = true;
				}
			}
			catch (Exception ex4)
			{
				Logger.Log(ex4, "AppUsageMonitor: Exception occured in Start");
			}
			this._wmiSyncSelaphore.Release();
			Logger.Log(Logger.LogSeverity.Information, "AppUsageMonitor: Start.exit: profile folder: {0} reason:{1}", new object[]
			{
				this._userInformation.UserProfileFolder,
				reason.ToString()
			});
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000652C File Offset: 0x0000472C
		public void Stop(EventHandlerReason reason)
		{
			Logger.Log(Logger.LogSeverity.Information, "AppUsageMonitor: Stop.entry: reason={0}", new object[] { reason.ToString() });
			if (!this.IsAppUsageMonitorEnabled())
			{
				Logger.Log(Logger.LogSeverity.Warning, "AppUsageMonitor.Stop: Not doing anything for app usage monior since ts disabled by IMC setting");
				return;
			}
			this._wmiSyncSelaphore.Wait();
			if (this._isProcessStartWatcherStarted)
			{
				try
				{
					this._processStartWatcher.Stop();
					this._processStartWatcher.EventArrived -= this.OnProcessStarted;
					this._processStartWatcher.Dispose();
					this._processStartWatcher = null;
					this._isProcessStartWatcherStarted = false;
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "AppUsageMonitor: Unable to stop mgmt watcher for Win32_ProcessStopTrace");
				}
				if (reason == EventHandlerReason.SystemShutdown || reason == EventHandlerReason.SystemSuspend || reason == EventHandlerReason.UserLogoff)
				{
					this.AddSystemEventData(reason, this._userInformation.SID, false);
				}
			}
			if (this._isProcessStopWatcherStarted)
			{
				try
				{
					this._processStopWatcher.Stop();
					this._processStopWatcher.EventArrived -= this.OnProcessStopped;
					this._processStopWatcher.Dispose();
					this._processStopWatcher = null;
					this._isProcessStopWatcherStarted = false;
				}
				catch (Exception ex2)
				{
					Logger.Log(ex2, "AppUsageMonitor: Unable to stop mgmt watcher for Win32_ProcessStopTrace");
				}
			}
			if (this._flushTimer != null)
			{
				this._flushTimer.Enabled = false;
				this._flushTimer = null;
			}
			this._wmiSyncSelaphore.Release();
			this.FlushToDisk(reason == EventHandlerReason.SystemShutdown);
			Logger.Log(Logger.LogSeverity.Information, "AppUsageMonitor: Stop.exit: reason={0}", new object[] { reason.ToString() });
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000066A8 File Offset: 0x000048A8
		public void OnWindowsEvent(EventHandlerReason reason)
		{
			Logger.Log(Logger.LogSeverity.Information, "AppUsageMonitor: OnWindowsEvent.Entry reason={0}", new object[] { reason.ToString() });
			try
			{
				if (this.IsAppUsageMonitorEnabled())
				{
					this.AddSystemEventData(reason, this._userInformation.SID, false);
					if (reason == EventHandlerReason.SystemShutdown || reason == EventHandlerReason.SystemSuspend || reason == EventHandlerReason.UserLogoff)
					{
						this.FlushToDisk(reason == EventHandlerReason.SystemShutdown);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "AppUsageMonitor: Exception in NotifyWindowsEvent");
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00006728 File Offset: 0x00004928
		private string GetUserProfileFolder(string sid)
		{
			string result = string.Empty;
			IContainer container = new SystemContextRegistrySystem().LoadContainer("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\ProfileList\\" + sid);
			if (container != null)
			{
				result = new DirectoryInfo(container.GetValue("ProfileImagePath").GetValueAsString()).FullName;
			}
			return result;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00006770 File Offset: 0x00004970
		private void AddUserTokenToDictionary(string sid)
		{
			IntPtr zero = IntPtr.Zero;
			try
			{
				Authorization.GetSessionUserToken(ref zero);
				if (zero != IntPtr.Zero && !this._sidUserTokens.TryAdd(sid, zero))
				{
					Impersonateuser.CloseHandle(zero);
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "AppusagMonitor.AddUserToken: Exception for sid={0}", new object[] { sid });
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000067D8 File Offset: 0x000049D8
		private void AddSystemEventData(EventHandlerReason reason, string sid, bool start)
		{
			Logger.Log(Logger.LogSeverity.Information, "AppUsageMonitor: AddSystemEventData entry for appId: SYSTEM sid: {0} reason: {1}", new object[]
			{
				sid,
				reason.ToString()
			});
			this._syncSempahore.Wait();
			ConcurrentQueue<AppUsageRecord> concurrentQueue;
			if (!this._sidAppUsageQ.TryGetValue(sid, out concurrentQueue))
			{
				concurrentQueue = new ConcurrentQueue<AppUsageRecord>();
			}
			concurrentQueue.Enqueue(new AppUsageRecord("SYSTEM", "", reason, sid));
			this._sidAppUsageQ.TryAdd(sid, concurrentQueue);
			this._syncSempahore.Release();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000685C File Offset: 0x00004A5C
		private void AddAppUsageData(string pid, string name, string sid, bool start)
		{
			this._syncSempahore.Wait();
			ConcurrentQueue<AppUsageRecord> concurrentQueue;
			if (!this._sidAppUsageQ.TryGetValue(sid, out concurrentQueue))
			{
				concurrentQueue = new ConcurrentQueue<AppUsageRecord>();
			}
			concurrentQueue.Enqueue(new AppUsageRecord(name, pid, start, sid));
			this._sidAppUsageQ.TryAdd(sid, concurrentQueue);
			this._syncSempahore.Release();
			if (concurrentQueue.Count > this.GetFlushCapacity())
			{
				this.FlushToDisk(false);
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000068CC File Offset: 0x00004ACC
		private void AddProcessStartStopEvent(EventArrivedEventArgs e, bool isProcessStarted)
		{
			try
			{
				string processId = e.NewEvent.Properties["ProcessId"].Value.ToString();
				byte[] binaryForm = (byte[])e.NewEvent.Properties["Sid"].Value;
				string sid = new SecurityIdentifier(binaryForm, 0).ToString();
				string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
				if (sid.Equals(this._userInformation.SID))
				{
					if (isProcessStarted)
					{
						this._pidNames.TryAdd(processId, processName);
						this.AddAppUsageData(processId, processName, sid, isProcessStarted);
					}
					else
					{
						Task.Run(delegate()
						{
							this.TryAddProcessStopData(processId, processName, sid);
						});
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "AppUsageMonitor: Exception in AddProcessStartStopEvent");
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000069E0 File Offset: 0x00004BE0
		private void TryAddProcessStopData(string processId, string processName, string sid)
		{
			string text = "";
			if (this._pidNames.TryRemove(processId, out text))
			{
				processName = text;
				this.AddAppUsageData(processId, processName, sid, false);
				return;
			}
			Task.Delay(500).Wait();
			if (this._pidNames.TryRemove(processId, out text))
			{
				processName = text;
			}
			this.AddAppUsageData(processId, processName, sid, false);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00006A3D File Offset: 0x00004C3D
		private void OnProcessStopped(object sender, EventArrivedEventArgs e)
		{
			this.AddProcessStartStopEvent(e, false);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00006A47 File Offset: 0x00004C47
		private void OnProcessStarted(object sender, EventArrivedEventArgs e)
		{
			this.AddProcessStartStopEvent(e, true);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00006A51 File Offset: 0x00004C51
		private void FlushToDisk(bool flushImmediately)
		{
			if (flushImmediately)
			{
				Task<bool> task = this.FlushToDiskAsync();
				task.Wait();
				bool result = task.Result;
				return;
			}
			Task.Run(async delegate()
			{
				await this.FlushToDiskAsync();
			});
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00006A7C File Offset: 0x00004C7C
		private async Task<bool> FlushToDiskAsync()
		{
			Logger.Log(Logger.LogSeverity.Information, "AppUsageMonitor: FlushToDiskAsync.Entry");
			bool result = false;
			foreach (KeyValuePair<string, ConcurrentQueue<AppUsageRecord>> keyValuePair in this._sidAppUsageQ)
			{
				ConcurrentQueue<AppUsageRecord> value = keyValuePair.Value;
				string sid = keyValuePair.Key;
				this._syncSempahore.Wait();
				AppUsageRecord[] usageDataArray = value.ToArray();
				while (value.Any<AppUsageRecord>())
				{
					AppUsageRecord appUsageRecord;
					value.TryDequeue(out appUsageRecord);
				}
				this._syncSempahore.Release();
				IntPtr zero = IntPtr.Zero;
				this._sidUserTokens.TryGetValue(sid, out zero);
				if (zero != IntPtr.Zero)
				{
					Impersonateuser impersonateuser = new Impersonateuser();
					if (impersonateuser.EnterImpersonation(zero))
					{
						foreach (string pluginName in ((IEnumerable<string>)this._pluginList))
						{
							string pluginCsvFileFolderForUser = this.GetPluginCsvFileFolderForUser(sid, pluginName);
							this._syncSempahore.Wait();
							await this.WriteRecordsToFile(usageDataArray, pluginCsvFileFolderForUser);
							this._syncSempahore.Release();
							result = true;
						}
						IEnumerator<string> enumerator2 = null;
					}
					else
					{
						Logger.Log(Logger.LogSeverity.Warning, "AppUsageMonitor: Not writing to file since we failed to impersonate user");
					}
					impersonateuser.ExitImpersonation();
					impersonateuser = null;
				}
				else
				{
					Logger.Log(Logger.LogSeverity.Warning, "AppUsageMonitor: Not writing to file since we failed to get token for sid {0}", new object[] { sid });
				}
				sid = null;
				usageDataArray = null;
			}
			IEnumerator<KeyValuePair<string, ConcurrentQueue<AppUsageRecord>>> enumerator = null;
			return result;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00006AC4 File Offset: 0x00004CC4
		private async Task<bool> WriteRecordsToFile(AppUsageRecord[] appUsageDataArray, string csvFileFolderPath)
		{
			if (!Utility.SanitizePath(ref csvFileFolderPath))
			{
				Logger.Log(Logger.LogSeverity.Error, "AppUsageMonitor: Not writing to file since folderpath is invalid. Path - {0}", new object[] { csvFileFolderPath });
			}
			else if (!Directory.Exists(csvFileFolderPath))
			{
				Logger.Log(Logger.LogSeverity.Warning, "AppUsageMonitor: Not writing to file since folder not found");
			}
			else
			{
				try
				{
					using (FileStream fileStream = new FileStream(csvFileFolderPath + "\\ProcessLog.csv", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
					{
						using (StreamWriter streamWriter = new StreamWriter(fileStream))
						{
							foreach (AppUsageRecord appUsageRecord in appUsageDataArray)
							{
								streamWriter.Write("\"" + appUsageRecord.AppName + "\",");
								streamWriter.Write("\"" + appUsageRecord.PID + "\", ");
								streamWriter.Write("\"" + appUsageRecord.ActionType + "\", ");
								streamWriter.Write("\"" + appUsageRecord.DateTime + "\", ");
								streamWriter.Write("\"" + appUsageRecord.YogaMode + "\", ");
								streamWriter.Write(Environment.NewLine);
							}
							streamWriter.Flush();
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "AppUsageMonitor: Exception during FlushToDisk");
				}
			}
			bool result;
			return result;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00006B14 File Offset: 0x00004D14
		private void OnFlushTimerEvent()
		{
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "AppUsageMonitor: OnFlushTimerEvent. Flushing data to disk due to timer event");
				this.FlushToDisk(false);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception in AppUsageMonitor.OnFlushTimerEvent");
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00006B54 File Offset: 0x00004D54
		private string GetPluginCsvFileFolderForUser(string sid, string pluginName)
		{
			string str;
			this._sidUserProfileDir.TryGetValue(sid, out str);
			return str + "\\AppData\\Local\\Lenovo\\ImController\\PluginData\\" + pluginName + "\\AppUsageMonitor";
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00006B84 File Offset: 0x00004D84
		private bool IsAppUsageMonitorEnabled()
		{
			Task<bool> task = this.IsAppUsageMonitorEnabledAsync(default(CancellationToken));
			task.Wait();
			return task.Result;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00006BAC File Offset: 0x00004DAC
		private async Task<bool> IsAppUsageMonitorEnabledAsync(CancellationToken cancellationToken)
		{
			bool value;
			if (this._isAppUsageMonitorEnabled != null)
			{
				value = this._isAppUsageMonitorEnabled.Value;
			}
			else
			{
				this._isAppUsageMonitorEnabled = new bool?(false);
				Setting setting = await this._subscriptionSettingsAgent.GetApplicableSettingAsync("Imc.Events.UsageMonitor.IsEnabled", cancellationToken);
				if (setting != null)
				{
					bool? valueAsBool = setting.GetValueAsBool();
					if (valueAsBool != null && valueAsBool.GetValueOrDefault(false))
					{
						this._isAppUsageMonitorEnabled = new bool?(true);
					}
					bool? isAppUsageMonitorEnabled = this._isAppUsageMonitorEnabled;
					bool flag = false;
					if ((isAppUsageMonitorEnabled.GetValueOrDefault() == flag) & (isAppUsageMonitorEnabled != null))
					{
						Logger.Log(Logger.LogSeverity.Warning, "EventLogger: Subscription Global setting is for UsageMonitor is disabled");
					}
				}
				value = this._isAppUsageMonitorEnabled.Value;
			}
			return value;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00006BFC File Offset: 0x00004DFC
		private int GetFlushCapacity()
		{
			Task<int> flushCapacityAsync = this.GetFlushCapacityAsync(default(CancellationToken));
			flushCapacityAsync.Wait();
			return flushCapacityAsync.Result;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00006C24 File Offset: 0x00004E24
		private async Task<int> GetFlushCapacityAsync(CancellationToken cancellationToken)
		{
			int value;
			if (this._flushCapacity != null)
			{
				value = this._flushCapacity.Value;
			}
			else
			{
				this._flushCapacity = new int?(1000);
				Setting setting = await this._subscriptionSettingsAgent.GetApplicableSettingAsync("Imc.Events.UsageMonitor.App.FlushCapacity", cancellationToken);
				if (setting != null)
				{
					int value2 = 0;
					int? valueAsInt = setting.GetValueAsInt();
					if (valueAsInt != null && valueAsInt.GetValueOrDefault(0) != 0)
					{
						value2 = valueAsInt.GetValueOrDefault(0);
					}
					this._flushCapacity = new int?(value2);
				}
				value = this._flushCapacity.Value;
			}
			return value;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00006C74 File Offset: 0x00004E74
		private int GetFlushIntervalMinutes()
		{
			Task<int> flushIntervalMinutesAsync = this.GetFlushIntervalMinutesAsync(default(CancellationToken));
			flushIntervalMinutesAsync.Wait();
			return flushIntervalMinutesAsync.Result;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00006C9C File Offset: 0x00004E9C
		private async Task<int> GetFlushIntervalMinutesAsync(CancellationToken cancellationToken)
		{
			int value;
			if (this._flushIntervalMinutes != null)
			{
				value = this._flushIntervalMinutes.Value;
			}
			else
			{
				this._flushIntervalMinutes = new int?(0);
				Setting setting = await this._subscriptionSettingsAgent.GetApplicableSettingAsync("Imc.Events.UsageMonitor.App.FlushIntervalMinutes", cancellationToken);
				if (setting != null)
				{
					int value2 = 0;
					int? valueAsInt = setting.GetValueAsInt();
					if (valueAsInt != null && valueAsInt.GetValueOrDefault(0) != 0)
					{
						value2 = valueAsInt.GetValueOrDefault(0);
					}
					this._flushIntervalMinutes = new int?(value2);
				}
				value = this._flushIntervalMinutes.Value;
			}
			return value;
		}

		// Token: 0x04000071 RID: 113
		private bool _isProcessStartWatcherStarted;

		// Token: 0x04000072 RID: 114
		private bool _isProcessStopWatcherStarted;

		// Token: 0x04000073 RID: 115
		private ManagementEventWatcher _processStartWatcher;

		// Token: 0x04000074 RID: 116
		private ManagementEventWatcher _processStopWatcher;

		// Token: 0x04000075 RID: 117
		private ConcurrentDictionary<string, ConcurrentQueue<AppUsageRecord>> _sidAppUsageQ;

		// Token: 0x04000076 RID: 118
		private ConcurrentDictionary<string, string> _pidNames;

		// Token: 0x04000077 RID: 119
		private ConcurrentDictionary<string, string> _sidUserProfileDir;

		// Token: 0x04000078 RID: 120
		private ConcurrentDictionary<string, IntPtr> _sidUserTokens;

		// Token: 0x04000079 RID: 121
		private BlockingCollection<string> _pluginList;

		// Token: 0x0400007A RID: 122
		private SemaphoreSlim _syncSempahore;

		// Token: 0x0400007B RID: 123
		private SemaphoreSlim _wmiSyncSelaphore;

		// Token: 0x0400007C RID: 124
		private UserInformation _userInformation;

		// Token: 0x0400007D RID: 125
		private const int _maxUsageRecordsCount = 1000;

		// Token: 0x0400007E RID: 126
		private Guid _windowsSessionGuid;

		// Token: 0x0400007F RID: 127
		private readonly SubscriptionSettingsAgent _subscriptionSettingsAgent;

		// Token: 0x04000080 RID: 128
		private bool? _isAppUsageMonitorEnabled;

		// Token: 0x04000081 RID: 129
		private int? _flushCapacity;

		// Token: 0x04000082 RID: 130
		private int? _flushIntervalMinutes;

		// Token: 0x04000083 RID: 131
		private System.Timers.Timer _flushTimer;

		// Token: 0x04000084 RID: 132
		private const string PartialRelativePathToLogFolder = "\\AppData\\Local\\Lenovo\\ImController\\PluginData\\";

		// Token: 0x04000085 RID: 133
		private const string CsvFileSubFolder = "\\AppUsageMonitor";

		// Token: 0x04000086 RID: 134
		private const string CsvFileName = "ProcessLog.csv";

		// Token: 0x04000087 RID: 135
		private const string UsageMonitorIsEnabled = "Imc.Events.UsageMonitor.IsEnabled";

		// Token: 0x04000088 RID: 136
		private const string AppUsageMonitorFlushInterval = "Imc.Events.UsageMonitor.App.FlushIntervalMinutes";

		// Token: 0x04000089 RID: 137
		private const string AppUsageMonitorFlushCapacity = "Imc.Events.UsageMonitor.App.FlushCapacity";

		// Token: 0x0400008A RID: 138
		private static AppUsageMonitor _instance;
	}
}
