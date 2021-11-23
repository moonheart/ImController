using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Contracts.FileSystem;
using Lenovo.Modern.Utilities.Services.Wrappers.Storage;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.FileSystem
{
	// Token: 0x02000018 RID: 24
	public class PerformActionOnItemsAgent : FileLoad
	{
		// Token: 0x060000CD RID: 205 RVA: 0x000079B5 File Offset: 0x00005BB5
		public override IDirectory FilePath(string path)
		{
			return FileSystemAgent.LoadFileSystem.LoadFileSystemBasedOnPrivilege(path);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000079C0 File Offset: 0x00005BC0
		public string ExpandEnvironmentVariables(string path)
		{
			string result;
			try
			{
				result = Environment.ExpandEnvironmentVariables(path);
			}
			catch (Exception item)
			{
				this.loopExceptions.Add(item);
				result = path;
			}
			return result;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000079F8 File Offset: 0x00005BF8
		public async Task<ItemActionResponse> PerformActionOnItems(ItemActionRequest requestXml)
		{
			ItemActionResponse itemActionResponse = null;
			if (requestXml != null)
			{
				List<Lenovo.Modern.CoreTypes.Contracts.FileSystem.Action> keyList = requestXml.ItemActionList.ToList<Lenovo.Modern.CoreTypes.Contracts.FileSystem.Action>();
				PermissionWhiteListManager _whiteListManager = new PermissionWhiteListManager();
				IEnumerable<string> enumerable = await _whiteListManager.PermissionSourceWhiteListManagerAsync();
				IEnumerable<string> sourceWhiteList = enumerable;
				enumerable = await _whiteListManager.PermissionDestinationWhiteListManagerAsync();
				IEnumerable<string> destinationWhiteList = enumerable;
				for (int counter = 0; counter < keyList.Count; counter++)
				{
					string sourcePath = this.ExpandEnvironmentVariables(keyList[counter].Source);
					string destinationPath = this.ExpandEnvironmentVariables(keyList[counter].Destination);
					string type = keyList[counter].Type;
					bool actionResult = false;
					bool flag = false;
					try
					{
						if ((File.GetAttributes(sourcePath) & FileAttributes.Directory) == FileAttributes.Directory)
						{
							if (this.FilePath(sourcePath).Exists)
							{
								flag = true;
							}
						}
						else
						{
							string directoryName = Path.GetDirectoryName(sourcePath);
							if (!string.IsNullOrEmpty(directoryName) && this.FilePath(directoryName).Exists)
							{
								flag = true;
							}
						}
					}
					catch (Exception item)
					{
						this.loopExceptions.Add(item);
						flag = false;
					}
					if (flag && sourceWhiteList.Any((string f) => sourcePath.IndexOf(Environment.ExpandEnvironmentVariables(f), StringComparison.OrdinalIgnoreCase) >= 0) && destinationWhiteList.Any((string f) => destinationPath.IndexOf(Environment.ExpandEnvironmentVariables(f), StringComparison.OrdinalIgnoreCase) >= 0) && string.Compare(type, ContractConstants.Get.ActionTypeCopy, StringComparison.OrdinalIgnoreCase) == 0)
					{
						try
						{
							actionResult = await this.CopyAsync(sourcePath, destinationPath);
						}
						catch (Exception item2)
						{
							this.loopExceptions.Add(item2);
						}
					}
					keyList[counter].Result = actionResult;
				}
				itemActionResponse = new ItemActionResponse();
				itemActionResponse.ItemActionList = requestXml.ItemActionList;
				keyList = null;
				_whiteListManager = null;
				sourceWhiteList = null;
				destinationWhiteList = null;
			}
			return itemActionResponse;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00007A45 File Offset: 0x00005C45
		public List<Exception> ExceptionList()
		{
			return this.loopExceptions;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00007A50 File Offset: 0x00005C50
		private async Task<bool> CopyAsync(string sourcePath, string destinationDirectoryPath)
		{
			bool flag = false;
			string fileName = Path.GetFileName(sourcePath);
			bool result;
			if (string.IsNullOrEmpty(sourcePath))
			{
				result = flag;
			}
			else
			{
				string directoryName = Path.GetDirectoryName(sourcePath);
				if (string.IsNullOrEmpty(directoryName))
				{
					result = flag;
				}
				else
				{
					IDirectory directory = this.FilePath(directoryName);
					if (!this.FilePath(destinationDirectoryPath).Exists)
					{
						await directory.CreateDirectoryAsync(destinationDirectoryPath, CreationOption.ReplaceExisting);
					}
					if ((File.GetAttributes(sourcePath) & FileAttributes.Directory) == FileAttributes.Directory)
					{
						directory = this.FilePath(sourcePath);
						flag = await directory.CopyAsync(Path.Combine(destinationDirectoryPath, fileName), CollisionOption.ReplaceExisting);
					}
					else
					{
						flag = await(await directory.GetFileAsync(fileName)).CopyAsync(destinationDirectoryPath, true);
					}
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x04000049 RID: 73
		private List<Exception> loopExceptions = new List<Exception>();
	}
}
