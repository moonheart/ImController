using System;
using System.Runtime.Serialization;

namespace Lenovo.Modern.ImController.ContractBroker
{
	// Token: 0x02000009 RID: 9
	[DataContract]
	public class UDCEntitledResponse
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002393 File Offset: 0x00000593
		// (set) Token: 0x06000044 RID: 68 RVA: 0x0000239B File Offset: 0x0000059B
		[DataMember(Name = "isentitled")]
		public bool isEntitled { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000023A4 File Offset: 0x000005A4
		// (set) Token: 0x06000046 RID: 70 RVA: 0x000023AC File Offset: 0x000005AC
		[DataMember(Name = "campaignTags")]
		public string[] campaignTags { get; set; }
	}
}
