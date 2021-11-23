using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences
{
	// Token: 0x0200005D RID: 93
	[XmlRoot(ElementName = "App", Namespace = null)]
	public sealed class App
	{
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x000061AA File Offset: 0x000043AA
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x000061B2 File Offset: 0x000043B2
		[XmlAttribute("ID")]
		public string ID { get; set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x000061BB File Offset: 0x000043BB
		// (set) Token: 0x060003F0 RID: 1008 RVA: 0x000061C3 File Offset: 0x000043C3
		[XmlElement("MetricCollectionState")]
		public string MetricCollectionState { get; set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000061CC File Offset: 0x000043CC
		// (set) Token: 0x060003F2 RID: 1010 RVA: 0x000061D4 File Offset: 0x000043D4
		[XmlElement("FridendlyName")]
		public string FridendlyName { get; set; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x000061DD File Offset: 0x000043DD
		// (set) Token: 0x060003F4 RID: 1012 RVA: 0x000061E5 File Offset: 0x000043E5
		[XmlElement("Descriptions")]
		public Description[] Descriptions { get; set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x000061EE File Offset: 0x000043EE
		// (set) Token: 0x060003F6 RID: 1014 RVA: 0x000061F6 File Offset: 0x000043F6
		[XmlElement("Confirmations")]
		public Confirmation[] Confirmations { get; set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x000061FF File Offset: 0x000043FF
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x00006207 File Offset: 0x00004407
		[XmlElement("RequireAdmin")]
		public string RequireAdmin { get; set; }
	}
}
