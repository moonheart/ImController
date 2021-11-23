using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation.StorageInformation
{
	// Token: 0x0200004C RID: 76
	[XmlRoot(ElementName = "StorageInformationResponse", Namespace = null)]
	public sealed class StorageInformationResponse
	{
		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00005BAF File Offset: 0x00003DAF
		// (set) Token: 0x06000349 RID: 841 RVA: 0x00005BB7 File Offset: 0x00003DB7
		[XmlArray("PhysicalDiskList")]
		[XmlArrayItem("PhysicalDisk")]
		public PhysicalDisk[] PhysicalDiskList { get; set; }
	}
}
