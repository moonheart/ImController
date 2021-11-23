using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Conditions
{
	// Token: 0x02000098 RID: 152
	public sealed class OobeProximity : IProximityBoundary
	{
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x00007A8A File Offset: 0x00005C8A
		// (set) Token: 0x060005F3 RID: 1523 RVA: 0x00007A92 File Offset: 0x00005C92
		[XmlAttribute(AttributeName = "minTimespan")]
		public string MinTimespan { get; set; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x00007A9B File Offset: 0x00005C9B
		// (set) Token: 0x060005F5 RID: 1525 RVA: 0x00007AA3 File Offset: 0x00005CA3
		[XmlAttribute(AttributeName = "maxTimespan")]
		public string MaxTimespan { get; set; }
	}
}
