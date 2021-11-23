using System;
using Microsoft.Win32;

namespace Lenovo.Modern.ImController.Shared.Utilities
{
	// Token: 0x02000037 RID: 55
	public class UserUtility
	{
		// Token: 0x06000197 RID: 407 RVA: 0x0000829C File Offset: 0x0000649C
		public static bool IsOSReadyForUser(string sid)
		{
			bool flag = false;
			try
			{
				using (RegistryKey registryKey = Registry.Users.OpenSubKey(Constants.AppReadinessImcPath + "\\" + sid))
				{
					flag = registryKey != null;
				}
			}
			catch (Exception)
			{
			}
			if (!flag)
			{
				try
				{
					string name = string.Format("{0}\\{1}", sid, Constants.AppReadinessKeyPath);
					using (RegistryKey registryKey2 = Registry.Users.OpenSubKey(name))
					{
						if (registryKey2 != null)
						{
							flag = (int)registryKey2.GetValue("AppReadinessLogonComplete") == 1;
						}
					}
				}
				catch (Exception)
				{
				}
			}
			return flag;
		}
	}
}
