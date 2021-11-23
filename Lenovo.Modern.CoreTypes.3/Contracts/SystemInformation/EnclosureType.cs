using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation
{
	// Token: 0x02000042 RID: 66
	public enum EnclosureType
	{
		// Token: 0x04000131 RID: 305
		[XmlEnum(Name = "none")]
		None,
		// Token: 0x04000132 RID: 306
		[XmlEnum(Name = "notebook")]
		Notebook,
		// Token: 0x04000133 RID: 307
		[XmlEnum(Name = "laptop")]
		Laptop,
		// Token: 0x04000134 RID: 308
		[XmlEnum(Name = "desktop")]
		Desktop,
		// Token: 0x04000135 RID: 309
		[XmlEnum(Name = "tower")]
		Tower,
		// Token: 0x04000136 RID: 310
		[XmlEnum(Name = "allinone")]
		AllInOne,
		// Token: 0x04000137 RID: 311
		[XmlEnum(Name = "workstation")]
		Workstation,
		// Token: 0x04000138 RID: 312
		[XmlEnum(Name = "handheld")]
		HandHeld,
		// Token: 0x04000139 RID: 313
		[XmlEnum(Name = "tablet")]
		Tablet,
		// Token: 0x0400013A RID: 314
		[XmlEnum(Name = "convertible")]
		Convertible,
		// Token: 0x0400013B RID: 315
		[XmlEnum(Name = "detachable")]
		Detachable,
		// Token: 0x0400013C RID: 316
		[XmlEnum(Name = "StickPc")]
		StickPc,
		// Token: 0x0400013D RID: 317
		[XmlEnum(Name = "MiniPc")]
		MiniPc,
		// Token: 0x0400013E RID: 318
		[XmlEnum(Name = "other")]
		Other
	}
}
