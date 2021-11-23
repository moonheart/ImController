using System;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.ImController.Shared.Utilities
{
	// Token: 0x02000038 RID: 56
	public class VantageUtility
	{
		// Token: 0x06000199 RID: 409 RVA: 0x0000835C File Offset: 0x0000655C
		public static void RegisterVantageLaunchedAfterOOBE()
		{
			string path = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Lenovo\\ImController\\";
			IContainer container = ((IContainerSystem)new RegistrySystem()).LoadContainer(path);
			if (container != null)
			{
				container.SetValue("IsVantageLaunchedAfterOOBE", "1");
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00008390 File Offset: 0x00006590
		public static bool IsVantageLaunchedAfterOOBERegistered()
		{
			bool result = false;
			string path = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Lenovo\\ImController\\";
			IContainer container = ((IContainerSystem)new RegistrySystem()).LoadContainer(path);
			if (container != null && container.GetValue("IsVantageLaunchedAfterOOBE") != null && !string.IsNullOrEmpty(container.GetValue("IsVantageLaunchedAfterOOBE").GetValueAsString()))
			{
				result = true;
			}
			return result;
		}
	}
}
