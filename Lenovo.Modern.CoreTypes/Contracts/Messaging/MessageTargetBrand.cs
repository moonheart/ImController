using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging
{
	// Token: 0x02000074 RID: 116
	public enum MessageTargetBrand
	{
		// Token: 0x04000264 RID: 612
		[XmlEnum(Name = "other")]
		Other,
		// Token: 0x04000265 RID: 613
		[XmlEnum(Name = "lenovo")]
		Lenovo,
		// Token: 0x04000266 RID: 614
		[XmlEnum(Name = "think")]
		Think,
		// Token: 0x04000267 RID: 615
		[XmlEnum(Name = "idea")]
		Idea,
		// Token: 0x04000268 RID: 616
		[XmlEnum(Name = "idea-tab")]
		IdeaTab,
		// Token: 0x04000269 RID: 617
		[XmlEnum(Name = "nec-consumer")]
		NecConsumer,
		// Token: 0x0400026A RID: 618
		[XmlEnum(Name = "nec-commercial")]
		NecCommercial,
		// Token: 0x0400026B RID: 619
		[XmlEnum(Name = "medion")]
		Medion
	}
}
