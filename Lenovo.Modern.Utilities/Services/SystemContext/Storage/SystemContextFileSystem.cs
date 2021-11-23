using System;
using System.IO;
using Lenovo.Modern.Utilities.Services.Wrappers.Storage;

namespace Lenovo.Modern.Utilities.Services.SystemContext.Storage
{
	// Token: 0x0200002B RID: 43
	public class SystemContextFileSystem : IFileSystem
	{
		// Token: 0x0600010D RID: 269 RVA: 0x00005E98 File Offset: 0x00004098
		public SystemContextFileSystem()
		{
			this._systemPathMapper = SystemPathMapper.Instance;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005EAB File Offset: 0x000040AB
		public IDirectory LoadDirectory(string path)
		{
			return new SystemContextDirectory(new DirectoryInfo(this._systemPathMapper.GetUserContextFolder(path)));
		}

		// Token: 0x0400004B RID: 75
		private ISystemPathMapper _systemPathMapper;
	}
}
