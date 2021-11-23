using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging
{
	// Token: 0x02000073 RID: 115
	public enum MessageActionType
	{
		// Token: 0x0400025F RID: 607
		[XmlEnum(Name = "webaddress")]
		WebAddress,
		// Token: 0x04000260 RID: 608
		[XmlEnum(Name = "protocol")]
		Protocol,
		// Token: 0x04000261 RID: 609
		[XmlEnum(Name = "installeditem")]
		InstalledItem,
		// Token: 0x04000262 RID: 610
		[XmlEnum(Name = "other")]
		Other
	}
}
