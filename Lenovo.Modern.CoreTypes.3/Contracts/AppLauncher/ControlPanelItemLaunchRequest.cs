using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.AppLauncher
{
	// Token: 0x020000C0 RID: 192
	[XmlRoot(ElementName = "ControlPanelItemLaunchRequest", Namespace = "")]
	public sealed class ControlPanelItemLaunchRequest
	{
		// Token: 0x17000327 RID: 807
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x00008490 File Offset: 0x00006690
		// (set) Token: 0x0600070B RID: 1803 RVA: 0x00008498 File Offset: 0x00006698
		[XmlElement("DesktopAppLaunchDetails")]
		public DesktopAppLaunchDetails DesktopAppLaunchDetails { get; set; }
	}
}
