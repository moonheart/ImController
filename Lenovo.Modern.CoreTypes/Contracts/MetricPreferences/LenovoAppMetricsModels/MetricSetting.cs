using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences.LenovoAppMetricsModels
{
	// Token: 0x0200006C RID: 108
	[XmlRoot(ElementName = "MetricSetting", Namespace = null)]
	public sealed class MetricSetting
	{
		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x00006560 File Offset: 0x00004760
		// (set) Token: 0x0600045E RID: 1118 RVA: 0x00006568 File Offset: 0x00004768
		[XmlAttribute("type")]
		public string Type { get; set; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x00006571 File Offset: 0x00004771
		// (set) Token: 0x06000460 RID: 1120 RVA: 0x00006579 File Offset: 0x00004779
		[XmlAttribute("bitness")]
		public string Bitness { get; set; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00006582 File Offset: 0x00004782
		// (set) Token: 0x06000462 RID: 1122 RVA: 0x0000658A File Offset: 0x0000478A
		[XmlElement("Location")]
		public Location Location { get; set; }

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x00006593 File Offset: 0x00004793
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x0000659B File Offset: 0x0000479B
		[XmlElement("XPath")]
		public string XPath { get; set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x000065A4 File Offset: 0x000047A4
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x000065AC File Offset: 0x000047AC
		[XmlElement("OnValue")]
		public OnValue OnValue { get; set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x000065B5 File Offset: 0x000047B5
		// (set) Token: 0x06000468 RID: 1128 RVA: 0x000065BD File Offset: 0x000047BD
		[XmlElement("OffValue")]
		public OffValue OffValue { get; set; }
	}
}
