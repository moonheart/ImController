using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.FileSystem
{
	// Token: 0x020000B8 RID: 184
	[XmlRoot(ElementName = "DirectoryListingResponse", Namespace = null)]
	public sealed class DirectoryListingResponse
	{
		// Token: 0x17000310 RID: 784
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0000827C File Offset: 0x0000647C
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00008284 File Offset: 0x00006484
		[XmlArray("DirectoryList")]
		[XmlArrayItem("Directory")]
		public Directory[] DirectoryList { get; set; }
	}
}
