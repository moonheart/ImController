using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Registry
{
	// Token: 0x0200005B RID: 91
	[XmlRoot(ElementName = "Key", Namespace = null)]
	public sealed class KeyList
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00006177 File Offset: 0x00004377
		// (set) Token: 0x060003E6 RID: 998 RVA: 0x0000617F File Offset: 0x0000437F
		[XmlAttribute(AttributeName = "Location")]
		public string Location { get; set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x00006188 File Offset: 0x00004388
		// (set) Token: 0x060003E8 RID: 1000 RVA: 0x00006190 File Offset: 0x00004390
		[XmlArray("KeyChildren")]
		[XmlArrayItem("KeyChild")]
		public KeyChild[] KeyChildren { get; set; }
	}
}
