using System;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppLauncher
{
	// Token: 0x0200003C RID: 60
	public class ShortcutPathDetails
	{
		// Token: 0x06000169 RID: 361 RVA: 0x0000A2DD File Offset: 0x000084DD
		public ShortcutPathDetails(string targetPath, string arguments)
		{
			this.Path = targetPath;
			this.Arguments = arguments;
		}

		// Token: 0x04000097 RID: 151
		public string Path;

		// Token: 0x04000098 RID: 152
		public string Arguments;
	}
}
