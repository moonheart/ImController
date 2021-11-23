using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation.MemoryInformation
{
	// Token: 0x0200004F RID: 79
	[XmlRoot(ElementName = "MemoryInformationResponse", Namespace = null)]
	public sealed class MemoryInformationResponse
	{
		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600035A RID: 858 RVA: 0x00005C89 File Offset: 0x00003E89
		// (set) Token: 0x0600035B RID: 859 RVA: 0x00005C91 File Offset: 0x00003E91
		[XmlElement("SystemMemoryInformation")]
		public SystemMemoryInformation SystemMemoryInformation { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00005C9A File Offset: 0x00003E9A
		// (set) Token: 0x0600035D RID: 861 RVA: 0x00005CA2 File Offset: 0x00003EA2
		[XmlArray("MemorySlotList")]
		[XmlArrayItem("MemorySlot")]
		public MemorySlot[] MemorySlotList { get; set; }
	}
}
