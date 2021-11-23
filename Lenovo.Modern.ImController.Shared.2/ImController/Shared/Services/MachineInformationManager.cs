using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.Utilities.Patterns.Ioc;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.SystemContext.Interop;
using Lenovo.Modern.Utilities.Services.SystemContext.Settings;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x02000012 RID: 18
	public class MachineInformationManager : IMachineInformationManager, IDataCleanup
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00003012 File Offset: 0x00001212
		private MachineInformationManager()
		{
			this._processPrivilegeDetector = new ProcessPrivilegeDetector();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003047 File Offset: 0x00001247
		public static IMachineInformationManager GetInstance()
		{
			IMachineInformationManager result;
			if ((result = MachineInformationManager._instance) == null)
			{
				result = (MachineInformationManager._instance = new MachineInformationManager());
			}
			return result;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003060 File Offset: 0x00001260
		public void UpdateCacheWithDelay(CancellationToken cancelToken)
		{
			MachineInformationManager.<>c__DisplayClass9_0 CS$<>8__locals1 = new MachineInformationManager.<>c__DisplayClass9_0();
			CS$<>8__locals1.cancelToken = cancelToken;
			CS$<>8__locals1.<>4__this = this;
			if (Interlocked.Exchange(ref MachineInformationManager._cacheUpdateInProgress, 1) == 0)
			{
				Task.Run(delegate()
				{
					MachineInformationManager.<>c__DisplayClass9_0.<<UpdateCacheWithDelay>b__0>d <<UpdateCacheWithDelay>b__0>d;
					<<UpdateCacheWithDelay>b__0>d.<>4__this = CS$<>8__locals1;
					<<UpdateCacheWithDelay>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<UpdateCacheWithDelay>b__0>d.<>1__state = -1;
					AsyncTaskMethodBuilder <>t__builder = <<UpdateCacheWithDelay>b__0>d.<>t__builder;
					<>t__builder.Start<MachineInformationManager.<>c__DisplayClass9_0.<<UpdateCacheWithDelay>b__0>d>(ref <<UpdateCacheWithDelay>b__0>d);
					return <<UpdateCacheWithDelay>b__0>d.<>t__builder.Task;
				});
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000030A0 File Offset: 0x000012A0
		public async Task<MachineInformation> GetMachineInformationAsync(CancellationToken cancelToken)
		{
			await this._getMachineInformationSempahore.WaitAsync();
			try
			{
				if (this._cachedMachineInformation != null && !DateTime.MinValue.Equals(this._lastCacheUpdate))
				{
					TimeSpan timeSpan = DateTime.Now - this._lastCacheUpdate;
					if (timeSpan.TotalMinutes > (double)this._latestUpdateIntervalMinutes)
					{
						Logger.Log(Logger.LogSeverity.Information, "GetMachineInformationAsync: Updating cache since last updation was {0} before which is more than {1} minutes ago", new object[] { timeSpan.TotalMinutes, this._latestUpdateIntervalMinutes });
						this.UpdateCacheWithDelay(cancelToken);
					}
				}
				if (this._cachedMachineInformation == null)
				{
					if (cancelToken.IsCancellationRequested)
					{
						return null;
					}
					MachineInformation shareFiledMachineInfo = await this.TryLoadFromFileAsync();
					if (shareFiledMachineInfo != null && !string.IsNullOrWhiteSpace(shareFiledMachineInfo.SerialNumber) && shareFiledMachineInfo.Brand != BrandType.None)
					{
						this._cachedMachineInformation = shareFiledMachineInfo;
						this.UpdateCacheWithDelay(cancelToken);
					}
					else
					{
						Logger.Log(Logger.LogSeverity.Warning, "MachineInfoManager: Machineinfo was not cached, will make a plugin request");
						MachineInformation pluginMachineInfo = await this.TryLoadFromPluginAsync(cancelToken);
						MachineInformation newData = await this.TryLoadFromRegistryAsync();
						this._cachedMachineInformation = MachineInformationManager.MergeMachineInformation(MachineInformationManager.MergeMachineInformation(pluginMachineInfo, shareFiledMachineInfo), newData);
						this._lastCacheUpdate = DateTime.Now;
						pluginMachineInfo = null;
					}
					shareFiledMachineInfo = null;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "MachineInfoManager: Unable to retrieve machine information");
			}
			finally
			{
				this._getMachineInformationSempahore.Release();
			}
			return this._cachedMachineInformation;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003010 File Offset: 0x00001210
		public void CleanupData()
		{
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000030F0 File Offset: 0x000012F0
		private static MachineInformation MergeMachineInformation(MachineInformation originalData, MachineInformation newData)
		{
			MachineInformation machineInformation = null;
			try
			{
				if (originalData != null && newData == null)
				{
					machineInformation = originalData;
				}
				else if (originalData == null && newData != null)
				{
					machineInformation = newData;
				}
				else if (originalData != null && newData != null)
				{
					machineInformation = new MachineInformation
					{
						BiosDate = (string.IsNullOrWhiteSpace(originalData.BiosDate) ? newData.BiosDate : originalData.BiosDate),
						BiosVersion = (string.IsNullOrWhiteSpace(originalData.BiosVersion) ? newData.BiosVersion : originalData.BiosVersion),
						Brand = (originalData.Brand.Equals(BrandType.None) ? newData.Brand : originalData.Brand),
						CountryCode = (string.IsNullOrWhiteSpace(originalData.CountryCode) ? newData.CountryCode : originalData.CountryCode),
						CPUAddressWidth = (string.IsNullOrWhiteSpace(originalData.CPUAddressWidth) ? newData.CPUAddressWidth : originalData.CPUAddressWidth),
						CPUArchitecture = (string.IsNullOrWhiteSpace(originalData.CPUArchitecture) ? newData.CPUArchitecture : originalData.CPUArchitecture),
						DateCreated = ((originalData.DateCreated <= DateTimeOffset.Now && originalData.DateCreated.DateTime > DateTime.MinValue) ? originalData.DateCreated : ((newData.DateCreated <= DateTimeOffset.Now && newData.DateCreated.DateTime > DateTime.MinValue) ? newData.DateCreated : DateTimeOffset.Now)),
						ECVersion = (string.IsNullOrWhiteSpace(originalData.ECVersion) ? newData.ECVersion : originalData.ECVersion),
						Enclosure = (originalData.Enclosure.Equals(EnclosureType.None) ? newData.Enclosure : originalData.Enclosure),
						Family = (string.IsNullOrWhiteSpace(originalData.Family) ? newData.Family : originalData.Family),
						FirstRunDate = ((originalData.FirstRunDate <= DateTimeOffset.Now && originalData.FirstRunDate.DateTime > DateTime.MinValue) ? originalData.FirstRunDate : ((newData.FirstRunDate <= DateTimeOffset.Now && newData.FirstRunDate.DateTime > DateTime.MinValue) ? newData.FirstRunDate : DateTimeOffset.Now)),
						Locale = (string.IsNullOrWhiteSpace(originalData.Locale) ? newData.Locale : originalData.Locale),
						Manufacturer = (string.IsNullOrWhiteSpace(originalData.Manufacturer) ? newData.Manufacturer : originalData.Manufacturer),
						MT = (string.IsNullOrWhiteSpace(originalData.MT) ? newData.MT : originalData.MT),
						MTM = (string.IsNullOrWhiteSpace(originalData.MTM) ? newData.MTM : originalData.MTM),
						OperatingSystemBitness = (string.IsNullOrWhiteSpace(originalData.OperatingSystemBitness) ? newData.OperatingSystemBitness : originalData.OperatingSystemBitness),
						OperatingSystemVerion = (string.IsNullOrWhiteSpace(originalData.OperatingSystemVerion) ? newData.OperatingSystemVerion : originalData.OperatingSystemVerion),
						OS = (string.IsNullOrWhiteSpace(originalData.OS) ? newData.OS : originalData.OS),
						OSName = (string.IsNullOrWhiteSpace(originalData.OSName) ? newData.OSName : originalData.OSName),
						SerialNumber = (string.IsNullOrWhiteSpace(originalData.SerialNumber) ? newData.SerialNumber : originalData.SerialNumber),
						SKU = (string.IsNullOrWhiteSpace(originalData.SKU) ? newData.SKU : originalData.SKU),
						SubBrand = (string.IsNullOrWhiteSpace(originalData.SubBrand) ? newData.SubBrand : originalData.SubBrand),
						XmlDateCreatedDoNotUse = (string.IsNullOrWhiteSpace(originalData.XmlDateCreatedDoNotUse) ? newData.XmlDateCreatedDoNotUse : originalData.XmlDateCreatedDoNotUse),
						XmlFirstRunDateDoNotUse = (string.IsNullOrWhiteSpace(originalData.XmlFirstRunDateDoNotUse) ? newData.XmlFirstRunDateDoNotUse : originalData.XmlFirstRunDateDoNotUse)
					};
					machineInformation.PreloadTagList = originalData.PreloadTagList;
					if (originalData.PreloadTagList != null)
					{
						if (newData.PreloadTagList != null)
						{
							machineInformation.PreloadTagList = ((originalData.PreloadTagList.Count<string>() > newData.PreloadTagList.Count<string>()) ? originalData.PreloadTagList : newData.PreloadTagList);
						}
					}
					else
					{
						machineInformation.PreloadTagList = newData.PreloadTagList;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Critical, "Error while merging two machineInformation types", new object[] { ex });
				machineInformation = null;
			}
			return machineInformation ?? originalData;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000359C File Offset: 0x0000179C
		private async Task<MachineInformation> TryLoadFromPluginAsync(CancellationToken cancelToken)
		{
			MachineInformation machineInformation = null;
			using (CancellationTokenSource thisRequestToken = new CancellationTokenSource(TimeSpan.FromSeconds(20.0)))
			{
				using (CancellationTokenSource.CreateLinkedTokenSource(cancelToken, thisRequestToken.Token))
				{
					try
					{
						ContractRequest contractRequest = new ContractRequest
						{
							Name = ContractConstants.Get.ContractName,
							Command = new ContractCommandRequest
							{
								Name = ContractConstants.Get.CommandNameGetMachineInformation,
								RequestType = "sync"
							}
						};
						IPluginManager pluginManager = InstanceContainer.GetInstance().Resolve<IPluginManager>();
						Func<string, PluginRepository.PluginInformation> func = delegate(string pluginName)
						{
							try
							{
								PluginRepository.PluginInformation pluginInformation2 = PluginRepository.GetPluginInformation(pluginName);
								if (pluginInformation2 != null && File.Exists(pluginInformation2.PathToPlugin))
								{
									return pluginInformation2;
								}
							}
							catch (Exception ex3)
							{
								Logger.Log(ex3, "MachineInfoManager: Unable to locate plugin {0}", new object[] { pluginName });
							}
							return null;
						};
						PluginRepository.PluginInformation pluginInformation = func("GenericCorePlugin") ?? func("GenericMachineInformationPlugin");
						if (pluginInformation == null)
						{
							Logger.Log(Logger.LogSeverity.Error, "MachineInfoManager: Unable to locate plugin in any location");
							return null;
						}
						PluginRequestInformation pluginRequestInformation = new PluginRequestInformation
						{
							ContractRequest = contractRequest,
							PluginName = pluginInformation.PluginName,
							TaskId = Guid.NewGuid().ToString(),
							RunAs = RunAs.User,
							RequestType = RequestType.Internal
						};
						if (Environment.GetCommandLineArgs().Length > 1)
						{
							IntPtr zero = IntPtr.Zero;
							if (!Authorization.GetSessionUserToken(ref zero))
							{
								bool isSystem;
								using (WindowsIdentity current = WindowsIdentity.GetCurrent())
								{
									isSystem = current.IsSystem;
								}
								if (isSystem)
								{
									pluginRequestInformation.RunAs = RunAs.System;
									Logger.Log(Logger.LogSeverity.Information, "MachineInfoManager: User is not logged In. Current process is System process. Plugin Runas={0}", new object[] { pluginRequestInformation.RunAs.ToString() });
								}
								else
								{
									Logger.Log(Logger.LogSeverity.Information, "MachineInfoManager: User is not logged In or current process is not System process. Plugin Runas={0}", new object[] { pluginRequestInformation.RunAs.ToString() });
								}
							}
							else
							{
								Logger.Log(Logger.LogSeverity.Information, "MachineInfoManager: User is logged In. Plugin Runas={0}", new object[] { pluginRequestInformation.RunAs.ToString() });
							}
						}
						Logger.Log(Logger.LogSeverity.Information, "MachineInfoManager: Making plugin request with runas={0}", new object[] { pluginRequestInformation.RunAs.ToString() });
						BrokerResponseTask brokerResponseTask = await pluginManager.MakePluginRequest(pluginRequestInformation, cancelToken);
						if (brokerResponseTask != null && brokerResponseTask.ContractResponse != null && brokerResponseTask.ContractResponse.Response != null && !string.IsNullOrWhiteSpace(brokerResponseTask.ContractResponse.Response.Data))
						{
							machineInformation = Serializer.Deserialize<MachineInformation>(brokerResponseTask.ContractResponse.Response.Data);
							if (machineInformation != null)
							{
								try
								{
									string pathToSharedMachineInfoFile = MachineInformationManager.GetPathToSharedMachineInfoFile();
									Logger.Log(Logger.LogSeverity.Information, "MachineInfoManager: Writing data to file");
									File.WriteAllText(pathToSharedMachineInfoFile, brokerResponseTask.ContractResponse.Response.Data, Encoding.UTF8);
									goto IL_346;
								}
								catch (Exception ex)
								{
									Logger.Log(ex, "MachineInfoManager: Unable to save data to shared file");
									goto IL_346;
								}
							}
							Logger.Log(Logger.LogSeverity.Error, "MachineInfoManager: Response from plugin did not serialize");
						}
						else
						{
							Logger.Log(Logger.LogSeverity.Error, "MachineInfoManager: Response from plugin is empty or invalid");
						}
						IL_346:;
					}
					catch (OperationCanceledException)
					{
						Logger.Log(Logger.LogSeverity.Error, "MachineInfoManager: contract request timed out or was canceled");
					}
					catch (Exception ex2)
					{
						Logger.Log(ex2, "MachineInfoManager: Unhandled exception while getting from plugin.");
					}
				}
				CancellationTokenSource mergedCancellationToken = null;
			}
			CancellationTokenSource thisRequestToken = null;
			return machineInformation;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000035E4 File Offset: 0x000017E4
		private Task<MachineInformation> TryLoadFromRegistryAsync()
		{
			MachineInformation result = null;
			try
			{
				IContainer container = this.LoadRegistryBasedOnPrivilege("HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\System\\BIOS");
				if (container != null)
				{
					IEnumerable<IContainerValue> values = container.GetValues(true);
					string mt = null;
					string family = "other";
					string sku = MachineInformationManager.TryGetRegistryValueAsString(values, "SystemSKU");
					Dictionary<string, string> valuesFromSku = MachineInformationManager.GetValuesFromSku(sku);
					if (valuesFromSku.ContainsKey("MT"))
					{
						mt = valuesFromSku["MT"];
					}
					if (valuesFromSku.ContainsKey("FM"))
					{
						family = valuesFromSku["FM"];
					}
					result = new MachineInformation
					{
						BiosVersion = MachineInformationManager.TryGetRegistryValueAsString(values, "BIOSVersion"),
						Brand = this.GetSystemBrand(family),
						Family = MachineInformationManager.TryGetRegistryValueAsString(values, "SystemFamily"),
						MT = mt,
						Manufacturer = MachineInformationManager.TryGetRegistryValueAsString(values, "SystemManufacturer"),
						SKU = sku,
						DateCreated = new DateTimeOffset(DateTime.Now),
						PreloadTagList = new List<string>().ToArray()
					};
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "MachineInfoManager: Unable to load machine information from registry");
			}
			return Task.FromResult<MachineInformation>(result);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003700 File Offset: 0x00001900
		private Task<MachineInformation> TryLoadFromFileAsync()
		{
			MachineInformation result = null;
			string pathToSharedMachineInfoFile = MachineInformationManager.GetPathToSharedMachineInfoFile();
			try
			{
				if (File.Exists(pathToSharedMachineInfoFile))
				{
					string text = File.ReadAllText(pathToSharedMachineInfoFile);
					if (!string.IsNullOrWhiteSpace(text))
					{
						result = Serializer.Deserialize<MachineInformation>(text);
					}
					else
					{
						Logger.Log(Logger.LogSeverity.Warning, "MachineInfoManager: Machine info file exists but is empty");
					}
				}
				else
				{
					Logger.Log(Logger.LogSeverity.Warning, "MachineInfoManager: Machine info file does not exist");
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "MachineInfoManager: Unable to load machine information from {0}", new object[] { pathToSharedMachineInfoFile ?? "null" });
			}
			return Task.FromResult<MachineInformation>(result);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003784 File Offset: 0x00001984
		private static Dictionary<string, string> GetValuesFromSku(string sku)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (!string.IsNullOrWhiteSpace(sku))
			{
				sku = "BRAND_" + sku;
				List<string> list = sku.Split(new char[] { '_' }).ToList<string>();
				for (int i = 0; i < list.Count<string>(); i++)
				{
					if (list.Count >= i + 1)
					{
						string text = list[i];
						string value = list[++i];
						if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
						{
							dictionary.Add(text, value);
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003810 File Offset: 0x00001A10
		private static string TryGetRegistryValueAsString(IEnumerable<IContainerValue> containerValues, string valueName)
		{
			string result = null;
			try
			{
				if (containerValues != null && !string.IsNullOrWhiteSpace(valueName))
				{
					IContainerValue containerValue = containerValues.FirstOrDefault((IContainerValue v) => v != null && !string.IsNullOrWhiteSpace(v.GetName()) && v.GetName() == valueName);
					if (containerValue != null)
					{
						result = containerValue.GetValueAsString();
					}
					if (containerValue == null)
					{
						Logger.Log(Logger.LogSeverity.Information, "cannot find contanierItem from registry");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception in MachineInformationManager.TryGetRegistryValueAsString");
			}
			return result;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003888 File Offset: 0x00001A88
		private static string GetPathToSharedMachineInfoFile()
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Lenovo\\ImController\\shared");
			Directory.CreateDirectory(text);
			return text + "\\MachineInformation.xml";
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000038AC File Offset: 0x00001AAC
		private BrandType GetSystemBrand(string family)
		{
			string text = family.ToLower();
			BrandType result;
			if (text.Contains("think"))
			{
				result = BrandType.Think;
			}
			else if (text.Contains("idea"))
			{
				result = BrandType.Idea;
			}
			else if (text.Contains("lenovo"))
			{
				result = BrandType.Lenovo;
			}
			else if (text.Contains("medion"))
			{
				result = BrandType.Medion;
			}
			else if (text.Contains("nec"))
			{
				result = BrandType.NecConsumer;
			}
			else
			{
				result = BrandType.Other;
			}
			return result;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000391C File Offset: 0x00001B1C
		private IContainer LoadRegistryBasedOnPrivilege(string location)
		{
			IContainer result = null;
			IContainerSystem containerSystem;
			if (this._processPrivilegeDetector.GetCurrentProcessPrivilege().ToString() == ProcessPrivilegeDetector.RunAsPrivilege.System.ToString())
			{
				containerSystem = new SystemContextRegistrySystem();
			}
			else
			{
				containerSystem = new RegistrySystem();
			}
			try
			{
				if (location != null)
				{
					result = containerSystem.LoadContainer(location);
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, string.Format("Exception while loading location.\r\n{0}", location));
			}
			return result;
		}

		// Token: 0x04000058 RID: 88
		private static IMachineInformationManager _instance;

		// Token: 0x04000059 RID: 89
		private readonly SemaphoreSlim _getMachineInformationSempahore = new SemaphoreSlim(1);

		// Token: 0x0400005A RID: 90
		private MachineInformation _cachedMachineInformation;

		// Token: 0x0400005B RID: 91
		private readonly ProcessPrivilegeDetector _processPrivilegeDetector;

		// Token: 0x0400005C RID: 92
		private DateTime _lastCacheUpdate = DateTime.MinValue;

		// Token: 0x0400005D RID: 93
		private readonly int _latestUpdateIntervalMinutes = 1440;

		// Token: 0x0400005E RID: 94
		private static int _cacheUpdateInProgress;

		// Token: 0x0200004A RID: 74
		private static class MachineInfoConstants
		{
			// Token: 0x0400010F RID: 271
			public const string OldPluginName = "GenericMachineInformationPlugin";

			// Token: 0x04000110 RID: 272
			public const string CorePluginName = "GenericCorePlugin";
		}

		// Token: 0x0200004B RID: 75
		private static class RegistryAgentConstants
		{
			// Token: 0x02000098 RID: 152
			public static class RegistryKeys
			{
				// Token: 0x04000287 RID: 647
				public const string PathToBiosRegistrykey = "HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\System\\BIOS";

				// Token: 0x04000288 RID: 648
				public const string SystemSku = "SystemSKU";

				// Token: 0x04000289 RID: 649
				public const string BIOSVersion = "BIOSVersion";

				// Token: 0x0400028A RID: 650
				public const string SystemFamily = "SystemFamily";

				// Token: 0x0400028B RID: 651
				public const string SystemManufacturer = "SystemManufacturer";
			}

			// Token: 0x02000099 RID: 153
			public static class SkuKeys
			{
				// Token: 0x0400028C RID: 652
				public const string Mt = "MT";

				// Token: 0x0400028D RID: 653
				public const string Family = "FM";

				// Token: 0x0400028E RID: 654
				public const string Brand = "BU";
			}
		}
	}
}
