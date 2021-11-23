using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.WindowMessage
{
	// Token: 0x02000026 RID: 38
	[XmlRoot(ElementName = "SessionChangeEventSubscription", Namespace = null)]
	public sealed class SessionChangeEventSubscription
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00004661 File Offset: 0x00002861
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00004669 File Offset: 0x00002869
		[XmlArray("WtsSessionValueList")]
		[XmlArrayItem("WtsSessionValue")]
		public int[] WtsSessionValueList { get; set; }
	}
}
