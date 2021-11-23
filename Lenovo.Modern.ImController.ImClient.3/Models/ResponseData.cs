using System;
using System.Xml;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x02000029 RID: 41
	public sealed class ResponseData
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00004B4D File Offset: 0x00002D4D
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00004B55 File Offset: 0x00002D55
		[XmlAttribute(AttributeName = "dataType")]
		public string DataType { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00004B5E File Offset: 0x00002D5E
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00004B66 File Offset: 0x00002D66
		[XmlIgnore]
		public string Data { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00004B6F File Offset: 0x00002D6F
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00004B81 File Offset: 0x00002D81
		[XmlElement(ElementName = "Data")]
		public XmlCDataSection DataCCDATA
		{
			get
			{
				return new XmlDocument().CreateCDataSection(this.Data);
			}
			set
			{
				this.Data = value.Value;
			}
		}
	}
}
