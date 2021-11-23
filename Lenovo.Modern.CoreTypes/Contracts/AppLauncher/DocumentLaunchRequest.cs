using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.AppLauncher
{
	// Token: 0x020000C4 RID: 196
	[XmlRoot(ElementName = "DocumentLaunchRequest", Namespace = "")]
	public sealed class DocumentLaunchRequest
	{
		// Token: 0x1700032D RID: 813
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x000084F6 File Offset: 0x000066F6
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x000084FE File Offset: 0x000066FE
		[XmlElement("DocumentLaunchDetails")]
		public DocumentLaunchDetails DocumentLaunchDetails { get; set; }
	}
}
