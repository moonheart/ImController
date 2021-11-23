using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using Lenovo.Modern.CoreTypes.Contracts.Capabilities.CapabilityResponse;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation.StorageInformation;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.StorageInformation
{
	// Token: 0x02000005 RID: 5
	public class StorageInformationAgent
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00003A48 File Offset: 0x00001C48
		public StorageInformationAgent()
		{
			Logger.IsLoggingEnabled = true;
			this.MediaTypeDictionary = new Dictionary<string, string>
			{
				{
					"External hard disk media".ToLower(),
					"true"
				},
				{
					"Removable media other than floppy".ToLower(),
					"true"
				},
				{
					"Fixed hard disk media".ToLower(),
					"false"
				},
				{
					"Format is unknown".ToLower(),
					"false"
				},
				{
					"Removable media".ToLower(),
					"true"
				},
				{
					"Fixed hard disk".ToLower(),
					"false"
				},
				{
					"Unknown".ToLower(),
					"false"
				}
			};
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003B00 File Offset: 0x00001D00
		public StorageInformationResponse GetStorageInformation()
		{
			StorageInformationResponse storageInformationResponse = new StorageInformationResponse();
			List<PhysicalDisk> list = new List<PhysicalDisk>();
			try
			{
				List<StorageInformationAgent.Win32DiskDrive> win32DiskDriveInformation = this.GetWin32DiskDriveInformation();
				if (win32DiskDriveInformation != null && win32DiskDriveInformation.Any<StorageInformationAgent.Win32DiskDrive>())
				{
					foreach (StorageInformationAgent.Win32DiskDrive win32Disk in win32DiskDriveInformation)
					{
						try
						{
							PhysicalDisk physicalDisk = new PhysicalDisk();
							physicalDisk.Manufacturer = this.GetManufacturer(win32Disk) ?? string.Empty;
							physicalDisk.Model = this.GetModel(win32Disk) ?? string.Empty;
							physicalDisk.Removable = this.GetRemovable(win32Disk) ?? string.Empty;
							physicalDisk.ReturnCode = "0";
							physicalDisk.SerialNumber = this.GetSerialNumber(win32Disk) ?? string.Empty;
							physicalDisk.Size = this.GetStorageSize(win32Disk) ?? string.Empty;
							physicalDisk.Status = this.GetStorageStatus(win32Disk) ?? string.Empty;
							physicalDisk.Caption = this.GetStorageCaption(win32Disk) ?? string.Empty;
							physicalDisk.DeviceID = this.GetStorageDeviceID(win32Disk) ?? string.Empty;
							physicalDisk.FirmwareRevision = this.GetStorageFirmwareRevision(win32Disk) ?? string.Empty;
							physicalDisk.FreeSpace = this.GetStorageFreeSpace(win32Disk) ?? string.Empty;
							physicalDisk.PartitionStyle = this.GetStoragePartitionStyle(win32Disk) ?? string.Empty;
							physicalDisk.LogicalDiskList = this.GetLogicalDiskList(win32Disk);
							physicalDisk.VolumeName = this.GetVolumeName(physicalDisk.LogicalDiskList) ?? string.Empty;
							physicalDisk.DriveLetter = this.GetDriveLetter(physicalDisk.LogicalDiskList) ?? string.Empty;
							physicalDisk.Primary = this.GetPrimary(physicalDisk.LogicalDiskList) ?? string.Empty;
							list.Add(physicalDisk);
						}
						catch (Exception ex)
						{
							Logger.Log(ex, "Exception thrown trying to initialize a physical disk object for the storage information response");
						}
					}
					storageInformationResponse.PhysicalDiskList = list.ToArray();
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "Exception thrown trying to get the storage information on the user's PC");
			}
			return storageInformationResponse;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003D60 File Offset: 0x00001F60
		public CapabilityResponse GetCapability()
		{
			CapabilityResponse capabilityResponse = new CapabilityResponse();
			try
			{
				capabilityResponse.CapabilityList = new Capability[1];
				uint num = 0U;
				while ((ulong)num < (ulong)((long)capabilityResponse.CapabilityList.Length))
				{
					capabilityResponse.CapabilityList[(int)num] = new Capability();
					if (num == 0U)
					{
						capabilityResponse.CapabilityList[(int)num].Key = (new Capability().Key = "StorageInformation");
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

		// Token: 0x0600002F RID: 47 RVA: 0x00003DF8 File Offset: 0x00001FF8
		private string GetManufacturer(StorageInformationAgent.Win32DiskDrive win32Disk)
		{
			string result = string.Empty;
			try
			{
				if (win32Disk != null && win32Disk.DiskInfo != null)
				{
					string valueFromDictionary = this.GetValueFromDictionary(win32Disk.DiskInfo, "Model");
					if (!string.IsNullOrWhiteSpace(valueFromDictionary))
					{
						result = valueFromDictionary.Split(new char[] { ' ' }).FirstOrDefault<string>();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the manufacturer for a win32 physical disk");
			}
			return result;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003E68 File Offset: 0x00002068
		private string GetModel(StorageInformationAgent.Win32DiskDrive win32Disk)
		{
			string result = string.Empty;
			try
			{
				if (win32Disk != null && win32Disk.DiskInfo != null)
				{
					string valueFromDictionary = this.GetValueFromDictionary(win32Disk.DiskInfo, "Model");
					if (!string.IsNullOrWhiteSpace(valueFromDictionary))
					{
						string[] array = valueFromDictionary.Split(new char[] { ' ' });
						if (array != null)
						{
							result = string.Join(" ", array.Skip(1));
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the model of the physical disk");
			}
			return result;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003EE8 File Offset: 0x000020E8
		private string GetPrimary(LogicalDisk[] logicalDiskList)
		{
			string result = string.Empty;
			try
			{
				string b = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)).Replace("\\", "");
				foreach (LogicalDisk logicalDisk in logicalDiskList)
				{
					try
					{
						if (string.Equals(logicalDisk.Caption ?? string.Empty, b, StringComparison.OrdinalIgnoreCase))
						{
							result = "true";
							break;
						}
						result = "false";
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "Exception thrown looping through the primary disk checker");
					}
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "Exception thrown trying to determine if a physical disk is the primary disk");
			}
			return result;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003F8C File Offset: 0x0000218C
		private string GetRemovable(StorageInformationAgent.Win32DiskDrive win32Disk)
		{
			string result = string.Empty;
			string empty = string.Empty;
			try
			{
				if (win32Disk != null && win32Disk.DiskInfo != null)
				{
					string valueFromDictionary = this.GetValueFromDictionary(win32Disk.DiskInfo, "MediaType");
					if (!string.IsNullOrWhiteSpace(valueFromDictionary))
					{
						result = this.GetValueFromDictionary(this.MediaTypeDictionary, valueFromDictionary.ToLower());
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to determine if the physical disk is removable");
			}
			return result;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003FFC File Offset: 0x000021FC
		private string GetSerialNumber(StorageInformationAgent.Win32DiskDrive win32Disk)
		{
			string result = string.Empty;
			try
			{
				if (win32Disk != null && win32Disk.DiskInfo != null)
				{
					string valueFromDictionary = this.GetValueFromDictionary(win32Disk.DiskInfo, "SerialNumber");
					if (!string.IsNullOrWhiteSpace(valueFromDictionary))
					{
						result = valueFromDictionary;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the serial number");
			}
			return result;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00004058 File Offset: 0x00002258
		private string GetStorageSize(StorageInformationAgent.Win32DiskDrive win32Disk)
		{
			string result = string.Empty;
			try
			{
				if (win32Disk != null && win32Disk.DiskInfo != null)
				{
					string valueFromDictionary = this.GetValueFromDictionary(win32Disk.DiskInfo, "Size");
					if (!string.IsNullOrWhiteSpace(valueFromDictionary))
					{
						result = valueFromDictionary;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the storage size of a physical disk");
			}
			return result;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000040B4 File Offset: 0x000022B4
		private string GetStorageStatus(StorageInformationAgent.Win32DiskDrive win32Disk)
		{
			string result = string.Empty;
			try
			{
				if (win32Disk != null && win32Disk.DiskInfo != null)
				{
					string valueFromDictionary = this.GetValueFromDictionary(win32Disk.DiskInfo, "Status");
					if (!string.IsNullOrWhiteSpace(valueFromDictionary))
					{
						result = valueFromDictionary;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the status of the storage");
			}
			return result;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00004110 File Offset: 0x00002310
		private string GetStorageCaption(StorageInformationAgent.Win32DiskDrive win32Disk)
		{
			string result = string.Empty;
			try
			{
				if (win32Disk != null && win32Disk.DiskInfo != null)
				{
					string valueFromDictionary = this.GetValueFromDictionary(win32Disk.DiskInfo, "Description");
					string valueFromDictionary2 = this.GetValueFromDictionary(win32Disk.DiskInfo, "Index");
					result = string.Format("{0} {1}", valueFromDictionary, valueFromDictionary2);
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the storage caption");
			}
			return result;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00004180 File Offset: 0x00002380
		private string GetStorageDeviceID(StorageInformationAgent.Win32DiskDrive win32Disk)
		{
			string result = string.Empty;
			try
			{
				if (win32Disk != null && win32Disk.DiskInfo != null)
				{
					string valueFromDictionary = this.GetValueFromDictionary(win32Disk.DiskInfo, "DeviceID");
					if (!string.IsNullOrWhiteSpace(valueFromDictionary))
					{
						result = valueFromDictionary;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the storage device ID of a physical disk");
			}
			return result;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000041DC File Offset: 0x000023DC
		private string GetStorageFirmwareRevision(StorageInformationAgent.Win32DiskDrive win32Disk)
		{
			string result = string.Empty;
			try
			{
				if (win32Disk != null && win32Disk.DiskInfo != null)
				{
					string valueFromDictionary = this.GetValueFromDictionary(win32Disk.DiskInfo, "FirmwareRevision");
					if (!string.IsNullOrWhiteSpace(valueFromDictionary))
					{
						result = valueFromDictionary;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the storage firmware revision");
			}
			return result;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00004238 File Offset: 0x00002438
		private string GetStorageFreeSpace(StorageInformationAgent.Win32DiskDrive win32Disk)
		{
			return string.Empty;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00004240 File Offset: 0x00002440
		private string GetStoragePartitionStyle(StorageInformationAgent.Win32DiskDrive win32Disk)
		{
			string result = "Unknown";
			try
			{
				if (win32Disk != null && win32Disk.PartitionList != null)
				{
					foreach (Dictionary<string, string> dictionary in win32Disk.PartitionList)
					{
						string valueFromDictionary = this.GetValueFromDictionary(dictionary, "Type");
						if (!string.IsNullOrWhiteSpace(valueFromDictionary))
						{
							if (valueFromDictionary.StartsWith("GPT"))
							{
								result = "GPT";
							}
							else
							{
								result = "MBR";
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the partition style");
			}
			return result;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000042E8 File Offset: 0x000024E8
		private string GetVolumeName(LogicalDisk[] logicalDiskArray)
		{
			string result = string.Empty;
			try
			{
				foreach (LogicalDisk logicalDisk in logicalDiskArray)
				{
					if (!string.IsNullOrWhiteSpace(logicalDisk.VolumeName))
					{
						result = logicalDisk.VolumeName;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the volume name for a whole physical disk to make it easier to identify");
			}
			return result;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00004344 File Offset: 0x00002544
		private string GetDriveLetter(LogicalDisk[] logicalDiskArray)
		{
			string result = string.Empty;
			try
			{
				foreach (LogicalDisk logicalDisk in logicalDiskArray)
				{
					if (!string.IsNullOrWhiteSpace(logicalDisk.Caption))
					{
						result = logicalDisk.Caption;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the drive letter for a whole physical disk to make it easier to identify");
			}
			return result;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000043A0 File Offset: 0x000025A0
		private List<StorageInformationAgent.Win32DiskDrive> GetWin32DiskDriveInformation()
		{
			List<StorageInformationAgent.Win32DiskDrive> list = new List<StorageInformationAgent.Win32DiskDrive>();
			try
			{
				using (ManagementClass managementClass = new ManagementClass("Win32_DiskDrive"))
				{
					foreach (ManagementBaseObject managementBaseObject in managementClass.GetInstances())
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						StorageInformationAgent.Win32DiskDrive win32DiskDrive = new StorageInformationAgent.Win32DiskDrive();
						win32DiskDrive.DiskInfo = new Dictionary<string, string>();
						win32DiskDrive.PartitionList = new List<Dictionary<string, string>>();
						win32DiskDrive.LogicalDiskList = new List<Dictionary<string, string>>();
						try
						{
							if (managementObject.Properties != null)
							{
								foreach (PropertyData propertyData in managementObject.Properties)
								{
									try
									{
										string name = propertyData.Name;
										string value = string.Empty;
										if (propertyData.Value != null)
										{
											value = propertyData.Value.ToString();
										}
										win32DiskDrive.DiskInfo.Add(name, value);
									}
									catch (Exception ex)
									{
										Logger.Log(ex, "Exception thrown trying to get a physical disk property: " + propertyData.Name);
									}
								}
							}
						}
						catch (Exception ex2)
						{
							Logger.Log(ex2, "Exception thrown trying to get physical disk information");
						}
						foreach (ManagementBaseObject managementBaseObject2 in managementObject.GetRelated("Win32_DiskPartition"))
						{
							ManagementObject managementObject2 = (ManagementObject)managementBaseObject2;
							try
							{
								if (managementObject2.Properties != null)
								{
									Dictionary<string, string> dictionary = new Dictionary<string, string>();
									foreach (PropertyData propertyData2 in managementObject2.Properties)
									{
										try
										{
											string name2 = propertyData2.Name;
											string value2 = string.Empty;
											if (propertyData2.Value != null)
											{
												value2 = propertyData2.Value.ToString();
											}
											dictionary.Add(name2, value2);
										}
										catch (Exception ex3)
										{
											Logger.Log(ex3, "Exception thrown while trying to get a disk partition property: " + propertyData2.Name);
										}
									}
									win32DiskDrive.PartitionList.Add(dictionary);
								}
							}
							catch (Exception ex4)
							{
								Logger.Log(ex4, "Exception thrown trying to get disk partition information");
							}
							foreach (ManagementBaseObject managementBaseObject3 in managementObject2.GetRelated("Win32_LogicalDisk"))
							{
								ManagementObject managementObject3 = (ManagementObject)managementBaseObject3;
								try
								{
									if (managementObject3.Properties != null)
									{
										Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
										foreach (PropertyData propertyData3 in managementObject3.Properties)
										{
											try
											{
												string name3 = propertyData3.Name;
												string value3 = string.Empty;
												if (propertyData3.Value != null)
												{
													value3 = propertyData3.Value.ToString();
												}
												dictionary2.Add(name3, value3);
											}
											catch (Exception ex5)
											{
												Logger.Log(ex5, "Exception thrown trying to get a logical disk property: " + propertyData3.Name);
											}
										}
										win32DiskDrive.LogicalDiskList.Add(dictionary2);
									}
								}
								catch (Exception ex6)
								{
									Logger.Log(ex6, "Exception thrown trying to get logical disk information");
								}
							}
						}
						list.Add(win32DiskDrive);
					}
				}
			}
			catch (Exception ex7)
			{
				Logger.Log(ex7, "Exception thrown getting physical disks");
			}
			return list;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00004820 File Offset: 0x00002A20
		private LogicalDisk[] GetLogicalDiskList(StorageInformationAgent.Win32DiskDrive win32Disk)
		{
			List<LogicalDisk> list = new List<LogicalDisk>();
			try
			{
				if (win32Disk != null && win32Disk.LogicalDiskList != null)
				{
					foreach (Dictionary<string, string> logicalDisk in win32Disk.LogicalDiskList)
					{
						try
						{
							Dictionary<string, string> matchedVolume = this.MatchVolumeToLogicalDisk(logicalDisk);
							LogicalDisk item = this.CreateLogicalDisk(logicalDisk, matchedVolume);
							list.Add(item);
						}
						catch (Exception ex)
						{
							Logger.Log(ex, "Exception thrown trying to create a logical disk for a physical disk");
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "Exception thrown trying to get the logical disk list");
			}
			return list.ToArray();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000048D4 File Offset: 0x00002AD4
		public LogicalDisk CreateLogicalDisk(Dictionary<string, string> logicalDisk, Dictionary<string, string> matchedVolume)
		{
			LogicalDisk logicalDisk2 = new LogicalDisk();
			logicalDisk2.Alert = string.Empty;
			if (logicalDisk != null)
			{
				logicalDisk2.Caption = this.GetValueFromDictionary(logicalDisk, "Caption") ?? string.Empty;
				logicalDisk2.Dsk_DeviceID = this.GetValueFromDictionary(logicalDisk, "DeviceID") ?? string.Empty;
				logicalDisk2.FileSystem = this.GetValueFromDictionary(logicalDisk, "FileSystem") ?? string.Empty;
				logicalDisk2.FreeSpace = this.GetValueFromDictionary(logicalDisk, "FreeSpace") ?? string.Empty;
				logicalDisk2.Size = this.GetValueFromDictionary(logicalDisk, "Size") ?? string.Empty;
				logicalDisk2.VolumeName = this.GetValueFromDictionary(logicalDisk, "VolumeName") ?? string.Empty;
			}
			if (matchedVolume != null)
			{
				logicalDisk2.BootVolume = this.GetValueFromDictionary(matchedVolume, "BootVolume") ?? string.Empty;
				logicalDisk2.DeviceID = this.GetValueFromDictionary(matchedVolume, "DeviceID") ?? string.Empty;
			}
			logicalDisk2.UsedSpace = this.CalculateUsedSpaceOnDiskAsString(logicalDisk2.Size, logicalDisk2.FreeSpace) ?? string.Empty;
			return logicalDisk2;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000049F8 File Offset: 0x00002BF8
		private string GetValueFromDictionary(IDictionary<string, string> dictionary, string key)
		{
			string empty = string.Empty;
			if (dictionary != null && !string.IsNullOrWhiteSpace(key))
			{
				dictionary.TryGetValue(key, out empty);
			}
			return empty;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00004A24 File Offset: 0x00002C24
		private Dictionary<string, string> MatchVolumeToLogicalDisk(Dictionary<string, string> logicalDisk)
		{
			Dictionary<string, string> result = new Dictionary<string, string>();
			try
			{
				using (ManagementClass managementClass = new ManagementClass("Win32_Volume"))
				{
					foreach (ManagementBaseObject managementBaseObject in managementClass.GetInstances())
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						try
						{
							Dictionary<string, string> dictionary = new Dictionary<string, string>();
							foreach (PropertyData propertyData in managementObject.Properties)
							{
								try
								{
									string name = propertyData.Name;
									string value = string.Empty;
									if (propertyData.Value != null)
									{
										value = propertyData.Value.ToString();
									}
									dictionary.Add(name, value);
								}
								catch (Exception ex)
								{
									Logger.Log(ex, "Exception thrown trying to get a Win32_Volume property for a potential matching volume");
								}
							}
							string valueFromDictionary = this.GetValueFromDictionary(dictionary, "DriveLetter");
							string valueFromDictionary2 = this.GetValueFromDictionary(logicalDisk, "Caption");
							if (!string.IsNullOrWhiteSpace(valueFromDictionary) && !string.IsNullOrWhiteSpace(valueFromDictionary2) && string.Equals(valueFromDictionary, valueFromDictionary2, StringComparison.OrdinalIgnoreCase))
							{
								result = dictionary;
								break;
							}
						}
						catch (Exception ex2)
						{
							Logger.Log(ex2, "Exception thrown trying to map a Win32_Volume to a logical disk");
						}
					}
				}
			}
			catch (Exception ex3)
			{
				Logger.Log(ex3, "Exception thrown trying to match the Win32 volume to the logical disk");
			}
			return result;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004BF4 File Offset: 0x00002DF4
		private string CalculateUsedSpaceOnDiskAsString(string size, string freespace)
		{
			string result = string.Empty;
			if (!string.IsNullOrWhiteSpace(size) && !string.IsNullOrWhiteSpace(freespace))
			{
				long num = 0L;
				long.TryParse(size, out num);
				long num2 = 0L;
				long.TryParse(freespace, out num2);
				result = (num - num2).ToString();
			}
			return result;
		}

		// Token: 0x04000012 RID: 18
		private ProcessPrivilegeDetector _processPrivilegeDetector;

		// Token: 0x04000013 RID: 19
		private string _process;

		// Token: 0x04000014 RID: 20
		private readonly IDictionary<string, string> MediaTypeDictionary;

		// Token: 0x02000060 RID: 96
		private class Win32DiskDrive
		{
			// Token: 0x17000023 RID: 35
			// (get) Token: 0x060001BD RID: 445 RVA: 0x0000CD06 File Offset: 0x0000AF06
			// (set) Token: 0x060001BE RID: 446 RVA: 0x0000CD0E File Offset: 0x0000AF0E
			public Dictionary<string, string> DiskInfo { get; set; }

			// Token: 0x17000024 RID: 36
			// (get) Token: 0x060001BF RID: 447 RVA: 0x0000CD17 File Offset: 0x0000AF17
			// (set) Token: 0x060001C0 RID: 448 RVA: 0x0000CD1F File Offset: 0x0000AF1F
			public List<Dictionary<string, string>> PartitionList { get; set; }

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000CD28 File Offset: 0x0000AF28
			// (set) Token: 0x060001C2 RID: 450 RVA: 0x0000CD30 File Offset: 0x0000AF30
			public List<Dictionary<string, string>> LogicalDiskList { get; set; }
		}
	}
}
