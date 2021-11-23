using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences.LenovoAppMetricsModels
{
	// Token: 0x02000068 RID: 104
	[XmlRoot(ElementName = "InstallationCheck", Namespace = null)]
	public sealed class InstallationCheck
	{
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x00006494 File Offset: 0x00004694
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x0000649C File Offset: 0x0000469C
		[XmlText]
		public string Text { get; set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x000064A5 File Offset: 0x000046A5
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x000064AD File Offset: 0x000046AD
		[XmlAttribute("type")]
		public string Type { get; set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x000064B6 File Offset: 0x000046B6
		// (set) Token: 0x06000446 RID: 1094 RVA: 0x000064BE File Offset: 0x000046BE
		[XmlAttribute("bitness")]
		public string Bitness { get; set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x000064C7 File Offset: 0x000046C7
		// (set) Token: 0x06000448 RID: 1096 RVA: 0x000064CF File Offset: 0x000046CF
		[XmlAttribute("root")]
		public string Root { get; set; }
	}
}
