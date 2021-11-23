using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.ImController.Shared.Utilities
{
	// Token: 0x02000031 RID: 49
	public class ImcPolicy
	{
		// Token: 0x0600017F RID: 383 RVA: 0x00007978 File Offset: 0x00005B78
		public static bool IsUpdateDisabledByPolicy()
		{
			bool flag = false;
			try
			{
				string text = ConfigurationSettings.AppSettings["DisableUpdates"];
				if (!string.IsNullOrWhiteSpace(text) && text.Equals("true", StringComparison.OrdinalIgnoreCase))
				{
					Logger.Log(Logger.LogSeverity.Warning, "ImcPolicy: Warning: Updates has been disabled by IT Admin (through config file)");
					flag = true;
				}
				else
				{
					string imcPolicyRegKeyPath = Constants.ImcPolicyRegKeyPath;
					IContainer container = ((IContainerSystem)new RegistrySystem()).LoadContainer(imcPolicyRegKeyPath);
					if (container != null)
					{
						IContainerValue value = container.GetValue("DisableAutoupdate");
						if (value != null)
						{
							string valueAsString = value.GetValueAsString();
							if (!string.IsNullOrWhiteSpace(valueAsString) && "1".Equals(valueAsString))
							{
								flag = true;
								Logger.Log(Logger.LogSeverity.Warning, "ImcPolicy: Warning: IT Admin has set policy (DisableAutoupdate) to disable updates");
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			if (flag && (Environment.UserInteractive || Environment.GetCommandLineArgs().Length != 0))
			{
				Logger.Log(Logger.LogSeverity.Warning, "ImcPolicy: Warning: Overriding disableUpdates since this is installation");
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00007A44 File Offset: 0x00005C44
		public static PackageSubscription RemoveITAdminDisabledPluginSubscriptionAsync(PackageSubscription subscription)
		{
			try
			{
				IContainer container = ((IContainerSystem)new RegistrySystem()).LoadContainer(Constants.ImcPluginPolicyRegKeyPath);
				if (container != null)
				{
					List<Package> packageList = (from package in subscription.PackageList
						where !ImcPolicy.IsPluginBlockedBypolicy(package)
						select package).ToList<Package>();
					subscription.PackageList = packageList;
				}
			}
			catch (Exception)
			{
			}
			return subscription;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007AB4 File Offset: 0x00005CB4
		public static bool IsPackageDeleteOnUapUnInstallEnabled()
		{
			if (ImcPolicy._cachedIsPackageDeletionEnabled != null)
			{
				return ImcPolicy._cachedIsPackageDeletionEnabled.Value;
			}
			IContainer container = ((IContainerSystem)new RegistrySystem()).LoadContainer(Constants.ImcPolicyRegKeyPath);
			if (container != null)
			{
				IContainerValue value = container.GetValue("RequireAppAssociation");
				if (value != null)
				{
					string valueAsString = value.GetValueAsString();
					if (!string.IsNullOrWhiteSpace(valueAsString) && "1".Equals(valueAsString))
					{
						ImcPolicy._cachedIsPackageDeletionEnabled = new bool?(true);
						Logger.Log(Logger.LogSeverity.Warning, "ImcPolicy: Warning: IT Admin has set policy (RequireAppAssociation) to remove pluigns associated with apps not installed");
					}
				}
			}
			if (ImcPolicy._cachedIsPackageDeletionEnabled == null)
			{
				ImcPolicy._cachedIsPackageDeletionEnabled = new bool?(false);
			}
			return ImcPolicy._cachedIsPackageDeletionEnabled.Value;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007B50 File Offset: 0x00005D50
		public static bool GetIsPackageTypeDisabled()
		{
			if (ImcPolicy._cachedIsPackageTypeDisabled != null)
			{
				return ImcPolicy._cachedIsPackageTypeDisabled.Value;
			}
			IContainer container = ((IContainerSystem)new RegistrySystem()).LoadContainer(Constants.ImcPolicyRegKeyPath);
			if (container != null)
			{
				IContainerValue value = container.GetValue("DisableAppPackages");
				if (value != null)
				{
					string valueAsString = value.GetValueAsString();
					if (!string.IsNullOrWhiteSpace(valueAsString) && "1".Equals(valueAsString))
					{
						ImcPolicy._cachedIsPackageTypeDisabled = new bool?(true);
						Logger.Log(Logger.LogSeverity.Warning, "ImcPolicy: Warning: IT Admin has set policy (DisableAppPackages) to disable packages");
					}
				}
			}
			if (ImcPolicy._cachedIsPackageTypeDisabled == null)
			{
				ImcPolicy._cachedIsPackageTypeDisabled = new bool?(false);
			}
			return ImcPolicy._cachedIsPackageTypeDisabled.Value;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007BEC File Offset: 0x00005DEC
		private static bool IsPluginBlockedBypolicy(Package plugin)
		{
			bool result = false;
			try
			{
				string path = Constants.ImcPluginPolicyRegKeyPath + plugin.PackageInformation.Name;
				IContainer container = ((IContainerSystem)new RegistrySystem()).LoadContainer(path);
				if (container != null)
				{
					IContainerValue value = container.GetValue("Imc-Block");
					if (value != null)
					{
						string valueAsString = value.GetValueAsString();
						if (!string.IsNullOrWhiteSpace(valueAsString) && "1".Equals(valueAsString))
						{
							result = true;
							Logger.Log(Logger.LogSeverity.Warning, "ImcPolicy: Warning: IT admin has set policy (IMC-Block) to block Plugin {0}", new object[] { plugin.PackageInformation.Name });
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00007C88 File Offset: 0x00005E88
		public static bool IsDeviceImprovementDisabled()
		{
			if (ImcPolicy._cachedIsDeviceImprovementDisabled != null)
			{
				return ImcPolicy._cachedIsDeviceImprovementDisabled.Value;
			}
			try
			{
				IContainer container = ((IContainerSystem)new RegistrySystem()).LoadContainer(Constants.ImcPolicyRegKeyPath);
				if (container != null)
				{
					IContainerValue value = container.GetValue("DisableSystemInterfaceUsageStats");
					if (value != null)
					{
						string valueAsString = value.GetValueAsString();
						if (!string.IsNullOrWhiteSpace(valueAsString) && "1".Equals(valueAsString))
						{
							ImcPolicy._cachedIsDeviceImprovementDisabled = new bool?(true);
							Logger.Log(Logger.LogSeverity.Warning, "ImcPolicy: Warning: IT Admin has set policy (DisableSystemInterfaceUsageStats) to disable telemetry");
						}
					}
				}
			}
			catch (Exception)
			{
			}
			if (ImcPolicy._cachedIsDeviceImprovementDisabled == null)
			{
				ImcPolicy._cachedIsDeviceImprovementDisabled = new bool?(false);
			}
			return ImcPolicy._cachedIsDeviceImprovementDisabled.Value;
		}

		// Token: 0x040000DF RID: 223
		private static bool? _cachedIsPackageDeletionEnabled;

		// Token: 0x040000E0 RID: 224
		private static bool? _cachedIsPackageTypeDisabled;

		// Token: 0x040000E1 RID: 225
		private static bool? _cachedIsDeviceImprovementDisabled;
	}
}
