using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x02000023 RID: 35
	[XmlRoot(ElementName = "BrokerRequest", Namespace = null)]
	public sealed class BrokerRequest
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x000048A2 File Offset: 0x00002AA2
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x000048AA File Offset: 0x00002AAA
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x000048B3 File Offset: 0x00002AB3
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x000048BB File Offset: 0x00002ABB
		[XmlElement(ElementName = "Requirements")]
		public BrokerRequirements BrokerRequirements { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x000048C4 File Offset: 0x00002AC4
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x000048CC File Offset: 0x00002ACC
		[XmlElement(ElementName = "ContractRequest")]
		public ContractRequest ContractRequest { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x000048D5 File Offset: 0x00002AD5
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x000048DD File Offset: 0x00002ADD
		[XmlElement(ElementName = "Authentication")]
		public BrokerAuthentication Authentication { get; set; }
	}
}
