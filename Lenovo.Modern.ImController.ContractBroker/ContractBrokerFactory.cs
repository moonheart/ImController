using System;
using Lenovo.Modern.ImController.ContractBroker.Services;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Services.Contracts.SystemInformation.AppTag;
using Lenovo.Modern.Utilities.Patterns.Ioc;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Network;

namespace Lenovo.Modern.ImController.ContractBroker
{
	// Token: 0x0200000D RID: 13
	public static class ContractBrokerFactory
	{
		// Token: 0x06000075 RID: 117 RVA: 0x0000251C File Offset: 0x0000071C
		public static ContractBroker GetContractBrokerInstance()
		{
			try
			{
				if (ContractBrokerFactory._contractBroker == null)
				{
					ISubscriptionManager instance = SubscriptionManager.GetInstance(new NetworkAgent());
					IMachineInformationManager instance2 = MachineInformationManager.GetInstance();
					IAppAndTagManager instance3 = AppAndTagManager.GetInstance();
					PluginRepository pluginRepository = new PluginRepository();
					IRequestMapper requestMapper = new RequestMapper(instance, instance2, instance3, pluginRepository);
					IPluginManager pluginManager = InstanceContainer.GetInstance().Resolve<IPluginManager>();
					ImcContractHandler imcContractHandler = new ImcContractHandler(pluginRepository, instance);
					ContractBrokerFactory._contractBroker = new ContractBroker(requestMapper, pluginManager, pluginRepository, imcContractHandler, instance);
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to initialize contract broker instance");
			}
			return ContractBrokerFactory._contractBroker;
		}

		// Token: 0x04000035 RID: 53
		private static ContractBroker _contractBroker;
	}
}
