using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ImController
{
	// Token: 0x020000AD RID: 173
	[XmlRoot(ElementName = "InstallPendingResponse", Namespace = null)]
	public sealed class InstallPendingResponse
	{
		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x00008025 File Offset: 0x00006225
		// (set) Token: 0x06000694 RID: 1684 RVA: 0x0000802D File Offset: 0x0000622D
		[XmlAttribute("totalPercentageComplete")]
		public string totalPercentageComplete { get; set; }

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x00008036 File Offset: 0x00006236
		// (set) Token: 0x06000696 RID: 1686 RVA: 0x0000803E File Offset: 0x0000623E
		[XmlArray("PackageList")]
		[XmlArrayItem("Package")]
		public Package[] PackageList { get; set; }
	}
}
