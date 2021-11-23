using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.Tags
{
	// Token: 0x02000032 RID: 50
	internal interface ITagAgent
	{
		// Token: 0x0600013B RID: 315
		Task<IEnumerable<Tag>> CollectTagsAsync();
	}
}
