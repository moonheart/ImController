using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.Subscription
{
	// Token: 0x02000014 RID: 20
	[XmlRoot("Setting")]
	public sealed class Setting
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00003E68 File Offset: 0x00002068
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00003E70 File Offset: 0x00002070
		[XmlText]
		public string Value { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00003E79 File Offset: 0x00002079
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x00003E81 File Offset: 0x00002081
		[XmlAttribute("key")]
		public string Key { get; set; }
	}
}
