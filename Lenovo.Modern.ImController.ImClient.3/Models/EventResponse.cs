using System;
using System.Xml;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x0200002C RID: 44
	public sealed class EventResponse
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00004BF3 File Offset: 0x00002DF3
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00004BFB File Offset: 0x00002DFB
		[XmlElement]
		public string Data { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00004C04 File Offset: 0x00002E04
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00004C0C File Offset: 0x00002E0C
		[XmlAttribute(AttributeName = "taskId")]
		public string TaskId { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00004C15 File Offset: 0x00002E15
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00004C1D File Offset: 0x00002E1D
		[XmlAttribute(AttributeName = "keepAliveMins")]
		public int KeepAliveMinutes { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00004C26 File Offset: 0x00002E26
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00004C2E File Offset: 0x00002E2E
		[XmlAttribute(AttributeName = "dataType")]
		public string DataType { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00004C37 File Offset: 0x00002E37
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00004C3F File Offset: 0x00002E3F
		[XmlIgnore]
		public string Parameter { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00004C48 File Offset: 0x00002E48
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00004C5A File Offset: 0x00002E5A
		[XmlElement(ElementName = "Parameter")]
		public XmlCDataSection ParameterCCDATA
		{
			get
			{
				return new XmlDocument().CreateCDataSection(this.Parameter);
			}
			set
			{
				this.Parameter = value.Value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00004C68 File Offset: 0x00002E68
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00004C70 File Offset: 0x00002E70
		[XmlElement(ElementName = "FailureData")]
		public FailureData Error { get; set; }
	}
}
