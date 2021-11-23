using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation
{
	// Token: 0x02000047 RID: 71
	[XmlRoot(ElementName = "OutputLocationResponse")]
	public sealed class OutputLocationResponse
	{
		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060002FC RID: 764 RVA: 0x000058EE File Offset: 0x00003AEE
		// (set) Token: 0x060002FD RID: 765 RVA: 0x000058F6 File Offset: 0x00003AF6
		[XmlAttribute(AttributeName = "Success")]
		public string Success { get; set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060002FE RID: 766 RVA: 0x000058FF File Offset: 0x00003AFF
		// (set) Token: 0x060002FF RID: 767 RVA: 0x00005907 File Offset: 0x00003B07
		[XmlAttribute(AttributeName = "Location")]
		public string Location { get; set; }
	}
}
