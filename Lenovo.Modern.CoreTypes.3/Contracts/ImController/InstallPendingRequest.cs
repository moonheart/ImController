using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ImController
{
	// Token: 0x020000AC RID: 172
	[XmlRoot(ElementName = "InstallPendingRequest", Namespace = "")]
	public sealed class InstallPendingRequest
	{
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x00008014 File Offset: 0x00006214
		// (set) Token: 0x06000691 RID: 1681 RVA: 0x0000801C File Offset: 0x0000621C
		[XmlArray("PackageList")]
		[XmlArrayItem("Package")]
		public Package[] PackageList { get; set; }
	}
}
