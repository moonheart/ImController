using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Commands
{
	// Token: 0x0200009F RID: 159
	[XmlRoot(ElementName = "MessageRequestResult", Namespace = null)]
	public sealed class MessageRequestResult
	{
		// Token: 0x170002BE RID: 702
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x00007BAB File Offset: 0x00005DAB
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x00007BB3 File Offset: 0x00005DB3
		[XmlAttribute(AttributeName = "result")]
		public Result Result { get; set; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00007BBC File Offset: 0x00005DBC
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x00007BC4 File Offset: 0x00005DC4
		[XmlAttribute(AttributeName = "description")]
		public string Description { get; set; }
	}
}
