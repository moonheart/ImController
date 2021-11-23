using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.Subscription
{
	// Token: 0x0200000E RID: 14
	[XmlRoot(ElementName = "AppVersion", Namespace = null)]
	public sealed class AppVersion
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00003409 File Offset: 0x00001609
		public AppVersion()
		{
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003CDC File Offset: 0x00001EDC
		public AppVersion(string version, bool forceUpgrade)
		{
			this.Version = version;
			this.ForceUpgrade = forceUpgrade;
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00003CF2 File Offset: 0x00001EF2
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00003CFA File Offset: 0x00001EFA
		[XmlAttribute(AttributeName = "force")]
		public bool ForceUpgrade { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00003D03 File Offset: 0x00001F03
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00003D0B File Offset: 0x00001F0B
		[XmlText]
		public string Version { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00003D14 File Offset: 0x00001F14
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x00003D1C File Offset: 0x00001F1C
		[XmlAttribute(AttributeName = "id")]
		public Guid Id { get; set; }
	}
}
