using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.WindowMessage
{
	// Token: 0x02000024 RID: 36
	[XmlRoot(ElementName = "PowerBroadcastEventSubscription", Namespace = null)]
	public sealed class PowerBroadcastEventSubscription
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000183 RID: 387 RVA: 0x0000461D File Offset: 0x0000281D
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00004625 File Offset: 0x00002825
		[XmlArray("PbtValueList")]
		[XmlArrayItem("PbtValue")]
		public int[] PbtValueList { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000185 RID: 389 RVA: 0x0000462E File Offset: 0x0000282E
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00004636 File Offset: 0x00002836
		[XmlArray("PowerSettingGuidList")]
		[XmlArrayItem("PowerSettingGuid")]
		public string[] PowerSettingGuidList { get; set; }
	}
}
