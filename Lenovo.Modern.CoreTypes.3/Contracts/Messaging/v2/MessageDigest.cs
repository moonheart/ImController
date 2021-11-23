using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2
{
	// Token: 0x02000082 RID: 130
	[XmlRoot("MessageDigest")]
	public sealed class MessageDigest
	{
		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00007681 File Offset: 0x00005881
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x00007689 File Offset: 0x00005889
		[XmlElement(ElementName = "Information")]
		public MessageDigestInformation Information { get; set; }

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00007692 File Offset: 0x00005892
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x0000769A File Offset: 0x0000589A
		[XmlArray("MessageList")]
		[XmlArrayItem("Message")]
		public Message[] MessageList { get; set; }
	}
}
