using System;
using System.Threading.Tasks;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x02000020 RID: 32
	public interface IPluginRepository
	{
		// Token: 0x060000BE RID: 190
		Task<bool> InstallPackageAsync(string packagePath);

		// Token: 0x060000BF RID: 191
		PluginRepository.PluginInformation GetPluginPathWithPluginName(string pluginName);
	}
}
