using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Lenovo.Modern.ImController.PluginManager.Services;
using Lenovo.Modern.ImController.Services;
using Lenovo.Modern.ImController.Shared;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Telemetry;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.ImController.UpdateManager;
using Lenovo.Modern.ImController.UpdateManager.Services;
using Lenovo.Modern.Utilities.Patterns.Ioc;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.SystemContext.Interop;
using Lenovo.Modern.Utilities.Services.Wrappers.Network;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;

namespace Lenovo.Modern.ImController.CommandLineServices
{
	// Token: 0x02000014 RID: 20
	internal class CommadLineResponder
	{
		// Token: 0x0600004E RID: 78 RVA: 0x000042A8 File Offset: 0x000024A8
		public static bool HandleImcCommandLine(string[] args)
		{
			Logger.Setup(new Logger.Configuration
			{
				LogIdentifier = "ImController.Service",
				FileNameEnding = "Cmd",
				FileSizeRollOverKb = 3072,
				IsEnabled = new bool?(true)
			});
			Logger.Log(Logger.LogSeverity.Information, string.Format("Invoked with args: {0}", string.Join(" ", args)));
			string[] array = new string[args.Length];
			for (int i = 0; i < args.Length; i++)
			{
				array[i] = args[i].ToLowerInvariant();
			}
			Dictionary<string, string> argsDictionary = CommadLineResponder.GetArgsDictionary(array);
			bool flag = argsDictionary.ContainsKey(Constants.Command.InstallPackagesWithReboot);
			bool flag2 = argsDictionary.ContainsKey(Constants.Command.InstallPackages);
			if (flag || flag2)
			{
				string text;
				if (flag)
				{
					text = argsDictionary[Constants.Command.InstallPackagesWithReboot];
				}
				else
				{
					text = argsDictionary[Constants.Command.InstallPackages];
				}
				if (text != null)
				{
					try
					{
						CommadLineResponder.HandleInstallPackages(text, flag);
					}
					catch (Exception)
					{
						Logger.Log(Logger.LogSeverity.Information, "{0} .Exception", new object[] { flag2 ? "InstallPackages" : "InstallPackagesWithReboot" });
					}
					CommadLineResponder.FixFileSystemPermissions();
				}
				return true;
			}
			if (argsDictionary.ContainsKey(Constants.Command.GetStatus))
			{
				ImStats.Print();
				return true;
			}
			if (argsDictionary.ContainsKey(Constants.Command.InstallSubscription))
			{
				string text2 = argsDictionary[Constants.Command.InstallSubscription];
				if (text2 != null)
				{
					SubscriptionManager.GetInstance(null).UpdateSubscriptionFile(text2).Wait();
				}
				return true;
			}
			if (argsDictionary.ContainsKey(Constants.Command.UnInstallPackages))
			{
				CommadLineResponder.UninstallPackages();
				return true;
			}
			if (argsDictionary.ContainsKey(Constants.Command.ProtocolEvent) && argsDictionary.ContainsKey(Constants.Command.ProtocolEventValue))
			{
				string text3 = argsDictionary[Constants.Command.ProtocolEvent];
				string text4 = argsDictionary[Constants.Command.ProtocolEventValue];
				if (text3 != null && text4 != null)
				{
					CommadLineResponder.HandleProtocolEvent(text3, text4);
				}
				return true;
			}
			if (argsDictionary.ContainsKey(Constants.Command.TimeBasedEventTrigger))
			{
				string text5 = argsDictionary[Constants.Command.TimeBasedEventTrigger];
				if (text5 != null)
				{
					CommadLineResponder.HandleTimeBasedEventTrigger(text5);
				}
				return true;
			}
			if (argsDictionary.ContainsKey(Constants.Command.InfUninstallation))
			{
				CommadLineResponder.DoUnInstallation();
				return true;
			}
			if (argsDictionary.ContainsKey(Constants.Command.DeleteAllImcScheduledTasks))
			{
				Logger.Log(Logger.LogSeverity.Information, "DeleteAllImcScheduledTasks.Entry");
				try
				{
					CommadLineResponder.DeleteAllImcScheduledTasks();
				}
				catch (Exception)
				{
				}
				return true;
			}
			if (argsDictionary.ContainsKey(Constants.Command.DeleteImcTimebasedScheduledTasks))
			{
				Logger.Log(Logger.LogSeverity.Information, "deleteimctimebasedscheduledtasks.Entry");
				try
				{
					CommadLineResponder.DeleteImcTimebasedScheduledTasks();
				}
				catch (Exception)
				{
				}
				return true;
			}
			if (argsDictionary.ContainsKey(Constants.Command.FixPermissions))
			{
				Logger.Log(Logger.LogSeverity.Information, "FixPermissions.Entry");
				CommadLineResponder.FixFileSystemPermissions();
				return true;
			}
			if (argsDictionary.ContainsKey(Constants.Command.InhibitOldImcInstaller))
			{
				Logger.Log(Logger.LogSeverity.Information, "InhibitOldImcInstaller.command");
				CommadLineResponder.InhibitOldImcInstaller();
				return true;
			}
			return false;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000453C File Offset: 0x0000273C
		private static void InhibitOldImcInstaller()
		{
			Logger.Log(Logger.LogSeverity.Information, "InhibitOldImcInstaller.Entry");
			try
			{
				int parentProcessId = ParentProcessInformation.GetParentProcessId(Process.GetCurrentProcess().Id);
				int parentProcessId2 = ParentProcessInformation.GetParentProcessId(parentProcessId);
				Logger.Log(Logger.LogSeverity.Information, "InhibitOldImcInstaller: pid={0} parent={1} grandParentPid={2}", new object[]
				{
					Process.GetCurrentProcess().Id,
					parentProcessId,
					parentProcessId2
				});
				try
				{
					FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(ParentProcessInformation.GetProcessExecutablePath(parentProcessId2));
					Version version = new Version(versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart);
					Version value = new Version("1.0.78.0");
					if (version.CompareTo(value) > 0)
					{
						Logger.Log(Logger.LogSeverity.Information, "InhibitOldImcInstaller: Not killing installer since version is {0}", new object[] { version.ToString() });
					}
					else
					{
						Logger.Log(Logger.LogSeverity.Information, "InhibitOldImcInstaller: Killing old version installer. Version={0}", new object[] { version.ToString() });
						Process.GetProcessById(parentProcessId).Kill();
					}
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "InhibitOldImcInstaller.Exception occured while trying to check version and kill parent");
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "InhibitOldImcInstaller.Exception occured");
			}
			Logger.Log(Logger.LogSeverity.Information, "InhibitOldImcInstaller.Exit");
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004668 File Offset: 0x00002868
		private static void FixFileSystemPermissions()
		{
			Logger.Log(Logger.LogSeverity.Information, "FixFileSystemPermissions.Entry");
			try
			{
				string b = new ProcessPrivilegeDetector().GetCurrentProcessPrivilege().ToString();
				if (ProcessPrivilegeDetector.RunAsPrivilege.User.ToString() == b)
				{
					Logger.Log(Logger.LogSeverity.Information, "FixFileSystemPermissions.Exiting since caller dont have admin rights");
				}
				else
				{
					FilesystemPermissionEnforcer.EnforcePermissionsForService();
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "FixFileSystemPermissions.Exception occured");
			}
			Logger.Log(Logger.LogSeverity.Information, "FixFileSystemPermissions.Exit");
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000046EC File Offset: 0x000028EC
		private static void DeleteAllImcScheduledTasks()
		{
			using (TaskService taskService = new TaskService())
			{
				try
				{
					if (taskService.RootFolder.SubFolders.Exists("Lenovo") && taskService.RootFolder.SubFolders["Lenovo"].SubFolders.Exists("ImController"))
					{
						CommadLineResponder.DeleteScheduleTaskSubFolderRecursively(taskService.RootFolder.SubFolders["Lenovo"].SubFolders["ImController"]);
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004790 File Offset: 0x00002990
		private static void DeleteImcTimebasedScheduledTasks()
		{
			using (TaskService taskService = new TaskService())
			{
				try
				{
					if (taskService.RootFolder.SubFolders.Exists("Lenovo") && taskService.RootFolder.SubFolders["Lenovo"].SubFolders.Exists("ImController") && taskService.RootFolder.SubFolders["Lenovo"].SubFolders["ImController"].SubFolders.Exists("TimeBasedEvents"))
					{
						CommadLineResponder.DeleteScheduleTaskSubFolderRecursively(taskService.RootFolder.SubFolders["Lenovo"].SubFolders["ImController"].SubFolders["TimeBasedEvents"]);
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000487C File Offset: 0x00002A7C
		private static void DeleteScheduleTaskSubFolderRecursively(TaskFolder folder)
		{
			using (new TaskService())
			{
				foreach (Task task in folder.Tasks)
				{
					try
					{
						folder.DeleteTask(task.Name, false);
					}
					catch (Exception)
					{
					}
				}
				foreach (TaskFolder folder2 in folder.SubFolders)
				{
					try
					{
						CommadLineResponder.DeleteScheduleTaskSubFolderRecursively(folder2);
					}
					catch (Exception)
					{
					}
				}
				try
				{
					TaskFolder parent = folder.Parent;
					if (parent != null)
					{
						parent.DeleteFolder(folder.Name, false);
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004974 File Offset: 0x00002B74
		private static void DoUnInstallation()
		{
			Logger.Log(Logger.LogSeverity.Information, "DoUnInstallation.Entry");
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004984 File Offset: 0x00002B84
		private static bool HandleInstallPackages(string packagesLocation, bool installRebootPackages)
		{
			bool result = false;
			using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
			{
				EventLogger.GetInstance().StartOrResume(cancellationTokenSource.Token);
				IUpdateManager updateManagerInstance = UpdateManagerFactory.GetUpdateManagerInstance();
				if (updateManagerInstance != null)
				{
					try
					{
						Bootstrap.RegisterComponents();
						InstanceContainer.GetInstance();
					}
					catch (Exception)
					{
					}
					if (CommadLineResponder.PrepareForUpdatesFromFolder(packagesLocation, installRebootPackages))
					{
						updateManagerInstance.ApplyPendingUpdates();
					}
					updateManagerInstance.StopAndWait();
					UpdateManagerFactory.DisposeUpdateManager();
					cancellationTokenSource.Cancel();
					EventLogger.GetInstance().Stop();
					PluginManager instance = PluginManager.GetInstance();
					if (instance != null)
					{
						instance.Stop(true, true);
					}
				}
			}
			return result;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004A28 File Offset: 0x00002C28
		private static bool HandleTimeBasedEventTrigger(string taskId)
		{
			bool result = false;
			try
			{
				using (RegistryKey registryKey = Registry.Users.CreateSubKey("S-1-5-19\\Software\\Lenovo\\ImController\\ScheduledTasks\\" + taskId))
				{
					try
					{
						if (registryKey != null)
						{
							registryKey.DeleteValue("LastNotificationTime");
						}
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "IMC.HandleTimeBasedEventTrigger: Exception occured while trying to delete LastNotificationTime");
					}
					try
					{
						if (registryKey != null)
						{
							registryKey.SetValue("ExecutionTime", DateTime.Now.ToUniversalTime().ToString());
						}
					}
					catch (Exception ex2)
					{
						Logger.Log(ex2, "IMC.HandleTimeBasedEventTrigger: Exception occured while trying to set ExecutionTime");
					}
				}
			}
			catch (Exception ex3)
			{
				Logger.Log(ex3, "IMC.HandleTimeBasedEventTrigger: Exception");
			}
			return result;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004AEC File Offset: 0x00002CEC
		private static bool HandleProtocolEvent(string packageName, string protocolEventValue)
		{
			bool result = false;
			try
			{
				PackageSubscription result2 = SubscriptionManager.GetInstance(new NetworkAgent()).GetSubscriptionAsync(CancellationToken.None).Result;
				foreach (Package package in result2.PackageList)
				{
					if (package.PackageInformation.Name.Equals(packageName, StringComparison.InvariantCultureIgnoreCase) && SubscribedPackageManager.IsPackageApplicable(result2, package, default(CancellationToken)))
					{
						try
						{
							string text = "Software\\Lenovo\\ImController\\PluginData\\" + packageName;
							Logger.Log(Logger.LogSeverity.Information, "ImController: Handling event for protocol whose regKey is {0} and Param is {1}", new object[] { text, protocolEventValue });
							RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(text);
							if (registryKey != null)
							{
								string value = protocolEventValue + "~" + Guid.NewGuid().ToString();
								registryKey.SetValue(Constants.protocolEventRegValueName, value);
								result = true;
							}
						}
						catch (Exception ex)
						{
							Logger.Log(ex, "IMC.ProtocolEventHandler: Exception");
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "IMC.ProtocolEventHandler: Exception");
			}
			return result;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004C24 File Offset: 0x00002E24
		private static Dictionary<string, string> GetArgsDictionary(string[] lowerCaseArgs)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			for (int i = 0; i < lowerCaseArgs.Count<string>(); i++)
			{
				if (lowerCaseArgs[i].StartsWith("/") && lowerCaseArgs[i].Length > 1)
				{
					if (i + 1 < lowerCaseArgs.Count<string>() && !lowerCaseArgs[i + 1].StartsWith("/"))
					{
						dictionary.Add(lowerCaseArgs[i].Substring(1), lowerCaseArgs[i + 1]);
					}
					else
					{
						dictionary.Add(lowerCaseArgs[i].Substring(1), null);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004CAC File Offset: 0x00002EAC
		private static bool UninstallPackages()
		{
			bool result = false;
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(InstallationLocator.GetPluginInstallationLocation());
				if (directoryInfo.Exists)
				{
					DirectoryInfo[] directories = directoryInfo.GetDirectories();
					if (directories != null)
					{
						PluginRepository pluginRepository = new PluginRepository();
						foreach (DirectoryInfo directoryInfo2 in directories)
						{
							if (directoryInfo2.Exists)
							{
								try
								{
									result = pluginRepository.UninstallPackage(directoryInfo2.Name, true).Result;
								}
								catch (Exception)
								{
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "UninstallPackages: Exception occured");
			}
			return result;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004D48 File Offset: 0x00002F48
		private static bool PrepareForUpdatesFromFolder(string packagesLocation, bool installRebootPackages)
		{
			int num = 0;
			bool result = false;
			if (!Utility.SanitizePath(ref packagesLocation))
			{
				Logger.Log(Logger.LogSeverity.Error, "PrepareForUpdatesFromFolder: Failed to prepare updates as package location path is invalid. Path - {0}", new object[] { packagesLocation });
				return result;
			}
			if (Directory.Exists(packagesLocation))
			{
				try
				{
					PluginRepository pluginRepository = new PluginRepository();
					DirectoryInfo directoryInfo = new DirectoryInfo(packagesLocation);
					string text = Path.Combine(Environment.ExpandEnvironmentVariables(Constants.ImControllerCoreDataFolder), "Temp");
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					PackageSubscription result2 = SubscriptionManager.GetInstance(new NetworkAgent()).GetSubscriptionAsync(CancellationToken.None).Result;
					PluginManager instance = PluginManager.GetInstance();
					if (instance != null)
					{
						instance.SetPackageSubscription(result2);
					}
					foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*.cab"))
					{
						result = true;
						try
						{
							string text2 = text + "\\" + fileInfo.ToString();
							FileSystemInfo fileSystemInfo = fileInfo.CopyTo(text2, true);
							string packageName = fileInfo.Name.Remove(fileInfo.Name.Length - 4);
							Package package = result2.PackageList.FirstOrDefault((Package p) => p != null && p.PackageInformation != null && (!string.IsNullOrWhiteSpace(p.PackageInformation.Name) & p.PackageInformation.Name.Equals(packageName, StringComparison.InvariantCultureIgnoreCase)));
							if (package != null)
							{
								if (SubscribedPackageManager.IsPackageApplicable(result2, package, default(CancellationToken)))
								{
									bool pendingFolderCandidate = false;
									if (!installRebootPackages && !ServiceEventHandler.IsWindowsInAuditBoot())
									{
										pendingFolderCandidate = PackageSettingsAgent.DoesPackageInstallRequireReboot(package);
									}
									if (pluginRepository.PreInstallPackageAsync(text2, pendingFolderCandidate).Result)
									{
										num++;
									}
								}
								else
								{
									Logger.Log(Logger.LogSeverity.Information, "PrepareForUpdatesFromFolder: Package " + packageName + " not applicable");
								}
							}
							else
							{
								Logger.Log(Logger.LogSeverity.Information, "PrepareForUpdatesFromFolder: Package " + packageName + " not found in subscription");
							}
							fileSystemInfo.Delete();
						}
						catch (Exception ex)
						{
							Logger.Log(ex, "PrepareForUpdatesFromFolder: Exception while processing CAB file " + ((fileInfo != null) ? fileInfo.FullName : null));
						}
					}
				}
				catch (Exception ex2)
				{
					Logger.Log(ex2, "PrepareForUpdatesFromFolder: Exception occured");
				}
			}
			Logger.Log(Logger.LogSeverity.Information, string.Format("PrepareForUpdatesFromFolder: Completed installing {0} items", num));
			return result;
		}
	}
}
