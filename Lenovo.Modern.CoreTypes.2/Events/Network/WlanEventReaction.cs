using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.Network
{
	// Token: 0x02000038 RID: 56
	[XmlRoot(ElementName = "WlanEventReaction", Namespace = null)]
	public sealed class WlanEventReaction
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00005015 File Offset: 0x00003215
		// (set) Token: 0x0600025F RID: 607 RVA: 0x0000501D File Offset: 0x0000321D
		[XmlElement(ElementName = "NotificationSource")]
		public int NotificationSource { get; set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00005026 File Offset: 0x00003226
		// (set) Token: 0x06000261 RID: 609 RVA: 0x0000502E File Offset: 0x0000322E
		[XmlElement(ElementName = "NotificationCode")]
		public int NotificationCode { get; set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000262 RID: 610 RVA: 0x00005037 File Offset: 0x00003237
		// (set) Token: 0x06000263 RID: 611 RVA: 0x0000503F File Offset: 0x0000323F
		[XmlElement(ElementName = "InterfaceGuid")]
		public string InterfaceGuid { get; set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000264 RID: 612 RVA: 0x00005048 File Offset: 0x00003248
		// (set) Token: 0x06000265 RID: 613 RVA: 0x00005050 File Offset: 0x00003250
		[XmlElement(ElementName = "DwDataSize")]
		public int DwDataSize { get; set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000266 RID: 614 RVA: 0x00005059 File Offset: 0x00003259
		// (set) Token: 0x06000267 RID: 615 RVA: 0x00005061 File Offset: 0x00003261
		[XmlElement(ElementName = "Data", DataType = "base64Binary")]
		public byte[] Data { get; set; }
	}
}
