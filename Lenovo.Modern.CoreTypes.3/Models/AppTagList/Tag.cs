using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.AppTagList
{
	// Token: 0x02000020 RID: 32
	[XmlRoot(ElementName = "Tag", Namespace = null)]
	public sealed class Tag
	{
		// Token: 0x06000168 RID: 360 RVA: 0x0000452E File Offset: 0x0000272E
		public Tag()
		{
			this.Key = string.Empty;
			this.Value = string.Empty;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000454C File Offset: 0x0000274C
		public Tag(string tagKey, string tagValue)
		{
			this.Key = tagKey;
			this.Value = tagValue;
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00004562 File Offset: 0x00002762
		// (set) Token: 0x0600016B RID: 363 RVA: 0x0000456A File Offset: 0x0000276A
		[XmlAttribute(AttributeName = "key")]
		public string Key { get; set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00004573 File Offset: 0x00002773
		// (set) Token: 0x0600016D RID: 365 RVA: 0x0000457B File Offset: 0x0000277B
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
	}
}
