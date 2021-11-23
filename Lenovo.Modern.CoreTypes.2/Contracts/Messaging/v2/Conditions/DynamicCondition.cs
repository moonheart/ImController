using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Conditions
{
	// Token: 0x02000090 RID: 144
	public sealed class DynamicCondition
	{
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x00007936 File Offset: 0x00005B36
		// (set) Token: 0x060005C0 RID: 1472 RVA: 0x0000793E File Offset: 0x00005B3E
		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x00007947 File Offset: 0x00005B47
		// (set) Token: 0x060005C2 RID: 1474 RVA: 0x0000794F File Offset: 0x00005B4F
		[XmlAttribute(AttributeName = "path")]
		public string Path { get; set; }

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x00007958 File Offset: 0x00005B58
		// (set) Token: 0x060005C4 RID: 1476 RVA: 0x00007960 File Offset: 0x00005B60
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x00007969 File Offset: 0x00005B69
		// (set) Token: 0x060005C6 RID: 1478 RVA: 0x00007971 File Offset: 0x00005B71
		[XmlAttribute(AttributeName = "matchValue")]
		public string MatchValue { get; set; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0000797A File Offset: 0x00005B7A
		// (set) Token: 0x060005C8 RID: 1480 RVA: 0x00007982 File Offset: 0x00005B82
		[XmlAttribute(AttributeName = "isNegated")]
		public bool IsNegated { get; set; }

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x0000798B File Offset: 0x00005B8B
		// (set) Token: 0x060005CA RID: 1482 RVA: 0x00007993 File Offset: 0x00005B93
		[XmlText]
		public string Value { get; set; }
	}
}
