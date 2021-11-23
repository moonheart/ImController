using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.AppLauncher
{
	// Token: 0x020000C1 RID: 193
	[XmlRoot(ElementName = "DesktopAppLaunchDetails", Namespace = "")]
	public sealed class DesktopAppLaunchDetails
	{
		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x000084A1 File Offset: 0x000066A1
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x000084A9 File Offset: 0x000066A9
		[XmlAttribute(AttributeName = "pathToLnkFile")]
		public string PathToLnkFile { get; set; }

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x000084B2 File Offset: 0x000066B2
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x000084BA File Offset: 0x000066BA
		[XmlAttribute(AttributeName = "cplFile")]
		public string CplFile { get; set; }

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x000084C3 File Offset: 0x000066C3
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x000084CB File Offset: 0x000066CB
		[XmlAttribute(AttributeName = "arguments")]
		public string Arguments { get; set; }
	}
}
