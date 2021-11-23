using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.AppLauncher
{
	// Token: 0x020000BE RID: 190
	[XmlRoot(ElementName = "AppLaunchResponse", Namespace = null)]
	public sealed class AppLaunchResponse
	{
		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x00008326 File Offset: 0x00006526
		// (set) Token: 0x060006F0 RID: 1776 RVA: 0x0000832E File Offset: 0x0000652E
		[XmlElement("Result")]
		public Result Result { get; set; }
	}
}
