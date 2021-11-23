using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ImController
{
	// Token: 0x020000AF RID: 175
	[XmlRoot(ElementName = "PendingUpdateResponse", Namespace = null)]
	public sealed class PendingUpdateResponse
	{
		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x00008058 File Offset: 0x00006258
		// (set) Token: 0x0600069C RID: 1692 RVA: 0x00008060 File Offset: 0x00006260
		[XmlArray("PackageList")]
		[XmlArrayItem("Package")]
		public Package[] PackageList { get; set; }
	}
}
