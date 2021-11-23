using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;
using Lenovo.Modern.CoreTypes.Models;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.CoreTypes.Services;
using Lenovo.Modern.ImController.Shared.Model.Subscription;
using Lenovo.Modern.ImController.Shared.Services.Contracts.SystemInformation.AppTag;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Network;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x0200001E RID: 30
	public class SubscriptionSettingsAgent
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x00005A98 File Offset: 0x00003C98
		private SubscriptionSettingsAgent()
		{
			this._subscriptionManager = SubscriptionManager.GetInstance(new NetworkAgent());
			this._machineInformationManager = MachineInformationManager.GetInstance();
			this._appTagManager = AppAndTagManager.GetInstance();
			this._workingSemaphore = new SemaphoreSlim(1);
			if (this._subscriptionManager == null || this._machineInformationManager == null || this._appTagManager == null)
			{
				throw new ArgumentNullException("Dependencies for SubscriptionSettingsAgent are invalid or null");
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005B00 File Offset: 0x00003D00
		public static SubscriptionSettingsAgent GetInstance()
		{
			SubscriptionSettingsAgent result;
			if ((result = SubscriptionSettingsAgent._instance) == null)
			{
				result = (SubscriptionSettingsAgent._instance = new SubscriptionSettingsAgent());
			}
			return result;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00005B18 File Offset: 0x00003D18
		private async Task InitializeIfNecessaryAsync(CancellationToken cancelToken)
		{
			try
			{
				await this._workingSemaphore.WaitAsync();
				Func<bool> isAllCachedInfoAvailable = () => this._cachedSubscription != null && this._cachedMachineInformation != null && this._cachedAppTagCollection != null;
				if (!cancelToken.IsCancellationRequested)
				{
					if (!isAllCachedInfoAvailable())
					{
						this._cachedSubscription = await this._subscriptionManager.GetSubscriptionAsync(cancelToken);
						this._cachedMachineInformation = await this._machineInformationManager.GetMachineInformationAsync(cancelToken);
						this._cachedAppTagCollection = await this._appTagManager.GetAppAndTagsAsync(cancelToken);
					}
					if (!isAllCachedInfoAvailable())
					{
						throw new InvalidOperationException("SubscriptionSettingsAgent: Unable to initialize dependency information");
					}
					isAllCachedInfoAvailable = null;
				}
			}
			finally
			{
				this._workingSemaphore.Release();
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005B68 File Offset: 0x00003D68
		public async Task<Setting> GetApplicableSettingAsync(string settingName, CancellationToken cancelToken)
		{
			await this.InitializeIfNecessaryAsync(cancelToken);
			Setting result;
			if (cancelToken.IsCancellationRequested)
			{
				result = null;
			}
			else if (this._cachedSubscription.Service == null || this._cachedSubscription.Service.SettingsList == null || !this._cachedSubscription.Service.SettingsList.Any<Setting>())
			{
				result = null;
			}
			else
			{
				IEnumerable<Setting> enumerable = from s in this._cachedSubscription.Service.SettingsList
					where s != null && !string.IsNullOrWhiteSpace(s.Key) && !string.IsNullOrWhiteSpace(s.Value) && s.Key.Equals(settingName, StringComparison.InvariantCultureIgnoreCase)
					select s;
				if (!enumerable.Any<Setting>())
				{
					result = null;
				}
				else
				{
					IBasicEligabilityRequirements firstApplicableMatch = EligibilityFilter.GetFirstApplicableMatch(enumerable, this._cachedMachineInformation, this._cachedAppTagCollection, Constants.ImControllerVersion);
					if (firstApplicableMatch == null)
					{
						Logger.Log(Logger.LogSeverity.Information, "SettingsAgent: NO applicable REQUIREMENTS found matching " + settingName);
						result = null;
					}
					else
					{
						Setting setting = firstApplicableMatch as Setting;
						if (setting == null)
						{
							Logger.Log(Logger.LogSeverity.Information, "SettingsAgent: NO applicable SETTING found matching " + settingName);
							result = null;
						}
						else
						{
							result = setting;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04000076 RID: 118
		private readonly ISubscriptionManager _subscriptionManager;

		// Token: 0x04000077 RID: 119
		private readonly IMachineInformationManager _machineInformationManager;

		// Token: 0x04000078 RID: 120
		private readonly IAppAndTagManager _appTagManager;

		// Token: 0x04000079 RID: 121
		private readonly SemaphoreSlim _workingSemaphore;

		// Token: 0x0400007A RID: 122
		private MachineInformation _cachedMachineInformation;

		// Token: 0x0400007B RID: 123
		private AppAndTagCollection _cachedAppTagCollection;

		// Token: 0x0400007C RID: 124
		private PackageSubscription _cachedSubscription;

		// Token: 0x0400007D RID: 125
		private static SubscriptionSettingsAgent _instance;
	}
}
