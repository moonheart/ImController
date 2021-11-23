using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Utilities.Services.Wrappers.Storage
{
	// Token: 0x0200000E RID: 14
	public class WinDirectory : IDirectory
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002398 File Offset: 0x00000598
		public WinDirectory(DirectoryInfo dirInfo)
		{
			if (dirInfo == null)
			{
				throw new ArgumentException("Directory info was null, need the information");
			}
			this._directoryInfo = dirInfo;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000023B5 File Offset: 0x000005B5
		public string Name
		{
			get
			{
				return this._directoryInfo.Name;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000023C2 File Offset: 0x000005C2
		public string FullPath
		{
			get
			{
				return this._directoryInfo.FullName;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000023CF File Offset: 0x000005CF
		public IDirectory ParentDirectory
		{
			get
			{
				return new WinDirectory(this._directoryInfo.Parent);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000023E1 File Offset: 0x000005E1
		public bool Exists
		{
			get
			{
				return this._directoryInfo.Exists;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000023EE File Offset: 0x000005EE
		public long Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000023F6 File Offset: 0x000005F6
		public Task<IEnumerable<IFile>> GetFilesAsync()
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<IEnumerable<IFile>>(delegate()
			{
				IEnumerable<IFile> result = new List<IFile>();
				FileInfo[] files = this._directoryInfo.GetFiles();
				if (files != null && files.Any<FileInfo>())
				{
					result = files.Select((FileInfo file, int i) => new WinFile(file));
				}
				return result;
			});
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002414 File Offset: 0x00000614
		public Task<IFile> GetFileAsync(string pathToFile)
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<IFile>(delegate()
			{
				IFile result = null;
				if (!string.IsNullOrWhiteSpace(pathToFile))
				{
					string text = Path.Combine(this.FullPath, pathToFile);
					if (File.Exists(text))
					{
						result = new WinFile(new FileInfo(text));
					}
				}
				return result;
			});
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002451 File Offset: 0x00000651
		public Task<IEnumerable<IDirectory>> GetDirectoriesAsync()
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<IEnumerable<IDirectory>>(delegate()
			{
				IEnumerable<IDirectory> enumerable = new List<IDirectory>();
				DirectoryInfo[] directories = this._directoryInfo.GetDirectories();
				if (directories != null && directories.Any<DirectoryInfo>())
				{
					enumerable = directories.Select((DirectoryInfo directory, int i) => new WinDirectory(directory));
					enumerable.ToList<IDirectory>().ForEach(delegate(IDirectory directory)
					{
						directory.CalculateDirectorySize();
					});
				}
				return enumerable;
			});
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002470 File Offset: 0x00000670
		public Task<IDirectory> GetDirectoryAsync(string pathToDirectory)
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<IDirectory>(delegate()
			{
				IDirectory result = null;
				if (!string.IsNullOrWhiteSpace(pathToDirectory))
				{
					string path = Environment.ExpandEnvironmentVariables(Path.Combine(this.FullPath, pathToDirectory));
					if (Directory.Exists(path))
					{
						result = new WinDirectory(new DirectoryInfo(path));
					}
				}
				return result;
			});
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000024B0 File Offset: 0x000006B0
		public Task<IDirectory> CreateDirectoryAsync(string directoryPath, CreationOption collisionOption)
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<IDirectory>(delegate()
			{
				string path = Environment.ExpandEnvironmentVariables(directoryPath);
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
						return new WinDirectory(new DirectoryInfo(path));
					case CreationOption.ThrowIfExists:
						throw new Exceptions.DirectoryAlreadyExistsException("The directory to be created already exists!");
					}
					Directory.Delete(path, true);
					result = new WinDirectory(Directory.CreateDirectory(path));
				}
				else
				{
					result = new WinDirectory(Directory.CreateDirectory(path));
				}
				return result;
			});
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000024F4 File Offset: 0x000006F4
		public Task<IFile> CreateFileAsync(string fileName, string contents, CreationOption collisionOption)
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<IFile>(delegate()
			{
				string text = Path.Combine(this.FullPath, fileName);
				if (File.Exists(text))
				{
					switch (collisionOption)
					{
					case CreationOption.OpenIfExists:
						File.AppendAllText(text, contents);
						goto IL_82;
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
				IL_82:
				return new WinFile(new FileInfo(text));
			});
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002540 File Offset: 0x00000740
		public Task<bool> MoveAsync(string destinationDirectoryPath, CollisionOption collisionOption)
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<bool>(delegate()
			{
				string text = Environment.ExpandEnvironmentVariables(Path.Combine(destinationDirectoryPath, this.Name));
				bool result;
				if (Directory.Exists(text))
				{
					CollisionOption collisionOption2 = collisionOption;
					if (collisionOption2 != CollisionOption.ReplaceExisting && collisionOption2 == CollisionOption.ThrowExisting)
					{
						throw new Exceptions.DirectoryAlreadyExistsException("Directory already exists in the target directory");
					}
					Directory.Delete(text, true);
					Directory.Move(this.FullPath, text);
					this._directoryInfo = new DirectoryInfo(text);
					result = true;
				}
				else
				{
					Directory.Move(this.FullPath, text);
					this._directoryInfo = new DirectoryInfo(text);
					result = true;
				}
				return result;
			});
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002584 File Offset: 0x00000784
		public Task<bool> CopyAsync(string destinationPath, CollisionOption collisionOption)
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<bool>(() => this.CopyDirectory(this._directoryInfo.FullName, destinationPath, collisionOption));
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000025C8 File Offset: 0x000007C8
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

		// Token: 0x0600003B RID: 59 RVA: 0x00002784 File Offset: 0x00000984
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

		// Token: 0x0600003C RID: 60 RVA: 0x000027C1 File Offset: 0x000009C1
		public Task<bool> DeleteAsync()
		{
			this.AssertDirectoryExists();
			return Task.Factory.StartNew<bool>(delegate()
			{
				this._directoryInfo.Delete(true);
				return true;
			});
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000027E0 File Offset: 0x000009E0
		public void CalculateDirectorySize()
		{
			try
			{
				this._length = this._directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).Sum((FileInfo file) => file.Length);
			}
			catch (UnauthorizedAccessException ex)
			{
				Logger.Log(ex, "Exception in WinDirectory.CalculateDirectorySize");
				this._length = 0L;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002850 File Offset: 0x00000A50
		private void AssertDirectoryExists()
		{
			if (this._directoryInfo != null && !this._directoryInfo.Exists)
			{
				throw new FileNotFoundException("The Directory does not exist!");
			}
		}

		// Token: 0x0400000B RID: 11
		private DirectoryInfo _directoryInfo;

		// Token: 0x0400000C RID: 12
		private long _length;
	}
}
