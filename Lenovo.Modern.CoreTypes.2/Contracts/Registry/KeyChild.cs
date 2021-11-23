using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Registry
{
	// Token: 0x02000057 RID: 87
	[XmlRoot(ElementName = "KeyChild", Namespace = null)]
	public sealed class KeyChild
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00006122 File Offset: 0x00004322
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x0000612A File Offset: 0x0000432A
		[XmlAttribute(AttributeName = "Type")]
		public RegistryKind Type { get; set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060003DA RID: 986 RVA: 0x00006133 File Offset: 0x00004333
		// (set) Token: 0x060003DB RID: 987 RVA: 0x0000613B File Offset: 0x0000433B
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060003DC RID: 988 RVA: 0x00006144 File Offset: 0x00004344
		// (set) Token: 0x060003DD RID: 989 RVA: 0x0000614C File Offset: 0x0000434C
		[XmlAttribute(AttributeName = "Value")]
		public string Value { get; set; }
	}
}
