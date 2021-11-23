using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.AppLauncher
{
	// Token: 0x020000C7 RID: 199
	[XmlRoot(ElementName = "UniversalAppLaunchRequest", Namespace = "")]
	public sealed class UniversalAppLaunchRequest
	{
		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x0000853A File Offset: 0x0000673A
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x00008542 File Offset: 0x00006742
		[XmlElement("UniversalAppLaunchDetails")]
		public UniversalAppLaunchDetails UniversalAppLaunchDetails { get; set; }
	}
}
