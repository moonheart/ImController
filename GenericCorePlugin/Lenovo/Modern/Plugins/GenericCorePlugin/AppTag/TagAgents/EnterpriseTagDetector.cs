using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.Tags;
using Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation;
using Lenovo.Modern.Utilities.Services.Logging;
using Microsoft.Win32;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents
{
	// Token: 0x0200001F RID: 31
	internal class EnterpriseTagDetector : ITagAgent
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x00007DE4 File Offset: 0x00005FE4
		public Task<IEnumerable<Tag>> CollectTagsAsync()
		{
			List<Tag> list = new List<Tag>();
			bool? flag = this.IsEnterpriseDeviceViaImcPolicy();
			bool? flag2 = flag;
			bool flag3 = true;
			if (((flag2.GetValueOrDefault() == flag3) & (flag2 != null)) || (flag == null && (this.IsCloudDomainExist() || this.IsEnterpriseProductName() || this.IsLEDomainRole())))
			{
				list.Add(new Tag
				{
					Key = "System.IsEnterpriseDevice",
					Value = string.Empty
				});
			}
			return Task.FromResult<IEnumerable<Tag>>(list);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00007E60 File Offset: 0x00006060
		private bool? IsEnterpriseDeviceViaImcPolicy()
		{
			bool? result;
			try
			{
				int? num = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Lenovo\\ImController", "IsEnterpriseDevice", null) as int?;
				Logger.Log(Logger.LogSeverity.Information, string.Format("IsEnterpriseDevice value: {0}", num ?? (-1)));
				if (num != null)
				{
					result = new bool?(num.Value == 1);
				}
				else
				{
					result = null;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to check the IsEnterpriseDevice in registry key.");
				result = null;
			}
			return result;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00007F00 File Offset: 0x00006100
		private bool IsEnterpriseProductName()
		{
			try
			{
				string sku = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName", string.Empty) as string;
				if (!string.IsNullOrWhiteSpace(sku) && this.LE_SKU.Any((string s) => sku.Contains(s, StringComparison.InvariantCultureIgnoreCase)))
				{
					Logger.Log(Logger.LogSeverity.Information, "LE system identified: ProductName related to enterprise or education.");
					return true;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to check the product name in registry key.");
				return false;
			}
			return false;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00007F8C File Offset: 0x0000618C
		private bool IsCloudDomainExist()
		{
			try
			{
				bool flag = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\CloudDomainJoin\\JoinInfo") != null;
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\CloudDomainJoin\\TenantInfo");
				if (!flag && registryKey == null)
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to check the cloud domain in registry key.");
				return false;
			}
			Logger.Log(Logger.LogSeverity.Information, "LE system identified: As cloud domain folder exists.");
			return true;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00007FF0 File Offset: 0x000061F0
		private bool IsLEDomainRole()
		{
			string value = null;
			try
			{
				ManagementScope managementScope = new ManagementScope("\\\\.\\root\\cimv2");
				managementScope.Connect();
				ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(managementScope, query))
				{
					using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
					{
						foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
						{
							value = ((ManagementObject)managementBaseObject)["DomainRole"].ToString();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to check the local domain role.");
				return false;
			}
			if (!string.IsNullOrWhiteSpace(value) && Convert.ToUInt16(value) > 0)
			{
				Logger.Log(Logger.LogSeverity.Information, "LE system identified: Domain Role is not Standalone Workstation (0).");
				return true;
			}
			return false;
		}

		// Token: 0x04000058 RID: 88
		private const string ImcontrollerRegistryKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Lenovo\\ImController";

		// Token: 0x04000059 RID: 89
		private const string ImcontrollerRegistryValue = "IsEnterpriseDevice";

		// Token: 0x0400005A RID: 90
		private const string WindowsCurrentVersionRegistryKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion";

		// Token: 0x0400005B RID: 91
		private const string WindowsCurrentVersionRegistryValue = "ProductName";

		// Token: 0x0400005C RID: 92
		private const string CloudDomainJoinJIRegisterKey = "System\\CurrentControlSet\\Control\\CloudDomainJoin\\JoinInfo";

		// Token: 0x0400005D RID: 93
		private const string CloudDomainJoinTIRegisterKey = "System\\CurrentControlSet\\Control\\CloudDomainJoin\\TenantInfo";

		// Token: 0x0400005E RID: 94
		private string[] LE_SKU = new string[] { "enterprise", "education" };
	}
}
