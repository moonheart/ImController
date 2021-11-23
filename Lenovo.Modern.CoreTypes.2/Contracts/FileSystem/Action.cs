using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.FileSystem
{
	// Token: 0x020000B4 RID: 180
	[XmlRoot(ElementName = "Action", Namespace = null)]
	public sealed class Action
	{
		// Token: 0x17000300 RID: 768
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x00008104 File Offset: 0x00006304
		// (set) Token: 0x060006B3 RID: 1715 RVA: 0x0000810C File Offset: 0x0000630C
		[XmlAttribute(AttributeName = "Type")]
		public string Type { get; set; }

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x00008115 File Offset: 0x00006315
		// (set) Token: 0x060006B5 RID: 1717 RVA: 0x0000811D File Offset: 0x0000631D
		[XmlAttribute(AttributeName = "Result")]
		public bool Result { get; set; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00008126 File Offset: 0x00006326
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x0000812E File Offset: 0x0000632E
		[XmlAttribute(AttributeName = "Source")]
		public string Source { get; set; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00008137 File Offset: 0x00006337
		// (set) Token: 0x060006B9 RID: 1721 RVA: 0x0000813F File Offset: 0x0000633F
		[XmlAttribute(AttributeName = "Destination")]
		public string Destination { get; set; }
	}
}
