using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences.LenovoAppMetricsModels
{
	// Token: 0x02000066 RID: 102
	[XmlRoot(ElementName = "App", Namespace = null)]
	public sealed class App
	{
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x000063D9 File Offset: 0x000045D9
		// (set) Token: 0x0600042A RID: 1066 RVA: 0x000063E1 File Offset: 0x000045E1
		[XmlElement("FriendlyName")]
		public string FriendlyName { get; set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x000063EA File Offset: 0x000045EA
		// (set) Token: 0x0600042C RID: 1068 RVA: 0x000063F2 File Offset: 0x000045F2
		[XmlElement("InstallationCheck")]
		public InstallationCheck InstallationCheck { get; set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x000063FB File Offset: 0x000045FB
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x00006403 File Offset: 0x00004603
		[XmlElement("MetricSetting")]
		public MetricSetting MetricSetting { get; set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x0000640C File Offset: 0x0000460C
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x00006414 File Offset: 0x00004614
		[XmlElement("LaunchPath")]
		public LaunchPath LaunchPath { get; set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x0000641D File Offset: 0x0000461D
		// (set) Token: 0x06000432 RID: 1074 RVA: 0x00006425 File Offset: 0x00004625
		[XmlArray("Descriptions")]
		public Description[] Descriptions { get; set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x0000642E File Offset: 0x0000462E
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x00006436 File Offset: 0x00004636
		[XmlArray("Confirmations")]
		public Confirmation[] Confirmations { get; set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x0000643F File Offset: 0x0000463F
		// (set) Token: 0x06000436 RID: 1078 RVA: 0x00006447 File Offset: 0x00004647
		[XmlElement("PackageName")]
		public string PackageName { get; set; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x00006450 File Offset: 0x00004650
		// (set) Token: 0x06000438 RID: 1080 RVA: 0x00006458 File Offset: 0x00004658
		[XmlAttribute("type")]
		public string Type { get; set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x00006461 File Offset: 0x00004661
		// (set) Token: 0x0600043A RID: 1082 RVA: 0x00006469 File Offset: 0x00004669
		[XmlAttribute("ID")]
		public string ID { get; set; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x00006472 File Offset: 0x00004672
		// (set) Token: 0x0600043C RID: 1084 RVA: 0x0000647A File Offset: 0x0000467A
		[XmlElement("RequireAdmin")]
		public string RequireAdmin { get; set; }
	}
}
