using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x02000022 RID: 34
	public sealed class BrokerRequirements
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00004880 File Offset: 0x00002A80
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00004888 File Offset: 0x00002A88
		[XmlAttribute(AttributeName = "minVersion")]
		public string MinVersion { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00004891 File Offset: 0x00002A91
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00004899 File Offset: 0x00002A99
		[XmlAttribute(AttributeName = "maxVersion")]
		public string MaxVersion { get; set; }
	}
}
