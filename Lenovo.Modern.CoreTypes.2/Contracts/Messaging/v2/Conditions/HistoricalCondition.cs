using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Conditions
{
	// Token: 0x02000091 RID: 145
	public sealed class HistoricalCondition
	{
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x0000799C File Offset: 0x00005B9C
		// (set) Token: 0x060005CD RID: 1485 RVA: 0x000079A4 File Offset: 0x00005BA4
		[XmlElement(ElementName = "Repeat", IsNullable = true)]
		public HistoricalRepeat Repeat { get; set; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x000079AD File Offset: 0x00005BAD
		// (set) Token: 0x060005CF RID: 1487 RVA: 0x000079B5 File Offset: 0x00005BB5
		[XmlElement(ElementName = "DuplicateMessages", IsNullable = true)]
		public HistoricalDuplicateMessages DuplicateMessages { get; set; }
	}
}
