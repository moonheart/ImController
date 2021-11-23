using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation
{
	// Token: 0x02000043 RID: 67
	public enum BrandType
	{
		// Token: 0x04000140 RID: 320
		[XmlEnum(Name = "none")]
		None,
		// Token: 0x04000141 RID: 321
		[XmlEnum(Name = "other")]
		Other,
		// Token: 0x04000142 RID: 322
		[XmlEnum(Name = "lenovo")]
		Lenovo,
		// Token: 0x04000143 RID: 323
		[XmlEnum(Name = "think")]
		Think,
		// Token: 0x04000144 RID: 324
		[XmlEnum(Name = "idea")]
		Idea,
		// Token: 0x04000145 RID: 325
		[XmlEnum(Name = "ideatab")]
		IdeaTab,
		// Token: 0x04000146 RID: 326
		[XmlEnum(Name = "necconsumer")]
		NecConsumer,
		// Token: 0x04000147 RID: 327
		[XmlEnum(Name = "neccommercial")]
		NecCommercial,
		// Token: 0x04000148 RID: 328
		[XmlEnum(Name = "medion")]
		Medion
	}
}
