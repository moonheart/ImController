using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management;
using Lenovo.Modern.CoreTypes.Contracts.Capabilities.CapabilityResponse;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation.MemoryInformation;
using Lenovo.Modern.Plugins.Generic.MemoryPlugin;
using Lenovo.Modern.Utilities.Services;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.MemoryInformation
{
	// Token: 0x02000008 RID: 8
	public class MemoryInformationAgent
	{
		// Token: 0x0600004D RID: 77 RVA: 0x000053E9 File Offset: 0x000035E9
		public MemoryInformationAgent()
		{
			this._memoryInfo = this.MemoryInfo();
			this._memoryArrayInfo = this.MemoryArrayInfo();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000540C File Offset: 0x0000360C
		public MemoryInformationResponse GetMemoryInformation()
		{
			MemoryInformationResponse memoryInformationResponse = new MemoryInformationResponse();
			memoryInformationResponse.SystemMemoryInformation = new SystemMemoryInformation();
			memoryInformationResponse.SystemMemoryInformation.NumberOfSlots = this.GetSlotNum();
			memoryInformationResponse.SystemMemoryInformation.MaxCapacity = this.GetMaxMem();
			memoryInformationResponse.SystemMemoryInformation.InstalledMemory = this.GetInstalledMem();
			memoryInformationResponse.MemorySlotList = new MemorySlot[this._memoryInfo.Count];
			uint num = 0U;
			while ((ulong)num < (ulong)((long)this._memoryInfo.Count))
			{
				memoryInformationResponse.MemorySlotList[(int)num] = new MemorySlot();
				memoryInformationResponse.MemorySlotList[(int)num].SlotIndex = (new MemorySlot().SlotIndex = num.ToString());
				memoryInformationResponse.MemorySlotList[(int)num].BankLabel = (new MemorySlot().BankLabel = this.GetBankLabel(num));
				memoryInformationResponse.MemorySlotList[(int)num].InstalledMemory = (new MemorySlot().InstalledMemory = this.GetSlotInstalledMemory(num));
				memoryInformationResponse.MemorySlotList[(int)num].DeviceLocator = (new MemorySlot().DeviceLocator = this.GetDeviceLocator(num));
				memoryInformationResponse.MemorySlotList[(int)num].Manufacturer = (new MemorySlot().Manufacturer = this.GetManufacturer(num));
				memoryInformationResponse.MemorySlotList[(int)num].MaxCapacity = (new MemorySlot().MaxCapacity = this.GetMaxSlotMem());
				memoryInformationResponse.MemorySlotList[(int)num].MemoryType = (new MemorySlot().MemoryType = this.GetMemType(num));
				memoryInformationResponse.MemorySlotList[(int)num].Onboard = (new MemorySlot().Onboard = this.GetOnboard(num));
				memoryInformationResponse.MemorySlotList[(int)num].ReturnCode = (new MemorySlot().ReturnCode = "0");
				memoryInformationResponse.MemorySlotList[(int)num].SerialNumber = (new MemorySlot().SerialNumber = this.GetSerialNumber(num));
				memoryInformationResponse.MemorySlotList[(int)num].Speed = (new MemorySlot().Speed = this.GetMemorySpeed(num));
				num += 1U;
			}
			return memoryInformationResponse;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00005608 File Offset: 0x00003808
		private string GetSlotNum()
		{
			int num = 0;
			int num2 = 0;
			string empty = string.Empty;
			try
			{
				using (List<Dictionary<string, string>>.Enumerator enumerator = this._memoryArrayInfo.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.TryGetValue("MemoryDevices", out empty) && int.TryParse(empty, out num2))
						{
							num += num2;
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return num.ToString();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00005694 File Offset: 0x00003894
		private string GetMaxMem()
		{
			long num = 0L;
			long num2 = 0L;
			string empty = string.Empty;
			try
			{
				foreach (Dictionary<string, string> dictionary in this._memoryArrayInfo)
				{
					if ((dictionary.TryGetValue("MaxCapacity", out empty) || dictionary.TryGetValue("MaxCapacityEx", out empty)) && long.TryParse(empty, out num2))
					{
						num += num2;
					}
				}
				num *= 1024L;
			}
			catch (Exception)
			{
			}
			return num.ToString();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000573C File Offset: 0x0000393C
		private string GetInstalledMem()
		{
			long num = 0L;
			long num2 = 0L;
			string empty = string.Empty;
			try
			{
				using (List<Dictionary<string, string>>.Enumerator enumerator = this._memoryInfo.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.TryGetValue("Capacity", out empty) && long.TryParse(empty, out num2))
						{
							num += num2;
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return num.ToString();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000057C8 File Offset: 0x000039C8
		private string GetBankLabel(uint i)
		{
			string empty = string.Empty;
			try
			{
				if ((long)this._memoryInfo.Count > (long)((ulong)i))
				{
					this._memoryInfo[(int)i].TryGetValue("BankLabel", out empty);
				}
			}
			catch (Exception)
			{
			}
			return empty;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000581C File Offset: 0x00003A1C
		private string GetSlotInstalledMemory(uint i)
		{
			long num = 0L;
			long num2 = 0L;
			string empty = string.Empty;
			try
			{
				if ((long)this._memoryInfo.Count > (long)((ulong)i) && this._memoryInfo[(int)i].TryGetValue("Capacity", out empty) && long.TryParse(empty, out num2))
				{
					num += num2;
				}
			}
			catch (Exception)
			{
			}
			return num.ToString();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000588C File Offset: 0x00003A8C
		private string GetMaxSlotMem()
		{
			long num = 0L;
			long num2 = 0L;
			long num3 = 0L;
			string s = string.Empty;
			try
			{
				if (string.Empty != (s = this.GetMaxMem()) && long.TryParse(s, out num2) && string.Empty != (s = this.GetSlotNum()) && long.TryParse(s, out num3))
				{
					num = num2 / num3;
				}
			}
			catch (Exception)
			{
			}
			return num.ToString();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00005904 File Offset: 0x00003B04
		private string GetDeviceLocator(uint i)
		{
			string empty = string.Empty;
			try
			{
				if ((long)this._memoryInfo.Count > (long)((ulong)i))
				{
					this._memoryInfo[(int)i].TryGetValue("DeviceLocator", out empty);
				}
			}
			catch (Exception)
			{
			}
			return empty;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00005958 File Offset: 0x00003B58
		private string GetManufacturer(uint i)
		{
			string empty = string.Empty;
			try
			{
				if ((long)this._memoryInfo.Count > (long)((ulong)i))
				{
					this._memoryInfo[(int)i].TryGetValue("Manufacturer", out empty);
				}
			}
			catch (Exception)
			{
			}
			return empty;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000059AC File Offset: 0x00003BAC
		private string GetMemType(uint i)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			try
			{
				if ((long)this._memoryInfo.Count > (long)((ulong)i) && this._memoryInfo[(int)i].TryGetValue("MemoryType", out empty2))
				{
					Constants.MemTypeDictionary.TryGetValue(empty2, out empty);
				}
			}
			catch (Exception)
			{
			}
			return empty;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00005A14 File Offset: 0x00003C14
		private string GetOnboard(uint i)
		{
			string result = "true";
			string empty = string.Empty;
			ulong num = 0UL;
			try
			{
				if ((long)this._memoryInfo.Count > (long)((ulong)i) && this._memoryInfo[(int)i].TryGetValue("SerialNumber", out empty) && ulong.TryParse(empty, NumberStyles.HexNumber, null, out num) && num != 0UL)
				{
					result = "false";
				}
			}
			catch (OverflowException)
			{
				result = "false";
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00005AA0 File Offset: 0x00003CA0
		private string GetSerialNumber(uint i)
		{
			string empty = string.Empty;
			try
			{
				if ((long)this._memoryInfo.Count > (long)((ulong)i))
				{
					this._memoryInfo[(int)i].TryGetValue("SerialNumber", out empty);
				}
			}
			catch (Exception)
			{
			}
			return empty;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00005AF4 File Offset: 0x00003CF4
		private string GetMemorySpeed(uint i)
		{
			string empty = string.Empty;
			try
			{
				if ((long)this._memoryInfo.Count > (long)((ulong)i))
				{
					this._memoryInfo[(int)i].TryGetValue("Speed", out empty);
				}
			}
			catch (Exception)
			{
			}
			return empty;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00005B48 File Offset: 0x00003D48
		public CapabilityResponse GetCapability()
		{
			CapabilityResponse capabilityResponse = new CapabilityResponse();
			try
			{
				capabilityResponse.CapabilityList = new Capability[2];
				uint num = 0U;
				while ((ulong)num < (ulong)((long)capabilityResponse.CapabilityList.Length))
				{
					capabilityResponse.CapabilityList[(int)num] = new Capability();
					if (num == 0U)
					{
						capabilityResponse.CapabilityList[(int)num].Key = (new Capability().Key = "MemoryInformationFeature");
						capabilityResponse.CapabilityList[(int)num].Keyvalue = (new Capability().Keyvalue = "true");
					}
					if (1U == num)
					{
						capabilityResponse.CapabilityList[(int)num].Key = (new Capability().Key = "MemorySlotsReplaceableInformation");
						capabilityResponse.CapabilityList[(int)num].Keyvalue = (new Capability().Keyvalue = "true");
					}
					num += 1U;
				}
			}
			catch (Exception)
			{
			}
			return capabilityResponse;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00005C28 File Offset: 0x00003E28
		private List<Dictionary<string, string>> MemoryInfo()
		{
			List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
			try
			{
				using (ManagementClass managementClass = new ManagementClass("Win32_PhysicalMemory"))
				{
					PropertyDataCollection properties = managementClass.Properties;
					foreach (ManagementBaseObject managementBaseObject in managementClass.GetInstances())
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						list.Add(new Dictionary<string, string>());
						foreach (PropertyData propertyData in properties)
						{
							if (managementObject.Properties[propertyData.Name.ToString()].Value != null)
							{
								list[list.Count - 1].Add(propertyData.Name, managementObject.Properties[propertyData.Name.ToString()].Value.ToString());
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return list;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00005D64 File Offset: 0x00003F64
		private List<Dictionary<string, string>> MemoryArrayInfo()
		{
			List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
			try
			{
				using (ManagementClass managementClass = new ManagementClass("Win32_PhysicalMemoryArray"))
				{
					PropertyDataCollection properties = managementClass.Properties;
					foreach (ManagementBaseObject managementBaseObject in managementClass.GetInstances())
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						list.Add(new Dictionary<string, string>());
						foreach (PropertyData propertyData in properties)
						{
							if (managementObject.Properties[propertyData.Name.ToString()].Value != null)
							{
								list[list.Count - 1].Add(propertyData.Name, managementObject.Properties[propertyData.Name.ToString()].Value.ToString());
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return list;
		}

		// Token: 0x0400001A RID: 26
		private List<Dictionary<string, string>> _memoryInfo;

		// Token: 0x0400001B RID: 27
		private List<Dictionary<string, string>> _memoryArrayInfo;

		// Token: 0x0400001C RID: 28
		private ProcessPrivilegeDetector _processPrivilegeDetector;

		// Token: 0x0400001D RID: 29
		private string _process;
	}
}
