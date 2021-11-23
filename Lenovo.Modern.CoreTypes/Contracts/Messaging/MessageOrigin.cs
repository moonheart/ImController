using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging
{
	// Token: 0x02000072 RID: 114
	public enum MessageOrigin
	{
		// Token: 0x0400025C RID: 604
		[XmlEnum(Name = "offer")]
		Offer,
		// Token: 0x0400025D RID: 605
		[XmlEnum(Name = "notification")]
		Notification
	}
}
