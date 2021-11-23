using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.AppLauncher
{
	// Token: 0x020000C5 RID: 197
	[XmlRoot(ElementName = "Result", Namespace = "")]
	public sealed class Result
	{
		// Token: 0x1700032E RID: 814
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x00008507 File Offset: 0x00006707
		// (set) Token: 0x0600071E RID: 1822 RVA: 0x0000850F File Offset: 0x0000670F
		[XmlAttribute(AttributeName = "success")]
		public string Success { get; set; }
	}
}
