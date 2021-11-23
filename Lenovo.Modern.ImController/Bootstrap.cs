using System;
using Lenovo.Modern.ImController.ImClient.Services;
using Lenovo.Modern.ImController.ImClient.Services.Umdf;
using Lenovo.Modern.ImController.PluginManager.Services;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.Utilities.Patterns.Ioc;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController
{
	// Token: 0x02000002 RID: 2
	public static class Bootstrap
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void RegisterComponents()
		{
			try
			{
				InstanceContainer instance = InstanceContainer.GetInstance();
				instance.RegisterInstance<IBrokerResponseAgent>(BrokerResponseAgent.GetInstance());
				instance.RegisterInstance<IPluginManager>(PluginManager.GetInstance());
			}
			catch (DeviceDriverMissingException)
			{
				throw;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Error while registering components in bootstrap");
			}
		}
	}
}
