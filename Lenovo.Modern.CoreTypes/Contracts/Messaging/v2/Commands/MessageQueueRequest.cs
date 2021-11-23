using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Commands
{
	// Token: 0x0200009C RID: 156
	[XmlRoot(ElementName = "MessageQueueRequest", Namespace = null)]
	public sealed class MessageQueueRequest
	{
		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x00007AF0 File Offset: 0x00005CF0
		// (set) Token: 0x06000602 RID: 1538 RVA: 0x00007AF8 File Offset: 0x00005CF8
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x00007B01 File Offset: 0x00005D01
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x00007B09 File Offset: 0x00005D09
		[XmlAttribute(AttributeName = "associatedApp")]
		public string AssociatedApp { get; set; }

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x00007B12 File Offset: 0x00005D12
		// (set) Token: 0x06000606 RID: 1542 RVA: 0x00007B1A File Offset: 0x00005D1A
		[XmlAttribute(AttributeName = "requestingComponent")]
		public string RequestingComponent { get; set; }

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x00007B23 File Offset: 0x00005D23
		// (set) Token: 0x06000608 RID: 1544 RVA: 0x00007B2B File Offset: 0x00005D2B
		[XmlAttribute(AttributeName = "fileLocation")]
		public string FileLocation { get; set; }

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x00007B34 File Offset: 0x00005D34
		// (set) Token: 0x0600060A RID: 1546 RVA: 0x00007B3C File Offset: 0x00005D3C
		[XmlArray(ElementName = "Resources")]
		[XmlArrayItem(ElementName = "Substitution")]
		public Substitution[] Resources { get; set; }
	}
}
