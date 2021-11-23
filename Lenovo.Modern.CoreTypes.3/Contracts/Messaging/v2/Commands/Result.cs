using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Commands
{
	// Token: 0x0200009B RID: 155
	public enum Result
	{
		// Token: 0x0400030D RID: 781
		[XmlEnum(Name = "success")]
		Success,
		// Token: 0x0400030E RID: 782
		[XmlEnum(Name = "fail")]
		Fail,
		// Token: 0x0400030F RID: 783
		[XmlEnum(Name = "warning")]
		Warning
	}
}
