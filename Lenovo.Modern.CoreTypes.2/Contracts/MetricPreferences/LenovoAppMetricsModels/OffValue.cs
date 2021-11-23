using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences.LenovoAppMetricsModels
{
	// Token: 0x0200006D RID: 109
	[XmlRoot(ElementName = "OffValue", Namespace = null)]
	public sealed class OffValue
	{
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x000065C6 File Offset: 0x000047C6
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x000065CE File Offset: 0x000047CE
		[XmlAttribute("type")]
		public string Type { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x000065D7 File Offset: 0x000047D7
		// (set) Token: 0x0600046D RID: 1133 RVA: 0x000065DF File Offset: 0x000047DF
		[XmlText]
		public string Text { get; set; }
	}
}
