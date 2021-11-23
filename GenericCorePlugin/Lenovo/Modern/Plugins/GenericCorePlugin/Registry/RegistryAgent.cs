using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lenovo.Modern.CoreTypes.Contracts.Registry;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;
using Microsoft.Win32;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.Registry
{
	// Token: 0x02000007 RID: 7
	public class RegistryAgent
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00004D21 File Offset: 0x00002F21
		public RegistryAgent()
		{
			this._whiteListManager = new PermissionWhiteListManager();
			this._registrySystem = new RegistrySystem();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004D4C File Offset: 0x00002F4C
		public KeyChildrenResponse GetKeyChildren(KeyChildrenRequest requestXml)
		{
			KeyChildrenResponse keyChildrenResponse = null;
			if (requestXml != null)
			{
				IEnumerable<string> result = this._whiteListManager.PermissionWhiteListManagerAsync("Get").Result;
				int count = requestXml.KeyList.ToList<KeyList>().Count;
				Hashtable hashtable = new Hashtable();
				int num = 0;
				for (int i = 0; i < count; i++)
				{
					try
					{
						this._container = this.LoadRegistryBasedOnPrivilege(requestXml.KeyList[i].Location);
						if (this._container == null)
						{
							throw new DirectoryNotFoundException("Directory not found when setting the Key Children in Registry Plugin: " + requestXml.KeyList[i].Location);
						}
						string path = this._container.GetPath();
						string[] array = path.Split(new string[] { "\\" }, StringSplitOptions.None);
						string a = array[0];
						RegistryKey registryKey;
						if (!(a == "HKEY_CLASSES_ROOT"))
						{
							if (!(a == "HKEY_CURRENT_USER"))
							{
								if (!(a == "HKEY_LOCAL_MACHINE"))
								{
									if (!(a == "HKEY_USERS"))
									{
										if (!(a == "HKEY_CURRENT_CONFIG"))
										{
											registryKey = Registry.CurrentUser.OpenSubKey(array[1]);
										}
										else
										{
											registryKey = Registry.CurrentConfig.OpenSubKey(array[1]);
										}
									}
									else
									{
										registryKey = Registry.Users.OpenSubKey(array[1]);
									}
								}
								else
								{
									registryKey = Registry.LocalMachine.OpenSubKey(array[1]);
								}
							}
							else
							{
								registryKey = Registry.CurrentUser.OpenSubKey(array[1]);
							}
						}
						else
						{
							registryKey = Registry.ClassesRoot.OpenSubKey(array[1]);
						}
						if (registryKey != null)
						{
							if (result.Any((string f) => path.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0))
							{
								try
								{
									List<string> list = this._container.GetSubContainerNames().ToList<string>();
									hashtable.Add(num, requestXml.KeyList[i].Location);
									num++;
									for (int j = 0; j < list.Count; j++)
									{
										hashtable.Add(num, requestXml.KeyList[i].Location + "\\" + list[j]);
										num++;
									}
									KeyList[] array2 = new KeyList[hashtable.Count];
									for (int k = 0; k < hashtable.Count; k++)
									{
										array2[k] = this.LoadRegistryKeyList(hashtable[k].ToString());
									}
									keyChildrenResponse = new KeyChildrenResponse();
									keyChildrenResponse.KeyList = array2;
									goto IL_27A;
								}
								catch (Exception item)
								{
									this.loopExceptions.Add(item);
									goto IL_27A;
								}
								goto IL_26E;
								IL_27A:
								goto IL_28D;
							}
							IL_26E:
							throw new UnauthorizedAccessException();
						}
						throw new KeyNotFoundException();
					}
					catch (Exception item2)
					{
						this.loopExceptions.Add(item2);
					}
					IL_28D:;
				}
				return keyChildrenResponse;
			}
			throw new ArgumentNullException();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00005034 File Offset: 0x00003234
		public void SetKeyChildren(KeyChildrenRequest requestXml)
		{
			if (requestXml != null)
			{
				IEnumerable<string> result = this._whiteListManager.PermissionWhiteListManagerAsync("Set").Result;
				int count = requestXml.KeyList.ToList<KeyList>().Count;
				for (int i = 0; i < count; i++)
				{
					try
					{
						this._container = this.LoadRegistryBasedOnPrivilege(requestXml.KeyList[i].Location);
						if (this._container == null)
						{
							throw new DirectoryNotFoundException("Directory not found when setting the Key Children in Registry Plugin: " + requestXml.KeyList[i].Location);
						}
						string path = this._container.GetPath();
						string[] array = path.Split(new char[] { '\\' }, 2);
						string a = array[0];
						RegistryKey registryKey;
						if (!(a == "HKEY_CLASSES_ROOT"))
						{
							if (!(a == "HKEY_CURRENT_USER"))
							{
								if (!(a == "HKEY_LOCAL_MACHINE"))
								{
									if (!(a == "HKEY_USERS"))
									{
										if (!(a == "HKEY_CURRENT_CONFIG"))
										{
											registryKey = Registry.CurrentUser.OpenSubKey(array[1]);
										}
										else
										{
											registryKey = Registry.CurrentConfig.OpenSubKey(array[1]);
										}
									}
									else
									{
										registryKey = Registry.Users.OpenSubKey(array[1]);
									}
								}
								else
								{
									registryKey = Registry.LocalMachine.OpenSubKey(array[1]);
								}
							}
							else
							{
								registryKey = Registry.CurrentUser.OpenSubKey(array[1]);
							}
						}
						else
						{
							registryKey = Registry.ClassesRoot.OpenSubKey(array[1]);
						}
						if (registryKey == null)
						{
							throw new KeyNotFoundException();
						}
						if (!result.Any((string f) => path.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0))
						{
							throw new UnauthorizedAccessException();
						}
						int count2 = requestXml.KeyList[i].KeyChildren.ToList<KeyChild>().Count;
						for (int j = 0; j < count2; j++)
						{
							try
							{
								Lenovo.Modern.CoreTypes.Contracts.Registry.RegistryKind type = requestXml.KeyList[i].KeyChildren[j].Type;
								if (type == Lenovo.Modern.CoreTypes.Contracts.Registry.RegistryKind.DWord)
								{
									uint num = Convert.ToUInt32(requestXml.KeyList[i].KeyChildren[j].Value);
									this._container.SetValue(requestXml.KeyList[i].KeyChildren[j].Name, num, Lenovo.Modern.Utilities.Services.Wrappers.Settings.RegistryKind.DWord);
								}
								else
								{
									this._container.SetValue(requestXml.KeyList[i].KeyChildren[j].Name, requestXml.KeyList[i].KeyChildren[j].Value, Lenovo.Modern.Utilities.Services.Wrappers.Settings.RegistryKind.String);
								}
							}
							catch (Exception item)
							{
								this.loopExceptions.Add(item);
							}
						}
					}
					catch (Exception item2)
					{
						this.loopExceptions.Add(item2);
					}
				}
				return;
			}
			throw new ArgumentNullException();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00005304 File Offset: 0x00003504
		public List<Exception> exceptionList()
		{
			return this.loopExceptions;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000530C File Offset: 0x0000350C
		private IContainer LoadRegistryBasedOnPrivilege(string location)
		{
			this._container = null;
			if (location != null)
			{
				this._container = this._registrySystem.LoadContainer(location);
			}
			return this._container;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00005330 File Offset: 0x00003530
		private KeyList LoadRegistryKeyList(string location)
		{
			this._keyList = null;
			if (location != null)
			{
				this._keyList = new KeyList();
				this._container = this.LoadRegistryBasedOnPrivilege(location);
				this._keyList.Location = location;
				KeyChild[] array = null;
				if (this._container != null)
				{
					List<IContainerValue> list = this._container.GetValues(true).ToList<IContainerValue>();
					array = new KeyChild[list.Count];
					for (int i = 0; i < list.Count; i++)
					{
						array[i] = new KeyChild();
						array[i].Name = list[i].GetName();
						array[i].Value = list[i].GetValueAsString();
					}
				}
				this._keyList.KeyChildren = array;
			}
			return this._keyList;
		}

		// Token: 0x04000015 RID: 21
		private List<Exception> loopExceptions = new List<Exception>();

		// Token: 0x04000016 RID: 22
		private KeyList _keyList;

		// Token: 0x04000017 RID: 23
		private IContainer _container;

		// Token: 0x04000018 RID: 24
		private IContainerSystem _registrySystem;

		// Token: 0x04000019 RID: 25
		private PermissionWhiteListManager _whiteListManager;
	}
}
