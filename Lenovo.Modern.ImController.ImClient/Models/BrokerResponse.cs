using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x02000024 RID: 36
	[XmlRoot(ElementName = "BrokerResponse", Namespace = null)]
	public sealed class BrokerResponse
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000048E6 File Offset: 0x00002AE6
		// (set) Token: 0x060000BA RID: 186 RVA: 0x000048EE File Offset: 0x00002AEE
		[XmlAttribute(AttributeName = "taskId")]
		public string TaskId { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000048F7 File Offset: 0x00002AF7
		// (set) Token: 0x060000BC RID: 188 RVA: 0x000048FF File Offset: 0x00002AFF
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00004908 File Offset: 0x00002B08
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00004910 File Offset: 0x00002B10
		[XmlAttribute(AttributeName = "result")]
		public string Result { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00004919 File Offset: 0x00002B19
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00004921 File Offset: 0x00002B21
		[XmlElement(ElementName = "BrokerResponseTask")]
		public BrokerResponseTask Task { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x0000492A File Offset: 0x00002B2A
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00004932 File Offset: 0x00002B32
		[XmlElement(ElementName = "FailureData")]
		public FailureData Error { get; set; }
	}
}
