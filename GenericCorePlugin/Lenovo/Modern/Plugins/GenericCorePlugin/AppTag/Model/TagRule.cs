using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Model
{
	// Token: 0x0200003B RID: 59
	public class TagRule
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600015C RID: 348 RVA: 0x0000A277 File Offset: 0x00008477
		// (set) Token: 0x0600015D RID: 349 RVA: 0x0000A27F File Offset: 0x0000847F
		[XmlAttribute(AttributeName = "tgtType")]
		public string TargetType { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600015E RID: 350 RVA: 0x0000A288 File Offset: 0x00008488
		// (set) Token: 0x0600015F RID: 351 RVA: 0x0000A290 File Offset: 0x00008490
		[XmlAttribute(AttributeName = "tgtPath")]
		public string TargetPath { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000A299 File Offset: 0x00008499
		// (set) Token: 0x06000161 RID: 353 RVA: 0x0000A2A1 File Offset: 0x000084A1
		[XmlAttribute(AttributeName = "tgtName")]
		public string TargetName { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000A2AA File Offset: 0x000084AA
		// (set) Token: 0x06000163 RID: 355 RVA: 0x0000A2B2 File Offset: 0x000084B2
		[XmlAttribute(AttributeName = "tgtReqValue")]
		public string TargetReqValue { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000164 RID: 356 RVA: 0x0000A2BB File Offset: 0x000084BB
		// (set) Token: 0x06000165 RID: 357 RVA: 0x0000A2C3 File Offset: 0x000084C3
		[XmlAttribute(AttributeName = "tagValueRule")]
		public string TargetValueRule { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000166 RID: 358 RVA: 0x0000A2CC File Offset: 0x000084CC
		// (set) Token: 0x06000167 RID: 359 RVA: 0x0000A2D4 File Offset: 0x000084D4
		[XmlAttribute(AttributeName = "tagValueCondition")]
		public string TargetValueCondition { get; set; }
	}
}
