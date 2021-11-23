using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.Network
{
	// Token: 0x02000035 RID: 53
	[XmlRoot(ElementName = "NetworkEventReaction", Namespace = null)]
	public sealed class NetworkEventReaction
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600023E RID: 574 RVA: 0x00004EA1 File Offset: 0x000030A1
		// (set) Token: 0x0600023F RID: 575 RVA: 0x00004EA9 File Offset: 0x000030A9
		[XmlElement(ElementName = "NetworkAddressChange")]
		public bool NetworkAddressChange { get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000240 RID: 576 RVA: 0x00004EB2 File Offset: 0x000030B2
		// (set) Token: 0x06000241 RID: 577 RVA: 0x00004EBA File Offset: 0x000030BA
		[XmlElement(ElementName = "InternetConnectivityChanged")]
		public bool InternetConnectivityChanged { get; set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00004EC3 File Offset: 0x000030C3
		// (set) Token: 0x06000243 RID: 579 RVA: 0x00004ECB File Offset: 0x000030CB
		[XmlElement(ElementName = "InternetConnectionOn")]
		public bool InternetConnectionOn { get; set; }
	}
}
