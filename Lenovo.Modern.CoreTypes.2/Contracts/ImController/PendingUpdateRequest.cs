using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ImController
{
	// Token: 0x020000AE RID: 174
	[XmlRoot(ElementName = "PendingUpdateRequest", Namespace = "")]
	public sealed class PendingUpdateRequest
	{
		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x00008047 File Offset: 0x00006247
		// (set) Token: 0x06000699 RID: 1689 RVA: 0x0000804F File Offset: 0x0000624F
		[XmlAttribute("appId")]
		public string appId { get; set; }
	}
}
