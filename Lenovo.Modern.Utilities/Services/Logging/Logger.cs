using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace Lenovo.Modern.Utilities.Services.Logging
{
	// Token: 0x02000036 RID: 54
	public static class Logger
	{
		// Token: 0x06000152 RID: 338 RVA: 0x00006DE8 File Offset: 0x00004FE8
		[Obsolete("This deprecated, please use Setup(Config) instead.", false)]
		public static void Setup(string baseFileName)
		{
			Logger.Setup(new Logger.Configuration
			{
				LogIdentifier = baseFileName,
				FileSizeRollOverKb = 3072
			});
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006E06 File Offset: 0x00005006
		[Obsolete("This deprecated, please use Setup(Config) instead.", false)]
		public static void Setup(Logger.LogSeverity minimumThreshold, string baseFileName)
		{
			Logger.Setup(baseFileName);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00006E10 File Offset: 0x00005010
		public static void Setup(Logger.Configuration configuration)
		{
			if (Logger._isSetup)
			{
				return;
			}
			if (configuration == null)
			{
				throw new ArgumentNullException("Configuration required");
			}
			if (string.IsNullOrWhiteSpace(configuration.LogIdentifier))
			{
				throw new ArgumentNullException("Must provide a base file name");
			}
			Logger._isSetup = true;
			Logger.LogSeverity? minimumThreshold = Logger.TryGetRegistryPreference(configuration.LogIdentifier);
			if (configuration.IsEnabled != null)
			{
				Logger.IsLoggingEnabled = configuration.IsEnabled.Value;
			}
			Logger._logIdentifier = configuration.LogIdentifier;
			Logger._minimumThreshold = minimumThreshold;
			Logger._baseFileName = configuration.LogIdentifier + (string.IsNullOrWhiteSpace(configuration.FileNameEnding) ? "" : ("." + configuration.FileNameEnding));
			Logger._rolloverKb = configuration.FileSizeRollOverKb;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00006ECE File Offset: 0x000050CE
		public static void RefreshLogSeverity()
		{
			Logger._minimumThreshold = Logger.TryGetRegistryPreference(Logger._logIdentifier);
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00006EDF File Offset: 0x000050DF
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00006EE6 File Offset: 0x000050E6
		public static bool IsLoggingEnabled { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00006EF0 File Offset: 0x000050F0
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00006F54 File Offset: 0x00005154
		public static DirectoryInfo LogDirectory
		{
			get
			{
				if (Logger._logDirectory == null)
				{
					PermissionSet permissionSet = new PermissionSet(PermissionState.None);
					FileIOPermission perm = new FileIOPermission(FileIOPermissionAccess.Write, Logger.AdminContextDefaultLogPath);
					permissionSet.AddPermission(perm);
					if (permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
					{
						Logger._logDirectory = new DirectoryInfo(Logger.AdminContextDefaultLogPath);
					}
					else
					{
						Logger._logDirectory = new DirectoryInfo(Logger.UserContextLogPath);
					}
				}
				return Logger._logDirectory;
			}
			set
			{
				Logger._logDirectory = value;
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00006F5C File Offset: 0x0000515C
		public static void Log(Logger.LogSeverity severity, string message)
		{
			try
			{
				if (Logger.IsLoggingEnabledForSeverity(severity))
				{
					Logger.Instance.Log(severity, message);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00006F94 File Offset: 0x00005194
		public static void Log(Exception ex, string message)
		{
			try
			{
				if (Logger.IsLoggingEnabled && Logger._minimumThreshold != null && Logger.LogDirectory != null)
				{
					Logger.Instance.Log(ex, message + "\r\n" + ex.Message);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00006FEC File Offset: 0x000051EC
		public static void Log(Exception ex, string format, params object[] args)
		{
			try
			{
				if (Logger.IsLoggingEnabled && Logger._minimumThreshold != null && Logger.LogDirectory != null)
				{
					Logger.Instance.Log(ex, string.Format(format, args));
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000703C File Offset: 0x0000523C
		public static void Log(Logger.LogSeverity severity, string format, params object[] args)
		{
			try
			{
				if (Logger.IsLoggingEnabled && Logger._minimumThreshold != null)
				{
					Logger.LogSeverity? minimumThreshold = Logger._minimumThreshold;
					if (((severity >= minimumThreshold.GetValueOrDefault()) & (minimumThreshold != null)) && Logger.LogDirectory != null)
					{
						Logger.Instance.Log(severity, string.Format(format, args));
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000070A8 File Offset: 0x000052A8
		public static bool IsLoggingEnabledForSeverity(Logger.LogSeverity severity)
		{
			if (Logger.IsLoggingEnabled && Logger._minimumThreshold != null)
			{
				Logger.LogSeverity? minimumThreshold = Logger._minimumThreshold;
				if ((severity >= minimumThreshold.GetValueOrDefault()) & (minimumThreshold != null))
				{
					return Logger.LogDirectory != null;
				}
			}
			return false;
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600015F RID: 351 RVA: 0x000070EF File Offset: 0x000052EF
		private static ILogger Instance
		{
			get
			{
				ILogger result;
				if ((result = Logger._instance) == null)
				{
					result = (Logger._instance = new TextFileLogger(Logger.LogDirectory.FullName, Logger._baseFileName, Logger._rolloverKb));
				}
				return result;
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000711C File Offset: 0x0000531C
		private static Logger.LogSeverity? TryGetRegistryPreference(string baseFileName)
		{
			Logger.LogSeverity? result = null;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Lenovo\\Modern\\Logs"))
				{
					if (registryKey != null)
					{
						object value = registryKey.GetValue(baseFileName);
						if (value != null)
						{
							string text = value as string;
							if (!string.IsNullOrWhiteSpace(text))
							{
								int value2 = Convert.ToInt32(text.Trim());
								result = new Logger.LogSeverity?((Logger.LogSeverity)value2);
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x04000064 RID: 100
		private static bool _isSetup;

		// Token: 0x04000065 RID: 101
		private static int _rolloverKb;

		// Token: 0x04000066 RID: 102
		private static Logger.LogSeverity? _minimumThreshold;

		// Token: 0x04000067 RID: 103
		private static string _baseFileName;

		// Token: 0x04000068 RID: 104
		private static string _logIdentifier;

		// Token: 0x0400006A RID: 106
		private static DirectoryInfo _logDirectory;

		// Token: 0x0400006B RID: 107
		private static ILogger _instance;

		// Token: 0x0400006C RID: 108
		private static readonly string AdminContextDefaultLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Lenovo\\Modern\\Logs");

		// Token: 0x0400006D RID: 109
		private static readonly string UserContextLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Lenovo\\Modern\\Logs");

		// Token: 0x0400006E RID: 110
		private const string RelativePathToLogsFolder = "Lenovo\\Modern\\Logs";

		// Token: 0x0400006F RID: 111
		private const string RelativePathToLogsRegistryKey = "Software\\Lenovo\\Modern\\Logs";

		// Token: 0x02000090 RID: 144
		public class Configuration
		{
			// Token: 0x1700003D RID: 61
			// (get) Token: 0x06000208 RID: 520 RVA: 0x00009F15 File Offset: 0x00008115
			// (set) Token: 0x06000209 RID: 521 RVA: 0x00009F1D File Offset: 0x0000811D
			public int FileSizeRollOverKb { get; set; }

			// Token: 0x1700003E RID: 62
			// (get) Token: 0x0600020A RID: 522 RVA: 0x00009F26 File Offset: 0x00008126
			// (set) Token: 0x0600020B RID: 523 RVA: 0x00009F2E File Offset: 0x0000812E
			public string LogIdentifier { get; set; }

			// Token: 0x1700003F RID: 63
			// (get) Token: 0x0600020C RID: 524 RVA: 0x00009F37 File Offset: 0x00008137
			// (set) Token: 0x0600020D RID: 525 RVA: 0x00009F3F File Offset: 0x0000813F
			public string FileNameEnding { get; set; }

			// Token: 0x17000040 RID: 64
			// (get) Token: 0x0600020E RID: 526 RVA: 0x00009F48 File Offset: 0x00008148
			// (set) Token: 0x0600020F RID: 527 RVA: 0x00009F50 File Offset: 0x00008150
			public bool? IsEnabled { get; set; }
		}

		// Token: 0x02000091 RID: 145
		public enum LogSeverity
		{
			// Token: 0x04000238 RID: 568
			Information,
			// Token: 0x04000239 RID: 569
			Warning,
			// Token: 0x0400023A RID: 570
			Error,
			// Token: 0x0400023B RID: 571
			Critical
		}
	}
}
