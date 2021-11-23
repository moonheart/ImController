using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Commands
{
	// Token: 0x020000A1 RID: 161
	[XmlRoot(ElementName = "MessagingPreferenceGetResponse", Namespace = null)]
	public sealed class MessagingPreferenceGetResponse
	{
		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x00007BEF File Offset: 0x00005DEF
		// (set) Token: 0x06000625 RID: 1573 RVA: 0x00007BF7 File Offset: 0x00005DF7
		[XmlElement(ElementName = "AppPreference")]
		public ApplicationPreference AppPreference { get; set; }
	}
}
