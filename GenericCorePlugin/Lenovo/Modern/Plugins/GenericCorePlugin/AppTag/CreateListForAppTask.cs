using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Output;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.LegacyApp;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.ModernApp;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.Tags;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag
{
	// Token: 0x0200001E RID: 30
	internal class CreateListForAppTask
	{
		// Token: 0x060000EE RID: 238 RVA: 0x00002048 File Offset: 0x00000248
		internal CreateListForAppTask()
		{
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00007D9C File Offset: 0x00005F9C
		internal async Task<bool> GenerateAndSaveAsync(IEnumerable<DirectoryInfo> outputDirectories)
		{
			bool wasWritten = false;
			try
			{
				if (outputDirectories != null && outputDirectories.Any<DirectoryInfo>())
				{
					Logger.Log(Logger.LogSeverity.Information, "Starting collection for <{0}> output locations", new object[] { outputDirectories.Count<DirectoryInfo>() });
					PackageManagerAgent modernAgent = new PackageManagerAgent();
					StartMenuAgent legacyAgent = new StartMenuAgent();
					TagManager tagAgent = new TagManager();
					OutputCollector outputCollector = new OutputCollector(legacyAgent, modernAgent, tagAgent);
					OutputWriter writer = new OutputWriter();
					AppAndTagCollection appAndTagCollection = await outputCollector.CollectOutputAsync(false, false);
					AppAndTagCollection items = appAndTagCollection;
					if (items != null)
					{
						foreach (DirectoryInfo outputLocation in outputDirectories)
						{
							if (outputLocation != null)
							{
								try
								{
									Logger.Log(Logger.LogSeverity.Information, "Writing to file location: <{0}>", new object[] { outputLocation.FullName });
									wasWritten = await writer.WriteFileAsync(items, outputLocation);
								}
								catch (Exception ex)
								{
									Logger.Log(ex, "Unable to write to file location: <{0}>", new object[] { outputLocation.FullName });
								}
							}
							outputLocation = null;
						}
						IEnumerator<DirectoryInfo> enumerator = null;
					}
					writer = null;
					items = null;
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "Unable to generate list and save");
			}
			return wasWritten;
		}
	}
}
