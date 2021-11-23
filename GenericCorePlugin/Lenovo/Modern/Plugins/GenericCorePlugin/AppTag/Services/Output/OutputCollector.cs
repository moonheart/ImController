using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.LegacyApp;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.ModernApp;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.Tags;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Output
{
	// Token: 0x02000037 RID: 55
	internal class OutputCollector
	{
		// Token: 0x06000149 RID: 329 RVA: 0x0000A030 File Offset: 0x00008230
		internal OutputCollector(StartMenuAgent legacyAgent, PackageManagerAgent modernAgent, TagManager tagAgent)
		{
			if (legacyAgent == null || (modernAgent == null && tagAgent != null))
			{
				throw new NotImplementedException("OutputCollector dependencies cannot be null");
			}
			this._legacyAgent = legacyAgent;
			this._modernAgent = modernAgent;
			this._tagAgent = tagAgent;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000A061 File Offset: 0x00008261
		public Task<AppAndTagCollection> CollectOutputAsync(bool noApps = false, bool noTags = false)
		{
			return this.FetchInstalledItemsAndTagsAsync(noApps, noTags);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000A06C File Offset: 0x0000826C
		private async Task<AppAndTagCollection> FetchInstalledItemsAndTagsAsync(bool noApps = false, bool noTags = false)
		{
			AppAndTagCollection output = new AppAndTagCollection
			{
				InstalledApps = new InstalledApp[0],
				Tags = new Tag[0]
			};
			AppAndTagCollection result;
			if (noApps && noTags)
			{
				result = output;
			}
			else
			{
				try
				{
					IEnumerable<InstalledApp> installedAppsList = null;
					if (noApps)
					{
						installedAppsList = new List<InstalledApp>();
					}
					else
					{
						List<InstalledApp> installedItems = new List<InstalledApp>();
						List<InstalledApp> list = await this.FetchInstalledLegacyAppsAsync();
						List<InstalledApp> legacyApps = list;
						List<InstalledApp> list2 = await this.FetchInstalledModernAppsAsync();
						installedItems.AddRange(legacyApps ?? new List<InstalledApp>());
						installedItems.AddRange(list2 ?? new List<InstalledApp>());
						installedAppsList = from i in installedItems
							orderby i.Type, i.DateInstalled descending
							select i;
						string message = string.Format("Stats: Modern: {0}, Legacy: {1}", (list2 == null) ? "null" : list2.Count<InstalledApp>().ToString(), (legacyApps == null) ? "null" : legacyApps.Count<InstalledApp>().ToString());
						Logger.Log(Logger.LogSeverity.Information, message);
						installedItems = null;
						legacyApps = null;
					}
					IEnumerable<Tag> source;
					if (noTags)
					{
						source = new List<Tag>();
					}
					else
					{
						IEnumerable<Tag> enumerable = await this.FetchTagsAsync();
						if (enumerable != null)
						{
							source = from i in enumerable
								orderby i.Key, i.Value descending
								select i;
						}
						else
						{
							source = new List<Tag>();
						}
						Logger.Log(Logger.LogSeverity.Information, string.Format("Stats: Tags:{0}", (enumerable == null) ? "null" : enumerable.Count<Tag>().ToString()));
					}
					output.InstalledApps = installedAppsList.ToArray<InstalledApp>();
					output.Tags = source.ToArray<Tag>();
					installedAppsList = null;
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "Unable to retrieve and combine items");
				}
				result = output;
			}
			return result;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000A0C4 File Offset: 0x000082C4
		private async Task<IEnumerable<Tag>> FetchTagsAsync()
		{
			List<Tag> list = new List<Tag>();
			try
			{
				if (this._tagAgent != null)
				{
					IEnumerable<Tag> collection = await this._tagAgent.GetTagListAsync();
					list.AddRange(collection);
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to collect tags");
			}
			return list;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000A10C File Offset: 0x0000830C
		private async Task<List<InstalledApp>> FetchInstalledModernAppsAsync()
		{
			List<InstalledApp> list = new List<InstalledApp>();
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "Starting collection of modern apps");
				if (this._modernAgent != null)
				{
					IEnumerable<ModernAppInformation> enumerable = await this._modernAgent.CollectAppListAsync();
					if (enumerable != null && enumerable.Any<ModernAppInformation>())
					{
						list.AddRange(from modernApp in enumerable
							select new InstalledApp
							{
								DateInstalled = OutputCollector.GetDateInstalled(modernApp),
								Key = modernApp.FamilyName,
								Protocol = ProtocolGenerator.CreateProtocolCommand(modernApp),
								Type = AppType.WindowsStore,
								DisplayName = modernApp.Name,
								Version = modernApp.Version
							});
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to gather installed modern apps");
			}
			return list;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000A154 File Offset: 0x00008354
		private static DateTime GetDateInstalled(ModernAppInformation modernApp)
		{
			DateTime result = DateTime.MinValue;
			try
			{
				if (modernApp != null && !string.IsNullOrWhiteSpace(modernApp.DataDirectory))
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(modernApp.DataDirectory);
					if (directoryInfo.Exists)
					{
						result = directoryInfo.CreationTime;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to get date installed for modern app");
			}
			return result;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000A1B4 File Offset: 0x000083B4
		private async Task<List<InstalledApp>> FetchInstalledLegacyAppsAsync()
		{
			List<InstalledApp> list = new List<InstalledApp>();
			try
			{
				if (this._modernAgent != null)
				{
					IEnumerable<LegacyAppInformation> enumerable = await this._legacyAgent.CollectAppListAsync();
					if (enumerable != null && enumerable.Any<LegacyAppInformation>())
					{
						list.AddRange(from legacyApp in enumerable
							select new InstalledApp
							{
								DateInstalled = legacyApp.DateModified,
								Key = legacyApp.ExecutableName,
								Protocol = ProtocolGenerator.CreateProtocolCommand(legacyApp),
								Type = AppType.WindowsLegacy,
								DisplayName = legacyApp.DisplayName,
								Version = legacyApp.Version
							});
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to gather installed modern apps");
			}
			return list;
		}

		// Token: 0x0400008A RID: 138
		private readonly StartMenuAgent _legacyAgent;

		// Token: 0x0400008B RID: 139
		private readonly PackageManagerAgent _modernAgent;

		// Token: 0x0400008C RID: 140
		private readonly TagManager _tagAgent;
	}
}
