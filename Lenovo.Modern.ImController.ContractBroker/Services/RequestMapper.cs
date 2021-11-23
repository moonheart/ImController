using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;
using Lenovo.Modern.CoreTypes.Models;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.CoreTypes.Models.Subscription;
using Lenovo.Modern.CoreTypes.Services;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Services.Contracts.SystemInformation.AppTag;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.ContractBroker.Services
{
	// Token: 0x02000011 RID: 17
	public class RequestMapper : IRequestMapper
	{
		// Token: 0x0600009C RID: 156 RVA: 0x000039E8 File Offset: 0x00001BE8
		public RequestMapper(ISubscriptionManager subscriptionManager, IMachineInformationManager machineInformationManager, IAppAndTagManager appTagManager, IPluginRepository pluginRepository)
		{
			if (subscriptionManager == null || machineInformationManager == null || appTagManager == null || pluginRepository == null)
			{
				throw new ArgumentNullException("Cannot provide null argument for RequestMapper");
			}
			this._subscriptionManager = subscriptionManager;
			this._machineInformationManager = machineInformationManager;
			this._appTagManager = appTagManager;
			this._pluginRepository = pluginRepository;
			this._cachedMappings = new ConcurrentDictionary<string, Tuple<PackageInformation, ContractMapping, bool>>();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003A3B File Offset: 0x00001C3B
		public Task<bool> InitializeAsync()
		{
			return Task.FromResult<bool>(true);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003A44 File Offset: 0x00001C44
		public async Task<Tuple<PackageInformation, ContractMapping>> GetPluginMappingAsync(ContractRequest contractRequest, CancellationToken cancelToken)
		{
			if (contractRequest == null || string.IsNullOrWhiteSpace(contractRequest.Name))
			{
				Logger.Log(Logger.LogSeverity.Error, "Contract Request cannot be null for Plugin Mapping");
				throw new ArgumentNullException("Contract Request cannot be null for Plugin Mapping");
			}
			string contractName = contractRequest.Name;
			if (this._cachedMappings.ContainsKey(contractName))
			{
				if (this._cachedMappings[contractName].Item3)
				{
					try
					{
						PluginRepository.PluginInformation pluginPathWithPluginName = this._pluginRepository.GetPluginPathWithPluginName(this._cachedMappings[contractName].Item1.Name);
						if (pluginPathWithPluginName != null && pluginPathWithPluginName.PathToPlugin != null && File.Exists(pluginPathWithPluginName.PathToPlugin))
						{
							Logger.Log(Logger.LogSeverity.Information, "Dynamic applicability detected, but Old plugin {0} is still valid.", new object[] { this._cachedMappings[contractName].Item1.Name });
							return Tuple.Create<PackageInformation, ContractMapping>(this._cachedMappings[contractName].Item1, this._cachedMappings[contractName].Item2);
						}
						goto IL_1E7;
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "Exception happened while checking for dynamic applicable plugin. Old plugin {0} seems to be deleted and new is installed.", new object[] { this._cachedMappings[contractName].Item1.Name });
						goto IL_1E7;
					}
				}
				return Tuple.Create<PackageInformation, ContractMapping>(this._cachedMappings[contractName].Item1, this._cachedMappings[contractName].Item2);
			}
			IL_1E7:
			PackageSubscription packageSubscription = await this._subscriptionManager.GetSubscriptionAsync(cancelToken);
			PackageSubscription subscription = packageSubscription;
			if (subscription == null || subscription.PackageList == null || !subscription.PackageList.Any<Lenovo.Modern.ImController.Shared.Model.Packages.Package>())
			{
				Logger.Log(Logger.LogSeverity.Error, "Request mapper: Unable to retrieve valid subscription file.");
				throw new RequestMapperException("Request mapper: Unable to retrieve valid subscription file.")
				{
					ResponseCode = 510
				};
			}
			MachineInformation machineInformation = await this._machineInformationManager.GetMachineInformationAsync(cancelToken);
			if (machineInformation == null)
			{
				Logger.Log(Logger.LogSeverity.Error, "Request mapper: Unable to retrieve valid machine information.");
				throw new RequestMapperException("Request mapper: Unable to retrieve valid machine information.")
				{
					ResponseCode = 511
				};
			}
			IEnumerable<Lenovo.Modern.ImController.Shared.Model.Packages.Package> packagesThatSupportContract = subscription.PackageList.Where(delegate(Lenovo.Modern.ImController.Shared.Model.Packages.Package package)
			{
				IEnumerable<ContractMapping> enumerable2 = RequestMapper.FindMappingsForPackage(package, contractName);
				return enumerable2 != null && enumerable2.Any<ContractMapping>();
			});
			if (packagesThatSupportContract == null || !packagesThatSupportContract.Any<Lenovo.Modern.ImController.Shared.Model.Packages.Package>())
			{
				Logger.Log(Logger.LogSeverity.Error, "No packages found that implement the contract {0}", new object[] { contractName });
				throw new RequestMapperException(string.Format("No packages found that implement the contract {0}", contractName))
				{
					ResponseCode = 512
				};
			}
			AppAndTagCollection appAndTagCollection = await this._appTagManager.GetAppAndTagsAsync(cancelToken);
			if (appAndTagCollection == null)
			{
				Logger.Log(Logger.LogSeverity.Error, "RequestMapper: Unable to retrieve apps and tags");
			}
			Tuple<Lenovo.Modern.ImController.Shared.Model.Packages.Package, bool> tuple = RequestMapper.FindFirstApplicablePackage(packagesThatSupportContract, machineInformation, appAndTagCollection, this._pluginRepository);
			if (tuple == null || tuple.Item1 == null)
			{
				Logger.Log(Logger.LogSeverity.Error, "Request mapper: Unable to find applicable packages that implement contract: {0}", new object[] { contractName });
				throw new RequestMapperException(string.Format("Request mapper: Unable to find applicable packages that implement contract: {0}", contractName))
				{
					ResponseCode = 513
				};
			}
			IEnumerable<ContractMapping> enumerable = RequestMapper.FindMappingsForPackage(tuple.Item1, contractName);
			if (enumerable == null || !enumerable.Any<ContractMapping>())
			{
				Logger.Log(Logger.LogSeverity.Error, "Request mapper: Unable to find matching mappings for the contract: {0}", new object[] { contractName });
				throw new RequestMapperException(string.Format("Request mapper: Unable to find matching mappings for the contract: {0}", contractName))
				{
					ResponseCode = 514
				};
			}
			Tuple<PackageInformation, ContractMapping, bool> tuple2 = new Tuple<PackageInformation, ContractMapping, bool>(tuple.Item1.PackageInformation, enumerable.FirstOrDefault<ContractMapping>(), tuple.Item2);
			if (!this._cachedMappings.ContainsKey(contractName))
			{
				this._cachedMappings.TryAdd(contractName, tuple2);
			}
			else if (this._cachedMappings[contractName].Item3)
			{
				this._cachedMappings[contractName] = tuple2;
				Logger.Log(Logger.LogSeverity.Information, "Updating the cached contract mapping since dynamic contract mapping has been detected for {0}", new object[] { tuple2.Item1.Name });
			}
			return Tuple.Create<PackageInformation, ContractMapping>(tuple2.Item1, tuple2.Item2);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003A9C File Offset: 0x00001C9C
		private static Tuple<Lenovo.Modern.ImController.Shared.Model.Packages.Package, bool> FindFirstApplicablePackage(IEnumerable<Lenovo.Modern.ImController.Shared.Model.Packages.Package> listOfPackages, MachineInformation machineInformation, AppAndTagCollection appsAndTags, IPluginRepository pluginRepository)
		{
			Lenovo.Modern.ImController.Shared.Model.Packages.Package item = null;
			bool item2 = false;
			if (listOfPackages != null && machineInformation != null && pluginRepository != null)
			{
				string imControllerVersion = Constants.ImControllerVersion;
				foreach (Lenovo.Modern.ImController.Shared.Model.Packages.Package package in listOfPackages)
				{
					try
					{
						Filter filter = EligibilityFilter.GetFirstApplicableMatch(package.ApplicabilityFilter, machineInformation, appsAndTags, imControllerVersion) as Filter;
						if (filter != null)
						{
							bool? valueAsBool = filter.GetValueAsBool();
							if (valueAsBool != null && valueAsBool.GetValueOrDefault(false))
							{
								PluginRepository.PluginInformation pluginPathWithPluginName = pluginRepository.GetPluginPathWithPluginName(package.PackageInformation.Name);
								if (pluginPathWithPluginName != null && pluginPathWithPluginName.PathToPlugin != null && File.Exists(pluginPathWithPluginName.PathToPlugin))
								{
									item = package;
									if (package.SettingList != null)
									{
										AppSetting appSetting = package.SettingList.FirstOrDefault((AppSetting s) => !string.IsNullOrWhiteSpace(s.Key) && s.Key.Equals("ImController.Package.Replacement.Name"));
										if (appSetting != null)
										{
											item2 = true;
											Logger.Log(Logger.LogSeverity.Information, "Dynamic applicability detected, for plugin {0}", new object[] { package.PackageInformation.Name });
										}
									}
									break;
								}
								Logger.Log(Logger.LogSeverity.Warning, "Plugin {0} is applicable, but does not exit", new object[] { package.PackageInformation.Name ?? "NULL" });
							}
						}
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "Exception while checking package {0}", new object[] { package.PackageInformation.Name });
					}
				}
			}
			return Tuple.Create<Lenovo.Modern.ImController.Shared.Model.Packages.Package, bool>(item, item2);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003C48 File Offset: 0x00001E48
		private static IEnumerable<ContractMapping> FindMappingsForPackage(Lenovo.Modern.ImController.Shared.Model.Packages.Package package, string contractName)
		{
			IEnumerable<ContractMapping> result = null;
			if (package != null && package.ContractMappingList != null)
			{
				result = from packageMapping in package.ContractMappingList
					where packageMapping.Name != null && packageMapping.Name.Equals(contractName, StringComparison.InvariantCultureIgnoreCase)
					select packageMapping;
			}
			return result;
		}

		// Token: 0x0400004B RID: 75
		private readonly ISubscriptionManager _subscriptionManager;

		// Token: 0x0400004C RID: 76
		private readonly IMachineInformationManager _machineInformationManager;

		// Token: 0x0400004D RID: 77
		private readonly IAppAndTagManager _appTagManager;

		// Token: 0x0400004E RID: 78
		private readonly IPluginRepository _pluginRepository;

		// Token: 0x0400004F RID: 79
		private readonly ConcurrentDictionary<string, Tuple<PackageInformation, ContractMapping, bool>> _cachedMappings;
	}
}
