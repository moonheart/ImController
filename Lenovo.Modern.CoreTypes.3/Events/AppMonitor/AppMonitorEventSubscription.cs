using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.AppMonitor
{
	// Token: 0x02000040 RID: 64
	[XmlRoot(ElementName = "AppMonitorSubscription", Namespace = null)]
	public sealed class AppMonitorEventSubscription
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000535B File Offset: 0x0000355B
		// (set) Token: 0x060002AA RID: 682 RVA: 0x00005363 File Offset: 0x00003563
		[XmlArray("AppEventList")]
		[XmlArrayItem("AppEvent")]
		public AppEvent[] AppEventList { get; set; }
	}
}
