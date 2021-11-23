using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using IWshRuntimeLibrary;
using Lenovo.Modern.CoreTypes.Contracts.AppLauncher;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Validation;
using Lenovo.Modern.Utilities.Services.Wrappers.Process;
using Microsoft.CSharp.RuntimeBinder;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppLauncher
{
	// Token: 0x0200003D RID: 61
	public class AppLauncherAgent
	{
		// Token: 0x0600016A RID: 362
		[DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
		[DllImport("SMBiosInformationRetriever.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern IntPtr GetLinkTargetAndArguments(StringBuilder shortcutFilepath, StringBuilder targetPath, int targetpathSize, StringBuilder arguments, int argumentsSize);

		// Token: 0x0600016B RID: 363 RVA: 0x0000A2F3 File Offset: 0x000084F3
		public AppLauncherAgent()
		{
			this.locationWhiteList = new List<string>();
			this.AddLocationWhiteList();
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000A30C File Offset: 0x0000850C
		public AppLaunchResponse LaunchDesktopApp(DesktopAppLaunchRequest requestXml)
		{
			AppLaunchResponse appLaunchResponse = new AppLaunchResponse
			{
				Result = new Result
				{
					Success = "false"
				}
			};
			try
			{
				string newValue = string.Empty;
				bool flag = false;
				if (requestXml != null)
				{
					string text = requestXml.DesktopAppLaunchDetails.PathToLnkFile.ToString();
					Match match = new Regex("%(.*)%").Match(text);
					string text2 = string.Empty;
					if (match.Length > 0)
					{
						text2 = match.Groups[0].ToString();
						if (!string.IsNullOrEmpty(text2))
						{
							newValue = Environment.ExpandEnvironmentVariables(text2);
						}
						text = text.Replace(text2, newValue);
					}
					if (!Path.GetExtension(text).Equals(".lnk", StringComparison.OrdinalIgnoreCase))
					{
						throw new UnauthorizedAccessException("The file being launched is not a .lnk file!");
					}
					StringBuilder stringBuilder = new StringBuilder(AppLauncherAgent.MAX_SIZE);
					stringBuilder.Append(text);
					StringBuilder stringBuilder2 = new StringBuilder(AppLauncherAgent.MAX_SIZE);
					StringBuilder stringBuilder3 = new StringBuilder(AppLauncherAgent.MAX_SIZE);
					AppLauncherAgent.GetLinkTargetAndArguments(stringBuilder, stringBuilder2, AppLauncherAgent.MAX_SIZE, stringBuilder3, AppLauncherAgent.MAX_SIZE);
					ShortcutPathDetails shortcutPathDetails = new ShortcutPathDetails(stringBuilder2.ToString(), stringBuilder3.ToString());
					string path = shortcutPathDetails.Path;
					string path2 = shortcutPathDetails.Arguments.Replace("\"", string.Empty);
					if (string.IsNullOrEmpty(path))
					{
						Logger.Log(Logger.LogSeverity.Error, "Target path for the sortcut is invalid.");
					}
					if (path.Contains("rundll"))
					{
						if (this.IsWhileListPath(path2))
						{
							flag = true;
						}
					}
					else if (this.IsWhileListPath(path))
					{
						flag = true;
					}
					else if (new CertificateValidator().AssertDigitalSignatureIsValid(path))
					{
						flag = true;
					}
					else if (new ExternalCertificateValidator().AssertDigitalSignatureIsValid(path))
					{
						flag = true;
					}
					int? num;
					if (flag)
					{
						string arguments = requestXml.DesktopAppLaunchDetails.Arguments;
						num = CurrentPriviligeProcessLauncher.Instance.LaunchUserProcess(text, arguments, null, true);
					}
					else
					{
						num = null;
					}
					if (num != null)
					{
						int? num2 = num;
						int num3 = 0;
						if ((num2.GetValueOrDefault() > num3) & (num2 != null))
						{
							appLaunchResponse.Result.Success = "true";
						}
						else
						{
							appLaunchResponse.Result.Success = "false";
						}
					}
					else
					{
						appLaunchResponse.Result.Success = "false";
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Failed to Launch Desktop application");
			}
			return appLaunchResponse;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000A54C File Offset: 0x0000874C
		public AppLaunchResponse LaunchUniversalApp(UniversalAppLaunchRequest requestXml)
		{
			AppLaunchResponse appLaunchResponse = new AppLaunchResponse
			{
				Result = new Result
				{
					Success = "false"
				}
			};
			if (requestXml != null)
			{
				ApplicationActivationManager applicationActivationManager = new ApplicationActivationManager();
				uint num = 0U;
				try
				{
					applicationActivationManager.ActivateApplication(requestXml.UniversalAppLaunchDetails.PackageFamilyName, requestXml.UniversalAppLaunchDetails.Arguments, ActivateOptions.None, out num);
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "Failed to Launch Universal application");
				}
				if (num > 0U)
				{
					appLaunchResponse.Result.Success = "true";
				}
				else
				{
					appLaunchResponse.Result.Success = "false";
				}
			}
			return appLaunchResponse;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000A5E8 File Offset: 0x000087E8
		public AppLaunchResponse LaunchContorlPanelItem(ControlPanelItemLaunchRequest requestXml)
		{
			AppLaunchResponse appLaunchResponse = new AppLaunchResponse
			{
				Result = new Result
				{
					Success = "false"
				}
			};
			try
			{
				string newValue = string.Empty;
				if (requestXml == null)
				{
					throw new UnauthorizedAccessException();
				}
				string text = requestXml.DesktopAppLaunchDetails.CplFile.ToString();
				if (Path.GetExtension(text).Equals(".cpl", StringComparison.OrdinalIgnoreCase))
				{
					Match match = new Regex("%(.*)%").Match(text);
					string text2 = string.Empty;
					if (match.Length > 0)
					{
						text2 = match.Groups[0].ToString();
						if (!string.IsNullOrEmpty(text2))
						{
							newValue = Environment.ExpandEnvironmentVariables(text2);
						}
						text = text.Replace(text2, newValue);
					}
					string arguments = requestXml.DesktopAppLaunchDetails.Arguments;
					int? num = CurrentPriviligeProcessLauncher.Instance.LaunchUserProcess(text, arguments, null, true);
					if (num != null)
					{
						int? num2 = num;
						int num3 = 0;
						if ((num2.GetValueOrDefault() > num3) & (num2 != null))
						{
							appLaunchResponse.Result.Success = "true";
						}
						else
						{
							appLaunchResponse.Result.Success = "false";
						}
					}
					else
					{
						appLaunchResponse.Result.Success = "false";
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Failed to Launch Desktop application");
			}
			return appLaunchResponse;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000A740 File Offset: 0x00008940
		public AppLaunchResponse LaunchDocument(DocumentLaunchRequest request)
		{
			AppLaunchResponse appLaunchResponse = new AppLaunchResponse
			{
				Result = new Result
				{
					Success = "false"
				}
			};
			try
			{
				string newValue = string.Empty;
				if (request != null)
				{
					string text = request.DocumentLaunchDetails.PathToDocument;
					if (!Path.GetExtension(text).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
					{
						throw new UnauthorizedAccessException();
					}
					Match match = new Regex("%(.*)%").Match(text);
					string text2 = string.Empty;
					if (match.Length > 0)
					{
						text2 = match.Groups[0].ToString();
						if (!string.IsNullOrEmpty(text2))
						{
							newValue = Environment.ExpandEnvironmentVariables(text2);
						}
						text = text.Replace(text2, newValue);
					}
					string cmdLine = null;
					int? num = CurrentPriviligeProcessLauncher.Instance.LaunchUserProcess(text, cmdLine, null, true);
					if (num != null)
					{
						int? num2 = num;
						int num3 = 0;
						if ((num2.GetValueOrDefault() > num3) & (num2 != null))
						{
							appLaunchResponse.Result.Success = "true";
						}
						else
						{
							appLaunchResponse.Result.Success = "false";
						}
					}
					else
					{
						appLaunchResponse.Result.Success = "false";
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Failed to Launch document");
			}
			return appLaunchResponse;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000A87C File Offset: 0x00008A7C
		private IWshShortcut GetShortcutTargetFile(string shortcutFilename)
		{
			IWshShortcut result = null;
			if (File.Exists(shortcutFilename))
			{
				WshShell wshShell = (WshShell)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
				if (AppLauncherAgent.<>o__7.<>p__0 == null)
				{
					AppLauncherAgent.<>o__7.<>p__0 = CallSite<Func<CallSite, object, IWshShortcut>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(IWshShortcut), typeof(AppLauncherAgent)));
				}
				result = AppLauncherAgent.<>o__7.<>p__0.Target(AppLauncherAgent.<>o__7.<>p__0, wshShell.CreateShortcut(shortcutFilename));
			}
			else
			{
				Logger.Log(Logger.LogSeverity.Error, "Shortcut path is invalid with error code:" + Marshal.GetLastWin32Error().ToString());
			}
			return result;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000A918 File Offset: 0x00008B18
		private void AddLocationWhiteList()
		{
			this.locationWhiteList.Add(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
			this.locationWhiteList.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
			this.locationWhiteList.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
			this.locationWhiteList.Add(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000A970 File Offset: 0x00008B70
		private bool IsWhileListPath(string path)
		{
			return this.locationWhiteList.Any((string x) => path.StartsWith(x, StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x04000099 RID: 153
		private static readonly int MAX_SIZE = 260;

		// Token: 0x0400009A RID: 154
		private List<string> locationWhiteList;
	}
}
