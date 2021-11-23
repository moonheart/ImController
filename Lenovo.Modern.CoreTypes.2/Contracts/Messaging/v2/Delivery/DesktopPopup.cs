using System;
using System.Xml;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Delivery
{
	// Token: 0x02000089 RID: 137
	[XmlRoot("DesktopPopup")]
	public sealed class DesktopPopup
	{
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x00007791 File Offset: 0x00005991
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x00007799 File Offset: 0x00005999
		[XmlAttribute(AttributeName = "templateName")]
		public string TemplateName { get; set; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x000077A2 File Offset: 0x000059A2
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x000077AA File Offset: 0x000059AA
		[XmlIgnore]
		public string TemplateData { get; set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x000077B3 File Offset: 0x000059B3
		// (set) Token: 0x0600058F RID: 1423 RVA: 0x000077C5 File Offset: 0x000059C5
		[XmlElement(ElementName = "TemplateData")]
		public XmlCDataSection XmlIgnoreTemplateData
		{
			get
			{
				return new XmlDocument().CreateCDataSection(this.TemplateData);
			}
			set
			{
				this.TemplateData = value.Value;
			}
		}
	}
}
