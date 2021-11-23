using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.WindowMessage
{
	// Token: 0x02000023 RID: 35
	[XmlRoot(ElementName = "PowerBroadcastEventReaction", Namespace = null)]
	public sealed class PowerBroadcastEventReaction
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600017A RID: 378 RVA: 0x000045D9 File Offset: 0x000027D9
		// (set) Token: 0x0600017B RID: 379 RVA: 0x000045E1 File Offset: 0x000027E1
		[XmlElement(ElementName = "PbtValue")]
		public int PbtValue { get; set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600017C RID: 380 RVA: 0x000045EA File Offset: 0x000027EA
		// (set) Token: 0x0600017D RID: 381 RVA: 0x000045F2 File Offset: 0x000027F2
		[XmlElement(ElementName = "PowerSettingGuid")]
		public string PowerSettingGuid { get; set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600017E RID: 382 RVA: 0x000045FB File Offset: 0x000027FB
		// (set) Token: 0x0600017F RID: 383 RVA: 0x00004603 File Offset: 0x00002803
		[XmlElement(ElementName = "DataLength")]
		public int DataLength { get; set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000460C File Offset: 0x0000280C
		// (set) Token: 0x06000181 RID: 385 RVA: 0x00004614 File Offset: 0x00002814
		[XmlElement(ElementName = "Data", DataType = "base64Binary")]
		public byte[] Data { get; set; }
	}
}
