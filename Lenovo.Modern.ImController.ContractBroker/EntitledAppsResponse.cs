using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ContractBroker
{
	// Token: 0x02000006 RID: 6
	[XmlRoot(ElementName = "EntitledAppsResponse", Namespace = null)]
	public sealed class EntitledAppsResponse
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001A RID: 26 RVA: 0x0000222F File Offset: 0x0000042F
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00002237 File Offset: 0x00000437
		[XmlElement("UdcVersion")]
		public string UdcVersion { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002240 File Offset: 0x00000440
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002248 File Offset: 0x00000448
		[XmlArray("AppList")]
		[XmlArrayItem("App")]
		public App[] AppList { get; set; }
	}
}
