using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ActiveDirectory
{
	// Token: 0x020000C9 RID: 201
	[XmlRoot(ElementName = "AppPoliciesResponse", Namespace = null)]
	public sealed class AppPoliciesResponse
	{
		// Token: 0x17000333 RID: 819
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x0000855C File Offset: 0x0000675C
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x00008564 File Offset: 0x00006764
		[XmlElement("PolicyInformation")]
		public PolicyInformation Policy { get; set; }

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x0000856D File Offset: 0x0000676D
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x00008575 File Offset: 0x00006775
		[XmlElement("PolicyList")]
		public PolicyList PolicyList { get; set; }
	}
}
