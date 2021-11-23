using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.Network
{
	// Token: 0x02000036 RID: 54
	[XmlRoot(ElementName = "NetworkEventSubscription", Namespace = null)]
	public sealed class NetworkEventSubscription
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00004ED4 File Offset: 0x000030D4
		// (set) Token: 0x06000246 RID: 582 RVA: 0x00004EDC File Offset: 0x000030DC
		[XmlElement(ElementName = "NetworkAddressChange")]
		public bool NetworkAddressChange { get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000247 RID: 583 RVA: 0x00004EE5 File Offset: 0x000030E5
		// (set) Token: 0x06000248 RID: 584 RVA: 0x00004EED File Offset: 0x000030ED
		[XmlElement(ElementName = "InternetConnectivityChanged")]
		public bool InternetConnectivityChanged { get; set; }
	}
}
