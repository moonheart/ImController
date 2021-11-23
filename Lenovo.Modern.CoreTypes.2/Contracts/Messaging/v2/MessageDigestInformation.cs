using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2
{
	// Token: 0x02000083 RID: 131
	[XmlRoot("MessageDigestInformation")]
	public sealed class MessageDigestInformation
	{
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x000076A3 File Offset: 0x000058A3
		// (set) Token: 0x0600056A RID: 1386 RVA: 0x000076AB File Offset: 0x000058AB
		[XmlElement(ElementName = "DateLastModified")]
		public string DateLastModified { get; set; }

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x000076B4 File Offset: 0x000058B4
		// (set) Token: 0x0600056C RID: 1388 RVA: 0x000076BC File Offset: 0x000058BC
		[XmlElement(ElementName = "FriendlyName")]
		public string FriendlyName { get; set; }

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x000076C5 File Offset: 0x000058C5
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x000076CD File Offset: 0x000058CD
		[XmlElement(ElementName = "MinVersion")]
		public string MinVersion { get; set; }
	}
}
