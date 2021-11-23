using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Commands
{
	// Token: 0x020000A0 RID: 160
	[XmlRoot(ElementName = "MessagingPreferenceGetRequest", Namespace = null)]
	public sealed class MessagingPreferenceGetRequest
	{
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x00007BCD File Offset: 0x00005DCD
		// (set) Token: 0x06000620 RID: 1568 RVA: 0x00007BD5 File Offset: 0x00005DD5
		[XmlAttribute(AttributeName = "appId")]
		public string AppId { get; set; }

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x00007BDE File Offset: 0x00005DDE
		// (set) Token: 0x06000622 RID: 1570 RVA: 0x00007BE6 File Offset: 0x00005DE6
		[XmlAttribute(AttributeName = "lang")]
		public string lang { get; set; }
	}
}
