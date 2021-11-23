using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;
using Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;
using Microsoft.Win32;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation
{
	// Token: 0x02000009 RID: 9
	public class MachineInformationAgent
	{
		// Token: 0x0600005E RID: 94 RVA: 0x00005EA0 File Offset: 0x000040A0
		private MachineInformationAgent()
		{
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00005EB4 File Offset: 0x000040B4
		public static MachineInformationAgent GetInstance()
		{
			MachineInformationAgent result;
			if ((result = MachineInformationAgent._instance) == null)
			{
				result = (MachineInformationAgent._instance = new MachineInformationAgent());
			}
			return result;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00005ECC File Offset: 0x000040CC
		public async Task<MachineInformation> GetMachineInformation()
		{
			await this._machineInfoSemaphore.WaitAsync();
			try
			{
				if (this._cachedMachineInformation == null)
				{
					MachineInformation machineInformation = new MachineInformation();
					SMBiosInformation smbiosInformation = new SMBiosInformationAgent().GetSMBiosInformation();
					string text = MachineInformationAgent.SanitizeString(smbiosInformation.SKU);
					string familyFromBios = MachineInformationAgent.SanitizeString(smbiosInformation.Family);
					string text2 = MachineInformationAgent.SanitizeString(smbiosInformation.Manufacturer);
					machineInformation.BiosVersion = MachineInformationAgent.SanitizeString(smbiosInformation.BiosVersion);
					machineInformation.BiosDate = MachineInformationAgent.SanitizeString(smbiosInformation.BiosDate);
					machineInformation.Brand = new BrandAgent().GetBrand(text, familyFromBios, text2);
					machineInformation.CountryCode = new CountryCodeAgent().GetCountryCode();
					machineInformation.DateCreated = new DateTimeOffset(DateTime.Now);
					machineInformation.Enclosure = this.ConvertEnclosureStringToEnum(smbiosInformation.EnclosureType);
					machineInformation.ECVersion = MachineInformationAgent.SanitizeString(smbiosInformation.ECVersion);
					machineInformation.Family = new FamilyAgent().GetFamily(familyFromBios, text);
					machineInformation.FirstRunDate = this.GetFirstRunDate();
					machineInformation.Locale = this.GetLocale();
					machineInformation.Manufacturer = text2;
					machineInformation.MTM = MachineInformationAgent.SanitizeString(smbiosInformation.MTM);
					OperatingSystemAgent operatingSystemAgent = new OperatingSystemAgent();
					machineInformation.OperatingSystemVerion = operatingSystemAgent.GetOperatingSystemBuildNumber();
					machineInformation.OSName = operatingSystemAgent.GetOperatingSystemProductName();
					machineInformation.OS = operatingSystemAgent.GetOS();
					machineInformation.OperatingSystemVerion = operatingSystemAgent.GetOperatingSystemBuildNumber();
					machineInformation.OperatingSystemBitness = (Environment.Is64BitOperatingSystem ? "64" : "32");
					machineInformation.CPUAddressWidth = (Environment.Is64BitOperatingSystem ? "64" : "32");
					machineInformation.CPUArchitecture = this.GetCPUArchitecture();
					machineInformation.PreloadTagList = new List<string>().ToArray();
					machineInformation.SerialNumber = this.GetSerialNumber(machineInformation.Brand, machineInformation.Enclosure, MachineInformationAgent.SanitizeString(smbiosInformation.SerialNumber), MachineInformationAgent.SanitizeString(smbiosInformation.IdeaSerialNumber));
					machineInformation.SKU = text;
					machineInformation.SubBrand = new SubBrandAgent().GetSubBrand(text, smbiosInformation.Family, machineInformation.Brand.ToString());
					this._cachedMachineInformation = machineInformation;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown during information retrieval for the machine information");
			}
			finally
			{
				this._machineInfoSemaphore.Release();
			}
			return this._cachedMachineInformation;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00005F14 File Offset: 0x00004114
		private string GetSerialNumber(BrandType brand, EnclosureType enclosureType, string type1SerialNumber, string type2SerialNumber)
		{
			string result = string.Empty;
			try
			{
				if (brand == BrandType.Idea || brand == BrandType.IdeaTab)
				{
					if (enclosureType == EnclosureType.Notebook || enclosureType == EnclosureType.HandHeld || enclosureType == EnclosureType.Laptop || enclosureType == EnclosureType.Tablet)
					{
						result = type2SerialNumber;
					}
					else
					{
						result = type1SerialNumber;
					}
				}
				else
				{
					result = type1SerialNumber;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown getting the system's serial number");
			}
			return result;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00005F6C File Offset: 0x0000416C
		private string GetLocale()
		{
			string result = string.Empty;
			string name = CultureInfo.CurrentUICulture.Name;
			try
			{
				if (!string.IsNullOrWhiteSpace(name))
				{
					if (name.StartsWith("zh-", StringComparison.OrdinalIgnoreCase))
					{
						if (name.StartsWith("zh-cn", StringComparison.OrdinalIgnoreCase))
						{
							result = "zh-hans";
						}
						else if (name.StartsWith("zh-tw", StringComparison.OrdinalIgnoreCase))
						{
							result = "zh-hant";
						}
						else
						{
							result = name;
						}
					}
					else if (name.Equals("pt-br", StringComparison.OrdinalIgnoreCase))
					{
						result = name;
					}
					else if (name.Equals("no", StringComparison.OrdinalIgnoreCase))
					{
						result = "nb";
					}
					else
					{
						string[] array = name.Split(new char[] { '-' });
						if (array != null && array.Length >= 1)
						{
							result = array[0] ?? name;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown getting the Locale");
			}
			return result;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00006040 File Offset: 0x00004240
		private EnclosureType ConvertEnclosureStringToEnum(string enclosureString)
		{
			EnclosureType enclosureType = EnclosureType.None;
			if (string.Equals(enclosureString, "notebook", StringComparison.OrdinalIgnoreCase) || string.Equals(enclosureString, "convertible", StringComparison.OrdinalIgnoreCase) || string.Equals(enclosureString, "detachable", StringComparison.OrdinalIgnoreCase))
			{
				enclosureType = EnclosureType.Notebook;
			}
			else if (string.Equals(enclosureString, "laptop", StringComparison.OrdinalIgnoreCase))
			{
				enclosureType = EnclosureType.Laptop;
			}
			else if (string.Equals(enclosureString, "desktop", StringComparison.OrdinalIgnoreCase))
			{
				enclosureType = EnclosureType.Desktop;
			}
			else if (string.Equals(enclosureString, "tower", StringComparison.OrdinalIgnoreCase))
			{
				enclosureType = EnclosureType.Tower;
			}
			else if (string.Equals(enclosureString, "allinone", StringComparison.OrdinalIgnoreCase) || string.Equals(enclosureString, "all in one", StringComparison.OrdinalIgnoreCase))
			{
				enclosureType = EnclosureType.AllInOne;
			}
			else if (string.Equals(enclosureString, "workstation", StringComparison.OrdinalIgnoreCase))
			{
				enclosureType = EnclosureType.Workstation;
			}
			else if (string.Equals(enclosureString, "handheld", StringComparison.OrdinalIgnoreCase))
			{
				enclosureType = EnclosureType.HandHeld;
			}
			else if (string.Equals(enclosureString, "tablet", StringComparison.OrdinalIgnoreCase))
			{
				enclosureType = EnclosureType.Tablet;
			}
			else if (string.Equals(enclosureString, "other", StringComparison.OrdinalIgnoreCase))
			{
				enclosureType = EnclosureType.Other;
			}
			else if (string.Equals(enclosureString, "minipc", StringComparison.OrdinalIgnoreCase) || string.Equals(enclosureString, "mini pc", StringComparison.OrdinalIgnoreCase))
			{
				enclosureType = EnclosureType.Desktop;
			}
			else if (string.Equals(enclosureString, "stickpc", StringComparison.OrdinalIgnoreCase) || string.Equals(enclosureString, "stick pc", StringComparison.OrdinalIgnoreCase))
			{
				enclosureType = EnclosureType.Desktop;
			}
			try
			{
				if (enclosureType == EnclosureType.None && string.Equals(new Regex("\\s+", RegexOptions.Compiled).Replace(enclosureString, ""), "allinone", StringComparison.OrdinalIgnoreCase))
				{
					enclosureType = EnclosureType.AllInOne;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown doing no whitespace comparison for allinone");
			}
			return enclosureType;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000061B4 File Offset: 0x000043B4
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

		// Token: 0x06000065 RID: 101 RVA: 0x00006254 File Offset: 0x00004454
		private static string GetSysPrepPath()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "Sysprep");
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00006268 File Offset: 0x00004468
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

		// Token: 0x06000067 RID: 103 RVA: 0x000062E8 File Offset: 0x000044E8
		private DateTime GetFirstRunDate()
		{
			DateTime result = default(DateTime);
			try
			{
				if (MachineInformationAgent.IsWindowsInAuditBoot())
				{
					result = DateTime.Now;
				}
				else
				{
					string empty = string.Empty;
					result = Directory.GetLastWriteTime(MachineInformationAgent.GetDefaultUserDocumentsPath());
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown getting the first run date");
			}
			return result;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000633C File Offset: 0x0000453C
		private string GetCPUArchitecture()
		{
			string text = string.Empty;
			string key = string.Empty;
			try
			{
				IContainer container = new RegistrySystem().LoadContainer(Constants.WindowsCPURegistryKey);
				if (container != null)
				{
					IContainerValue value = container.GetValue(Constants.CPUIdentifierRegistryValue);
					if (value != null)
					{
						text = value.GetValueAsString();
					}
				}
				if (text.Contains("Intel64"))
				{
					key = "9";
				}
				else
				{
					key = "0";
				}
				Constants.CPUValueDictionary.TryGetValue(key, out text);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown getting the CPU architecture");
			}
			return text;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000063C8 File Offset: 0x000045C8
		private static string SanitizeString(string text)
		{
			string result = text;
			if (text != null)
			{
				try
				{
					result = Regex.Replace(text, "[^\\w\\.@\\-_ ]", string.Empty, RegexOptions.None, TimeSpan.FromMilliseconds(200.0));
				}
				catch (Exception)
				{
					result = string.Empty;
				}
			}
			return result;
		}

		// Token: 0x0400001E RID: 30
		private static readonly string OobeSetupFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Setup\\State\\State.ini");

		// Token: 0x0400001F RID: 31
		private const string profileList = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\ProfileList";

		// Token: 0x04000020 RID: 32
		private const string defaultAccount = "Default";

		// Token: 0x04000021 RID: 33
		private MachineInformation _cachedMachineInformation;

		// Token: 0x04000022 RID: 34
		private SemaphoreSlim _machineInfoSemaphore = new SemaphoreSlim(1);

		// Token: 0x04000023 RID: 35
		private static MachineInformationAgent _instance;
	}
}
