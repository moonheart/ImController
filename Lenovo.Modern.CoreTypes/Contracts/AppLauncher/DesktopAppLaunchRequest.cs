using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.AppLauncher
{
	// Token: 0x020000C2 RID: 194
	[XmlRoot(ElementName = "DesktopAppLaunchRequest", Namespace = "")]
	public sealed class DesktopAppLaunchRequest
	{
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x000084D4 File Offset: 0x000066D4
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x000084DC File Offset: 0x000066DC
		[XmlElement("DesktopAppLaunchDetails")]
		public DesktopAppLaunchDetails DesktopAppLaunchDetails { get; set; }
	}
}
