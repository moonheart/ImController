using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.Subscription
{
	// Token: 0x02000012 RID: 18
	[XmlRoot("Version")]
	public sealed class PackageVersion
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00003E02 File Offset: 0x00002002
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x00003E0A File Offset: 0x0000200A
		[XmlAttribute("currentVersion")]
		public string CurrentVersion { get; set; }
	}
}
