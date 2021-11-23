using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences.LenovoAppMetricsModels
{
	// Token: 0x0200006A RID: 106
	[XmlRoot(ElementName = "LenovoAppMetrics", Namespace = null)]
	public sealed class LenovoAppMetrics
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x0000652D File Offset: 0x0000472D
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x00006535 File Offset: 0x00004735
		[XmlElement("App")]
		public App[] App { get; set; }
	}
}
