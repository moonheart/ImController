using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences
{
	// Token: 0x0200005E RID: 94
	[XmlRoot(ElementName = "AppIdentifier", Namespace = null)]
	public sealed class AppIdentifier
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00006210 File Offset: 0x00004410
		// (set) Token: 0x060003FB RID: 1019 RVA: 0x00006218 File Offset: 0x00004418
		[XmlAttribute("lang")]
		public string Lang { get; set; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x00006221 File Offset: 0x00004421
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x00006229 File Offset: 0x00004429
		[XmlElement("App")]
		public App App { get; set; }
	}
}
