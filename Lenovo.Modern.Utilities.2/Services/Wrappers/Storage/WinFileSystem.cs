using System;
using System.IO;

namespace Lenovo.Modern.Utilities.Services.Wrappers.Storage
{
	// Token: 0x02000010 RID: 16
	public class WinFileSystem : IFileSystem
	{
		// Token: 0x06000054 RID: 84 RVA: 0x00002B64 File Offset: 0x00000D64
		public IDirectory LoadDirectory(string path)
		{
			return new WinDirectory(new DirectoryInfo(Environment.ExpandEnvironmentVariables(path)));
		}
	}
}
