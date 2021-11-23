using System;
using System.IO;
using System.Threading.Tasks;

namespace Lenovo.Modern.Utilities.Services.Wrappers.Storage
{
	// Token: 0x0200000F RID: 15
	public class WinFile : IFile
	{
		// Token: 0x06000042 RID: 66 RVA: 0x00002952 File Offset: 0x00000B52
		internal WinFile(FileInfo fileInfo)
		{
			if (fileInfo == null)
			{
				throw new ArgumentException("File info was null, need the information");
			}
			this._fileInfo = fileInfo;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000043 RID: 67 RVA: 0x0000296F File Offset: 0x00000B6F
		public string Filename
		{
			get
			{
				return this._fileInfo.Name;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000044 RID: 68 RVA: 0x0000297C File Offset: 0x00000B7C
		public string DirectoryName
		{
			get
			{
				return this._fileInfo.Directory.Name;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000045 RID: 69 RVA: 0x0000298E File Offset: 0x00000B8E
		public string FullPath
		{
			get
			{
				return this._fileInfo.FullName;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000046 RID: 70 RVA: 0x0000299B File Offset: 0x00000B9B
		public string DirectoryPath
		{
			get
			{
				return this._fileInfo.DirectoryName;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000029A8 File Offset: 0x00000BA8
		public IDirectory ParentDirectory
		{
			get
			{
				return new WinDirectory(this._fileInfo.Directory);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000048 RID: 72 RVA: 0x000029BA File Offset: 0x00000BBA
		public bool Exists
		{
			get
			{
				return this._fileInfo.Exists;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000049 RID: 73 RVA: 0x000029C7 File Offset: 0x00000BC7
		public long Length
		{
			get
			{
				return this._fileInfo.Length;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600004A RID: 74 RVA: 0x000029D4 File Offset: 0x00000BD4
		public DateTime DateLastModified
		{
			get
			{
				return this._fileInfo.LastWriteTime;
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000029E1 File Offset: 0x00000BE1
		public Task<string> ReadContentsAsync()
		{
			this.AssertFileExists();
			return Task.Factory.StartNew<string>(() => File.ReadAllText(this.FullPath));
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002A00 File Offset: 0x00000C00
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

		// Token: 0x0600004D RID: 77 RVA: 0x00002A44 File Offset: 0x00000C44
		public Task<bool> MoveAsync(string destinationDirectoryPath, CollisionOption option)
		{
			this.AssertFileExists();
			return Task.Factory.StartNew<bool>(delegate()
			{
				string text = Path.Combine(destinationDirectoryPath, this.Filename);
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

		// Token: 0x0600004E RID: 78 RVA: 0x00002A88 File Offset: 0x00000C88
		public Task<bool> CopyAsync(string destinationDirectoryPath, bool overwriteExisting)
		{
			this.AssertFileExists();
			return Task.Factory.StartNew<bool>(delegate()
			{
				string destFileName = Path.Combine(destinationDirectoryPath, this.Filename);
				this._fileInfo.CopyTo(destFileName, overwriteExisting);
				return true;
			});
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002ACC File Offset: 0x00000CCC
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

		// Token: 0x06000050 RID: 80 RVA: 0x00002B09 File Offset: 0x00000D09
		public Task<bool> DeleteAsync()
		{
			this.AssertFileExists();
			return Task.Factory.StartNew<bool>(delegate()
			{
				this._fileInfo.Delete();
				return true;
			});
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002B27 File Offset: 0x00000D27
		private void AssertFileExists()
		{
			if (this._fileInfo != null && !this._fileInfo.Exists)
			{
				throw new FileNotFoundException("The File does not exist!");
			}
		}

		// Token: 0x0400000D RID: 13
		private FileInfo _fileInfo;
	}
}
