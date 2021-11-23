using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging
{
	// Token: 0x02000070 RID: 112
	public enum MessagePriority
	{
		// Token: 0x04000252 RID: 594
		[XmlEnum(Name = "high")]
		High,
		// Token: 0x04000253 RID: 595
		[XmlEnum(Name = "medium")]
		Medium,
		// Token: 0x04000254 RID: 596
		[XmlEnum(Name = "low")]
		Low
	}
}
