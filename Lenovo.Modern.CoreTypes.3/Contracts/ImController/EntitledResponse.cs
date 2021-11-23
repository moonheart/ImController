using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ImController
{
	// Token: 0x020000A9 RID: 169
	[XmlRoot(ElementName = "EntitledResponse", Namespace = null)]
	public sealed class EntitledResponse
	{
		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x00007DA9 File Offset: 0x00005FA9
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x00007DB1 File Offset: 0x00005FB1
		[XmlElement("Result")]
		public bool Result { get; set; }

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x00007DBA File Offset: 0x00005FBA
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x00007DC2 File Offset: 0x00005FC2
		[XmlArray("CampaignTagList")]
		[XmlArrayItem("CampaignTag")]
		public string[] CampaignTagList { get; set; }
	}
}
