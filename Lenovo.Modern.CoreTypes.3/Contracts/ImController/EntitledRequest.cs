using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ImController
{
	// Token: 0x020000A6 RID: 166
	[XmlRoot(ElementName = "EntitledRequest", Namespace = "")]
	public sealed class EntitledRequest
	{
		// Token: 0x170002CB RID: 715
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x00007C88 File Offset: 0x00005E88
		// (set) Token: 0x0600063C RID: 1596 RVA: 0x00007C90 File Offset: 0x00005E90
		[XmlElement("DeviceID")]
		public string DeviceID { get; set; }
	}
}
