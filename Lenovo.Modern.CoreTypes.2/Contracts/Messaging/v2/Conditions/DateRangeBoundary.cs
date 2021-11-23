using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Conditions
{
	// Token: 0x02000095 RID: 149
	public sealed class DateRangeBoundary
	{
		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x00007A46 File Offset: 0x00005C46
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x00007A4E File Offset: 0x00005C4E
		[XmlAttribute(AttributeName = "startDate")]
		public string StartDateTime { get; set; }

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x00007A57 File Offset: 0x00005C57
		// (set) Token: 0x060005E7 RID: 1511 RVA: 0x00007A5F File Offset: 0x00005C5F
		[XmlAttribute(AttributeName = "stopDate")]
		public string StopDateTime { get; set; }
	}
}
