using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Conditions
{
	// Token: 0x02000096 RID: 150
	public sealed class RepeatBoundary
	{
		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x00007A68 File Offset: 0x00005C68
		// (set) Token: 0x060005EA RID: 1514 RVA: 0x00007A70 File Offset: 0x00005C70
		[XmlAttribute(AttributeName = "repeatNumber")]
		public string RepeatNumber { get; set; }

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x00007A79 File Offset: 0x00005C79
		// (set) Token: 0x060005EC RID: 1516 RVA: 0x00007A81 File Offset: 0x00005C81
		[XmlAttribute(AttributeName = "timespanBetween")]
		public string TimespanBetween { get; set; }
	}
}
