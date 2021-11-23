using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ContractBroker
{
	// Token: 0x02000005 RID: 5
	[XmlRoot(ElementName = "EntitledResponse", Namespace = null)]
	public sealed class EntitledResponse
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000021FC File Offset: 0x000003FC
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002204 File Offset: 0x00000404
		[XmlElement("Result")]
		public bool Result { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000220D File Offset: 0x0000040D
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002215 File Offset: 0x00000415
		[XmlArray("CampaignTagList")]
		[XmlArrayItem("CampaignTag")]
		public string[] CampaignTagList { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000221E File Offset: 0x0000041E
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002226 File Offset: 0x00000426
		[XmlElement("UdcVersion")]
		public string UdcVersion { get; set; }
	}
}
