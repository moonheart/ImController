using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Conditions
{
	// Token: 0x02000099 RID: 153
	public sealed class MessageReceivedProximity : IProximityBoundary
	{
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x00007AAC File Offset: 0x00005CAC
		// (set) Token: 0x060005F8 RID: 1528 RVA: 0x00007AB4 File Offset: 0x00005CB4
		[XmlAttribute(AttributeName = "minTimespan")]
		public string MinTimespan { get; set; }

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x00007ABD File Offset: 0x00005CBD
		// (set) Token: 0x060005FA RID: 1530 RVA: 0x00007AC5 File Offset: 0x00005CC5
		[XmlAttribute(AttributeName = "maxTimespan")]
		public string MaxTimespan { get; set; }
	}
}
