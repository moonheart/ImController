using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.SystemContext.Shared;
using Microsoft.Win32;
using Windows.ApplicationModel;
using Windows.Management.Deployment;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x0200001F RID: 31
	public class UapInstallMonitor
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00005BDA File Offset: 0x00003DDA
		public static UapInstallMonitor GetInstance()
		{
			UapInstallMonitor result;
			if ((result = UapInstallMonitor._instance) == null)
			{
				result = (UapInstallMonitor._instance = new UapInstallMonitor());
			}
			return result;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005BF0 File Offset: 0x00003DF0
		private UapInstallMonitor()
		{
			this._appStatusList = new ConcurrentBag<UapInstallMonitor.AppStatus>();
			this._monitoredPfnList = new ConcurrentDictionary<string, bool>();
			this._eventTerminate = new ManualResetEvent(false);
			this._appStatusSemaphore = new SemaphoreSlim(1);
			this._RegUnregSemaphore = new SemaphoreSlim(1);
			this._unNotifiedAppStatusQ = new ConcurrentQueue<UapInstallMonitor.AppStatus>();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00005C48 File Offset: 0x00003E48
		public void Reset()
		{
			this._eventTerminate.Set();
			this._appMonStarted = false;
			this._appStatusList = new ConcurrentBag<UapInstallMonitor.AppStatus>();
			this._monitoredPfnList = new ConcurrentDictionary<string, bool>();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005C73 File Offset: 0x00003E73
		public void ResetTerminateEvent()
		{
			this._eventTerminate.Reset();
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000AD RID: 173 RVA: 0x00005C84 File Offset: 0x00003E84
		// (remove) Token: 0x060000AE RID: 174 RVA: 0x00005CBC File Offset: 0x00003EBC
		public event UapInstallMonitor.AppStatusChanged AppStatusChangedEvent;

		// Token: 0x060000AF RID: 175 RVA: 0x00005CF4 File Offset: 0x00003EF4
		public void RegisterMonitor(string pfn)
		{
			UapInstallMonitor.<>c__DisplayClass16_0 CS$<>8__locals1 = new UapInstallMonitor.<>c__DisplayClass16_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pfn = pfn;
			string loggedInUserSID = UserInformationProvider.GetLoggedInUserSID();
			if (string.Compare("s-1-5-18", loggedInUserSID, StringComparison.InvariantCultureIgnoreCase) != 0)
			{
				Task.Factory.StartNew<Task>(delegate()
				{
					UapInstallMonitor.<>c__DisplayClass16_0.<<RegisterMonitor>b__0>d <<RegisterMonitor>b__0>d;
					<<RegisterMonitor>b__0>d.<>4__this = CS$<>8__locals1;
					<<RegisterMonitor>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<RegisterMonitor>b__0>d.<>1__state = -1;
					AsyncTaskMethodBuilder <>t__builder = <<RegisterMonitor>b__0>d.<>t__builder;
					<>t__builder.Start<UapInstallMonitor.<>c__DisplayClass16_0.<<RegisterMonitor>b__0>d>(ref <<RegisterMonitor>b__0>d);
					return <<RegisterMonitor>b__0>d.<>t__builder.Task;
				}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
			}
			this.IsAppInstalled(CS$<>8__locals1.pfn, false);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005D5C File Offset: 0x00003F5C
		public bool IsAppInstalled(string pfn, bool checkGlobalInstallState)
		{
			string sid = UserInformationProvider.GetLoggedInUserSID();
			this._appStatusSemaphore.Wait();
			UapInstallMonitor.AppStatus appStatus = this._appStatusList.FirstOrDefault((UapInstallMonitor.AppStatus s) => s.Pfn.Equals(checkGlobalInstallState ? pfn : (pfn + sid), StringComparison.InvariantCultureIgnoreCase));
			this._appStatusSemaphore.Release();
			if (appStatus != null)
			{
				return appStatus.InstallState;
			}
			bool flag = this.IsPackageInstalledForCurrentUser(pfn);
			Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.IsPackageInstalledForCurrentUser: Using PackageManager api for current user,install: {0} pfn: {1}", new object[]
			{
				flag.ToString(),
				pfn
			});
			if (!flag)
			{
				flag = UapInstallMonitor.GetInstallUsingProvisionedApis(pfn);
			}
			bool flag2 = this.IsPackageInstalled(pfn);
			this.UpdateAppStatus(pfn, flag, flag2);
			this.RegUnregApp(sid, pfn, flag);
			if (checkGlobalInstallState)
			{
				flag = flag2;
			}
			return flag;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005E3C File Offset: 0x0000403C
		public Version GetInstalledAppVersion(string pfn)
		{
			Version result = new Version("0.0.0.0");
			string sid = UserInformationProvider.GetLoggedInUserSID();
			this._appStatusSemaphore.Wait();
			try
			{
				UapInstallMonitor.AppStatus appStatus = this._appStatusList.FirstOrDefault((UapInstallMonitor.AppStatus s) => s.Pfn.Equals(pfn + sid, StringComparison.InvariantCultureIgnoreCase));
				if (appStatus != null && appStatus.AppVersion != null)
				{
					result = appStatus.AppVersion;
					Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.GetInstalledAppVersion: pfn={0}. got version now {1}", new object[]
					{
						pfn,
						appStatus.AppVersion.ToString()
					});
				}
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Critical, "UapInstallMonitor.GetInstalledAppVersion: Exception: {0}", new object[] { ex.Message });
			}
			this._appStatusSemaphore.Release();
			return result;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005F0C File Offset: 0x0000410C
		public void TriggerPendingAppStatusNotificationsAsync()
		{
			Task.Factory.StartNew<Task>(async delegate()
			{
				Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.TriggerPendingAppStatusNotificationsAsync: Invoked.");
				if (this._unNotifiedAppStatusQ != null && this._unNotifiedAppStatusQ.Any<UapInstallMonitor.AppStatus>())
				{
					while (!this._unNotifiedAppStatusQ.IsEmpty)
					{
						UapInstallMonitor.AppStatus appStatus = null;
						if (this._unNotifiedAppStatusQ.TryDequeue(out appStatus) && this.AppStatusChangedEvent != null)
						{
							Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.TriggerPendingAppStatusNotificationsAsync: Raising the pending event with Pfn={0} and InstallState={1}.", new object[] { appStatus.Pfn, appStatus.InstallState });
							this.AppStatusChangedEvent(appStatus);
						}
					}
				}
			}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005F30 File Offset: 0x00004130
		private bool IsAppInstallRegistered(string sid, string pfn)
		{
			bool result = false;
			string name = string.Format("{0}\\{1}", Constants.IMCRegistryKeyPathForUAPApps, pfn + sid);
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name))
			{
				if (registryKey != null)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005F84 File Offset: 0x00004184
		private void RegUnregApp(string sid, string pfn, bool isInstalled)
		{
			Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.RegUnregApp: Calling with pfn={0}. App isInstalled={1}", new object[]
			{
				pfn,
				isInstalled.ToString()
			});
			string text = string.Format("{0}\\{1}", Constants.IMCRegistryKeyPathForUAPApps, pfn + sid);
			this._RegUnregSemaphore.Wait();
			if (isInstalled)
			{
				using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(Constants.IMCRegistryKeyPathForUAPApps))
				{
					if (registryKey != null)
					{
						using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey(text))
						{
							if (registryKey2 == null)
							{
								using (Registry.LocalMachine.CreateSubKey(text))
								{
									Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.RegUnregApp: Registered UAP app at RegKey={0} and call to raise install event with pfn={1}.", new object[] { text, pfn });
									this.HandleAppStatusChange(new UapInstallMonitor.AppStatus(pfn, isInstalled, new Version("0.0.0.0")));
								}
							}
						}
					}
					goto IL_123;
				}
			}
			using (RegistryKey registryKey4 = Registry.LocalMachine.OpenSubKey(text))
			{
				if (registryKey4 != null)
				{
					Registry.LocalMachine.DeleteSubKey(text);
					Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.RegUnregApp: Unregistered UAP app at RegKey={0} and call to raise uninstall event with pfn={1}.", new object[] { text, pfn });
					this.HandleAppStatusChange(new UapInstallMonitor.AppStatus(pfn, isInstalled, new Version("0.0.0.0")));
				}
			}
			IL_123:
			this._RegUnregSemaphore.Release();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000060F4 File Offset: 0x000042F4
		private void RegisterPfnMonitor(string pfn)
		{
			if (!this._monitoredPfnList.ContainsKey(pfn))
			{
				try
				{
					string loggedInUserSID = UserInformationProvider.GetLoggedInUserSID();
					bool flag = this.IsAppInstallRegistered(loggedInUserSID, pfn);
					bool flag2 = this.IsPackageInstalledForCurrentUser(pfn);
					Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.IsPackageInstalledForCurrentUser: Using PackageManager api for current user,install: {0} pfn: {1}", new object[]
					{
						flag2.ToString(),
						pfn
					});
					if (flag != flag2)
					{
						this._monitoredPfnList.TryAdd(pfn, flag);
					}
					else
					{
						this._monitoredPfnList.TryAdd(pfn, flag2);
					}
					if (!this._appMonStarted)
					{
						this._appMonStarted = true;
						this.StartInstallMonitor();
					}
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "Exception occured in RegisterPfnMonitor");
				}
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000619C File Offset: 0x0000439C
		private void HandleAppStatusChange(UapInstallMonitor.AppStatus status)
		{
			try
			{
				if (this.AppStatusChangedEvent != null)
				{
					this.AppStatusChangedEvent(status);
				}
				else if (!this._unNotifiedAppStatusQ.Contains(status))
				{
					Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.HandleAppStatusChange: EventManager is not intialized to recieve event. So holding the event with Pfn={0} and InstallState={1}.", new object[] { status.Pfn, status.InstallState });
					this._unNotifiedAppStatusQ.Enqueue(status);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00006218 File Offset: 0x00004418
		private bool IsPackageInstalledForCurrentUser(string packageFamilyName)
		{
			bool result = false;
			try
			{
				if (string.IsNullOrEmpty(packageFamilyName))
				{
					return result;
				}
				string loggedInUserSID = UserInformationProvider.GetLoggedInUserSID();
				result = new PackageManager().FindPackagesForUser(loggedInUserSID, packageFamilyName).Any<Package>();
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Critical, "UapInstallMonitor.IsPackageInstalledForUser : Exception occured while getting package details.: {0}", new object[] { ex.Message });
			}
			return result;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00006280 File Offset: 0x00004480
		private static bool GetInstallUsingProvisionedApis(string packageFamilyName)
		{
			bool result = false;
			try
			{
				PackageManager packageManager = new PackageManager();
				IEnumerable<Package> enumerable = packageManager.FindPackages(packageFamilyName);
				string loggedInUserSID = UserInformationProvider.GetLoggedInUserSID();
				foreach (Package package in enumerable)
				{
					foreach (PackageUserInformation packageUserInformation in packageManager.FindUsers(package.Id.FullName))
					{
						if (string.Compare("s-1-5-18", loggedInUserSID, StringComparison.InvariantCultureIgnoreCase) != 0 && string.Compare(packageUserInformation.UserSecurityId, loggedInUserSID, StringComparison.InvariantCultureIgnoreCase) == 0 && packageUserInformation.InstallState != null)
						{
							Logger.Log(Logger.LogSeverity.Information, "GetInstallUsingProvisionedApis: status: {0} sid:{1} pfn: {2}", new object[]
							{
								packageUserInformation.InstallState.ToString(),
								packageUserInformation.UserSecurityId,
								packageFamilyName
							});
							result = true;
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Critical, "GetInstallUsingProvisionedApis: Exception occured while getting package details. Exception: {0}", new object[] { ex.Message });
			}
			return result;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000063B4 File Offset: 0x000045B4
		private bool IsPackageInstalled(string packageFamilyName)
		{
			bool result = false;
			try
			{
				if (string.IsNullOrEmpty(packageFamilyName))
				{
					return result;
				}
				result = new PackageManager().FindPackages(packageFamilyName).Any<Package>();
				Logger.Log(Logger.LogSeverity.Information, "IsPackageInstalledAnyUser: Using PackageManager api, the package installation status is {0}", new object[] { result.ToString() });
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Critical, "IsPackageInstalledAnyUser: Exception occured while checking if package installed using PackageManager api: {0}", new object[] { ex.Message });
			}
			return result;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00006430 File Offset: 0x00004630
		private bool UpdateAppStatus(string pfn, bool status, bool globalStatus)
		{
			bool flag = false;
			Version version = new Version("0.0.0.0");
			string sid = UserInformationProvider.GetLoggedInUserSID();
			this._appStatusSemaphore.Wait();
			UapInstallMonitor.AppStatus appStatus = this._appStatusList.FirstOrDefault((UapInstallMonitor.AppStatus s) => s.Pfn.Equals(pfn + sid, StringComparison.InvariantCultureIgnoreCase));
			if (appStatus != null)
			{
				if (appStatus.InstallState != status)
				{
					flag = true;
					appStatus.InstallState = status;
					version = appStatus.AppVersion;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				if (appStatus != null)
				{
					Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.UpdateAppStatus: Detected change in install state of app for pfn={0} InstallState={1}", new object[]
					{
						pfn,
						status.ToString()
					});
				}
				if (status)
				{
					Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.UpdateAppStatus: getting UAP versifon for pfn={0} now", new object[] { pfn });
					for (int i = 0; i < 10; i++)
					{
						version = UapInstallMonitor.GetPackageInstalledVersion(pfn);
						if (version > new Version("0.0.0.0"))
						{
							Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.UpdateAppStatus: Got UAP versifon for pfn={0} as {1}", new object[]
							{
								pfn,
								version.ToString()
							});
							break;
						}
						Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.UpdateAppStatus: Failed to get UAP version for pfn={0}. Trying again", new object[] { pfn });
						this._eventTerminate.WaitOne(500);
					}
				}
				if (appStatus != null)
				{
					appStatus.AppVersion = version;
					Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.UpdateAppStatus: Detected new app with pfn={0}. got version now {1}", new object[]
					{
						pfn,
						appStatus.AppVersion.ToString()
					});
				}
				else
				{
					this._appStatusList.Add(new UapInstallMonitor.AppStatus(pfn + sid, status, version));
				}
			}
			appStatus = this._appStatusList.FirstOrDefault((UapInstallMonitor.AppStatus s) => s.Pfn.Equals(pfn, StringComparison.InvariantCultureIgnoreCase));
			if (appStatus != null)
			{
				appStatus.InstallState = globalStatus;
			}
			else
			{
				this._appStatusList.Add(new UapInstallMonitor.AppStatus(pfn, globalStatus, version));
			}
			this._appStatusSemaphore.Release();
			return flag;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00006614 File Offset: 0x00004814
		private void StartInstallMonitor()
		{
			Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.StartInstallMonitor: Started");
			try
			{
				do
				{
					if (!this._eventTerminate.WaitOne(3000))
					{
						try
						{
							string loggedInUserSID = UserInformationProvider.GetLoggedInUserSID();
							foreach (KeyValuePair<string, bool> keyValuePair in this._monitoredPfnList)
							{
								bool value = keyValuePair.Value;
								bool flag = this.IsPackageInstalledForCurrentUser(keyValuePair.Key);
								if (value != flag)
								{
									Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.StartInstallMonitor: Change detected for the app pfn: {0} from OldState: {1} to NewState: {2}.", new object[]
									{
										keyValuePair.Key,
										value.ToString(),
										flag.ToString()
									});
									this._monitoredPfnList[keyValuePair.Key] = flag;
									if (this.UpdateAppStatus(keyValuePair.Key, flag, this.IsPackageInstalled(keyValuePair.Key)))
									{
										this.RegUnregApp(loggedInUserSID, keyValuePair.Key, flag);
									}
								}
							}
						}
						catch (Exception ex)
						{
							Logger.Log(ex, "UapInstallMonitor.StartInstallMonitor: Exception occured checking UAP install/uninstall state and sending alerts.");
						}
					}
				}
				while (!this._eventTerminate.WaitOne(Constants.UAPInstallMonitorTimeIntervalInSecs * 1000));
				Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.StartInstallMonitor: Install monitoring closed");
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "UapInstallMonitor.StartInstallMonitor: Exception occured while polling for UAP install/uninstall state.");
			}
			Logger.Log(Logger.LogSeverity.Information, "UapInstallMonitor.StartInstallMonitor: Exit for pfn");
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000679C File Offset: 0x0000499C
		private static Version GetPackageInstalledVersion(string packageFamilyName)
		{
			Version version = new Version("0.0.0.0");
			try
			{
				foreach (Package package in new PackageManager().FindPackages(packageFamilyName))
				{
					version = new Version((int)package.Id.Version.Major, (int)package.Id.Version.Minor, (int)package.Id.Version.Build, (int)package.Id.Version.Revision);
					if (version > new Version("0.0.0.0"))
					{
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Critical, "UapInstallMonitor: Exception occured in GetPackageInstalledVersion: {0}", new object[] { ex.Message });
			}
			return version;
		}

		// Token: 0x0400007E RID: 126
		public static UapInstallMonitor _instance;

		// Token: 0x0400007F RID: 127
		private ConcurrentBag<UapInstallMonitor.AppStatus> _appStatusList;

		// Token: 0x04000080 RID: 128
		private ConcurrentDictionary<string, bool> _monitoredPfnList;

		// Token: 0x04000081 RID: 129
		private ConcurrentQueue<UapInstallMonitor.AppStatus> _unNotifiedAppStatusQ;

		// Token: 0x04000082 RID: 130
		private bool _appMonStarted;

		// Token: 0x04000083 RID: 131
		private readonly SemaphoreSlim _appStatusSemaphore;

		// Token: 0x04000084 RID: 132
		private readonly SemaphoreSlim _RegUnregSemaphore;

		// Token: 0x04000086 RID: 134
		private readonly ManualResetEvent _eventTerminate;

		// Token: 0x0200006D RID: 109
		public class AppStatus
		{
			// Token: 0x1700005A RID: 90
			// (get) Token: 0x06000242 RID: 578 RVA: 0x0000C88A File Offset: 0x0000AA8A
			public string Pfn { get; }

			// Token: 0x1700005B RID: 91
			// (get) Token: 0x06000243 RID: 579 RVA: 0x0000C892 File Offset: 0x0000AA92
			// (set) Token: 0x06000244 RID: 580 RVA: 0x0000C89A File Offset: 0x0000AA9A
			public bool InstallState { get; set; }

			// Token: 0x1700005C RID: 92
			// (get) Token: 0x06000245 RID: 581 RVA: 0x0000C8A3 File Offset: 0x0000AAA3
			// (set) Token: 0x06000246 RID: 582 RVA: 0x0000C8AB File Offset: 0x0000AAAB
			public Version AppVersion { get; set; }

			// Token: 0x06000247 RID: 583 RVA: 0x0000C8B4 File Offset: 0x0000AAB4
			public AppStatus(string pfn, bool state, Version version)
			{
				this.Pfn = pfn;
				this.InstallState = state;
				this.AppVersion = version;
			}
		}

		// Token: 0x0200006E RID: 110
		// (Invoke) Token: 0x06000249 RID: 585
		public delegate void AppStatusChanged(UapInstallMonitor.AppStatus appStatus);

		// Token: 0x0200006F RID: 111
		public static class Win32
		{
			// Token: 0x0600024C RID: 588
			[DllImport("Advapi32.dll", SetLastError = true)]
			public static extern int RegNotifyChangeKeyValue(IntPtr hKey, bool watchSubtree, UapInstallMonitor.Win32.REG_NOTIFY_CHANGE notifyFilter, IntPtr hEvent, bool asynchronous);

			// Token: 0x0600024D RID: 589
			[DllImport("advapi32.dll", SetLastError = true)]
			public static extern int RegOpenKeyEx(IntPtr hKey, string subKey, uint options, int samDesired, out IntPtr phkResult);

			// Token: 0x0600024E RID: 590
			[DllImport("advapi32.dll", SetLastError = true)]
			public static extern int RegCreateKeyEx(IntPtr hKey, string lpSubKey, int Reserved, string lpClass, uint dwOptions, int samDesired, IntPtr lpSecurityAttributes, out IntPtr phkResult, out uint lpdwDisposition);

			// Token: 0x0600024F RID: 591
			[DllImport("advapi32.dll", SetLastError = true)]
			public static extern int RegCloseKey(IntPtr hKey);

			// Token: 0x06000250 RID: 592
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern int PackageIdFromFullName([MarshalAs(UnmanagedType.LPWStr)] string packageFullName, int flags, out int bufferLength, IntPtr buffer);

			// Token: 0x06000251 RID: 593
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern int PackageNameAndPublisherIdFromFamilyName(string packageFamilyName, out int packageNameLength, StringBuilder packageName, out int packagePublisherIdLength, StringBuilder packagePublisherId);

			// Token: 0x06000252 RID: 594
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern int PackageFamilyNameFromFullName(string packageFullName, out int packageFamilyNameLength, StringBuilder packageFamilyName);

			// Token: 0x040001B4 RID: 436
			public const int ERROR_SUCCESS = 0;

			// Token: 0x040001B5 RID: 437
			public const int ERROR_INSUFFICIENT_BUFFER = 122;

			// Token: 0x040001B6 RID: 438
			public const int PACKAGE_INFORMATION_BASIC = 0;

			// Token: 0x040001B7 RID: 439
			public const int PACKAGE_INFORMATION_FULL = 256;

			// Token: 0x040001B8 RID: 440
			public const int KEY_QUERY_VALUE = 1;

			// Token: 0x040001B9 RID: 441
			public const int KEY_NOTIFY = 16;

			// Token: 0x040001BA RID: 442
			public const int KEY_ALLACCESS = 983103;

			// Token: 0x040001BB RID: 443
			public const int KEY_WOW64_32KEY = 512;

			// Token: 0x040001BC RID: 444
			public const int STANDARD_RIGHTS_READ = 131072;

			// Token: 0x040001BD RID: 445
			public const int REG_OPTION_NON_VOLATILE = 0;

			// Token: 0x040001BE RID: 446
			public const int ERROR_KEY_DELETED = 1018;

			// Token: 0x040001BF RID: 447
			public static readonly IntPtr HKEY_CLASSES_ROOT = new IntPtr(int.MinValue);

			// Token: 0x040001C0 RID: 448
			public static readonly IntPtr HKEY_CURRENT_USER = new IntPtr(-2147483647);

			// Token: 0x040001C1 RID: 449
			public static readonly IntPtr HKEY_LOCAL_MACHINE = new IntPtr(-2147483646);

			// Token: 0x040001C2 RID: 450
			public static readonly IntPtr HKEY_USERS = new IntPtr(-2147483645);

			// Token: 0x040001C3 RID: 451
			public static readonly IntPtr HKEY_PERFORMANCE_DATA = new IntPtr(-2147483644);

			// Token: 0x040001C4 RID: 452
			public static readonly IntPtr HKEY_CURRENT_CONFIG = new IntPtr(-2147483643);

			// Token: 0x040001C5 RID: 453
			public static readonly IntPtr HKEY_DYN_DATA = new IntPtr(-2147483642);

			// Token: 0x0200009C RID: 156
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			public struct PACKAGE_ID
			{
				// Token: 0x04000299 RID: 665
				private int reserved;

				// Token: 0x0400029A RID: 666
				private int processorArchitecture;

				// Token: 0x0400029B RID: 667
				public UapInstallMonitor.Win32.PACKAGE_VERSION version;

				// Token: 0x0400029C RID: 668
				private IntPtr name;

				// Token: 0x0400029D RID: 669
				private IntPtr publisher;

				// Token: 0x0400029E RID: 670
				private IntPtr resourceId;

				// Token: 0x0400029F RID: 671
				private IntPtr publisherId;
			}

			// Token: 0x0200009D RID: 157
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			public struct PACKAGE_VERSION
			{
				// Token: 0x040002A0 RID: 672
				public ushort Revision;

				// Token: 0x040002A1 RID: 673
				public ushort Build;

				// Token: 0x040002A2 RID: 674
				public ushort Minor;

				// Token: 0x040002A3 RID: 675
				public ushort Major;
			}

			// Token: 0x0200009E RID: 158
			[Flags]
			public enum REG_NOTIFY_CHANGE : uint
			{
				// Token: 0x040002A5 RID: 677
				NAME = 1U,
				// Token: 0x040002A6 RID: 678
				ATTRIBUTES = 2U,
				// Token: 0x040002A7 RID: 679
				LAST_SET = 4U,
				// Token: 0x040002A8 RID: 680
				SECURITY = 8U
			}
		}
	}
}
