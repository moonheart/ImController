using System;
using System.Xml;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Delivery
{
	// Token: 0x0200008E RID: 142
	[XmlRoot("WindowsToast")]
	public sealed class WindowsToast
	{
		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x000078B0 File Offset: 0x00005AB0
		// (set) Token: 0x060005B0 RID: 1456 RVA: 0x000078B8 File Offset: 0x00005AB8
		[XmlAttribute(AttributeName = "timespan")]
		public string Timespan { get; set; }

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x000078C1 File Offset: 0x00005AC1
		// (set) Token: 0x060005B2 RID: 1458 RVA: 0x000078C9 File Offset: 0x00005AC9
		[XmlAttribute(AttributeName = "aumid")]
		public string Aumid { get; set; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x000078D2 File Offset: 0x00005AD2
		// (set) Token: 0x060005B4 RID: 1460 RVA: 0x000078DA File Offset: 0x00005ADA
		[XmlAttribute(AttributeName = "pfn")]
		public string Pfn { get; set; }

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x000078E3 File Offset: 0x00005AE3
		// (set) Token: 0x060005B6 RID: 1462 RVA: 0x000078EB File Offset: 0x00005AEB
		[XmlIgnore]
		public string TemplateData { get; set; }

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x000078F4 File Offset: 0x00005AF4
		// (set) Token: 0x060005B8 RID: 1464 RVA: 0x00007906 File Offset: 0x00005B06
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
