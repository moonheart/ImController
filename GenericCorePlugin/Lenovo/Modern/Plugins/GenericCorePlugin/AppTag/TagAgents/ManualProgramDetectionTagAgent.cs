using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.Tags;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents
{
	// Token: 0x02000021 RID: 33
	internal class ManualProgramDetectionTagAgent : ITagAgent
	{
		// Token: 0x060000FB RID: 251 RVA: 0x0000820D File Offset: 0x0000640D
		public ManualProgramDetectionTagAgent()
		{
			this._registrySystem = new RegistrySystem();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00008220 File Offset: 0x00006420
		public Task<IEnumerable<Tag>> CollectTagsAsync()
		{
			List<Tag> list = new List<Tag>();
			Tag lscTagIfPresent = this.GetLscTagIfPresent();
			if (lscTagIfPresent != null)
			{
				list.Add(lscTagIfPresent);
			}
			Tag dolbyAudioTagIfPresent = this.GetDolbyAudioTagIfPresent();
			if (dolbyAudioTagIfPresent != null)
			{
				list.Add(dolbyAudioTagIfPresent);
			}
			Tag maxxAudioTagIfPresent = this.GetMaxxAudioTagIfPresent();
			if (maxxAudioTagIfPresent != null)
			{
				list.Add(maxxAudioTagIfPresent);
			}
			return Task.FromResult<IEnumerable<Tag>>(list);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000826C File Offset: 0x0000646C
		private Tag GetDolbyAudioTagIfPresent()
		{
			try
			{
				Tag result = new Tag("System.Audio.Dolby", "");
				string path = "HKEY_CLASSES_ROOT\\CLSID\\{E0760680-E3E3-41A6-A5BE-275F5C21BDD9}";
				string path2 = "HKEY_CLASSES_ROOT\\CLSID\\{54E2FC61-FA9E-4667-BC4B-005BD2EF3EA1}";
				string path3 = "HKEY_CLASSES_ROOT\\CLSID\\{6A28A945-790C-4B68-B0F4-34EEB1626EE3}";
				if (this.DoesRegistryKeyExist(path))
				{
					return result;
				}
				if (this.DoesRegistryKeyExist(path2))
				{
					return result;
				}
				if (this.DoesRegistryKeyExist(path3))
				{
					return result;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Error while detecting dolby System.Audio.Dolby");
			}
			return null;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000082E8 File Offset: 0x000064E8
		private Tag GetMaxxAudioTagIfPresent()
		{
			try
			{
				Tag result = new Tag("System.Audio.MaxxAudio", "");
				string path = "HKEY_USERS\\.DEFAULT\\Software\\Waves Audio\\Installer";
				string value = "Error Code";
				int data = 0;
				if (this.DoesRegistryKeyValueDataExist(path, value, data))
				{
					return result;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Error while detecting tag: System.Audio.MaxxAudio");
			}
			return null;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00008348 File Offset: 0x00006548
		private Tag GetLscTagIfPresent()
		{
			Tag result = null;
			try
			{
				string value = this.DetectLscIsInstalled();
				if (!string.IsNullOrWhiteSpace(value))
				{
					result = new Tag
					{
						Key = "lsc.exe",
						Value = value
					};
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Error while detecting tag: lsc.exe");
			}
			return result;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000083A0 File Offset: 0x000065A0
		private string DetectLscIsInstalled()
		{
			string result = string.Empty;
			RegistrySystem registrySystem = new RegistrySystem();
			IContainer container = registrySystem.LoadContainer("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\LSC.exe");
			if (container != null)
			{
				result = container.GetValue("Version").GetValueAsString();
			}
			else
			{
				IContainer container2 = registrySystem.LoadContainer("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\App Paths\\LSC.exe");
				if (container2 != null)
				{
					result = container2.GetValue("Version").GetValueAsString();
				}
			}
			return result;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000083FC File Offset: 0x000065FC
		private bool DoesRegistryKeyExist(string path)
		{
			bool result = false;
			try
			{
				result = this._registrySystem.LoadContainer(path) != null;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "checking registry key {0}", new object[] { path });
			}
			return result;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00008444 File Offset: 0x00006644
		private bool DoesRegistryKeyValueDataExist(string path, string value, int data)
		{
			bool result = false;
			try
			{
				IContainer container = this._registrySystem.LoadContainer(path);
				if (container != null)
				{
					IContainerValue value2 = container.GetValue(value);
					if (value2 != null)
					{
						int? valueAsInt = value2.GetValueAsInt();
						if (valueAsInt != null)
						{
							int? num = valueAsInt;
							result = (num.GetValueOrDefault() == data) & (num != null);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "checking registry key {0}", new object[] { path });
			}
			return result;
		}

		// Token: 0x0400005F RID: 95
		private readonly RegistrySystem _registrySystem;

		// Token: 0x04000060 RID: 96
		private const string LscRegistryKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\LSC.exe";

		// Token: 0x04000061 RID: 97
		private const string LscRegistryKeyWow6432 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\App Paths\\LSC.exe";
	}
}
