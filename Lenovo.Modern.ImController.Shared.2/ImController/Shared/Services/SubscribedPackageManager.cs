using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;
using Lenovo.Modern.CoreTypes.Models;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.CoreTypes.Models.Subscription;
using Lenovo.Modern.CoreTypes.Services;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.ImController.Shared.Services.Contracts.SystemInformation.AppTag;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x0200001B RID: 27
	public class SubscribedPackageManager
	{
		// Token: 0x06000088 RID: 136 RVA: 0x00004ECC File Offset: 0x000030CC
		public static bool IsPackageApplicable(PackageSubscription subscription, Lenovo.Modern.ImController.Shared.Model.Packages.Package package, CancellationToken cancelToken)
		{
			bool flag = false;
			string text = "NULL";
			try
			{
				Task<MachineInformation> machineInformationAsync = MachineInformationManager.GetInstance().GetMachineInformationAsync(cancelToken);
				Task<AppAndTagCollection> appAndTagsAsync = AppAndTagManager.GetInstance().GetAppAndTagsAsync(cancelToken);
				machineInformationAsync.Wait();
				appAndTagsAsync.Wait();
				MachineInformation result = machineInformationAsync.Result;
				AppAndTagCollection result2 = appAndTagsAsync.Result;
				string imControllerVersion = Constants.ImControllerVersion;
				text = package.PackageInformation.Name;
				Filter filter = EligibilityFilter.GetFirstApplicableMatch(package.ApplicabilityFilter, result, result2, imControllerVersion) as Filter;
				if (filter != null)
				{
					bool? valueAsBool = filter.GetValueAsBool();
					if (valueAsBool != null && valueAsBool.GetValueOrDefault(false))
					{
						flag = true;
						if (ImcPolicy.IsPackageDeleteOnUapUnInstallEnabled() && !SubscribedPackageManager.ValidateDeviceUapAssociation(subscription, package.PackageInformation.Name, true))
						{
							Logger.Log(Logger.LogSeverity.Information, "IsPackageApplicable: Package {0} is not applicable because disabled through policy of UAP association", new object[] { package.PackageInformation.Name });
							flag = false;
						}
						if (SubscribedPackageManager.IsReplacementPluginAvailable(package))
						{
							Logger.Log(Logger.LogSeverity.Information, "IsPackageApplicable: Package {0} is not applicable because replacement plugin is available", new object[] { package.PackageInformation.Name });
							flag = false;
						}
						if (SubscribedPackageManager.IsPluginVersionDisabled(package))
						{
							Logger.Log(Logger.LogSeverity.Information, "IsPackageApplicable: Package {0} is not applicable because this plugin version is disabled", new object[] { package.PackageInformation.Name });
							flag = false;
						}
						if (flag && MsSignability.IsMsSignRequired() && package.PackageInformation.Packagetype == PackageType.App)
						{
							PluginSettingsAgent pluginSettingsAgent = new PluginSettingsAgent(subscription);
							if (pluginSettingsAgent != null)
							{
								PluginSettingsAgent.Setting prioritizedSetting = pluginSettingsAgent.GetPrioritizedSetting(text, "ImController.Privilege.CustomInstall", PluginSettingsAgent.SettingLocation.SetByManifest);
								if (prioritizedSetting != null && !string.IsNullOrWhiteSpace(prioritizedSetting.ValueAsString))
								{
									int? valueAsInt = prioritizedSetting.ValueAsInt;
									int num = 1;
									if ((valueAsInt.GetValueOrDefault() == num) & (valueAsInt != null))
									{
										Logger.Log(Logger.LogSeverity.Information, "IsPackageApplicable: Package {0} is not applicable because it requires custom script and OS is cloud", new object[] { package.PackageInformation.Name });
										flag = false;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PluginRepository: Unable to determine package applicability");
			}
			if (!flag)
			{
				Logger.Log(Logger.LogSeverity.Information, "PluginRepository: Package {0} NOT applicable", new object[] { text });
			}
			return flag;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000050C4 File Offset: 0x000032C4
		public static bool ValidateDeviceUapAssociation(PackageSubscription subscription, string packageName, bool checkGlobalInstallState)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = true;
			if (subscription != null && subscription.PackageList != null && subscription.PackageList.Any<Lenovo.Modern.ImController.Shared.Model.Packages.Package>())
			{
				try
				{
					Lenovo.Modern.ImController.Shared.Model.Packages.Package package = subscription.PackageList.FirstOrDefault((Lenovo.Modern.ImController.Shared.Model.Packages.Package p) => p != null && p.PackageInformation != null && (!string.IsNullOrWhiteSpace(p.PackageInformation.Name) & p.PackageInformation.Name.Equals(packageName, StringComparison.InvariantCultureIgnoreCase)));
					if (package == null)
					{
						Logger.Log(Logger.LogSeverity.Warning, "ValidateDeviceUapAssociation: Cannot find UAP association '{0}' because plugin not found.", new object[] { packageName });
					}
					else
					{
						flag = PackageSettingsAgent.DoesPackageHaveDeviceAssociation(package);
						if (!flag)
						{
							Dictionary<string, string> packageUapAssociationIds = PackageSettingsAgent.GetPackageUapAssociationIds(package);
							if (packageUapAssociationIds == null || !packageUapAssociationIds.Any<KeyValuePair<string, string>>())
							{
								goto IL_1B9;
							}
							flag2 = true;
							flag3 = false;
							using (Dictionary<string, string>.Enumerator enumerator = packageUapAssociationIds.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									KeyValuePair<string, string> keyValuePair = enumerator.Current;
									flag3 = UapInstallMonitor.GetInstance().IsAppInstalled(keyValuePair.Key, checkGlobalInstallState);
									if (flag3)
									{
										if (!string.IsNullOrWhiteSpace(keyValuePair.Value))
										{
											flag3 = false;
											Version installedAppVersion = UapInstallMonitor.GetInstance().GetInstalledAppVersion(keyValuePair.Key);
											Version version = new Version("0.0.0.0");
											Version.TryParse(keyValuePair.Value, out version);
											if (version == null || version.CompareTo(installedAppVersion) <= 0)
											{
												flag3 = true;
											}
											Logger.Log(Logger.LogSeverity.Information, "ValidateDeviceUapAssociation: Uap association Found for {0}. Uap is {1} installation state with Version is {2}", new object[] { packageName, keyValuePair, flag3 });
										}
										else
										{
											Logger.Log(Logger.LogSeverity.Information, "ValidateDeviceUapAssociation: Uap association Found for {0}. Uap is {1} installation state is {2}", new object[] { packageName, keyValuePair, flag3 });
										}
										if (flag3)
										{
											break;
										}
									}
								}
								goto IL_1B9;
							}
						}
						Logger.Log(Logger.LogSeverity.Information, "ValidateDeviceUapAssociation: Device association Found for {0}. ", new object[] { packageName });
					}
					IL_1B9:;
				}
				catch (Exception)
				{
				}
			}
			return flag || (!flag && flag2 && flag3) || (!flag2 && !flag);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000052DC File Offset: 0x000034DC
		public static async Task<bool> IsCustomInstallPrivilegeEnabled(PackageSubscription subscription, string packageName)
		{
			bool customInstallEnabled = false;
			bool isCustomInstallApplicable = false;
			ISubscriptionManager instance = SubscriptionManager.GetInstance(null);
			if (instance != null)
			{
				if (subscription == null)
				{
					PackageSubscription packageSubscription = await instance.GetSubscriptionAsync(default(CancellationToken));
					subscription = packageSubscription;
				}
				if (subscription != null && subscription.PackageList != null && subscription.PackageList.Any<Lenovo.Modern.ImController.Shared.Model.Packages.Package>())
				{
					Lenovo.Modern.ImController.Shared.Model.Packages.Package package = subscription.PackageList.FirstOrDefault((Lenovo.Modern.ImController.Shared.Model.Packages.Package p) => p != null && p.PackageInformation != null && (!string.IsNullOrWhiteSpace(p.PackageInformation.Name) & p.PackageInformation.Name.Equals(packageName, StringComparison.InvariantCultureIgnoreCase)));
					if (package == null)
					{
						Logger.Log(Logger.LogSeverity.Warning, "PluginRepository: IsCustomInstallPrivilegeEnabled: Cannot get manifest setting '{0}' because plugin not found.", new object[] { packageName });
					}
					else
					{
						isCustomInstallApplicable = package.PackageInformation.Packagetype == PackageType.App;
					}
					if (isCustomInstallApplicable)
					{
						PluginSettingsAgent pluginSettingsAgent = new PluginSettingsAgent(subscription);
						if (pluginSettingsAgent != null)
						{
							PluginSettingsAgent.Setting prioritizedSetting = pluginSettingsAgent.GetPrioritizedSetting(packageName, "ImController.Privilege.CustomInstall", PluginSettingsAgent.SettingLocation.SetByManifest);
							if (prioritizedSetting != null && !string.IsNullOrWhiteSpace(prioritizedSetting.ValueAsString))
							{
								int? valueAsInt = prioritizedSetting.ValueAsInt;
								int num = 1;
								customInstallEnabled = (valueAsInt.GetValueOrDefault() == num) & (valueAsInt != null);
							}
						}
					}
				}
			}
			return customInstallEnabled;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000532C File Offset: 0x0000352C
		private static bool IsReplacementPluginAvailable(Lenovo.Modern.ImController.Shared.Model.Packages.Package package)
		{
			bool result = false;
			try
			{
				if (package.SettingList != null)
				{
					AppSetting appSetting = package.SettingList.FirstOrDefault((AppSetting s) => !string.IsNullOrWhiteSpace(s.Key) && s.Key.Equals("ImController.Package.Replacement.Name"));
					if (appSetting != null)
					{
						AppSetting appSetting2 = package.SettingList.FirstOrDefault((AppSetting s) => !string.IsNullOrWhiteSpace(s.Key) && s.Key.Equals("ImController.Package.Replacement.MinimumVersion"));
						if (appSetting2 != null)
						{
							PluginRepository.PluginInformation pluginInformation = PluginRepository.GetPluginInformation(appSetting.Value);
							if (pluginInformation != null)
							{
								if (pluginInformation.Version.CompareTo(Version.Parse(appSetting2.Value)) >= 0)
								{
									Logger.Log(Logger.LogSeverity.Information, "IsReplacementPluginAvailable: Package {0} is not applicable since replacement is available {1}", new object[]
									{
										package.PackageInformation.Name,
										appSetting.Value
									});
									result = true;
								}
								else
								{
									Logger.Log(Logger.LogSeverity.Information, "IsReplacementPluginAvailable: Package {0} is not replaced with {1} because minimumVer={2} but availableVer={3}", new object[]
									{
										package.PackageInformation.Name,
										appSetting.Value,
										appSetting2.Value.ToString(),
										pluginInformation.Version.ToString()
									});
								}
							}
							else
							{
								Logger.Log(Logger.LogSeverity.Warning, "IsReplacementPluginAvailable: Could not get plugin Info for {0}", new object[] { appSetting.Value });
							}
						}
					}
				}
			}
			catch (Exception)
			{
				Logger.Log(Logger.LogSeverity.Information, "IsReplacementPluginAvailable: Exception occured");
			}
			return result;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00005490 File Offset: 0x00003690
		private static bool IsPluginVersionDisabled(Lenovo.Modern.ImController.Shared.Model.Packages.Package package)
		{
			bool result = false;
			try
			{
				if (package.SettingList != null)
				{
					List<Version> disabledVersions = SubscribedPackageManager.GetDisabledVersions(package);
					if (disabledVersions != null && disabledVersions.Count > 0)
					{
						PluginRepository.PluginInformation pluginInformation = PluginRepository.GetPluginInformation(package.PackageInformation.Name);
						if (pluginInformation != null && disabledVersions.Contains(pluginInformation.Version))
						{
							Logger.Log(Logger.LogSeverity.Information, "IsPluginVersionDisabled: Package {0} is not applicable since plugin version is disabled {1}", new object[]
							{
								package.PackageInformation.Name,
								pluginInformation.Version.ToString()
							});
							result = true;
						}
					}
				}
			}
			catch (Exception)
			{
				Logger.Log(Logger.LogSeverity.Information, "IsPluginVersionDisabled: Exception occured");
			}
			return result;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000552C File Offset: 0x0000372C
		private static List<Version> GetDisabledVersions(Lenovo.Modern.ImController.Shared.Model.Packages.Package package)
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
