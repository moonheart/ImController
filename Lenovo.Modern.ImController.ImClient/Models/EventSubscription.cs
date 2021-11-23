using System;
using System.Xml;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x0200002D RID: 45
	public sealed class EventSubscription
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00004C79 File Offset: 0x00002E79
		// (set) Token: 0x06000113 RID: 275 RVA: 0x00004C81 File Offset: 0x00002E81
		[XmlAttribute(AttributeName = "monitor")]
		public string Monitor { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00004C8A File Offset: 0x00002E8A
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00004C92 File Offset: 0x00002E92
		[XmlAttribute(AttributeName = "trigger")]
		public string Trigger { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00004C9B File Offset: 0x00002E9B
		// (set) Token: 0x06000117 RID: 279 RVA: 0x00004CA3 File Offset: 0x00002EA3
		[XmlIgnore]
		public string Parameter { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00004CAC File Offset: 0x00002EAC
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00004CBE File Offset: 0x00002EBE
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
	}
}
