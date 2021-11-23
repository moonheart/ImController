using System;
using System.Threading.Tasks;

namespace Lenovo.Modern.Utilities.Services.Wrappers.Storage
{
	// Token: 0x0200000C RID: 12
	public interface IFile
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001D RID: 29
		string Filename { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001E RID: 30
		string DirectoryName { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001F RID: 31
		string FullPath { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000020 RID: 32
		long Length { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000021 RID: 33
		string DirectoryPath { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000022 RID: 34
		IDirectory ParentDirectory { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000023 RID: 35
		bool Exists { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000024 RID: 36
		DateTime DateLastModified { get; }

		// Token: 0x06000025 RID: 37
		Task<string> ReadContentsAsync();

		// Token: 0x06000026 RID: 38
		Task<bool> WriteContentsAsync(string contents, WritingOption writingOption);

		// Token: 0x06000027 RID: 39
		Task<bool> MoveAsync(string destinationDirectoryPath, CollisionOption option);

		// Token: 0x06000028 RID: 40
		Task<bool> CopyAsync(string destinationDirectoryPath, bool overwriteExisting);

		// Token: 0x06000029 RID: 41
		Task<bool> RenameAsync(string fileName);

		// Token: 0x0600002A RID: 42
		Task<bool> DeleteAsync();
	}
}
