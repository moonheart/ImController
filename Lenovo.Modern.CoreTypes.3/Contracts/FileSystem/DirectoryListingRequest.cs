using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.FileSystem
{
	// Token: 0x020000B7 RID: 183
	[XmlRoot(ElementName = "DirectoryListingRequest", Namespace = null)]
	public sealed class DirectoryListingRequest
	{
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x0000826B File Offset: 0x0000646B
		// (set) Token: 0x060006D3 RID: 1747 RVA: 0x00008273 File Offset: 0x00006473
		[XmlArray("DirectoryList")]
		[XmlArrayItem("Directory")]
		public Directory[] DirectoryList { get; set; }
	}
}
