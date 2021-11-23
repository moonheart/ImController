using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Lenovo.Modern.ImController.ImClient.Utilities;
using Microsoft.Win32;

namespace Lenovo.Modern.ImController.ImClient.Plugin
{
	// Token: 0x0200000B RID: 11
	public class PluginManager
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002DC6 File Offset: 0x00000FC6
		public static PluginManager GetInstance()
		{
			PluginManager result;
			if ((result = PluginManager._instance) == null)
			{
				result = (PluginManager._instance = new PluginManager());
			}
			return result;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002DDC File Offset: 0x00000FDC
		private PluginManager()
		{
			this._currentProcessPrivilege = new ProcessPrivilegeDetector().GetCurrentProcessPrivilege();
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002DF4 File Offset: 0x00000FF4
		public void Setup(string pluginName)
		{
			this.PluginName = pluginName;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002DFD File Offset: 0x00000FFD
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002E05 File Offset: 0x00001005
		public string PluginName { get; private set; }

		// Token: 0x06000031 RID: 49 RVA: 0x00002E10 File Offset: 0x00001010
		public bool SetPluginSetting(string settingName, object value, RegistryValueKind valueKind)
		{
			bool result = false;
			try
			{
				RegistryKey registryKey = RegistryKey.OpenBaseKey((this._currentProcessPrivilege == ProcessPrivilegeDetector.RunAsPrivilege.System) ? RegistryHive.LocalMachine : RegistryHive.CurrentUser, RegistryView.Default);
				if (registryKey != null)
				{
					RegistryKey registryKey2 = registryKey.CreateSubKey(string.Format("{0}\\{1}", "Software\\Lenovo\\ImController\\PluginData", this.PluginName));
					if (registryKey2 != null)
					{
						registryKey2.SetValue("ImController.Privilege.ReceiveEvents", value, RegistryValueKind.DWord);
						result = true;
					}
				}
			}
			catch (Exception ex)
			{
				ExternalLogger.Instance.Log(ex, "Unable to save plugin setting for " + this.PluginName);
			}
			return result;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002EA0 File Offset: 0x000010A0
		public bool SetEventPreference(PluginManager.PluginEventPreference preference)
		{
			this.AssertSetupCalled();
			return this.SetPluginSetting("ImController.Privilege.ReceiveEvents", (preference == PluginManager.PluginEventPreference.Enabled) ? 1 : 0, RegistryValueKind.DWord);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002EC4 File Offset: 0x000010C4
		public bool UnloadPlugin()
		{
			bool result = false;
			Dictionary<string, string> commandLineArgsDictionary = this.GetCommandLineArgsDictionary();
			if (commandLineArgsDictionary.ContainsKey("name"))
			{
				try
				{
					string text = commandLineArgsDictionary["name"];
					if (text != null)
					{
						string name = "Global\\evt_" + text;
						bool flag = false;
						using (EventWaitHandle eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, name, ref flag))
						{
							if (!flag)
							{
								result = true;
								eventWaitHandle.Set();
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}
			return result;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002F50 File Offset: 0x00001150
		private void AssertSetupCalled()
		{
			if (string.IsNullOrWhiteSpace(this.PluginName))
			{
				throw new InvalidOperationException("You must call the Setup() method on PluginManager prior to using");
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002F6C File Offset: 0x0000116C
		private Dictionary<string, string> GetCommandLineArgsDictionary()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Count<string>(); i++)
			{
				if (commandLineArgs[i].StartsWith("-") && commandLineArgs[i].Length > 1)
				{
					if (i + 1 < commandLineArgs.Count<string>() && !commandLineArgs[i + 1].StartsWith("-"))
					{
						dictionary.Add(commandLineArgs[i].Substring(1), commandLineArgs[i + 1]);
					}
					else
					{
						dictionary.Add(commandLineArgs[i].Substring(1), null);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x0400000E RID: 14
		private readonly ProcessPrivilegeDetector.RunAsPrivilege _currentProcessPrivilege;

		// Token: 0x0400000F RID: 15
		private static PluginManager _instance;

		// Token: 0x0200004A RID: 74
		public enum PluginEventPreference
		{
			// Token: 0x040000E3 RID: 227
			Disabled,
			// Token: 0x040000E4 RID: 228
			Enabled
		}

		// Token: 0x0200004B RID: 75
		internal static class PluginManagerConstants
		{
			// Token: 0x040000E5 RID: 229
			public const string RegistryKeyPathToPluginData = "Software\\Lenovo\\ImController\\PluginData";

			// Token: 0x040000E6 RID: 230
			public const string NameOfEventPreference = "ImController.Privilege.ReceiveEvents";
		}
	}
}
