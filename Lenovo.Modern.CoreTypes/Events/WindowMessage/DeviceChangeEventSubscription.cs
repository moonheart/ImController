using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.WindowMessage
{
	// Token: 0x02000022 RID: 34
	[XmlRoot(ElementName = "DeviceChangeEventSubscription", Namespace = null)]
	public sealed class DeviceChangeEventSubscription
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000177 RID: 375 RVA: 0x000045C8 File Offset: 0x000027C8
		// (set) Token: 0x06000178 RID: 376 RVA: 0x000045D0 File Offset: 0x000027D0
		[XmlArray("DevInterfaceClassGuidList")]
		[XmlArrayItem("DevInterfaceClassGuid")]
		public string[] DevInterfaceClassGuidList { get; set; }
	}
}
