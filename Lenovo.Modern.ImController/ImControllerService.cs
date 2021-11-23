using System;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.ImClient.Utilities;
using Lenovo.Modern.ImController.Services;
using Lenovo.Modern.ImController.Shared;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Telemetry;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.SystemContext.Shared;

namespace Lenovo.Modern.ImController
{
	// Token: 0x02000004 RID: 4
	internal class ImControllerService : ServiceBase
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x000020A4 File Offset: 0x000002A4
		// (set) Token: 0x06000003 RID: 3 RVA: 0x000020AB File Offset: 0x000002AB
		public static bool RunAsApp { get; set; }

		// Token: 0x06000004 RID: 4 RVA: 0x000020B4 File Offset: 0x000002B4
		public ImControllerService()
		{
			this.InitializeComponent();
			Logger.Setup(new Logger.Configuration
			{
				LogIdentifier = "ImController.Service",
				FileNameEnding = null,
				FileSizeRollOverKb = 3072,
				IsEnabled = new bool?(true)
			});
			ExternalLogger.Configure(new Action<string>(this.LogFunction));
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002128 File Offset: 0x00000328
		public void LogFunction(string text)
		{
			Logger.Log(Logger.LogSeverity.Information, text);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002134 File Offset: 0x00000334
		internal async Task InitSvcForRunasApp()
		{
			ImControllerService.RunAsApp = true;
			this.OnStart(null);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000217C File Offset: 0x0000037C
		internal async Task SvcCtrlForRunAsApp(int control, int eventType)
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021BC File Offset: 0x000003BC
		protected override void OnStart(string[] args)
		{
			try
			{
				base.RequestAdditionalTime(300000);
			}
			catch (Exception)
			{
				Logger.Log(Logger.LogSeverity.Information, "Exception occured while requesting additional time");
			}
			Logger.Log(Logger.LogSeverity.Information, "ImController OnStart Entered. Version: " + Constants.ImControllerVersion);
			base.OnStart(args);
			if (!ImControllerService.RunAsApp)
			{
				this._svcCtrlCallback = new Win32.ServiceControlHandlerEx(this.ServiceControlHandler);
				this._scHandle = Win32.RegisterServiceCtrlHandlerEx(base.ServiceName, this._svcCtrlCallback, IntPtr.Zero);
			}
			this._serviceEventHandler = new ServiceEventHandler(this._scHandle);
			ServiceEventBroker.GetInstance().AddEventHandler(this._serviceEventHandler);
			this._userSIDForLastResume = UserInformationProvider.GetLoggedInUserSID();
			Task.Factory.StartNew<Task>(async delegate()
			{
				try
				{
					SessionTracker.GetInstance().StartProcess();
					ImcEvent userEvent = EventFactory.CreateImcLifecycleEvent(EventFactory.Constants.LifcycleActivity.Start);
					EventLogger.GetInstance().LogEvent(userEvent);
					TaskAwaiter<bool> taskAwaiter = ServiceEventBroker.GetInstance().HandleInitializeAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (!taskAwaiter.GetResult())
					{
						Logger.Log(Logger.LogSeverity.Critical, "Critical error in service startup");
						base.Stop();
					}
					else
					{
						this._initializedEvent.Set();
						this.AddELWatcher("Microsoft-Windows-Kernel-Power", "System", ImControllerService.ModernStandbyEventID, ImControllerService.ModernResumeEventID);
					}
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "ImService: Exception while handling event broker initialize");
				}
			}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
			Logger.Log(Logger.LogSeverity.Information, "ImController OnStart Finished");
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022A0 File Offset: 0x000004A0
		protected override void OnStop()
		{
			try
			{
				if (this._elWatcher != null)
				{
					this._elWatcher.Enabled = false;
					this._elWatcher.EventRecordWritten -= this.OnEventRecordWritten;
					this._elWatcher = null;
				}
				ServiceEventBroker.GetInstance().HandleSuspendAsync(EventHandlerReason.SystemShutdown).Wait();
				ServiceEventBroker.GetInstance().HandleUninitializeAsync().Wait();
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "ImController Service: exception in OnStop");
			}
			base.OnStop();
			SessionTracker.GetInstance().StopProcess();
			ImcEvent userEvent = EventFactory.CreateImcLifecycleEvent(EventFactory.Constants.LifcycleActivity.Stop);
			EventLogger.GetInstance().LogEvent(userEvent);
			Logger.Log(Logger.LogSeverity.Information, "ImController Service stopped");
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000234C File Offset: 0x0000054C
		protected override void OnCustomCommand(int command)
		{
			base.OnCustomCommand(command);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002358 File Offset: 0x00000558
		private void AddELWatcher(string source, string log, int id1 = -1, int id2 = -1)
		{
			if (source == log && log == null)
			{
				return;
			}
			if (this._elWatcher != null)
			{
				Logger.Log(Logger.LogSeverity.Information, "AddELWatcher: Returning since EventLog watcher is already started");
				return;
			}
			try
			{
				string text = string.Concat(new object[] { "<QueryList><Query Id='0' Path='", log, "'><Select Path='", log, "'>*[System[Provider[@Name='", source, "']]] and *[System[EventID=", id1, "]]</Select></Query></QueryList>" });
				if (id1 == -1)
				{
					text = string.Concat(new string[] { "<QueryList><Query Id='0' Path='", log, "'><Select Path='", log, "'>*[System[Provider[@Name='", source, "']]]</Select></Query></QueryList>" });
				}
				else if (id2 != -1)
				{
					text = string.Concat(new object[]
					{
						"<QueryList><Query Id='0' Path='", log, "'><Select Path='", log, "'>*[System[Provider[@Name='", source, "']]] and *[System[(EventID=", id1, ") or (EventID=", id2,
						")]]</Select></Query></QueryList>"
					});
				}
				Logger.Log(Logger.LogSeverity.Information, "AddELWatcher: Adding Event Log Watcher with query: {0}", new object[] { text });
				EventLogQuery eventQuery = new EventLogQuery(log, PathType.LogName, text);
				this._elWatcher = new EventLogWatcher(eventQuery);
				this._elWatcher.EventRecordWritten += this.OnEventRecordWritten;
				this._elWatcher.Enabled = true;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "AddELWatcher: Exception occured");
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000024EC File Offset: 0x000006EC
		private void ServicePowerEventDispatacher(bool suspendHandler, EventHandlerReason reason)
		{
			this._initializedEvent.Wait(30000);
			Logger.Log(Logger.LogSeverity.Information, "ServicePowerEventDispatacher- {0} : SysStandby-{1}", new object[]
			{
				reason.ToString(),
				this._isInStandby
			});
			if (suspendHandler)
			{
				ServiceEventBroker.GetInstance().HandleSuspendAsync(reason);
				return;
			}
			this._userSIDForLastResume = UserInformationProvider.GetLoggedInUserSID();
			ServiceEventBroker.GetInstance().HandleResumeAsync(reason);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002560 File Offset: 0x00000760
		private void OnEventRecordWritten(object sender, EventRecordWrittenEventArgs e)
		{
			Logger.Log(Logger.LogSeverity.Information, "EventRecordWritten: Event Log entry occured");
			try
			{
				if (e != null && e.EventRecord != null)
				{
					if (e.EventRecord.Id == ImControllerService.ModernStandbyEventID)
					{
						this._isInStandby = true;
						Logger.Log(Logger.LogSeverity.Information, "EventRecordWritten: Modern standby occured");
						Logger.Log(Logger.LogSeverity.Information, "Power is going down, suspending service");
						this.ServicePowerEventDispatacher(true, EventHandlerReason.SystemSuspend);
					}
					if (e.EventRecord.Id == ImControllerService.ModernResumeEventID)
					{
						this._isInStandby = false;
						Logger.Log(Logger.LogSeverity.Information, "EventRecordWritten: Modern resume occured");
						Logger.Log(Logger.LogSeverity.Information, "Power is up, resuming service");
						this.ServicePowerEventDispatacher(false, EventHandlerReason.SystemResume);
						if (this._serviceEventHandler != null && this._serviceEventHandler._eventMgrserviceControlHandler != null)
						{
							try
							{
								Logger.Log(Logger.LogSeverity.Information, "Forwarding Resume event to Event Manager");
								this._serviceEventHandler._eventMgrserviceControlHandler(13, 18, IntPtr.Zero, IntPtr.Zero);
							}
							catch (Exception ex)
							{
								Logger.Log(ex, "ImController Service: exception calling _eventMgrserviceControlHandler");
							}
						}
						Logger.Log(Logger.LogSeverity.Information, "Exiting POWER_EVENT_RESUME handler");
					}
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "AddELWatcher: Exception occured");
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000267C File Offset: 0x0000087C
		private int ServiceControlHandler(int control, int eventType, IntPtr eventData, IntPtr context)
		{
			ImControllerService.<>c__DisplayClass23_0 CS$<>8__locals1 = new ImControllerService.<>c__DisplayClass23_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.control = control;
			CS$<>8__locals1.eventType = eventType;
			CS$<>8__locals1.context = context;
			Logger.Log(Logger.LogSeverity.Information, "ServiceControlHandler entered: control = {0}, eventType = {1}", new object[] { CS$<>8__locals1.control, CS$<>8__locals1.eventType });
			if (1 == CS$<>8__locals1.control || 5 == CS$<>8__locals1.control)
			{
				base.Stop();
			}
			else
			{
				if (13 == CS$<>8__locals1.control)
				{
					if (4 == CS$<>8__locals1.eventType)
					{
						this._isInStandby = true;
						Logger.Log(Logger.LogSeverity.Information, "Power is going down, suspending service");
						this.ServicePowerEventDispatacher(true, EventHandlerReason.SystemSuspend);
						goto IL_2D3;
					}
					if (18 == CS$<>8__locals1.eventType)
					{
						this._isInStandby = false;
						Task.Run(delegate()
						{
							ImControllerService.<>c__DisplayClass23_0.<<ServiceControlHandler>b__0>d <<ServiceControlHandler>b__0>d;
							<<ServiceControlHandler>b__0>d.<>4__this = CS$<>8__locals1;
							<<ServiceControlHandler>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<ServiceControlHandler>b__0>d.<>1__state = -1;
							AsyncTaskMethodBuilder <>t__builder = <<ServiceControlHandler>b__0>d.<>t__builder;
							<>t__builder.Start<ImControllerService.<>c__DisplayClass23_0.<<ServiceControlHandler>b__0>d>(ref <<ServiceControlHandler>b__0>d);
							return <<ServiceControlHandler>b__0>d.<>t__builder.Task;
						});
						goto IL_2D3;
					}
					if (32787 != CS$<>8__locals1.eventType || this._serviceEventHandler == null || this._serviceEventHandler._eventMgrserviceControlHandler == null)
					{
						goto IL_2D3;
					}
					try
					{
						this._serviceEventHandler._eventMgrserviceControlHandler(CS$<>8__locals1.control, CS$<>8__locals1.eventType, eventData, CS$<>8__locals1.context);
						goto IL_2D3;
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "ImController Service: exception calling _eventMgrserviceControlHandler");
						goto IL_2D3;
					}
				}
				if (14 == CS$<>8__locals1.control && (5 == CS$<>8__locals1.eventType || 6 == CS$<>8__locals1.eventType || 1 == CS$<>8__locals1.eventType))
				{
					ImControllerService.<>c__DisplayClass23_1 CS$<>8__locals2 = new ImControllerService.<>c__DisplayClass23_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.win32wtsSessionNotification = (Win32.WTSSESSION_NOTIFICATION)Marshal.PtrToStructure(eventData, typeof(Win32.WTSSESSION_NOTIFICATION));
					CS$<>8__locals2.isLogoff = 6 == CS$<>8__locals2.CS$<>8__locals1.eventType;
					CS$<>8__locals2.isUserSwitch = 1 == CS$<>8__locals2.CS$<>8__locals1.eventType;
					CS$<>8__locals2.isLogon = 5 == CS$<>8__locals2.CS$<>8__locals1.eventType;
					CS$<>8__locals2.userSid = string.Empty;
					if (CS$<>8__locals2.isUserSwitch | CS$<>8__locals2.isLogon)
					{
						try
						{
							CS$<>8__locals2.userSid = UserInformationProvider.GetLoggedInUserSID();
							Logger.Log(Logger.LogSeverity.Information, "User Switch: sid is {0}", new object[] { CS$<>8__locals2.userSid });
						}
						catch (Exception ex2)
						{
							Logger.Log(ex2, "User Switch: Exception occured");
						}
					}
					if (CS$<>8__locals2.isUserSwitch)
					{
						if (string.IsNullOrWhiteSpace(CS$<>8__locals2.userSid))
						{
							Logger.Log(Logger.LogSeverity.Information, "User Switch: No user is logged in. Not handling WTS_CONSOLE_CONNECT anymore");
							return 0;
						}
						if (string.Compare("s-1-5-18", CS$<>8__locals2.userSid, StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							Logger.Log(Logger.LogSeverity.Information, "User Switch. Its System SID: No user is logged in. Not handling WTS_CONSOLE_CONNECT anymore");
							return 0;
						}
					}
					if (CS$<>8__locals2.isLogoff)
					{
						Logger.Log(Logger.LogSeverity.Information, "User Logoff. Stop session");
						SessionTracker.GetInstance().StopSession();
					}
					Task.Factory.StartNew<Task>(delegate()
					{
						ImControllerService.<>c__DisplayClass23_1.<<ServiceControlHandler>b__3>d <<ServiceControlHandler>b__3>d;
						<<ServiceControlHandler>b__3>d.<>4__this = CS$<>8__locals2;
						<<ServiceControlHandler>b__3>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<ServiceControlHandler>b__3>d.<>1__state = -1;
						AsyncTaskMethodBuilder <>t__builder = <<ServiceControlHandler>b__3>d.<>t__builder;
						<>t__builder.Start<ImControllerService.<>c__DisplayClass23_1.<<ServiceControlHandler>b__3>d>(ref <<ServiceControlHandler>b__3>d);
						return <<ServiceControlHandler>b__3>d.<>t__builder.Task;
					}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
				}
				else if (this._serviceEventHandler != null && this._serviceEventHandler._eventMgrserviceControlHandler != null)
				{
					try
					{
						this._serviceEventHandler._eventMgrserviceControlHandler(CS$<>8__locals1.control, CS$<>8__locals1.eventType, eventData, CS$<>8__locals1.context);
					}
					catch (Exception ex3)
					{
						Logger.Log(ex3, "ImController Service: exception calling _eventMgrserviceControlHandler");
					}
				}
			}
			IL_2D3:
			if (CS$<>8__locals1.control != 129 && CS$<>8__locals1.control != 130 && CS$<>8__locals1.control != 131)
			{
				if (CS$<>8__locals1.control == 132)
				{
					Logger.RefreshLogSeverity();
				}
				else if (CS$<>8__locals1.control == 133)
				{
					Logger.Log(Logger.LogSeverity.Information, "ImController Service: Control code 133 to cleanup temp folder received");
					Task.Run(delegate()
					{
						Task.Delay(5000).Wait();
						try
						{
							string str = Environment.ExpandEnvironmentVariables(Constants.ImControllerCoreDataFolder + "\\temp");
							string path = str + "\\installer";
							if (Directory.Exists(path))
							{
								Directory.Delete(path, true);
							}
							path = str + "\\install";
							if (Directory.Exists(path))
							{
								Directory.Delete(path, true);
							}
						}
						catch (Exception)
						{
						}
					});
				}
				else if (CS$<>8__locals1.control >= 128 && CS$<>8__locals1.control <= 255)
				{
					Task.Run(delegate()
					{
						try
						{
							CS$<>8__locals1.<>4__this.OnCustomCommand(CS$<>8__locals1.control);
						}
						catch (Exception ex4)
						{
							Logger.Log(ex4, "Exception in OnCustomCommand");
						}
					});
				}
			}
			return 0;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002A34 File Offset: 0x00000C34
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002A53 File Offset: 0x00000C53
		private void InitializeComponent()
		{
			this.components = new Container();
			base.ServiceName = Constants.ImControllerServiceName;
			base.CanHandlePowerEvent = true;
			base.CanHandleSessionChangeEvent = true;
			base.CanShutdown = true;
		}

		// Token: 0x04000003 RID: 3
		private Win32.ServiceControlHandlerEx _svcCtrlCallback;

		// Token: 0x04000004 RID: 4
		private ServiceEventHandler _serviceEventHandler;

		// Token: 0x04000005 RID: 5
		private IntPtr _scHandle;

		// Token: 0x04000007 RID: 7
		private EventLogWatcher _elWatcher;

		// Token: 0x04000008 RID: 8
		private static readonly int ModernStandbyEventID = 506;

		// Token: 0x04000009 RID: 9
		private static readonly int ModernResumeEventID = 507;

		// Token: 0x0400000A RID: 10
		private bool _isInStandby;

		// Token: 0x0400000B RID: 11
		private ManualResetEventSlim _initializedEvent = new ManualResetEventSlim(false);

		// Token: 0x0400000C RID: 12
		private string _userSIDForLastResume = string.Empty;

		// Token: 0x0400000D RID: 13
		private IContainer components;
	}
}
