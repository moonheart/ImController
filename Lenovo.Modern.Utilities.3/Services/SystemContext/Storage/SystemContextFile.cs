using System;
using System.IO;
using System.Threading.Tasks;
using Lenovo.Modern.Utilities.Services.Wrappers.Storage;

namespace Lenovo.Modern.Utilities.Services.SystemContext.Storage
{
	// Token: 0x0200002A RID: 42
	public class SystemContextFile : IFile
	{
		// Token: 0x060000FB RID: 251 RVA: 0x00005C7A File Offset: 0x00003E7A
		internal SystemContextFile(FileInfo fileInfo)
		{
			if (fileInfo == null)
			{
				throw new ArgumentException("File info was null, need the information");
			}
			this._fileInfo = fileInfo;
			this._systemPathMapper = SystemPathMapper.Instance;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00005CA2 File Offset: 0x00003EA2
		public string Filename
		{
			get
			{
				return this._fileInfo.Name;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00005CAF File Offset: 0x00003EAF
		public string DirectoryName
		{
			get
			{
				return this._fileInfo.Directory.Name;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00005CC1 File Offset: 0x00003EC1
		public string FullPath
		{
			get
			{
				return this._fileInfo.FullName;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00005CCE File Offset: 0x00003ECE
		public string DirectoryPath
		{
			get
			{
				return this._fileInfo.DirectoryName;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00005CDB File Offset: 0x00003EDB
		public IDirectory ParentDirectory
		{
			get
			{
				return new SystemContextDirectory(this._fileInfo.Directory);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00005CED File Offset: 0x00003EED
		public bool Exists
		{
			get
			{
				return this._fileInfo.Exists;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00005CFA File Offset: 0x00003EFA
		public DateTime DateLastModified
		{
			get
			{
				return this._fileInfo.LastWriteTime;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00005D07 File Offset: 0x00003F07
		public long Length
		{
			get
			{
				return this._fileInfo.Length;
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005D14 File Offset: 0x00003F14
		public Task<string> ReadContentsAsync()
		{
			this.AssertFileExists();
			return Task.Factory.StartNew<string>(() => File.ReadAllText(this.FullPath));
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005D34 File Offset: 0x00003F34
		public Task<bool> WriteContentsAsync(string contents, WritingOption writingOption)
		{
			this.AssertFileExists();
			return Task.Factory.StartNew<bool>(delegate()
			{
				WritingOption writingOption2 = writingOption;
				bool result;
				if (writingOption2 != WritingOption.Replace)
				{
					File.AppendAllText(this.FullPath, contents);
					result = true;
				}
				else
				{
					File.WriteAllText(this.FullPath, contents);
					result = true;
				}
				return result;
			});
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005D78 File Offset: 0x00003F78
		public Task<bool> MoveAsync(string destinationDirectoryPath, CollisionOption option)
		{
			this.AssertFileExists();
			return Task.Factory.StartNew<bool>(delegate()
			{
				string text = Path.Combine(this._systemPathMapper.GetUserContextFolder(destinationDirectoryPath), this.Filename);
				CollisionOption option2 = option;
				if (option2 != CollisionOption.ReplaceExisting && option2 == CollisionOption.ThrowExisting && File.Exists(text))
				{
					throw new Exceptions.FileAlreadyExistsException("File already exists in the target directory");
				}
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				this._fileInfo.MoveTo(text);
				return true;
			});
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005DBC File Offset: 0x00003FBC
		public Task<bool> CopyAsync(string destinationDirectoryPath, bool overwriteExisting)
		{
			this.AssertFileExists();
			return Task.Factory.StartNew<bool>(delegate()
			{
				string destFileName = Path.Combine(this._systemPathMapper.GetUserContextFolder(destinationDirectoryPath), this.Filename);
				this._fileInfo.CopyTo(destFileName, overwriteExisting);
				return true;
			});
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005E00 File Offset: 0x00004000
		public Task<bool> RenameAsync(string fileName)
		{
			this.AssertFileExists();
			return Task.Factory.StartNew<bool>(delegate()
			{
				string text = Path.Combine(this.DirectoryPath, fileName);
				File.Move(this.FullPath, text);
				this._fileInfo = new FileInfo(text);
				return true;
			});
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005E3D File Offset: 0x0000403D
		public Task<bool> DeleteAsync()
		{
			this.AssertFileExists();
			return Task.Factory.StartNew<bool>(delegate()
			{
				this._fileInfo.Delete();
				return true;
			});
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005E5B File Offset: 0x0000405B
		private void AssertFileExists()
		{
			if (this._fileInfo != null && !this._fileInfo.Exists)
			{
				throw new FileNotFoundException("The File does not exist!");
			}
		}

		// Token: 0x04000049 RID: 73
		private FileInfo _fileInfo;

		// Token: 0x0400004A RID: 74
		private ISystemPathMapper _systemPathMapper;
	}
}
