using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Lenovo.Modern.CoreTypes.Contracts.ActiveDirectory;
using Lenovo.Modern.CoreTypes.Contracts.AppLauncher;
using Lenovo.Modern.CoreTypes.Contracts.Capabilities.CapabilityResponse;
using Lenovo.Modern.CoreTypes.Contracts.FileSystem;
using Lenovo.Modern.CoreTypes.Contracts.Registry;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation.MemoryInformation;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation.StorageInformation;
using Lenovo.Modern.CoreTypes.Events.SystemEvent;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.Plugins.GenericCorePlugin.ActiveDirectory;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppLauncher;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag;
using Lenovo.Modern.Plugins.GenericCorePlugin.FileSystem;
using Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation;
using Lenovo.Modern.Plugins.GenericCorePlugin.MemoryInformation;
using Lenovo.Modern.Plugins.GenericCorePlugin.Registry;
using Lenovo.Modern.Plugins.GenericCorePlugin.StorageInformation;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;
using Microsoft.Win32;

namespace Lenovo.Modern.Plugins.GenericCorePlugin
{
	// Token: 0x02000004 RID: 4
	internal class RequestResponder
	{
		// Token: 0x06000006 RID: 6 RVA: 0x0000287C File Offset: 0x00000A7C
		public RequestResponder()
		{
			this._activeDirectoryAgent = new ActiveDirectiveAgent();
			this._appLauncherAgent = new AppLauncherAgent();
			this._appTagAgent = AppTagAgent.GetInstance();
			this._fileSystemAgent = new FileSystemAgent();
			this._performActionOnItemsAgent = new PerformActionOnItemsAgent();
			this._registryAgent = new RegistryAgent();
			this._machineInformationAgent = MachineInformationAgent.GetInstance();
			this._memoryInformationAgent = new MemoryInformationAgent();
			this._storageInformationAgent = new StorageInformationAgent();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000028F4 File Offset: 0x00000AF4
		public async Task RespondToAdEventAsync(EventReaction eventDetails)
		{
			if (eventDetails.Monitor == SystemEventConstants.Get.SystemEventMonitorName && eventDetails.Trigger == SystemEventConstants.Get.UserLoginTriggerName)
			{
				try
				{
					this._iContainerSystem = new RegistrySystem();
					IContainer container = this._iContainerSystem.LoadContainer(Path.Combine(Registry.LocalMachine.ToString(), Lenovo.Modern.Plugins.GenericCorePlugin.ActiveDirectory.Constants.ADLocation));
					if (container != null)
					{
						IEnumerable<string> subContainerNames = container.GetSubContainerNames();
						string empty = string.Empty;
						string text = string.Empty;
						foreach (string text2 in subContainerNames)
						{
							if (System.IO.Directory.Exists(Path.Combine(Environment.ExpandEnvironmentVariables(Lenovo.Modern.Plugins.GenericCorePlugin.ActiveDirectory.Constants.LocalEnvironmentVariable), Lenovo.Modern.Plugins.GenericCorePlugin.ActiveDirectory.Constants.Packages, text2)))
							{
								string contents = Serializer.Serialize<AppPoliciesResponse>(this._activeDirectoryAgent.GetPolicies(text2));
								text = Path.Combine(new string[]
								{
									Environment.ExpandEnvironmentVariables(Lenovo.Modern.Plugins.GenericCorePlugin.ActiveDirectory.Constants.LocalEnvironmentVariable),
									Lenovo.Modern.Plugins.GenericCorePlugin.ActiveDirectory.Constants.Packages,
									text2,
									Lenovo.Modern.Plugins.GenericCorePlugin.ActiveDirectory.Constants.LocalState,
									Lenovo.Modern.Plugins.GenericCorePlugin.ActiveDirectory.Constants.ADFile
								});
								string directoryName = Path.GetDirectoryName(text);
								if (!string.IsNullOrEmpty(directoryName) && !System.IO.Directory.Exists(directoryName))
								{
									System.IO.Directory.CreateDirectory(directoryName);
								}
								File.WriteAllText(text, contents);
							}
							Logger.Log(Logger.LogSeverity.Information, "Created AD policy file for " + text2 + " at location " + text);
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "Unable to respond to event");
				}
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002944 File Offset: 0x00000B44
		public async Task<BrokerResponseTask> GetActiveDirectoryAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			return await this.GetAndReplyWithActiveDirectory(request);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002994 File Offset: 0x00000B94
		private Task<BrokerResponseTask> GetAndReplyWithActiveDirectory(ContractRequest request)
		{
			AppPoliciesRequest appPoliciesRequest = Serializer.Deserialize<AppPoliciesRequest>(request.Command.Parameter);
			AppPoliciesResponse policies = this._activeDirectoryAgent.GetPolicies(appPoliciesRequest);
			string text = Serializer.Serialize<AppPoliciesResponse>(policies);
			BrokerResponseTask result = new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				ContractResponse = new ContractResponse
				{
					ContractVersion = Lenovo.Modern.CoreTypes.Contracts.ActiveDirectory.ContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = text,
						DataType = Lenovo.Modern.CoreTypes.Contracts.ActiveDirectory.ContractConstants.Get.DataTypeAppPoliciesResponse
					}
				}
			};
			if (policies != null)
			{
				XmlDocument xmlDocument = new XmlDocument();
				string empty = string.Empty;
				string empty2 = string.Empty;
				xmlDocument.LoadXml(text);
				string text2 = Path.Combine(new string[]
				{
					Environment.ExpandEnvironmentVariables(Lenovo.Modern.Plugins.GenericCorePlugin.ActiveDirectory.Constants.LocalEnvironmentVariable),
					Lenovo.Modern.Plugins.GenericCorePlugin.ActiveDirectory.Constants.Packages,
					appPoliciesRequest.PolicyInformation.AppName,
					Lenovo.Modern.Plugins.GenericCorePlugin.ActiveDirectory.Constants.LocalState,
					Lenovo.Modern.Plugins.GenericCorePlugin.ActiveDirectory.Constants.ADFile
				});
				if (!string.IsNullOrEmpty(text2))
				{
					string directoryName = Path.GetDirectoryName(text2);
					if (!string.IsNullOrEmpty(directoryName) && !System.IO.Directory.Exists(directoryName))
					{
						System.IO.Directory.CreateDirectory(directoryName);
					}
					else if (File.Exists(text2))
					{
						File.Delete(text2);
					}
					xmlDocument.Save(text2);
				}
			}
			return Task.FromResult<BrokerResponseTask>(result);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002AC8 File Offset: 0x00000CC8
		public Task<BrokerResponseTask> LaunchDesktopAppAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			BrokerResponseTask result;
			try
			{
				DesktopAppLaunchRequest requestXml = Serializer.Deserialize<DesktopAppLaunchRequest>(request.Command.Parameter);
				string text = Serializer.Serialize<AppLaunchResponse>(this._appLauncherAgent.LaunchDesktopApp(requestXml));
				result = new BrokerResponseTask
				{
					IsComplete = true,
					PercentageComplete = 100,
					ContractResponse = new ContractResponse
					{
						ContractVersion = Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.ContractVersion,
						Response = new ResponseData
						{
							Data = text.ToString(),
							DataType = Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.DataTypeDesktopAppLaunchRequest
						}
					},
					Error = new FailureData()
				};
			}
			catch (Exception)
			{
				throw;
			}
			return Task.FromResult<BrokerResponseTask>(result);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002B78 File Offset: 0x00000D78
		public Task<BrokerResponseTask> LaunchUniversalAppAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			UniversalAppLaunchRequest requestXml = Serializer.Deserialize<UniversalAppLaunchRequest>(request.Command.Parameter);
			string text = Serializer.Serialize<AppLaunchResponse>(this._appLauncherAgent.LaunchUniversalApp(requestXml));
			return Task.FromResult<BrokerResponseTask>(new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				ContractResponse = new ContractResponse
				{
					ContractVersion = Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = text.ToString(),
						DataType = Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.DataTypeUniversalAppLaunchRequest
					}
				},
				Error = new FailureData()
			});
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002C10 File Offset: 0x00000E10
		public Task<BrokerResponseTask> LaunchControlPanelItemAppAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			ControlPanelItemLaunchRequest requestXml = Serializer.Deserialize<ControlPanelItemLaunchRequest>(request.Command.Parameter);
			string text = Serializer.Serialize<AppLaunchResponse>(this._appLauncherAgent.LaunchContorlPanelItem(requestXml));
			return Task.FromResult<BrokerResponseTask>(new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				ContractResponse = new ContractResponse
				{
					ContractVersion = Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = text.ToString(),
						DataType = Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.DataTypeDesktopAppLaunchRequest
					}
				},
				Error = new FailureData()
			});
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002CA8 File Offset: 0x00000EA8
		public Task<BrokerResponseTask> LaunchDocumentAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			DocumentLaunchRequest request2 = Serializer.Deserialize<DocumentLaunchRequest>(request.Command.Parameter);
			string text = Serializer.Serialize<AppLaunchResponse>(this._appLauncherAgent.LaunchDocument(request2));
			return Task.FromResult<BrokerResponseTask>(new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				ContractResponse = new ContractResponse
				{
					ContractVersion = Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = text.ToString(),
						DataType = Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.DataTypeDocumentLaunchRequest
					}
				},
				Error = new FailureData()
			});
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002D40 File Offset: 0x00000F40
		private async Task RespondToAppTagEventAsync(EventReaction eventDetails)
		{
			if (eventDetails.Monitor == SystemEventConstants.Get.SystemEventMonitorName && eventDetails.Trigger == SystemEventConstants.Get.UserLoginTriggerName)
			{
				try
				{
					string pathToCompanionLocalState = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages\\E046963F.LenovoCompanion_k1h2ywk1493x8\\LocalState\\AppsAndTags.xml");
					string pathToSettingsLocalState = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages\\LenovoCorporation.LenovoSettings_4642shxvsv8s2\\LocalState\\AppsAndTags.xml");
					AppAndTagCollection appAndTagCollection = await this._appTagAgent.GetAppsAndTags(false, false, false);
					if (appAndTagCollection != null)
					{
						string fileContents = Serializer.Serialize<AppAndTagCollection>(appAndTagCollection);
						this.WriteToFile(pathToCompanionLocalState, fileContents, false);
						this.WriteToFile(pathToSettingsLocalState, fileContents, false);
					}
					pathToCompanionLocalState = null;
					pathToSettingsLocalState = null;
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "Unable to respond to event request for AppTag required");
				}
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002D90 File Offset: 0x00000F90
		public async Task<BrokerResponseTask> GetAppsAndTagsAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			return await this.GetAndReplyWithAppsAndTagsAsync(request);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002DE0 File Offset: 0x00000FE0
		public async Task<BrokerResponseTask> WriteAppsAndTagsAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			return await this.GetAndWriteAppsAndTagsAsync(request);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002E30 File Offset: 0x00001030
		private async Task<BrokerResponseTask> GetAndReplyWithAppsAndTagsAsync(ContractRequest request)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			string value;
			if (request == null)
			{
				value = null;
			}
			else
			{
				ContractCommandRequest command = request.Command;
				value = ((command != null) ? command.Parameter : null);
			}
			if (!string.IsNullOrWhiteSpace(value))
			{
				AppsAndTagsRequest appsAndTagsRequest = Serializer.Deserialize<AppsAndTagsRequest>(request.Command.Parameter);
				flag = appsAndTagsRequest != null && appsAndTagsRequest.IgnoreCache;
				flag2 = appsAndTagsRequest != null && appsAndTagsRequest.noApps;
				flag3 = appsAndTagsRequest != null && appsAndTagsRequest.noTags;
			}
			Logger.Log(Logger.LogSeverity.Information, "GetAndReplyWithAppsAndTagsAsync : Request received ignorePluginCache: {0}, noApps: {1}, noTags: {2}", new object[] { flag, flag2, flag3 });
			string data = Serializer.Serialize<AppAndTagCollection>(await this._appTagAgent.GetAppsAndTags(flag, flag2, flag3));
			return new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				ContractResponse = new ContractResponse
				{
					ContractVersion = Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.ContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = data,
						DataType = Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.ContractConstants.Get.DataTypeAppAndTagsResponse
					}
				}
			};
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002E80 File Offset: 0x00001080
		private async Task<BrokerResponseTask> GetAndWriteAppsAndTagsAsync(ContractRequest request)
		{
			Lenovo.Modern.CoreTypes.Models.AppTagList.OutputLocationRequest requestXml = Serializer.Deserialize<Lenovo.Modern.CoreTypes.Models.AppTagList.OutputLocationRequest>(request.Command.Parameter);
			string data = Serializer.Serialize<Lenovo.Modern.CoreTypes.Models.AppTagList.OutputLocationResponse>(await this._appTagAgent.WriteAppsAndTags(requestXml));
			return new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				ContractResponse = new ContractResponse
				{
					ContractVersion = Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.ContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = data,
						DataType = Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.ContractConstants.Get.DataTypeAppAndTagsResponse
					}
				}
			};
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002ED0 File Offset: 0x000010D0
		private void WriteToFile(string fullPathToFile, string fileContents, bool createDirIfNotExists)
		{
			try
			{
				string directoryName = Path.GetDirectoryName(fullPathToFile);
				bool flag = false;
				if (!string.IsNullOrEmpty(directoryName))
				{
					flag = System.IO.Directory.Exists(directoryName);
					if (!flag && createDirIfNotExists)
					{
						System.IO.Directory.CreateDirectory(directoryName);
						flag = true;
					}
				}
				if (flag)
				{
					File.WriteAllText(fullPathToFile, fileContents);
					Logger.Log(Logger.LogSeverity.Information, "Writing machine info to : {0}", new object[] { fullPathToFile });
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to write to {0}", new object[] { fullPathToFile });
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002F4C File Offset: 0x0000114C
		public async Task<BrokerResponseTask> GetDirectoryListingAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			return await this.GetAndReplyDirectoryListing(request);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002F9C File Offset: 0x0000119C
		public async Task<BrokerResponseTask> PerformActionItemsAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			return await this.GetAndPerformActions(request);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002FEC File Offset: 0x000011EC
		private async Task<BrokerResponseTask> GetAndReplyDirectoryListing(ContractRequest request)
		{
			BrokerResponseTask brokerResponseTask = new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				Error = new FailureData()
			};
			try
			{
				DirectoryListingRequest requestXml = Serializer.Deserialize<DirectoryListingRequest>(request.Command.Parameter);
				string data = Serializer.Serialize<DirectoryListingResponse>(await this._fileSystemAgent.GetDirectoryListing(requestXml));
				brokerResponseTask.ContractResponse = new ContractResponse
				{
					ContractVersion = Lenovo.Modern.CoreTypes.Contracts.FileSystem.ContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = data,
						DataType = Lenovo.Modern.CoreTypes.Contracts.FileSystem.ContractConstants.Get.DataTypeDirectoryListingResponse
					}
				};
				List<Exception> list = this._fileSystemAgent.exceptionList();
				int num = 1;
				foreach (Exception ex in list)
				{
					Logger.Log(Logger.LogSeverity.Warning, ex.Message);
					FailureData error = brokerResponseTask.Error;
					error.ResultDescription = string.Concat(new object[] { error.ResultDescription, num, ". ", ex.Message });
					num++;
				}
			}
			catch (DirectoryNotFoundException ex2)
			{
				Logger.Log(Logger.LogSeverity.Warning, "Specified Directory not found when calling the FileSystemPlugin:");
				brokerResponseTask.Error.ResultDescription = ex2.Message;
				return brokerResponseTask;
			}
			catch (Exception ex3)
			{
				Logger.Log(ex3, "Exception while running FileSystemPlugin:");
				brokerResponseTask.Error.ResultDescription = ex3.Message;
				return brokerResponseTask;
			}
			return brokerResponseTask;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000303C File Offset: 0x0000123C
		private async Task<BrokerResponseTask> GetAndPerformActions(ContractRequest request)
		{
			BrokerResponseTask brokerResponseTask = new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				Error = new FailureData()
			};
			try
			{
				ItemActionRequest requestXml = Serializer.Deserialize<ItemActionRequest>(request.Command.Parameter);
				string data = Serializer.Serialize<ItemActionResponse>(await this._performActionOnItemsAgent.PerformActionOnItems(requestXml));
				brokerResponseTask.ContractResponse = new ContractResponse
				{
					ContractVersion = Lenovo.Modern.CoreTypes.Contracts.FileSystem.ContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = data,
						DataType = Lenovo.Modern.CoreTypes.Contracts.FileSystem.ContractConstants.Get.DataTypeItemActionRequest
					}
				};
			}
			catch (FileNotFoundException ex)
			{
				Logger.Log(Logger.LogSeverity.Warning, "Specified Directory not found when calling FileSystemPlugin:");
				brokerResponseTask.Error.ResultDescription = ex.Message;
				return brokerResponseTask;
			}
			catch (DirectoryNotFoundException ex2)
			{
				Logger.Log(Logger.LogSeverity.Warning, "Directory not found when calling FileSystemPlugin:");
				brokerResponseTask.Error.ResultDescription = ex2.Message;
				return brokerResponseTask;
			}
			catch (UnauthorizedAccessException ex3)
			{
				Logger.Log(Logger.LogSeverity.Warning, "Access Denied:");
				brokerResponseTask.Error.ResultDescription = ex3.Message;
				return brokerResponseTask;
			}
			catch (Exception ex4)
			{
				Logger.Log(ex4, "Exception when calling FileSystemPlugin: ");
				brokerResponseTask.Error.ResultDescription = ex4.Message;
				return brokerResponseTask;
			}
			return brokerResponseTask;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000308C File Offset: 0x0000128C
		public async Task<BrokerResponseTask> GetRegistryKeyAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			return await this.GetAndReplyWithRegistry(request);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000030DC File Offset: 0x000012DC
		public async Task<BrokerResponseTask> SetRegistryKeyAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			return await this.GetAndSetRegistry(request);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000312C File Offset: 0x0000132C
		private Task<BrokerResponseTask> GetAndReplyWithRegistry(ContractRequest request)
		{
			BrokerResponseTask brokerResponseTask = new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				Error = new FailureData()
			};
			try
			{
				KeyChildrenRequest requestXml = Serializer.Deserialize<KeyChildrenRequest>(request.Command.Parameter);
				string data = Serializer.Serialize<KeyChildrenResponse>(this._registryAgent.GetKeyChildren(requestXml));
				brokerResponseTask.ContractResponse = new ContractResponse
				{
					ContractVersion = Lenovo.Modern.CoreTypes.Contracts.Registry.ContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = data,
						DataType = Lenovo.Modern.CoreTypes.Contracts.Registry.ContractConstants.Get.DataTypeRegistry
					}
				};
				List<Exception> list = this._registryAgent.exceptionList();
				int num = 1;
				foreach (Exception ex in list)
				{
					Logger.Log(Logger.LogSeverity.Warning, ex.Message);
					FailureData error = brokerResponseTask.Error;
					error.ResultDescription = string.Concat(new object[] { error.ResultDescription, num, ". ", ex.Message });
					num++;
				}
			}
			catch (UnauthorizedAccessException ex2)
			{
				Logger.Log(Logger.LogSeverity.Warning, "Access Denied:");
				brokerResponseTask.Error.ResultDescription = ex2.Message;
				return Task.FromResult<BrokerResponseTask>(brokerResponseTask);
			}
			catch (KeyNotFoundException ex3)
			{
				Logger.Log(Logger.LogSeverity.Warning, "Registry key not exists");
				brokerResponseTask.Error.ResultDescription = ex3.Message;
				return Task.FromResult<BrokerResponseTask>(brokerResponseTask);
			}
			catch (Exception ex4)
			{
				Logger.Log(ex4, "Registry key not exists or Invalid Key Location");
				brokerResponseTask.Error.ResultDescription = ex4.Message;
				return Task.FromResult<BrokerResponseTask>(brokerResponseTask);
			}
			return Task.FromResult<BrokerResponseTask>(brokerResponseTask);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000032FC File Offset: 0x000014FC
		private Task<BrokerResponseTask> GetAndSetRegistry(ContractRequest request)
		{
			BrokerResponseTask brokerResponseTask = new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				Error = new FailureData()
			};
			try
			{
				KeyChildrenRequest keyChildrenRequest = Serializer.Deserialize<KeyChildrenRequest>(request.Command.Parameter);
				this._registryAgent.SetKeyChildren(keyChildrenRequest);
				string data = Serializer.Serialize<KeyChildrenResponse>(this._registryAgent.GetKeyChildren(keyChildrenRequest));
				brokerResponseTask.ContractResponse = new ContractResponse
				{
					ContractVersion = Lenovo.Modern.CoreTypes.Contracts.Registry.ContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = data,
						DataType = Lenovo.Modern.CoreTypes.Contracts.Registry.ContractConstants.Get.DataTypeRegistry
					}
				};
				List<Exception> list = this._registryAgent.exceptionList();
				int num = 1;
				foreach (Exception ex in list)
				{
					Logger.Log(Logger.LogSeverity.Warning, ex.Message);
					FailureData error = brokerResponseTask.Error;
					error.ResultDescription = string.Concat(new object[] { error.ResultDescription, num, ". ", ex.Message });
					num++;
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "Throws exception when handling the Request");
				brokerResponseTask.Error.ResultDescription = ex2.Message;
				return Task.FromResult<BrokerResponseTask>(brokerResponseTask);
			}
			return Task.FromResult<BrokerResponseTask>(brokerResponseTask);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003470 File Offset: 0x00001670
		private async Task RespondToMachineInfoEvent(EventReaction eventDetails)
		{
			if ((eventDetails.Monitor == SystemEventConstants.Get.SystemEventMonitorName && eventDetails.Trigger == SystemEventConstants.Get.UserLoginTriggerName) || (eventDetails.Monitor == "ImControllerServiceEventMonitor" && eventDetails.Trigger == "imc-startup"))
			{
				try
				{
					string fileContents = Serializer.Serialize<MachineInformation>(await this._machineInformationAgent.GetMachineInformation());
					string fullPathToFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages\\E046963F.LenovoCompanion_k1h2ywk1493x8\\LocalState\\MachineInformation.xml");
					string fullPathToFile2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages\\LenovoCorporation.LenovoSettings_4642shxvsv8s2\\LocalState\\MachineInformation.xml");
					this.MachineInformationWriteToFile(fullPathToFile, fileContents, false);
					this.MachineInformationWriteToFile(fullPathToFile2, fileContents, false);
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "Unable to respond to event for MachineInformation required");
				}
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000034C0 File Offset: 0x000016C0
		public async Task<BrokerResponseTask> GetMachineInformationAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			return await this.GetAndReplyWithMachineInformationAsync(request);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003510 File Offset: 0x00001710
		public async Task<BrokerResponseTask> WriteMachineInformationAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			return await this.GetAndWriteMachineInformationToFolderAsync(request);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00003560 File Offset: 0x00001760
		private void MachineInformationWriteToFile(string fullPathToFile, string fileContents, bool createDirIfNotExists)
		{
			try
			{
				string directoryName = Path.GetDirectoryName(fullPathToFile);
				bool flag = false;
				if (!string.IsNullOrEmpty(directoryName))
				{
					flag = System.IO.Directory.Exists(directoryName);
					if (!flag && createDirIfNotExists)
					{
						System.IO.Directory.CreateDirectory(directoryName);
						flag = true;
					}
				}
				if (flag)
				{
					File.WriteAllText(fullPathToFile, fileContents);
					Logger.Log(Logger.LogSeverity.Information, "Writing machine info to : {0}", new object[] { fullPathToFile });
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to write to {0}", new object[] { fullPathToFile });
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000035DC File Offset: 0x000017DC
		private async Task<BrokerResponseTask> GetAndReplyWithMachineInformationAsync(ContractRequest request)
		{
			string data = Serializer.Serialize<MachineInformation>(await this._machineInformationAgent.GetMachineInformation());
			return new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				ContractResponse = new ContractResponse
				{
					ContractVersion = Lenovo.Modern.CoreTypes.Contracts.SystemInformation.ContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = data,
						DataType = Lenovo.Modern.CoreTypes.Contracts.SystemInformation.ContractConstants.Get.DataTypeMachineInformation
					}
				}
			};
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003624 File Offset: 0x00001824
		private async Task<BrokerResponseTask> GetAndWriteMachineInformationToFolderAsync(ContractRequest request)
		{
			string contents = Serializer.Serialize<MachineInformation>(await this._machineInformationAgent.GetMachineInformation());
			string text = Path.Combine(Serializer.Deserialize<Lenovo.Modern.CoreTypes.Contracts.SystemInformation.OutputLocationRequest>(request.Command.Parameter).Location, Lenovo.Modern.CoreTypes.Contracts.SystemInformation.ContractConstants.Get.FileNameMachineInformation);
			File.WriteAllText(text, contents);
			string data = Serializer.Serialize<Lenovo.Modern.CoreTypes.Contracts.SystemInformation.OutputLocationResponse>(new Lenovo.Modern.CoreTypes.Contracts.SystemInformation.OutputLocationResponse
			{
				Success = true.ToString(),
				Location = text
			});
			return new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				ContractResponse = new ContractResponse
				{
					ContractVersion = Lenovo.Modern.CoreTypes.Contracts.SystemInformation.ContractConstants.Get.ContractName,
					Response = new ResponseData
					{
						Data = data,
						DataType = Lenovo.Modern.CoreTypes.Contracts.SystemInformation.ContractConstants.Get.DataTypeOutputLocationResponse
					}
				}
			};
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003674 File Offset: 0x00001874
		public async Task<BrokerResponseTask> GetMemoryInformationAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			return await this.GetAndReplyWithMemoryInformation(request);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000036C4 File Offset: 0x000018C4
		public async Task<BrokerResponseTask> GetMemoryInformationCapabilityAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			return await this.GetAndWReplyWithMemoryInformationCapibilityInformation(request);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003714 File Offset: 0x00001914
		private Task<BrokerResponseTask> GetAndReplyWithMemoryInformation(ContractRequest request)
		{
			string data = Serializer.Serialize<MemoryInformationResponse>(this._memoryInformationAgent.GetMemoryInformation());
			return Task.FromResult<BrokerResponseTask>(new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				ContractResponse = new ContractResponse
				{
					ContractVersion = MemoryContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = data,
						DataType = MemoryContractConstants.Get.DataTypeMemoryInformation
					}
				}
			});
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00003788 File Offset: 0x00001988
		private Task<BrokerResponseTask> GetAndWReplyWithMemoryInformationCapibilityInformation(ContractRequest request)
		{
			string data = Serializer.Serialize<CapabilityResponse>(this._memoryInformationAgent.GetCapability());
			return Task.FromResult<BrokerResponseTask>(new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				ContractResponse = new ContractResponse
				{
					ContractVersion = MemoryContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = data,
						DataType = MemoryContractConstants.Get.DataTypeCapability
					}
				}
			});
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000037FC File Offset: 0x000019FC
		public async Task<BrokerResponseTask> GetStorageInformationAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			return await this.GetAndReplyWithStorageInformationAsync(request);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000384C File Offset: 0x00001A4C
		public async Task<BrokerResponseTask> WriteStorageInformationAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			return await this.GetAndWriteStorageInformationAsync(request);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000389C File Offset: 0x00001A9C
		public async Task<BrokerResponseTask> GetStorageInformationCapabilityAsync(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken)
		{
			return await this.GetAndWReplyWithStorageInformationCapibility(request);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000038EC File Offset: 0x00001AEC
		private Task<BrokerResponseTask> GetAndReplyWithStorageInformationAsync(ContractRequest request)
		{
			string data = Serializer.Serialize<StorageInformationResponse>(this._storageInformationAgent.GetStorageInformation());
			return Task.FromResult<BrokerResponseTask>(new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				ContractResponse = new ContractResponse
				{
					ContractVersion = StorageContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = data,
						DataType = StorageContractConstants.Get.DataTypeStorageInformation
					}
				}
			});
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003960 File Offset: 0x00001B60
		private Task<BrokerResponseTask> GetAndWriteStorageInformationAsync(ContractRequest request)
		{
			string data = Serializer.Serialize<StorageInformationResponse>(this._storageInformationAgent.GetStorageInformation());
			return Task.FromResult<BrokerResponseTask>(new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				ContractResponse = new ContractResponse
				{
					ContractVersion = StorageContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = data,
						DataType = StorageContractConstants.Get.DataTypeStorageInformation
					}
				}
			});
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000039D4 File Offset: 0x00001BD4
		private Task<BrokerResponseTask> GetAndWReplyWithStorageInformationCapibility(ContractRequest request)
		{
			string data = Serializer.Serialize<CapabilityResponse>(this._storageInformationAgent.GetCapability());
			return Task.FromResult<BrokerResponseTask>(new BrokerResponseTask
			{
				IsComplete = true,
				PercentageComplete = 100,
				ContractResponse = new ContractResponse
				{
					ContractVersion = StorageContractConstants.Get.ContractVersion,
					Response = new ResponseData
					{
						Data = data,
						DataType = StorageContractConstants.Get.DataTypeCapability
					}
				}
			});
		}

		// Token: 0x04000007 RID: 7
		private readonly ActiveDirectiveAgent _activeDirectoryAgent;

		// Token: 0x04000008 RID: 8
		private string strPath;

		// Token: 0x04000009 RID: 9
		private IContainerSystem _iContainerSystem;

		// Token: 0x0400000A RID: 10
		private readonly AppLauncherAgent _appLauncherAgent;

		// Token: 0x0400000B RID: 11
		private readonly AppTagAgent _appTagAgent;

		// Token: 0x0400000C RID: 12
		private readonly FileSystemAgent _fileSystemAgent;

		// Token: 0x0400000D RID: 13
		private readonly PerformActionOnItemsAgent _performActionOnItemsAgent;

		// Token: 0x0400000E RID: 14
		private readonly RegistryAgent _registryAgent;

		// Token: 0x0400000F RID: 15
		private readonly MachineInformationAgent _machineInformationAgent;

		// Token: 0x04000010 RID: 16
		private readonly MemoryInformationAgent _memoryInformationAgent;

		// Token: 0x04000011 RID: 17
		private readonly StorageInformationAgent _storageInformationAgent;
	}
}
