using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Capabilities.CapabilityResponse
{
	// Token: 0x020000BD RID: 189
	[XmlRoot(ElementName = "CapabilityResponse", Namespace = null)]
	public sealed class CapabilityResponse
	{
		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x00008315 File Offset: 0x00006515
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x0000831D File Offset: 0x0000651D
		[XmlArray("CapabilityList")]
		[XmlArrayItem("Capability")]
		public Capability[] CapabilityList { get; set; }
	}
}
