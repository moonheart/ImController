using System;
using System.Runtime.InteropServices;
using Lenovo.Modern.Utilities.Services.Logging;
using Microsoft.Win32;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x02000013 RID: 19
	public class MsSignability
	{
		// Token: 0x06000052 RID: 82 RVA: 0x0000399C File Offset: 0x00001B9C
		public static bool IsMsSignRequired()
		{
			if (MsSignability._isMsSignRequired == null)
			{
				MsSignability._isMsSignRequired = new bool?(MsSignability.CheckMsSignatureRequirement());
			}
			return MsSignability._isMsSignRequired != null && MsSignability._isMsSignRequired.Value;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000039D4 File Offset: 0x00001BD4
		private static bool CheckMsSignatureRequirement()
		{
			string text = "";
			MsSignability.WLDP_WINDOWS_LOCKDOWN_MODE wldp_WINDOWS_LOCKDOWN_MODE = MsSignability.WLDP_WINDOWS_LOCKDOWN_MODE.WLDP_WINDOWS_LOCKDOWN_MODE_UNLOCKED;
			int num;
			bool productInfo = MsSignability.GetProductInfo(Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor, 0, 0, out num);
			bool flag = 178 == num || 179 == num;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion"))
				{
					if (registryKey != null)
					{
						text = registryKey.GetValue("EditionID").ToString();
					}
					flag = flag || string.Compare(text, "cloud", StringComparison.InvariantCultureIgnoreCase) == 0;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "CheckMsSignatureRequirement: Exception getting edition info");
			}
			try
			{
				MsSignability.WldpQueryWindowsLockdownMode(out wldp_WINDOWS_LOCKDOWN_MODE);
				if (wldp_WINDOWS_LOCKDOWN_MODE != MsSignability.WLDP_WINDOWS_LOCKDOWN_MODE.WLDP_WINDOWS_LOCKDOWN_MODE_UNLOCKED)
				{
					flag = true;
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "CheckMsSignatureRequirement: Exception thrown calling WldpQueryWindowsLockdownMode.");
			}
			Logger.Log(Logger.LogSeverity.Information, string.Format("OS Product Version: GetProductInfo Success? '{0}' ProductNum? '0x{1}' Edition? '{2}' WLDP_WINDOWS_LOCKDOWN_MODE? '{3}' MSSigning ? '{4}'", new object[]
			{
				productInfo,
				num.ToString("X4"),
				text,
				wldp_WINDOWS_LOCKDOWN_MODE.ToString(),
				flag
			}));
			return flag;
		}

		// Token: 0x06000054 RID: 84
		[DllImport("kernel32.dll")]
		private static extern bool GetProductInfo(int dwOSMajorVersion, int dwOSMinorVersion, int dwSpMajorVersion, int dwSpMinorVersion, out int pdwReturnedProductType);

		// Token: 0x06000055 RID: 85
		[DllImport("Wldp.dll")]
		private static extern MsSignability.WLDP_WINDOWS_LOCKDOWN_MODE WldpQueryWindowsLockdownMode(out MsSignability.WLDP_WINDOWS_LOCKDOWN_MODE param);

		// Token: 0x0400005F RID: 95
		private static bool? _isMsSignRequired;

		// Token: 0x04000060 RID: 96
		public const int PRODUCT_CLOUD = 178;

		// Token: 0x04000061 RID: 97
		public const int PRODUCT_CLOUDN = 179;

		// Token: 0x02000051 RID: 81
		private enum WLDP_WINDOWS_LOCKDOWN_MODE
		{
			// Token: 0x04000126 RID: 294
			WLDP_WINDOWS_LOCKDOWN_MODE_UNLOCKED,
			// Token: 0x04000127 RID: 295
			WLDP_WINDOWS_LOCKDOWN_MODE_TRIAL,
			// Token: 0x04000128 RID: 296
			WLDP_WINDOWS_LOCKDOWN_MODE_LOCKED,
			// Token: 0x04000129 RID: 297
			WLDP_WINDOWS_LOCKDOWN_MODE_MAX
		}
	}
}
