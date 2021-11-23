using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.Subscription
{
	// Token: 0x02000010 RID: 16
	[XmlRoot("DependencyPackageSubscription", Namespace = "")]
	public sealed class MultiPackageSubscription
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00003D69 File Offset: 0x00001F69
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00003D71 File Offset: 0x00001F71
		[XmlAttribute("version")]
		public string Version { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00003D7A File Offset: 0x00001F7A
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00003D82 File Offset: 0x00001F82
		[XmlAttribute("dateCreated")]
		public string DateCreated { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00003D8B File Offset: 0x00001F8B
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00003D93 File Offset: 0x00001F93
		[XmlElement("Package")]
		public Package Package { get; set; }
	}
}
