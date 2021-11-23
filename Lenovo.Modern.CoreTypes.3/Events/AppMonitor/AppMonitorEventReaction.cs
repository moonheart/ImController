using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.AppMonitor
{
	// Token: 0x0200003E RID: 62
	[XmlRoot(ElementName = "AppMonitorEventReaction", Namespace = null)]
	public sealed class AppMonitorEventReaction
	{
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600029B RID: 667 RVA: 0x000052F5 File Offset: 0x000034F5
		// (set) Token: 0x0600029C RID: 668 RVA: 0x000052FD File Offset: 0x000034FD
		[XmlAttribute(AttributeName = "UapAppPfn")]
		public string UapAppPfn { get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00005306 File Offset: 0x00003506
		// (set) Token: 0x0600029E RID: 670 RVA: 0x0000530E File Offset: 0x0000350E
		[XmlAttribute(AttributeName = "event")]
		public string Event { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600029F RID: 671 RVA: 0x00005317 File Offset: 0x00003517
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x0000531F File Offset: 0x0000351F
		[XmlAttribute(AttributeName = "DesktopApp")]
		public string DesktopApp { get; set; }
	}
}
