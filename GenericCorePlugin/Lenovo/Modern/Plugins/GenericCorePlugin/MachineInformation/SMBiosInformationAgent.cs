using System;
using System.ComponentModel;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation
{
	// Token: 0x0200000B RID: 11
	public class SMBiosInformationAgent
	{
		// Token: 0x0600006D RID: 109 RVA: 0x0000654D File Offset: 0x0000474D
		public SMBiosInformationAgent()
		{
			SMBiosInformationAgent.CheckMachineInformationAvailability();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000655C File Offset: 0x0000475C
		public SMBiosInformation GetSMBiosInformation()
		{
			return new SMBiosInformation
			{
				AssetTagNumber = this.GetAssetTagNumber(),
				BiosVersion = (this.GetBiosVersionFromWmi() ?? this.GetBiosVersion()),
				BiosDate = this.GetBiosDate(),
				ECVersion = this.GetECVersion(),
				EnclosureType = this.GetEnclosureType(),
				Family = this.GetFamily(),
				IdeaSerialNumber = this.GetIdeaSerialNumber(),
				Manufacturer = this.GetManufacturer(),
				MTM = this.GetMTM(),
				ProductName = this.GetProductName(),
				SerialNumber = this.GetSerialNumber(),
				SKU = this.GetSKU(),
				Version = this.GetVersion()
			};
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00006614 File Offset: 0x00004814
		private string GetBiosVersion()
		{
			string result = string.Empty;
			StringBuilder stringBuilder = new StringBuilder(1024);
			try
			{
				Marshal.PtrToStringAnsi(SMBiosInformationAgent.BiosVersion(stringBuilder));
				result = stringBuilder.ToString();
				this.GetLastWin32Error();
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the Bios Version from SMBIOS type 0 table");
			}
			return result;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000666C File Offset: 0x0000486C
		private string GetBiosVersionFromWmi()
		{
			string result = null;
			try
			{
				using (ManagementClass managementClass = new ManagementClass("Win32_Bios"))
				{
					foreach (ManagementBaseObject managementBaseObject in managementClass.GetInstances())
					{
						foreach (PropertyData propertyData in ((ManagementObject)managementBaseObject).Properties)
						{
							try
							{
								string name = propertyData.Name;
								string empty = string.Empty;
								if (propertyData.Name == "SMBIOSBIOSVersion" && propertyData.Value != null)
								{
									result = propertyData.Value.ToString();
									break;
								}
							}
							catch (Exception ex)
							{
								Logger.Log(ex, "Exception thrown trying to get the " + propertyData.Name + " property");
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "Exception thrown trying to get the Bios Version from WMI");
			}
			return result;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000067A0 File Offset: 0x000049A0
		private string GetBiosDate()
		{
			string result = string.Empty;
			try
			{
				Lenovo.Modern.Utilities.Services.Wrappers.Settings.IContainer container = new RegistrySystem().LoadContainer(Constants.WindowsBIOSRegistryKey);
				if (container != null)
				{
					IContainerValue value = container.GetValue(Constants.WindowsBIOSReleaseDateRegistryValue);
					if (value != null)
					{
						result = value.GetValueAsString();
					}
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000067F4 File Offset: 0x000049F4
		private string GetECVersion()
		{
			string result = string.Empty;
			try
			{
				Lenovo.Modern.Utilities.Services.Wrappers.Settings.IContainer container = new RegistrySystem().LoadContainer(Constants.WindowsBIOSRegistryKey);
				if (container != null)
				{
					IContainerValue value = container.GetValue(Constants.WindowsECFirmwareMajorReleaseRegistryValue);
					IContainerValue value2 = container.GetValue(Constants.WindowsECFirmwareMinorReleaseRegistryValue);
					if (value != null && value2 != null)
					{
						result = string.Format("{0}.{1:D2}", value.GetValueAsString(), value2.GetValueAsInt());
					}
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000686C File Offset: 0x00004A6C
		private string GetManufacturer()
		{
			string result = string.Empty;
			StringBuilder stringBuilder = new StringBuilder(1024);
			try
			{
				Marshal.PtrToStringAnsi(SMBiosInformationAgent.Manufacturer(stringBuilder));
				result = stringBuilder.ToString();
				this.GetLastWin32Error();
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the Manufacturer from SMBIOS type 1 table");
			}
			return result;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000068C4 File Offset: 0x00004AC4
		private string GetMTM()
		{
			string result = string.Empty;
			StringBuilder stringBuilder = new StringBuilder(1024);
			try
			{
				Marshal.PtrToStringAnsi(SMBiosInformationAgent.ModelNumber(stringBuilder));
				result = stringBuilder.ToString();
				this.GetLastWin32Error();
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the MTM from SMBIOS type 1 table");
			}
			return result;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000691C File Offset: 0x00004B1C
		private string GetSerialNumber()
		{
			string result = string.Empty;
			StringBuilder stringBuilder = new StringBuilder(1024);
			try
			{
				Marshal.PtrToStringAnsi(SMBiosInformationAgent.SerialNumber(stringBuilder));
				result = stringBuilder.ToString();
				this.GetLastWin32Error();
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the Serial Number from SMBIOS type 1 table");
			}
			return result;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00006974 File Offset: 0x00004B74
		private string GetIdeaSerialNumber()
		{
			string result = string.Empty;
			StringBuilder stringBuilder = new StringBuilder(1024);
			try
			{
				Marshal.PtrToStringAnsi(SMBiosInformationAgent.IdeaSerialNumber(stringBuilder));
				result = stringBuilder.ToString();
				this.GetLastWin32Error();
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the Serial Number from SMBIOS type 2 table - IDEA SYSTEM ONLY");
			}
			return result;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000069CC File Offset: 0x00004BCC
		private string GetVersion()
		{
			string result = string.Empty;
			StringBuilder stringBuilder = new StringBuilder(1024);
			try
			{
				Marshal.PtrToStringAnsi(SMBiosInformationAgent.SystemVersion(stringBuilder));
				result = stringBuilder.ToString();
				this.GetLastWin32Error();
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the System Version from SMBIOS type 1 table");
			}
			return result;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00006A24 File Offset: 0x00004C24
		private string GetSKU()
		{
			string result = string.Empty;
			StringBuilder stringBuilder = new StringBuilder(1024);
			try
			{
				Marshal.PtrToStringAnsi(SMBiosInformationAgent.SKU(stringBuilder));
				result = stringBuilder.ToString();
				this.GetLastWin32Error();
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the SKU from SMBIOS type 1 table");
			}
			return result;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00006A7C File Offset: 0x00004C7C
		private string GetFamily()
		{
			string result = string.Empty;
			StringBuilder stringBuilder = new StringBuilder(1024);
			try
			{
				Marshal.PtrToStringAnsi(SMBiosInformationAgent.Family(stringBuilder));
				result = stringBuilder.ToString();
				this.GetLastWin32Error();
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the system family from SMBIOS type 1 table");
			}
			return result;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00006AD4 File Offset: 0x00004CD4
		private string GetEnclosureType()
		{
			string result = string.Empty;
			StringBuilder stringBuilder = new StringBuilder(1024);
			try
			{
				Marshal.PtrToStringAnsi(SMBiosInformationAgent.EnclosureType(stringBuilder));
				result = stringBuilder.ToString();
				this.GetLastWin32Error();
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the enclosure type from SMBIOS type 3 table");
			}
			return result;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00006B2C File Offset: 0x00004D2C
		private string GetAssetTagNumber()
		{
			string result = string.Empty;
			StringBuilder stringBuilder = new StringBuilder(1024);
			try
			{
				Marshal.PtrToStringAnsi(SMBiosInformationAgent.AssetTagNumber(stringBuilder));
				result = stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the asset tag number from SMBIOS type 3 table");
			}
			return result;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00006B7C File Offset: 0x00004D7C
		private string GetProductName()
		{
			string result = string.Empty;
			StringBuilder stringBuilder = new StringBuilder(1024);
			try
			{
				Marshal.PtrToStringAnsi(SMBiosInformationAgent.ProductName(stringBuilder));
				result = stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the product name from SMBIOS type 2 table");
			}
			return result;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00006BCC File Offset: 0x00004DCC
		private void GetLastWin32Error()
		{
			if (Marshal.GetLastWin32Error() != 0)
			{
				Logger.Log(Logger.LogSeverity.Error, new Win32Exception(Marshal.GetLastWin32Error()).Message);
			}
		}

		// Token: 0x0600007E RID: 126
		[DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
		[DllImport("SMBiosInformationRetriever.dll")]
		private static extern bool CheckMachineInformationAvailability();

		// Token: 0x0600007F RID: 127
		[DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
		[DllImport("SMBiosInformationRetriever.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr BiosVersion(StringBuilder bio);

		// Token: 0x06000080 RID: 128
		[DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
		[DllImport("SMBiosInformationRetriever.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Manufacturer(StringBuilder manu);

		// Token: 0x06000081 RID: 129
		[DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
		[DllImport("SMBiosInformationRetriever.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr ModelNumber(StringBuilder modNum);

		// Token: 0x06000082 RID: 130
		[DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
		[DllImport("SMBiosInformationRetriever.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr SerialNumber(StringBuilder serNum);

		// Token: 0x06000083 RID: 131
		[DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
		[DllImport("SMBiosInformationRetriever.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr IdeaSerialNumber(StringBuilder ideaSerNum);

		// Token: 0x06000084 RID: 132
		[DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
		[DllImport("SMBiosInformationRetriever.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr SystemVersion(StringBuilder ver);

		// Token: 0x06000085 RID: 133
		[DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
		[DllImport("SMBiosInformationRetriever.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr SKU(StringBuilder SKUNum);

		// Token: 0x06000086 RID: 134
		[DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
		[DllImport("SMBiosInformationRetriever.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr Family(StringBuilder fam);

		// Token: 0x06000087 RID: 135
		[DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
		[DllImport("SMBiosInformationRetriever.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr EnclosureType(StringBuilder encType);

		// Token: 0x06000088 RID: 136
		[DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
		[DllImport("SMBiosInformationRetriever.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr AssetTagNumber(StringBuilder assetTagNum);

		// Token: 0x06000089 RID: 137
		[DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
		[DllImport("SMBiosInformationRetriever.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern IntPtr ProductName(StringBuilder productName);

		// Token: 0x04000034 RID: 52
		private const int SMBiosBufferSize = 1024;
	}
}
