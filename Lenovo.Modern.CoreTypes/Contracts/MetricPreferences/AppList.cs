using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences
{
	// Token: 0x0200005F RID: 95
	[XmlRoot(ElementName = "AppList", Namespace = null)]
	public sealed class AppList
	{
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x00006232 File Offset: 0x00004432
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x0000623A File Offset: 0x0000443A
		[XmlArrayItem("App")]
		public App[] Apps { get; set; }
	}
}
