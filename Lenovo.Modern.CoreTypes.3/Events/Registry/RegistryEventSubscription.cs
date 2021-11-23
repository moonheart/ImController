using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.Registry
{
	// Token: 0x02000033 RID: 51
	[XmlRoot(ElementName = "RegistryEventSubscription", Namespace = null)]
	public sealed class RegistryEventSubscription
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000229 RID: 553 RVA: 0x00004DAD File Offset: 0x00002FAD
		// (set) Token: 0x0600022A RID: 554 RVA: 0x00004DB5 File Offset: 0x00002FB5
		[XmlElement(ElementName = "RegistryHiveName")]
		public string RegistryHiveName { get; set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00004DBE File Offset: 0x00002FBE
		// (set) Token: 0x0600022C RID: 556 RVA: 0x00004DC6 File Offset: 0x00002FC6
		[XmlElement(ElementName = "KeyPath")]
		public string KeyPath { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600022D RID: 557 RVA: 0x00004DCF File Offset: 0x00002FCF
		// (set) Token: 0x0600022E RID: 558 RVA: 0x00004DD7 File Offset: 0x00002FD7
		[XmlElement(ElementName = "ValueName")]
		public string ValueName { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00004DE0 File Offset: 0x00002FE0
		// (set) Token: 0x06000230 RID: 560 RVA: 0x00004DE8 File Offset: 0x00002FE8
		[XmlElement(ElementName = "MonitorRegTree")]
		public bool MonitorRegTree { get; set; }
	}
}
