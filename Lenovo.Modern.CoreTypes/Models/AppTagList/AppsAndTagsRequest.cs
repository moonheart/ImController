using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.AppTagList
{
	// Token: 0x0200001E RID: 30
	[XmlRoot(ElementName = "AppsAndTagsRequest", Namespace = null)]
	public sealed class AppsAndTagsRequest
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000044D9 File Offset: 0x000026D9
		// (set) Token: 0x0600015D RID: 349 RVA: 0x000044E1 File Offset: 0x000026E1
		[XmlAttribute(AttributeName = "IgnoreCache")]
		public bool IgnoreCache { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000044EA File Offset: 0x000026EA
		// (set) Token: 0x0600015F RID: 351 RVA: 0x000044F2 File Offset: 0x000026F2
		[XmlAttribute(AttributeName = "noApps")]
		public bool noApps { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000160 RID: 352 RVA: 0x000044FB File Offset: 0x000026FB
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00004503 File Offset: 0x00002703
		[XmlAttribute(AttributeName = "noTags")]
		public bool noTags { get; set; }
	}
}
