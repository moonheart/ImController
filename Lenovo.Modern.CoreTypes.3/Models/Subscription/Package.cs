using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.Subscription
{
	// Token: 0x02000011 RID: 17
	[XmlRoot("Package")]
	public sealed class Package
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00003D9C File Offset: 0x00001F9C
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00003DA4 File Offset: 0x00001FA4
		[XmlAttribute("friendlyname")]
		public string FriendlyName { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00003DAD File Offset: 0x00001FAD
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00003DB5 File Offset: 0x00001FB5
		[XmlAttribute("id")]
		public string Id { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00003DBE File Offset: 0x00001FBE
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00003DC6 File Offset: 0x00001FC6
		[XmlElement("Version")]
		public string Version { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00003DCF File Offset: 0x00001FCF
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00003DD7 File Offset: 0x00001FD7
		[XmlElement("Installinfo")]
		public InstallInfo InstallInfo { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00003DE0 File Offset: 0x00001FE0
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00003DE8 File Offset: 0x00001FE8
		[XmlArray("SettingList")]
		[XmlArrayItem("Setting")]
		public Setting[] SettingList { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00003DF1 File Offset: 0x00001FF1
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00003DF9 File Offset: 0x00001FF9
		[XmlArray("SubPackageList")]
		[XmlArrayItem("Package")]
		public Package[] SubPackageList { get; set; }
	}
}
