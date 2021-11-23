using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents.CloudTags;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.Tags
{
	// Token: 0x02000033 RID: 51
	internal class TagManager
	{
		// Token: 0x0600013C RID: 316 RVA: 0x00009824 File Offset: 0x00007A24
		internal TagManager()
		{
			this._tagAgents = new List<ITagAgent>
			{
				new HardwareTagAgent(),
				new SoftwareTagAgent(),
				new ManualProgramDetectionTagAgent(),
				new SystemOsTagAgent(),
				new RegistryTagAgent(),
				new EnterpriseTagDetector(),
				new WindowsCloudTagDetector(),
				new CloudTagManager(),
				new UDCTagAgent()
			};
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000098A8 File Offset: 0x00007AA8
		public async Task<IEnumerable<Tag>> GetTagListAsync()
		{
			List<Tag> list = new List<Tag>();
			if (this._tagAgents != null && this._tagAgents.Any<ITagAgent>())
			{
				Logger.Log(Logger.LogSeverity.Information, "Starting collection of tags from TagAgents");
				foreach (ITagAgent tagAgent in this._tagAgents)
				{
					try
					{
						IEnumerable<Tag> enumerable = await tagAgent.CollectTagsAsync();
						if (enumerable != null && enumerable.Any<Tag>())
						{
							list.AddRange(enumerable);
						}
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "Unable to collect tags from TagAgent");
					}
				}
				IEnumerator<ITagAgent> enumerator = null;
			}
			return list;
		}

		// Token: 0x04000089 RID: 137
		private readonly IList<ITagAgent> _tagAgents;
	}
}
