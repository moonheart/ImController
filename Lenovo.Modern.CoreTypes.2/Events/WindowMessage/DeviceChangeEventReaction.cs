using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.WindowMessage
{
	// Token: 0x02000021 RID: 33
	[XmlRoot(ElementName = "DeviceChangeEventReaction", Namespace = null)]
	public sealed class DeviceChangeEventReaction
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00004584 File Offset: 0x00002784
		// (set) Token: 0x0600016F RID: 367 RVA: 0x0000458C File Offset: 0x0000278C
		[XmlElement(ElementName = "DbtEventValue")]
		public int DbtEventValue { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00004595 File Offset: 0x00002795
		// (set) Token: 0x06000171 RID: 369 RVA: 0x0000459D File Offset: 0x0000279D
		[XmlElement(ElementName = "DevInterfaceClassGuid")]
		public string DevInterfaceClassGuid { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000172 RID: 370 RVA: 0x000045A6 File Offset: 0x000027A6
		// (set) Token: 0x06000173 RID: 371 RVA: 0x000045AE File Offset: 0x000027AE
		[XmlElement(ElementName = "DbccSize")]
		public int DbccSize { get; set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000174 RID: 372 RVA: 0x000045B7 File Offset: 0x000027B7
		// (set) Token: 0x06000175 RID: 373 RVA: 0x000045BF File Offset: 0x000027BF
		[XmlElement(ElementName = "Data", DataType = "base64Binary")]
		public byte[] Data { get; set; }
	}
}
