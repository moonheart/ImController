using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.Subscription
{
	// Token: 0x02000013 RID: 19
	[XmlRoot("InstallInfo")]
	public sealed class InstallInfo
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00003E13 File Offset: 0x00002013
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00003E1B File Offset: 0x0000201B
		[XmlAttribute("downloadLocation")]
		public string DownloadLocation { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00003E24 File Offset: 0x00002024
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00003E2C File Offset: 0x0000202C
		[XmlAttribute("installerargument")]
		public string InstallerArgument { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00003E35 File Offset: 0x00002035
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00003E3D File Offset: 0x0000203D
		[XmlAttribute("installerdescription")]
		public string InstallerDescription { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00003E46 File Offset: 0x00002046
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00003E4E File Offset: 0x0000204E
		[XmlAttribute("reboot")]
		public string Reboot { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00003E57 File Offset: 0x00002057
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00003E5F File Offset: 0x0000205F
		[XmlAttribute("autoUpdate")]
		public string AutoUpdate { get; set; }
	}
}
