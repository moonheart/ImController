using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Conditions
{
	// Token: 0x02000094 RID: 148
	public sealed class TimeBoundaryCondition
	{
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x000079F1 File Offset: 0x00005BF1
		// (set) Token: 0x060005DA RID: 1498 RVA: 0x000079F9 File Offset: 0x00005BF9
		[XmlElement(ElementName = "DateRange", IsNullable = true)]
		public DateRangeBoundary DateRange { get; set; }

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x00007A02 File Offset: 0x00005C02
		// (set) Token: 0x060005DC RID: 1500 RVA: 0x00007A0A File Offset: 0x00005C0A
		[XmlElement(ElementName = "Repeat", IsNullable = true)]
		public RepeatBoundary Repeat { get; set; }

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x00007A13 File Offset: 0x00005C13
		// (set) Token: 0x060005DE RID: 1502 RVA: 0x00007A1B File Offset: 0x00005C1B
		[XmlElement(ElementName = "OobeProximity", IsNullable = true)]
		public OobeProximity OobeProximity { get; set; }

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x00007A24 File Offset: 0x00005C24
		// (set) Token: 0x060005E0 RID: 1504 RVA: 0x00007A2C File Offset: 0x00005C2C
		[XmlElement(ElementName = "MessageCreatedProximity", IsNullable = true)]
		public MessageCreatedProximity MessageCreatedProximity { get; set; }

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x00007A35 File Offset: 0x00005C35
		// (set) Token: 0x060005E2 RID: 1506 RVA: 0x00007A3D File Offset: 0x00005C3D
		[XmlElement(ElementName = "MessageReceivedProximity", IsNullable = true)]
		public MessageReceivedProximity MessageReceivedProximity { get; set; }
	}
}
