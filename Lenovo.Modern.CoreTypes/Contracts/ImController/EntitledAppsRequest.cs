using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ImController
{
	// Token: 0x020000A7 RID: 167
	[XmlRoot(ElementName = "EntitledAppsRequest", Namespace = "")]
	public sealed class EntitledAppsRequest
	{
		// Token: 0x170002CC RID: 716
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x00007C99 File Offset: 0x00005E99
		// (set) Token: 0x0600063F RID: 1599 RVA: 0x00007CA1 File Offset: 0x00005EA1
		[XmlElement("DeviceID")]
		public string DeviceID { get; set; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x00007CAA File Offset: 0x00005EAA
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x00007CB2 File Offset: 0x00005EB2
		[XmlArray("AppList")]
		[XmlArrayItem("App")]
		public App[] AppList { get; set; }
	}
}
