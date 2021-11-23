using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Contracts.ImController;
using Lenovo.Modern.CoreTypes.Utilities;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Storage;
using Microsoft.Win32;
using NETWORKLIST;
using UDC.ClientBrokerAgent;

namespace Lenovo.Modern.ImController.ContractBroker.Services
{
	// Token: 0x0200000E RID: 14
	public class ImcContractHandler
	{
		// Token: 0x06000076 RID: 118 RVA: 0x000025A0 File Offset: 0x000007A0
		public ImcContractHandler(PluginRepository pluginRepository, ISubscriptionManager subscriptionManager)
		{
			this._pluginRepository = pluginRepository;
			this._subscriptionManager = subscriptionManager;
			IFileSystem fileSystem = new WinFileSystem();
			this._directory = fileSystem.LoadDirectory(Environment.ExpandEnvironmentVariables(Constants.ImControllerCoreDataFolder));
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002600 File Offset: 0x00000800
		public bool CancelImcContractRequest(Guid taskId)
		{
			Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "ImcContractHandler.CancelImcContractRequest: Entry for taskId {0}", new object[] { taskId.ToString() });
			bool result = false;
			if (this._requestCancelEventDictionary.ContainsKey(taskId))
			{
				result = true;
				try
				{
					CancellationTokenSource cancellationTokenSource = this._requestCancelEventDictionary[taskId];
					Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "ImcContractHandler.CancelImcContractRequest: Cancelling IMC request {0}", new object[] { taskId.ToString() });
					cancellationTokenSource.Cancel();
				}
				catch (Exception ex)
				{
					Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex, "ImcContractHandler.CancelImcContractRequest: Exception while cancelling request for {0}", new object[] { taskId.ToString() });
				}
			}
			return result;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000026AC File Offset: 0x000008AC
		public async Task<BrokerResponse> HandleRequestAsync(Guid taskId, BrokerRequest brokerRequest, CancellationToken cancelToken, ManualResetEventSlim updateInProgressEvent, Action<Guid, BrokerResponse> intermediateResponseHandler)
		{
			BrokerResponse brokerResponse = null;
			if (brokerRequest != null && brokerRequest.ContractRequest != null && brokerRequest.ContractRequest.Command != null && !string.IsNullOrWhiteSpace(brokerRequest.ContractRequest.Command.Name))
			{
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
				this._requestCancelEventDictionary.TryAdd(taskId, cancellationTokenSource);
				ContractConstants contractConstants = ContractConstants.Get;
				using (CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancelToken, cancellationTokenSource.Token))
				{
					if (brokerRequest.ContractRequest.Command.Name.Equals(contractConstants.CommandNameGetStatus, StringComparison.InvariantCultureIgnoreCase))
					{
						if (!VantageUtility.IsVantageLaunchedAfterOOBERegistered())
						{
							Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "HandleRequestAsync: Received first request from Vantage. Configure service for auto start to avoid delayed responses.");
							Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\sc.exe", "config imcontrollerservice start=auto");
							VantageUtility.RegisterVantageLaunchedAfterOOBE();
						}
						brokerResponse = await this.HandleGetStatusRequestAsync(taskId, brokerRequest, linkedTokenSource.Token, updateInProgressEvent);
					}
					if (brokerRequest.ContractRequest.Command.Name.Equals(contractConstants.CommandNameGetPendingUpdates, StringComparison.InvariantCultureIgnoreCase))
					{
						brokerResponse = await this.HandleGetPendingUpdatesRequestAsync(taskId, brokerRequest, linkedTokenSource.Token, updateInProgressEvent);
					}
					if (brokerRequest.ContractRequest.Command.Name.Equals(contractConstants.CommandNameInstallPendingUpdates, StringComparison.InvariantCultureIgnoreCase))
					{
						brokerResponse = await this.HandleInstallPendingUpdatesRequestAsync(taskId, brokerRequest, linkedTokenSource.Token, updateInProgressEvent, intermediateResponseHandler);
					}
					if (brokerRequest.ContractRequest.Command.Name.Equals(contractConstants.CommandNameRestart, StringComparison.InvariantCultureIgnoreCase))
					{
						brokerResponse = await this.HandleRestartRequestAsync(taskId, brokerRequest, linkedTokenSource.Token, updateInProgressEvent);
					}
					if (brokerRequest.ContractRequest.Command.Name.Equals(contractConstants.CommandNameIsEntitled, StringComparison.InvariantCultureIgnoreCase))
					{
						Task.Run(delegate()
						{
							if (!VantageUtility.IsVantageLaunchedAfterOOBERegistered())
							{
								Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "HandleRequestAsync: Received first request from Vantage. Configure service for auto start to avoid delayed responses.");
								Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\sc.exe", "config imcontrollerservice start=auto");
								VantageUtility.RegisterVantageLaunchedAfterOOBE();
							}
						});
						brokerResponse = await this.HandleIsEntitled(taskId, brokerRequest, linkedTokenSource.Token, updateInProgressEvent);
					}
					if (brokerRequest.ContractRequest.Command.Name.Equals(contractConstants.CommandNameGetEntitledApps, StringComparison.InvariantCultureIgnoreCase))
					{
						brokerResponse = await this.HandleGetEntitledApps(taskId, brokerRequest, linkedTokenSource.Token, updateInProgressEvent);
					}
					if (brokerRequest.ContractRequest.Command.Name.Equals("Get-AppsRedemptionCodes", StringComparison.InvariantCultureIgnoreCase))
					{
						brokerResponse = await this.HandleGetAppsRedemptionCodes(taskId, brokerRequest, linkedTokenSource.Token, updateInProgressEvent);
					}
					if (brokerRequest.ContractRequest.Command.Name.Equals(contractConstants.CommandNameInstallEntitledApps, StringComparison.InvariantCultureIgnoreCase))
					{
						brokerResponse = await this.HandleInstallEntitledApps(taskId, brokerRequest, linkedTokenSource.Token, updateInProgressEvent, intermediateResponseHandler);
					}
				}
				CancellationTokenSource linkedTokenSource = null;
				try
				{
					CancellationTokenSource cancellationTokenSource2;
					this._requestCancelEventDictionary.TryRemove(taskId, out cancellationTokenSource2);
					if (cancellationTokenSource2 != null)
					{
						cancellationTokenSource2.Dispose();
					}
				}
				catch (Exception ex)
				{
					Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex, "ImcContractHandler: Exception occured while trying to remove linked token for taskId {0}", new object[] { taskId.ToString() });
				}
				contractConstants = null;
			}
			bool flag;
			if (brokerResponse == null)
			{
				flag = null != null;
			}
			else
			{
				BrokerResponseTask task = brokerResponse.Task;
				flag = ((task != null) ? task.ContractResponse : null) != null;
			}
			if (flag)
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity severity = Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information;
				string format = "ImcContractHandler: Response for {0} : {1}";
				object[] array = new object[2];
				array[0] = taskId;
				int num = 1;
				object obj;
				if (brokerResponse == null)
				{
					obj = null;
				}
				else
				{
					BrokerResponseTask task2 = brokerResponse.Task;
					if (task2 == null)
					{
						obj = null;
					}
					else
					{
						ContractResponse contractResponse = task2.ContractResponse;
						if (contractResponse == null)
						{
							obj = null;
						}
						else
						{
							ResponseData response = contractResponse.Response;
							obj = ((response != null) ? response.Data : null);
						}
					}
				}
				array[num] = obj ?? "<NULL>";
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(severity, format, array);
			}
			else
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity severity2 = Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information;
				string format2 = "ImcContractHandler: Error Response for {0} :: Error Code:{1} :: Error Description:{2}";
				object[] array2 = new object[3];
				array2[0] = taskId;
				int num2 = 1;
				int? num3;
				if (brokerResponse == null)
				{
					num3 = null;
				}
				else
				{
					FailureData error = brokerResponse.Error;
					num3 = ((error != null) ? new int?(error.ResultCode) : null);
				}
				array2[num2] = num3;
				int num4 = 2;
				object obj2;
				if (brokerResponse == null)
				{
					obj2 = null;
				}
				else
				{
					FailureData error2 = brokerResponse.Error;
					obj2 = ((error2 != null) ? error2.ResultDescription : null);
				}
				array2[num4] = obj2;
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(severity2, format2, array2);
			}
			return brokerResponse;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000271C File Offset: 0x0000091C
		public async Task<BrokerResponse> HandleIsEntitled(Guid taskId, BrokerRequest brokerRequest, CancellationToken cancelToken, ManualResetEventSlim updateInProgressEvent)
		{
			EntitledRequest entitledRequest = null;
			try
			{
				entitledRequest = Lenovo.Modern.Utilities.Services.Serializer.Deserialize<EntitledRequest>(brokerRequest.ContractRequest.Command.Parameter);
			}
			catch (Exception ex)
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex, "HandleIsEntitled: Task {0} exception", new object[] { taskId });
				return this.CreateImcErrorResponse("IMC Broker Request Agent", 1063, "Problem in getting DeviceId from request");
			}
			BrokerResponse result;
			if (entitledRequest == null || string.IsNullOrWhiteSpace(entitledRequest.DeviceID))
			{
				result = this.CreateImcErrorResponse("IMC Broker Request Agent", 1063, "DeviceId not supplied");
			}
			else
			{
				if (entitledRequest.AppName == null)
				{
					entitledRequest.AppName = "";
				}
				if (entitledRequest.AppVersion == null)
				{
					entitledRequest.AppVersion = "";
				}
				bool result2 = false;
				int num = 0;
				string[] campaignTagList = new string[0];
				try
				{
					result2 = this.GetEntitledStatusFromUDC(entitledRequest, cancelToken, ref campaignTagList, ref num);
				}
				catch (Exception)
				{
					return this.CreateImcErrorResponse("IMC Broker Request Agent", 1062, "Problem in getting response from UDC.ClientBrokerAgent. UDC service maybe down.");
				}
				if (num != 0)
				{
					result = this.CreateImcErrorResponse("IMC Broker Request Agent", num, "");
				}
				else
				{
					string data = Lenovo.Modern.CoreTypes.Utilities.Serializer.Serialize<Lenovo.Modern.ImController.ContractBroker.EntitledResponse>(new Lenovo.Modern.ImController.ContractBroker.EntitledResponse
					{
						Result = result2,
						UdcVersion = this.GetUDCServiceVersion(),
						CampaignTagList = campaignTagList
					});
					ContractResponse contractResponse = new ContractResponse
					{
						Response = new ResponseData
						{
							Data = data,
							DataType = ContractConstants.Get.DataTypeEntitledResponse
						}
					};
					result = BrokerResponseFactory.CreateSuccessfulBrokerResponse(taskId, contractResponse);
				}
			}
			return result;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000277C File Offset: 0x0000097C
		public async Task<BrokerResponse> HandleGetEntitledApps(Guid taskId, BrokerRequest brokerRequest, CancellationToken cancelToken, ManualResetEventSlim updateInProgressEvent)
		{
			EntitledRequest entitledRequest = null;
			try
			{
				entitledRequest = Lenovo.Modern.Utilities.Services.Serializer.Deserialize<EntitledRequest>(brokerRequest.ContractRequest.Command.Parameter);
			}
			catch (Exception ex)
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex, "HandleGetEntitledApps: Task {0} exception", new object[] { taskId });
				return this.CreateImcErrorResponse("IMC Broker Request Agent", 1063, "Problem in getting DeviceId from request");
			}
			BrokerResponse result;
			if (entitledRequest == null || string.IsNullOrWhiteSpace(entitledRequest.DeviceID))
			{
				result = this.CreateImcErrorResponse("IMC Broker Request Agent", 1063, "DeviceId not supplied");
			}
			else
			{
				if (entitledRequest.AppName == null)
				{
					entitledRequest.AppName = "";
				}
				if (entitledRequest.AppVersion == null)
				{
					entitledRequest.AppVersion = "";
				}
				int num = 0;
				List<App> list = new List<App>();
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "HandleGetEntitledApps: Getting entitled app list from");
				try
				{
					list = this.GetEntitledAppsFromUDC(entitledRequest, cancelToken, ref num);
				}
				catch (Exception)
				{
					return this.CreateImcErrorResponse("IMC Broker Request Agent", 1062, "Problem in getting response from UDC.ClientBrokerAgent. UDC service maybe down.");
				}
				if (num != 0)
				{
					result = this.CreateImcErrorResponse("IMC Broker Request Agent", num, "");
				}
				else
				{
					string data = Lenovo.Modern.CoreTypes.Utilities.Serializer.Serialize<EntitledAppsResponse>(new EntitledAppsResponse
					{
						UdcVersion = this.GetUDCServiceVersion(),
						AppList = list.ToArray()
					});
					ContractResponse contractResponse = new ContractResponse
					{
						Response = new ResponseData
						{
							Data = data,
							DataType = ContractConstants.Get.DataTypeEntitledAppsResponse
						}
					};
					result = BrokerResponseFactory.CreateSuccessfulBrokerResponse(taskId, contractResponse);
				}
			}
			return result;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000027DC File Offset: 0x000009DC
		public async Task<BrokerResponse> HandleGetAppsRedemptionCodes(Guid taskId, BrokerRequest brokerRequest, CancellationToken cancelToken, ManualResetEventSlim updateInProgressEvent)
		{
			string text = "";
			List<App> list;
			try
			{
				EntitledAppsRequest entitledAppsRequest = Lenovo.Modern.Utilities.Services.Serializer.Deserialize<EntitledAppsRequest>(brokerRequest.ContractRequest.Command.Parameter);
				text = entitledAppsRequest.DeviceID;
				list = entitledAppsRequest.AppList.ToList<App>();
			}
			catch (Exception ex)
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex, "HandleGetAppsRedemptionCodes: Task {0} exception", new object[] { taskId });
				return this.CreateImcErrorResponse("IMC Broker Request Agent", 1063, "Problem in getting DeviceId from request");
			}
			BrokerResponse result;
			if (string.IsNullOrWhiteSpace(text))
			{
				result = this.CreateImcErrorResponse("IMC Broker Request Agent", 1063, "DeviceId not supplied");
			}
			else
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "HandleGetAppsRedemptionCodes: Get Apps Redemption Codes");
				int num = 0;
				try
				{
					this.GetAppsRedemptionCodes(taskId, text, ref list, cancelToken, ref num);
				}
				catch (Exception ex2)
				{
					Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex2, "HandleGetAppsRedemptionCodes: Exception");
					return this.CreateImcErrorResponse("IMC Broker Request Agent", 1062, "Problem in getting response from UDC.ClientBrokerAgent. UDC service maybe down.");
				}
				if (num != 0)
				{
					result = this.CreateImcErrorResponse("IMC Broker Request Agent", num, "");
				}
				else
				{
					string text2 = Lenovo.Modern.CoreTypes.Utilities.Serializer.Serialize<EntitledAppsResponse>(new EntitledAppsResponse
					{
						UdcVersion = this.GetUDCServiceVersion(),
						AppList = list.ToArray()
					});
					ContractResponse contractResponse = new ContractResponse
					{
						Response = new ResponseData
						{
							Data = text2,
							DataType = ContractConstants.Get.DataTypeEntitledAppsResponse
						}
					};
					Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "HandleGetAppsRedemptionCodes: Finished Response:{0}", new object[] { text2 });
					result = BrokerResponseFactory.CreateSuccessfulBrokerResponse(taskId, contractResponse);
				}
			}
			return result;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000283C File Offset: 0x00000A3C
		public async Task<BrokerResponse> HandleInstallEntitledApps(Guid taskId, BrokerRequest brokerRequest, CancellationToken cancelToken, ManualResetEventSlim updateInProgressEvent, Action<Guid, BrokerResponse> intermediateResponseHandler)
		{
			string text = "";
			List<App> list;
			try
			{
				EntitledAppsRequest entitledAppsRequest = Lenovo.Modern.Utilities.Services.Serializer.Deserialize<EntitledAppsRequest>(brokerRequest.ContractRequest.Command.Parameter);
				text = entitledAppsRequest.DeviceID;
				list = entitledAppsRequest.AppList.ToList<App>();
			}
			catch (Exception ex)
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex, "HandleInstallEntitledApps: Task {0} exception", new object[] { taskId });
				return this.CreateImcErrorResponse("IMC Broker Request Agent", 1063, "Problem in getting DeviceId from request");
			}
			BrokerResponse result;
			if (string.IsNullOrWhiteSpace(text))
			{
				result = this.CreateImcErrorResponse("IMC Broker Request Agent", 1063, "DeviceId not supplied");
			}
			else
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "HandleInstallEntitledApps: Download and install entitled apps");
				int num = 0;
				try
				{
					this.InstallEntitledAppsUsingUDC(taskId, text, ref list, cancelToken, ref num, intermediateResponseHandler);
				}
				catch (Exception ex2)
				{
					Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex2, "HandleInstallEntitledApps: Exception");
					return this.CreateImcErrorResponse("IMC Broker Request Agent", 1062, "Problem in getting response from UDC.ClientBrokerAgent. UDC service maybe down.");
				}
				if (num != 0)
				{
					result = this.CreateImcErrorResponse("IMC Broker Request Agent", num, "");
				}
				else
				{
					string text2 = Lenovo.Modern.CoreTypes.Utilities.Serializer.Serialize<EntitledAppsResponse>(new EntitledAppsResponse
					{
						UdcVersion = this.GetUDCServiceVersion(),
						AppList = list.ToArray()
					});
					ContractResponse contractResponse = new ContractResponse
					{
						Response = new ResponseData
						{
							Data = text2,
							DataType = ContractConstants.Get.DataTypeEntitledAppsResponse
						}
					};
					Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "HandleInstallEntitledApps: Finished Response:{0}", new object[] { text2 });
					result = BrokerResponseFactory.CreateSuccessfulBrokerResponse(taskId, contractResponse);
				}
			}
			return result;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000028A4 File Offset: 0x00000AA4
		public async Task<BrokerResponse> HandleGetStatusRequestAsync(Guid taskId, BrokerRequest brokerRequest, CancellationToken cancelToken, ManualResetEventSlim updateInProgressEvent)
		{
			string data = Lenovo.Modern.CoreTypes.Utilities.Serializer.Serialize<StatusResponse>(new StatusResponse
			{
				Status = new Status
				{
					ImControllerVersion = Constants.ImControllerVersion,
					Mode = ((updateInProgressEvent != null) ? (updateInProgressEvent.IsSet ? ContractConstants.Get.StatusMaintenanceMode : ContractConstants.Get.StatusNormal) : ContractConstants.Get.StatusNormal)
				}
			});
			ContractResponse contractResponse = new ContractResponse
			{
				Response = new ResponseData
				{
					Data = data,
					DataType = ContractConstants.Get.DataTypeStatusResponse
				}
			};
			return BrokerResponseFactory.CreateSuccessfulBrokerResponse(taskId, contractResponse);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000028F4 File Offset: 0x00000AF4
		public async Task<BrokerResponse> HandleInstallPendingUpdatesRequestAsync(Guid taskId, BrokerRequest brokerRequest, CancellationToken cancelToken, ManualResetEventSlim updateInProgressEvent, Action<Guid, BrokerResponse> intermediateResponseHandler)
		{
			Lenovo.Modern.CoreTypes.Contracts.ImController.Package[] packageList;
			try
			{
				InstallPendingRequest installPendingRequest = Lenovo.Modern.Utilities.Services.Serializer.Deserialize<InstallPendingRequest>(brokerRequest.ContractRequest.Command.Parameter);
				packageList = installPendingRequest.PackageList;
			}
			catch (Exception ex)
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex, "HandleInstallPendingUpdatesRequest: Task {0} exception", new object[] { taskId });
				return this.CreateImcErrorResponse("IMC Broker Request Agent", ImcContractHandler._errorResponseCode, "Problem in getting PackageList from request");
			}
			BrokerResponse result;
			if (packageList == null || packageList.Count<Lenovo.Modern.CoreTypes.Contracts.ImController.Package>() == 0)
			{
				result = this.CreateImcErrorResponse("IMC Broker Request Agent", ImcContractHandler._errorResponseCode, "No packages supplied in the request");
			}
			else
			{
				int percentIncreamentStep = 100 / packageList.Count<Lenovo.Modern.CoreTypes.Contracts.ImController.Package>();
				int totalPercentageComplete = 0;
				int numberOfFoldersRenamed = 0;
				int numberOfRenameAttempts = 0;
				BrokerResponse brokerResponse;
				foreach (Lenovo.Modern.CoreTypes.Contracts.ImController.Package package in packageList)
				{
					try
					{
						if (package != null && package.name != null)
						{
							if (this.IsValidPluginName(package.name, cancelToken))
							{
								if (PackageInstaller.DoesPluginPreinstallFolderExist(package.name, true))
								{
									int num = numberOfRenameAttempts;
									numberOfRenameAttempts = num + 1;
									Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "HandleInstallPendingUpdatesRequest: Attempt to install from pre-installed update: {0}", new object[] { package.name });
									package.status = "inprogress";
									brokerResponse = this.CreateInstallPendingResponse(taskId, packageList, totalPercentageComplete);
									intermediateResponseHandler(taskId, brokerResponse);
									string pluginInstallationFolder = InstallationLocator.GetPluginInstallationLocation();
									string pathToDirectory = Path.Combine(Environment.ExpandEnvironmentVariables(Constants.PendingPackagesTempLocation), package.name) + "_";
									bool flag = await(await this._directory.GetDirectoryAsync(pathToDirectory)).MoveAsync(pluginInstallationFolder, CollisionOption.ReplaceExisting);
									if (cancelToken.IsCancellationRequested)
									{
										return BrokerResponseFactory.GetImcRequestCancellationErrorMessage(taskId.ToString());
									}
									if (flag)
									{
										flag = await this._pluginRepository.FinalInstallPackage(package.name, false);
										if (flag)
										{
											num = numberOfFoldersRenamed;
											numberOfFoldersRenamed = num + 1;
											package.status = "success";
											Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "HandleInstallPendingUpdatesRequest: {0} successfully installed from pre-installed folder", new object[] { package.name });
											this._pluginRepository.UpdatePacakgeInformationInRegistry(package.name, package.version, false);
										}
										else
										{
											Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Error, "HandleInstallPendingUpdatesRequest: Failed to apply pending package: {0}", new object[] { package.name });
											package.status = "error";
										}
										if (cancelToken.IsCancellationRequested)
										{
											return BrokerResponseFactory.GetImcRequestCancellationErrorMessage(taskId.ToString());
										}
									}
									pluginInstallationFolder = null;
								}
							}
							else
							{
								Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Warning, "HandleInstallPendingUpdatesRequest: Invalid plugin name was specified", new object[] { package.name });
							}
						}
					}
					catch (Exception ex2)
					{
						Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex2, "HandleInstallPendingUpdatesRequest:  Exception while processing {0}", new object[] { package.name });
						package.status = "error";
					}
					totalPercentageComplete += percentIncreamentStep;
					brokerResponse = this.CreateInstallPendingResponse(taskId, packageList, totalPercentageComplete);
					intermediateResponseHandler(taskId, brokerResponse);
					package = null;
				}
				Lenovo.Modern.CoreTypes.Contracts.ImController.Package[] array = null;
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "PackageUpdater: ApplyPendingPluginFolders Summary: Applied {0} out of {1} packages", new object[] { numberOfFoldersRenamed, numberOfRenameAttempts });
				brokerResponse = this.CreateInstallPendingResponse(taskId, packageList, 100);
				result = brokerResponse;
			}
			return result;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000295C File Offset: 0x00000B5C
		public async Task<BrokerResponse> HandleRestartRequestAsync(Guid taskId, BrokerRequest brokerRequest, CancellationToken cancelToken, ManualResetEventSlim updateInProgressEvent)
		{
			try
			{
				await Task.Run(delegate()
				{
					Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\shutdown.exe", "/r /f");
				});
			}
			catch (Exception)
			{
			}
			return BrokerResponseFactory.CreateSuccessfulBrokerResponse(taskId, new ContractResponse());
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000029A4 File Offset: 0x00000BA4
		public async Task<BrokerResponse> HandleGetPendingUpdatesRequestAsync(Guid taskId, BrokerRequest brokerRequest, CancellationToken cancelToken, ManualResetEventSlim updateInProgressEvent)
		{
			PackageSubscription packageSubscription = await this._subscriptionManager.GetSubscriptionAsync(cancelToken);
			string text = "";
			try
			{
				text = Lenovo.Modern.Utilities.Services.Serializer.Deserialize<PendingUpdateRequest>(brokerRequest.ContractRequest.Command.Parameter).appId;
			}
			catch (Exception ex)
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex, "HandleGetPendingUpdatesRequest: Task {0} exception", new object[] { taskId });
				return this.CreateImcErrorResponse("IMC Broker Request Agent", ImcContractHandler._errorResponseCode, "Problem in getting AppId from request");
			}
			BrokerResponse result;
			if (string.IsNullOrWhiteSpace(text))
			{
				result = this.CreateImcErrorResponse("IMC Broker Request Agent", ImcContractHandler._errorResponseCode, "AppId not supplied");
			}
			else
			{
				BrokerResponse brokerResponse;
				if (packageSubscription != null && packageSubscription.PackageList != null && packageSubscription.PackageList.Any<Lenovo.Modern.ImController.Shared.Model.Packages.Package>())
				{
					List<Lenovo.Modern.CoreTypes.Contracts.ImController.Package> list = new List<Lenovo.Modern.CoreTypes.Contracts.ImController.Package>();
					Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "HandleGetPendingUpdatesRequest: Checking for pre-installed updates for packages...");
					foreach (Lenovo.Modern.ImController.Shared.Model.Packages.Package package in packageSubscription.PackageList)
					{
						try
						{
							if (package != null && package.PackageInformation != null && package.PackageInformation.Name != null && PackageInstaller.DoesPluginPreinstallFolderExist(package.PackageInformation.Name, true))
							{
								if (PackageSettingsAgent.IsPackageUpdateDisabled(package))
								{
									Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Warning, "HandleGetPendingUpdatesRequest: Package update disabled by subscription- {0}", new object[] { package.PackageInformation.Name });
									Directory.Delete(Environment.ExpandEnvironmentVariables(Constants.PendingPackagesTempLocation) + "\\" + package.PackageInformation.Name + "_", true);
								}
								else
								{
									Dictionary<string, string> packageUapAssociationIds = PackageSettingsAgent.GetPackageUapAssociationIds(package);
									if (packageUapAssociationIds != null && packageUapAssociationIds.ContainsKey(text))
									{
										Lenovo.Modern.CoreTypes.Contracts.ImController.Package package2 = new Lenovo.Modern.CoreTypes.Contracts.ImController.Package();
										package2.name = package.PackageInformation.Name;
										package2.version = (MsSignability.IsMsSignRequired() ? package.PackageInformation.MsVersion : package.PackageInformation.Version);
										package2.lastModified = package.PackageInformation.DateModified;
										package2.rebootRequired = true;
										if (!list.Contains(package2))
										{
											list.Add(package2);
										}
										Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "HandleGetPendingUpdatesRequest: Package {0} is pending.", new object[] { package.PackageInformation.Name });
									}
								}
							}
						}
						catch (Exception ex2)
						{
							Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex2, "HandleGetPendingUpdatesRequest: Exception while processing");
							return this.CreateImcErrorResponse("IMC Broker Request Agent", ImcContractHandler._errorResponseCode, "Execption occured in HandleGetPendingUpdatesRequest {");
						}
					}
					string data = Lenovo.Modern.CoreTypes.Utilities.Serializer.Serialize<PendingUpdateResponse>(new PendingUpdateResponse
					{
						PackageList = list.ToArray()
					});
					brokerResponse = BrokerResponseFactory.CreateSuccessfulBrokerResponse(taskId, new ContractResponse
					{
						Response = new ResponseData
						{
							Data = data,
							DataType = ContractConstants.Get.DataTypePendingUpdateResponse
						}
					});
				}
				else
				{
					brokerResponse = this.CreateImcErrorResponse("IMC Broker Request Agent", ImcContractHandler._errorResponseCode, "Error in getting packages from subscription");
				}
				result = brokerResponse;
			}
			return result;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002A04 File Offset: 0x00000C04
		private BrokerResponse CreateInstallPendingResponse(Guid taskId, Lenovo.Modern.CoreTypes.Contracts.ImController.Package[] packageList, int percentageComplete)
		{
			string data = Lenovo.Modern.CoreTypes.Utilities.Serializer.Serialize<InstallPendingResponse>(new InstallPendingResponse
			{
				totalPercentageComplete = percentageComplete.ToString(),
				PackageList = packageList.ToArray<Lenovo.Modern.CoreTypes.Contracts.ImController.Package>()
			});
			ContractResponse contractResponse = new ContractResponse
			{
				Response = new ResponseData
				{
					Data = data,
					DataType = ContractConstants.Get.DataTypeInstallPendingResponse
				}
			};
			if (percentageComplete < 100)
			{
				return BrokerResponseFactory.CreateIntermediateBrokerResponse(taskId, contractResponse, percentageComplete);
			}
			return BrokerResponseFactory.CreateSuccessfulBrokerResponse(taskId, contractResponse);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00002A74 File Offset: 0x00000C74
		private string GetInstallEntitledAppsStatusFromUDC(string deviceId, ref List<UDCEntitledApp> list, ref int error)
		{
			UDCEntitldeAppsResponse udcentitldeAppsResponse = new UDCEntitldeAppsResponse();
			udcentitldeAppsResponse.swCount = list.Count<UDCEntitledApp>();
			udcentitldeAppsResponse.swList = list.ToArray();
			error = 1062;
			ClientBrokerMessage clientBrokerMessage = new ClientBrokerMessage();
			clientBrokerMessage.Command = ClientBrokerCommandConstants.Get.GetInstallEntitledAppsStatusRequestCommand;
			clientBrokerMessage.DeviceId = deviceId;
			clientBrokerMessage.Message = JsonSerializer.WriteFromObject<UDCEntitldeAppsResponse>(udcentitldeAppsResponse);
			CBAWrapper cbawrapper = new CBAWrapper();
			using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)))
			{
				cbawrapper.Agent.SendMessageToUDCAndGetResponse(ref clientBrokerMessage, cancellationTokenSource.Token);
				if (clientBrokerMessage != null)
				{
					error = clientBrokerMessage.ServerReturnCode;
					if (clientBrokerMessage.Message != null)
					{
						udcentitldeAppsResponse = JsonSerializer.ReadToObject<UDCEntitldeAppsResponse>(clientBrokerMessage.Message);
					}
				}
			}
			cbawrapper.Dispose();
			List<App> list2 = new List<App>();
			this.GetImcAppListFromUDCEntitledResponse(ref list2, udcentitldeAppsResponse);
			this._semaphore.Wait();
			this._appList = list2;
			this._semaphore.Release();
			EntitledAppsResponse entitledAppsResponse = new EntitledAppsResponse
			{
				UdcVersion = this.GetUDCServiceVersion(),
				AppList = list2.ToArray()
			};
			App[] appList = entitledAppsResponse.AppList;
			for (int i = 0; i < appList.Length; i++)
			{
				App app = appList[i];
				if (string.IsNullOrWhiteSpace(app.status))
				{
					app.status = "downloading";
					app.progress = "0";
				}
				UDCEntitledApp udcentitledApp = list.FirstOrDefault((UDCEntitledApp s) => s.partNum == app.partNum);
				if (udcentitledApp != null)
				{
					udcentitledApp.progress = app.progress;
				}
			}
			return Lenovo.Modern.CoreTypes.Utilities.Serializer.Serialize<EntitledAppsResponse>(entitledAppsResponse);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002C2C File Offset: 0x00000E2C
		private BrokerResponse GetInstallEntitledAppsIntermediateResponseFromUDC(string deviceId, Guid taskId, List<UDCEntitledApp> appList, CancellationToken cancelToken, ref int error)
		{
			string installEntitledAppsStatusFromUDC = this.GetInstallEntitledAppsStatusFromUDC(deviceId, ref appList, ref error);
			ContractResponse contractResponse = new ContractResponse
			{
				Response = new ResponseData
				{
					Data = installEntitledAppsStatusFromUDC,
					DataType = ContractConstants.Get.DataTypeEntitledAppsResponse
				}
			};
			int num = Convert.ToInt32((appList.Count<UDCEntitledApp>() > 0) ? appList.ElementAt(0).progress : "100");
			if (num == 100)
			{
				num = 99;
			}
			return BrokerResponseFactory.CreateIntermediateBrokerResponse(taskId, contractResponse, num);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00002CA0 File Offset: 0x00000EA0
		private bool GetEntitledStatusFromUDC(EntitledRequest entitledRequest, CancellationToken cancelToken, ref string[] tags, ref int errorCode)
		{
			bool result = false;
			errorCode = 1062;
			CBAWrapper cbawrapper = new CBAWrapper();
			if (!cbawrapper.Connected)
			{
				errorCode = cbawrapper.GetLastErrorCode();
				return false;
			}
			if (!this.IsInternetConnected())
			{
				errorCode = 1067;
				return false;
			}
			ClientBrokerMessage clientBrokerMessage = new ClientBrokerMessage();
			clientBrokerMessage.Command = ClientBrokerCommandConstants.Get.IsEntitledRequestCommand;
			clientBrokerMessage.DeviceId = entitledRequest.DeviceID;
			clientBrokerMessage.Message = JsonSerializer.WriteFromObject<UDCEntitledRequest>(new UDCEntitledRequest(entitledRequest.AppName, entitledRequest.AppVersion));
			try
			{
				cbawrapper.Agent.SendMessageToUDCAndGetResponse(ref clientBrokerMessage, cancelToken);
				if (clientBrokerMessage != null)
				{
					errorCode = clientBrokerMessage.ServerReturnCode;
					if (clientBrokerMessage.Message != null)
					{
						UDCEntitledResponse udcentitledResponse = JsonSerializer.ReadToObject<UDCEntitledResponse>(clientBrokerMessage.Message);
						result = udcentitledResponse.isEntitled;
						tags = udcentitledResponse.campaignTags;
					}
				}
			}
			catch (Exception ex)
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex, "Exception in GetEntitledStatusFromUDC.");
			}
			cbawrapper.Dispose();
			return result;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002D84 File Offset: 0x00000F84
		private List<App> GetEntitledAppsFromUDC(EntitledRequest entitledRequest, CancellationToken cancelToken, ref int errorCode)
		{
			List<App> list = new List<App>();
			CBAWrapper cbawrapper = new CBAWrapper();
			if (!cbawrapper.Connected)
			{
				errorCode = cbawrapper.GetLastErrorCode();
				return list;
			}
			if (!this.IsInternetConnected())
			{
				errorCode = 1067;
				return list;
			}
			errorCode = 1062;
			ClientBrokerMessage clientBrokerMessage = new ClientBrokerMessage();
			clientBrokerMessage.DeviceId = entitledRequest.DeviceID;
			clientBrokerMessage.Message = JsonSerializer.WriteFromObject<UDCEntitledRequest>(new UDCEntitledRequest(entitledRequest.AppName, entitledRequest.AppVersion));
			clientBrokerMessage.Command = ClientBrokerCommandConstants.Get.GetEntitledAppsRequestCommand;
			try
			{
				cbawrapper.Agent.SendMessageToUDCAndGetResponse(ref clientBrokerMessage, cancelToken);
				if (clientBrokerMessage != null)
				{
					errorCode = clientBrokerMessage.ServerReturnCode;
					if (clientBrokerMessage.Message != null)
					{
						UDCEntitldeAppsResponse udcentitldeAppsResponse = JsonSerializer.ReadToObject<UDCEntitldeAppsResponse>(clientBrokerMessage.Message);
						this._entitledAppList = udcentitldeAppsResponse.swList.ToList<UDCEntitledApp>();
						this.GetImcAppListFromUDCEntitledResponse(ref list, udcentitldeAppsResponse);
					}
				}
			}
			catch (Exception ex)
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex, "Exception in GetEntitledAppsFromUDC.");
			}
			cbawrapper.Dispose();
			if (list.Count<App>() == 0 && errorCode == 0)
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "GetEntitledAppsFromUDC. UDC returned empty app list");
				errorCode = 1066;
			}
			return list;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002E94 File Offset: 0x00001094
		private bool GetAppsRedemptionCodes(Guid taskId, string deviceId, ref List<App> list, CancellationToken cancelToken, ref int error)
		{
			Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "GetAppsRedemptionCodes: Entry");
			CBAWrapper cbawrapper = new CBAWrapper();
			if (!cbawrapper.Connected)
			{
				error = cbawrapper.GetLastErrorCode();
				return false;
			}
			if (!this.IsInternetConnected())
			{
				error = 1067;
				return false;
			}
			error = 1062;
			try
			{
				UDCEntitldeAppsResponse instance = new UDCEntitldeAppsResponse();
				this.GetUDCEntitledResponseFromImcAppList(ref instance, list);
				if (list.Count<App>() > 0)
				{
					ClientBrokerMessage clientBrokerMessage = new ClientBrokerMessage();
					clientBrokerMessage.Command = "GetAppsRedemptionCodes";
					clientBrokerMessage.DeviceId = deviceId;
					clientBrokerMessage.Message = JsonSerializer.WriteFromObject<UDCEntitldeAppsResponse>(instance);
					cbawrapper.Agent.SendMessageToUDCAndGetResponse(ref clientBrokerMessage, cancelToken);
					if (clientBrokerMessage != null)
					{
						error = clientBrokerMessage.ServerReturnCode;
						if (clientBrokerMessage.Message != null)
						{
							UDCEntitldeAppsResponse response = JsonSerializer.ReadToObject<UDCEntitldeAppsResponse>(clientBrokerMessage.Message);
							this.GetImcAppListFromUDCEntitledResponse(ref list, response);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex, "Exception in GetAppsRedemptionCodes.");
			}
			cbawrapper.Dispose();
			Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "GetAppsRedemptionCodes: Exit");
			return true;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00002F88 File Offset: 0x00001188
		private bool InstallEntitledAppsUsingUDC(Guid taskId, string deviceId, ref List<App> list, CancellationToken cancelToken, ref int error, Action<Guid, BrokerResponse> intermediateResponseHandler)
		{
			Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "InstallEntitledAppsUsingUDC: Entry");
			this._semaphore.Wait();
			if (this._appList != null)
			{
				error = 1066;
				list = this._appList;
				this._semaphore.Release();
				return false;
			}
			this._semaphore.Release();
			CBAWrapper cbawrapper = new CBAWrapper();
			if (!cbawrapper.Connected)
			{
				error = cbawrapper.GetLastErrorCode();
				return false;
			}
			if (!this.IsInternetConnected())
			{
				error = 1067;
				return false;
			}
			error = 1062;
			new List<UDCEntitledApp>();
			ClientBrokerMessage clientBrokerMessage = new ClientBrokerMessage();
			clientBrokerMessage.Command = ClientBrokerCommandConstants.Get.GetEntitledAppsRequestCommand;
			clientBrokerMessage.DeviceId = deviceId;
			CancellationTokenSource cancellationTokenSource = null;
			try
			{
				UDCEntitldeAppsResponse entitledResponse = new UDCEntitldeAppsResponse();
				this.GetUDCEntitledResponseFromImcAppList(ref entitledResponse, list);
				cancellationTokenSource = new CancellationTokenSource();
				CancellationToken completionToken = cancellationTokenSource.Token;
				Task.Run(delegate()
				{
					try
					{
						while (!completionToken.IsCancellationRequested)
						{
							int num = 0;
							BrokerResponse installEntitledAppsIntermediateResponseFromUDC = this.GetInstallEntitledAppsIntermediateResponseFromUDC(deviceId, taskId, entitledResponse.swList.ToList<UDCEntitledApp>(), completionToken, ref num);
							if (completionToken.IsCancellationRequested)
							{
								Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "HandleInstallEntitledApps: Intermediate response. Breaking since cancellation token is set");
								break;
							}
							Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity severity = Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information;
							string format = "HandleInstallEntitledApps: Intermediate Response is {0}";
							object[] array = new object[1];
							int num2 = 0;
							object obj;
							if (installEntitledAppsIntermediateResponseFromUDC == null)
							{
								obj = null;
							}
							else
							{
								BrokerResponseTask task = installEntitledAppsIntermediateResponseFromUDC.Task;
								if (task == null)
								{
									obj = null;
								}
								else
								{
									ContractResponse contractResponse = task.ContractResponse;
									if (contractResponse == null)
									{
										obj = null;
									}
									else
									{
										ResponseData response2 = contractResponse.Response;
										obj = ((response2 != null) ? response2.Data : null);
									}
								}
							}
							array[num2] = obj ?? "<NULL>";
							Lenovo.Modern.Utilities.Services.Logging.Logger.Log(severity, format, array);
							intermediateResponseHandler(taskId, installEntitledAppsIntermediateResponseFromUDC);
							Task.Delay(3000).Wait(completionToken);
						}
					}
					catch (Exception ex2)
					{
						Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "HandleInstallEntitledApps: Stopping the task for Intermidate responses. {0}", new object[] { ex2.GetType().ToString() });
					}
					this._semaphore.Wait();
					this._appList = null;
					this._semaphore.Release();
				});
				UDCEntitldeAppsResponse response = new UDCEntitldeAppsResponse();
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "InstallEntitledAppsUsingUDC: Calling InstallEntitled apps");
				if (list.Count<App>() > 0)
				{
					clientBrokerMessage = new ClientBrokerMessage();
					clientBrokerMessage.Command = ClientBrokerCommandConstants.Get.InstallEntitledAppsRequestCommand;
					clientBrokerMessage.DeviceId = deviceId;
					clientBrokerMessage.Message = JsonSerializer.WriteFromObject<UDCEntitldeAppsResponse>(entitledResponse);
					cbawrapper.Agent.SendMessageToUDCAndGetResponse(ref clientBrokerMessage, cancelToken);
					if (clientBrokerMessage != null)
					{
						error = clientBrokerMessage.ServerReturnCode;
						if (clientBrokerMessage.Message != null)
						{
							list.Clear();
							response = JsonSerializer.ReadToObject<UDCEntitldeAppsResponse>(clientBrokerMessage.Message);
							this.GetImcAppListFromUDCEntitledResponse(ref list, response);
						}
					}
				}
				cancellationTokenSource.Cancel();
			}
			catch (Exception ex)
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex, "Exception in InstallEntitledAppsUsingUDC.");
			}
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Dispose();
			}
			cbawrapper.Dispose();
			this._semaphore.Wait();
			this._appList = null;
			this._semaphore.Release();
			Lenovo.Modern.Utilities.Services.Logging.Logger.Log(Lenovo.Modern.Utilities.Services.Logging.Logger.LogSeverity.Information, "InstallEntitledAppsUsingUDC: Exit");
			return true;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000031A4 File Offset: 0x000013A4
		private bool GetImcAppListFromUDCEntitledResponse(ref List<App> appList, UDCEntitldeAppsResponse response)
		{
			bool result = false;
			try
			{
				appList.Clear();
				if (response.swCount > 0)
				{
					foreach (UDCEntitledApp udcentitledApp in response.swList)
					{
						App app = new App();
						app.name = udcentitledApp.name;
						app.CampaignTagList = udcentitledApp.campaignTags;
						app.appID = udcentitledApp.fruPn;
						app.partNum = udcentitledApp.partNum;
						app.status = udcentitledApp.status;
						app.progress = udcentitledApp.progress;
						app.error = udcentitledApp.error;
						if (udcentitledApp.moduleList.Count<UDCSwModules>() > 0)
						{
							app.version = udcentitledApp.moduleList[0].version;
							app.redemptionURL = udcentitledApp.moduleList[0].redemptionUrl;
							app.swHomePageURL = udcentitledApp.moduleList[0].swHomePageUrl;
							app.activationCode = udcentitledApp.moduleList[0].licenseKey;
							app.licAgreementURL = udcentitledApp.moduleList[0].licenseAgreementUrl;
							app.size = udcentitledApp.moduleList[0].size;
						}
						appList.Add(app);
					}
				}
				result = true;
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000032FC File Offset: 0x000014FC
		private bool GetUDCEntitledResponseFromImcAppList(ref UDCEntitldeAppsResponse response, List<App> appList)
		{
			bool result = false;
			try
			{
				List<UDCEntitledApp> list = new List<UDCEntitledApp>();
				if (appList.Count<App>() > 0)
				{
					using (List<App>.Enumerator enumerator = appList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							App app = enumerator.Current;
							UDCEntitledApp udcentitledApp = new UDCEntitledApp();
							udcentitledApp.name = app.name;
							udcentitledApp.fruPn = app.appID;
							udcentitledApp.partNum = app.partNum;
							udcentitledApp.progress = app.progress;
							udcentitledApp.error = app.error;
							udcentitledApp.status = app.status;
							UDCEntitledApp udcentitledApp2 = udcentitledApp;
							string[] campaignTagList = app.CampaignTagList;
							udcentitledApp2.campaignTags = ((campaignTagList != null) ? campaignTagList.ToArray<string>() : null);
							List<UDCSwModules> list2 = new List<UDCSwModules>();
							UDCSwModules udcswModules = new UDCSwModules();
							udcswModules.version = app.version;
							udcswModules.redemptionUrl = app.redemptionURL;
							udcswModules.licenseAgreementUrl = app.licAgreementURL;
							udcswModules.licenseKey = app.activationCode;
							udcswModules.swHomePageUrl = app.swHomePageURL;
							udcswModules.size = app.size;
							UDCEntitledApp udcentitledApp3 = this._entitledAppList.FirstOrDefault((UDCEntitledApp s) => s.fruPn == app.appID);
							if (udcentitledApp3 != null && udcentitledApp3.moduleList != null && udcentitledApp3.moduleList.Count<UDCSwModules>() > 0)
							{
								udcswModules.fruPn = udcentitledApp3.moduleList.ElementAt(0).fruPn;
							}
							list2.Add(udcswModules);
							udcentitledApp.moduleList = list2.ToArray();
							list.Add(udcentitledApp);
						}
					}
					response.swCount = list.Count<UDCEntitledApp>();
					response.swList = list.ToArray();
				}
				result = true;
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003528 File Offset: 0x00001728
		private bool IsValidPluginName(string pluginName, CancellationToken cancelToken)
		{
			bool result = false;
			PackageSubscription result2 = this._subscriptionManager.GetSubscriptionAsync(cancelToken).Result;
			if (result2 != null && result2.PackageList != null && result2.PackageList.Any<Lenovo.Modern.ImController.Shared.Model.Packages.Package>())
			{
				try
				{
					Lenovo.Modern.ImController.Shared.Model.Packages.Package package = result2.PackageList.FirstOrDefault((Lenovo.Modern.ImController.Shared.Model.Packages.Package p) => p != null && p.PackageInformation != null && (!string.IsNullOrWhiteSpace(p.PackageInformation.Name) & p.PackageInformation.Name.Equals(pluginName, StringComparison.InvariantCultureIgnoreCase)));
					if (package != null && package.PackageInformation != null && package.PackageInformation.Name != null && SubscribedPackageManager.IsPackageApplicable(result2, package, cancelToken))
					{
						result = true;
					}
				}
				catch (Exception)
				{
				}
			}
			return result;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000035C0 File Offset: 0x000017C0
		private BrokerResponse CreateImcErrorResponse(string responseCodegroup, int responseCode, string responseDescription = "")
		{
			if (string.IsNullOrEmpty(responseDescription))
			{
				responseDescription = "Error";
				if (responseCode != 27)
				{
					if (responseCode != 62)
					{
						switch (responseCode)
						{
						case 1061:
							responseDescription = "UDC is not installed on the system";
							goto IL_BC;
						case 1062:
							if (this.IsUDCServiceInstalled())
							{
								responseDescription = "UDC is not running on the system";
								goto IL_BC;
							}
							responseCode = 1061;
							responseDescription = "UDC is not installed on the system";
							goto IL_BC;
						case 1063:
							responseDescription = "Invalid DeviceId supplied";
							goto IL_BC;
						case 1064:
							responseDescription = "Device is not configured";
							goto IL_BC;
						case 1065:
							break;
						case 1066:
							responseDescription = "APS is not available";
							goto IL_BC;
						case 1067:
							goto IL_9E;
						default:
							responseCode = 1066;
							responseDescription = "APS is not available";
							goto IL_BC;
						}
					}
					responseCode = 1065;
					responseDescription = "Device is not registered";
					goto IL_BC;
				}
				IL_9E:
				responseCode = 1067;
				responseDescription = "Internet connection is not there";
			}
			IL_BC:
			return BrokerResponseFactory.CreateErrorBrokerResponse(responseCodegroup, responseCode, responseDescription);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003694 File Offset: 0x00001894
		private bool IsInternetConnected()
		{
			bool result = false;
			try
			{
				if (this._networkListManager == null)
				{
					this._networkListManager = (NETWORKLIST.NetworkListManager)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("DCB00C01-570F-4A9B-8D69-199FDBA5723B")));
				}
				result = this._networkListManager.IsConnectedToInternet;
			}
			catch (Exception ex)
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex, "ImcContractHandler.IsInternetConnected: Exception {0}", new object[] { ex.Message });
			}
			return result;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003708 File Offset: 0x00001908
		private bool IsUDCServiceInstalled()
		{
			ServiceController serviceController = ServiceController.GetServices().FirstOrDefault((ServiceController s) => s.ServiceName.ToLower() == "udcservice");
			return serviceController != null;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003744 File Offset: 0x00001944
		private string GetUDCServiceVersion()
		{
			ServiceController serviceController = ServiceController.GetServices().FirstOrDefault((ServiceController s) => s.ServiceName.ToLower() == "udcservice");
			string text = "";
			if (serviceController != null)
			{
				string text2 = "";
				try
				{
					RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
					if (registryKey != null)
					{
						RegistryKey registryKey2 = registryKey.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\" + serviceController.ServiceName);
						if (registryKey2 != null)
						{
							text2 = registryKey2.GetValue("ImagePath").ToString();
							text2 = Environment.ExpandEnvironmentVariables(text2);
						}
					}
					if (!string.IsNullOrEmpty(text2) && File.Exists(text2))
					{
						text = FileVersionInfo.GetVersionInfo(text2).FileVersion;
					}
				}
				catch
				{
				}
				if (string.IsNullOrEmpty(text))
				{
					text2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers\\Lenovo\\udc\\Service\\UDClientService.exe");
					if (Environment.Is64BitOperatingSystem)
					{
						text2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SysNative", "drivers\\Lenovo\\udc\\Service\\UDClientService.exe");
					}
					if (File.Exists(text2))
					{
						text = FileVersionInfo.GetVersionInfo(text2).FileVersion;
					}
				}
			}
			return text;
		}

		// Token: 0x04000036 RID: 54
		private readonly PluginRepository _pluginRepository;

		// Token: 0x04000037 RID: 55
		private readonly ISubscriptionManager _subscriptionManager;

		// Token: 0x04000038 RID: 56
		private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

		// Token: 0x04000039 RID: 57
		private List<App> _appList;

		// Token: 0x0400003A RID: 58
		private IDirectory _directory;

		// Token: 0x0400003B RID: 59
		private static readonly int _errorResponseCode = 1050;

		// Token: 0x0400003C RID: 60
		private ConcurrentDictionary<Guid, CancellationTokenSource> _requestCancelEventDictionary = new ConcurrentDictionary<Guid, CancellationTokenSource>();

		// Token: 0x0400003D RID: 61
		private List<UDCEntitledApp> _entitledAppList = new List<UDCEntitledApp>();

		// Token: 0x0400003E RID: 62
		private const int UdcErrorCodeForNoInternet = 27;

		// Token: 0x0400003F RID: 63
		private const int UdcErrorAPSMin = 29;

		// Token: 0x04000040 RID: 64
		private const int UdcErrorAPSMax = 60;

		// Token: 0x04000041 RID: 65
		private const int UdcErrorCodeDeviceNotRegistered = 62;

		// Token: 0x04000042 RID: 66
		private const string CommandNameGetAppsRedemptionCodes = "Get-AppsRedemptionCodes";

		// Token: 0x04000043 RID: 67
		private const string UdcGetAppsRedemptionCodesCommand = "GetAppsRedemptionCodes";

		// Token: 0x04000044 RID: 68
		private const string UDCSERVICENAME = "udcservice";

		// Token: 0x04000045 RID: 69
		private NETWORKLIST.NetworkListManager _networkListManager;
	}
}
