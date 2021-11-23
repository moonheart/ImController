using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lenovo.Modern.Utilities.Services.Wrappers.Storage;

namespace Lenovo.Modern.Utilities.Services.SystemContext.Storage
{
	// Token: 0x02000029 RID: 41
	public class SystemContextDirectory : IDirectory
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x000056BC File Offset: 0x000038BC
		public SystemContextDirectory(DirectoryInfo dirInfo)
		{
			if (dirInfo == null)
			{
				throw new ArgumentException("Directory info was null, need the information");
			}
			this._directoryInfo = dirInfo;
			this._systemPathMapper = SystemPathMapper.Instance;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000056E4 File Offset: 0x000038E4
		public string Name
		{
			get
			{
				return this._directoryInfo.Name;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x000056F1 File Offset: 0x000038F1
		public string FullPath
		{
			get
			{
				return this._directoryInfo.FullName;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x000056FE File Offset: 0x000038FE
		public IDirectory ParentDirectory
		{
			get
			{
				return new SystemContextDirectory(this._directoryInfo.Parent);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00005710 File Offset: 0x00003910
		public bool Exists
		{
			get
			{
				return this._directoryInfo.Exists;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000EA RID: 234 RVA: 0x0000571D File Offset: 0x0000391D
		public long Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005725 File Offset: 0x00003925
		public Task<IEnumerable<IFile>> GetFilesAsync()
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<IEnumerable<IFile>>(delegate()
			{
				IEnumerable<IFile> result = new List<IFile>();
				FileInfo[] files = this._directoryInfo.GetFiles();
				if (files != null && files.Any<FileInfo>())
				{
					result = files.Select((FileInfo file, int i) => new SystemContextFile(file));
				}
				return result;
			});
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005744 File Offset: 0x00003944
		public Task<IFile> GetFileAsync(string pathToFile)
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<IFile>(delegate()
			{
				IFile result = null;
				if (!string.IsNullOrWhiteSpace(pathToFile))
				{
					string text = Path.Combine(this._systemPathMapper.GetUserContextFolder(this.FullPath), pathToFile);
					if (File.Exists(text))
					{
						result = new SystemContextFile(new FileInfo(text));
					}
				}
				return result;
			});
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005781 File Offset: 0x00003981
		public Task<IEnumerable<IDirectory>> GetDirectoriesAsync()
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<IEnumerable<IDirectory>>(delegate()
			{
				IEnumerable<IDirectory> enumerable = new List<IDirectory>();
				DirectoryInfo[] directories = this._directoryInfo.GetDirectories();
				if (directories != null && directories.Any<DirectoryInfo>())
				{
					enumerable = directories.Select((DirectoryInfo directory, int i) => new SystemContextDirectory(directory));
					enumerable.ToList<IDirectory>().ForEach(delegate(IDirectory directory)
					{
						directory.CalculateDirectorySize();
					});
				}
				return enumerable;
			});
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000057A0 File Offset: 0x000039A0
		public Task<IDirectory> GetDirectoryAsync(string pathToDirectory)
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<IDirectory>(delegate()
			{
				IDirectory result = null;
				if (!string.IsNullOrWhiteSpace(pathToDirectory))
				{
					string path = Path.Combine(this._systemPathMapper.GetUserContextFolder(this.FullPath), pathToDirectory);
					if (Directory.Exists(path))
					{
						result = new SystemContextDirectory(new DirectoryInfo(path));
					}
				}
				return result;
			});
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000057E0 File Offset: 0x000039E0
		public void CalculateDirectorySize()
		{
			try
			{
				this._length = this._directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).Sum((FileInfo file) => file.Length);
			}
			catch (UnauthorizedAccessException)
			{
				this._length = 0L;
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005848 File Offset: 0x00003A48
		public Task<IDirectory> CreateDirectoryAsync(string directoryPath, CreationOption collisionOption)
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<IDirectory>(delegate()
			{
				string path = this._systemPathMapper.GetUserContextFolder(directoryPath);
				if (!Path.IsPathRooted(path))
				{
					path = Path.Combine(this.FullPath, directoryPath);
				}
				IDirectory result;
				if (Directory.Exists(path))
				{
					switch (collisionOption)
					{
					case CreationOption.OpenIfExists:
						return new SystemContextDirectory(new DirectoryInfo(path));
					case CreationOption.ThrowIfExists:
						throw new Exceptions.DirectoryAlreadyExistsException("The directory to be created already exists!");
					}
					Directory.Delete(path, true);
					result = new SystemContextDirectory(Directory.CreateDirectory(path));
				}
				else
				{
					result = new SystemContextDirectory(Directory.CreateDirectory(path));
				}
				return result;
			});
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000588C File Offset: 0x00003A8C
		public Task<IFile> CreateFileAsync(string fileName, string contents, CreationOption collisionOption)
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<IFile>(delegate()
			{
				string text = Path.Combine(this._systemPathMapper.GetUserContextFolder(this.FullPath), fileName);
				if (File.Exists(text))
				{
					switch (collisionOption)
					{
					case CreationOption.OpenIfExists:
						File.AppendAllText(text, contents);
						goto IL_92;
					case CreationOption.ThrowIfExists:
						throw new Exceptions.FileAlreadyExistsException("The file to be created already exists in the target directory");
					}
					File.WriteAllText(text, contents);
				}
				else if (contents != null)
				{
					File.WriteAllText(text, contents);
				}
				else
				{
					File.Create(text).Dispose();
				}
				IL_92:
				return new SystemContextFile(new FileInfo(text));
			});
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000058D8 File Offset: 0x00003AD8
		public Task<bool> MoveAsync(string destinationDirectoryPath, CollisionOption collisionOption)
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<bool>(delegate()
			{
				CollisionOption collisionOption2 = collisionOption;
				if (collisionOption2 == CollisionOption.ReplaceExisting || collisionOption2 != CollisionOption.ThrowExisting)
				{
					string text = Path.Combine(this._systemPathMapper.GetUserContextFolder(destinationDirectoryPath), this.Name);
					if (Directory.Exists(text))
					{
						Directory.Delete(text);
					}
					Directory.Move(this.FullPath, text);
					this._directoryInfo = new DirectoryInfo(text);
					return true;
				}
				throw new Exceptions.DirectoryAlreadyExistsException("Directory already exists in the target directory");
			});
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000591C File Offset: 0x00003B1C
		public Task<bool> CopyAsync(string destinationPath, CollisionOption collisionOption)
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<bool>(delegate()
			{
				string userContextFolder = this._systemPathMapper.GetUserContextFolder(destinationPath);
				return this.CopyDirectory(this._directoryInfo.FullName, userContextFolder, collisionOption);
			});
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005960 File Offset: 0x00003B60
		private bool CopyDirectory(string source, string destination, CollisionOption collisionOption)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(source);
			if (!Directory.Exists(destination))
			{
				Directory.CreateDirectory(destination);
			}
			FileInfo[] files = directoryInfo.GetFiles();
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			bool result;
			if (collisionOption == CollisionOption.ReplaceExisting || collisionOption != CollisionOption.ThrowExisting)
			{
				foreach (FileInfo fileInfo in files)
				{
					FileInfo fileInfo2 = new FileInfo(Path.Combine(destination, fileInfo.Name));
					if (fileInfo2.Exists)
					{
						fileInfo2.Delete();
					}
					fileInfo.CopyTo(Path.Combine(destination, fileInfo.Name), true);
				}
				foreach (DirectoryInfo directoryInfo2 in directories)
				{
					DirectoryInfo directoryInfo3 = new DirectoryInfo(Path.Combine(destination, directoryInfo2.Name));
					if (directoryInfo3.Exists)
					{
						directoryInfo3.Delete(true);
					}
					this.CopyDirectory(directoryInfo2.FullName, directoryInfo3.FullName, collisionOption);
				}
				result = true;
			}
			else
			{
				foreach (FileInfo fileInfo3 in files)
				{
					FileInfo fileInfo4 = new FileInfo(Path.Combine(destination, fileInfo3.Name));
					if (fileInfo4.Exists)
					{
						throw new Exceptions.FileAlreadyExistsException(string.Format("File path {0} already exists.", fileInfo3.FullName));
					}
					fileInfo3.CopyTo(fileInfo4.FullName, true);
				}
				foreach (DirectoryInfo directoryInfo4 in directories)
				{
					DirectoryInfo directoryInfo5 = new DirectoryInfo(Path.Combine(destination, directoryInfo4.Name));
					if (directoryInfo5.Exists)
					{
						throw new Exceptions.FileAlreadyExistsException(string.Format("Directory path {0} already exists.", directoryInfo4.FullName));
					}
					this.CopyDirectory(directoryInfo4.FullName, directoryInfo5.FullName, collisionOption);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005B1C File Offset: 0x00003D1C
		public Task<bool> RenameAsync(string newName)
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<bool>(delegate()
			{
				bool result = false;
				string text = Path.Combine(this._directoryInfo.Parent.FullName, newName);
				if (!string.IsNullOrWhiteSpace(text))
				{
					Directory.Move(this.FullPath, text);
					this._directoryInfo = new DirectoryInfo(text);
					result = true;
				}
				return result;
			});
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005B59 File Offset: 0x00003D59
		public Task<bool> DeleteAsync()
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<bool>(delegate()
			{
				this._directoryInfo.Delete(true);
				return true;
			});
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005B77 File Offset: 0x00003D77
		private void AssertDirectoryExists()
		{
			if (this._directoryInfo != null && !this._directoryInfo.Exists)
			{
				throw new FileNotFoundException("The Directory does not exist!");
			}
		}

		// Token: 0x04000046 RID: 70
		private DirectoryInfo _directoryInfo;

		// Token: 0x04000047 RID: 71
		private ISystemPathMapper _systemPathMapper;

		// Token: 0x04000048 RID: 72
		private long _length;
	}
}
