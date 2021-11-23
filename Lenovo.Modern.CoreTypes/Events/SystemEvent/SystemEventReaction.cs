using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.SystemEvent
{
	// Token: 0x0200002F RID: 47
	[XmlRoot(ElementName = "SystemEventReaction", Namespace = null)]
	public sealed class SystemEventReaction
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00004C91 File Offset: 0x00002E91
		// (set) Token: 0x0600020D RID: 525 RVA: 0x00004C99 File Offset: 0x00002E99
		[XmlElement(ElementName = "SystemShutdown")]
		public bool SystemShutdown { get; set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00004CA2 File Offset: 0x00002EA2
		// (set) Token: 0x0600020F RID: 527 RVA: 0x00004CAA File Offset: 0x00002EAA
		[XmlElement(ElementName = "SystemStart")]
		public bool SystemStart { get; set; }
	}
}
