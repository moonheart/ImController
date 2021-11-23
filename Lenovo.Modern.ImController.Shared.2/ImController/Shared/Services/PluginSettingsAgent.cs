using System;
using System.Linq;
using Lenovo.Modern.CoreTypes.Models.Subscription;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.SystemContext.Settings;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x02000017 RID: 23
	public class PluginSettingsAgent
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00004888 File Offset: 0x00002A88
		public PluginSettingsAgent(PackageSubscription subscription)
		{
			if (subscription == null)
			{
				throw new ArgumentNullException("Cannot provide a null subscription");
			}
			this._subscription = subscription;
			this._systemContextRegistry = new SystemContextRegistrySystem();
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000048B0 File Offset: 0x00002AB0
		public PluginSettingsAgent.Setting GetPrioritizedSetting(string pluginName, string settingName, PluginSettingsAgent.SettingLocation prioritizedLocation)
		{
			if (prioritizedLocation == PluginSettingsAgent.SettingLocation.None)
			{
				throw new ArgumentException("You cannot request location=none");
			}
			if (string.IsNullOrWhiteSpace(pluginName) || string.IsNullOrWhiteSpace(settingName))
			{
				throw new ArgumentNullException("You must provide a plugin name and setting name");
			}
			Func<PluginSettingsAgent.Setting> func = delegate()
			{
				AppSetting settingFromManifest = this.GetSettingFromManifest(pluginName, settingName);
				if (settingFromManifest != null && !string.IsNullOrWhiteSpace(settingFromManifest.Value))
				{
					return new PluginSettingsAgent.Setting(settingFromManifest.Value, PluginSettingsAgent.SettingLocation.SetByManifest);
				}
				return null;
			};
			Func<PluginSettingsAgent.Setting> func2 = delegate()
			{
				IContainerValue settingFromRegistry = this.GetSettingFromRegistry(pluginName, settingName);
				if (settingFromRegistry != null)
				{
					return new PluginSettingsAgent.Setting(settingFromRegistry.GetValueAsString(), PluginSettingsAgent.SettingLocation.SetDynamically);
				}
				return null;
			};
			PluginSettingsAgent.Setting setting;
			switch (prioritizedLocation)
			{
			case PluginSettingsAgent.SettingLocation.SetByManifest:
				setting = func() ?? func2();
				goto IL_AF;
			case PluginSettingsAgent.SettingLocation.SetDynamically:
				setting = func2() ?? func();
				goto IL_AF;
			}
			throw new InvalidOperationException("Unexpected enum value");
			IL_AF:
			Logger.Log(Logger.LogSeverity.Information, "PluginSettingsAgent: Seeking setting '{0}' for plugin '{1}' with priority for '{2}'.  Found Value: '{3}' inside: '{4}'", new object[]
			{
				settingName,
				pluginName,
				prioritizedLocation,
				(setting == null) ? "null" : setting.ValueAsString,
				(setting == null) ? "null" : setting.Location.ToString()
			});
			return setting;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000049D0 File Offset: 0x00002BD0
		public AppSetting GetSettingFromManifest(string pluginName, string settingName)
		{
			if (string.IsNullOrWhiteSpace(pluginName) || string.IsNullOrWhiteSpace(settingName))
			{
				throw new ArgumentNullException("You must provide a plugin name and setting name");
			}
			AppSetting result = null;
			try
			{
				Lenovo.Modern.ImController.Shared.Model.Packages.Package package = this._subscription.PackageList.FirstOrDefault((Lenovo.Modern.ImController.Shared.Model.Packages.Package p) => p != null && p.PackageInformation != null && (!string.IsNullOrWhiteSpace(p.PackageInformation.Name) & p.PackageInformation.Name.Equals(pluginName, StringComparison.InvariantCultureIgnoreCase)));
				if (package == null)
				{
					Logger.Log(Logger.LogSeverity.Warning, "PluginSettingsAgent: Cannot get manifest setting '{0}' for '{1}' because plugin not found.", new object[] { settingName, pluginName });
				}
				else if (package.SettingList != null)
				{
					result = package.SettingList.FirstOrDefault((AppSetting s) => !string.IsNullOrWhiteSpace(s.Key) && s.Key.Equals(settingName));
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PluginSettingsAgent: Exception getting manifest setting {0} for {1}.", new object[] { settingName, pluginName });
			}
			return result;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004AB4 File Offset: 0x00002CB4
		public IContainerValue GetSettingFromRegistry(string pluginName, string settingName)
		{
			if (string.IsNullOrWhiteSpace(pluginName) || string.IsNullOrWhiteSpace(settingName))
			{
				throw new ArgumentNullException("You must provide a plugin name and setting name");
			}
			Func<bool, IContainerValue> func = delegate(bool isHklm)
			{
				IContainerValue result = null;
				try
				{
					IContainer container = this._systemContextRegistry.LoadContainer(string.Format("{0}\\{1}\\{2}", isHklm ? "HKEY_LOCAL_MACHINE" : "HKEY_CURRENT_USER", "Software\\Lenovo\\ImController\\PluginData", pluginName));
					if (container != null)
					{
						result = container.GetValue(settingName);
					}
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "PluginSettingsAgent: Exception getting registry setting '{0}' for '{1}' inside '{2}' ", new object[]
					{
						settingName,
						pluginName,
						isHklm ? "HKEY_LOCAL_MACHINE" : "HKEY_CURRENT_USER"
					});
				}
				return result;
			};
			return func(true) ?? func(false);
		}

		// Token: 0x04000062 RID: 98
		private readonly PackageSubscription _subscription;

		// Token: 0x04000063 RID: 99
		private readonly IContainerSystem _systemContextRegistry;

		// Token: 0x02000053 RID: 83
		public enum SettingLocation
		{
			// Token: 0x04000134 RID: 308
			None,
			// Token: 0x04000135 RID: 309
			SetByManifest,
			// Token: 0x04000136 RID: 310
			SetDynamically
		}

		// Token: 0x02000054 RID: 84
		public class Setting
		{
			// Token: 0x06000203 RID: 515 RVA: 0x0000A7C3 File Offset: 0x000089C3
			public Setting(string value, PluginSettingsAgent.SettingLocation location)
			{
				this._originalValue = value;
				this._location = location;
			}

			// Token: 0x17000053 RID: 83
			// (get) Token: 0x06000204 RID: 516 RVA: 0x0000A7D9 File Offset: 0x000089D9
			public PluginSettingsAgent.SettingLocation Location
			{
				get
				{
					return this._location;
				}
			}

			// Token: 0x17000054 RID: 84
			// (get) Token: 0x06000205 RID: 517 RVA: 0x0000A7E1 File Offset: 0x000089E1
			public string ValueAsString
			{
				get
				{
					return this._originalValue;
				}
			}

			// Token: 0x17000055 RID: 85
			// (get) Token: 0x06000206 RID: 518 RVA: 0x0000A7EC File Offset: 0x000089EC
			public int? ValueAsInt
			{
				get
				{
					int? result = null;
					int value;
					if (!string.IsNullOrWhiteSpace(this._originalValue) && int.TryParse(this._originalValue, out value))
					{
						result = new int?(value);
					}
					return result;
				}
			}

			// Token: 0x17000056 RID: 86
			// (get) Token: 0x06000207 RID: 519 RVA: 0x0000A828 File Offset: 0x00008A28
			public bool? ValueAsBool
			{
				get
				{
					bool? result = null;
					if (!string.IsNullOrWhiteSpace(this._originalValue))
					{
						string text = this._originalValue.Trim();
						bool value;
						if (bool.TryParse(text, out value))
						{
							result = new bool?(value);
						}
						else if (text == "0")
						{
							result = new bool?(false);
						}
						else if (text == "1")
						{
							result = new bool?(true);
						}
					}
					return result;
				}
			}

			// Token: 0x04000137 RID: 311
			private readonly string _originalValue;

			// Token: 0x04000138 RID: 312
			private readonly PluginSettingsAgent.SettingLocation _location;
		}

		// Token: 0x02000055 RID: 85
		private static class PluginSettingsConstants
		{
			// Token: 0x04000139 RID: 313
			public const string Hkcu = "HKEY_CURRENT_USER";

			// Token: 0x0400013A RID: 314
			public const string Hklm = "HKEY_LOCAL_MACHINE";
		}
	}
}
