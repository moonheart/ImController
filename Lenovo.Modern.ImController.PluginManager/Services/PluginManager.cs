using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.Subscription;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.ImClient.Services;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Patterns.Ioc;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.PluginManager.Services
{
	// Token: 0x02000005 RID: 5
	public class PluginManager : IPluginManager, IDataCleanup
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00003014 File Offset: 0x00001214
		private PluginManager()
		{
			this._pluginToWrapperMap = new ConcurrentDictionary<string, PluginHostWrapper>();
			this._requestToWrapperMap = new ConcurrentDictionary<string, PluginHostWrapper>();
			this._pluginResponseTimeMap = new ConcurrentDictionary<string, DateTime>();
			this._phLifeControlMutex = new Semaphore(1, 1);
			this._stopEvent = new ManualResetEventSlim(false);
			IBrokerResponseAgent brokerResponseAgent = InstanceContainer.GetInstance().Resolve<IBrokerResponseAgent>();
			this._responseTaskQueue = brokerResponseAgent.GetResponseQueue();
			new Thread(new ThreadStart(this.PluginLifeEnderThread)).Start();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000308E File Offset: 0x0000128E
		public void CleanupData()
		{
			PluginManager._pluginHostsStopped = false;
			this._pluginToWrapperMap.Clear();
			this._requestToWrapperMap.Clear();
			this._pluginResponseTimeMap.Clear();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000030B7 File Offset: 0x000012B7
		public static PluginManager GetInstance()
		{
			PluginManager result;
			if ((result = PluginManager._instance) == null)
			{
				result = (PluginManager._instance = new PluginManager());
			}
			return result;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000030CD File Offset: 0x000012CD
		public void SetPackageSubscription(PackageSubscription ps)
		{
			this._subscription = ps;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000030D6 File Offset: 0x000012D6
		public void Stop(bool systemSuspending, bool systemShuttingdown)
		{
			Logger.Log(Logger.LogSeverity.Information, "PluginManager: Received request to stop");
			if (systemShuttingdown)
			{
				this._stopEvent.Set();
			}
			this.StopPluginHosts(systemSuspending);
			Logger.Log(Logger.LogSeverity.Information, "PluginManager: Stopped");
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003104 File Offset: 0x00001304
		private void PluginLifeEnderThread()
		{
			try
			{
				while (!this._stopEvent.Wait(30000))
				{
					try
					{
						if (this._pluginResponseTimeMap.Count != 0)
						{
							this._phLifeControlMutex.WaitOne();
							using (IEnumerator<KeyValuePair<string, DateTime>> enumerator = this._pluginResponseTimeMap.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									KeyValuePair<string, DateTime> timer = enumerator.Current;
									if (timer.Value <= DateTime.Now)
									{
										PluginHostWrapper hostWrapper = null;
										if (this._pluginToWrapperMap.TryRemove(timer.Key, out hostWrapper))
										{
											Task.Run(delegate()
											{
												try
												{
													Logger.Log(Logger.LogSeverity.Information, "PluginManager: Plugin: {0}. Ending life for plugin host hosting plugin {0}", new object[] { timer.Key });
													hostWrapper.StopPluginHost();
													if (!hostWrapper.WaitForPluginHostToStop(0))
													{
														hostWrapper.Kill();
													}
												}
												catch (Exception ex3)
												{
													Logger.Log(ex3, "PluginManager: Plugin: {0}. Exception in hostWrapper.WaitForPluginHostToStop()", new object[] { timer.Key });
												}
											});
										}
										else
										{
											Logger.Log(Logger.LogSeverity.Error, "PluginManager: Plugin: {0}. PluginLifeEnderThread: Plugin Host was not found in the map for plugin {0}", new object[] { timer.Key });
										}
										DateTime dateTime;
										this._pluginResponseTimeMap.TryRemove(timer.Key, out dateTime);
									}
								}
							}
							this._phLifeControlMutex.Release();
						}
						this.CheckForStoppedPluginHosts();
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "PluginManager: Exception in PluginLifeEnderThread inside while loop");
					}
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "PluginManager: This is an expected exception in PluginLifeEnder Thread: Thread is probably exiting");
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000032A4 File Offset: 0x000014A4
		private void CheckForStoppedPluginHosts()
		{
			try
			{
				if (!PluginManager._pluginHostsStopped)
				{
					this._phLifeControlMutex.WaitOne();
					if (!PluginManager._pluginHostsStopped)
					{
						using (IEnumerator<KeyValuePair<string, PluginHostWrapper>> enumerator = this._pluginToWrapperMap.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, PluginHostWrapper> host = enumerator.Current;
								if (host.Value.IsPluginHostStopped())
								{
									Logger.Log(Logger.LogSeverity.Information, "PluginManager.CheckForStoppedPluginHosts: PluginHost for [PLUGIN: {0}] was stopped", new object[] { host.Value.PluginName });
									Task.Run(delegate()
									{
										try
										{
											host.Value.StopPluginHost();
											host.Value.WaitForPluginHostToStop(PluginManager._serviceSuspendTimeout);
										}
										catch (Exception ex2)
										{
											Logger.Log(ex2, "PluginManager.CheckForStoppedPluginHosts: Exception occured");
										}
									});
									PluginHostWrapper pluginHostWrapper;
									this._pluginToWrapperMap.TryRemove(host.Key, out pluginHostWrapper);
								}
							}
						}
					}
					this._phLifeControlMutex.Release();
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PluginManager.CheckForStoppedPluginHosts: Exception");
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000339C File Offset: 0x0000159C
		public async Task<BrokerResponseTask> MakePluginRequest(PluginRequestInformation pluginRequestInformation, CancellationToken cancelToken)
		{
			BrokerResponseTask response = null;
			PluginHostWrapper pluginHostWrapper = null;
			if (PluginManager._pluginHostsStopped)
			{
				this._phLifeControlMutex.WaitOne();
				if (PluginManager._pluginHostsStopped)
				{
					response = Serializer.Deserialize<BrokerResponseTask>("<BrokerResponseTask isComplete=\"false\" percentageComplete=\"0\"><FailureData><ResultCodeGroup>Plugin Host</ResultCodeGroup><ResultCode>606</ResultCode><ResultDescription>Error processing request: Plugin Host is not started. Please try again in a minute.</ResultDescription></FailureData></BrokerResponseTask>");
				}
				this._phLifeControlMutex.Release();
			}
			if (response == null)
			{
				if (pluginRequestInformation.RequestType == RequestType.Cancel)
				{
					string taskIdToCancel = pluginRequestInformation.ContractRequest.Command.Parameter;
					Logger.Log(Logger.LogSeverity.Information, "PluginManager: Plugin: {0}. Task {1} was requested to cancel", new object[] { pluginRequestInformation.PluginName, taskIdToCancel });
					bool flag = false;
					for (int i = 0; i < 3; i++)
					{
						if (this._requestToWrapperMap.TryGetValue(taskIdToCancel.ToString(), out pluginHostWrapper))
						{
							flag = true;
							break;
						}
						Task.Delay(1000).Wait();
					}
					if (!flag)
					{
						Logger.Log(Logger.LogSeverity.Information, "PluginManager: Plugin: {0}. Task {1} was not found", new object[] { pluginRequestInformation.PluginName, taskIdToCancel });
						throw new PluginManagerException
						{
							TaskId = pluginRequestInformation.TaskId
						};
					}
					if (!pluginHostWrapper.IsPluginHostStopped())
					{
						Logger.Log(Logger.LogSeverity.Information, "PluginManager: Plugin: {0}. Task {1} was found and will be canceled", new object[] { pluginRequestInformation.PluginName, taskIdToCancel });
						await pluginHostWrapper.MakePluginRequest(pluginRequestInformation, cancelToken);
					}
					else
					{
						Logger.Log(Logger.LogSeverity.Information, "PluginManager: Plugin: {0}. Task {1} was found but pluginhost was already closed. Do nothing.", new object[] { pluginRequestInformation.PluginName, taskIdToCancel });
					}
					this._requestToWrapperMap.TryRemove(taskIdToCancel, out pluginHostWrapper);
					taskIdToCancel = null;
				}
				else
				{
					PluginRepository.PluginInformation pluginPathWithPluginName = new PluginRepository().GetPluginPathWithPluginName(pluginRequestInformation.PluginName);
					if (pluginPathWithPluginName != null)
					{
						pluginRequestInformation.PluginLocation = pluginPathWithPluginName.PathToPlugin;
						pluginRequestInformation.Bitness = pluginPathWithPluginName.Bitness;
						this._phLifeControlMutex.WaitOne();
						try
						{
							bool flag2 = this._pluginToWrapperMap.TryGetValue(pluginRequestInformation.PluginName, out pluginHostWrapper);
							if (flag2 && pluginHostWrapper.IsPluginHostStopped())
							{
								Logger.Log(Logger.LogSeverity.Information, "PluginManager: Old pluginHost for {0} was found but it was stopped. Removing it and starting new one", new object[] { pluginRequestInformation.PluginName });
								this._pluginToWrapperMap.TryRemove(pluginRequestInformation.PluginName, out pluginHostWrapper);
								flag2 = false;
							}
							if (!flag2)
							{
								Lenovo.Modern.ImController.Shared.Model.Packages.Package package = null;
								bool deviceAssociation = true;
								string associatedUapId = "";
								try
								{
									package = this._subscription.PackageList.FirstOrDefault((Lenovo.Modern.ImController.Shared.Model.Packages.Package p) => p != null && p.PackageInformation != null && !string.IsNullOrWhiteSpace(p.PackageInformation.Name) && p.PackageInformation.Name.Equals(pluginRequestInformation.PluginName, StringComparison.InvariantCultureIgnoreCase));
								}
								catch (Exception ex)
								{
									Logger.Log(ex, "MakePluginrequest: Exception occured trying to get package");
								}
								if (package != null)
								{
									deviceAssociation = PackageSettingsAgent.DoesPackageHaveDeviceAssociation(package);
									Dictionary<string, string> packageUapAssociationIds = PackageSettingsAgent.GetPackageUapAssociationIds(package);
									if (packageUapAssociationIds.Count > 0)
									{
										associatedUapId = packageUapAssociationIds.ElementAt(0).Key;
									}
								}
								pluginHostWrapper = new PluginHostWrapper(pluginRequestInformation.PluginName, pluginPathWithPluginName.Version.ToString(), pluginRequestInformation.RunAs, pluginRequestInformation.Bitness, new Func<Guid, BrokerResponse, bool>(this.HandleAndForwardResponse), deviceAssociation, associatedUapId);
								this._pluginToWrapperMap.TryAdd(pluginRequestInformation.PluginName, pluginHostWrapper);
								bool flag3 = false;
								try
								{
									flag3 = pluginHostWrapper.StartPluginHost(cancelToken);
								}
								catch (Exception ex2)
								{
									Logger.Log(ex2, "PluginManager: Plugin: {0}. Exception when looking for Plugin Host Wrapper or starting new one", new object[] { pluginRequestInformation.PluginName });
								}
								if (!flag3)
								{
									Logger.Log(Logger.LogSeverity.Information, "PluginManager: Plugin: {0}. Something went wrong while launching pluginhost", new object[] { pluginRequestInformation.PluginName });
									this._pluginToWrapperMap.TryRemove(pluginRequestInformation.PluginName, out pluginHostWrapper);
									pluginHostWrapper.StopPluginHost();
								}
								else
								{
									Logger.Log(Logger.LogSeverity.Information, "PluginManager: Plugin: {0}. New Plugin Host Wrapper was started for plugin {0}", new object[] { pluginRequestInformation.PluginName });
								}
							}
							else
							{
								Logger.Log(Logger.LogSeverity.Information, "PluginManager: Plugin: {0}. Existing Plugin Host Wrapper found for plugin {0}", new object[] { pluginRequestInformation.PluginName });
							}
							this.SetTimeoutTimer(pluginRequestInformation.PluginName, pluginRequestInformation.RequestType, 0);
						}
						catch (Exception ex3)
						{
							Logger.Log(ex3, "PluginManager: Plugin: {0}. Exception when looking for Plugin Host Wrapper or starting new one", new object[] { pluginRequestInformation.PluginName });
						}
						this._phLifeControlMutex.Release();
						this._requestToWrapperMap.TryAdd(pluginRequestInformation.TaskId, pluginHostWrapper);
						response = await pluginHostWrapper.MakePluginRequest(pluginRequestInformation, cancelToken);
						PluginHostWrapper pluginHostWrapper2;
						this._requestToWrapperMap.TryRemove(pluginRequestInformation.TaskId, out pluginHostWrapper2);
					}
				}
			}
			return response;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000033F4 File Offset: 0x000015F4
		private int GetLifeTimeFromRequestType(RequestType rqType)
		{
			int result;
			switch (rqType)
			{
			case RequestType.Application:
			case RequestType.Internal:
				result = PluginManager._pluginUnloadTimeoutRQMS;
				break;
			case RequestType.Event:
				result = PluginManager._pluginUnloadTimeoutEVMS;
				break;
			case RequestType.Cancel:
				result = 0;
				break;
			default:
				result = 0;
				break;
			}
			return result;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003434 File Offset: 0x00001634
		private void SetTimeoutTimer(string pluginName, RequestType rqType, int overrideTimeout = 0)
		{
			int lifeTimeFromRequestType = this.GetLifeTimeFromRequestType(rqType);
			DateTime dateTime = DateTime.Now + TimeSpan.FromMilliseconds((double)lifeTimeFromRequestType);
			bool flag = (from p in new PluginPrivilegeReader(this._subscription).GetPluginPrivileges(pluginName)
				select p.Equals(PluginPrivilege.ImmediateEventNotification)).Count<bool>() != 0;
			if (overrideTimeout != 0 && flag)
			{
				try
				{
					dateTime = DateTime.Now + TimeSpan.FromMinutes((double)overrideTimeout);
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "Exception occured in SetTimeoutTimer.");
				}
			}
			if (!this._pluginResponseTimeMap.TryAdd(pluginName, dateTime) && (this._pluginResponseTimeMap[pluginName] < dateTime || overrideTimeout != 0))
			{
				this._pluginResponseTimeMap[pluginName] = dateTime;
			}
			try
			{
				Logger.Log(Logger.LogSeverity.Information, string.Format("PluginManager: Plugin: {0}. Updated plugin unload timer for {0}. overrideTimeout = {1}. New unload time = {2}", pluginName, overrideTimeout, this._pluginResponseTimeMap[pluginName].ToString()));
			}
			catch (Exception)
			{
				Logger.Log(Logger.LogSeverity.Information, string.Format("PluginManager: Plugin: {0}. Updated plugin unload timer for {0}. overrideTimeout = {1}. New unload time cant be logged", pluginName, overrideTimeout));
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003558 File Offset: 0x00001758
		private bool IsSkipSuspendTerminationPrivilegeEnabled(string pluginName)
		{
			bool result = false;
			PluginSettingsAgent pluginSettingsAgent = new PluginSettingsAgent(this._subscription);
			if (pluginSettingsAgent != null)
			{
				PluginSettingsAgent.Setting prioritizedSetting = pluginSettingsAgent.GetPrioritizedSetting(pluginName, "ImController.Privilege.SkipSuspendTermination", PluginSettingsAgent.SettingLocation.SetByManifest);
				if (prioritizedSetting != null && !string.IsNullOrWhiteSpace(prioritizedSetting.ValueAsString))
				{
					int? valueAsInt = prioritizedSetting.ValueAsInt;
					int num = 1;
					result = (valueAsInt.GetValueOrDefault() == num) & (valueAsInt != null);
				}
			}
			return result;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000035B4 File Offset: 0x000017B4
		private List<string> GetPluginsToSkipSuspendTermination()
		{
			List<string> result = new List<string>();
			try
			{
				result = (from p in this._subscription.PackageList.Where(delegate(Lenovo.Modern.ImController.Shared.Model.Packages.Package x)
					{
						if (x != null && x.PackageInformation != null && !string.IsNullOrWhiteSpace(x.PackageInformation.Name) && x.SettingList != null)
						{
							return x.SettingList.FirstOrDefault((AppSetting s) => !string.IsNullOrWhiteSpace(s.Key) && s.Key.Equals("ImController.Privilege.SkipSuspendTermination") && s.Value.Equals("1")) != null;
						}
						return false;
					})
					select p.PackageInformation.Name).ToList<string>();
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception occured in GetPluginsToSkipSuspendTermination");
			}
			return result;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003640 File Offset: 0x00001840
		private bool DiscardResponse(Guid taskId, BrokerResponse brokerResponse)
		{
			Logger.Log(Logger.LogSeverity.Information, "PluginManager: Task: {0}. Response is discarded", new object[] { taskId });
			return true;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003660 File Offset: 0x00001860
		private bool HandleAndForwardResponse(Guid taskId, BrokerResponse brokerResponse)
		{
			bool flag = false;
			this._phLifeControlMutex.WaitOne();
			PluginHostWrapper pluginHostWrapper = null;
			if (this._requestToWrapperMap.TryGetValue(taskId.ToString(), out pluginHostWrapper))
			{
				if (brokerResponse.Task != null)
				{
					if (brokerResponse.Task.ContractResponse != null)
					{
						this.SetTimeoutTimer(pluginHostWrapper.PluginName, RequestType.Application, brokerResponse.Task.KeepAliveMinutes);
					}
					else if (brokerResponse.Task.EventResponse != null)
					{
						this.SetTimeoutTimer(pluginHostWrapper.PluginName, RequestType.Event, brokerResponse.Task.KeepAliveMinutes);
					}
				}
			}
			else
			{
				flag = true;
			}
			this._phLifeControlMutex.Release();
			if (!flag)
			{
				if (brokerResponse.Task != null && brokerResponse.Task.EventResponse == null)
				{
					if (brokerResponse.Task.ContractResponse == null)
					{
						string text = string.Empty;
						if (brokerResponse.Task.Error != null)
						{
							text = string.Format("Code: {0}, Description: {1}", brokerResponse.Task.Error.ResultCode, brokerResponse.Task.Error.ResultDescription);
						}
						Logger.Log(Logger.LogSeverity.Error, "PluginManager: Task: {0}. Response for task {0} had missing ContractResponse.  Error description: {1}", new object[] { taskId, text });
						brokerResponse.Task.ContractResponse = new ContractResponse
						{
							Response = new ResponseData
							{
								DataType = "ImControllerGenerated",
								Data = string.Empty
							}
						};
					}
					this._responseTaskQueue.Add(new ResponseTask
					{
						TaskId = taskId,
						Response = brokerResponse
					});
					Logger.Log(Logger.LogSeverity.Information, "PluginManager: Task: {0}. Response added to response queue for task {0}", new object[] { taskId });
				}
				else
				{
					Logger.Log(Logger.LogSeverity.Error, "PluginManager: Task: {0}. Response for task {0} was missing the Task information", new object[] { taskId });
				}
			}
			else
			{
				Logger.Log(Logger.LogSeverity.Information, "PluginManager: Task: {0}. Plugin was unloaded before it had a chance to respond", new object[] { taskId });
				Serializer.Deserialize<BrokerResponseTask>("<BrokerResponseTask isComplete=\"false\" percentageComplete=\"0\"><FailureData><ResultCodeGroup>Plugin Host</ResultCodeGroup><ResultCode>606</ResultCode><ResultDescription>Error processing request: Plugin was unloaded because it did not respond for too long.</ResultDescription></FailureData></BrokerResponseTask>");
				BrokerResponse response = BrokerResponseFactory.CreateErrorBrokerResponse("PluginHost", 606, "Error processing request: Plugin was unloaded because it did not respond for too long.");
				this._responseTaskQueue.Add(new ResponseTask
				{
					TaskId = taskId,
					Response = response
				});
			}
			return true;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003870 File Offset: 0x00001A70
		public void StopPluginHosts(bool systemSuspending)
		{
			List<string> list = new List<string>();
			if (systemSuspending)
			{
				list = this.GetPluginsToSkipSuspendTermination();
			}
			if (!PluginManager._pluginHostsStopped)
			{
				this._phLifeControlMutex.WaitOne();
				if (!PluginManager._pluginHostsStopped)
				{
					Logger.Log(Logger.LogSeverity.Information, "PluginManager: Stopping Plugin Hosts");
					foreach (KeyValuePair<string, PluginHostWrapper> keyValuePair in this._pluginToWrapperMap)
					{
						if (systemSuspending && list.Contains(keyValuePair.Value.PluginName))
						{
							Logger.Log(Logger.LogSeverity.Information, "PluginManager: Not Stopping PluginHost for [PLUGIN: {0}] due to skip privileges", new object[] { keyValuePair.Value.PluginName });
						}
						else
						{
							keyValuePair.Value.StopPluginHost();
						}
					}
					List<Task> list2 = new List<Task>();
					using (IEnumerator<KeyValuePair<string, PluginHostWrapper>> enumerator = this._pluginToWrapperMap.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, PluginHostWrapper> host = enumerator.Current;
							if (!systemSuspending || !list.Contains(host.Value.PluginName))
							{
								PluginHostWrapper removedHost = null;
								Task item = Task.Factory.StartNew(delegate()
								{
									if (!host.Value.WaitForPluginHostToStop(PluginManager._serviceSuspendTimeout))
									{
										host.Value.Kill();
									}
									this._pluginToWrapperMap.TryRemove(host.Key, out removedHost);
								}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
								list2.Add(item);
							}
						}
					}
					Task.WaitAll(list2.ToArray());
				}
				if (!systemSuspending)
				{
					PluginHostWrapper.KillZombie();
					PluginManager._pluginHostsStopped = true;
				}
				this._phLifeControlMutex.Release();
			}
		}

		// Token: 0x04000019 RID: 25
		private ConcurrentDictionary<string, PluginHostWrapper> _pluginToWrapperMap;

		// Token: 0x0400001A RID: 26
		private ConcurrentDictionary<string, PluginHostWrapper> _requestToWrapperMap;

		// Token: 0x0400001B RID: 27
		private ConcurrentDictionary<string, DateTime> _pluginResponseTimeMap;

		// Token: 0x0400001C RID: 28
		private Semaphore _phLifeControlMutex;

		// Token: 0x0400001D RID: 29
		private ManualResetEventSlim _stopEvent;

		// Token: 0x0400001E RID: 30
		private static bool _pluginHostsStopped = false;

		// Token: 0x0400001F RID: 31
		private static int _pluginUnloadTimeoutRQMS = 300000;

		// Token: 0x04000020 RID: 32
		private static int _pluginUnloadTimeoutEVMS = 60000;

		// Token: 0x04000021 RID: 33
		private static int _serviceSuspendTimeout = 25;

		// Token: 0x04000022 RID: 34
		private static PluginManager _instance;

		// Token: 0x04000023 RID: 35
		private BlockingCollection<ResponseTask> _responseTaskQueue;

		// Token: 0x04000024 RID: 36
		private PackageSubscription _subscription;
	}
}
