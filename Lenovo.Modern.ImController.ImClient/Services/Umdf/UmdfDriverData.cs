using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ImClient.Services.Umdf
{
	// Token: 0x02000020 RID: 32
	[XmlRoot(ElementName = "UmdfDriverData", Namespace = null)]
	public sealed class UmdfDriverData
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x0000483C File Offset: 0x00002A3C
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00004844 File Offset: 0x00002A44
		[XmlAttribute(AttributeName = "TaskId")]
		public Guid TaskId { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000484D File Offset: 0x00002A4D
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00004855 File Offset: 0x00002A55
		[XmlAttribute(AttributeName = "XmlString")]
		public string XmlString { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x0000485E File Offset: 0x00002A5E
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00004866 File Offset: 0x00002A66
		[XmlAttribute(AttributeName = "DriverPath")]
		public string DriverPath { get; set; }
	}
}
