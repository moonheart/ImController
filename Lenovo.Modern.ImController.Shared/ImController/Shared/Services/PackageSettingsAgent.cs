using System;
using System.Collections.Generic;
using System.Linq;
using Lenovo.Modern.CoreTypes.Models.Subscription;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x02000016 RID: 22
	public class PackageSettingsAgent
	{
		// Token: 0x06000063 RID: 99 RVA: 0x000044C0 File Offset: 0x000026C0
		public static Dictionary<string, string> GetPackageUapAssociationIds(Lenovo.Modern.ImController.Shared.Model.Packages.Package package)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			try
			{
				if (package.SettingList != null)
				{
					foreach (AppSetting appSetting in (from s in package.SettingList
						where !string.IsNullOrWhiteSpace(s.Key) && s.Key.Equals("ImController.Association.Uap") && !string.IsNullOrWhiteSpace(s.Value)
						select s).ToList<AppSetting>())
					{
						if (!dictionary.ContainsKey(appSetting.Value))
						{
							dictionary.Add(appSetting.Value, appSetting.AppVersion);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "GetPackageUapAssociationId: Exception occured while checking for appId setting");
			}
			return dictionary;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004580 File Offset: 0x00002780
		public static bool DoesPackageUpdateRequireReboot(Lenovo.Modern.ImController.Shared.Model.Packages.Package package)
		{
			bool result = false;
			try
			{
				if (package.SettingList != null)
				{
					AppSetting appSetting = package.SettingList.FirstOrDefault((AppSetting s) => !string.IsNullOrWhiteSpace(s.Key) && s.Key.Equals("ImController.Package.UpdateRequiresReboot"));
					if (appSetting != null)
					{
						Logger.Log(Logger.LogSeverity.Information, "DoesPackageUpdateRequireReboot: Package update require reboot");
						result = true;
					}
				}
			}
			catch (Exception)
			{
				Logger.Log(Logger.LogSeverity.Information, "DoesPackageUpdateRequireReboot: Exception occured while checking for updaterequiresreboot");
			}
			return result;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000045F8 File Offset: 0x000027F8
		public static bool DoesPackageInstallRequireReboot(Lenovo.Modern.ImController.Shared.Model.Packages.Package package)
		{
			bool result = false;
			try
			{
				if (package.SettingList != null)
				{
					AppSetting appSetting = package.SettingList.FirstOrDefault((AppSetting s) => !string.IsNullOrWhiteSpace(s.Key) && s.Key.Equals("ImController.Package.InstallRequiresReboot"));
					if (appSetting != null)
					{
						Logger.Log(Logger.LogSeverity.Information, "DoesPackageInstallRequireReboot: Package installation requires reboot");
						result = true;
					}
				}
			}
			catch (Exception)
			{
				Logger.Log(Logger.LogSeverity.Information, "DoesPackageInstallRequireReboot: Exception occured while checking for InstallRequiresReboot");
			}
			return result;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004670 File Offset: 0x00002870
		public static bool DoesPackageHaveDeviceAssociation(Lenovo.Modern.ImController.Shared.Model.Packages.Package package)
		{
			bool result = false;
			try
			{
				if (package.SettingList != null)
				{
					AppSetting appSetting = package.SettingList.FirstOrDefault((AppSetting s) => !string.IsNullOrWhiteSpace(s.Key) && s.Key.Equals("ImController.Association.Device"));
					if (appSetting != null)
					{
						Logger.Log(Logger.LogSeverity.Information, "DoesPackageHaveDeviceAssociation: Package {0} has device associationt", new object[] { package.PackageInformation.Name });
						result = true;
					}
				}
			}
			catch (Exception)
			{
				Logger.Log(Logger.LogSeverity.Information, "DoesPackageInstallRequireReboot: Exception occured while checking for InstallRequiresReboot");
			}
			return result;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000046FC File Offset: 0x000028FC
		public static bool IsPackageUpdateDisabled(Lenovo.Modern.ImController.Shared.Model.Packages.Package package)
		{
			bool result = false;
			try
			{
				if (package.SettingList != null)
				{
					AppSetting appSetting = package.SettingList.FirstOrDefault((AppSetting s) => !string.IsNullOrWhiteSpace(s.Key) && s.Key.Equals("ImController.Package.DisableUpdate"));
					if (appSetting != null)
					{
						Logger.Log(Logger.LogSeverity.Information, "IsPackageUpdateDisabled: Package installation/updation is disabled");
						result = true;
					}
				}
			}
			catch (Exception)
			{
				Logger.Log(Logger.LogSeverity.Information, "IsPackageUpdateDisabled: Exception occured while checking for update requirement");
			}
			return result;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004774 File Offset: 0x00002974
		public static bool IsForcedInstallationEnabled(Lenovo.Modern.ImController.Shared.Model.Packages.Package package)
		{
			bool result = false;
			try
			{
				if (package.SettingList != null)
				{
					AppSetting appSetting = package.SettingList.FirstOrDefault((AppSetting s) => !string.IsNullOrWhiteSpace(s.Key) && s.Key.Equals("ImController.Package.ForcedInstallation"));
					if (appSetting != null)
					{
						Logger.Log(Logger.LogSeverity.Information, "IsFirstUpdateEnabled: Package installation/updation is enabled");
						result = true;
					}
				}
			}
			catch (Exception)
			{
				Logger.Log(Logger.LogSeverity.Information, "IsFirstUpdateEnabled: Exception occured while checking for update requirement");
			}
			return result;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000047EC File Offset: 0x000029EC
		public static List<Version> GetDisabledVersions(Lenovo.Modern.ImController.Shared.Model.Packages.Package package)
		{
			List<Version> result = new List<Version>();
			try
			{
				if (package.SettingList != null)
				{
					List<AppSetting> list = (from s in package.SettingList
						where !string.IsNullOrWhiteSpace(s.Key) && s.Key.Equals("ImController.Package.DisabledVersion")
						select s).ToList<AppSetting>();
					if (list != null)
					{
						result = (from p in list
							select Version.Parse(p.Value)).ToList<Version>();
					}
				}
			}
			catch (Exception)
			{
				Logger.Log(Logger.LogSeverity.Information, "GetDisabledVersions: Exception occured while checking for disabled versions");
			}
			return result;
		}
	}
}
