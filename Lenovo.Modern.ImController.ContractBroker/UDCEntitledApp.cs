using System;
using System.Runtime.Serialization;

namespace Lenovo.Modern.ImController.ContractBroker
{
	// Token: 0x0200000B RID: 11
	[DataContract]
	public class UDCEntitledApp
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600004D RID: 77 RVA: 0x000023D7 File Offset: 0x000005D7
		// (set) Token: 0x0600004E RID: 78 RVA: 0x000023DF File Offset: 0x000005DF
		[DataMember(Name = "l1FruPn")]
		public string fruPn { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600004F RID: 79 RVA: 0x000023E8 File Offset: 0x000005E8
		// (set) Token: 0x06000050 RID: 80 RVA: 0x000023F0 File Offset: 0x000005F0
		[DataMember(Name = "partNum")]
		public string partNum { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000023F9 File Offset: 0x000005F9
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002401 File Offset: 0x00000601
		[DataMember(Name = "name")]
		public string name { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000053 RID: 83 RVA: 0x0000240A File Offset: 0x0000060A
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002412 File Offset: 0x00000612
		[DataMember(Name = "campaignTags")]
		public string[] campaignTags { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000055 RID: 85 RVA: 0x0000241B File Offset: 0x0000061B
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00002423 File Offset: 0x00000623
		[DataMember(Name = "moduleList")]
		public UDCSwModules[] moduleList { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000057 RID: 87 RVA: 0x0000242C File Offset: 0x0000062C
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002434 File Offset: 0x00000634
		[DataMember(Name = "status")]
		public string status { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000059 RID: 89 RVA: 0x0000243D File Offset: 0x0000063D
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00002445 File Offset: 0x00000645
		[DataMember(Name = "error")]
		public string error { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600005B RID: 91 RVA: 0x0000244E File Offset: 0x0000064E
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00002456 File Offset: 0x00000656
		[DataMember(Name = "progress")]
		public string progress { get; set; }
	}
}
