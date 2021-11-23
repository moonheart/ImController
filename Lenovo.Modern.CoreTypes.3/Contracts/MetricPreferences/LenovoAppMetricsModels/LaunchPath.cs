using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences.LenovoAppMetricsModels
{
	// Token: 0x02000069 RID: 105
	[XmlRoot(ElementName = "LaunchPath", Namespace = null)]
	public sealed class LaunchPath
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x000064D8 File Offset: 0x000046D8
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x000064E0 File Offset: 0x000046E0
		[XmlAttribute("type")]
		public string Type { get; set; }

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x000064E9 File Offset: 0x000046E9
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x000064F1 File Offset: 0x000046F1
		[XmlAttribute("bitness")]
		public string Bitness { get; set; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x000064FA File Offset: 0x000046FA
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x00006502 File Offset: 0x00004702
		[XmlAttribute("root")]
		public string Root { get; set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0000650B File Offset: 0x0000470B
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x00006513 File Offset: 0x00004713
		[XmlElement("Location")]
		public string Location { get; set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x0000651C File Offset: 0x0000471C
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x00006524 File Offset: 0x00004724
		[XmlElement("Name")]
		public string Name { get; set; }
	}
}
