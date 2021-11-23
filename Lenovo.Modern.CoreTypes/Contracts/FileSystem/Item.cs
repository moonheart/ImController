using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.FileSystem
{
	// Token: 0x020000B9 RID: 185
	[XmlRoot(ElementName = "Item", Namespace = null)]
	public sealed class Item
	{
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x0000828D File Offset: 0x0000648D
		// (set) Token: 0x060006D9 RID: 1753 RVA: 0x00008295 File Offset: 0x00006495
		[XmlAttribute(AttributeName = "Type")]
		public string Type { get; set; }

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x0000829E File Offset: 0x0000649E
		// (set) Token: 0x060006DB RID: 1755 RVA: 0x000082A6 File Offset: 0x000064A6
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x000082AF File Offset: 0x000064AF
		// (set) Token: 0x060006DD RID: 1757 RVA: 0x000082B7 File Offset: 0x000064B7
		[XmlAttribute(AttributeName = "FileSize")]
		public double FileSize { get; set; }

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x000082C0 File Offset: 0x000064C0
		// (set) Token: 0x060006DF RID: 1759 RVA: 0x000082C8 File Offset: 0x000064C8
		[XmlAttribute(AttributeName = "FullPath")]
		public string FullPath { get; set; }
	}
}
