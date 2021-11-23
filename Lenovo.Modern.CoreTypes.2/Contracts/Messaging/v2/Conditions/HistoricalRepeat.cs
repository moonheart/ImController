using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Conditions
{
	// Token: 0x02000092 RID: 146
	public sealed class HistoricalRepeat
	{
		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x000079BE File Offset: 0x00005BBE
		// (set) Token: 0x060005D2 RID: 1490 RVA: 0x000079C6 File Offset: 0x00005BC6
		[XmlAttribute(AttributeName = "repeatNumber")]
		public string RepeatNumber { get; set; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x000079CF File Offset: 0x00005BCF
		// (set) Token: 0x060005D4 RID: 1492 RVA: 0x000079D7 File Offset: 0x00005BD7
		[XmlAttribute(AttributeName = "daysBetweenRepeat")]
		public string DaysBetweenRepeat { get; set; }
	}
}
