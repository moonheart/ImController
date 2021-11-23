using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Lenovo.Modern.ImController.Shared;
using Lenovo.Modern.ImController.Shared.Model;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Telemetry;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.SystemContext.ProcessLauncher;
using Lenovo.Modern.Utilities.Services.Validation;

namespace Lenovo.Modern.ImController.UpdateManager.Services
{
	// Token: 0x02000005 RID: 5
	public class InstallManager : IInstallManager
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000021A8 File Offset: 0x000003A8
		public InstallManager(IPackageHistory packageHistory, IImcCertificateValidator certValidator)
		{
			if (packageHistory == null || certValidator == null)
			{
				throw new ArgumentNullException("Missing reqs for InstallManager");
			}
			this._packageHistory = packageHistory;
			this._certValidator = certValidator;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021D0 File Offset: 0x000003D0
		public int GetNumberOfInstallAttempts(string packageCacheName, string versionNumber)
		{
			int result = 0;
			CacheInformation packageInformationFromCache = this._packageHistory.GetPackageInformationFromCache(packageCacheName, versionNumber);
			if (packageInformationFromCache != null)
			{
				result = packageInformationFromCache.NumberOfInstallAttempts;
			}
			return result;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021F8 File Offset: 0x000003F8
		public bool IsInstallerValid(string installerFilePath)
		{
			bool result = false;
			if (!string.IsNullOrWhiteSpace(installerFilePath) && File.Exists(installerFilePath))
			{
				if (Constants.IsSecurityDisabled)
				{
					Logger.Log(Logger.LogSeverity.Critical, "Warning: Security has been disabled for installer");
					result = true;
				}
				else if (this._certValidator.AssertDigitalSignatureIsValid(installerFilePath))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002240 File Offset: 0x00000440
		public void RunMsInstaller(string cabFilePath)
		{
			Logger.Log(Logger.LogSeverity.Information, "RunMsInstaller: Entry");
			if (Utility.SanitizePath(ref cabFilePath) && ".cab".Equals(cabFilePath.Substring(cabFilePath.Length - 4).ToLower()))
			{
				string directoryName = Path.GetDirectoryName(cabFilePath);
				try
				{
					if (!string.IsNullOrEmpty(directoryName) && PackageExtractor.UnzipPackage(cabFilePath, directoryName))
					{
						string str = cabFilePath.Remove(cabFilePath.Length - 3) + "inf";
						new DirectoryInfo(directoryName);
						string[] files = Directory.GetFiles(directoryName, "imdriver.inf");
						if (!Environment.Is64BitOperatingSystem)
						{
							files = Directory.GetFiles(directoryName, "imdriver_x86.inf");
						}
						if (files.Any<string>())
						{
							str = files[0];
						}
						string text = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\sysnative\\" + Constants.InfInstaller;
						files = Directory.GetFiles(directoryName, Constants.InfInstallerSource);
						if (!Environment.Is64BitOperatingSystem)
						{
							text = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\system32\\" + Constants.InfInstaller;
							files = Directory.GetFiles(directoryName, Constants.InfInstallerSourcex86);
						}
						if (files.Any<string>())
						{
							try
							{
								File.Copy(files[0], text, true);
							}
							catch (Exception)
							{
							}
						}
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
							EventFactory.Constants.UpdateResult result = EventFactory.Constants.UpdateResult.Fail;
							try
							{
								Logger.Log(Logger.LogSeverity.Information, "Starting IMC installer process now.  File: {0}, Args: {1}", new object[] { processStartInfo.FileName, processStartInfo.Arguments });
								if (SystemProcessLauncher.Instance.LaunchSystemProcessInUserSession(text, processStartInfo.Arguments, Path.GetDirectoryName(text), true) == null)
								{
									Process.Start(processStartInfo);
								}
								result = EventFactory.Constants.UpdateResult.Success;
							}
							catch (Exception ex)
							{
								Logger.Log(ex, "Can't start installer for service self auto-update");
							}
							ImcEvent userEvent = EventFactory.CreateImcUpdateEvent(EventFactory.Constants.UpdateType.Service, EventFactory.Constants.UpdateAction.Install, result, "");
							EventLogger.GetInstance().LogEvent(userEvent);
						}
					}
				}
				catch (Exception ex2)
				{
					Logger.Log(ex2, "RunMsInstaller: Exception occured");
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000246C File Offset: 0x0000066C
		public void RunInstaller(string installerFilePath, bool fistAttempt)
		{
			Logger.Log(Logger.LogSeverity.Information, "RunInstaller: Entry");
			if (Utility.SanitizePath(ref installerFilePath))
			{
				bool flag = ".msi".Equals(installerFilePath.Substring(installerFilePath.Length - 4).ToLower());
				bool flag2 = ".exe".Equals(installerFilePath.Substring(installerFilePath.Length - 4).ToLower());
				if (flag || flag2)
				{
					Path.GetDirectoryName(installerFilePath);
					string arguments = "";
					if (flag)
					{
						if (fistAttempt)
						{
							arguments = string.Format("/i \"{0}\" /qn /norestart REINSTALL=All REINSTALLMODE=vomusa", installerFilePath);
						}
						else
						{
							arguments = string.Format("/fvomusa \"{0}\" /qn /norestart", installerFilePath);
						}
					}
					ProcessStartInfo processStartInfo = null;
					if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
					{
						if (flag)
						{
							processStartInfo = new ProcessStartInfo
							{
								FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "sysnative\\msiexec.exe"),
								Arguments = arguments
							};
						}
						if (flag2)
						{
							string text = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Logs\\ImController";
							Directory.CreateDirectory(text);
							text += "\\ImcInstall.log";
							processStartInfo = new ProcessStartInfo
							{
								FileName = installerFilePath,
								Arguments = "/VERYSILENT /NORESTART /Log=" + text
							};
						}
					}
					else
					{
						processStartInfo = new ProcessStartInfo
						{
							FileName = "msiexec.exe",
							Arguments = arguments
						};
					}
					if (processStartInfo != null)
					{
						EventFactory.Constants.UpdateResult result = EventFactory.Constants.UpdateResult.Fail;
						try
						{
							Logger.Log(Logger.LogSeverity.Information, "Starting IMC installer process now.  File: {0}, Args: {1}", new object[] { processStartInfo.FileName, processStartInfo.Arguments });
							if (flag2)
							{
								SystemProcessLauncher.Instance.LaunchSystemProcessInUserSession(processStartInfo.FileName, processStartInfo.Arguments, Path.GetDirectoryName(processStartInfo.FileName), false);
							}
							else
							{
								Process.Start(processStartInfo);
							}
							result = EventFactory.Constants.UpdateResult.Success;
						}
						catch (Exception ex)
						{
							Logger.Log(ex, "Can't start installer for service self auto-update");
						}
						ImcEvent userEvent = EventFactory.CreateImcUpdateEvent(EventFactory.Constants.UpdateType.Service, EventFactory.Constants.UpdateAction.Install, result, "");
						EventLogger.GetInstance().LogEvent(userEvent);
					}
				}
			}
		}

		// Token: 0x04000015 RID: 21
		private IPackageHistory _packageHistory;

		// Token: 0x04000016 RID: 22
		private IImcCertificateValidator _certValidator;
	}
}
