using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Delivery
{
	// Token: 0x0200008B RID: 139
	public sealed class Option
	{
		// Token: 0x17000288 RID: 648
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x00007828 File Offset: 0x00005A28
		// (set) Token: 0x0600059D RID: 1437 RVA: 0x00007830 File Offset: 0x00005A30
		[XmlAttribute(AttributeName = "content")]
		public string Content { get; set; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x00007839 File Offset: 0x00005A39
		// (set) Token: 0x0600059F RID: 1439 RVA: 0x00007841 File Offset: 0x00005A41
		[XmlAttribute(AttributeName = "protocol")]
		public string Protocol { get; set; }

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0000784A File Offset: 0x00005A4A
		// (set) Token: 0x060005A1 RID: 1441 RVA: 0x00007852 File Offset: 0x00005A52
		[XmlAttribute(AttributeName = "siftEventPlugin")]
		public string SiftEventPlugin { get; set; }

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x0000785B File Offset: 0x00005A5B
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x00007863 File Offset: 0x00005A63
		[XmlAttribute(AttributeName = "siftEventParam")]
		public string SiftEventParam { get; set; }
	}
}
