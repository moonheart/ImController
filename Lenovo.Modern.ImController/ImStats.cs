using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Network;

namespace Lenovo.Modern.ImController
{
	// Token: 0x0200000F RID: 15
	internal class ImStats
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002D84 File Offset: 0x00000F84
		public static void Print()
		{
			string filePath = "c:\\windows\\lenovo\\ImController\\Service\\Lenovo.Modern.ImController.exe";
			string filePath2 = "c:\\Windows\\System32\\drivers\\UMDF\\iMDriver.dll";
			string filePath3 = "c:\\Windows\\System32\\iMDriverHelper.dll";
			string filePath4 = "c:\\ProgramData\\Microsoft\\Windows\\DeviceMetadataStore\\66855021-e07a-46a7-a96c-070a12c6de28.devicemetadata-ms";
			string text = Utility.SanitizePath1(Environment.GetEnvironmentVariable("TEMP"));
			if (string.IsNullOrEmpty(text))
			{
				Logger.Log(Logger.LogSeverity.Error, "Logger: Failed to get temp path as it seems to invalid/tainted. path - {0}", new object[] { text });
				return;
			}
			string str = "ImController-Status.txt";
			string text2 = text + "\\" + str;
			File.WriteAllText(text2, "");
			ImStats.Dout(text2, "[Core]");
			ImStats.PrintFileVersion(text2, "Lenovo.Modern.ImController.exe", filePath);
			ImStats.PrintFileVersion(text2, "Lenovo.Modern.ImController.PluginHost.exe (32 bit)", InstallationLocator.GetPluginHostInstallationLocation(Bitness.X86));
			if (Environment.Is64BitOperatingSystem)
			{
				ImStats.PrintFileVersion(text2, "Lenovo.Modern.ImController.PluginHost.exe (64 bit)", InstallationLocator.GetPluginHostInstallationLocation(Bitness.X64));
			}
			ImStats.PrintFileVersion(text2, "iMDriver.dll", filePath2);
			ImStats.PrintFileVersion(text2, "iMDriverHelper.dll", filePath3);
			ImStats.PrintFileDateModified(text2, "66855021-e07a-46a7-a96c-070a12c6de28.devicemetadata-ms", filePath4);
			ImStats.Dout(text2, "\n[Packages]");
			PackageSubscription result = SubscriptionManager.GetInstance(new NetworkAgent()).GetSubscriptionAsync(CancellationToken.None).Result;
			PluginRepository pluginRepository = new PluginRepository();
			foreach (Package package in result.PackageList)
			{
				if (SubscribedPackageManager.IsPackageApplicable(result, package, default(CancellationToken)))
				{
					try
					{
						PluginRepository.PluginInformation pluginPathWithPluginName = pluginRepository.GetPluginPathWithPluginName(package.PackageInformation.Name);
						if (pluginPathWithPluginName != null)
						{
							ImStats.PrintFileVersion(text2, package.PackageInformation.Name, pluginPathWithPluginName.PathToPlugin);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			ImStats.Dout(text2, "\n[PendingUpdates]");
			foreach (Package package2 in result.PackageList)
			{
				if (SubscribedPackageManager.IsPackageApplicable(result, package2, default(CancellationToken)))
				{
					try
					{
						string pendingUpdateFilePath = ImStats.GetPendingUpdateFilePath(package2.PackageInformation);
						if (!string.IsNullOrWhiteSpace(pendingUpdateFilePath))
						{
							ImStats.PrintFileVersion(text2, package2.PackageInformation.Name, pendingUpdateFilePath);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			Logger.Log(Logger.LogSeverity.Information, "This output was saved to file: {0}", new object[] { text2 });
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002FE4 File Offset: 0x000011E4
		private static void Dout(string file, string msg)
		{
			if (string.IsNullOrEmpty(Utility.SanitizePath1(file)))
			{
				Logger.Log(Logger.LogSeverity.Error, "Dout: Failed to update text to file as file path is invalid. Path - {0}", new object[] { file });
				return;
			}
			if (string.IsNullOrEmpty(Utility.Sanitize1(msg)))
			{
				Logger.Log(Logger.LogSeverity.Error, "Dout: Failed to update text to file as message is invalid. Message - {0}", new object[] { msg });
				return;
			}
			File.AppendAllText(file, msg + "\n");
			Logger.Log(Logger.LogSeverity.Information, msg);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003050 File Offset: 0x00001250
		private static string GetFileVersionString(string filePath)
		{
			try
			{
				if (!Utility.SanitizePath(ref filePath))
				{
					Logger.Log(Logger.LogSeverity.Error, "GetFileVersionString: Failed to get file version as file path is invalid. Path - {0}", new object[] { filePath });
					return null;
				}
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(filePath);
				return new Version(versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart).ToString();
			}
			catch (FileNotFoundException)
			{
			}
			return null;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000030C4 File Offset: 0x000012C4
		private static string GetFileModificationDateString(string filePath)
		{
			DateTime d = DateTime.MinValue;
			try
			{
				if (File.Exists(filePath))
				{
					d = File.GetLastWriteTime(filePath);
				}
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (ArgumentException)
			{
			}
			catch (PathTooLongException)
			{
			}
			catch (NotSupportedException)
			{
			}
			if (d == DateTime.MinValue)
			{
				return null;
			}
			return d.ToString();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003140 File Offset: 0x00001340
		private static void PrintFileVersion(string file, string title, string filePath)
		{
			string fileVersionString = ImStats.GetFileVersionString(filePath);
			if (!string.IsNullOrWhiteSpace(fileVersionString))
			{
				ImStats.Dout(file, title + ", " + fileVersionString);
				return;
			}
			ImStats.Dout(file, title + ", file not found");
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003180 File Offset: 0x00001380
		private static void PrintFileDateModified(string file, string title, string filePath)
		{
			string fileModificationDateString = ImStats.GetFileModificationDateString(filePath);
			if (!string.IsNullOrWhiteSpace(fileModificationDateString))
			{
				ImStats.Dout(file, title + " (Date Modified), " + fileModificationDateString);
				return;
			}
			ImStats.Dout(file, title + ", file not found");
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000031C0 File Offset: 0x000013C0
		private static string GetPendingUpdateFilePath(PackageInformation package)
		{
			string text = Path.Combine(InstallationLocator.GetPluginInstallationLocation(), package.Name) + "_";
			string text2 = string.Concat(new string[]
			{
				text,
				"\\",
				Constants.X64folder,
				"\\",
				package.Name,
				".dll"
			});
			string text3 = string.Concat(new string[]
			{
				text,
				"\\",
				Constants.X86folder,
				"\\",
				package.Name,
				".dll"
			});
			string text4 = null;
			if (Environment.Is64BitOperatingSystem && File.Exists(text2))
			{
				text4 = text2;
			}
			else if (File.Exists(text3))
			{
				text4 = text3;
			}
			if (!string.IsNullOrWhiteSpace(text4) && File.Exists(text4))
			{
				return text4;
			}
			string text5 = Path.Combine(Environment.ExpandEnvironmentVariables("%PROGRAMDATA%\\Lenovo\\iMController\\Temp"), package.Location.Split(new char[] { '/' }).Last<string>());
			string text6 = text5.Remove(text5.Length - 4);
			string text7 = string.Concat(new string[]
			{
				text6,
				"\\",
				Constants.X64folder,
				"\\",
				package.Name,
				".dll"
			});
			string text8 = string.Concat(new string[]
			{
				text6,
				"\\",
				Constants.X86folder,
				"\\",
				package.Name,
				".dll"
			});
			string text9 = null;
			if (Environment.Is64BitOperatingSystem && File.Exists(text7))
			{
				text9 = text7;
			}
			else if (File.Exists(text8))
			{
				text9 = text8;
			}
			if (!string.IsNullOrWhiteSpace(text9) && File.Exists(text9))
			{
				return text9;
			}
			return null;
		}
	}
}
