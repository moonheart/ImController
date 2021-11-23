using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.FileSystem
{
	// Token: 0x02000019 RID: 25
	internal class PermissionWhiteListManager
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x00007AB8 File Offset: 0x00005CB8
		public async Task<IEnumerable<string>> PermissionSourceWhiteListManagerAsync()
		{
			return await this.GetSourceWhiteListDirectoriesAsync();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00007B00 File Offset: 0x00005D00
		public async Task<IEnumerable<string>> PermissionDestinationWhiteListManagerAsync()
		{
			return await this.GetDestinationWhiteListDirectoriesAsync();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00007B48 File Offset: 0x00005D48
		private Task<List<string>> GetSourceWhiteListDirectoriesAsync()
		{
			return Task.FromResult<List<string>>(new List<string> { "%programdata%\\Lenovo\\", "%SystemRoot%\\Lenovo\\", "%programfiles%\\Lenovo\\", "%programfiles(x86)%\\Lenovo\\", "%localappdata%\\Packages\\LenovoCorporation.LenovoSettings_4642shxvsv8s2\\LocalState", "%localappdata%\\Packages\\E046963F.LenovoCompanion_k1h2ywk1493x8\\LocalState", "%systemdrive%\\UserGuidePDF" });
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00007BAC File Offset: 0x00005DAC
		private Task<List<string>> GetDestinationWhiteListDirectoriesAsync()
		{
			return Task.FromResult<List<string>>(new List<string> { "%programdata%\\Lenovo\\ImController\\PluginData", "%localappdata%\\Packages\\LenovoCorporation.LenovoSettings_4642shxvsv8s2\\LocalState", "%localappdata%\\Packages\\E046963F.LenovoCompanion_k1h2ywk1493x8\\LocalState", "%localappdata%\\Packages\\windows.immersivecontrolpanel_cw5n1h2txyewy\\LocalState\\Indexed" });
		}
	}
}
