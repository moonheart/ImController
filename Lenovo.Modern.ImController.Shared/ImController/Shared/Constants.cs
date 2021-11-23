using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Lenovo.Modern.ImController.Shared.Utilities;

namespace Lenovo.Modern.ImController.Shared
{
	// Token: 0x02000004 RID: 4
	public static class Constants
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002084 File Offset: 0x00000284
		public static bool IsSecurityDisabled
		{
			get
			{
				if (Constants._cachedIsSecurityDisabled == null)
				{
					string text = ConfigurationSettings.AppSettings["DisableSecurity"];
					if (!string.IsNullOrWhiteSpace(text) && text.Equals("true", StringComparison.OrdinalIgnoreCase))
					{
						Constants._cachedIsSecurityDisabled = new bool?(true);
					}
					else
					{
						Constants._cachedIsSecurityDisabled = new bool?(false);
					}
				}
				return Constants._cachedIsSecurityDisabled.Value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000020E8 File Offset: 0x000002E8
		public static string ImControllerVersion
		{
			get
			{
				if (Constants._cachedImControllerVersion == null)
				{
					string text = ConfigurationSettings.AppSettings["VersionOverride"];
					if (!string.IsNullOrWhiteSpace(text))
					{
						Constants._cachedImControllerVersion = text.Trim();
					}
					else
					{
						Assembly entryAssembly = Assembly.GetEntryAssembly();
						string cachedImControllerVersion;
						if (entryAssembly == null)
						{
							cachedImControllerVersion = null;
						}
						else
						{
							AssemblyName name = entryAssembly.GetName();
							if (name == null)
							{
								cachedImControllerVersion = null;
							}
							else
							{
								Version version = name.Version;
								cachedImControllerVersion = ((version != null) ? version.ToString() : null);
							}
						}
						Constants._cachedImControllerVersion = cachedImControllerVersion;
					}
				}
				return Constants._cachedImControllerVersion ?? "0.0.0.0";
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002160 File Offset: 0x00000360
		public static string ImDriverVersion
		{
			get
			{
				if (Constants._cachedDriverVersion == null)
				{
					Func<string, string> func = delegate(string path)
					{
						if (!Utility.SanitizePath(ref path))
						{
							return null;
						}
						if (!File.Exists(path))
						{
							return null;
						}
						try
						{
							FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(path);
							return string.Format("{0}.{1}.{2}.{3}", new object[] { versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart });
						}
						catch
						{
						}
						return null;
					};
					string arg = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SysNative", "drivers\\UMDF", "iMDriver.dll");
					string arg2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "drivers\\UMDF", "iMDriver.dll");
					string cachedDriverVersion;
					if ((cachedDriverVersion = func(arg)) == null)
					{
						cachedDriverVersion = func(arg2) ?? "0.0.0.0";
					}
					Constants._cachedDriverVersion = cachedDriverVersion;
				}
				return Constants._cachedDriverVersion ?? "0.0.0.0";
			}
		}

		// Token: 0x04000001 RID: 1
		private static bool? _cachedIsSecurityDisabled;

		// Token: 0x04000002 RID: 2
		public const bool DoNotUpdateSubscription = false;

		// Token: 0x04000003 RID: 3
		public static readonly string PluginHostFileName = "Lenovo.Modern.ImController.PluginHost.exe";

		// Token: 0x04000004 RID: 4
		public static readonly string PluginHostFileNameLowerCaseFriendly = "lenovo.modern.imcontroller.pluginhost";

		// Token: 0x04000005 RID: 5
		public static readonly string PluginHostFriendlyName = "SIFPluginHost";

		// Token: 0x04000006 RID: 6
		public static readonly string PluginHostInstallationRelativePath = "Lenovo\\iMController\\PluginHost";

		// Token: 0x04000007 RID: 7
		public static readonly string PluginHost86InstallationRelativePath = "Lenovo\\iMController\\PluginHost86";

		// Token: 0x04000008 RID: 8
		public static readonly string PluginsInstallationRelativePath = "Lenovo\\iMController\\Plugins";

		// Token: 0x04000009 RID: 9
		public static readonly string ImControllerServiceDisplayName = "System Interface Foundation Service";

		// Token: 0x0400000A RID: 10
		public static readonly string ImControllerServiceName = "ImControllerService";

		// Token: 0x0400000B RID: 11
		public static readonly string ImControllerServiceDescription = "Provides core functionality to Lenovo applications. If this service is disabled, some Lenovo applications will not work properly.";

		// Token: 0x0400000C RID: 12
		public static readonly string ImControllerMetricsProductName = "imc";

		// Token: 0x0400000D RID: 13
		public static readonly string PluginExtension = ".dll";

		// Token: 0x0400000E RID: 14
		public static readonly string PluginEntry = ".EntryPoint";

		// Token: 0x0400000F RID: 15
		internal static readonly string BinariesSigned = "Are all binaries Signed";

		// Token: 0x04000010 RID: 16
		internal static readonly string ManifestExists = "Is Manifest Exists";

		// Token: 0x04000011 RID: 17
		internal static readonly string ManifestValid = "Is Mainfest Valid";

		// Token: 0x04000012 RID: 18
		internal static readonly string ManifestSigned = "Is Manifest Signed";

		// Token: 0x04000013 RID: 19
		internal static readonly string IsPluginExists = "Is Plugin Exists";

		// Token: 0x04000014 RID: 20
		internal static readonly string IsPluginValid = "Is Plugin Valid";

		// Token: 0x04000015 RID: 21
		internal static readonly string IsPlatformFolderExists = "Is Platform Folder Exists";

		// Token: 0x04000016 RID: 22
		internal static readonly string DateFormat = "yyyy-MM-dd";

		// Token: 0x04000017 RID: 23
		internal static readonly string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

		// Token: 0x04000018 RID: 24
		internal static readonly string SubscriptionFileUrlVersionedFormat = "https://filedownload.lenovo.com/enm/sift/config/ImControllerSubscription-{0}.{1}.xml";

		// Token: 0x04000019 RID: 25
		internal static readonly string SubscriptionFileTempLocation = "%PROGRAMDATA%\\Lenovo\\ImController\\Temp";

		// Token: 0x0400001A RID: 26
		public static readonly string SharedFolderTempLocation = "%PROGRAMDATA%\\Lenovo\\ImController\\shared";

		// Token: 0x0400001B RID: 27
		internal static readonly string RootFolder = "%PROGRAMDATA%";

		// Token: 0x0400001C RID: 28
		public static readonly string ImControllerCoreDataFolder = "%PROGRAMDATA%\\Lenovo\\ImController";

		// Token: 0x0400001D RID: 29
		public static readonly string SubscriptionFileName = "ImControllerSubscription.xml";

		// Token: 0x0400001E RID: 30
		public static readonly string SystemPluginDataFolderLocation = "%PROGRAMDATA%\\Lenovo\\ImController\\SystemPluginData";

		// Token: 0x0400001F RID: 31
		public static readonly string SharedPluginDataFolderLocation = "%PROGRAMDATA%\\Lenovo\\ImController\\SharedPluginData";

		// Token: 0x04000020 RID: 32
		public static readonly string PluginsFolderLocation = "%PROGRAMDATA%\\Lenovo\\ImController\\Plugins";

		// Token: 0x04000021 RID: 33
		public static readonly string PendingPackagesTempLocation = "%PROGRAMDATA%\\Lenovo\\iMController\\Temp\\PendingPackages";

		// Token: 0x04000022 RID: 34
		public static readonly string EventLogon = "EvUserLogon";

		// Token: 0x04000023 RID: 35
		public static readonly string EventLogoff = "EvUserLogoff";

		// Token: 0x04000024 RID: 36
		public static readonly string EventSessionLock = "EvSessionLock";

		// Token: 0x04000025 RID: 37
		public static readonly string EventSessionUnlock = "EvSessionUnlock";

		// Token: 0x04000026 RID: 38
		public static readonly string EventSystemStart = "EvSystemStart";

		// Token: 0x04000027 RID: 39
		public static readonly string EventSystemShutdown = "EvSystemShutdown";

		// Token: 0x04000028 RID: 40
		public static readonly string EventImDriverRemoval = "EvImDriverRemoval";

		// Token: 0x04000029 RID: 41
		public static readonly string ProtocolRegistrationKey = "shell\\open\\command";

		// Token: 0x0400002A RID: 42
		public static readonly string protocolEventRegValueName = "Request";

		// Token: 0x0400002B RID: 43
		public static readonly string ImcPolicyRegKeyPath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Lenovo\\ImController\\";

		// Token: 0x0400002C RID: 44
		public static readonly string ImcPluginPolicyRegKeyPath = Constants.ImcPolicyRegKeyPath + "Plugins\\";

		// Token: 0x0400002D RID: 45
		public static readonly string IMCRegistryKeyPathForUAPApps = "SOFTWARE\\WOW6432Node\\Lenovo\\ImController\\AppId";

		// Token: 0x0400002E RID: 46
		public static readonly string AppReadinessKeyPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer";

		// Token: 0x0400002F RID: 47
		public static readonly string AppReadinessImcPath = "Software\\Lenovo\\ImController\\AppReadiness";

		// Token: 0x04000030 RID: 48
		public static readonly int UAPInstallMonitorTimeIntervalInSecs = 30;

		// Token: 0x04000031 RID: 49
		public static readonly int AllUsersSessionId = -1;

		// Token: 0x04000032 RID: 50
		public static readonly string X64folder = "x64";

		// Token: 0x04000033 RID: 51
		public static readonly string X86folder = "x86";

		// Token: 0x04000034 RID: 52
		internal static readonly int AttemptCount = 2;

		// Token: 0x04000035 RID: 53
		public const string AppConfigKeyVersionOverride = "VersionOverride";

		// Token: 0x04000036 RID: 54
		public const string AppConfigKeyDisableSecurity = "DisableSecurity";

		// Token: 0x04000037 RID: 55
		public const string AppConfigKeyDisableUpdates = "DisableUpdates";

		// Token: 0x04000038 RID: 56
		internal static readonly string InstallationCmdFileName = "Install.CMD";

		// Token: 0x04000039 RID: 57
		internal static readonly string UninstallCmdFileName = "Uninstall.CMD";

		// Token: 0x0400003A RID: 58
		internal static readonly string InstallationPsFileName = "Install.PS1";

		// Token: 0x0400003B RID: 59
		internal static readonly string UninstallPsFileName = "UnInstall.PS1";

		// Token: 0x0400003C RID: 60
		internal static readonly string PowerShellScriptCmdLineWithSecurityEnabled = "-ExecutionPolicy RemoteSigned -NoProfile -NonInteractive -WindowStyle Hidden -File ";

		// Token: 0x0400003D RID: 61
		internal static readonly string PowerShellScriptCmdLineWithSecurityDisabled = "-ExecutionPolicy bypass -NoProfile -NonInteractive -WindowStyle Hidden -File ";

		// Token: 0x0400003E RID: 62
		internal static readonly string PowerShellPartialPath = "\\WindowsPowerShell\\v1.0\\powershell.exe";

		// Token: 0x0400003F RID: 63
		private static string _cachedImControllerVersion;

		// Token: 0x04000040 RID: 64
		private static string _cachedDriverVersion;

		// Token: 0x02000045 RID: 69
		public static class PluginSettings
		{
			// Token: 0x04000104 RID: 260
			public const string RegistryKeyPathToPluginData = "Software\\Lenovo\\ImController\\PluginData";

			// Token: 0x02000090 RID: 144
			public static class Names
			{
				// Token: 0x0400025E RID: 606
				public const string ReceiveEvents = "ImController.Privilege.ReceiveEvents";

				// Token: 0x0400025F RID: 607
				public const string CustomInstall = "ImController.Privilege.CustomInstall";

				// Token: 0x04000260 RID: 608
				public const string UpdateRequiresReboot = "ImController.Package.UpdateRequiresReboot";

				// Token: 0x04000261 RID: 609
				public const string InstallRequiresReboot = "ImController.Package.InstallRequiresReboot";

				// Token: 0x04000262 RID: 610
				public const string SkipSuspendTermination = "ImController.Privilege.SkipSuspendTermination";

				// Token: 0x04000263 RID: 611
				public const string ImcEventHandlerProtocol = "ImController.Privilege.Protocol";

				// Token: 0x04000264 RID: 612
				public const string UapAssociationId = "ImController.Association.Uap";

				// Token: 0x04000265 RID: 613
				public const string DeviceAssociation = "ImController.Association.Device";

				// Token: 0x04000266 RID: 614
				public const string DisableUpdate = "ImController.Package.DisableUpdate";

				// Token: 0x04000267 RID: 615
				public const string ForcedInstallation = "ImController.Package.ForcedInstallation";

				// Token: 0x04000268 RID: 616
				public const string ReplacementName = "ImController.Package.Replacement.Name";

				// Token: 0x04000269 RID: 617
				public const string ReplacementMinVersion = "ImController.Package.Replacement.MinimumVersion";

				// Token: 0x0400026A RID: 618
				public const string DisabledVersion = "ImController.Package.DisabledVersion";
			}
		}
	}
}
