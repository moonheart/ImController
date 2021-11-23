using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ImController
{
	// Token: 0x020000B3 RID: 179
	public sealed class Status
	{
		// Token: 0x170002FE RID: 766
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x000080E2 File Offset: 0x000062E2
		// (set) Token: 0x060006AE RID: 1710 RVA: 0x000080EA File Offset: 0x000062EA
		[XmlElement("ImControllerVersion")]
		public string ImControllerVersion { get; set; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x000080F3 File Offset: 0x000062F3
		// (set) Token: 0x060006B0 RID: 1712 RVA: 0x000080FB File Offset: 0x000062FB
		[XmlElement("Mode")]
		public string Mode { get; set; }
	}
}
