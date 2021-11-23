using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.Registry
{
	// Token: 0x02000032 RID: 50
	[XmlRoot(ElementName = "RegistryEventReaction", Namespace = null)]
	public sealed class RegistryEventReaction
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00004D69 File Offset: 0x00002F69
		// (set) Token: 0x06000221 RID: 545 RVA: 0x00004D71 File Offset: 0x00002F71
		[XmlElement(ElementName = "RegistryHiveName")]
		public string RegistryHiveName { get; set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00004D7A File Offset: 0x00002F7A
		// (set) Token: 0x06000223 RID: 547 RVA: 0x00004D82 File Offset: 0x00002F82
		[XmlElement(ElementName = "KeyPath")]
		public string KeyPath { get; set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00004D8B File Offset: 0x00002F8B
		// (set) Token: 0x06000225 RID: 549 RVA: 0x00004D93 File Offset: 0x00002F93
		[XmlElement(ElementName = "ValueName")]
		public string ValueName { get; set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00004D9C File Offset: 0x00002F9C
		// (set) Token: 0x06000227 RID: 551 RVA: 0x00004DA4 File Offset: 0x00002FA4
		[XmlElement(ElementName = "MonitorRegTree")]
		public bool MonitorRegTree { get; set; }
	}
}
