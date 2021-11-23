using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ImController
{
	// Token: 0x020000AA RID: 170
	[XmlRoot(ElementName = "EntitledAppsResponse", Namespace = null)]
	public sealed class EntitledAppsResponse
	{
		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00007DCB File Offset: 0x00005FCB
		// (set) Token: 0x06000666 RID: 1638 RVA: 0x00007DD3 File Offset: 0x00005FD3
		[XmlArray("AppList")]
		[XmlArrayItem("App")]
		public App[] AppList { get; set; }
	}
}
