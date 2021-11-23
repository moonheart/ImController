using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ImController
{
	// Token: 0x020000B2 RID: 178
	[XmlRoot(ElementName = "AppPoliciesResponse", Namespace = null)]
	public sealed class StatusResponse
	{
		// Token: 0x170002FD RID: 765
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x000080D1 File Offset: 0x000062D1
		// (set) Token: 0x060006AB RID: 1707 RVA: 0x000080D9 File Offset: 0x000062D9
		[XmlElement("StatusResponse")]
		public Status Status { get; set; }
	}
}
