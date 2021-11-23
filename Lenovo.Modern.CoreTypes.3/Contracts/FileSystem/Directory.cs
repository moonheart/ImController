using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.FileSystem
{
	// Token: 0x020000B6 RID: 182
	[XmlRoot(ElementName = "Directory", Namespace = null)]
	public sealed class Directory
	{
		// Token: 0x1700030D RID: 781
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x00008249 File Offset: 0x00006449
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x00008251 File Offset: 0x00006451
		[XmlAttribute(AttributeName = "Location")]
		public string Location { get; set; }

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x0000825A File Offset: 0x0000645A
		// (set) Token: 0x060006D0 RID: 1744 RVA: 0x00008262 File Offset: 0x00006462
		[XmlArray("Directory")]
		[XmlArrayItem("Item")]
		public Item[] Items { get; set; }
	}
}
