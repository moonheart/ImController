using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.Shared;
using Lenovo.Modern.ImController.Shared.Model;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Telemetry;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Network;

namespace Lenovo.Modern.ImController.UpdateManager.Services
{
	// Token: 0x02000008 RID: 8
	internal class SelfUpdater : UpdaterBase
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002638 File Offset: 0x00000838
		public SelfUpdater(INetworkAgent networkAgent, IPackageHistory packageHistory, ISubscriptionManager subManager, IInstallManager installManager)
			: base(networkAgent)
		{
			if (networkAgent == null || packageHistory == null || subManager == null || installManager == null)
			{
				throw new ArgumentNullException("Missing requirements for SelfUpdater");
			}
			this._packageHistory = packageHistory;
			this._subscriptionManager = subManager;
			this._installManager = installManager;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002670 File Offset: 0x00000870
		protected override async Task RunWhenNetworkConnects()
		{
			if (ImcPolicy.IsUpdateDisabledByPolicy())
			{
				Logger.Log(Logger.LogSeverity.Information, "RunWhenNetworkConnects: Will not update due to policy");
			}
			else
			{
				PackageSubscription packageSubscription = await this._subscriptionManager.GetLatestSubscriptionAsync(this._cancelToken);
				if (packageSubscription != null)
				{
					string expandedTmpPath = SelfUpdater.GetExapndedTmpPath(packageSubscription);
					bool settingValueAsBool = SelfUpdater.GetSettingValueAsBool(Constants.ImcIsRollBackEnabled);
					Version currentServiceVersion = SelfUpdater.GetCurrentServiceVersion();
					Version latestServiceVersionFromSubscription = SelfUpdater.GetLatestServiceVersionFromSubscription(packageSubscription);
					string serviceDownloadURLFromSubscription = this.GetServiceDownloadURLFromSubscription(packageSubscription);
					if (currentServiceVersion != null && latestServiceVersionFromSubscription != null && serviceDownloadURLFromSubscription != null)
					{
						Logger.Log(Logger.LogSeverity.Information, "SelfUpdater: Checking if update is needed. Installed version: {0}, available version: {1}", new object[]
						{
							currentServiceVersion.ToString(),
							latestServiceVersionFromSubscription.ToString()
						});
						if (latestServiceVersionFromSubscription.CompareTo(currentServiceVersion) > 0 || (settingValueAsBool && !latestServiceVersionFromSubscription.Equals(currentServiceVersion)))
						{
							int numberOfInstallAttempts = this._installManager.GetNumberOfInstallAttempts("Service", latestServiceVersionFromSubscription.ToString());
							if (numberOfInstallAttempts < Constants.InstallAttemptThreshold)
							{
								Logger.Log(Logger.LogSeverity.Information, "SelfUpdater: Downloading update...");
								TaskAwaiter<bool> taskAwaiter = base.DownloadFileAsync(serviceDownloadURLFromSubscription, expandedTmpPath, this._cancelToken).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									await taskAwaiter;
									TaskAwaiter<bool> taskAwaiter2;
									taskAwaiter = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter<bool>);
								}
								EventFactory.Constants.UpdateResult result;
								if (taskAwaiter.GetResult())
								{
									Logger.Log(Logger.LogSeverity.Information, "SelfUpdater: Update downloaded successfully");
									if (this._installManager.IsInstallerValid(expandedTmpPath))
									{
										result = EventFactory.Constants.UpdateResult.Success;
										Logger.Log(Logger.LogSeverity.Information, "SelfUpdater: Update downloaded is valid, file will be left for pending updates");
									}
									else
									{
										result = EventFactory.Constants.UpdateResult.Corrupt;
										File.Delete(expandedTmpPath);
										Logger.Log(Logger.LogSeverity.Error, "SelfUpdater: Update package is not valid");
									}
								}
								else
								{
									result = EventFactory.Constants.UpdateResult.Fail;
								}
								ImcEvent userEvent = EventFactory.CreateImcUpdateEvent(EventFactory.Constants.UpdateType.Service, EventFactory.Constants.UpdateAction.Download, result, "");
								EventLogger.GetInstance().LogEvent(userEvent);
							}
							else
							{
								Logger.Log(Logger.LogSeverity.Error, "SelfUpdater: Number of install attempts ({0}) has reached threshhold ({1})", new object[]
								{
									numberOfInstallAttempts,
									Constants.InstallAttemptThreshold
								});
							}
						}
					}
					expandedTmpPath = null;
				}
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000026B5 File Offset: 0x000008B5
		private void UpdateWorker()
		{
			Task.Run(async delegate()
			{
				try
				{
					await Task.Delay(UpdaterBase._startDelayMS, this._cancelToken);
					string text = Environment.ExpandEnvironmentVariables(Constants.PackageTempLocation);
					try
					{
						if (!Directory.Exists(text))
						{
							Directory.CreateDirectory(text);
						}
						DirectoryInfo directoryInfo = new DirectoryInfo(text);
						foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*.msi"))
						{
							fileInfo.Delete();
						}
						foreach (FileInfo fileInfo2 in directoryInfo.EnumerateFiles("*SystemInterfaceFoundation*.cab"))
						{
							fileInfo2.Delete();
						}
						foreach (FileInfo fileInfo3 in directoryInfo.EnumerateFiles("*.exe"))
						{
							fileInfo3.Delete();
						}
						foreach (FileInfo fileInfo4 in directoryInfo.EnumerateFiles("*.log"))
						{
							fileInfo4.Delete();
						}
						foreach (FileInfo fileInfo5 in directoryInfo.EnumerateFiles("*.txt"))
						{
							fileInfo5.Delete();
						}
						string path = text + "\\install";
						if (Directory.Exists(path))
						{
							Directory.Delete(path, true);
						}
						path = text + "\\installer";
						if (Directory.Exists(path))
						{
							Directory.Delete(path, true);
						}
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "SelfUpater: Exception in SelfUpdater.UpdateWorker cleanup");
					}
					if (ImcPolicy.IsUpdateDisabledByPolicy())
					{
						Logger.Log(Logger.LogSeverity.Information, "SelfUpdater: Will not update due to policy");
					}
					else
					{
						base.StartHandlingNetwork();
					}
				}
				catch (Exception ex2)
				{
					Logger.Log(ex2, "SelfUpdater:  exception during update worker");
				}
			}).Wait();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000026CD File Offset: 0x000008CD
		public override void Start(CancellationToken cancelToken)
		{
			this._cancelToken = cancelToken;
			this._updater = new Thread(new ThreadStart(this.UpdateWorker));
			this._updater.Start();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026F8 File Offset: 0x000008F8
		public override void StopAndWait()
		{
			Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Received request to exit thread");
			base.SetStop();
			if (this._updater != null)
			{
				this._updater.Join();
			}
			Logger.Log(Logger.LogSeverity.Information, "SelfUpdater: Updater thread exited");
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000272C File Offset: 0x0000092C
		public override void ApplyPendingUpdates()
		{
			try
			{
				string text = Environment.ExpandEnvironmentVariables(Constants.PackageTempLocation);
				string text2 = text + "\\installer";
				if (Directory.Exists(text2))
				{
					try
					{
						Directory.Delete(text2, true);
					}
					catch (DirectoryNotFoundException)
					{
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "SelfUpdater: ApplyPendingUpdates: Could not delete a directory (it might be missing)");
					}
				}
				if (Directory.EnumerateFiles(text, "*SystemInterfaceFoundation*.*").Any<string>())
				{
					Task<PackageSubscription> subscriptionAsync = this._subscriptionManager.GetSubscriptionAsync(this._cancelToken);
					subscriptionAsync.Wait(this._cancelToken);
					PackageSubscription result = subscriptionAsync.Result;
					string text3 = SelfUpdater.GetExapndedTmpPath(result);
					string str = Path.GetFileName(text3);
					if (!Environment.Is64BitOperatingSystem)
					{
						if (MsSignability.IsMsSignRequired())
						{
							str = "ms-SystemInterfaceFoundation32.cab";
						}
						else
						{
							str = "SystemInterfaceFoundation32.msi";
						}
					}
					string text4 = text2 + "\\" + str;
					Logger.Log(Logger.LogSeverity.Information, "SelfUpdater: SelfUpdater.ApplyPendingUpdates: runMsiFilePath = {0} expandedTmpPath={1}", new object[] { text4, text3 });
					if (File.Exists(text3))
					{
						Logger.Log(Logger.LogSeverity.Information, "SelfUpdater: ApplyPendingUpdates: msi update found and will be applied {0}", new object[] { text3 });
						Directory.CreateDirectory(text2);
						File.Move(text3, text4);
						text3 = text4;
						Version latestServiceVersionFromSubscription = SelfUpdater.GetLatestServiceVersionFromSubscription(result);
						int numberOfInstallAttempts = this._installManager.GetNumberOfInstallAttempts("Service", (latestServiceVersionFromSubscription != null) ? latestServiceVersionFromSubscription.ToString() : null);
						Logger.Log(Logger.LogSeverity.Information, "SelfUpdater: ApplyPendingUpdates: called, number of install attempts so far: {0}", new object[] { numberOfInstallAttempts });
						if (numberOfInstallAttempts < Constants.InstallAttemptThreshold)
						{
							Logger.Log(Logger.LogSeverity.Information, "SelfUpdater: ApplyPendingUpdates will run msi from: {0}", new object[] { text3 });
							if (this._installManager.IsInstallerValid(text3))
							{
								Logger.Log(Logger.LogSeverity.Information, "SelfUpdater: ApplyPendingUpdates: Update file is valid and will be run now");
								this._packageHistory.CachePackageInformation(new CacheInformation
								{
									DateLastModified = DateTime.Now.ToString(),
									Location = "",
									Name = "Service",
									NumberOfInstallAttempts = numberOfInstallAttempts + 1,
									Version = ((latestServiceVersionFromSubscription != null) ? latestServiceVersionFromSubscription.ToString() : null)
								}, true);
								if (MsSignability.IsMsSignRequired() || !InstallMethod.IsMsiInstall())
								{
									this._installManager.RunMsInstaller(text3);
								}
								else
								{
									this._installManager.RunInstaller(text3, numberOfInstallAttempts == 0);
								}
							}
							else
							{
								File.Delete(text3);
								Logger.Log(Logger.LogSeverity.Error, "SelfUpdater: ApplyPendingUpdates: Update package is not valid");
							}
						}
						else
						{
							Logger.Log(Logger.LogSeverity.Error, "SelfUpdater: ApplyPendingUpdates: Will not install because number of install attempts greater than");
						}
					}
					else
					{
						Logger.Log(Logger.LogSeverity.Information, "SelfUpdater.ApplyPendingUpdates: update not found, will not update this time: {0}", new object[] { text3 });
					}
				}
				else
				{
					Logger.Log(Logger.LogSeverity.Information, "SelfUpdater.ApplyPendingUpdates: update not found, will not update.");
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "SelfUpdater: ApplyPendingUpdates: Exception in ApplyPendingUpdates in SelfUpdater");
			}
			Logger.Log(Logger.LogSeverity.Information, "SelfUpdater: ApplyPendingUpdates: Complete");
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000029E0 File Offset: 0x00000BE0
		public static Version GetCurrentServiceVersion()
		{
			return new Version(Constants.ImControllerVersion);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000029EC File Offset: 0x00000BEC
		public static Version GetLatestServiceVersionFromSubscription(PackageSubscription subscription)
		{
			Version version = null;
			try
			{
				if (subscription != null)
				{
					if (subscription.Service != null)
					{
						if (MsSignability.IsMsSignRequired() || !InstallMethod.IsMsiInstall())
						{
							if (subscription.Service.MsVersion != null)
							{
								version = new Version(subscription.Service.MsVersion);
							}
						}
						else if (subscription.Service.Version != null)
						{
							version = new Version(subscription.Service.Version);
						}
					}
					if (null == version && subscription.PackageList != null)
					{
						Package package = (from x in subscription.PackageList
							where x.PackageInformation.Name == "Service"
							select x).FirstOrDefault<Package>();
						if (package != null)
						{
							version = new Version((MsSignability.IsMsSignRequired() || !InstallMethod.IsMsiInstall()) ? package.PackageInformation.MsVersion : package.PackageInformation.Version);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "SelfUpdater: Exception thrown trying to get the latest service version from the subscription file");
			}
			return version;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002AE4 File Offset: 0x00000CE4
		private string GetServiceDownloadURLFromSubscription(PackageSubscription subscription)
		{
			string text = null;
			if (subscription != null && subscription.Service != null)
			{
				if (Environment.Is64BitOperatingSystem)
				{
					if (MsSignability.IsMsSignRequired() || !InstallMethod.IsMsiInstall())
					{
						text = SelfUpdater.GetSettingValue(Constants.MsDownloadLocationSettingKeyName);
						if (string.IsNullOrWhiteSpace(text) && subscription.Service.MsDownloadLocation64 != null)
						{
							text = subscription.Service.MsDownloadLocation64;
						}
					}
					else if (InstallMethod.IsMsiToInfBridgeEnabled(subscription))
					{
						text = SelfUpdater.GetSettingValue(Constants.MsiToInfBridgeSettingKyName);
						if (string.IsNullOrWhiteSpace(text) && subscription.Service.DownloadLocation != null)
						{
							text = subscription.Service.DownloadLocation;
						}
					}
					else
					{
						text = SelfUpdater.GetSettingValue(Constants.MsiLocationSettingKeyName);
						if (string.IsNullOrWhiteSpace(text) && subscription.Service.DownloadLocation64 != null)
						{
							text = subscription.Service.DownloadLocation64;
						}
					}
				}
				if (!Environment.Is64BitOperatingSystem)
				{
					if (MsSignability.IsMsSignRequired())
					{
						text = SelfUpdater.GetSettingValue(Constants.MsDownloadLocationSettingKeyName);
						if (string.IsNullOrWhiteSpace(text) && subscription.Service.MsDownloadLocation32 != null)
						{
							text = subscription.Service.MsDownloadLocation32;
						}
					}
					else
					{
						text = SelfUpdater.GetSettingValue(Constants.MsiLocationSettingKeyName);
						if (string.IsNullOrWhiteSpace(text) && subscription.Service.DownloadLocation32 != null)
						{
							text = subscription.Service.DownloadLocation32;
						}
					}
				}
			}
			return text;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002C18 File Offset: 0x00000E18
		private static string GetExapndedTmpPath(PackageSubscription subscription)
		{
			string result;
			if (MsSignability.IsMsSignRequired() || !InstallMethod.IsMsiInstall())
			{
				result = Environment.ExpandEnvironmentVariables(Constants.MsInstallerFilePathPlaceholder);
			}
			else if (InstallMethod.IsMsiToInfBridgeEnabled(subscription))
			{
				result = Environment.ExpandEnvironmentVariables(Constants.BridgeInstallerFilePathPlaceholder);
			}
			else
			{
				result = Environment.ExpandEnvironmentVariables(Constants.InstallerFilePathPlaceholder);
			}
			return result;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002C68 File Offset: 0x00000E68
		private static string GetSettingValue(string settingkeyName)
		{
			string text = "";
			Setting result = SubscriptionSettingsAgent.GetInstance().GetApplicableSettingAsync(settingkeyName, CancellationToken.None).Result;
			if (result != null && result.Value != null)
			{
				text = result.Value;
				if (text != null)
				{
					Logger.Log(Logger.LogSeverity.Warning, "GetSettingValue: " + settingkeyName + " location is overridden to " + text);
				}
			}
			return text;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002CC0 File Offset: 0x00000EC0
		private static bool GetSettingValueAsBool(string settingkeyName)
		{
			bool result = false;
			Setting result2 = SubscriptionSettingsAgent.GetInstance().GetApplicableSettingAsync(settingkeyName, CancellationToken.None).Result;
			if (result2 != null && result2.Value != null)
			{
				bool? valueAsBool = result2.GetValueAsBool();
				if (valueAsBool != null && valueAsBool.GetValueOrDefault(false))
				{
					result = valueAsBool.Value;
				}
			}
			return result;
		}

		// Token: 0x04000017 RID: 23
		private Thread _updater;

		// Token: 0x04000018 RID: 24
		private readonly IPackageHistory _packageHistory;

		// Token: 0x04000019 RID: 25
		private readonly ISubscriptionManager _subscriptionManager;

		// Token: 0x0400001A RID: 26
		private readonly IInstallManager _installManager;

		// Token: 0x0400001B RID: 27
		private const string ServicePackageName = "Service";

		// Token: 0x0400001C RID: 28
		private CancellationToken _cancelToken;
	}
}
