using System;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation.Services
{
	// Token: 0x02000012 RID: 18
	public class MTMAgent
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x00007218 File Offset: 0x00005418
		public string GetMTM(string mtmFromBios)
		{
			string result = string.Empty;
			try
			{
				if (!string.IsNullOrWhiteSpace(mtmFromBios))
				{
					result = mtmFromBios;
				}
				else
				{
					result = this.GetMTMFromRegistry();
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown getting the MTM from the BIOS and/or Registry");
			}
			return result;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00007260 File Offset: 0x00005460
		private string GetMTMFromRegistry()
		{
			string result = string.Empty;
			IContainer container = new RegistrySystem().LoadContainer(Constants.HardwareConfigRegistryKey);
			if (container != null)
			{
				IContainerValue value = container.GetValue("SystemProductName");
				if (value != null)
				{
					result = value.GetValueAsString();
				}
			}
			return result;
		}
	}
}
