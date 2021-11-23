using System;

namespace Lenovo.Modern.ImController.Shared.Utilities.Validation
{
	// Token: 0x0200003A RID: 58
	public interface IPluginVerifier
	{
		// Token: 0x0600019D RID: 413
		void VerifyAllPluginsInDirectory(string path);

		// Token: 0x0600019E RID: 414
		void VerifyPlugin(string pluginFullPath);
	}
}
