using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.AppLauncher
{
	// Token: 0x020000C3 RID: 195
	[XmlRoot(ElementName = "DocumentLaunchDetails", Namespace = "")]
	public sealed class DocumentLaunchDetails
	{
		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x000084E5 File Offset: 0x000066E5
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x000084ED File Offset: 0x000066ED
		[XmlAttribute(AttributeName = "pathToDocument")]
		public string PathToDocument { get; set; }
	}
}
