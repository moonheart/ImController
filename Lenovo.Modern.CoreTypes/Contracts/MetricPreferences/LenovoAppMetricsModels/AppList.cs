using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences.LenovoAppMetricsModels
{
	// Token: 0x02000067 RID: 103
	[XmlRoot(ElementName = "AppList", Namespace = null)]
	public sealed class AppList
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x00006483 File Offset: 0x00004683
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x0000648B File Offset: 0x0000468B
		[XmlArrayItem("App")]
		public App[] App { get; set; }
	}
}
