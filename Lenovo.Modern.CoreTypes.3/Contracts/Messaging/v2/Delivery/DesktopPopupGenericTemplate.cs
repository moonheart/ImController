using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Delivery
{
	// Token: 0x0200008A RID: 138
	[XmlRoot(ElementName = "GenericTemplateModel", Namespace = null)]
	public sealed class DesktopPopupGenericTemplate
	{
		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x000077D3 File Offset: 0x000059D3
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x000077DB File Offset: 0x000059DB
		[XmlElement(ElementName = "Header")]
		public string Header { get; set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x000077E4 File Offset: 0x000059E4
		// (set) Token: 0x06000594 RID: 1428 RVA: 0x000077EC File Offset: 0x000059EC
		[XmlElement(ElementName = "Subheader")]
		public string Subheader { get; set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x000077F5 File Offset: 0x000059F5
		// (set) Token: 0x06000596 RID: 1430 RVA: 0x000077FD File Offset: 0x000059FD
		[XmlElement(ElementName = "Description")]
		public string Description { get; set; }

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00007806 File Offset: 0x00005A06
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x0000780E File Offset: 0x00005A0E
		[XmlElement(ElementName = "LearnmoreDescription")]
		public string LearnMoreDescription { get; set; }

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x00007817 File Offset: 0x00005A17
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x0000781F File Offset: 0x00005A1F
		[XmlArray("OptionList")]
		[XmlArrayItem("Option")]
		public Option[] OptionList { get; set; }
	}
}
