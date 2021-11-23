using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.AppTagList
{
	// Token: 0x02000018 RID: 24
	[XmlRoot(ElementName = "InstalledAppsAndTagsInfo", Namespace = null)]
	public sealed class AppAndTagCollection
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600011B RID: 283 RVA: 0x0000410C File Offset: 0x0000230C
		// (set) Token: 0x0600011C RID: 284 RVA: 0x00004114 File Offset: 0x00002314
		[XmlArray("InstalledAppList")]
		[XmlArrayItem("InstalledApp")]
		public InstalledApp[] InstalledApps { get; set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600011D RID: 285 RVA: 0x0000411D File Offset: 0x0000231D
		// (set) Token: 0x0600011E RID: 286 RVA: 0x00004125 File Offset: 0x00002325
		[XmlArray("TagList")]
		[XmlArrayItem("Tag")]
		public Tag[] Tags { get; set; }

		// Token: 0x0600011F RID: 287 RVA: 0x00004130 File Offset: 0x00002330
		public static IEnumerable<KeyValuePair<string, string>> ExtractCollecitonOfKeys(AppAndTagCollection instance)
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			if (instance != null)
			{
				if (instance.InstalledApps != null)
				{
					foreach (InstalledApp installedApp in instance.InstalledApps)
					{
						if (installedApp.Version != null)
						{
							list.Add(new KeyValuePair<string, string>(installedApp.Key, installedApp.Version.ToString()));
						}
						else
						{
							list.Add(new KeyValuePair<string, string>(installedApp.Key, null));
						}
					}
				}
				if (instance.Tags != null)
				{
					list.AddRange(from t in instance.Tags
						select new KeyValuePair<string, string>(t.Key, t.Value));
				}
			}
			return list;
		}
	}
}
