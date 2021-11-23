using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.ImController.Shared.Telemetry;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Validation;
using Lenovo.Modern.Utilities.Services.Wrappers.Network;
using Lenovo.Modern.Utilities.Services.Wrappers.Storage;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x0200001C RID: 28
	public class SubscriptionManager : ISubscriptionManager, IDataCleanup
	{
		// Token: 0x0600008F RID: 143 RVA: 0x000055C8 File Offset: 0x000037C8
		private SubscriptionManager(INetworkAgent networkAgent)
		{
			if (networkAgent != null)
			{
				this._networkAgent = networkAgent;
			}
			IFileSystem fileSystem = new WinFileSystem();
			string path = Environment.ExpandEnvironmentVariables(Constants.RootFolder);
			this._directory = fileSystem.LoadDirectory(path);
			this._fileAccessMutex = new SemaphoreSlim(1, 1);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000563D File Offset: 0x0000383D
		private void SetCachedETag(string eTag)
		{
			this._cachedETag = eTag;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005646 File Offset: 0x00003846
		private string GetCachedETag()
		{
			return this._cachedETag;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005650 File Offset: 0x00003850
		public static ISubscriptionManager GetInstance(INetworkAgent networkAgent)
		{
			if (SubscriptionManager._instance == null)
			{
				object syncRoot = SubscriptionManager._syncRoot;
				lock (syncRoot)
				{
					if (SubscriptionManager._instance == null)
					{
						SubscriptionManager._instance = new SubscriptionManager(networkAgent);
					}
				}
			}
			return SubscriptionManager._instance;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000056A8 File Offset: 0x000038A8
		public async Task<PackageSubscription> GetSubscriptionAsync(CancellationToken cancelToken)
		{
			SubscriptionManager.<>c__DisplayClass15_0 CS$<>8__locals1 = new SubscriptionManager.<>c__DisplayClass15_0();
			CS$<>8__locals1.cancelToken = cancelToken;
			CS$<>8__locals1.<>4__this = this;
			await this._getSubscriptionSemaphore.WaitAsync();
			if (this._cachedSbscriptionFile == null)
			{
				try
				{
					PackageSubscription freshSubscription = null;
					string subscriptionFilePath = Path.Combine(Environment.ExpandEnvironmentVariables(Constants.ImControllerCoreDataFolder), Constants.SubscriptionFileName);
					try
					{
						Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager: Waiting for subscription file mutex");
						this._fileAccessMutex.Wait(CS$<>8__locals1.cancelToken);
						Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager: Entered subscription file mutex");
						TaskAwaiter<IFile> taskAwaiter = this._directory.GetFileAsync(subscriptionFilePath).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<IFile> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<IFile>);
						}
						if (taskAwaiter.GetResult() != null)
						{
							if (this.IsSubscriptionFileSignedByLenovo(subscriptionFilePath))
							{
								freshSubscription = this.CheckAndGetValidSubscriptionFile(subscriptionFilePath);
							}
							else
							{
								Logger.Log(Logger.LogSeverity.Critical, "SubscriptionManager: Existing Subscription file {0} is not signed", new object[] { subscriptionFilePath });
							}
						}
					}
					catch (OperationCanceledException ex)
					{
						Logger.Log(ex, "SubscriptionManager: OperationCanceledException in GetSubscriptionAsync");
					}
					catch (ObjectDisposedException ex2)
					{
						Logger.Log(ex2, "SubscriptionManager: ObjectDisposedException in GetSubscriptionAsync");
					}
					finally
					{
						this._fileAccessMutex.Release();
						Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager: Left subscription file mutex");
					}
					if (ImcPolicy.IsUpdateDisabledByPolicy())
					{
						Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager: Will not update subscription due to policy");
					}
					else if (freshSubscription == null)
					{
						freshSubscription = await this.GetLatestSubscriptionAsync(CS$<>8__locals1.cancelToken);
					}
					else
					{
						Task.Run(delegate()
						{
							SubscriptionManager.<>c__DisplayClass15_0.<<GetSubscriptionAsync>b__0>d <<GetSubscriptionAsync>b__0>d;
							<<GetSubscriptionAsync>b__0>d.<>4__this = CS$<>8__locals1;
							<<GetSubscriptionAsync>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<GetSubscriptionAsync>b__0>d.<>1__state = -1;
							AsyncTaskMethodBuilder <>t__builder = <<GetSubscriptionAsync>b__0>d.<>t__builder;
							<>t__builder.Start<SubscriptionManager.<>c__DisplayClass15_0.<<GetSubscriptionAsync>b__0>d>(ref <<GetSubscriptionAsync>b__0>d);
							return <<GetSubscriptionAsync>b__0>d.<>t__builder.Task;
						});
					}
					if (freshSubscription == null)
					{
						throw new Exception("Unable to locate a valid subscription file");
					}
					this._cachedSbscriptionFile = SubscriptionManager.FormatSubscription(freshSubscription);
					freshSubscription = null;
					subscriptionFilePath = null;
				}
				catch (Exception ex3)
				{
					Logger.Log(ex3, "SubscriptionManager: Exception thrown trying to get and save the Subscription File");
					this._cachedSbscriptionFile = null;
				}
			}
			this._getSubscriptionSemaphore.Release();
			return this._cachedSbscriptionFile;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000056F8 File Offset: 0x000038F8
		public async Task<PackageSubscription> GetLatestSubscriptionAsync(CancellationToken cancelToken)
		{
			PackageSubscription result2;
			if (this._networkAgent == null || this._directory == null)
			{
				result2 = null;
			}
			else if (ImcPolicy.IsUpdateDisabledByPolicy())
			{
				Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager: Will not update subscription due to policy");
				result2 = null;
			}
			else
			{
				if (!DateTime.MinValue.Equals(this._lastSubscriptionUpdate))
				{
					TimeSpan timeSpan = DateTime.Now - this._lastSubscriptionUpdate;
					if (timeSpan.TotalMinutes < (double)this._latestUpdateIntervalMinutes)
					{
						Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager: Skipping subscription file download since last time it was downloaded was {0} less than {1} minutes ago", new object[] { timeSpan.TotalMinutes, this._latestUpdateIntervalMinutes });
						return this._cachedLatestSubscription;
					}
				}
				PackageSubscription subscription = null;
				bool success = false;
				int maxRetries = 2;
				int retries = 0;
				EventFactory.Constants.UpdateResult result = EventFactory.Constants.UpdateResult.Fail;
				while (!success && !cancelToken.IsCancellationRequested && retries < maxRetries)
				{
					int num = retries;
					retries = num + 1;
					if (this._networkAgent.GetNetworkConnectivity() == NetworkConnectivity.InternetConnected)
					{
						try
						{
							IDirectory directory = await this.GetOrCreateSubscriptionTempDirectoryAsync();
							string tempSubscriptionFileTempPath = Path.Combine(directory.FullPath, Constants.SubscriptionFileName);
							Uri subscriptionDownloadLocation = SubscriptionManager.GetSubscriptionDownloadLocation();
							Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager: Trying to download subscription from {0} and save it to {1}", new object[] { subscriptionDownloadLocation, tempSubscriptionFileTempPath });
							try
							{
								Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager: Waiting for subscription file mutex");
								this._fileAccessMutex.Wait(cancelToken);
								Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager: Entered subscription file mutex");
								TaskAwaiter<bool> taskAwaiter = this.IsUpdateNecessaryAsync(subscriptionDownloadLocation).GetAwaiter();
								TaskAwaiter<bool> taskAwaiter2;
								if (!taskAwaiter.IsCompleted)
								{
									await taskAwaiter;
									taskAwaiter = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter<bool>);
								}
								if (taskAwaiter.GetResult())
								{
									taskAwaiter = this._networkAgent.DownloadToFileAsync(subscriptionDownloadLocation, tempSubscriptionFileTempPath, cancelToken).GetAwaiter();
									if (!taskAwaiter.IsCompleted)
									{
										await taskAwaiter;
										taskAwaiter = taskAwaiter2;
										taskAwaiter2 = default(TaskAwaiter<bool>);
									}
									if (taskAwaiter.GetResult())
									{
										Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager: New subscription file was downloaded");
										if (this.IsSubscriptionFileSignedByLenovo(tempSubscriptionFileTempPath))
										{
											subscription = this.CheckAndGetValidSubscriptionFile(tempSubscriptionFileTempPath);
											if (subscription != null)
											{
												IFile tempFile = await this._directory.GetFileAsync(tempSubscriptionFileTempPath);
												if (tempFile != null)
												{
													string subscriptionFileFolderLocation = Environment.ExpandEnvironmentVariables(Constants.ImControllerCoreDataFolder);
													await tempFile.CopyAsync(subscriptionFileFolderLocation, true);
													await tempFile.DeleteAsync();
													Path.Combine(subscriptionFileFolderLocation, Constants.SubscriptionFileName);
													this._lastSubscriptionUpdate = DateTime.Now;
													this._cachedLatestSubscription = subscription;
													this._cachedSbscriptionFile = SubscriptionManager.FormatSubscription(this._cachedLatestSubscription);
													success = true;
													result = EventFactory.Constants.UpdateResult.Success;
													Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager: Subscription file is valid and was replaced successfully");
													subscriptionFileFolderLocation = null;
												}
												tempFile = null;
											}
										}
										else
										{
											result = EventFactory.Constants.UpdateResult.Corrupt;
											Logger.Log(Logger.LogSeverity.Critical, "SubscriptionManager: Downloaded subscription file {0} is not signed", new object[] { tempSubscriptionFileTempPath });
										}
									}
								}
								else
								{
									success = true;
									this._lastSubscriptionUpdate = DateTime.Now;
									subscription = this._cachedLatestSubscription;
									Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager: Subscription update is not necessary");
								}
							}
							catch (OperationCanceledException)
							{
								Logger.Log(Logger.LogSeverity.Error, "SubscriptionManager: OperationCanceledException in GetLatestSubscriptionAsync");
							}
							catch (ObjectDisposedException)
							{
								Logger.Log(Logger.LogSeverity.Error, "SubscriptionManager: ObjectDisposedException in GetLatestSubscriptionAsync");
							}
							finally
							{
								if (!success)
								{
									this.SetCachedETag("");
								}
								this._fileAccessMutex.Release();
								Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager: Left subscription file mutex");
							}
							if (!success)
							{
								try
								{
									result = EventFactory.Constants.UpdateResult.Fail;
									await Task.Delay(1000, cancelToken);
								}
								catch (OperationCanceledException ex)
								{
									Logger.Log(ex, "OperationCanceledException in GetLatestSubscriptionAsync");
								}
							}
							ImcEvent userEvent = EventFactory.CreateImcUpdateEvent(EventFactory.Constants.UpdateType.Subscription, EventFactory.Constants.UpdateAction.Download, result, "");
							EventLogger.GetInstance().LogEvent(userEvent);
							tempSubscriptionFileTempPath = null;
							subscriptionDownloadLocation = null;
							continue;
						}
						catch (UnauthorizedAccessException)
						{
							Logger.Log(Logger.LogSeverity.Error, "SubscriptionManager: Unable to access subscription file, {ex.Message}");
							continue;
						}
						catch (Exception ex2)
						{
							Logger.Log(ex2, "SubscriptionManager: Exception in GetLatestSubscriptionAsync");
							continue;
						}
					}
					await Task.Delay(1000, cancelToken);
				}
				result2 = subscription;
			}
			return result2;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005748 File Offset: 0x00003948
		private async Task<bool> IsUpdateNecessaryAsync(Uri uri)
		{
			bool result = false;
			try
			{
				string text = (await new NetworkAgent().GetHttpResponseHeadersAsync(uri)).FirstOrDefault((KeyValuePair<string, IEnumerable<string>> h) => h.Key.Equals("etag", StringComparison.OrdinalIgnoreCase)).Value.FirstOrDefault<string>();
				if (!string.IsNullOrWhiteSpace(text))
				{
					if (this.GetCachedETag().Equals(text, StringComparison.OrdinalIgnoreCase))
					{
						Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager : Online Subscription will not download because file not changed ");
						result = false;
					}
					else
					{
						this.SetCachedETag(text);
						result = true;
					}
				}
				else
				{
					result = true;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "SubscriptionManager : Exception in IsUpdateNecessaryAsync");
				result = true;
			}
			if (this._cachedSbscriptionFile == null)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005798 File Offset: 0x00003998
		public async Task<bool> UpdateSubscriptionFile(string newSubscriptionFilePath)
		{
			bool isUpdateSuccess = false;
			try
			{
				if (!Utility.SanitizePath(ref newSubscriptionFilePath))
				{
					Logger.Log(Logger.LogSeverity.Error, "UpdateSubscriptionFile: Failed to update subscription file as file path is invalid. Path - {0}", new object[] { newSubscriptionFilePath });
					return isUpdateSuccess;
				}
				bool isUpdateRequired = false;
				if (this.IsSubscriptionFileSignedByLenovo(newSubscriptionFilePath))
				{
					PackageSubscription subscription = this.CheckAndGetValidSubscriptionFile(newSubscriptionFilePath);
					if (subscription != null)
					{
						PackageSubscription packageSubscription = await this.GetSubscriptionAsync(default(CancellationToken));
						if (packageSubscription != null)
						{
							if (DateTime.Parse(packageSubscription.Datecreated) < DateTime.Parse(subscription.Datecreated))
							{
								isUpdateRequired = true;
							}
						}
						else
						{
							isUpdateRequired = true;
						}
						if (isUpdateRequired)
						{
							FileInfo fileInfo = new FileInfo(newSubscriptionFilePath);
							if (fileInfo != null)
							{
								fileInfo.CopyTo(Path.Combine(Environment.ExpandEnvironmentVariables(Constants.ImControllerCoreDataFolder), Constants.SubscriptionFileName), true);
								isUpdateSuccess = true;
								Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager: UpdateSubscriptionFile: Subscription file is valid and was replaced successfully");
							}
						}
						else
						{
							Logger.Log(Logger.LogSeverity.Information, "SubscriptionManager: UpdateSubscriptionFile: Not updated since dateCreated is less than existing subcription file");
						}
					}
					subscription = null;
				}
				else
				{
					Logger.Log(Logger.LogSeverity.Critical, "SubscriptionManager: UpdateSubscriptionFile: New subscription file {0} is not signed", new object[] { newSubscriptionFilePath });
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "SubscriptionManager: UpdateSubscriptionFile: Exception occured");
			}
			return isUpdateSuccess;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003010 File Offset: 0x00001210
		public void CleanupData()
		{
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000057E8 File Offset: 0x000039E8
		private static PackageSubscription FormatSubscription(PackageSubscription subscription)
		{
			if (subscription != null && subscription.PackageList != null && subscription.PackageList.Count<Package>() != 0)
			{
				foreach (Package package in subscription.PackageList)
				{
					string name = package.PackageInformation.Name;
					if (package.SubscribedEventList != null && package.SubscribedEventList.Count<SubscribedEvent>() != 0)
					{
						SubscribedEvent[] subscribedEventList = package.SubscribedEventList;
						for (int i = 0; i < subscribedEventList.Length; i++)
						{
							subscribedEventList[i].Plugin = name;
						}
					}
					if (package.ContractMappingList != null && package.ContractMappingList.Count<ContractMapping>() != 0)
					{
						ContractMapping[] array = package.ContractMappingList.ToArray<ContractMapping>();
						for (int i = 0; i < array.Length; i++)
						{
							array[i].Plugin = name;
						}
					}
				}
			}
			return subscription;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000058E0 File Offset: 0x00003AE0
		private async Task<IDirectory> GetOrCreateSubscriptionTempDirectoryAsync()
		{
			string directoryPath = Environment.ExpandEnvironmentVariables(Constants.SubscriptionFileTempLocation);
			return await this._directory.CreateDirectoryAsync(directoryPath, CreationOption.OpenIfExists);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005928 File Offset: 0x00003B28
		private bool IsSubscriptionFileSignedByLenovo(string subscriptionXmlPath)
		{
			if (Constants.IsSecurityDisabled)
			{
				Logger.Log(Logger.LogSeverity.Critical, "SubscriptionManager: Warning: Security has been disabled for checking subscription file");
				return true;
			}
			if (!Utility.SanitizePath(ref subscriptionXmlPath))
			{
				Logger.Log(Logger.LogSeverity.Error, "IsSubscriptionFileSignedByLenovo: Failed to check subscription signature as file path is invalid. Path - {0}", new object[] { subscriptionXmlPath });
				return false;
			}
			if (new ImcCertificateValidator().IsXmlValid(subscriptionXmlPath))
			{
				return true;
			}
			throw new InvalidOperationException("Subscription file is not signed by Lenovo");
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00005984 File Offset: 0x00003B84
		private PackageSubscription CheckAndGetValidSubscriptionFile(string subscriptionFilePath)
		{
			PackageSubscription packageSubscription = null;
			if (!Utility.SanitizePath(ref subscriptionFilePath))
			{
				Logger.Log(Logger.LogSeverity.Error, "CheckAndGetValidSubscriptionFile: Failed to check and get valid subscription file as file path is invalid. Path - {0}", new object[] { subscriptionFilePath });
				return packageSubscription;
			}
			packageSubscription = Serializer.Deserialize<PackageSubscription>(File.ReadAllText(subscriptionFilePath));
			if (packageSubscription == null || packageSubscription.PackageList == null || !packageSubscription.PackageList.Any<Package>())
			{
				throw new SubscriptionManagerException("Subscription xml file is not matching with PackageSubscription schema definition");
			}
			try
			{
				packageSubscription = ImcPolicy.RemoveITAdminDisabledPluginSubscriptionAsync(packageSubscription);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "SubscriptionManager: Unable to remove disabled plugins");
			}
			return packageSubscription;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005A0C File Offset: 0x00003C0C
		private static Uri GetSubscriptionDownloadLocation()
		{
			Version version = new Version(Constants.ImControllerVersion);
			return CacheBuster.MakeUnique(new Uri(string.Format(Constants.SubscriptionFileUrlVersionedFormat, version.Major, version.Minor)));
		}

		// Token: 0x0400006A RID: 106
		private PackageSubscription _cachedSbscriptionFile;

		// Token: 0x0400006B RID: 107
		private readonly INetworkAgent _networkAgent;

		// Token: 0x0400006C RID: 108
		private readonly IDirectory _directory;

		// Token: 0x0400006D RID: 109
		private readonly int _latestUpdateIntervalMinutes = 1440;

		// Token: 0x0400006E RID: 110
		private PackageSubscription _cachedLatestSubscription;

		// Token: 0x0400006F RID: 111
		private DateTime _lastSubscriptionUpdate = DateTime.MinValue;

		// Token: 0x04000070 RID: 112
		private readonly SemaphoreSlim _fileAccessMutex;

		// Token: 0x04000071 RID: 113
		private string _cachedETag = "";

		// Token: 0x04000072 RID: 114
		private static SubscriptionManager _instance = null;

		// Token: 0x04000073 RID: 115
		private static readonly object _syncRoot = new object();

		// Token: 0x04000074 RID: 116
		private readonly SemaphoreSlim _getSubscriptionSemaphore = new SemaphoreSlim(1);
	}
}
