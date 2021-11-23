using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ActiveDirectory
{
	// Token: 0x020000C8 RID: 200
	[XmlRoot(ElementName = "AppPoliciesRequest", Namespace = "")]
	public sealed class AppPoliciesRequest
	{
		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0000854B File Offset: 0x0000674B
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x00008553 File Offset: 0x00006753
		[XmlElement("PolicyInformation")]
		public PolicyInformation PolicyInformation { get; set; }
	}
}
