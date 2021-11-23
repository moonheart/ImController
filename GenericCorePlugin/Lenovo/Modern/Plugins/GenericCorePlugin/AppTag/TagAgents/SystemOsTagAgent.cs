using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.Tags;
using Lenovo.Modern.Utilities.Services.Logging;
using Microsoft.Win32;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents
{
	// Token: 0x02000024 RID: 36
	internal class SystemOsTagAgent : ITagAgent
	{
		// Token: 0x06000109 RID: 265 RVA: 0x00008838 File Offset: 0x00006A38
		public Task<IEnumerable<Tag>> CollectTagsAsync()
		{
			List<Tag> list = new List<Tag>();
			Tag preloadOsNameTag = this.GetPreloadOsNameTag();
			Tag preloadDateTag = this.GetPreloadDateTag();
			Tag firstRunDateTag = this.GetFirstRunDateTag(preloadDateTag);
			Tag daysFromOobeTag = this.GetDaysFromOobeTag(firstRunDateTag);
			Tag oobeWeekTag = this.GetOobeWeekTag(firstRunDateTag);
			if (preloadDateTag != null)
			{
				list.Add(preloadDateTag);
			}
			if (preloadOsNameTag != null)
			{
				list.Add(preloadOsNameTag);
			}
			if (firstRunDateTag != null)
			{
				list.Add(firstRunDateTag);
			}
			if (daysFromOobeTag != null)
			{
				list.Add(daysFromOobeTag);
			}
			if (oobeWeekTag != null)
			{
				list.Add(oobeWeekTag);
			}
			return Task.FromResult<IEnumerable<Tag>>(list);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000088AF File Offset: 0x00006AAF
		private static string GetModulesLogFilePath()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "modules.log");
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006254 File Offset: 0x00004454
		private static string GetSysPrepPath()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "Sysprep");
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000088C4 File Offset: 0x00006AC4
		private static string GetDefaultUserDocumentsPath()
		{
			string result = string.Empty;
			try
			{
				RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\ProfileList");
				string text = string.Empty;
				if (registryKey != null)
				{
					text = registryKey.GetValue("Default").ToString();
				}
				if (Environment.Is64BitOperatingSystem)
				{
					result = Path.Combine(text, "Documents");
				}
				else
				{
					result = text;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown getting the Default User's Documents path");
			}
			return result;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00008944 File Offset: 0x00006B44
		private Tag GetFirstRunDateTag(Tag preload)
		{
			Tag result = null;
			try
			{
				string text = string.Empty;
				if (SystemOsTagAgent.IsWindowsInAuditBoot())
				{
					return new Tag("System.Os.FirstRunDateTime", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
				}
				text = SystemOsTagAgent.GetDefaultUserDocumentsPath();
				if (!Directory.Exists(text))
				{
					Logger.Log(Logger.LogSeverity.Warning, "Directory {0} does not exist", new object[] { text });
					return null;
				}
				DateTime lastWriteTime = Directory.GetLastWriteTime(text);
				if (lastWriteTime != DateTime.MinValue)
				{
					return new Tag("System.Os.FirstRunDateTime", lastWriteTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception while getting FirstRunDate tag");
			}
			return result;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00008A00 File Offset: 0x00006C00
		private Tag GetPreloadDateTag()
		{
			Tag result = null;
			try
			{
				string modulesLogFilePath = SystemOsTagAgent.GetModulesLogFilePath();
				if (!File.Exists(modulesLogFilePath))
				{
					Logger.Log(Logger.LogSeverity.Warning, "File {0} does not exist", new object[] { modulesLogFilePath });
					return null;
				}
				DateTime creationTime = File.GetCreationTime(modulesLogFilePath);
				if (creationTime != DateTime.MinValue)
				{
					return new Tag("System.Os.Preload.DateTime", creationTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception while getting Preloaded Date tag");
			}
			return result;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00008A8C File Offset: 0x00006C8C
		private Tag GetPreloadOsNameTag()
		{
			Tag result = null;
			try
			{
				string modulesLogFilePath = SystemOsTagAgent.GetModulesLogFilePath();
				if (!File.Exists(modulesLogFilePath))
				{
					Logger.Log(Logger.LogSeverity.Warning, "File {0} does not exist", new object[] { modulesLogFilePath });
					return null;
				}
				List<string> arg = File.ReadLines(modulesLogFilePath).Take(10).ToList<string>();
				Func<IEnumerable<string>, string, bool> func = (IEnumerable<string> lines, string keyword) => lines.Any((string line) => !string.IsNullOrEmpty(line) && line.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
				if (func(arg, "Win7") || func(arg, "Win 7"))
				{
					result = new Tag("System.Os.Preload.OsName", "Win7");
				}
				else if (func(arg, "Win8") || func(arg, "Win 8"))
				{
					result = new Tag("System.Os.Preload.OsName", "Win8");
				}
				else if (func(arg, "Win8.1") || func(arg, "Win 8.1"))
				{
					result = new Tag("System.Os.Preload.OsName", "Win81");
				}
				else if (func(arg, "Win10") || func(arg, "Windows 10"))
				{
					result = new Tag("System.Os.Preload.OsName", "Win10");
				}
				else
				{
					Logger.Log(Logger.LogSeverity.Warning, "The file {0} doesn't have any indication of OS name", new object[] { modulesLogFilePath });
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception while getting Preloaded OS tag");
			}
			return result;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00008BF8 File Offset: 0x00006DF8
		private Tag GetDaysFromOobeTag(Tag oobe)
		{
			Tag result = null;
			try
			{
				if (oobe != null && !string.IsNullOrEmpty(oobe.Value))
				{
					int days = (DateTime.Now.Date - DateTime.Parse(oobe.Value).Date).Days;
					return new Tag("System.Os.DaysFromOobe", ((days < 0) ? 0 : days).ToString());
				}
				Logger.Log(Logger.LogSeverity.Error, "Exception while getting DaysFromOobe tag due to failure in getting first run date.");
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception while getting DaysFromOobe tag");
			}
			return result;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00008C90 File Offset: 0x00006E90
		private Tag GetOobeWeekTag(Tag oobe)
		{
			Tag result = null;
			try
			{
				if (oobe != null && (DateTime.Now.Date - DateTime.Parse(oobe.Value).Date).Days < 8)
				{
					return new Tag("System.Os.OobeWeek", "true");
				}
				return new Tag("System.Os.OobeWeek", "false");
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception while getting OobeWeek tag");
			}
			return result;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00008D18 File Offset: 0x00006F18
		private static bool IsWindowsInAuditBoot()
		{
			int num = 0;
			try
			{
				RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SYSTEM\\\\Setup\\\\Status");
				int num2 = 0;
				if (registryKey != null)
				{
					num2 = (int)registryKey.GetValue("AuditBoot", 0);
				}
				Logger.Log(Logger.LogSeverity.Information, "AuditBoot flag value in registry: (in IsWindowsInAuditBoot): {0}", new object[] { num2 });
				num = num2;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception reading audit boot flag");
			}
			Logger.Log(Logger.LogSeverity.Information, "AuditBoot flag (in IsWindowsInAuditBoot): {0}", new object[] { num });
			return num != 0;
		}
	}
}
