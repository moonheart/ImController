using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ContractBroker
{
	// Token: 0x02000004 RID: 4
	[XmlRoot(ElementName = "EntitledAppsRequest", Namespace = "")]
	public sealed class EntitledAppsRequest
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000021B8 File Offset: 0x000003B8
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000021C0 File Offset: 0x000003C0
		[XmlElement("DeviceID")]
		public string DeviceID { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000021C9 File Offset: 0x000003C9
		// (set) Token: 0x0600000D RID: 13 RVA: 0x000021D1 File Offset: 0x000003D1
		[XmlElement("AppName")]
		public string AppName { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000021DA File Offset: 0x000003DA
		// (set) Token: 0x0600000F RID: 15 RVA: 0x000021E2 File Offset: 0x000003E2
		[XmlElement("AppVersion")]
		public string AppVersion { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000021EB File Offset: 0x000003EB
		// (set) Token: 0x06000011 RID: 17 RVA: 0x000021F3 File Offset: 0x000003F3
		[XmlArray("AppList")]
		[XmlArrayItem("App")]
		public App[] AppList { get; set; }
	}
}
