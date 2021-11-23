using System;
using System.Xml.Serialization;
using Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Delivery;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2
{
	// Token: 0x0200007F RID: 127
	[XmlRoot(ElementName = "Message", Namespace = null)]
	public sealed class Message
	{
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x00007560 File Offset: 0x00005760
		// (set) Token: 0x06000540 RID: 1344 RVA: 0x00007568 File Offset: 0x00005768
		[XmlElement(ElementName = "Information", IsNullable = true)]
		public MessageInformation Information { get; set; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x00007571 File Offset: 0x00005771
		// (set) Token: 0x06000542 RID: 1346 RVA: 0x00007579 File Offset: 0x00005779
		[XmlElement(ElementName = "Conditions", IsNullable = true)]
		public MessageConditions Conditions { get; set; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x00007582 File Offset: 0x00005782
		// (set) Token: 0x06000544 RID: 1348 RVA: 0x0000758A File Offset: 0x0000578A
		[XmlElement(ElementName = "Delivery", IsNullable = true)]
		public MessageDelivery Delivery { get; set; }
	}
}
