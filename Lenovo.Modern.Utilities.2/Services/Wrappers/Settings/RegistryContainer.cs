using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;

namespace Lenovo.Modern.Utilities.Services.Wrappers.Settings
{
	// Token: 0x02000014 RID: 20
	public class RegistryContainer : IContainer
	{
		// Token: 0x06000066 RID: 102 RVA: 0x00002B76 File Offset: 0x00000D76
		public RegistryContainer(RegistryHive registryHive, RegistryView registryView, string relativePath)
		{
			if (!string.IsNullOrWhiteSpace(relativePath))
			{
				this._relativeContainerPath = relativePath;
			}
			this._registryHive = registryHive;
			this._registryView = registryView;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002B9C File Offset: 0x00000D9C
		public IEnumerable<IContainerValue> GetValues(bool IsADSetting = false)
		{
			List<IContainerValue> list = new List<IContainerValue>();
			using (RegistryKey registryKeyForThisContainer = this.GetRegistryKeyForThisContainer(false))
			{
				foreach (string name in registryKeyForThisContainer.GetValueNames())
				{
					list.Add(new RegistryContainerValue(name, registryKeyForThisContainer.GetValue(name).ToString()));
				}
			}
			return list;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002C08 File Offset: 0x00000E08
		public string GetPath()
		{
			return Path.Combine(RegistryContainer.RootKeyDictionary[this._registryHive], this._relativeContainerPath);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002C28 File Offset: 0x00000E28
		public IEnumerable<string> GetSubContainerNames()
		{
			List<string> list = new List<string>();
			using (RegistryKey registryKeyForThisContainer = this.GetRegistryKeyForThisContainer(false))
			{
				string[] subKeyNames = registryKeyForThisContainer.GetSubKeyNames();
				if (subKeyNames != null)
				{
					foreach (string item in subKeyNames)
					{
						list.Add(item);
					}
				}
			}
			return list;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002C8C File Offset: 0x00000E8C
		public IContainer GetSubContainer(string containerName)
		{
			RegistryContainer result = null;
			using (RegistryKey registryKeyForThisContainer = this.GetRegistryKeyForThisContainer(false))
			{
				using (RegistryKey registryKey = registryKeyForThisContainer.OpenSubKey(containerName, RegistryKeyPermissionCheck.ReadWriteSubTree))
				{
					if (registryKey != null)
					{
						result = new RegistryContainer(this._registryHive, this._registryView, Path.Combine(this._relativeContainerPath, containerName));
					}
				}
			}
			return result;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002D00 File Offset: 0x00000F00
		public bool CreateSubContainer(string containerName)
		{
			bool result = false;
			using (RegistryKey registryKeyForThisContainer = this.GetRegistryKeyForThisContainer(true))
			{
				if (registryKeyForThisContainer.CreateSubKey(containerName, RegistryKeyPermissionCheck.ReadWriteSubTree) != null)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002D40 File Offset: 0x00000F40
		public bool CreateSubContainer(string containerName, IDictionary<string, string> containerValues)
		{
			bool result = false;
			using (RegistryKey registryKeyForThisContainer = this.GetRegistryKeyForThisContainer(true))
			{
				RegistryKey registryKey = registryKeyForThisContainer.CreateSubKey(containerName, RegistryKeyPermissionCheck.ReadWriteSubTree);
				if (registryKey != null)
				{
					foreach (KeyValuePair<string, string> keyValuePair in containerValues)
					{
						registryKey.SetValue(keyValuePair.Key, keyValuePair.Value);
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002DC8 File Offset: 0x00000FC8
		public bool DeleteSubContainer(string containerName)
		{
			bool result = false;
			using (RegistryKey registryKeyForThisContainer = this.GetRegistryKeyForThisContainer(true))
			{
				if (registryKeyForThisContainer != null)
				{
					registryKeyForThisContainer.DeleteSubKeyTree(containerName, true);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002E0C File Offset: 0x0000100C
		public bool SetValue(string valueName, string value)
		{
			bool result = false;
			if (!string.IsNullOrWhiteSpace(valueName) && value != null)
			{
				using (RegistryKey registryKeyForThisContainer = this.GetRegistryKeyForThisContainer(true))
				{
					registryKeyForThisContainer.SetValue(valueName, value);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002E58 File Offset: 0x00001058
		public bool SetValue(string valueName, object value, RegistryKind registryKind)
		{
			bool result = false;
			if (!string.IsNullOrWhiteSpace(valueName) && value != null)
			{
				using (RegistryKey registryKeyForThisContainer = this.GetRegistryKeyForThisContainer(true))
				{
					switch (registryKind)
					{
					case RegistryKind.String:
						registryKeyForThisContainer.SetValue(valueName, value, RegistryValueKind.String);
						break;
					case RegistryKind.Binary:
						registryKeyForThisContainer.SetValue(valueName, value, RegistryValueKind.Binary);
						break;
					case RegistryKind.DWord:
						registryKeyForThisContainer.SetValue(valueName, value, RegistryValueKind.DWord);
						break;
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002ECC File Offset: 0x000010CC
		public bool DeleteValue(string valueName)
		{
			bool result = false;
			using (RegistryKey registryKeyForThisContainer = this.GetRegistryKeyForThisContainer(true))
			{
				registryKeyForThisContainer.DeleteValue(valueName, true);
				result = true;
			}
			return result;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002F0C File Offset: 0x0000110C
		public IContainerValue GetValue(string valueName)
		{
			RegistryContainerValue result = null;
			using (RegistryKey registryKeyForThisContainer = this.GetRegistryKeyForThisContainer(false))
			{
				object value = registryKeyForThisContainer.GetValue(valueName);
				if (value != null)
				{
					result = new RegistryContainerValue(valueName, value.ToString());
				}
			}
			return result;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002F58 File Offset: 0x00001158
		private RegistryKey GetRegistryKeyForThisContainer(bool IsADKey = true)
		{
			RegistryKey result = null;
			using (RegistryKey registryKey = RegistryKey.OpenBaseKey(this._registryHive, this._registryView))
			{
				result = registryKey.OpenSubKey(this._relativeContainerPath, IsADKey);
			}
			return result;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002FA4 File Offset: 0x000011A4
		private RegistryKey GetRegistryKeyForADContainer()
		{
			RegistryKey result = null;
			using (RegistryKey registryKey = RegistryKey.OpenBaseKey(this._registryHive, this._registryView))
			{
				result = registryKey.OpenSubKey(this._relativeContainerPath, false);
			}
			return result;
		}

		// Token: 0x0400000E RID: 14
		private string _relativeContainerPath;

		// Token: 0x0400000F RID: 15
		private RegistryHive _registryHive;

		// Token: 0x04000010 RID: 16
		private RegistryView _registryView;

		// Token: 0x04000011 RID: 17
		private static readonly IReadOnlyDictionary<RegistryHive, string> RootKeyDictionary = new Dictionary<RegistryHive, string>
		{
			{
				RegistryHive.ClassesRoot,
				"HKEY_CLASSES_ROOT"
			},
			{
				RegistryHive.CurrentUser,
				"HKEY_CURRENT_USER"
			},
			{
				RegistryHive.LocalMachine,
				"HKEY_LOCAL_MACHINE"
			},
			{
				RegistryHive.Users,
				"HKEY_USERS"
			},
			{
				RegistryHive.CurrentConfig,
				"HKEY_CURRENT_CONFIG"
			}
		};
	}
}
