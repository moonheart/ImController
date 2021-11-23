using System;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.Utilities.Services.Logging;
using Microsoft.Win32;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x0200000C RID: 12
	public class InstallMethod
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00002F07 File Offset: 0x00001107
		public static bool IsMsiInstall()
		{
			return false;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002F0A File Offset: 0x0000110A
		public static bool IsMsiToInfBridgeEnabled(PackageSubscription subscription)
		{
			return subscription.Service.DownloadLocation != null && subscription.Service.DownloadLocation.ToLower().Contains(".exe");
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002F38 File Offset: 0x00001138
		private static bool CheckMsiInstallation()
		{
			bool flag = false;
			if (Environment.Is64BitOperatingSystem)
			{
				try
				{
					RegistryKey registryKey2;
					RegistryKey registryKey = (registryKey2 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64));
					try
					{
						using (RegistryKey registryKey3 = registryKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{C2E5CA37-C862-4A69-AC6D-24F450A20C16}"))
						{
							flag = registryKey3 != null;
						}
					}
					finally
					{
						if (registryKey2 != null)
						{
							((IDisposable)registryKey2).Dispose();
						}
					}
					goto IL_74;
				}
				catch (Exception)
				{
					goto IL_74;
				}
			}
			try
			{
				using (RegistryKey registryKey4 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{884BAF97-AC8D-463E-846A-47DD41866A19}"))
				{
					flag = registryKey4 != null;
				}
			}
			catch (Exception)
			{
			}
			IL_74:
			Logger.Log(Logger.LogSeverity.Information, string.Format("CheckMsiInstallation: IsMsiInstall? '{0}'", flag));
			return flag;
		}

		// Token: 0x0400004D RID: 77
		private static bool? _isMsiInstall;
	}
}
