using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Commands
{
	// Token: 0x020000A4 RID: 164
	[XmlRoot(ElementName = "NotificationAction", Namespace = null)]
	public sealed class NotificationAction
	{
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x00007C44 File Offset: 0x00005E44
		// (set) Token: 0x06000632 RID: 1586 RVA: 0x00007C4C File Offset: 0x00005E4C
		[XmlAttribute(AttributeName = "notificationId")]
		public string NotificationID { get; set; }

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x00007C55 File Offset: 0x00005E55
		// (set) Token: 0x06000634 RID: 1588 RVA: 0x00007C5D File Offset: 0x00005E5D
		[XmlAttribute(AttributeName = "notificationArguments")]
		public string NotificationArguments { get; set; }

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x00007C66 File Offset: 0x00005E66
		// (set) Token: 0x06000636 RID: 1590 RVA: 0x00007C6E File Offset: 0x00005E6E
		[XmlAttribute(AttributeName = "plugin")]
		public string PluginName { get; set; }
	}
}
