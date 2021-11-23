using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences.LenovoAppMetricsModels
{
	// Token: 0x0200006E RID: 110
	[XmlRoot(ElementName = "OnValue", Namespace = null)]
	public sealed class OnValue
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x000065E8 File Offset: 0x000047E8
		// (set) Token: 0x06000470 RID: 1136 RVA: 0x000065F0 File Offset: 0x000047F0
		[XmlAttribute("type")]
		public string Type { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x000065F9 File Offset: 0x000047F9
		// (set) Token: 0x06000472 RID: 1138 RVA: 0x00006601 File Offset: 0x00004801
		[XmlText]
		public string Text { get; set; }
	}
}
