using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.FileSystem
{
	// Token: 0x020000BB RID: 187
	[XmlRoot(ElementName = "ItemActionResponse", Namespace = null)]
	public sealed class ItemActionResponse
	{
		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x000082E2 File Offset: 0x000064E2
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x000082EA File Offset: 0x000064EA
		[XmlArray("ItemActionList")]
		[XmlArrayItem("Action")]
		public Action[] ItemActionList { get; set; }
	}
}
