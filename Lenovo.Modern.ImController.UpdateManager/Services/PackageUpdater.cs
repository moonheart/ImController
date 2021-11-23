using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Lenovo.Modern.ImController.Shared.Model;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Services.Contracts.SystemInformation.AppTag;
using Lenovo.Modern.ImController.Shared.Telemetry;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Network;
using Microsoft.Win32;

namespace Lenovo.Modern.ImController.UpdateManager.Services
{
	// Token: 0x0200000A RID: 10
	internal class PackageUpdater : UpdaterBase, IDisposable
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00002E4C File Offset: 0x0000104C
		public PackageUpdater(INetworkAgent networkAgent, IPackageHistory packageHistory, ISubscriptionManager subManager, IInstallManager installManager)
			: base(networkAgent)
		{
			this._subscriptionManager = subManager;
			this._packageHistory = packageHistory;
			this._installManager = installManager;
			this._pluginRepository = new PluginRepository();
			this._installSem = new SemaphoreSlim(1, 1);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002E99 File Offset: 0x00001099
		public void Dispose()
		{
			this._applyPendingUpdateTimer.Dispose();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002EA8 File Offset: 0x000010A8
		private async Task OnTimerEvent()
		{
			try
			{
				await this.ApplyPendingPluginFoldersAsync(true);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception in PackageUpdater.OnTimerEvent");
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002EF0 File Offset: 0x000010F0
		private async Task GetPluginsThatNeedToBeUpdatedAsync()
		{
			Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: UpdateUpdateList: Network is ready");
			if (ImcPolicy.IsUpdateDisabledByPolicy())
			{
				Logger.Log(Logger.LogSeverity.Information, "GetPluginsThatNeedToBeUpdatedAsync: Will not update due to policy");
			}
			else
			{
				PackageSubscription packageSubscription = await this._subscriptionManager.GetLatestSubscriptionAsync(this._cancelToken);
				if (packageSubscription != null && packageSubscription.PackageList != null && packageSubscription.PackageList.Any<Package>())
				{
					Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: UpdateUpdateList: Checking for updates in subscription file...");
					foreach (Package package in packageSubscription.PackageList)
					{
						try
						{
							int num;
							if (this.IsPackageUpdateNecessary(packageSubscription, package, out num))
							{
								package.PackageInformation.NumberOfInstallAttempts = num;
								if (!this._listOfPackagesThatNeedUpdating.Contains(package))
								{
									this._listOfPackagesThatNeedUpdating.Enqueue(package);
								}
								Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Package {0} will be updated.", new object[] { package.PackageInformation.Name });
							}
							else
							{
								Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Package {0} will NOT be updated.  InstallAttempts {1}", new object[]
								{
									package.PackageInformation.Name,
									num
								});
							}
						}
						catch (Exception ex)
						{
							Logger.Log(ex, "PackageUpdater: Exception in UpdateUpdateList");
						}
					}
					Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: {0} packages need to be updated", new object[] { this._listOfPackagesThatNeedUpdating.Count });
				}
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002F38 File Offset: 0x00001138
		private async Task DownloadAndPrepareFolderForAllUpdatesAsync()
		{
			int attempts = 0;
			int numberOfsuccessfulDownloads = 0;
			int numberOfsuccessfulExtracts = 0;
			int numberOfItemsNeedUpdating = this._listOfPackagesThatNeedUpdating.Count;
			Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: DownloadAndInstall Starting. Number of items: {0}", new object[] { numberOfItemsNeedUpdating });
			for (int iteration = 0; iteration < Constants.InstallAttemptThreshold; iteration++)
			{
				int listSize = this._listOfPackagesThatNeedUpdating.Count;
				attempts = 0;
				while (this._listOfPackagesThatNeedUpdating.Any<Package>())
				{
					string packageTempLocation = "";
					Package package;
					if (!this._listOfPackagesThatNeedUpdating.TryDequeue(out package))
					{
						break;
					}
					try
					{
						string path = Environment.ExpandEnvironmentVariables(Constants.PackageTempLocation);
						packageTempLocation = Path.Combine(path, package.PackageInformation.Location.Split(new char[] { '/' }).Last<string>().Split(new char[] { '-' })
							.Last<string>());
						string packagesBaseAddress = Constants.PackagesBaseAddress;
						Setting setting = await SubscriptionSettingsAgent.GetInstance().GetApplicableSettingAsync(Constants.PackageLocationSettingKeyName, CancellationToken.None);
						if (setting != null && setting.Value != null)
						{
							packagesBaseAddress = setting.Value;
							if (packagesBaseAddress != null)
							{
								Logger.Log(Logger.LogSeverity.Warning, "DownloadAndPrepareFolderForAllUpdatesAsync: Package location is overridden to " + packagesBaseAddress);
							}
						}
						string url = packagesBaseAddress + (MsSignability.IsMsSignRequired() ? package.PackageInformation.MsLocation : package.PackageInformation.Location);
						bool pendingFolderCandidate = PackageSettingsAgent.DoesPackageInstallRequireReboot(package);
						EventFactory.Constants.UpdateResult result = EventFactory.Constants.UpdateResult.Fail;
						TaskAwaiter<bool> taskAwaiter = base.DownloadFileAsync(url, packageTempLocation, this._cancelToken).GetAwaiter();
						TaskAwaiter<bool> taskAwaiter2;
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (taskAwaiter.GetResult())
						{
							numberOfsuccessfulDownloads++;
							result = EventFactory.Constants.UpdateResult.Success;
							this._pluginRepository.UpdatePacakgeInformationInRegistry(package.PackageInformation.Name, MsSignability.IsMsSignRequired() ? package.PackageInformation.MsVersion : package.PackageInformation.Version, true);
							taskAwaiter = this._pluginRepository.PreInstallPackageAsync(packageTempLocation, pendingFolderCandidate).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								await taskAwaiter;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (taskAwaiter.GetResult())
							{
								numberOfsuccessfulExtracts++;
							}
						}
						else
						{
							Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Package {0} failed to download", new object[] { package.PackageInformation.Name });
							this._listOfPackagesThatNeedUpdating.Enqueue(package);
						}
						ImcEvent userEvent = EventFactory.CreateImcUpdateEvent(EventFactory.Constants.UpdateType.Package, EventFactory.Constants.UpdateAction.Download, result, package.PackageInformation.Name);
						EventLogger.GetInstance().LogEvent(userEvent);
						if (result == EventFactory.Constants.UpdateResult.Success)
						{
							await this.ApplyFirstVersionPendingFoldersAsync(package.PackageInformation.Name);
						}
						packagesBaseAddress = null;
					}
					catch (UriFormatException ex)
					{
						Logger.Log(ex, "PackageUpdater: Exception thrown trying to download and install the " + package.PackageInformation.Name + " package");
					}
					catch (PluginRepositoryException ex2)
					{
						Logger.Log(ex2, "PackageUpdater: Exception thrown trying to download and install the " + package.PackageInformation.Name + " package");
					}
					catch (WebException)
					{
						this._listOfPackagesThatNeedUpdating.Enqueue(package);
					}
					catch (Exception ex3)
					{
						Logger.Log(ex3, "PackageUpdater: Exception thrown trying to download and install the " + package.PackageInformation.Name + " package");
					}
					finally
					{
						try
						{
							File.Delete(packageTempLocation);
						}
						catch (Exception ex4)
						{
							Logger.Log(ex4, "PackageUpdater: DownloadAndInstall Cannot delete temp file: {0}", new object[] { packageTempLocation });
						}
					}
					if (++attempts >= listSize)
					{
						Logger.Log(Logger.LogSeverity.Warning, "PackageUpdater: DownloadAndInstall {0} attempts were made and no more downloads will be done.", new object[] { listSize });
						break;
					}
					if (this._stopEvent.WaitOne(0))
					{
						Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: DownloadAndInstall Stopping download thread by stop event.");
						break;
					}
					package = null;
					packageTempLocation = null;
				}
			}
			Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: DownloadAndInstall Summary: # of plugins to update: {0}, # of download success: {1}, # of extract success: {2}, # of attempts {3}", new object[] { numberOfItemsNeedUpdating, numberOfsuccessfulDownloads, numberOfsuccessfulExtracts, attempts });
			if (!this._listOfPackagesThatNeedUpdating.Any<Package>())
			{
				Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: No more plugins need to be downloaded. Exiting update download thread.");
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002F80 File Offset: 0x00001180
		private void UpdateWorker()
		{
			try
			{
				Task.Run(async delegate()
				{
					try
					{
						await Task.Delay(UpdaterBase._startDelayMS, this._cancelToken);
						string path = Environment.ExpandEnvironmentVariables(Constants.PackageTempLocation);
						try
						{
							if (!Directory.Exists(path))
							{
								Directory.CreateDirectory(path);
							}
							foreach (FileInfo fileInfo in new DirectoryInfo(path).EnumerateFiles("*.cab"))
							{
								if (!fileInfo.Name.ToLowerInvariant().Contains("systeminterfacefoundation"))
								{
									fileInfo.Delete();
								}
							}
						}
						catch (Exception ex2)
						{
							Logger.Log(ex2, "PackageUpdater: Exception in PackageUpdater.UpdateWorker cleanup");
						}
						if (ImcPolicy.IsUpdateDisabledByPolicy())
						{
							Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Will not update due to policy");
						}
						else
						{
							base.StartHandlingNetwork();
						}
					}
					catch (Exception ex3)
					{
						Logger.Log(ex3, "PackageUpdater: Exception in update worker");
					}
				}).Wait(this._cancelToken);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PackageUpdater: Exception in update worker task");
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002FC8 File Offset: 0x000011C8
		protected override async Task RunWhenNetworkConnects()
		{
			await this.GetPluginsThatNeedToBeUpdatedAsync();
			await this.DownloadAndPrepareFolderForAllUpdatesAsync();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003010 File Offset: 0x00001210
		private async Task<bool> ApplyFirstVersionPendingFoldersAsync(string pluginName)
		{
			this._installSem.Wait(this._cancelToken);
			PackageSubscription packageSubscription = await this._subscriptionManager.GetSubscriptionAsync(this._cancelToken);
			bool allSuccessful = true;
			this.CreatePluginsDirectoryIfMissing();
			bool flag = !Environment.UserInteractive && Environment.GetCommandLineArgs().Length == 0;
			if (packageSubscription != null && packageSubscription.PackageList != null && packageSubscription.PackageList.Any<Package>())
			{
				try
				{
					Package package = packageSubscription.PackageList.FirstOrDefault((Package p) => p != null && p.PackageInformation != null && (!string.IsNullOrWhiteSpace(p.PackageInformation.Name) & p.PackageInformation.Name.Equals(pluginName, StringComparison.InvariantCultureIgnoreCase)));
					if (package != null && package.PackageInformation != null && package.PackageInformation.Name != null && SubscribedPackageManager.IsPackageApplicable(packageSubscription, package, this._cancelToken))
					{
						if (PackageSettingsAgent.IsPackageUpdateDisabled(package) && (flag || !PackageSettingsAgent.IsForcedInstallationEnabled(package)))
						{
							Logger.Log(Logger.LogSeverity.Warning, "PackageUpdater: ApplyFirstVersionPendingFoldersAsync: Package update disabled by subscription- {0}", new object[] { package.PackageInformation.Name });
						}
						else if (!PackageInstaller.DoesPluginFolderExist(package.PackageInformation.Name, false) && PackageInstaller.DoesPluginPreinstallFolderExist(package.PackageInformation.Name, false))
						{
							Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: ApplyFirstVersionPendingFoldersAsync: Attempt to install from pre-installed update: {0}", new object[] { package.PackageInformation.Name });
							TaskAwaiter<bool> taskAwaiter = this._pluginRepository.FinalInstallPackage(package.PackageInformation.Name, false).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								await taskAwaiter;
								TaskAwaiter<bool> taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (taskAwaiter.GetResult())
							{
								Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: ApplyFirstVersionPendingFoldersAsync: {0} successfully installed from pre-installed folder", new object[] { package.PackageInformation.Name });
								this._pluginRepository.UpdatePacakgeInformationInRegistry(package.PackageInformation.Name, MsSignability.IsMsSignRequired() ? package.PackageInformation.MsVersion : package.PackageInformation.Version, false);
							}
							else
							{
								Logger.Log(Logger.LogSeverity.Error, "PackageUpdater: ApplyFirstVersionPendingFoldersAsync Failed to appy pending package: {0}", new object[] { package.PackageInformation.Name });
								allSuccessful = false;
							}
						}
					}
					package = null;
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "PackageUpdater: ApplyFirstVersionPendingFoldersAsync: Exception while processing {0}", new object[] { pluginName });
				}
			}
			this._installSem.Release();
			return allSuccessful;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003060 File Offset: 0x00001260
		private async Task<bool> ApplyPendingPluginFoldersAsync(bool skipCustomInstallablePackages)
		{
			bool allSuccessful = true;
			Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: ApplyPendingPluginFoldersAsync: Entry");
			IEnumerable<string> enumerable = Directory.EnumerateDirectories(InstallationLocator.GetPluginInstallationLocation(), "*_");
			if (enumerable != null && enumerable.Any<string>())
			{
				bool isServiceProcess = !Environment.UserInteractive && Environment.GetCommandLineArgs().Length == 0;
				TaskAwaiter<bool> taskAwaiter2;
				if (isServiceProcess)
				{
					TaskAwaiter<bool> taskAwaiter = this.IsServiceCurrent().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (!taskAwaiter.GetResult())
					{
						return false;
					}
				}
				this._installSem.Wait(this._cancelToken);
				PackageSubscription subscriptionFile = await this._subscriptionManager.GetSubscriptionAsync(this._cancelToken);
				this.CreatePluginsDirectoryIfMissing();
				if (subscriptionFile != null && subscriptionFile.PackageList != null && subscriptionFile.PackageList.Any<Package>())
				{
					Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: ApplyPendingPluginFolders: Checking for pre-installed updates...");
					int numberOfFoldersRenamed = 0;
					int numberOfRenameAttempts = 0;
					foreach (Package package in subscriptionFile.PackageList)
					{
						try
						{
							if (package != null && package.PackageInformation != null && package.PackageInformation.Name != null && SubscribedPackageManager.IsPackageApplicable(subscriptionFile, package, this._cancelToken))
							{
								if (PackageSettingsAgent.IsPackageUpdateDisabled(package) && (isServiceProcess || !PackageSettingsAgent.IsForcedInstallationEnabled(package)))
								{
									Logger.Log(Logger.LogSeverity.Warning, "PackageUpdater: ApplyPendingPluginFolders: Package update disabled by subscription- {0}", new object[] { package.PackageInformation.Name });
								}
								else
								{
									this.CleanupPluginBackupFolder(package.PackageInformation.Name);
									if (PackageInstaller.DoesPluginPreinstallFolderExist(package.PackageInformation.Name, false))
									{
										numberOfRenameAttempts++;
										Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: ApplyPendingPluginFolders: Attempt to install from pre-installed update: {0}", new object[] { package.PackageInformation.Name });
										TaskAwaiter<bool> taskAwaiter = this._pluginRepository.FinalInstallPackage(package.PackageInformation.Name, skipCustomInstallablePackages).GetAwaiter();
										if (!taskAwaiter.IsCompleted)
										{
											await taskAwaiter;
											taskAwaiter = taskAwaiter2;
											taskAwaiter2 = default(TaskAwaiter<bool>);
										}
										if (taskAwaiter.GetResult())
										{
											numberOfFoldersRenamed++;
											Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: ApplyPendingPluginFolders: {0} successfully installed from pre-installed folder", new object[] { package.PackageInformation.Name });
											this._pluginRepository.UpdatePacakgeInformationInRegistry(package.PackageInformation.Name, MsSignability.IsMsSignRequired() ? package.PackageInformation.MsVersion : package.PackageInformation.Version, false);
											if (string.Compare(package.PackageInformation.Name, "GenericCorePlugin", StringComparison.InvariantCultureIgnoreCase) == 0)
											{
												Logger.Log(Logger.LogSeverity.Error, "PackageUpdater: ApplyPendingPluginFolders: Updating AppTags and MachineInfo cache");
												MachineInformationManager.GetInstance().UpdateCacheWithDelay(this._cancelToken);
												AppAndTagManager.GetInstance().UpdateCacheWithDelay(this._cancelToken);
											}
										}
										else
										{
											Logger.Log(Logger.LogSeverity.Error, "PackageUpdater: ApplyPendingPluginFolders Failed to appy pending package: {0}", new object[] { package.PackageInformation.Name });
											allSuccessful = false;
										}
									}
								}
							}
						}
						catch (Exception ex)
						{
							Logger.Log(ex, "PackageUpdater: PackageUPdater: ApplyPendingPluginFolders: Exception while processing {0}", new object[] { package.PackageInformation.Name });
						}
						package = null;
					}
					List<Package>.Enumerator enumerator = default(List<Package>.Enumerator);
					Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: ApplyPendingPluginFolders Summary: Applied {0} out of {1} packages", new object[] { numberOfFoldersRenamed, numberOfRenameAttempts });
				}
				this._installSem.Release();
				subscriptionFile = null;
			}
			Task.Run(async delegate()
			{
				try
				{
					Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Trying to delete those folders which are not referred by subscription file");
					this.DeleteUnusedPackages();
				}
				catch (Exception ex2)
				{
					Logger.Log(ex2, "PackageUpdater: Exception while deleting unused packages");
				}
			});
			try
			{
				this._applyPendingUpdateTimer.Stop();
				this._applyPendingUpdateTimer.Interval = 1200000.0;
				this._applyPendingUpdateTimer.Start();
			}
			catch (Exception)
			{
			}
			Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: ApplyPendingPluginFoldersAsync: Complete");
			return allSuccessful;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000030B0 File Offset: 0x000012B0
		private bool IsPackageUpdateNecessary(PackageSubscription subscription, Package package, out int installAttempts)
		{
			Version version = null;
			installAttempts = 1;
			if (package != null && package.PackageInformation != null)
			{
				Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Checking if it is necessary to update package: {0}", new object[] { package.PackageInformation.Name ?? "NULL" });
			}
			if (package == null || package.PackageInformation == null || package.PackageInformation.Name == null)
			{
				return false;
			}
			if (!SubscribedPackageManager.IsPackageApplicable(subscription, package, this._cancelToken))
			{
				return false;
			}
			if ((package.PackageInformation.Packagetype == PackageType.App || package.PackageInformation.Packagetype == PackageType.Dependency) && ImcPolicy.GetIsPackageTypeDisabled())
			{
				Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Package {0} will not used because disabled through policy", new object[] { package.PackageInformation.Name });
				return false;
			}
			if (this._packageHistory == null)
			{
				Logger.Log(Logger.LogSeverity.Error, "PackageUpdater: IsPackageUpdateNecessary: _packageHistory is null");
				return false;
			}
			try
			{
				if (MsSignability.IsMsSignRequired())
				{
					if (package.PackageInformation.MsVersion == null || package.PackageInformation.MsLocation == null || null == (version = new Version(package.PackageInformation.MsVersion)))
					{
						return false;
					}
				}
				else if (package.PackageInformation.Version == null || package.PackageInformation.Location == null || null == (version = new Version(package.PackageInformation.Version)))
				{
					return false;
				}
			}
			catch (ArgumentOutOfRangeException ex)
			{
				Logger.Log(ex, "PackageUpdater: Version ArgumentOutOfRangeException in Update Worker, string version: {0}", new object[] { package.PackageInformation.Version });
				return false;
			}
			catch (ArgumentException ex2)
			{
				Logger.Log(ex2, "PackageUpdater: Version ArgumentException in Update Worker, string version: {0}", new object[] { package.PackageInformation.Version });
				return false;
			}
			catch (FormatException ex3)
			{
				Logger.Log(ex3, "PackageUpdater: Version FormatException in Update Worker, string version: {0}", new object[] { package.PackageInformation.Version });
				return false;
			}
			catch (OverflowException ex4)
			{
				Logger.Log(ex4, "PackageUpdater: Version OverflowException in Update Worker, string version: {0}", new object[] { package.PackageInformation.Version });
				return false;
			}
			if (PackageSettingsAgent.IsPackageUpdateDisabled(package))
			{
				Logger.Log(Logger.LogSeverity.Warning, "PackageUpdater: Package update disabled by subscription- {0}", new object[] { package.PackageInformation.Name });
				return false;
			}
			bool flag = PackageSettingsAgent.DoesPackageInstallRequireReboot(package);
			if (!flag && PackageSettingsAgent.DoesPackageUpdateRequireReboot(package))
			{
				return false;
			}
			bool flag2 = false;
			PluginRepository.PluginInformation pluginInformation = null;
			try
			{
				pluginInformation = this._pluginRepository.GetPluginPathWithPluginName(package.PackageInformation.Name);
				flag2 = true;
			}
			catch (DirectoryNotFoundException)
			{
				Logger.Log(Logger.LogSeverity.Warning, "PackageUpdater: Unable to locate installed plugin {0}, (possibly new?) will need to update/install", new object[] { package.PackageInformation.Name });
			}
			catch (Exception ex5)
			{
				Logger.Log(ex5, "PackageUpdater: Exception while getting plugin path from plugin name");
			}
			if (pluginInformation == null || pluginInformation.PathToPlugin == null)
			{
				flag2 = false;
			}
			if (PackageInstaller.DoesPluginPreinstallFolderExist(package.PackageInformation.Name, flag))
			{
				return false;
			}
			if (!flag2 || !File.Exists(pluginInformation.PathToPlugin))
			{
				Logger.Log(Logger.LogSeverity.Warning, "PackageUpdater: Package files or folder are missing and will be updated: {0}", new object[] { package.PackageInformation.Name });
				return true;
			}
			CacheInformation packageInformationFromCache = this._packageHistory.GetPackageInformationFromCache(package.PackageInformation.Name, MsSignability.IsMsSignRequired() ? package.PackageInformation.MsVersion : package.PackageInformation.Version);
			if (packageInformationFromCache == null)
			{
				installAttempts = 1;
			}
			else if (packageInformationFromCache.NumberOfInstallAttempts > 0)
			{
				installAttempts = packageInformationFromCache.NumberOfInstallAttempts + 1;
			}
			if (installAttempts > Constants.InstallAttemptThreshold)
			{
				Logger.Log(Logger.LogSeverity.Warning, "PackageUpdater: Package will not be updated because exceeded number of attempts: {0}", new object[] { package.PackageInformation.Name });
				return false;
			}
			Version version2 = pluginInformation.Version;
			if (null == version2)
			{
				Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Plugin file exists, but we can't read it for some reason. Update might be needed: {0}", new object[] { package.PackageInformation.Name });
				return true;
			}
			bool flag3 = false;
			if (version.CompareTo(version2) > 0)
			{
				Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Current plugin version is {0}, and available version is {1}. Update is needed. Plugin name: {2}", new object[]
				{
					version2.ToString(),
					version.ToString(),
					package.PackageInformation.Name
				});
				flag3 = true;
			}
			if (!flag3 && SubscribedPackageManager.IsCustomInstallPrivilegeEnabled(subscription, package.PackageInformation.Name).Result)
			{
				Version currentInstalledVersionOfPackage = this._packageHistory.GetCurrentInstalledVersionOfPackage(package.PackageInformation.Name);
				if (null == currentInstalledVersionOfPackage)
				{
					flag3 = false;
				}
				else if ("0.0.0.0" == currentInstalledVersionOfPackage.ToString())
				{
					flag3 = true;
				}
				else if (!File.Exists(Path.Combine(Path.GetDirectoryName(pluginInformation.PathToPlugin), "uninstall.ps1")))
				{
					flag3 = true;
				}
				if (flag3)
				{
					Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Current version in registry is either 0.0.0.0 or not present. Update is needed. Plugin name: {0}", new object[] { package.PackageInformation.Name });
				}
			}
			return flag3;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003564 File Offset: 0x00001764
		private void CreatePluginsDirectoryIfMissing()
		{
			try
			{
				string pluginInstallationLocation = InstallationLocator.GetPluginInstallationLocation();
				if (!Directory.Exists(pluginInstallationLocation))
				{
					Directory.CreateDirectory(pluginInstallationLocation);
					Logger.Log(Logger.LogSeverity.Information, "Plugin folder was missing and was created");
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception in PluginRepository.CreatePluginsDirectoryIfMissing");
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000035B0 File Offset: 0x000017B0
		private bool CleanupPluginBackupFolder(string pluginFolder)
		{
			bool result = false;
			if (!Utility.SanitizePath(ref pluginFolder))
			{
				Logger.Log(Logger.LogSeverity.Error, "CleanupPluginBackupFolder: Failed to cleanup plugin backup folder as path is invalid. Path - {0}", new object[] { pluginFolder });
				return result;
			}
			string text = InstallationLocator.GetPluginInstallationLocation();
			text = text + "\\~" + pluginFolder;
			try
			{
				if (Directory.Exists(text))
				{
					Directory.Delete(text, true);
					result = true;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception occured in PluginRepository.CleanupPluginBackupFolder");
			}
			return result;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003624 File Offset: 0x00001824
		private async Task<bool> IsServiceCurrent()
		{
			bool result;
			if (this._subscriptionManager == null)
			{
				Logger.Log(Logger.LogSeverity.Error, "PackageUpdater: Exiting because _subscriptionManager is null. Version in the subscription cannot be checked.");
				result = false;
			}
			else if (ImcPolicy.IsUpdateDisabledByPolicy())
			{
				Logger.Log(Logger.LogSeverity.Information, "IsServiceCurrent: IMC update policy is set to false. Current version is always latest");
				result = true;
			}
			else
			{
				PackageSubscription packageSubscription = await this._subscriptionManager.GetLatestSubscriptionAsync(this._cancelToken);
				if (packageSubscription == null)
				{
					Logger.Log(Logger.LogSeverity.Information, "IsServiceCurrent: Getting cached subscription since network may be down");
					packageSubscription = await this._subscriptionManager.GetSubscriptionAsync(this._cancelToken);
				}
				Version currentServiceVersion = SelfUpdater.GetCurrentServiceVersion();
				Version latestServiceVersionFromSubscription = SelfUpdater.GetLatestServiceVersionFromSubscription(packageSubscription);
				if (packageSubscription != null && null != currentServiceVersion && latestServiceVersionFromSubscription != null && latestServiceVersionFromSubscription.CompareTo(currentServiceVersion) > 0)
				{
					Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Exiting because installed Service version ({0}) is not current ({1})", new object[]
					{
						currentServiceVersion.ToString(),
						latestServiceVersionFromSubscription.ToString()
					});
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000366C File Offset: 0x0000186C
		private void DeletePluginSafely(DirectoryInfo plugin)
		{
			try
			{
				string path = string.Concat(new object[] { plugin.FullName, "\\x64\\", plugin, ".dll" });
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				path = string.Concat(new object[] { plugin.FullName, "\\x86\\", plugin, ".dll" });
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				plugin.Delete(true);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "DeletePluginSafely: Exception");
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000370C File Offset: 0x0000190C
		private async Task<bool> DeleteUnusedPackages()
		{
			int timeDelayDeleteUnusedPkgs = this.GetTimeDelayDeleteUnusedPkgs();
			Logger.Log(Logger.LogSeverity.Information, "DeleteUnusedPackages: trying to delete unnecessary folders after {0} minutes", new object[] { timeDelayDeleteUnusedPkgs });
			Task.Delay(timeDelayDeleteUnusedPkgs * 60 * 1000).Wait(this._cancelToken);
			bool result;
			if (this._cancelToken.IsCancellationRequested)
			{
				result = false;
			}
			else
			{
				bool retVal = true;
				PackageSubscription subscription2 = await this._subscriptionManager.GetSubscriptionAsync(this._cancelToken);
				PackageSubscription subscription = subscription2;
				if (subscription != null && subscription.PackageList != null && subscription.PackageList.Any<Package>())
				{
					List<Package> packageList = subscription.PackageList;
					string pluginInstallationLocation = InstallationLocator.GetPluginInstallationLocation();
					try
					{
						IEnumerable<DirectoryInfo> enumerable = new DirectoryInfo(pluginInstallationLocation).EnumerateDirectories("*");
						IEnumerable<Package> source = (from package in packageList
							where SubscribedPackageManager.IsPackageApplicable(subscription, package, default(CancellationToken))
							select package).ToList<Package>();
						IEnumerable<Package> source2 = (from package in packageList
							where SubscribedPackageManager.IsPackageApplicable(subscription, package, default(CancellationToken)) && PackageSettingsAgent.IsPackageUpdateDisabled(package)
							select package).ToList<Package>();
						List<string> source3 = (from p in source
							select p.PackageInformation.Name).ToList<string>();
						List<string> source4 = (from p in source2
							select p.PackageInformation.Name).ToList<string>();
						foreach (DirectoryInfo directoryInfo in enumerable)
						{
							bool flag = false;
							string text = directoryInfo.Name;
							if (text.EndsWith("_"))
							{
								text = text.Substring(0, text.LastIndexOf("_"));
								flag = true;
							}
							if (!source3.Contains(text, StringComparer.InvariantCultureIgnoreCase) || (flag && source4.Contains(text, StringComparer.InvariantCultureIgnoreCase)))
							{
								Logger.Log(Logger.LogSeverity.Information, "DeleteUnusedPackages: Deleting the folder {0} since its not used anymore", new object[] { directoryInfo });
								try
								{
									if (!flag)
									{
										FileInfo[] files = new DirectoryInfo(directoryInfo.FullName).GetFiles("UnInstall.ps1", SearchOption.AllDirectories);
										if (files != null && files.Any<FileInfo>())
										{
											PackageInstaller.RunPackageInstallationCommand(directoryInfo.FullName, false);
										}
										this.DeletePluginSafely(directoryInfo);
									}
									else
									{
										directoryInfo.Delete(true);
									}
								}
								catch (Exception)
								{
								}
							}
						}
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "DeleteUnusedPackages: Exception in DeleteUnusedPackages");
						retVal = false;
					}
				}
				result = retVal;
			}
			return result;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003754 File Offset: 0x00001954
		private int GetTimeDelayDeleteUnusedPkgs()
		{
			int result = 90;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Lenovo\\ImController"))
				{
					if (registryKey != null)
					{
						object value = registryKey.GetValue("DelayDeleteUnusedPkgsInMins");
						if (value != null)
						{
							double num = 0.0;
							if (double.TryParse(Convert.ToString(value), out num))
							{
								result = ((num < 1.0) ? 1 : Convert.ToInt32(num));
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "GetTimeDelayDeleteUnusedPkgs: Exception in GetTimeDelayDeleteUnusedPkgs");
			}
			return result;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000037EC File Offset: 0x000019EC
		public override void Start(CancellationToken cancelToken)
		{
			PackageUpdater.<>c__DisplayClass26_0 CS$<>8__locals1 = new PackageUpdater.<>c__DisplayClass26_0();
			CS$<>8__locals1.<>4__this = this;
			this._cancelToken = cancelToken;
			Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Start Entry");
			CS$<>8__locals1.serviceIsCurrent = false;
			try
			{
				Task.Run(delegate()
				{
					PackageUpdater.<>c__DisplayClass26_0.<<Start>b__0>d <<Start>b__0>d;
					<<Start>b__0>d.<>4__this = CS$<>8__locals1;
					<<Start>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<Start>b__0>d.<>1__state = -1;
					AsyncTaskMethodBuilder <>t__builder = <<Start>b__0>d.<>t__builder;
					<>t__builder.Start<PackageUpdater.<>c__DisplayClass26_0.<<Start>b__0>d>(ref <<Start>b__0>d);
					return <<Start>b__0>d.<>t__builder.Task;
				}).Wait(this._cancelToken);
			}
			catch (Exception)
			{
			}
			if (!CS$<>8__locals1.serviceIsCurrent || this._cancelToken.IsCancellationRequested)
			{
				return;
			}
			this._updater = new Thread(new ThreadStart(this.UpdateWorker));
			this._updater.Start();
			this._applyPendingUpdateTimer = new System.Timers.Timer((double)Constants.PendingUpdateTimerInterval);
			this._applyPendingUpdateTimer.Elapsed += delegate(object s, ElapsedEventArgs e)
			{
				CS$<>8__locals1.<>4__this.OnTimerEvent();
			};
			this._applyPendingUpdateTimer.AutoReset = true;
			this._applyPendingUpdateTimer.Enabled = true;
			Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Start Exit");
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000038D4 File Offset: 0x00001AD4
		public override void StopAndWait()
		{
			Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Received request to exit thread");
			if (this._applyPendingUpdateTimer != null)
			{
				try
				{
					this._applyPendingUpdateTimer.Enabled = false;
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "Exception in PackageUpdater.StopAndWait");
				}
			}
			base.SetStop();
			if (this._updater != null)
			{
				this._updater.Join();
			}
			Logger.Log(Logger.LogSeverity.Information, "PackageUpdater: Updater thread exited");
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003944 File Offset: 0x00001B44
		public override void ApplyPendingUpdates()
		{
			Task.Run(async delegate()
			{
				try
				{
					await this.ApplyPendingPluginFoldersAsync(false);
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "Exception in PackageUpdater.DoInstallOnce");
				}
			}).Wait();
		}

		// Token: 0x0400001E RID: 30
		private const string CorePluginName = "GenericCorePlugin";

		// Token: 0x0400001F RID: 31
		private readonly IPackageHistory _packageHistory;

		// Token: 0x04000020 RID: 32
		private readonly ISubscriptionManager _subscriptionManager;

		// Token: 0x04000021 RID: 33
		private readonly IInstallManager _installManager;

		// Token: 0x04000022 RID: 34
		private Thread _updater;

		// Token: 0x04000023 RID: 35
		private CancellationToken _cancelToken;

		// Token: 0x04000024 RID: 36
		private readonly SemaphoreSlim _installSem;

		// Token: 0x04000025 RID: 37
		private PluginRepository _pluginRepository;

		// Token: 0x04000026 RID: 38
		private ConcurrentQueue<Package> _listOfPackagesThatNeedUpdating = new ConcurrentQueue<Package>();

		// Token: 0x04000027 RID: 39
		private System.Timers.Timer _applyPendingUpdateTimer;
	}
}
