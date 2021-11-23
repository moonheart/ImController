using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences
{
	// Token: 0x02000063 RID: 99
	[XmlRoot(ElementName = "Description", Namespace = null)]
	public sealed class Description
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00006298 File Offset: 0x00004498
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x000062A0 File Offset: 0x000044A0
		[XmlAttribute("lang")]
		public string Lang { get; set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x000062A9 File Offset: 0x000044A9
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x000062B1 File Offset: 0x000044B1
		[XmlText]
		public string Text { get; set; }
	}
}
