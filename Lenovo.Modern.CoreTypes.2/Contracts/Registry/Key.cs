using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Registry
{
	// Token: 0x02000056 RID: 86
	[XmlRoot(ElementName = "Key", Namespace = null)]
	public sealed class Key
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x00006111 File Offset: 0x00004311
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x00006119 File Offset: 0x00004319
		[XmlArray("KeyList")]
		public KeyChild[] KeyList { get; set; }
	}
}
