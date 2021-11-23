using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences.LenovoAppMetricsModels
{
	// Token: 0x0200006B RID: 107
	[XmlRoot(ElementName = "Location", Namespace = null)]
	public sealed class Location
	{
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x0000653E File Offset: 0x0000473E
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x00006546 File Offset: 0x00004746
		[XmlAttribute("root")]
		public string Root { get; set; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x0000654F File Offset: 0x0000474F
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x00006557 File Offset: 0x00004757
		[XmlText]
		public string Text { get; set; }
	}
}
