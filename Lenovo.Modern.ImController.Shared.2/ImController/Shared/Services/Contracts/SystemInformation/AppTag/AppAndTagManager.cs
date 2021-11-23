using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.Utilities.Patterns.Ioc;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.SystemContext.Interop;
using Lenovo.Modern.Utilities.Services.Wrappers.Storage;

namespace Lenovo.Modern.ImController.Shared.Services.Contracts.SystemInformation.AppTag
{
	// Token: 0x02000024 RID: 36
	public class AppAndTagManager : IAppAndTagManager, IDataCleanup
	{
		// Token: 0x060000CF RID: 207 RVA: 0x00006CE5 File Offset: 0x00004EE5
		private AppAndTagManager()
		{
			this._fileSystem = new WinFileSystem();
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006D1A File Offset: 0x00004F1A
		public static IAppAndTagManager GetInstance()
		{
			IAppAndTagManager result;
			if ((result = AppAndTagManager._instance) == null)
			{
				result = (AppAndTagManager._instance = new AppAndTagManager());
			}
			return result;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003010 File Offset: 0x00001210
		public void CleanupData()
		{
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00006D30 File Offset: 0x00004F30
		public void UpdateCacheWithDelay(CancellationToken cancellationToken)
		{
			AppAndTagManager.<>c__DisplayClass5_0 CS$<>8__locals1 = new AppAndTagManager.<>c__DisplayClass5_0();
			CS$<>8__locals1.cancellationToken = cancellationToken;
			CS$<>8__locals1.<>4__this = this;
			if (Interlocked.Exchange(ref AppAndTagManager._cacheUpdateInProgress, 1) == 0)
			{
				Task.Run(delegate()
				{
					AppAndTagManager.<>c__DisplayClass5_0.<<UpdateCacheWithDelay>b__0>d <<UpdateCacheWithDelay>b__0>d;
					<<UpdateCacheWithDelay>b__0>d.<>4__this = CS$<>8__locals1;
					<<UpdateCacheWithDelay>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<UpdateCacheWithDelay>b__0>d.<>1__state = -1;
					AsyncTaskMethodBuilder <>t__builder = <<UpdateCacheWithDelay>b__0>d.<>t__builder;
					<>t__builder.Start<AppAndTagManager.<>c__DisplayClass5_0.<<UpdateCacheWithDelay>b__0>d>(ref <<UpdateCacheWithDelay>b__0>d);
					return <<UpdateCacheWithDelay>b__0>d.<>t__builder.Task;
				});
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00006D70 File Offset: 0x00004F70
		public async Task<AppAndTagCollection> GetAppAndTagsAsync(CancellationToken cancellationToken)
		{
			try
			{
				await this._getAppTagsRequestSempahore.WaitAsync();
				if (cancellationToken.IsCancellationRequested)
				{
					return null;
				}
				if (this._cachedAppTagCollection != null && !DateTime.MinValue.Equals(this._lastCacheUpdate))
				{
					TimeSpan timeSpan = DateTime.Now - this._lastCacheUpdate;
					if (timeSpan.TotalMinutes > (double)this._latestUpdateIntervalMinutes)
					{
						Logger.Log(Logger.LogSeverity.Information, "GetAppAndTagsAsync: Updating cache since last updation was {0} before which is more than {1} minutes ago", new object[] { timeSpan.TotalMinutes, this._latestUpdateIntervalMinutes });
						this.UpdateCacheWithDelay(cancellationToken);
					}
				}
				if (this._cachedAppTagCollection == null && !this._hasFileRequestFailed)
				{
					AppAndTagCollection appAndTagCollection = await this.TryGetFromFileAsync();
					if (appAndTagCollection != null)
					{
						this._cachedAppTagCollection = appAndTagCollection;
						this.UpdateCacheWithDelay(cancellationToken);
					}
					else
					{
						this._hasFileRequestFailed = true;
					}
				}
				if (this._cachedAppTagCollection == null && !this._hasPluginRequestFailed)
				{
					Logger.Log(Logger.LogSeverity.Information, "AppTagManager: Making Plugin request because local file not found");
					AppAndTagCollection appAndTagCollection2 = await this.TryGetFromPluginAsync(cancellationToken);
					if (appAndTagCollection2 != null)
					{
						this._lastCacheUpdate = DateTime.Now;
						this._cachedAppTagCollection = appAndTagCollection2;
					}
					else
					{
						this._hasPluginRequestFailed = true;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception in GetAppAndTagsAsync");
			}
			finally
			{
				this._getAppTagsRequestSempahore.Release();
			}
			return this._cachedAppTagCollection;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006DC0 File Offset: 0x00004FC0
		private async Task<AppAndTagCollection> TryGetFromPluginAsync(CancellationToken cancellationToken)
		{
			AppAndTagCollection appTagList = null;
			using (CancellationTokenSource thisRequestToken = new CancellationTokenSource(TimeSpan.FromSeconds(60.0)))
			{
				using (CancellationTokenSource mergedCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, thisRequestToken.Token))
				{
					try
					{
						ContractRequest contractRequest = new ContractRequest
						{
							Name = "SystemInformation.AppTag",
							Command = new ContractCommandRequest
							{
								Name = "Get-AppsAndTags",
								Parameter = string.Empty,
								RequestType = "sync"
							}
						};
						IPluginManager pluginManager = InstanceContainer.GetInstance().Resolve<IPluginManager>();
						if (pluginManager == null)
						{
							throw new InvalidOperationException("AppTagManager: IPluginManager instance not available");
						}
						Func<string, PluginRepository.PluginInformation> func = delegate(string pluginName)
						{
							try
							{
								PluginRepository.PluginInformation pluginInformation2 = PluginRepository.GetPluginInformation(pluginName);
								if (pluginInformation2 != null && File.Exists(pluginInformation2.PathToPlugin))
								{
									return pluginInformation2;
								}
							}
							catch (Exception ex2)
							{
								Logger.Log(ex2, "AppTagManager: Unable to locate plugin {0}", new object[] { pluginName });
							}
							return null;
						};
						PluginRepository.PluginInformation pluginInformation = func("GenericCorePlugin") ?? func("GenericAppTagProviderPlugin");
						if (pluginInformation == null)
						{
							Logger.Log(Logger.LogSeverity.Error, "AppTagManager: Unable to locate plugin in any location");
							return null;
						}
						PluginRequestInformation pluginRequestInformation = new PluginRequestInformation
						{
							ContractRequest = contractRequest,
							PluginName = pluginInformation.PluginName,
							TaskId = Guid.NewGuid().ToString(),
							RunAs = RunAs.User,
							RequestType = RequestType.Internal
						};
						if (Environment.GetCommandLineArgs().Length > 1)
						{
							IntPtr zero = IntPtr.Zero;
							if (!Authorization.GetSessionUserToken(ref zero))
							{
								bool isSystem;
								using (WindowsIdentity current = WindowsIdentity.GetCurrent())
								{
									isSystem = current.IsSystem;
								}
								if (isSystem)
								{
									pluginRequestInformation.RunAs = RunAs.System;
									Logger.Log(Logger.LogSeverity.Information, "AppTagManager: User is not logged In. Current process is System process. Plugin Runas={0}", new object[] { pluginRequestInformation.RunAs.ToString() });
								}
								else
								{
									Logger.Log(Logger.LogSeverity.Information, "AppTagManager: User is not logged In or current process is not System process. Plugin Runas={0}", new object[] { pluginRequestInformation.RunAs.ToString() });
								}
							}
							else
							{
								Logger.Log(Logger.LogSeverity.Information, "AppTagManager: User is logged In. Plugin Runas={0}", new object[] { pluginRequestInformation.RunAs.ToString() });
							}
						}
						Logger.Log(Logger.LogSeverity.Information, "AppTagManager: Making plugin request with runas={0}", new object[] { pluginRequestInformation.RunAs.ToString() });
						BrokerResponseTask brokerResponseTask = await pluginManager.MakePluginRequest(pluginRequestInformation, mergedCancellationToken.Token);
						if (brokerResponseTask != null && brokerResponseTask.ContractResponse != null && brokerResponseTask.ContractResponse.Response != null && !string.IsNullOrWhiteSpace(brokerResponseTask.ContractResponse.Response.Data))
						{
							appTagList = Serializer.Deserialize<AppAndTagCollection>(brokerResponseTask.ContractResponse.Response.Data);
							if (appTagList != null)
							{
								Logger.Log(Logger.LogSeverity.Information, "AppTagManager: Writing data to file");
								AppAndTagManager.SaveToFile(brokerResponseTask.ContractResponse.Response.Data);
							}
							else
							{
								Logger.Log(Logger.LogSeverity.Error, "AppTagManager: contract response returned data that cannot be serialize");
							}
						}
						else
						{
							Logger.Log(Logger.LogSeverity.Error, "AppTagManager: contract request returned empty data");
						}
					}
					catch (OperationCanceledException)
					{
						Logger.Log(Logger.LogSeverity.Error, "AppTagManager: contract request timed out or was canceled");
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "AppTagManager: Unhandled exception in AppsAndTags contract request");
					}
				}
				CancellationTokenSource mergedCancellationToken = null;
			}
			CancellationTokenSource thisRequestToken = null;
			return appTagList;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006E08 File Offset: 0x00005008
		private async Task<AppAndTagCollection> TryGetFromFileAsync()
		{
			AppAndTagCollection result = null;
			try
			{
				IDirectory directory = this._fileSystem.LoadDirectory("%programdata%\\Lenovo\\iMController\\shared");
				if (directory == null)
				{
					Logger.Log(Logger.LogSeverity.Warning, "AppTagManager: Expected directory does not exist. {0}", new object[] { "%programdata%\\Lenovo\\iMController\\shared" });
				}
				else
				{
					IFile file = await directory.GetFileAsync("AppsAndTags.xml");
					if (file == null)
					{
						Logger.Log(Logger.LogSeverity.Warning, "AppTagManager: Expected file does not exist. {0}", new object[] { "AppsAndTags.xml" });
					}
					else
					{
						result = Serializer.Deserialize<AppAndTagCollection>(await file.ReadContentsAsync());
					}
				}
			}
			catch (FileNotFoundException)
			{
				this._hasFileRequestFailed = true;
				Logger.Log(Logger.LogSeverity.Warning, "AppTagManager: Expected file does not exist {0}\\{1}", new object[] { "%programdata%\\Lenovo\\iMController\\shared", "AppsAndTags.xml" });
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "AppTagManager: Exception while retrieving apps and tags {0}\\{1}", new object[] { "%programdata%\\Lenovo\\iMController\\shared", "AppsAndTags.xml" });
			}
			return result;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00006E50 File Offset: 0x00005050
		private static bool SaveToFile(string fileContents)
		{
			bool result = false;
			try
			{
				if (!string.IsNullOrWhiteSpace(fileContents))
				{
					Directory.CreateDirectory(Path.Combine(new string[] { Environment.ExpandEnvironmentVariables("%programdata%\\Lenovo\\iMController\\shared") }));
					File.WriteAllText(Path.Combine(Environment.ExpandEnvironmentVariables("%programdata%\\Lenovo\\iMController\\shared"), "AppsAndTags.xml"), fileContents, Encoding.UTF8);
					result = true;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "AppTagManager: Exception while saving file");
			}
			return result;
		}

		// Token: 0x0400008F RID: 143
		private readonly IFileSystem _fileSystem;

		// Token: 0x04000090 RID: 144
		private static IAppAndTagManager _instance;

		// Token: 0x04000091 RID: 145
		private AppAndTagCollection _cachedAppTagCollection;

		// Token: 0x04000092 RID: 146
		private bool _hasFileRequestFailed;

		// Token: 0x04000093 RID: 147
		private bool _hasPluginRequestFailed;

		// Token: 0x04000094 RID: 148
		private readonly SemaphoreSlim _getAppTagsRequestSempahore = new SemaphoreSlim(1);

		// Token: 0x04000095 RID: 149
		private DateTime _lastCacheUpdate = DateTime.MinValue;

		// Token: 0x04000096 RID: 150
		private readonly int _latestUpdateIntervalMinutes = 1440;

		// Token: 0x04000097 RID: 151
		private static int _cacheUpdateInProgress;

		// Token: 0x0200007A RID: 122
		private static class MachineInfoConstants
		{
		}

		// Token: 0x0200007B RID: 123
		private static class AppTagManagerConstants
		{
			// Token: 0x040001F7 RID: 503
			public const string PathToImSharedDir = "%programdata%\\Lenovo\\iMController\\shared";

			// Token: 0x040001F8 RID: 504
			public const string AppsAndTagsFileName = "AppsAndTags.xml";

			// Token: 0x040001F9 RID: 505
			public const string OldPluginName = "GenericAppTagProviderPlugin";

			// Token: 0x040001FA RID: 506
			public const string CorePluginName = "GenericCorePlugin";
		}
	}
}
