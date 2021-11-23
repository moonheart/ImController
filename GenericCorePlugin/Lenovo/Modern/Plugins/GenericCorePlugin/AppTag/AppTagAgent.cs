using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Output;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.LegacyApp;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.ModernApp;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.Tags;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Storage;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag
{
	// Token: 0x0200001D RID: 29
	public class AppTagAgent : FileLoad
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x00007CAD File Offset: 0x00005EAD
		public static AppTagAgent GetInstance()
		{
			AppTagAgent result;
			if ((result = AppTagAgent._instance) == null)
			{
				result = (AppTagAgent._instance = new AppTagAgent());
			}
			return result;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00007CC3 File Offset: 0x00005EC3
		private AppTagAgent()
		{
			AppTagAgent._winFileSystem = new WinFileSystem();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00007CE1 File Offset: 0x00005EE1
		public override IDirectory FilePath(string path)
		{
			return AppTagAgent.LoadFileSystem.LoadFileSystemBasedOnPrivilege(path);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00007CEC File Offset: 0x00005EEC
		public async Task<AppAndTagCollection> GetAppsAndTags(bool ignorePluginCache = false, bool noApps = false, bool noTags = false)
		{
			try
			{
				await this._getAppsAndTagsSemaphore.WaitAsync();
				if (ignorePluginCache || this._cachedAppTagCollection == null)
				{
					Logger.Log(Logger.LogSeverity.Information, "AppTagAgent : Retriving fresh copy of apps and tags. ignorePluginCache: {0}", new object[] { ignorePluginCache });
					PackageManagerAgent modernAgent = new PackageManagerAgent();
					AppAndTagCollection appAndTagCollection = await new OutputCollector(new StartMenuAgent(), modernAgent, new TagManager()).CollectOutputAsync(noApps, noTags);
					if (appAndTagCollection != null)
					{
						Logger.Log(Logger.LogSeverity.Information, "AppTagAgent : Storing fresh copy of apps and tags from response.");
						this._cachedAppTagCollection = appAndTagCollection;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to retrieve apps and tags");
			}
			finally
			{
				this._getAppsAndTagsSemaphore.Release();
			}
			return this._cachedAppTagCollection;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00007D4C File Offset: 0x00005F4C
		public async Task<OutputLocationResponse> WriteAppsAndTags(OutputLocationRequest requestXml)
		{
			OutputLocationResponse response = null;
			CreateListForAppTask task = new CreateListForAppTask();
			response = new OutputLocationResponse();
			try
			{
				if (requestXml != null)
				{
					object[] location = requestXml.Location;
					foreach (string text in location)
					{
						DirectoryInfo item = new DirectoryInfo(text.ToString());
						this._directory = AppTagAgent.LoadFileSystem.LoadFileSystemBasedOnPrivilege(text.ToString());
						if (this._directory.Exists)
						{
							bool success = await task.GenerateAndSaveAsync(new List<DirectoryInfo> { item });
							response.Success = success;
						}
					}
					object[] array = null;
					response.Location = location;
					location = null;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, string.Format("Exception while handling the Apps and Tags Output Location Response contract request.\r\n{0}", requestXml));
			}
			return response;
		}

		// Token: 0x04000053 RID: 83
		private static AppTagAgent _instance;

		// Token: 0x04000054 RID: 84
		private readonly SemaphoreSlim _getAppsAndTagsSemaphore = new SemaphoreSlim(1);

		// Token: 0x04000055 RID: 85
		private AppAndTagCollection _cachedAppTagCollection;

		// Token: 0x04000056 RID: 86
		private IDirectory _directory;

		// Token: 0x04000057 RID: 87
		private static IFileSystem _winFileSystem;

		// Token: 0x02000075 RID: 117
		public static class LoadFileSystem
		{
			// Token: 0x060001E6 RID: 486 RVA: 0x0000DED2 File Offset: 0x0000C0D2
			public static IDirectory LoadFileSystemBasedOnPrivilege(string location)
			{
				return AppTagAgent._winFileSystem.LoadDirectory(location);
			}
		}
	}
}
