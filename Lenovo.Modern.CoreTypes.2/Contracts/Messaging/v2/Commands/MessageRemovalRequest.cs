using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Commands
{
	// Token: 0x0200009E RID: 158
	[XmlRoot(ElementName = "MessageQueueRequest", Namespace = null)]
	public sealed class MessageRemovalRequest
	{
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x00007B67 File Offset: 0x00005D67
		// (set) Token: 0x06000612 RID: 1554 RVA: 0x00007B6F File Offset: 0x00005D6F
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x00007B78 File Offset: 0x00005D78
		// (set) Token: 0x06000614 RID: 1556 RVA: 0x00007B80 File Offset: 0x00005D80
		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x00007B89 File Offset: 0x00005D89
		// (set) Token: 0x06000616 RID: 1558 RVA: 0x00007B91 File Offset: 0x00005D91
		[XmlAttribute(AttributeName = "associatedApp")]
		public string AssociatedApp { get; set; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x00007B9A File Offset: 0x00005D9A
		// (set) Token: 0x06000618 RID: 1560 RVA: 0x00007BA2 File Offset: 0x00005DA2
		[XmlAttribute(AttributeName = "requestingComponent")]
		public string RequestingComponent { get; set; }
	}
}
