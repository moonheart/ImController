using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences
{
	// Token: 0x02000060 RID: 96
	[XmlRoot(ElementName = "AppMetricCollectionResponse", Namespace = null)]
	public sealed class AppMetricCollectionResponse
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x00006243 File Offset: 0x00004443
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x0000624B File Offset: 0x0000444B
		[XmlElement("App")]
		public App App { get; set; }
	}
}
