using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model;
using Lenovo.Modern.ImController.Shared.Services.Contracts.SystemInformation.AppTag;
using Lenovo.Modern.ImController.Shared.Telemetry;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.ImController.Shared.Utilities.Validation;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Storage;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x02000021 RID: 33
	public class PluginRepository : IPluginRepository
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x000068BD File Offset: 0x00004ABD
		public PluginRepository()
			: this(new WinFileSystem(), new PackageVerifier(), new PluginVerifier())
		{
			this._packageHistory = new PackageHistory();
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000068DF File Offset: 0x00004ADF
		public PluginRepository(IFileSystem system, IPackageVerifier packageVerifier, IPluginVerifier pluginVerifier)
		{
			this._system = system;
			this._packageVerifier = packageVerifier;
			this._pluginVerifier = pluginVerifier;
			this._machineInformationManager = MachineInformationManager.GetInstance();
			this._appTagManager = AppAndTagManager.GetInstance();
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006914 File Offset: 0x00004B14
		public async Task<bool> InstallPackageAsync(string packagePath)
		{
			bool flag = false;
			this.LoadPluginInstallationFolder();
			if (this._packageVerifier.IsPackageValid(packagePath))
			{
				string text = packagePath.Remove(packagePath.Length - 4);
				if (PackageExtractor.UnzipPackage(packagePath, text))
				{
					IDirectory directory = await this._directory.GetDirectoryAsync(text);
					string pluginInstallationLocation = InstallationLocator.GetPluginInstallationLocation();
					await directory.MoveAsync(pluginInstallationLocation, CollisionOption.ReplaceExisting);
					flag = true;
				}
			}
			if (flag)
			{
				return flag;
			}
			throw new PluginRepositoryException(string.Format("PluginRepository: Cannot install package {0}.", packagePath))
			{
				ResponseCode = 202
			};
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00006964 File Offset: 0x00004B64
		public async Task<bool> PreInstallPackageAsync(string packagePath, bool pendingFolderCandidate)
		{
			bool isInstalled = false;
			Logger.Log(Logger.LogSeverity.Information, "PreInstallPackageAsync: Entry for {0}", new object[] { packagePath });
			if (Utility.SanitizePath(ref packagePath))
			{
				string extractedPath = packagePath.Remove(packagePath.Length - 4);
				string extractedPendingPath = extractedPath + "_";
				try
				{
					string pluginInstallationFolder = InstallationLocator.GetPluginInstallationLocation();
					if (pendingFolderCandidate)
					{
						pluginInstallationFolder = Environment.ExpandEnvironmentVariables(Constants.PendingPackagesTempLocation);
					}
					if (!Directory.Exists(pluginInstallationFolder))
					{
						Logger.Log(Logger.LogSeverity.Information, "PreInstallPackageAsync: {0} folder is missing and will be created", new object[] { pluginInstallationFolder });
						Directory.CreateDirectory(pluginInstallationFolder);
					}
					this._directory = this._system.LoadDirectory(pluginInstallationFolder);
					if (this._packageVerifier.IsPackageValid(packagePath))
					{
						try
						{
							if (Directory.Exists(extractedPath))
							{
								Directory.Delete(extractedPath, true);
							}
							if (Directory.Exists(extractedPendingPath))
							{
								Directory.Delete(extractedPendingPath, true);
							}
						}
						catch (Exception ex)
						{
							Logger.Log(ex, "PreInstallPackageAsync: PreInstallPackageAsync: Could not delete a directory (it might be missing)");
						}
						if (PackageExtractor.UnzipPackage(packagePath, extractedPath))
						{
							Directory.Move(extractedPath, extractedPendingPath);
							await(await this._directory.GetDirectoryAsync(extractedPendingPath)).MoveAsync(pluginInstallationFolder, CollisionOption.ReplaceExisting);
							isInstalled = true;
							Logger.Log(Logger.LogSeverity.Information, "PreInstallPackageAsync: Package {0} was installed to {1} from {2}", new object[] { pluginInstallationFolder, packagePath, extractedPendingPath });
						}
						else
						{
							Logger.Log(Logger.LogSeverity.Error, "PreInstallPackageAsync: Package {0} failed to unzip", new object[] { packagePath });
						}
					}
					else
					{
						ImcEvent userEvent = EventFactory.CreateImcUpdateEvent(EventFactory.Constants.UpdateType.Package, EventFactory.Constants.UpdateAction.Download, EventFactory.Constants.UpdateResult.Corrupt, packagePath);
						EventLogger.GetInstance().LogEvent(userEvent);
						Logger.Log(Logger.LogSeverity.Error, "PreInstallPackageAsync: Package {0} failed validation", new object[] { packagePath });
					}
					pluginInstallationFolder = null;
				}
				catch (Exception ex2)
				{
					Logger.Log(ex2, "PluginRepository: Exception in PreInstallPackageAsync");
				}
				finally
				{
					try
					{
						if (Directory.Exists(extractedPath))
						{
							Directory.Delete(extractedPath, true);
						}
						if (Directory.Exists(extractedPendingPath))
						{
							Directory.Delete(extractedPendingPath, true);
						}
					}
					catch (Exception ex3)
					{
						Logger.Log(ex3, "PluginRepository: PreInstallPackageAsync: Could not delete a directory (it might be missing)");
					}
				}
				extractedPath = null;
				extractedPendingPath = null;
			}
			return isInstalled;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000069BC File Offset: 0x00004BBC
		public async Task<bool> FinalInstallPackage(string pluginFolder, bool skipCustomInstallPackages)
		{
			bool isInstalled = false;
			try
			{
				if (!Utility.SanitizePath(ref pluginFolder))
				{
					Logger.Log(Logger.LogSeverity.Error, "FinalInstallPackage: Failed to install package as pluginfolder path is invalid. Path - {0}", new object[] { pluginFolder });
					return isInstalled;
				}
				string pluginInstallationLocation = InstallationLocator.GetPluginInstallationLocation();
				string prePath = pluginInstallationLocation + "\\" + pluginFolder + "_";
				string finalPath = pluginInstallationLocation + "\\" + pluginFolder;
				string bkpPath = pluginInstallationLocation + "\\~" + pluginFolder;
				try
				{
					if (Directory.Exists(bkpPath))
					{
						Directory.Delete(bkpPath, true);
					}
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "PluginRepository: Expected DirectoryNotFoundException in PluginRepository.FinalInstallPackage");
				}
				TaskAwaiter<bool> taskAwaiter = SubscribedPackageManager.IsCustomInstallPrivilegeEnabled(null, pluginFolder).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					if (ImcPolicy.GetIsPackageTypeDisabled())
					{
						Logger.Log(Logger.LogSeverity.Information, "PluginRepository: Skipping Installation of Custom Installable Package due to policy: {0}", new object[] { pluginFolder });
						goto IL_272;
					}
					if (skipCustomInstallPackages)
					{
						Logger.Log(Logger.LogSeverity.Information, "PluginRepository: Skipping Installation of Custom Installable Package due to setting: {0}", new object[] { pluginFolder });
						goto IL_272;
					}
					PackageInstaller.RunPackageInstallationCommand(prePath, true);
					ImcEvent userEvent = EventFactory.CreateImcUpdateEvent(EventFactory.Constants.UpdateType.Package, EventFactory.Constants.UpdateAction.Install, EventFactory.Constants.UpdateResult.Success, pluginFolder);
					EventLogger.GetInstance().LogEvent(userEvent);
					isInstalled = true;
					try
					{
						Directory.Delete(prePath, true);
						goto IL_272;
					}
					catch (Exception ex2)
					{
						Logger.Log(ex2, "PluginRepository: Exception while deleting preinstall folder.FinalInstallPackage");
						goto IL_272;
					}
				}
				try
				{
					if (!Directory.Exists(finalPath))
					{
						Directory.CreateDirectory(finalPath);
					}
					Directory.Move(finalPath, bkpPath);
				}
				catch (Exception ex3)
				{
					Logger.Log(ex3, "PluginRepository: Exception while moving plugin folder to backup in PluginRepository.FinalInstallPackage");
				}
				try
				{
					Directory.Move(prePath, finalPath);
					isInstalled = true;
				}
				catch (Exception ex4)
				{
					Logger.Log(ex4, "PluginRepository: Exception while moving preinstall folder to final. PluginRepository.FinalInstallPackage");
				}
				ImcEvent userEvent2 = EventFactory.CreateImcUpdateEvent(EventFactory.Constants.UpdateType.Package, EventFactory.Constants.UpdateAction.Install, isInstalled ? EventFactory.Constants.UpdateResult.Success : EventFactory.Constants.UpdateResult.Fail, pluginFolder);
				EventLogger.GetInstance().LogEvent(userEvent2);
				try
				{
					if (Directory.Exists(bkpPath))
					{
						Directory.Delete(bkpPath, true);
					}
				}
				catch (Exception ex5)
				{
					Logger.Log(ex5, "PluginRepository: Exception while deleting backup folder.FinalInstallPackage");
				}
				IL_272:
				prePath = null;
				finalPath = null;
				bkpPath = null;
			}
			catch (Exception ex6)
			{
				Logger.Log(ex6, "PluginRepository: Exception in FinalInstallPackageAsync");
			}
			return isInstalled;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006A0C File Offset: 0x00004C0C
		public async Task<bool> UninstallPackage(string pluginFolder, bool skipCustomInstallCheck = false)
		{
			bool isUnInstalled = false;
			try
			{
				string pluginInstallationLocation = InstallationLocator.GetPluginInstallationLocation();
				string finalPath = pluginInstallationLocation + "\\" + pluginFolder;
				if (!skipCustomInstallCheck)
				{
					TaskAwaiter<bool> taskAwaiter = SubscribedPackageManager.IsCustomInstallPrivilegeEnabled(null, pluginFolder).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						PackageInstaller.RunPackageInstallationCommand(finalPath, false);
						isUnInstalled = true;
					}
				}
				else
				{
					PackageInstaller.RunPackageInstallationCommand(finalPath, false);
					isUnInstalled = true;
				}
				finalPath = null;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception in UninstallPackage");
			}
			return isUnInstalled;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00006A59 File Offset: 0x00004C59
		public PluginRepository.PluginInformation GetPluginPathWithPluginName(string pluginName)
		{
			return PluginRepository.GetPluginInformation(pluginName);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00006A64 File Offset: 0x00004C64
		internal static PluginRepository.PluginInformation GetPluginInformation(string pluginName)
		{
			if (!Utility.SanitizeString(ref pluginName))
			{
				Logger.Log(Logger.LogSeverity.Error, "GetPluginInformation: Failed to get plugin information as pluginName is invalid. PluginName - {0}", new object[] { pluginName });
				return null;
			}
			string path = pluginName + Constants.PluginExtension;
			string path2 = Path.Combine(InstallationLocator.GetPluginInstallationLocation(), pluginName);
			string text = Path.Combine(path2, Constants.X64folder);
			string text2 = Path.Combine(path2, Constants.X86folder);
			PluginRepository.PluginInformation pluginInformation = new PluginRepository.PluginInformation();
			string text3 = "";
			if (Environment.Is64BitOperatingSystem)
			{
				if (Directory.Exists(text))
				{
					text3 = text;
					pluginInformation.Bitness = Bitness.X64;
				}
				else if (Directory.Exists(text2))
				{
					text3 = text2;
					pluginInformation.Bitness = Bitness.X86;
				}
			}
			else
			{
				if (Directory.Exists(text2))
				{
					text3 = text2;
				}
				pluginInformation.Bitness = Bitness.X86;
			}
			if (string.IsNullOrWhiteSpace(text3))
			{
				throw new DirectoryNotFoundException(string.Format("PluginRepository: The plugin path either X86: {0}, or X64: {1}, could not be found", text2, text));
			}
			string pathToPlugin = Path.Combine(text3, path);
			pluginInformation.PathToPlugin = pathToPlugin;
			pluginInformation.PluginName = pluginName;
			try
			{
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(pluginInformation.PathToPlugin);
				pluginInformation.Version = new Version(versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PluginRepository: Exception in PluginRepository.GetPluginPathWithPluginName");
				pluginInformation.Version = null;
			}
			return pluginInformation;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00006B9C File Offset: 0x00004D9C
		private void LoadPluginInstallationFolder()
		{
			string pluginInstallationLocation = InstallationLocator.GetPluginInstallationLocation();
			this._directory = this._system.LoadDirectory(pluginInstallationLocation);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00006BC4 File Offset: 0x00004DC4
		public void UpdatePacakgeInformationInRegistry(string packageName, string version, bool incrementInstallAttempts)
		{
			CacheInformation cacheInformation = this._packageHistory.GetPackageInformationFromCache(packageName, version);
			PluginRepository.PluginInformation pluginInformation = null;
			string text = null;
			try
			{
				pluginInformation = new PluginRepository().GetPluginPathWithPluginName(packageName);
				if (pluginInformation != null && null != pluginInformation.Version)
				{
					text = pluginInformation.Version.ToString();
				}
			}
			catch (DirectoryNotFoundException)
			{
				Logger.Log(Logger.LogSeverity.Warning, "PackageUpdater: UpdateRegistry: Directory not found for {0}, perhaps it is new", new object[] { packageName });
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PackageUpdater: UpdateRegistry: Exception while retreiving installed info");
			}
			if (cacheInformation != null)
			{
				if (pluginInformation != null && null != pluginInformation.Version)
				{
					cacheInformation.Version = ((text == null) ? "0.0.0.0" : text);
				}
				if (incrementInstallAttempts)
				{
					CacheInformation cacheInformation2 = cacheInformation;
					int numberOfInstallAttempts = cacheInformation2.NumberOfInstallAttempts;
					cacheInformation2.NumberOfInstallAttempts = numberOfInstallAttempts + 1;
				}
			}
			else
			{
				cacheInformation = new CacheInformation
				{
					Name = packageName,
					DateLastModified = "",
					Location = "",
					Version = ((text == null) ? "0.0.0.0" : text),
					NumberOfInstallAttempts = 1
				};
			}
			this._packageHistory.CachePackageInformation(cacheInformation, !incrementInstallAttempts);
		}

		// Token: 0x04000087 RID: 135
		private IDirectory _directory;

		// Token: 0x04000088 RID: 136
		private readonly IFileSystem _system;

		// Token: 0x04000089 RID: 137
		private readonly IPackageVerifier _packageVerifier;

		// Token: 0x0400008A RID: 138
		private readonly IPluginVerifier _pluginVerifier;

		// Token: 0x0400008B RID: 139
		private readonly IMachineInformationManager _machineInformationManager;

		// Token: 0x0400008C RID: 140
		private readonly IAppAndTagManager _appTagManager;

		// Token: 0x0400008D RID: 141
		private readonly PackageHistory _packageHistory;

		// Token: 0x02000075 RID: 117
		public class PluginInformation
		{
			// Token: 0x1700005D RID: 93
			// (get) Token: 0x06000260 RID: 608 RVA: 0x0000CAF7 File Offset: 0x0000ACF7
			// (set) Token: 0x0600025F RID: 607 RVA: 0x0000CAEE File Offset: 0x0000ACEE
			public string PathToPlugin { get; set; }

			// Token: 0x1700005E RID: 94
			// (get) Token: 0x06000262 RID: 610 RVA: 0x0000CB08 File Offset: 0x0000AD08
			// (set) Token: 0x06000261 RID: 609 RVA: 0x0000CAFF File Offset: 0x0000ACFF
			public string PluginName { get; set; }

			// Token: 0x1700005F RID: 95
			// (get) Token: 0x06000264 RID: 612 RVA: 0x0000CB19 File Offset: 0x0000AD19
			// (set) Token: 0x06000263 RID: 611 RVA: 0x0000CB10 File Offset: 0x0000AD10
			public Bitness Bitness { get; set; }

			// Token: 0x17000060 RID: 96
			// (get) Token: 0x06000266 RID: 614 RVA: 0x0000CB2A File Offset: 0x0000AD2A
			// (set) Token: 0x06000265 RID: 613 RVA: 0x0000CB21 File Offset: 0x0000AD21
			public Version Version { get; set; }
		}
	}
}
