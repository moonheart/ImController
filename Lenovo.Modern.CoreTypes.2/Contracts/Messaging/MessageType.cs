using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging
{
	// Token: 0x02000071 RID: 113
	public enum MessageType
	{
		// Token: 0x04000256 RID: 598
		[XmlEnum(Name = "livetile")]
		LiveTile,
		// Token: 0x04000257 RID: 599
		[XmlEnum(Name = "toast")]
		Toast,
		// Token: 0x04000258 RID: 600
		[XmlEnum(Name = "criticalalert")]
		CriticalAlert,
		// Token: 0x04000259 RID: 601
		[XmlEnum(Name = "inappmessage")]
		InAppMessage,
		// Token: 0x0400025A RID: 602
		[XmlEnum(Name = "inappadvertisement")]
		InAppAdvertisement
	}
}
