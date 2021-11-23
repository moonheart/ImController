using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.Tags;
using Lenovo.Modern.Utilities.Services.Logging;
using Microsoft.Win32;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents
{
	// Token: 0x02000025 RID: 37
	internal class UDCTagAgent : ITagAgent
	{
		// Token: 0x06000114 RID: 276 RVA: 0x00008DB8 File Offset: 0x00006FB8
		Task<IEnumerable<Tag>> ITagAgent.CollectTagsAsync()
		{
			List<Tag> list = new List<Tag>();
			try
			{
				string udcserviceVersion = this.GetUDCServiceVersion();
				list.Add(new Tag("UdcInstalled", (!string.IsNullOrEmpty(udcserviceVersion)) ? "true" : "false"));
				if (!string.IsNullOrEmpty(udcserviceVersion))
				{
					list.Add(new Tag("UdcVersion", udcserviceVersion));
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown while getting UDC tags");
			}
			return Task.FromResult<IEnumerable<Tag>>(list);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00008E34 File Offset: 0x00007034
		private string GetUDCServiceVersion()
		{
			ServiceController serviceController = ServiceController.GetServices().FirstOrDefault((ServiceController s) => s.ServiceName.ToLower() == "udcservice");
			string text = null;
			if (serviceController != null)
			{
				string text2 = "";
				try
				{
					RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
					if (registryKey != null)
					{
						RegistryKey registryKey2 = registryKey.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\" + serviceController.ServiceName);
						if (registryKey2 != null)
						{
							text2 = registryKey2.GetValue("ImagePath").ToString();
						}
					}
					if (!string.IsNullOrEmpty(text2) && File.Exists(text2))
					{
						text = FileVersionInfo.GetVersionInfo(text2).FileVersion;
					}
				}
				catch
				{
				}
				if (string.IsNullOrEmpty(text))
				{
					text2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers\\Lenovo\\udc\\Service\\UDClientService.exe");
					if (File.Exists(text2))
					{
						text = FileVersionInfo.GetVersionInfo(text2).FileVersion;
					}
				}
			}
			return text;
		}

		// Token: 0x0400006D RID: 109
		private const string UDCSERVICENAME = "udcservice";

		// Token: 0x0400006E RID: 110
		private const string UDCINSTALLED = "UdcInstalled";

		// Token: 0x0400006F RID: 111
		private const string UDCVERSION = "UdcVersion";
	}
}
