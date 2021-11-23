using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ActiveDirectory
{
	// Token: 0x020000CD RID: 205
	[XmlRoot(ElementName = "PolicyList", Namespace = null)]
	public sealed class PolicyList
	{
		// Token: 0x17000342 RID: 834
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x000086A4 File Offset: 0x000068A4
		// (set) Token: 0x0600074D RID: 1869 RVA: 0x000086AC File Offset: 0x000068AC
		[XmlArrayItem("Policy")]
		public Policy[] Policy { get; set; }
	}
}
