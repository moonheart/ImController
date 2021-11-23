using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences
{
	// Token: 0x02000061 RID: 97
	[XmlRoot(ElementName = "AppMetricCollectionSettingRequest", Namespace = null)]
	public sealed class AppMetricCollectionSettingRequest
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x00006254 File Offset: 0x00004454
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x0000625C File Offset: 0x0000445C
		[XmlAttribute("lang")]
		public string Lang { get; set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00006265 File Offset: 0x00004465
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x0000626D File Offset: 0x0000446D
		[XmlElement("App")]
		public App App { get; set; }
	}
}
