using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Delivery
{
	// Token: 0x02000088 RID: 136
	[XmlRoot(ElementName = "TextResource", Namespace = null)]
	public sealed class TextResource
	{
		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0000775E File Offset: 0x0000595E
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x00007766 File Offset: 0x00005966
		[XmlAttribute(AttributeName = "lang")]
		public string Lang { get; set; }

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0000776F File Offset: 0x0000596F
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x00007777 File Offset: 0x00005977
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x00007780 File Offset: 0x00005980
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x00007788 File Offset: 0x00005988
		[XmlText]
		public string Value { get; set; }
	}
}
