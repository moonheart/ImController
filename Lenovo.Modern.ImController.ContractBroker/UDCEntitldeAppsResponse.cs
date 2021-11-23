using System;
using System.Runtime.Serialization;

namespace Lenovo.Modern.ImController.ContractBroker
{
	// Token: 0x0200000A RID: 10
	[DataContract]
	public class UDCEntitldeAppsResponse
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000048 RID: 72 RVA: 0x000023B5 File Offset: 0x000005B5
		// (set) Token: 0x06000049 RID: 73 RVA: 0x000023BD File Offset: 0x000005BD
		[DataMember(Name = "swCount")]
		public int swCount { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600004A RID: 74 RVA: 0x000023C6 File Offset: 0x000005C6
		// (set) Token: 0x0600004B RID: 75 RVA: 0x000023CE File Offset: 0x000005CE
		[DataMember(Name = "swList")]
		public UDCEntitledApp[] swList { get; set; }
	}
}
