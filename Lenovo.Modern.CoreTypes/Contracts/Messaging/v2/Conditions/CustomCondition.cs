using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Conditions
{
	// Token: 0x0200008F RID: 143
	public sealed class CustomCondition
	{
		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x00007914 File Offset: 0x00005B14
		// (set) Token: 0x060005BB RID: 1467 RVA: 0x0000791C File Offset: 0x00005B1C
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x00007925 File Offset: 0x00005B25
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x0000792D File Offset: 0x00005B2D
		[XmlText]
		public string Value { get; set; }
	}
}
