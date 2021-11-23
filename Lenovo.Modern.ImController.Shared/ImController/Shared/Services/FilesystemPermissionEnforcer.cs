using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x02000008 RID: 8
	public static class FilesystemPermissionEnforcer
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002A74 File Offset: 0x00000C74
		public static void EnforcePermissionsForService()
		{
			string text = Environment.ExpandEnvironmentVariables(Constants.ImControllerCoreDataFolder);
			if (Directory.Exists(text))
			{
				FilesystemPermissionEnforcer.EnforceOwnerhsipForService(text);
				FilesystemPermissionEnforcer.ApplyPermissionsOnFolder(text, "/inheritance:r /q /grant *S-1-5-32-545:(OI)(CI)RX *S-1-5-32-544:(OI)(CI)F *S-1-5-18:(OI)(CI)F");
				FilesystemPermissionEnforcer.ApplyPermissionsOnFolder(text + "\\*", "/q /c /t /inheritance:e");
				FilesystemPermissionEnforcer.ApplyPermissionsOnFolder(text + "\\*", "/q /c /t /reset");
				try
				{
					string path = Environment.ExpandEnvironmentVariables(Constants.SystemPluginDataFolderLocation);
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "EnforcePermissionsForService: Exception occured for SystemPluginData folder");
				}
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002B08 File Offset: 0x00000D08
		public static void EnforceOwnerhsipForService(string imcFolder)
		{
			FilesystemPermissionEnforcer.TakeOwnershipForSystem("/F " + Environment.ExpandEnvironmentVariables(Constants.ImControllerCoreDataFolder) + "\\*.xml", false);
			FilesystemPermissionEnforcer.TakeOwnershipForSystem("/F " + Environment.ExpandEnvironmentVariables(Constants.SharedFolderTempLocation), true);
			FilesystemPermissionEnforcer.TakeOwnershipForSystem("/F " + Environment.ExpandEnvironmentVariables(Constants.SystemPluginDataFolderLocation), true);
			FilesystemPermissionEnforcer.TakeOwnershipForSystem("/F " + Environment.ExpandEnvironmentVariables(Constants.PluginsFolderLocation), true);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002B84 File Offset: 0x00000D84
		internal static void ApplyPermissionsOnFolder(string fullPathToFolder, string permissions)
		{
			try
			{
				if (!Utility.SanitizePath(ref fullPathToFolder))
				{
					Logger.Log(Logger.LogSeverity.Error, "ApplyPermissionsOnFolder: Failed to apply permission to directory as path is invalid. Path - {0}", new object[] { fullPathToFolder });
				}
				else if (!Utility.Sanitize(ref permissions))
				{
					Logger.Log(Logger.LogSeverity.Error, "ApplyPermissionsOnFolder: Failed to apply permissions to directory as permission string is invalid. Permissions - {0}", new object[] { permissions });
				}
				else
				{
					Process process = Process.Start(new ProcessStartInfo
					{
						FileName = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\icacls.exe",
						Arguments = fullPathToFolder + " " + permissions,
						CreateNoWindow = true,
						WindowStyle = ProcessWindowStyle.Hidden,
						UseShellExecute = false
					});
					if (process != null)
					{
						process.WaitForExit(300000);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "ApplyPermissionsOnFolder: Exception occured for folder {0}", new object[] { fullPathToFolder });
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002C54 File Offset: 0x00000E54
		internal static void TakeOwnershipForSystem(string fullPathToFolder, bool recursive)
		{
			try
			{
				if (!Utility.SanitizePath(ref fullPathToFolder))
				{
					Logger.Log(Logger.LogSeverity.Error, "TakeOwnershipForSystem: Failed to take ownership for folder as path is invalid. Path - {0}", new object[] { fullPathToFolder });
				}
				else
				{
					ProcessStartInfo processStartInfo = new ProcessStartInfo
					{
						FileName = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\takeown.exe",
						Arguments = fullPathToFolder,
						CreateNoWindow = true,
						WindowStyle = ProcessWindowStyle.Hidden,
						UseShellExecute = false
					};
					if (recursive)
					{
						string takeOwnAnswerForCurrentCulture = FilesystemPermissionEnforcer.GetTakeOwnAnswerForCurrentCulture();
						processStartInfo.Arguments = processStartInfo.Arguments + " /R /D " + takeOwnAnswerForCurrentCulture;
					}
					Process process = Process.Start(processStartInfo);
					if (process != null)
					{
						process.WaitForExit(300000);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "ApplyPermissionsOnFolder: Exception occured for folder {0}", new object[] { fullPathToFolder });
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002D1C File Offset: 0x00000F1C
		private static string GetTakeOwnAnswerForCurrentCulture()
		{
			if (string.IsNullOrWhiteSpace(FilesystemPermissionEnforcer._recursionPromptAnswer))
			{
				FilesystemPermissionEnforcer._recursionPromptAnswer = "Y";
				FilesystemPermissionEnforcer._recursionPromptAnswer = FilesystemPermissionEnforcer.GetStringResourceForCulture("takeown.exe.mui", CultureInfo.CurrentCulture.ToString(), 410);
			}
			return FilesystemPermissionEnforcer._recursionPromptAnswer;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002D57 File Offset: 0x00000F57
		private static string GetStringResourceForCulture(string muiResourceFileName, string culture, int resourceId)
		{
			return FilesystemPermissionEnforcer.GetMuiString(string.Concat(new string[]
			{
				Environment.GetFolderPath(Environment.SpecialFolder.System),
				"\\",
				culture,
				"\\",
				muiResourceFileName
			}), resourceId);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002D8C File Offset: 0x00000F8C
		private static string GetMuiString(string resourceFile, int resourceId)
		{
			StringBuilder stringBuilder = new StringBuilder(2048);
			try
			{
				IntPtr intPtr = FilesystemPermissionEnforcer.LoadLibrary(resourceFile);
				FilesystemPermissionEnforcer.LoadString(intPtr, resourceId, stringBuilder, stringBuilder.Capacity);
				FilesystemPermissionEnforcer.FreeLibrary(intPtr);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "FileSystemPermissionEnforcer.GetMuiString: Exception occured for file {0} and resId {1}", new object[] { resourceFile, resourceId });
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000024 RID: 36
		[DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
		private static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

		// Token: 0x06000025 RID: 37
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern int LoadString(IntPtr hInstance, int ID, StringBuilder lpBuffer, int nBufferMax);

		// Token: 0x06000026 RID: 38
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool FreeLibrary(IntPtr hModule);

		// Token: 0x0400004B RID: 75
		private static string _recursionPromptAnswer;
	}
}
