using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Contracts.ImController;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.ImClient.Services;
using Lenovo.Modern.ImController.ImClient.Services.Umdf;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Patterns.Ioc;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Microsoft.Win32;

namespace Lenovo.Modern.ImController.ContractBroker.Services
{
	// Token: 0x02000013 RID: 19
	public class ContractBroker
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x00003CA4 File Offset: 0x00001EA4
		public ContractBroker(IRequestMapper requestMapper, IPluginManager pluginManager, IPluginRepository pluginRepository, ImcContractHandler imcContractHandler, ISubscriptionManager subscirptionMgr)
		{
			this._requestMapper = requestMapper;
			this._pluginManager = pluginManager;
			this._pluginRepository = pluginRepository;
			this._imcContractHandler = imcContractHandler;
			this.RequestEvent = null;
			this.UpdateInProgressEvent = null;
			this._subscirptionMgr = subscirptionMgr;
			IBrokerResponseAgent brokerResponseAgent = InstanceContainer.GetInstance().Resolve<IBrokerResponseAgent>();
			this._requestTaskQueue = brokerResponseAgent.GetRequestQueue();
			this._responseTaskQueue = brokerResponseAgent.GetResponseQueue();
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003D0D File Offset: 0x00001F0D
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00003D15 File Offset: 0x00001F15
		public ManualResetEventSlim RequestEvent { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00003D1E File Offset: 0x00001F1E
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00003D26 File Offset: 0x00001F26
		public ManualResetEventSlim UpdateInProgressEvent { get; set; }

		// Token: 0x060000A9 RID: 169 RVA: 0x00003D30 File Offset: 0x00001F30
		public async Task<bool> ResumeAsync(CancellationToken cancelToken)
		{
			if (this._machineInfo == null)
			{
				MachineInformation machineInfo = await MachineInformationManager.GetInstance().GetMachineInformationAsync(cancelToken);
				this._machineInfo = machineInfo;
			}
			if (this._machineInfo != null)
			{
				this._requestHandlerThread = new Thread(delegate()
				{
					this.HandleRequestsThread(cancelToken);
				});
				this._requestHandlerThread.Start();
				Logger.Log(Logger.LogSeverity.Information, "ContractBroker.HandleRequestsThread started");
			}
			else
			{
				Logger.Log(Logger.LogSeverity.Error, "ContractBroker unable to get machine info.  Will not start");
			}
			if (this._pluginManager != null)
			{
				PackageSubscription packageSubscription = await this._subscirptionMgr.GetSubscriptionAsync(cancelToken);
				this._pluginManager.SetPackageSubscription(packageSubscription);
			}
			return true;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003D80 File Offset: 0x00001F80
		private void SendErrorResponse(Guid taskId, string msg)
		{
			BrokerResponse response = BrokerResponseFactory.CreateErrorBrokerResponse("BrokerResponseAgent", 306, msg);
			ResponseTask item = new ResponseTask
			{
				TaskId = taskId,
				Response = response
			};
			this._responseTaskQueue.Add(item);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003DC0 File Offset: 0x00001FC0
		private void SendNormalResponse(Guid taskId, BrokerResponse response)
		{
			ResponseTask item = new ResponseTask
			{
				TaskId = taskId,
				Response = response
			};
			this._responseTaskQueue.Add(item);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003DF0 File Offset: 0x00001FF0
		private void IntermediateResponseHandler(Guid taskId, BrokerResponse response)
		{
			response.Task.IsComplete = false;
			ResponseTask item = new ResponseTask
			{
				TaskId = taskId,
				Response = response
			};
			this._responseTaskQueue.Add(item);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003E2C File Offset: 0x0000202C
		private void HandleRequestsThread(CancellationToken cancelToken)
		{
			try
			{
				while (!cancelToken.IsCancellationRequested)
				{
					try
					{
						RequestTask requestTask = this._requestTaskQueue.Take(cancelToken);
						if (!cancelToken.IsCancellationRequested)
						{
							if (requestTask != null)
							{
								Guid taskId = requestTask.TaskId;
								if (string.IsNullOrWhiteSpace(requestTask.Request))
								{
									this.SendErrorResponse(requestTask.TaskId, "Empty request");
								}
								else
								{
									Logger.Log(Logger.LogSeverity.Information, "ContractBroker: Task {0}. Received Task {0} ,request ({1} characters):\r\n{2}", new object[]
									{
										requestTask.TaskId,
										requestTask.Request.Length,
										requestTask.Request
									});
									try
									{
										BrokerRequest brokerRequest = Serializer.Deserialize<BrokerRequest>(requestTask.Request);
										this.ExecuteRequest(requestTask.TaskId, brokerRequest, cancelToken);
									}
									catch (Exception ex)
									{
										Logger.Log(ex, "ContractBroker: Task {0} exception", new object[] { requestTask.TaskId });
										this.SendErrorResponse(requestTask.TaskId, "Exception in Contract Broker");
									}
								}
							}
							else
							{
								Logger.Log(Logger.LogSeverity.Error, "ContractBroker: Null request or task ID");
							}
						}
					}
					catch (DeviceDriverMissingException)
					{
						Logger.Log(Logger.LogSeverity.Error, "ContractBroker.StartWaitingForRequestsAsync: Can't open UMDF driver");
					}
				}
			}
			catch (OperationCanceledException)
			{
				Logger.Log(Logger.LogSeverity.Information, "ContractBroker: HandleRequestsThread was stopped");
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "Exception in ContractBroker.HandleRequestsThread");
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003FBC File Offset: 0x000021BC
		private bool ExecuteRequest(Guid taskId, BrokerRequest brokerRequest, CancellationToken cancelToken)
		{
			ContractBroker.<>c__DisplayClass23_0 CS$<>8__locals1 = new ContractBroker.<>c__DisplayClass23_0();
			CS$<>8__locals1.taskId = taskId;
			CS$<>8__locals1.brokerRequest = brokerRequest;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.cancelToken = cancelToken;
			Task.Factory.StartNew<Task>(delegate()
			{
				ContractBroker.<>c__DisplayClass23_0.<<ExecuteRequest>b__0>d <<ExecuteRequest>b__0>d;
				<<ExecuteRequest>b__0>d.<>4__this = CS$<>8__locals1;
				<<ExecuteRequest>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<ExecuteRequest>b__0>d.<>1__state = -1;
				AsyncTaskMethodBuilder <>t__builder = <<ExecuteRequest>b__0>d.<>t__builder;
				<>t__builder.Start<ContractBroker.<>c__DisplayClass23_0.<<ExecuteRequest>b__0>d>(ref <<ExecuteRequest>b__0>d);
				return <<ExecuteRequest>b__0>d.<>t__builder.Task;
			}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
			return true;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004010 File Offset: 0x00002210
		private async Task InitiateRequestAsync(Guid taskId, BrokerRequest brokerRequest, CancellationToken cancelToken)
		{
			bool isPluginRequestNecessary = true;
			PluginRequestInformation pluginInfo = null;
			if (brokerRequest == null || brokerRequest.ContractRequest == null || brokerRequest.ContractRequest.Command == null)
			{
				Logger.Log(Logger.LogSeverity.Error, "ContractBroker: Task {0}. Bad parameters in InitiateRequestAsync", new object[] { taskId });
				throw new Exception(string.Format("ContractBroker: Task {0}. Received request for task {0} but request has empty values", taskId));
			}
			Logger.Log(Logger.LogSeverity.Information, "ContractBroker: Task {0}. InitiateRequestAsync entered with broker request with contract {1}", new object[]
			{
				taskId,
				brokerRequest.ContractRequest.Name
			});
			Lenovo.Modern.CoreTypes.Contracts.ImController.ContractConstants get = Lenovo.Modern.CoreTypes.Contracts.ImController.ContractConstants.Get;
			if (brokerRequest.ContractRequest.Command.RequestType == RequestType.Cancel.ToString())
			{
				pluginInfo = new PluginRequestInformation
				{
					ContractRequest = brokerRequest.ContractRequest,
					PluginName = "CancelContractBrokerRequest",
					PluginLocation = "CancelContractBrokerRequest",
					RequestType = RequestType.Cancel,
					TaskId = brokerRequest.ContractRequest.Command.Parameter
				};
				if (this._imcContractHandler.CancelImcContractRequest(new Guid(pluginInfo.TaskId)))
				{
					isPluginRequestNecessary = false;
				}
			}
			else if (!string.IsNullOrWhiteSpace(brokerRequest.ContractRequest.Name) && brokerRequest.ContractRequest.Name.Equals(get.ContractName, StringComparison.InvariantCultureIgnoreCase))
			{
				if (this.RequestEvent != null)
				{
					this.RequestEvent.Set();
				}
				isPluginRequestNecessary = false;
				BrokerResponse response = await this._imcContractHandler.HandleRequestAsync(taskId, brokerRequest, cancelToken, this.UpdateInProgressEvent, new Action<Guid, BrokerResponse>(this.IntermediateResponseHandler));
				this.SendNormalResponse(taskId, response);
			}
			else if (this.UpdateInProgressEvent != null && this.UpdateInProgressEvent.IsSet && brokerRequest.ContractRequest.Command.RequestType != "sync")
			{
				Logger.Log(Logger.LogSeverity.Warning, "ContractBroker: Task {0}. Returning error response since we are in service mode", new object[] { taskId });
				isPluginRequestNecessary = false;
				BrokerResponse serviceModeErrorMessage = BrokerResponseFactory.GetServiceModeErrorMessage(taskId.ToString());
				this.SendNormalResponse(taskId, serviceModeErrorMessage);
			}
			else
			{
				Tuple<PackageInformation, ContractMapping> tuple = null;
				bool useSpecificPlugin = false;
				if (string.IsNullOrWhiteSpace(brokerRequest.ContractRequest.PluginName))
				{
					tuple = await this._requestMapper.GetPluginMappingAsync(brokerRequest.ContractRequest, cancelToken);
					Logger.Log(Logger.LogSeverity.Information, "ContractBroker: Task {0}. Package mapped for request: {1}", new object[]
					{
						taskId,
						tuple.Item2.Plugin
					});
				}
				else
				{
					useSpecificPlugin = true;
					Logger.Log(Logger.LogSeverity.Information, "ContractBroker: Task {0}. PluginName was specified for request: {1}", new object[]
					{
						taskId,
						brokerRequest.ContractRequest.PluginName
					});
				}
				pluginInfo = new PluginRequestInformation
				{
					ContractRequest = brokerRequest.ContractRequest,
					PluginName = (useSpecificPlugin ? brokerRequest.ContractRequest.PluginName : tuple.Item2.Plugin),
					TaskId = taskId.ToString(),
					RunAs = (useSpecificPlugin ? RunAs.User : ((string.Compare(tuple.Item2.Runas, RunAs.System.ToString(), StringComparison.InvariantCultureIgnoreCase) == 0) ? RunAs.System : RunAs.User)),
					PluginType = (useSpecificPlugin ? PluginType.ManagedLibrary : tuple.Item1.MemoryManagementType),
					RequestType = RequestType.Application
				};
				Logger.Log(Logger.LogSeverity.Information, "ContractBroker: Task {0}. PluginRequestInformation created", new object[] { taskId });
			}
			if (isPluginRequestNecessary)
			{
				ContractBroker.LogPluginRequest(pluginInfo);
				Logger.Log(Logger.LogSeverity.Information, "ContractBroker: Task {0}. Making plugin request for plugin {1}", new object[] { taskId, pluginInfo.PluginName });
				long startTicks = DateTime.Now.Ticks;
				await this._pluginManager.MakePluginRequest(pluginInfo, cancelToken);
				ContractBroker.LogPluginResult(pluginInfo, startTicks, DateTime.Now.Ticks);
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004070 File Offset: 0x00002270
		private static void LogPluginRequest(PluginRequestInformation requestInfo)
		{
			try
			{
				if (ContractBroker.IsLoggingEnabled == null)
				{
					RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Lenovo\\Modern\\Logs");
					if (registryKey != null)
					{
						object value = registryKey.GetValue("ImController.Service");
						ContractBroker.IsLoggingEnabled = new bool?(value != null);
						bool? isLoggingEnabled = ContractBroker.IsLoggingEnabled;
						bool flag = true;
						if ((isLoggingEnabled.GetValueOrDefault() == flag) & (isLoggingEnabled != null))
						{
							Registry.LocalMachine.DeleteSubKeyTree("Software\\Lenovo\\ImController\\Logs", false);
						}
					}
					else
					{
						ContractBroker.IsLoggingEnabled = new bool?(false);
					}
				}
				if (ContractBroker.IsLoggingEnabled != null && ContractBroker.IsLoggingEnabled.Value && requestInfo != null && requestInfo.ContractRequest != null && requestInfo.ContractRequest.Command != null && !string.IsNullOrWhiteSpace(requestInfo.ContractRequest.Command.Name))
				{
					RegistryKey registryKey2 = Registry.LocalMachine.CreateSubKey("Software\\Lenovo\\ImController\\Logs\\Requests\\Plugins");
					if (registryKey2 != null)
					{
						int num = Convert.ToInt32(registryKey2.GetValue(requestInfo.PluginName, 0));
						registryKey2.SetValue(requestInfo.PluginName, num + 1, RegistryValueKind.DWord);
					}
					RegistryKey registryKey3 = Registry.LocalMachine.CreateSubKey("Software\\Lenovo\\ImController\\Logs\\Requests\\Commands\\");
					if (registryKey3 != null)
					{
						string text = requestInfo.ContractRequest.Name + ":" + requestInfo.ContractRequest.Command.Name;
						int num2 = Convert.ToInt32(registryKey3.GetValue(text, 0));
						registryKey3.SetValue(text, num2 + 1, RegistryValueKind.DWord);
						RegistryKey registryKey4 = registryKey3.CreateSubKey(text);
						if (registryKey4 != null)
						{
							registryKey4.SetValue(requestInfo.TaskId + ".Start", DateTime.Now.TimeOfDay.TotalSeconds);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "trying to log plugin request");
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004268 File Offset: 0x00002468
		private static void LogPluginResult(PluginRequestInformation requestInfo, long startTicks, long stopTicks)
		{
			try
			{
				if (ContractBroker.IsLoggingEnabled != null && ContractBroker.IsLoggingEnabled.Value)
				{
					RegistryKey registryKey = Registry.LocalMachine.CreateSubKey("Software\\Lenovo\\ImController\\Logs\\Requests\\Commands\\");
					if (registryKey != null)
					{
						string subkey = requestInfo.ContractRequest.Name + ":" + requestInfo.ContractRequest.Command.Name;
						RegistryKey registryKey2 = registryKey.CreateSubKey(subkey);
						if (registryKey2 != null)
						{
							registryKey2.SetValue(requestInfo.TaskId + ".Stop", DateTime.Now.TimeOfDay.TotalSeconds);
							registryKey2.SetValue(requestInfo.TaskId + ".Diff", TimeSpan.FromTicks(stopTicks - startTicks).TotalSeconds);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "trying to log plugin result");
			}
		}

		// Token: 0x04000051 RID: 81
		private readonly IPluginManager _pluginManager;

		// Token: 0x04000052 RID: 82
		private readonly IRequestMapper _requestMapper;

		// Token: 0x04000053 RID: 83
		private readonly IPluginRepository _pluginRepository;

		// Token: 0x04000054 RID: 84
		private readonly ImcContractHandler _imcContractHandler;

		// Token: 0x04000055 RID: 85
		private readonly ISubscriptionManager _subscirptionMgr;

		// Token: 0x04000056 RID: 86
		private MachineInformation _machineInfo;

		// Token: 0x04000057 RID: 87
		private Thread _requestHandlerThread;

		// Token: 0x04000058 RID: 88
		private BlockingCollection<RequestTask> _requestTaskQueue;

		// Token: 0x04000059 RID: 89
		private BlockingCollection<ResponseTask> _responseTaskQueue;

		// Token: 0x0400005C RID: 92
		private static bool? IsLoggingEnabled;

		// Token: 0x0200002B RID: 43
		private enum ContractBrokerError
		{
			// Token: 0x040000C2 RID: 194
			contractBrokerError = 101,
			// Token: 0x040000C3 RID: 195
			unknownContractBrokerError
		}
	}
}
