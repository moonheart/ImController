using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Commands
{
	// Token: 0x0200009D RID: 157
	public sealed class Substitution
	{
		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x00007B45 File Offset: 0x00005D45
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x00007B4D File Offset: 0x00005D4D
		[XmlAttribute(AttributeName = "key")]
		public string Key { get; set; }

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x00007B56 File Offset: 0x00005D56
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x00007B5E File Offset: 0x00005D5E
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
	}
}
