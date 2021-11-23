using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Contracts.FileSystem;
using Lenovo.Modern.Utilities.Services.Wrappers.Storage;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.FileSystem
{
	// Token: 0x02000016 RID: 22
	public class FileSystemAgent
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x00007940 File Offset: 0x00005B40
		public FileSystemAgent()
		{
			FileSystemAgent._winFileSystem = new WinFileSystem();
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00007960 File Offset: 0x00005B60
		public async Task<DirectoryListingResponse> GetDirectoryListing(DirectoryListingRequest requestXml)
		{
			DirectoryListingResponse response = null;
			if (requestXml != null)
			{
				response = new DirectoryListingResponse();
				List<Lenovo.Modern.CoreTypes.Contracts.FileSystem.Directory> directoryList = new List<Lenovo.Modern.CoreTypes.Contracts.FileSystem.Directory>();
				for (int index = 0; index < requestXml.DirectoryList.Count<Lenovo.Modern.CoreTypes.Contracts.FileSystem.Directory>(); index++)
				{
					try
					{
						Lenovo.Modern.CoreTypes.Contracts.FileSystem.Directory directory = requestXml.DirectoryList.ElementAt(index);
						new DirectoryInfo(directory.Location);
						this._directory = FileSystemAgent.LoadFileSystem.LoadFileSystemBasedOnPrivilege(directory.Location);
						if (!this._directory.Exists)
						{
							throw new DirectoryNotFoundException("Directory not exists : " + directory.Location);
						}
						directoryList.Add(directory);
						IEnumerable<IDirectory> source = await this._directory.GetDirectoriesAsync();
						List<Item> items = new List<Item>();
						items.AddRange(from s in source
							select new Item
							{
								Name = s.Name,
								FileSize = (double)s.Length,
								Type = "directory",
								FullPath = s.FullPath
							});
						IEnumerable<IFile> source2 = await this._directory.GetFilesAsync();
						items.AddRange(from f in source2
							select new Item
							{
								Name = f.Filename,
								FileSize = (double)f.Length,
								Type = "file",
								FullPath = f.FullPath
							});
						directory.Items = items.ToArray();
						items = null;
						directory = null;
					}
					catch (Exception item)
					{
						this.loopExceptions.Add(item);
					}
				}
				response.DirectoryList = requestXml.DirectoryList;
				directoryList = null;
			}
			return response;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000079AD File Offset: 0x00005BAD
		public List<Exception> exceptionList()
		{
			return this.loopExceptions;
		}

		// Token: 0x04000045 RID: 69
		private List<Exception> loopExceptions = new List<Exception>();

		// Token: 0x04000046 RID: 70
		private IDirectory _directory;

		// Token: 0x04000047 RID: 71
		private static IFileSystem _winFileSystem;

		// Token: 0x04000048 RID: 72
		private static bool isUser;

		// Token: 0x0200006C RID: 108
		public static class LoadFileSystem
		{
			// Token: 0x060001D4 RID: 468 RVA: 0x0000D29C File Offset: 0x0000B49C
			public static IDirectory LoadFileSystemBasedOnPrivilege(string location)
			{
				return FileSystemAgent._winFileSystem.LoadDirectory(location);
			}
		}
	}
}
