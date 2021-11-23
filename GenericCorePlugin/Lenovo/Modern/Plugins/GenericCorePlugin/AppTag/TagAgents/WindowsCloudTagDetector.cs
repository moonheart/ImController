using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.Tags;
using Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation;
using Lenovo.Modern.Utilities.Services.Logging;
using Microsoft.Win32;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents
{
	// Token: 0x02000026 RID: 38
	internal class WindowsCloudTagDetector : ITagAgent
	{
		// Token: 0x06000117 RID: 279 RVA: 0x00008F1C File Offset: 0x0000711C
		public Task<IEnumerable<Tag>> CollectTagsAsync()
		{
			List<Tag> list = new List<Tag>();
			if (this.IsWindowCloudDevice())
			{
				list.Add(new Tag
				{
					Key = "System.IsMsCiRequired",
					Value = string.Empty
				});
				list.Add(new Tag
				{
					Key = "System.SMode",
					Value = string.Empty
				});
			}
			return Task.FromResult<IEnumerable<Tag>>(list);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00008F80 File Offset: 0x00007180
		private bool IsWindowCloudDevice()
		{
			try
			{
				WindowsCloudTagDetector.WLDP_WINDOWS_LOCKDOWN_MODE wldp_WINDOWS_LOCKDOWN_MODE;
				WindowsCloudTagDetector.WldpQueryWindowsLockdownMode(out wldp_WINDOWS_LOCKDOWN_MODE);
				Logger.Log(Logger.LogSeverity.Information, "Value from WldpQueryWindowsLockdownMode is " + wldp_WINDOWS_LOCKDOWN_MODE.ToString());
				if (wldp_WINDOWS_LOCKDOWN_MODE != WindowsCloudTagDetector.WLDP_WINDOWS_LOCKDOWN_MODE.WLDP_WINDOWS_LOCKDOWN_MODE_UNLOCKED)
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to check the IsWindowCloudDevice lockdown setting.");
			}
			try
			{
				RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
				string text = null;
				if (registryKey != null)
				{
					RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion");
					if (registryKey2 != null)
					{
						text = registryKey2.GetValue("EditionID") as string;
					}
				}
				Logger.Log(Logger.LogSeverity.Information, "WindowsCloudTagDetector: 32 bit windows has IsWindowCloudDevice value: " + text);
				if (!string.IsNullOrWhiteSpace(text) && text.Contains("cloud", StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
				RegistryKey registryKey3 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
				if (registryKey3 != null)
				{
					RegistryKey registryKey4 = registryKey3.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion");
					if (registryKey4 != null)
					{
						text = registryKey4.GetValue("EditionID") as string;
					}
				}
				Logger.Log(Logger.LogSeverity.Information, "WindowsCloudTagDetector: 64 windows has IsWindowCloudDevice value: " + text);
				if (!string.IsNullOrWhiteSpace(text) && text.Contains("cloud", StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "Exception thrown trying to check the IsWindowCloudDevice in registry key.");
			}
			return false;
		}

		// Token: 0x06000119 RID: 281
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("Wldp.dll")]
		private static extern WindowsCloudTagDetector.WLDP_WINDOWS_LOCKDOWN_MODE WldpQueryWindowsLockdownMode(out WindowsCloudTagDetector.WLDP_WINDOWS_LOCKDOWN_MODE param);

		// Token: 0x04000070 RID: 112
		private const string WINDOWS_CLOUD_REGISTRY_KEY = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion";

		// Token: 0x04000071 RID: 113
		private const string WINDOWS_CLOUD_REGISTRY_VALUE = "EditionID";

		// Token: 0x04000072 RID: 114
		private const string WONDOWS_CLOUD_VERSION = "cloud";

		// Token: 0x02000081 RID: 129
		private enum WLDP_WINDOWS_LOCKDOWN_MODE
		{
			// Token: 0x040001BA RID: 442
			WLDP_WINDOWS_LOCKDOWN_MODE_UNLOCKED,
			// Token: 0x040001BB RID: 443
			WLDP_WINDOWS_LOCKDOWN_MODE_TRIAL,
			// Token: 0x040001BC RID: 444
			WLDP_WINDOWS_LOCKDOWN_MODE_LOCKED,
			// Token: 0x040001BD RID: 445
			WLDP_WINDOWS_LOCKDOWN_MODE_MAX
		}
	}
}
