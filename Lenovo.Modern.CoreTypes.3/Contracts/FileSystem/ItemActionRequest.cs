using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.FileSystem
{
	// Token: 0x020000BA RID: 186
	[XmlRoot(ElementName = "ItemActionRequest", Namespace = null)]
	public sealed class ItemActionRequest
	{
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x000082D1 File Offset: 0x000064D1
		// (set) Token: 0x060006E2 RID: 1762 RVA: 0x000082D9 File Offset: 0x000064D9
		[XmlArray("ItemActionList")]
		[XmlArrayItem("Action")]
		public Action[] ItemActionList { get; set; }
	}
}
