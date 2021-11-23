using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.SystemContext.ProcessLauncher;
using Lenovo.Modern.Utilities.Services.Validation;
using Lenovo.Modern.Utilities.Services.Wrappers.Process;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x02000015 RID: 21
	public class PackageInstaller
	{
		// Token: 0x0600005C RID: 92 RVA: 0x00003F9C File Offset: 0x0000219C
		public static void RunPackageInstallationCommand(string pluginFolder, bool isInstallation)
		{
			try
			{
				if (!Utility.SanitizePath(ref pluginFolder))
				{
					Logger.Log(Logger.LogSeverity.Error, "RunPackageInstallationCommand: Failed to run package installation command as path is invalid. Path - {0}", new object[] { pluginFolder });
				}
				else if (Directory.Exists(pluginFolder))
				{
					string text = Path.Combine(pluginFolder, Constants.X64folder);
					string text2 = Path.Combine(pluginFolder, Constants.X86folder);
					string text3 = "";
					string text4 = "";
					string text5 = "";
					if (Environment.Is64BitOperatingSystem && Directory.Exists(text))
					{
						bool flag = false;
						text5 = PackageInstaller.GetInstallationFileNameAndType(text, isInstallation, ref flag);
						if (flag)
						{
							if (!Environment.Is64BitProcess)
							{
								text3 = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Sysnative" + Constants.PowerShellPartialPath;
							}
							else
							{
								text3 = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\system32" + Constants.PowerShellPartialPath;
							}
							text4 = Constants.PowerShellScriptCmdLineWithSecurityDisabled + text5;
						}
					}
					if (string.IsNullOrEmpty(text4) && Directory.Exists(text2))
					{
						bool flag2 = false;
						text5 = PackageInstaller.GetInstallationFileNameAndType(text2, isInstallation, ref flag2);
						if (flag2)
						{
							text3 = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + Constants.PowerShellPartialPath;
							text4 = Constants.PowerShellScriptCmdLineWithSecurityDisabled + text5;
						}
					}
					if (File.Exists(text3) && !string.IsNullOrEmpty(text5))
					{
						bool flag3 = false;
						if (isInstallation)
						{
							flag3 = PackageInstaller.IsEarlierInstallRunning(pluginFolder, text3);
						}
						if (!flag3)
						{
							if (((IImcCertificateValidator)new ImcCertificateValidator()).AssertDigitalSignatureIsValid(text5) || Constants.IsSecurityDisabled)
							{
								IProcessLauncher instance = SystemProcessLauncher.Instance;
								Process process = null;
								try
								{
									Logger.Log(Logger.LogSeverity.Information, "PluginRepository: RunPackageInstallationCommand: Launching script file {0} {1}", new object[] { text3, text4 });
									int? num = instance.LaunchSystemProcessInUserSession(text3, text4, Path.GetDirectoryName(text3), false);
									if (num != null)
									{
										process = Process.GetProcessById(num.Value);
									}
								}
								catch (Exception)
								{
									process = Process.Start(new ProcessStartInfo
									{
										FileName = text3,
										Arguments = text4,
										CreateNoWindow = true,
										WindowStyle = ProcessWindowStyle.Hidden
									});
								}
								if (process != null && !process.WaitForExit(90000))
								{
									Logger.Log(Logger.LogSeverity.Error, "PluginRepository: RunPackageInstallationCommand: The command batch file did not finish executing within 30 seconds. {0} for {1}", new object[] { text4, pluginFolder });
								}
							}
							else
							{
								Logger.Log(Logger.LogSeverity.Error, "PluginRepository: RunPackageInstallationCommand: The installation script {0} for {1} is not signed by Lenovo", new object[] { text4, pluginFolder });
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PluginRepository: RunPackageInstallationCommand: Could not run (Installation={0}) for {1} (it might be missing)", new object[] { isInstallation, pluginFolder });
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004204 File Offset: 0x00002404
		public static bool DoesPluginPreinstallFolderExist(string pluginFolder, bool pendingFolderCandidate)
		{
			string text = InstallationLocator.GetPluginInstallationLocation();
			if (pendingFolderCandidate)
			{
				text = Environment.ExpandEnvironmentVariables(Constants.PendingPackagesTempLocation);
			}
			text = text + "\\" + pluginFolder + "_";
			return Directory.Exists(text);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004240 File Offset: 0x00002440
		public static bool DoesPluginFolderExist(string pluginFolder, bool pendingFolderCandidate)
		{
			string text = InstallationLocator.GetPluginInstallationLocation();
			if (pendingFolderCandidate)
			{
				text = Environment.ExpandEnvironmentVariables(Constants.PendingPackagesTempLocation);
			}
			text = text + "\\" + pluginFolder;
			return Directory.Exists(text);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004274 File Offset: 0x00002474
		private static string GetInstallationFileNameAndType(string folderPath, bool isItInstallation, ref bool isPowerShell)
		{
			isPowerShell = false;
			string result = "";
			if (!Utility.SanitizePath(ref folderPath))
			{
				Logger.Log(Logger.LogSeverity.Error, "GetInstallationFileNameAndType: Failed to get installation file name and type as path is invalid. Path - {0}", new object[] { folderPath });
				return result;
			}
			string text = Path.Combine(folderPath, isItInstallation ? Constants.InstallationCmdFileName : Constants.UninstallCmdFileName);
			if (File.Exists(text))
			{
				result = text;
			}
			else
			{
				text = Path.Combine(folderPath, isItInstallation ? Constants.InstallationPsFileName : Constants.UninstallPsFileName);
				if (File.Exists(text))
				{
					result = text;
					isPowerShell = true;
				}
			}
			return result;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000042F0 File Offset: 0x000024F0
		private static bool WaitIfProcessesRunning(string folderPath, string partialExeName1, string partialExeName2)
		{
			bool result = false;
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT CommandLine, ProcessID FROM Win32_Process"))
			{
				foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
				{
					try
					{
						object obj = managementBaseObject["CommandLine"];
						if (obj != null)
						{
							string text = obj.ToString();
							if (!string.IsNullOrWhiteSpace(text) && text.IndexOf(folderPath, 0, StringComparison.OrdinalIgnoreCase) >= 0)
							{
								Process processById = Process.GetProcessById(int.Parse(managementBaseObject["ProcessID"].ToString()));
								if (processById != null && ((!string.IsNullOrWhiteSpace(partialExeName1) && processById.MainModule.FileName.IndexOf(partialExeName1, 0, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrWhiteSpace(partialExeName2) && processById.MainModule.FileName.IndexOf(partialExeName2, 0, StringComparison.OrdinalIgnoreCase) >= 0)))
								{
									Logger.Log(Logger.LogSeverity.Information, "Earlier install [{0}] is still executing. Do not launch a new one but wait", new object[] { string.IsNullOrWhiteSpace(partialExeName1) ? partialExeName2 : partialExeName1 });
									result = true;
									processById.WaitForExit(150000);
								}
							}
						}
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "Exception occured in CheckRunningProcessAndWait");
					}
				}
			}
			return result;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004464 File Offset: 0x00002664
		private static bool IsEarlierInstallRunning(string prePath, string powershellPath)
		{
			bool result = false;
			try
			{
				string text = prePath;
				if (prePath.EndsWith("_"))
				{
					text = prePath.Substring(0, prePath.LastIndexOf("_"));
				}
				if (!string.IsNullOrWhiteSpace(text))
				{
					result = PackageInstaller.WaitIfProcessesRunning(text, powershellPath, "setup.exe");
				}
			}
			catch (Exception)
			{
			}
			return result;
		}
	}
}
