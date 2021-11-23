using System;
using System.Management;
using System.Text.RegularExpressions;
using Lenovo.Modern.Utilities.Services.Logging;
using Microsoft.Win32;

namespace Lenovo.Modern.ImController.Services
{
	// Token: 0x02000012 RID: 18
	public class DeviceUtility
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00003838 File Offset: 0x00001A38
		public static bool CheckIfNeedToDelayAutomaticStartup()
		{
			Logger.Log(Logger.LogSeverity.Information, "CheckIfNeedToDelayAutomaticStartup : Started checking Device processor and harddisk.");
			bool flag = DeviceUtility.CheckForSlowerProcessor();
			if (!flag)
			{
				flag = DeviceUtility.CheckForSlowerHarddisk();
			}
			return flag;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003864 File Offset: 0x00001A64
		private static bool CheckForSlowerHarddisk()
		{
			ManagementScope managementScope = new ManagementScope("\\\\.\\root\\microsoft\\windows\\storage");
			managementScope.Connect();
			bool flag = true;
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM MSFT_PhysicalDisk"))
			{
				managementObjectSearcher.Scope = managementScope;
				string systemDriveIndex = DeviceUtility.GetSystemDriveIndex();
				foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
				{
					ushort num = 0;
					uint maxValue = uint.MaxValue;
					if (!string.IsNullOrEmpty(systemDriveIndex))
					{
						if (managementBaseObject["DeviceID"] != null && !string.IsNullOrEmpty(managementBaseObject["DeviceID"].ToString()) && systemDriveIndex.Equals(managementBaseObject["DeviceID"].ToString(), StringComparison.CurrentCultureIgnoreCase) && managementBaseObject["MediaType"] != null && ushort.TryParse(managementBaseObject["MediaType"].ToString(), out num) && managementBaseObject["SpindleSpeed"] != null && uint.TryParse(managementBaseObject["SpindleSpeed"].ToString(), out maxValue))
						{
							flag = num == 3 && maxValue > 0U;
							Logger.Log(Logger.LogSeverity.Information, "MediaType:{0} SpindleSpeed:{1}", new object[] { num, maxValue });
							break;
						}
					}
					else if (managementBaseObject["MediaType"] != null && ushort.TryParse(managementBaseObject["MediaType"].ToString(), out num) && managementBaseObject["SpindleSpeed"] != null && uint.TryParse(managementBaseObject["SpindleSpeed"].ToString(), out maxValue))
					{
						flag |= num == 3 && maxValue > 0U;
						Logger.Log(Logger.LogSeverity.Information, "MediaType:{0} SpindleSpeed:{1}", new object[] { num, maxValue });
					}
				}
			}
			if (flag)
			{
				Logger.Log(Logger.LogSeverity.Information, "CheckIfNeedToDelayAutomaticStartup : Device Harddisk meets requirement to start IMC service in Delayed Automatic mode.");
			}
			else
			{
				Logger.Log(Logger.LogSeverity.Information, "CheckIfNeedToDelayAutomaticStartup : Device Harddisk meets requirement to start IMC service in Automatic mode.");
			}
			return flag;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003AA0 File Offset: 0x00001CA0
		private static string GetSystemDriveIndex()
		{
			string str = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 2);
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk WHERE DeviceID='" + str + "'"))
			{
				foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
				{
					foreach (ManagementBaseObject managementBaseObject2 in ((ManagementObject)managementBaseObject).GetRelated("Win32_DiskPartition"))
					{
						using (ManagementObjectCollection.ManagementObjectEnumerator enumerator3 = ((ManagementObject)managementBaseObject2).GetRelated("Win32_DiskDrive").GetEnumerator())
						{
							if (enumerator3.MoveNext())
							{
								return ((ManagementObject)enumerator3.Current)["Index"].ToString();
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003BC4 File Offset: 0x00001DC4
		private static bool CheckForSlowerProcessor()
		{
			bool flag = true;
			if (DeviceUtility.CheckVendorIdentifier())
			{
				string processorName = DeviceUtility.GetProcessorName();
				if (!string.IsNullOrEmpty(processorName))
				{
					flag = new Regex("Core[a-zA-Z() ]*i3|Pentium|Celeron|AMD", RegexOptions.IgnoreCase).IsMatch(processorName);
					if (!flag)
					{
						Logger.Log(Logger.LogSeverity.Information, "CheckIfNeedToDelayAutomaticStartup : Device processor name is '{0}' which meets requirement to start IMC service in Automatic mode.", new object[] { processorName });
					}
					else
					{
						Logger.Log(Logger.LogSeverity.Information, "CheckIfNeedToDelayAutomaticStartup : Device processor name is '{0}' which meets requirement to start IMC service in Delayed Automatic mode.", new object[] { processorName });
					}
				}
				else
				{
					Logger.Log(Logger.LogSeverity.Error, "CheckIfNeedToDelayAutomaticStartup : Failed to get processor name {0}", new object[] { processorName });
				}
			}
			else
			{
				Logger.Log(Logger.LogSeverity.Information, "CheckIfNeedToDelayAutomaticStartup : VendorIdentifier for CPU is not {0}", new object[] { "GenuineIntel" });
			}
			return flag;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003C5C File Offset: 0x00001E5C
		private static bool CheckVendorIdentifier()
		{
			bool result = false;
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0"))
			{
				if (registryKey != null)
				{
					string text = Convert.ToString(registryKey.GetValue("VendorIdentifier"));
					result = !string.IsNullOrEmpty(text) && text == "GenuineIntel";
				}
			}
			return result;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003CC4 File Offset: 0x00001EC4
		private static string GetProcessorName()
		{
			string text = null;
			try
			{
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
				{
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						if (managementObject["name"] != null)
						{
							text = managementObject["name"].ToString();
						}
					}
				}
			}
			catch (Exception)
			{
			}
			if (string.IsNullOrEmpty(text))
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0"))
				{
					if (registryKey != null)
					{
						text = Convert.ToString(registryKey.GetValue("ProcessorNameString"));
					}
				}
			}
			return text;
		}

		// Token: 0x0400003B RID: 59
		private const string ProcessorNamePatterns = "Core[a-zA-Z() ]*i3|Pentium|Celeron|AMD";

		// Token: 0x0400003C RID: 60
		private const string RegistryKey_CPUInformation = "HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0";

		// Token: 0x0400003D RID: 61
		private const string RegistrySubKey_ProcessorNameString = "ProcessorNameString";

		// Token: 0x0400003E RID: 62
		private const string RegistrySubKey_VendorIdentifier = "VendorIdentifier";

		// Token: 0x0400003F RID: 63
		private const string RegistrySubKey_VendorIdentifier_Value = "GenuineIntel";
	}
}
