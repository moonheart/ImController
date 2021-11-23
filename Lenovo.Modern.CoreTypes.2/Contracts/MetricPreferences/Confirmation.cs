using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences
{
	// Token: 0x02000062 RID: 98
	[XmlRoot(ElementName = "Confirmation", Namespace = null)]
	public sealed class Confirmation
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00006276 File Offset: 0x00004476
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x0000627E File Offset: 0x0000447E
		[XmlAttribute("lang")]
		public string Lang { get; set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x00006287 File Offset: 0x00004487
		// (set) Token: 0x0600040D RID: 1037 RVA: 0x0000628F File Offset: 0x0000448F
		[XmlText]
		public string Text { get; set; }
	}
}
