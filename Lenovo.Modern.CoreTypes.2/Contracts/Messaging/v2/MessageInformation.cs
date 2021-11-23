using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2
{
	// Token: 0x02000080 RID: 128
	[XmlRoot(ElementName = "MessageInformation", Namespace = null)]
	public sealed class MessageInformation
	{
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00007593 File Offset: 0x00005793
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x0000759B File Offset: 0x0000579B
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x000075A4 File Offset: 0x000057A4
		// (set) Token: 0x06000549 RID: 1353 RVA: 0x000075AC File Offset: 0x000057AC
		[XmlAttribute(AttributeName = "friendlyName")]
		public string FriendlyName { get; set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x000075B5 File Offset: 0x000057B5
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x000075BD File Offset: 0x000057BD
		[XmlAttribute(AttributeName = "priority")]
		public string Priority { get; set; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x000075C6 File Offset: 0x000057C6
		// (set) Token: 0x0600054D RID: 1357 RVA: 0x000075CE File Offset: 0x000057CE
		[XmlAttribute(AttributeName = "category")]
		public string Category { get; set; }

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x000075D7 File Offset: 0x000057D7
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x000075DF File Offset: 0x000057DF
		[XmlAttribute(AttributeName = "association")]
		public string Association { get; set; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x000075E8 File Offset: 0x000057E8
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x000075F0 File Offset: 0x000057F0
		[XmlAttribute(AttributeName = "dateCreated")]
		public string DateCreated { get; set; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x000075F9 File Offset: 0x000057F9
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x00007601 File Offset: 0x00005801
		[XmlAttribute(AttributeName = "dateLastUpdated")]
		public string DateLastUpdated { get; set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x0000760A File Offset: 0x0000580A
		// (set) Token: 0x06000555 RID: 1365 RVA: 0x00007612 File Offset: 0x00005812
		[XmlAttribute(AttributeName = "formatVersion")]
		public string FormatVersion { get; set; }

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x0000761B File Offset: 0x0000581B
		// (set) Token: 0x06000557 RID: 1367 RVA: 0x00007623 File Offset: 0x00005823
		[XmlAttribute(AttributeName = "requestRequired")]
		public string RequestRequired { get; set; }
	}
}
