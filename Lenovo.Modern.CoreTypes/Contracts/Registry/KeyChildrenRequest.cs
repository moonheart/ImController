using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Registry
{
	// Token: 0x02000059 RID: 89
	[XmlRoot(ElementName = "KeyChildrenRequest", Namespace = null)]
	public sealed class KeyChildrenRequest
	{
		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060003DF RID: 991 RVA: 0x00006155 File Offset: 0x00004355
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x0000615D File Offset: 0x0000435D
		[XmlArray("KeyList")]
		[XmlArrayItem("Key")]
		public KeyList[] KeyList { get; set; }
	}
}
