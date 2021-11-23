using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences
{
	// Token: 0x0200005C RID: 92
	[XmlRoot(ElementName = "AllAppsMetricCollectionSettingResponse", Namespace = null)]
	public sealed class AllAppsMetricCollectionSettingResponse
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x00006199 File Offset: 0x00004399
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x000061A1 File Offset: 0x000043A1
		[XmlElement("AppList")]
		public AppList Applist { get; set; }
	}
}
