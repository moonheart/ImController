using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.Tags;
using Microsoft.Win32;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents
{
	// Token: 0x02000022 RID: 34
	internal class RegistryTagAgent : ITagAgent
	{
		// Token: 0x06000103 RID: 259 RVA: 0x000084C4 File Offset: 0x000066C4
		public Task<IEnumerable<Tag>> CollectTagsAsync()
		{
			List<Tag> list = new List<Tag>();
			RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
			if (registryKey != null)
			{
				RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\WOW6432Node\\Lenovo\\ImController\\Applicability\\Tags");
				if (registryKey2 != null)
				{
					foreach (string text in registryKey2.GetValueNames())
					{
						list.Add(new Tag
						{
							Key = text,
							Value = registryKey2.GetValue(text).ToString()
						});
					}
				}
				registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Lenovo\\ImController\\Applicability\\Tags");
				if (registryKey2 != null)
				{
					foreach (string text2 in registryKey2.GetValueNames())
					{
						list.Add(new Tag
						{
							Key = text2,
							Value = registryKey2.GetValue(text2).ToString()
						});
					}
				}
			}
			return Task.FromResult<IEnumerable<Tag>>(list);
		}

		// Token: 0x04000062 RID: 98
		private const string tagLocation = "SOFTWARE\\Lenovo\\ImController\\Applicability\\Tags";

		// Token: 0x04000063 RID: 99
		private const string tagLocation6432 = "SOFTWARE\\WOW6432Node\\Lenovo\\ImController\\Applicability\\Tags";
	}
}
