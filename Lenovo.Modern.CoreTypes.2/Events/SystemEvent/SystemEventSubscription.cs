using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.SystemEvent
{
	// Token: 0x02000030 RID: 48
	[XmlRoot(ElementName = "SystemEventSubscription", Namespace = null)]
	public sealed class SystemEventSubscription
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00004CB3 File Offset: 0x00002EB3
		// (set) Token: 0x06000212 RID: 530 RVA: 0x00004CBB File Offset: 0x00002EBB
		[XmlElement(ElementName = "SystemShutdown")]
		public bool SystemShutdown { get; set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00004CC4 File Offset: 0x00002EC4
		// (set) Token: 0x06000214 RID: 532 RVA: 0x00004CCC File Offset: 0x00002ECC
		[XmlElement(ElementName = "SystemStart")]
		public bool SystemStart { get; set; }
	}
}
