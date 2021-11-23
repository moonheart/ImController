using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Capabilities.CapabilityResponse
{
	// Token: 0x020000BC RID: 188
	[XmlRoot(ElementName = "Capability", Namespace = null)]
	public sealed class Capability
	{
		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x000082F3 File Offset: 0x000064F3
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x000082FB File Offset: 0x000064FB
		[XmlAttribute(AttributeName = "Key")]
		public string Key { get; set; }

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x00008304 File Offset: 0x00006504
		// (set) Token: 0x060006EA RID: 1770 RVA: 0x0000830C File Offset: 0x0000650C
		[XmlText]
		public string Keyvalue { get; set; }
	}
}
