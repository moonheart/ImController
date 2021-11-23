using System;
using System.IO;
using Microsoft.Win32;

namespace Lenovo.Tools.Logging
{
	// Token: 0x02000013 RID: 19
	internal static class Logger
	{
		// Token: 0x0600007F RID: 127 RVA: 0x000057AE File Offset: 0x000039AE
		internal static void Setup(string baseFileName)
		{
			if (Logger._baseFileName == null && !string.IsNullOrEmpty(baseFileName))
			{
				Logger._baseFileName = baseFileName;
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000057C5 File Offset: 0x000039C5
		internal static void Setup(Logger.LogSeverity minimumThreshold, string baseFileName)
		{
			Logger.Setup(baseFileName);
			Logger._minimumThreshold = new Logger.LogSeverity?(minimumThreshold);
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000081 RID: 129 RVA: 0x000057D8 File Offset: 0x000039D8
		// (set) Token: 0x06000082 RID: 130 RVA: 0x000057DF File Offset: 0x000039DF
		internal static bool IsLoggingEnabled { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000083 RID: 131 RVA: 0x000057E7 File Offset: 0x000039E7
		// (set) Token: 0x06000084 RID: 132 RVA: 0x000057EE File Offset: 0x000039EE
		internal static string LogDirectory
		{
			get
			{
				return Logger._logDirectory;
			}
			set
			{
				Logger._logDirectory = value;
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000057F8 File Offset: 0x000039F8
		internal static void Log(Logger.LogSeverity severity, string message)
		{
			if (Logger.IsLoggingEnabled && Logger._minimumThreshold != null)
			{
				Logger.LogSeverity? minimumThreshold = Logger._minimumThreshold;
				if (((severity >= minimumThreshold.GetValueOrDefault()) & (minimumThreshold != null)) && Logger.LogDirectory != null)
				{
					Logger.Instance.Log(severity, message);
				}
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00005848 File Offset: 0x00003A48
		public static void Log(Exception ex, string message)
		{
			if (Logger.IsLoggingEnabled && Logger._minimumThreshold != null && Logger.LogDirectory != null)
			{
				Logger.Instance.Log(ex, message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005880 File Offset: 0x00003A80
		public static void Log(Exception ex, string format, params object[] args)
		{
			if (Logger.IsLoggingEnabled && Logger._minimumThreshold != null && Logger.LogDirectory != null)
			{
				Logger.Instance.Log(ex, string.Format(format, args));
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000058B0 File Offset: 0x00003AB0
		public static void Log(Logger.LogSeverity severity, string format, params object[] args)
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

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00005906 File Offset: 0x00003B06
		private static ILogger Instance
		{
			get
			{
				ILogger result;
				if ((result = Logger._instance) == null)
				{
					result = (Logger._instance = new TextFileLogger(Logger.LogDirectory, Logger._baseFileName));
				}
				return result;
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00005928 File Offset: 0x00003B28
		private static Logger.LogSeverity? TryGetRegistryPreference(string baseFileName)
		{
			Logger.LogSeverity? result = null;
			RegistryKey registryKey = null;
			try
			{
				registryKey = Registry.LocalMachine.OpenSubKey("Software\\Lenovo\\Modern\\Logs");
				if (registryKey != null)
				{
					object value = registryKey.GetValue(baseFileName);
					if (value != null)
					{
						string text = value as string;
						if (!string.IsNullOrEmpty(text))
						{
							int value2 = Convert.ToInt32(text.Trim());
							result = new Logger.LogSeverity?((Logger.LogSeverity)value2);
						}
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
			return result;
		}

		// Token: 0x04000060 RID: 96
		private static Logger.LogSeverity? _minimumThreshold;

		// Token: 0x04000061 RID: 97
		private static string _baseFileName;

		// Token: 0x04000063 RID: 99
		private static string _logDirectory;

		// Token: 0x04000064 RID: 100
		private static ILogger _instance;

		// Token: 0x04000065 RID: 101
		private static readonly string AdminContextDefaultLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Lenovo\\Modern\\Logs");

		// Token: 0x04000066 RID: 102
		private static readonly string UserContextLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Lenovo\\Modern\\Logs");

		// Token: 0x04000067 RID: 103
		private const string RelativePathToLogsFolder = "Lenovo\\Modern\\Logs";

		// Token: 0x04000068 RID: 104
		private const string RelativePathToLogsRegistryKey = "Software\\Lenovo\\Modern\\Logs";

		// Token: 0x02000033 RID: 51
		internal enum LogSeverity
		{
			// Token: 0x04000105 RID: 261
			Information,
			// Token: 0x04000106 RID: 262
			Warning,
			// Token: 0x04000107 RID: 263
			Error,
			// Token: 0x04000108 RID: 264
			Critical
		}
	}
}
