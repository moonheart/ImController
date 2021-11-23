using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lenovo.Modern.Utilities.Services.Logging;
using Microsoft.Win32;

namespace Lenovo.Modern.Utilities.Services.Wrappers.Settings
{
	// Token: 0x02000016 RID: 22
	public class RegistrySystem : IContainerSystem
	{
		// Token: 0x06000075 RID: 117 RVA: 0x00003058 File Offset: 0x00001258
		public IContainer LoadContainer(string path)
		{
			RegistryContainer result = null;
			Tuple<string, string> baseKeyStringAndSubKeyString = this.GetBaseKeyStringAndSubKeyString(path);
			if (baseKeyStringAndSubKeyString != null)
			{
				using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistrySystem.RootKeyDictionary[baseKeyStringAndSubKeyString.Item1], RegistryView.Default))
				{
					string name = Path.Combine(path.Split(new char[] { '\\' }).Skip(1).ToArray<string>());
					using (RegistryKey registryKey2 = registryKey.OpenSubKey(name))
					{
						if (registryKey2 != null)
						{
							result = new RegistryContainer(RegistrySystem.RootKeyDictionary[baseKeyStringAndSubKeyString.Item1], RegistryView.Default, baseKeyStringAndSubKeyString.Item2);
						}
						else
						{
							using (RegistryKey registryKey3 = RegistryKey.OpenBaseKey(RegistrySystem.RootKeyDictionary[baseKeyStringAndSubKeyString.Item1], RegistryView.Registry64))
							{
								string name2 = Path.Combine(path.Split(new char[] { '\\' }).Skip(1).ToArray<string>());
								using (RegistryKey registryKey4 = registryKey3.OpenSubKey(name2))
								{
									if (registryKey4 != null)
									{
										result = new RegistryContainer(RegistrySystem.RootKeyDictionary[baseKeyStringAndSubKeyString.Item1], RegistryView.Registry64, baseKeyStringAndSubKeyString.Item2);
										Logger.Log(Logger.LogSeverity.Information, "Registry lookup required 64-bit registry view for key: {0}", new object[] { path });
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000031C8 File Offset: 0x000013C8
		private Tuple<string, string> GetBaseKeyStringAndSubKeyString(string fullPath)
		{
			Tuple<string, string> result = null;
			if (!string.IsNullOrWhiteSpace(fullPath))
			{
				string text = fullPath.Split(new char[] { '\\' }).FirstOrDefault<string>();
				string item = fullPath.Replace(text + "\\", string.Empty);
				result = new Tuple<string, string>(text, item);
			}
			return result;
		}

		// Token: 0x04000016 RID: 22
		private static readonly IReadOnlyDictionary<string, RegistryHive> RootKeyDictionary = new Dictionary<string, RegistryHive>
		{
			{
				"HKEY_CLASSES_ROOT",
				RegistryHive.ClassesRoot
			},
			{
				"HKEY_CURRENT_USER",
				RegistryHive.CurrentUser
			},
			{
				"HKEY_LOCAL_MACHINE",
				RegistryHive.LocalMachine
			},
			{
				"HKEY_USERS",
				RegistryHive.Users
			},
			{
				"HKEY_CURRENT_CONFIG",
				RegistryHive.CurrentConfig
			}
		};
	}
}
