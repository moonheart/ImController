using System;
using System.Xml;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x0200002B RID: 43
	[XmlRoot(ElementName = "EventReaction", Namespace = null)]
	public sealed class EventReaction
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00004B8F File Offset: 0x00002D8F
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00004B97 File Offset: 0x00002D97
		[XmlAttribute(AttributeName = "monitor")]
		public string Monitor { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00004BA0 File Offset: 0x00002DA0
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00004BA8 File Offset: 0x00002DA8
		[XmlAttribute(AttributeName = "trigger")]
		public string Trigger { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00004BB1 File Offset: 0x00002DB1
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00004BB9 File Offset: 0x00002DB9
		[XmlAttribute(AttributeName = "dataType")]
		public string DataType { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00004BC2 File Offset: 0x00002DC2
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00004BCA File Offset: 0x00002DCA
		[XmlIgnore]
		public string Parameter { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00004BD3 File Offset: 0x00002DD3
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00004BE5 File Offset: 0x00002DE5
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
