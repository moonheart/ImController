using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.ContractBroker;
using Lenovo.Modern.ImController.ContractBroker.Services;
using Lenovo.Modern.ImController.EventManager;
using Lenovo.Modern.ImController.EventManager.Services;
using Lenovo.Modern.ImController.ImClient.Services;
using Lenovo.Modern.ImController.ImClient.Services.Umdf;
using Lenovo.Modern.ImController.PluginManager.Services;
using Lenovo.Modern.ImController.Shared;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Services.Contracts.SystemInformation.AppTag;
using Lenovo.Modern.ImController.Shared.Telemetry;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.ImController.UpdateManager;
using Lenovo.Modern.ImController.UpdateManager.Services;
using Lenovo.Modern.Utilities.Patterns.Ioc;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.SystemContext.Interop;
using Lenovo.Modern.Utilities.Services.SystemContext.ProcessLauncher;
using Lenovo.Modern.Utilities.Services.SystemContext.Shared;
using Microsoft.Win32;

namespace Lenovo.Modern.ImController.Services
{
	// Token: 0x02000013 RID: 19
	internal class ServiceEventHandler : IServiceEventHandler
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00003DB0 File Offset: 0x00001FB0
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00003DB8 File Offset: 0x00001FB8
		public ServiceControlHandlerDelegate _eventMgrserviceControlHandler { get; set; }

		// Token: 0x06000042 RID: 66
		[DllImport("wtsapi32.dll", SetLastError = true)]
		private static extern int WTSEnumerateSessionsW(IntPtr hServer, [MarshalAs(UnmanagedType.U4)] int Reserved, [MarshalAs(UnmanagedType.U4)] int Version, ref IntPtr ppSessionInfo, [MarshalAs(UnmanagedType.U4)] ref int pCount);

		// Token: 0x06000043 RID: 67 RVA: 0x00003DC4 File Offset: 0x00001FC4
		public ServiceEventHandler(IntPtr scHandle)
		{
			this._scHandle = scHandle;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003E48 File Offset: 0x00002048
		public async Task<bool> HandleInitializeAsync()
		{
			Logger.Log(Logger.LogSeverity.Information, "HandleInitializeAsync: Entry");
			try
			{
				Bootstrap.RegisterComponents();
				this._instanceContainer = InstanceContainer.GetInstance();
				this._brokerResponseAgent = this._instanceContainer.Resolve<IBrokerResponseAgent>();
				this._brokerResponseAgent.Start();
			}
			catch (DeviceDriverMissingException ex)
			{
				Logger.Log(ex, "HandleInitializeAsync: Missing Driver Exception. Trying to restore one time");
				Task.Delay(60000).Wait();
				this.ReInstallImDriver();
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "HandleInitializeAsync: Fatal exception occured");
				return false;
			}
			IntPtr zero = IntPtr.Zero;
			Logger.Log(Logger.LogSeverity.Information, "HandleInitializeAsync: Checking if there is active console session");
			IntPtr zero2 = IntPtr.Zero;
			if (Authorization.GetSessionUserToken(ref zero2) || ImControllerService.RunAsApp)
			{
				Logger.Log(Logger.LogSeverity.Information, "HandleInitializeAsync: A user is already logged in, starting ResumeAsync");
				await this.HandleResumeAsync(EventHandlerReason.SystemStart);
			}
			else
			{
				Logger.Log(Logger.LogSeverity.Information, "HandleInitializeAsync: There is no active console session");
			}
			Logger.Log(Logger.LogSeverity.Information, "HandleInitializeAsync: Exit");
			return true;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003E90 File Offset: 0x00002090
		public async Task<bool> HandleUninitializeAsync()
		{
			Logger.Log(Logger.LogSeverity.Information, "HandleUninitializeAsync: Entry");
			this._brokerResponseAgent.Stop();
			Logger.Log(Logger.LogSeverity.Information, "HandleUninitializeAsync: Exit");
			return true;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003ED8 File Offset: 0x000020D8
		public async Task<bool> HandleSuspendAsync(EventHandlerReason reason)
		{
			Logger.Log(Logger.LogSeverity.Information, "HandleSuspendAsync: Enrty. reason={0}", new object[] { reason.ToString() });
			bool retVal = true;
			try
			{
				object lockToken = this._lockToken;
				lock (lockToken)
				{
					if (this._bigCancelSource != null && !this._bigCancelToken.IsCancellationRequested)
					{
						this._bigCancelSource.Cancel();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception in HandleSuspendAsync on token lock");
				return false;
			}
			try
			{
				await this._suspendResumeSem.WaitAsync();
				ImcEvent userEvent = EventFactory.CreateImcLifecycleEvent(EventFactory.Constants.LifcycleActivity.Suspend);
				EventLogger.GetInstance().LogEvent(userEvent);
				SessionTracker.GetInstance().StopSession();
				if (this._serviceResumed)
				{
					object lockToken = this._lockToken;
					lock (lockToken)
					{
						if (!this._bigCancelToken.IsCancellationRequested)
						{
							this._bigCancelSource.Cancel();
						}
					}
					if (this._eventManager != null)
					{
						this._eventManager.Stop(reason);
						this._eventManager = null;
						this._eventMgrserviceControlHandler = null;
						EventManagerFactory.DisposeEventManager();
					}
					EventLogger.GetInstance().Stop();
					if (this._updateManager != null)
					{
						this._updateManager.StopAndWait();
						this._updateManager = null;
						UpdateManagerFactory.DisposeUpdateManager();
					}
					PluginManager instance = PluginManager.GetInstance();
					if (instance != null)
					{
						instance.Stop(reason == EventHandlerReason.SystemSuspend, reason == EventHandlerReason.SystemShutdown);
					}
					ISubscriptionManager instance2 = SubscriptionManager.GetInstance(null);
					if (instance2 != null)
					{
						instance2.CleanupData();
					}
					IMachineInformationManager instance3 = MachineInformationManager.GetInstance();
					if (instance3 != null)
					{
						instance3.CleanupData();
					}
					IAppAndTagManager instance4 = AppAndTagManager.GetInstance();
					if (instance4 != null)
					{
						instance4.CleanupData();
					}
					if (instance != null)
					{
						if (reason == EventHandlerReason.SystemSuspend)
						{
							Logger.Log(Logger.LogSeverity.Information, "HandleSuspendAsync: Not calling pluginManager.CleanupData() since system is suspending");
						}
						else
						{
							instance.CleanupData();
						}
					}
					lockToken = this._lockToken;
					lock (lockToken)
					{
						if (this._bigCancelSource != null)
						{
							this._bigCancelSource.Dispose();
							this._bigCancelSource = null;
						}
					}
					this._serviceResumed = false;
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "Exception in HandleSuspendAsync");
				retVal = false;
			}
			finally
			{
				this._suspendResumeSem.Release();
			}
			Logger.Log(Logger.LogSeverity.Information, "ImController Service suspended");
			return retVal;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003F28 File Offset: 0x00002128
		public async Task<bool> HandleResumeAsync(EventHandlerReason reason)
		{
			Logger.Log(Logger.LogSeverity.Information, "HandleResumeAsync: Enrty. reason={0}", new object[] { reason.ToString() });
			bool retVal = true;
			try
			{
				EventLogger.GetInstance().HandleUserChange();
				if (ServiceEventHandler.IsWindowsInAuditBoot())
				{
					Logger.Log(Logger.LogSeverity.Information, "HandleResumeAsync: We are in AuditBoot. Configure service for delayed start");
					Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\sc.exe", "config imcontrollerservice start= delayed-auto");
					await Task.Delay(this._auditbootDelayMS);
				}
				while (!ServiceEventHandler.IsWindowsSetupComplete())
				{
					Logger.Log(Logger.LogSeverity.Information, "HandleResumeAsync: Service is not yet resumed because OOBE is not complete, waiting 5 seconds...");
					await Task.Delay(this._oobeWaitStepDelay);
				}
				PluginManager instance = PluginManager.GetInstance();
				if (reason == EventHandlerReason.UserLogon && instance != null)
				{
					instance.Stop(false, false);
					instance.CleanupData();
				}
				Task.Run(async delegate()
				{
					await Task.Delay(this._serviceReconfigDelay);
					if (!this._serviceReconfigured)
					{
						Logger.Log(Logger.LogSeverity.Information, "HandleResumeAsync: We are out of audit boot. Configure service for auto start");
						string arguments = ((!DeviceUtility.CheckIfNeedToDelayAutomaticStartup() || VantageUtility.IsVantageLaunchedAfterOOBERegistered()) ? "config imcontrollerservice start=auto" : "config imcontrollerservice start=delayed-auto");
						Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\sc.exe", arguments);
						this._serviceReconfigured = true;
					}
				});
				await this._suspendResumeSem.WaitAsync();
				SessionTracker.GetInstance().StartSession();
				ImcEvent userEvent = EventFactory.CreateImcLifecycleEvent(EventFactory.Constants.LifcycleActivity.Resume);
				EventLogger.GetInstance().LogEvent(userEvent);
				if (!this._serviceResumed)
				{
					object lockToken = this._lockToken;
					lock (lockToken)
					{
						this._bigCancelSource = new CancellationTokenSource();
						this._bigCancelToken = this._bigCancelSource.Token;
					}
					try
					{
						string loggedInUserSID = UserInformationProvider.GetLoggedInUserSID();
						Logger.Log(Logger.LogSeverity.Information, "IsOSReadyForUser.sid={0} userName={1}", new object[]
						{
							loggedInUserSID,
							UserInformationProvider.Instance.GetUserInformation().UserName
						});
						int count = 0;
						while (!UserUtility.IsOSReadyForUser(loggedInUserSID) && count < 60)
						{
							await Task.Delay(this._oobeWaitStepDelay, this._bigCancelToken);
							loggedInUserSID = UserInformationProvider.GetLoggedInUserSID();
							count++;
						}
						Logger.Log(Logger.LogSeverity.Information, "IsOSReadyForUser.Exit with readyness={0} for sid={1}", new object[]
						{
							(count == 60) ? "false" : "true",
							loggedInUserSID
						});
						try
						{
							using (Registry.LocalMachine.CreateSubKey(Constants.AppReadinessImcPath + "\\" + loggedInUserSID))
							{
							}
						}
						catch (Exception)
						{
						}
					}
					catch (Exception ex)
					{
						Logger.Log(Logger.LogSeverity.Critical, "Exception in IsOSReadyForUser: {0}", new object[] { ex.Message });
					}
					if (!this._bigCancelToken.IsCancellationRequested)
					{
						this._updateInProgressEvent.Set();
						Task pendingUpdateTask = Task.Factory.StartNew(delegate()
						{
							this._updateManager = UpdateManagerFactory.GetUpdateManagerInstance();
							if (this._updateManager != null)
							{
								this._updateManager.ApplyPendingUpdates();
								this._updateInProgressEvent.Reset();
							}
						}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
						pendingUpdateTask.Wait(this._pendingUpdateMinDelay, this._bigCancelToken);
						if (!this._bigCancelToken.IsCancellationRequested)
						{
							EventLogger.GetInstance().StartOrResume(this._bigCancelToken);
						}
						if (!this._bigCancelToken.IsCancellationRequested)
						{
							this._firstContractBrokerRequestEvent.Reset();
							this._contractBroker = ContractBrokerFactory.GetContractBrokerInstance();
							if (this._contractBroker != null)
							{
								this._contractBroker.RequestEvent = this._firstContractBrokerRequestEvent;
								this._contractBroker.UpdateInProgressEvent = this._updateInProgressEvent;
								await this._contractBroker.ResumeAsync(this._bigCancelToken);
							}
						}
						pendingUpdateTask.Wait(this._pendingUpdateMaxDelay, this._bigCancelToken);
						this._updateInProgressEvent.Reset();
						if (!this._bigCancelToken.IsCancellationRequested)
						{
							ServiceControlHandlerDelegate eventMgrserviceControlHandler = this._eventMgrserviceControlHandler;
							this._eventManager = EventManagerFactory.GetEventManagerInstance(this._scHandle, ref eventMgrserviceControlHandler);
							this._eventMgrserviceControlHandler = eventMgrserviceControlHandler;
							if (this._eventManager != null)
							{
								await this._eventManager.InitializeAsync(this._bigCancelToken, this._firstContractBrokerRequestEvent, reason);
							}
							else
							{
								Logger.Log(Logger.LogSeverity.Warning, "_eventManager is null");
							}
						}
						if (!this._bigCancelToken.IsCancellationRequested && this._updateManager != null)
						{
							this._updateManager.Start(this._bigCancelToken);
						}
						if (!this._bigCancelToken.IsCancellationRequested && !Directory.Exists(Environment.ExpandEnvironmentVariables(Constants.SharedFolderTempLocation)))
						{
							this._firstContractBrokerRequestEvent.Set();
						}
						this._serviceResumed = true;
						pendingUpdateTask = null;
					}
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "Exception in HandleResumeAsync");
				retVal = false;
			}
			finally
			{
				this._suspendResumeSem.Release();
			}
			Logger.Log(Logger.LogSeverity.Information, "HandleResumeAsync: Service Resumed");
			return retVal;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003F78 File Offset: 0x00002178
		public static bool IsWindowsSetupComplete()
		{
			string text = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Setup\\State", "ImageState", string.Empty);
			Logger.Log(Logger.LogSeverity.Information, "Image State (in IsWindowsSetupComplete): {0}", new object[] { text });
			return "IMAGE_STATE_COMPLETE".Equals(text);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003FC0 File Offset: 0x000021C0
		public static bool IsWindowsInAuditBoot()
		{
			int num = 0;
			try
			{
				int num2 = (int)Registry.GetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\Setup\\Status", "AuditBoot", 0);
				Logger.Log(Logger.LogSeverity.Information, "AuditBoot flag value in registry: (in IsWindowsInAuditBoot): {0}", new object[] { num2 });
				num = num2;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception reading audit boot flag");
			}
			Logger.Log(Logger.LogSeverity.Information, "AuditBoot flag (in IsWindowsInAuditBoot): {0}", new object[] { num });
			return num != 0;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00004044 File Offset: 0x00002244
		private static bool IsServiceConfiguredForDelayedStart()
		{
			bool result = false;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Services\\ImcontrollerService"))
				{
					if (registryKey != null)
					{
						result = (int)registryKey.GetValue("DelayedAutoStart") != 0;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception reading delay start flag");
			}
			return result;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000040B4 File Offset: 0x000022B4
		private void ReInstallImDriver()
		{
			List<DirectoryInfo> list = (from f in new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\system32\\DriverStore\\FileRepository").GetDirectories("imdriver.inf*")
				orderby f.LastWriteTime descending
				select f).ToList<DirectoryInfo>();
			if (list != null && list.Any<DirectoryInfo>())
			{
				string str = list.ElementAt(0).FullName + "\\ImDriver.inf";
				string text = list.ElementAt(0).FullName + "\\x64\\ImController.InfInstaller\\x64_ImController.InfInstaller_ImController.InfInstaller.exe";
				if (!Environment.Is64BitOperatingSystem)
				{
					text = list.ElementAt(0).FullName + "\\x86\\ImController.InfInstaller\\x86_ImController.InfInstaller_ImController.InfInstaller.exe";
				}
				Logger.Log(Logger.LogSeverity.Information, "ReInstallImDriver: Reinstalling ImDriver now. DirverStorePath is {0}", new object[] { list.ElementAt(0).FullName });
				ProcessStartInfo processStartInfo = new ProcessStartInfo
				{
					FileName = text,
					Arguments = "-install " + str,
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden,
					UseShellExecute = false
				};
				if (processStartInfo != null)
				{
					try
					{
						Logger.Log(Logger.LogSeverity.Information, "ReInstallImDriver: Starting IMC installer process now.  File: {0}, Args: {1}", new object[] { processStartInfo.FileName, processStartInfo.Arguments });
						if (SystemProcessLauncher.Instance.LaunchSystemProcessInUserSession(text, processStartInfo.Arguments, Path.GetDirectoryName(text), true) == null)
						{
							Process.Start(processStartInfo);
						}
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "ReInstallImDriver: Can't start installer for service self auto-update");
					}
				}
				Logger.Log(Logger.LogSeverity.Information, "ReInstallImDriver: Exiting");
			}
		}

		// Token: 0x04000040 RID: 64
		private EventManager _eventManager;

		// Token: 0x04000041 RID: 65
		private ContractBroker _contractBroker;

		// Token: 0x04000042 RID: 66
		private IUpdateManager _updateManager;

		// Token: 0x04000043 RID: 67
		private CancellationTokenSource _bigCancelSource;

		// Token: 0x04000044 RID: 68
		private CancellationToken _bigCancelToken;

		// Token: 0x04000045 RID: 69
		private object _lockToken = new object();

		// Token: 0x04000046 RID: 70
		private ManualResetEventSlim _firstContractBrokerRequestEvent = new ManualResetEventSlim(false);

		// Token: 0x04000047 RID: 71
		private ManualResetEventSlim _updateInProgressEvent = new ManualResetEventSlim(true);

		// Token: 0x04000048 RID: 72
		private IntPtr _scHandle;

		// Token: 0x04000049 RID: 73
		private SemaphoreSlim _suspendResumeSem = new SemaphoreSlim(1, 1);

		// Token: 0x0400004A RID: 74
		private bool _serviceResumed;

		// Token: 0x0400004B RID: 75
		private IBrokerResponseAgent _brokerResponseAgent;

		// Token: 0x0400004C RID: 76
		private InstanceContainer _instanceContainer;

		// Token: 0x0400004E RID: 78
		private readonly int _auditbootDelayMS = 5000;

		// Token: 0x0400004F RID: 79
		private readonly int _oobeWaitStepDelay = 5000;

		// Token: 0x04000050 RID: 80
		private readonly int _pendingUpdateMinDelay = 10000;

		// Token: 0x04000051 RID: 81
		private readonly int _pendingUpdateMaxDelay = 180000;

		// Token: 0x04000052 RID: 82
		private readonly int _serviceReconfigDelay = 180000;

		// Token: 0x04000053 RID: 83
		private bool _serviceReconfigured;
	}
}
