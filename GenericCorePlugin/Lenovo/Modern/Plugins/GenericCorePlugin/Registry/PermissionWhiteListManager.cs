using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.Registry
{
	// Token: 0x02000006 RID: 6
	internal class PermissionWhiteListManager
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00004C3C File Offset: 0x00002E3C
		public async Task<IEnumerable<string>> PermissionWhiteListManagerAsync(string val)
		{
			IEnumerable<string> result;
			if (val.Equals("Set", StringComparison.InvariantCultureIgnoreCase))
			{
				result = await this.SetWhiteListDirectoriesAsync();
			}
			else
			{
				result = await this.GetWhiteListDirectoriesAsync();
			}
			return result;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004C8C File Offset: 0x00002E8C
		private Task<List<string>> SetWhiteListDirectoriesAsync()
		{
			List<string> list = new List<string>();
			foreach (string item in new string[] { "HKEY_CURRENT_USER\\Software\\Lenovo" })
			{
				list.Add(item);
			}
			return Task.FromResult<List<string>>(list);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00004CD0 File Offset: 0x00002ED0
		private Task<List<string>> GetWhiteListDirectoriesAsync()
		{
			List<string> list = new List<string>();
			foreach (string item in new string[] { "HKEY_CURRENT_USER", "HKEY_LOCAL_MACHINE", "HKEY_CLASSES_ROOT" })
			{
				list.Add(item);
			}
			return Task.FromResult<List<string>>(list);
		}
	}
}
