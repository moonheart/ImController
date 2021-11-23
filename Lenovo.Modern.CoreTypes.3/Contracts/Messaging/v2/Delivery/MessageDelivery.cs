using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Delivery
{
	// Token: 0x0200008C RID: 140
	[XmlRoot(ElementName = "MessageDelivery", Namespace = null)]
	public sealed class MessageDelivery
	{
		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x0000786C File Offset: 0x00005A6C
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x00007874 File Offset: 0x00005A74
		[XmlElement(ElementName = "Resources")]
		public DeliveryResources Resources { get; set; }

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x0000787D File Offset: 0x00005A7D
		// (set) Token: 0x060005A8 RID: 1448 RVA: 0x00007885 File Offset: 0x00005A85
		[XmlElement(ElementName = "Visualization")]
		public VisualizationOptions Visualization { get; set; }
	}
}
