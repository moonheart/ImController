using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lenovo.Modern.Utilities.Services.Wrappers.Storage
{
	// Token: 0x0200000B RID: 11
	public interface IDirectory
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000D RID: 13
		string Name { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000E RID: 14
		string FullPath { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000F RID: 15
		IDirectory ParentDirectory { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000010 RID: 16
		bool Exists { get; }

		// Token: 0x06000011 RID: 17
		Task<IEnumerable<IFile>> GetFilesAsync();

		// Token: 0x06000012 RID: 18
		Task<IFile> GetFileAsync(string pathToFile);

		// Token: 0x06000013 RID: 19
		Task<IEnumerable<IDirectory>> GetDirectoriesAsync();

		// Token: 0x06000014 RID: 20
		Task<IDirectory> GetDirectoryAsync(string pathToDirectory);

		// Token: 0x06000015 RID: 21
		Task<IDirectory> CreateDirectoryAsync(string directoryPath, CreationOption option);

		// Token: 0x06000016 RID: 22
		Task<IFile> CreateFileAsync(string fileName, string contents, CreationOption option);

		// Token: 0x06000017 RID: 23
		Task<bool> MoveAsync(string destinationDirectoryPath, CollisionOption collisionOption);

		// Token: 0x06000018 RID: 24
		Task<bool> CopyAsync(string destinationPath, CollisionOption collisionOption);

		// Token: 0x06000019 RID: 25
		Task<bool> RenameAsync(string newName);

		// Token: 0x0600001A RID: 26
		Task<bool> DeleteAsync();

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001B RID: 27
		long Length { get; }

		// Token: 0x0600001C RID: 28
		void CalculateDirectorySize();
	}
}
