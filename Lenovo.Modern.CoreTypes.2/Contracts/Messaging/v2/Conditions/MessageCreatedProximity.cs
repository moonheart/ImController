using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Conditions
{
	// Token: 0x0200009A RID: 154
	public sealed class MessageCreatedProximity : IProximityBoundary
	{
		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x00007ACE File Offset: 0x00005CCE
		// (set) Token: 0x060005FD RID: 1533 RVA: 0x00007AD6 File Offset: 0x00005CD6
		[XmlAttribute(AttributeName = "minTimespan")]
		public string MinTimespan { get; set; }

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x00007ADF File Offset: 0x00005CDF
		// (set) Token: 0x060005FF RID: 1535 RVA: 0x00007AE7 File Offset: 0x00005CE7
		[XmlAttribute(AttributeName = "maxTimespan")]
		public string MaxTimespan { get; set; }
	}
}
