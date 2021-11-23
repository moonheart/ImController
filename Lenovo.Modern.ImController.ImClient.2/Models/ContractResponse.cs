using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x02000028 RID: 40
	[XmlRoot(ElementName = "ContractResponse", Namespace = null)]
	public sealed class ContractResponse
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00004B1A File Offset: 0x00002D1A
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00004B22 File Offset: 0x00002D22
		[XmlAttribute(AttributeName = "contractVersion")]
		public string ContractVersion { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00004B2B File Offset: 0x00002D2B
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00004B33 File Offset: 0x00002D33
		[XmlElement(ElementName = "Response")]
		public ResponseData Response { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00004B3C File Offset: 0x00002D3C
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00004B44 File Offset: 0x00002D44
		[XmlElement(ElementName = "FailureData")]
		public FailureData FailureData { get; set; }
	}
}
