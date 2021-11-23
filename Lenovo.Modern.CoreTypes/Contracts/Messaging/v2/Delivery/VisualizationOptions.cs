using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Delivery
{
	// Token: 0x0200008D RID: 141
	[XmlRoot(ElementName = "VisualizationOptions", Namespace = null)]
	public sealed class VisualizationOptions
	{
		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x0000788E File Offset: 0x00005A8E
		// (set) Token: 0x060005AB RID: 1451 RVA: 0x00007896 File Offset: 0x00005A96
		[XmlElement(ElementName = "WindowsToast")]
		public WindowsToast WindowsToast { get; set; }

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x0000789F File Offset: 0x00005A9F
		// (set) Token: 0x060005AD RID: 1453 RVA: 0x000078A7 File Offset: 0x00005AA7
		[XmlElement(ElementName = "DesktopPopup")]
		public DesktopPopup DesktopPopup { get; set; }
	}
}
