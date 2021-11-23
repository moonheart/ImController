using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Commands
{
	// Token: 0x020000A5 RID: 165
	[XmlRoot(ElementName = "NotificationInvokedDetails", Namespace = null)]
	public sealed class NotificationInvokedDetailsRequest
	{
		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x00007C77 File Offset: 0x00005E77
		// (set) Token: 0x06000639 RID: 1593 RVA: 0x00007C7F File Offset: 0x00005E7F
		[XmlAttribute(AttributeName = "arguments")]
		public string arguments { get; set; }
	}
}
