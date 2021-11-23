using System;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Model;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents.CloudTags
{
	// Token: 0x02000030 RID: 48
	internal interface ICloudTagProvider
	{
		// Token: 0x06000138 RID: 312
		Task<Tag> GetCloudTagAsync(TagRule tagRule, string tagName);
	}
}
