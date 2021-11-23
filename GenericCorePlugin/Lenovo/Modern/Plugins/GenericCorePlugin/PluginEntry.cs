using System;
using System.Threading;
using Lenovo.Modern.CoreTypes.Contracts.ActiveDirectory;
using Lenovo.Modern.CoreTypes.Contracts.AppLauncher;
using Lenovo.Modern.CoreTypes.Contracts.FileSystem;
using Lenovo.Modern.CoreTypes.Contracts.Registry;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation.MemoryInformation;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation.StorageInformation;
using Lenovo.Modern.CoreTypes.Events.SystemEvent;
using Lenovo.Modern.ImController.ImClient.Plugin;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Plugins.GenericCorePlugin
{
	// Token: 0x02000003 RID: 3
	public class PluginEntry : IPlugin
	{
		// Token: 0x06000003 RID: 3 RVA: 0x0000248C File Offset: 0x0000068C
		public PluginEntry()
		{
			Logger.Setup(new Logger.Configuration
			{
				LogIdentifier = "Plugins.GenericCorePlugin",
				FileSizeRollOverKb = 4096,
				IsEnabled = new bool?(true)
			});
			this._requestResponder = new RequestResponder();
			this._commandMapper = new CommandMapper();
			this._pluginEntryWrapper = new PluginEntryWrapper(this._commandMapper);
			Lenovo.Modern.CoreTypes.Contracts.SystemInformation.ContractConstants get = Lenovo.Modern.CoreTypes.Contracts.SystemInformation.ContractConstants.Get;
			SystemEventConstants get2 = SystemEventConstants.Get;
			this._commandMapper.RegisterRequestCommandHandler(Lenovo.Modern.CoreTypes.Contracts.SystemInformation.ContractConstants.Get.ContractName, Lenovo.Modern.CoreTypes.Contracts.SystemInformation.ContractConstants.Get.CommandNameGetMachineInformation, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.GetMachineInformationAsync));
			this._commandMapper.RegisterRequestCommandHandler(Lenovo.Modern.CoreTypes.Contracts.SystemInformation.ContractConstants.Get.ContractName, Lenovo.Modern.CoreTypes.Contracts.SystemInformation.ContractConstants.Get.CommandNameWriteMachineInformation, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.WriteMachineInformationAsync));
			this._commandMapper.RegisterRequestCommandHandler(Lenovo.Modern.CoreTypes.Contracts.Registry.ContractConstants.Get.ContractName, Lenovo.Modern.CoreTypes.Contracts.Registry.ContractConstants.Get.CommandNameGetRegistryKey, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.GetRegistryKeyAsync));
			this._commandMapper.RegisterRequestCommandHandler(Lenovo.Modern.CoreTypes.Contracts.Registry.ContractConstants.Get.ContractName, Lenovo.Modern.CoreTypes.Contracts.Registry.ContractConstants.Get.CommandNameSetRegistryKey, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.SetRegistryKeyAsync));
			this._commandMapper.RegisterRequestCommandHandler(Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.ContractConstants.Get.ContractName, Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.ContractConstants.Get.CommandNameGetAppAndTags, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.GetAppsAndTagsAsync));
			this._commandMapper.RegisterRequestCommandHandler(Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.ContractConstants.Get.ContractName, Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.ContractConstants.Get.CommandNameWriteAppAndTags, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.WriteAppsAndTagsAsync));
			this._commandMapper.RegisterRequestCommandHandler(Lenovo.Modern.CoreTypes.Contracts.FileSystem.ContractConstants.Get.ContractName, Lenovo.Modern.CoreTypes.Contracts.FileSystem.ContractConstants.Get.CommandNameGetDirectoryListing, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.GetDirectoryListingAsync));
			this._commandMapper.RegisterRequestCommandHandler(Lenovo.Modern.CoreTypes.Contracts.FileSystem.ContractConstants.Get.ContractName, Lenovo.Modern.CoreTypes.Contracts.FileSystem.ContractConstants.Get.CommandNamePerformActionItems, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.PerformActionItemsAsync));
			this._commandMapper.RegisterRequestCommandHandler(Lenovo.Modern.CoreTypes.Contracts.ActiveDirectory.ContractConstants.Get.ContractName, Lenovo.Modern.CoreTypes.Contracts.ActiveDirectory.ContractConstants.Get.CommandNameGetPolicies, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.GetActiveDirectoryAsync));
			this._commandMapper.RegisterRequestCommandHandler(Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.ContractName, Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.CommandNameLaunchDesktopApp, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.LaunchDesktopAppAsync));
			this._commandMapper.RegisterRequestCommandHandler(Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.ContractName, Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.CommandNameLaunchUniversalApp, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.LaunchUniversalAppAsync));
			this._commandMapper.RegisterRequestCommandHandler(Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.ContractName, Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.CommandNameLaunchControlPanelApp, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.LaunchControlPanelItemAppAsync));
			this._commandMapper.RegisterRequestCommandHandler(Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.ContractName, Lenovo.Modern.CoreTypes.Contracts.AppLauncher.ContractConstants.Get.CommandNameLaunchDocument, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.LaunchDocumentAsync));
			this._commandMapper.RegisterRequestCommandHandler(MemoryContractConstants.Get.ContractName, MemoryContractConstants.Get.CommandNameGetCapability, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.GetMemoryInformationCapabilityAsync));
			this._commandMapper.RegisterRequestCommandHandler(MemoryContractConstants.Get.ContractName, MemoryContractConstants.Get.CommandNameGetMemoryInformation, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.GetMemoryInformationAsync));
			this._commandMapper.RegisterRequestCommandHandler(StorageContractConstants.Get.ContractName, StorageContractConstants.Get.CommandNameGetCapability, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.GetStorageInformationCapabilityAsync));
			this._commandMapper.RegisterRequestCommandHandler(StorageContractConstants.Get.ContractName, StorageContractConstants.Get.CommandNameGetStorageInformation, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.GetStorageInformationAsync));
			this._commandMapper.RegisterRequestCommandHandler(StorageContractConstants.Get.ContractName, StorageContractConstants.Get.CommandNameWriteStorageInformation, new PluginEntryWrapper.PluginEntryRequestFunction(this._requestResponder.WriteStorageInformationAsync));
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000285E File Offset: 0x00000A5E
		public string HandleEvent(string eventReaction)
		{
			return this._pluginEntryWrapper.HandleEvent(eventReaction);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000286C File Offset: 0x00000A6C
		public string HandleAppRequest(string contractRequestXml, Func<string, bool> responseFunction, WaitHandle cancelEvent)
		{
			return this._pluginEntryWrapper.HandleAppRequest(contractRequestXml, responseFunction, cancelEvent);
		}

		// Token: 0x04000004 RID: 4
		private readonly RequestResponder _requestResponder;

		// Token: 0x04000005 RID: 5
		private readonly CommandMapper _commandMapper;

		// Token: 0x04000006 RID: 6
		private readonly PluginEntryWrapper _pluginEntryWrapper;
	}
}
