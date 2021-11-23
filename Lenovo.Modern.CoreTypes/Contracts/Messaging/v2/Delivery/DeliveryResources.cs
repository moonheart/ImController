using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Delivery
{
	// Token: 0x02000087 RID: 135
	[XmlRoot(ElementName = "DeliveryResources", Namespace = null)]
	public sealed class DeliveryResources
	{
		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x0000774D File Offset: 0x0000594D
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x00007755 File Offset: 0x00005955
		[XmlArray("LocalizationList")]
		[XmlArrayItem("Text")]
		public TextResource[] LocalizationList { get; set; }
	}
}
