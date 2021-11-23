using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Registry
{
	// Token: 0x0200005A RID: 90
	[XmlRoot(ElementName = "KeyChildrenResponse", Namespace = null)]
	public sealed class KeyChildrenResponse
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x00006166 File Offset: 0x00004366
		// (set) Token: 0x060003E3 RID: 995 RVA: 0x0000616E File Offset: 0x0000436E
		[XmlArray("KeyList")]
		[XmlArrayItem("Key")]
		public KeyList[] KeyList { get; set; }
	}
}
