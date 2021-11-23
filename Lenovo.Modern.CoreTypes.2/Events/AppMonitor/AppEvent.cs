using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.AppMonitor
{
	// Token: 0x0200003F RID: 63
	[XmlRoot(ElementName = "AppEvent", Namespace = null)]
	public sealed class AppEvent
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x00005328 File Offset: 0x00003528
		// (set) Token: 0x060002A3 RID: 675 RVA: 0x00005330 File Offset: 0x00003530
		[XmlAttribute(AttributeName = "pfn")]
		public string Pfn { get; set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x00005339 File Offset: 0x00003539
		// (set) Token: 0x060002A5 RID: 677 RVA: 0x00005341 File Offset: 0x00003541
		[XmlAttribute(AttributeName = "event")]
		public string Event { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000534A File Offset: 0x0000354A
		// (set) Token: 0x060002A7 RID: 679 RVA: 0x00005352 File Offset: 0x00003552
		[XmlAttribute(AttributeName = "desktopApp")]
		public string DesktopApp { get; set; }
	}
}
