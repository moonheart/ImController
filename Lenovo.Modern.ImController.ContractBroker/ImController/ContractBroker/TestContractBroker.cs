using System;
using System.Threading;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.Utilities.Patterns.Ioc;
using Lenovo.Modern.Utilities.Services.Logging;

namespace ImController.ContractBroker
{
	// Token: 0x02000002 RID: 2
	public class TestContractBroker
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public void Test(string requestXml)
		{
			IPluginManager pluginManager = InstanceContainer.GetInstance().Resolve<IPluginManager>();
			Logger.Log(Logger.LogSeverity.Information, "Test: Entry");
			Logger.Log(Logger.LogSeverity.Information, "Test: Calling StartPluginHost on PluginManager");
			string text = "Lenovo.Modern.Plugins.Core.MachineInformationPlugin.dll";
			Guid guid = Guid.NewGuid();
			string text2 = guid.ToString();
			Logger.Log(Logger.LogSeverity.Information, "Test: Calling MakepluginRequest on PluginManager. Plugin={0} GUID={1} Request={2}", new object[] { text, text2, requestXml });
			PluginRequestInformation pluginRequestInformation = new PluginRequestInformation();
			pluginRequestInformation.PluginName = "MachineInformation";
			guid = default(Guid);
			pluginRequestInformation.TaskId = guid.ToString();
			pluginRequestInformation.PluginType = PluginType.ManagedLibrary;
			pluginRequestInformation.RunAs = RunAs.System;
			pluginRequestInformation.Bitness = Bitness.X64;
			pluginRequestInformation.RequestType = RequestType.Application;
			pluginRequestInformation.PluginLocation = "C:\\Source\\win10-modern\\win32\\services\\ImController\\ImController.TestBench\\bin\\Debug\\Lenovo.Modern.Plugins.Core.MachineInformationPlugin.dll";
			pluginRequestInformation.ContractRequest = new ContractRequest
			{
				Command = new ContractCommandRequest
				{
					Name = "Get-MachineInformation",
					Parameter = "",
					RequestType = "sync"
				},
				Name = "MachineInformation"
			};
			PluginRequestInformation pluginInfo = pluginRequestInformation;
			Thread.Sleep(2000);
			pluginManager.MakePluginRequest(pluginInfo, CancellationToken.None);
			Thread.Sleep(30000);
			pluginManager.StopPluginHosts(false);
			Logger.Log(Logger.LogSeverity.Information, "Test: Exit");
		}
	}
}
