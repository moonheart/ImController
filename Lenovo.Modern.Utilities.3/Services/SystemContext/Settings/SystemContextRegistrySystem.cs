using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lenovo.Modern.Utilities.Services.SystemContext.Shared;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;
using Microsoft.Win32;

namespace Lenovo.Modern.Utilities.Services.SystemContext.Settings
{
	// Token: 0x0200002F RID: 47
	public class SystemContextRegistrySystem : IContainerSystem
	{
		// Token: 0x0600011F RID: 287 RVA: 0x0000616C File Offset: 0x0000436C
		public SystemContextRegistrySystem()
		{
			this._userInformationProvider = UserInformationProvider.Instance;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006180 File Offset: 0x00004380
		public IContainer LoadContainer(string path)
		{
			if (path.ToUpper().Contains("HKEY_CURRENT_USER"))
			{
				UserInformation userInformation = this._userInformationProvider.GetUserInformation();
				path = path.Replace("HKEY_CURRENT_USER", "HKEY_USERS\\" + userInformation.SID);
			}
			RegistryContainer result = null;
			try
			{
				Tuple<string, string> baseKeyStringAndSubKeyString = this.GetBaseKeyStringAndSubKeyString(path);
				if (baseKeyStringAndSubKeyString != null)
				{
					using (RegistryKey registryKey = RegistryKey.OpenBaseKey(SystemContextRegistrySystem.RootKeyDictionary[baseKeyStringAndSubKeyString.Item1], RegistryView.Default))
					{
						string name = Path.Combine(path.Split(new char[] { '\\' }).Skip(1).ToArray<string>());
						using (RegistryKey registryKey2 = registryKey.OpenSubKey(name))
						{
							if (registryKey2 != null)
							{
								result = new RegistryContainer(SystemContextRegistrySystem.RootKeyDictionary[baseKeyStringAndSubKeyString.Item1], RegistryView.Default, baseKeyStringAndSubKeyString.Item2);
							}
							else
							{
								using (RegistryKey registryKey3 = RegistryKey.OpenBaseKey(SystemContextRegistrySystem.RootKeyDictionary[baseKeyStringAndSubKeyString.Item1], RegistryView.Registry64))
								{
									string name2 = Path.Combine(path.Split(new char[] { '\\' }).Skip(1).ToArray<string>());
									using (RegistryKey registryKey4 = registryKey3.OpenSubKey(name2))
									{
										if (registryKey4 != null)
										{
											result = new RegistryContainer(SystemContextRegistrySystem.RootKeyDictionary[baseKeyStringAndSubKeyString.Item1], RegistryView.Registry64, baseKeyStringAndSubKeyString.Item2);
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006368 File Offset: 0x00004568
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

		// Token: 0x04000053 RID: 83
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

		// Token: 0x04000054 RID: 84
		private IUserInformationProvider _userInformationProvider;
	}
}
