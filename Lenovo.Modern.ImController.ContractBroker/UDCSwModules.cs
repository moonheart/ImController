using System;
using System.Runtime.Serialization;

namespace Lenovo.Modern.ImController.ContractBroker
{
	// Token: 0x0200000C RID: 12
	[DataContract]
	public class UDCSwModules
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600005E RID: 94 RVA: 0x0000245F File Offset: 0x0000065F
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00002467 File Offset: 0x00000667
		[DataMember(Name = "moduleFruPn")]
		public string fruPn { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002470 File Offset: 0x00000670
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00002478 File Offset: 0x00000678
		[DataMember(Name = "type")]
		public string type { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002481 File Offset: 0x00000681
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00002489 File Offset: 0x00000689
		[DataMember(Name = "name")]
		public string name { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002492 File Offset: 0x00000692
		// (set) Token: 0x06000065 RID: 101 RVA: 0x0000249A File Offset: 0x0000069A
		[DataMember(Name = "version")]
		public string version { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000066 RID: 102 RVA: 0x000024A3 File Offset: 0x000006A3
		// (set) Token: 0x06000067 RID: 103 RVA: 0x000024AB File Offset: 0x000006AB
		[DataMember(Name = "moduleName")]
		public string moduleName { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000068 RID: 104 RVA: 0x000024B4 File Offset: 0x000006B4
		// (set) Token: 0x06000069 RID: 105 RVA: 0x000024BC File Offset: 0x000006BC
		[DataMember(Name = "downloadFrom")]
		public string detectCmd { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000024C5 File Offset: 0x000006C5
		// (set) Token: 0x0600006B RID: 107 RVA: 0x000024CD File Offset: 0x000006CD
		[DataMember(Name = "licenseKey")]
		public string licenseKey { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600006C RID: 108 RVA: 0x000024D6 File Offset: 0x000006D6
		// (set) Token: 0x0600006D RID: 109 RVA: 0x000024DE File Offset: 0x000006DE
		[DataMember(Name = "redemptionUrl")]
		public string redemptionUrl { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000024E7 File Offset: 0x000006E7
		// (set) Token: 0x0600006F RID: 111 RVA: 0x000024EF File Offset: 0x000006EF
		[DataMember(Name = "licenseAgreementUrl")]
		public string licenseAgreementUrl { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000070 RID: 112 RVA: 0x000024F8 File Offset: 0x000006F8
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00002500 File Offset: 0x00000700
		[DataMember(Name = "swHomePageUrl")]
		public string swHomePageUrl { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002509 File Offset: 0x00000709
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00002511 File Offset: 0x00000711
		[DataMember(Name = "size")]
		public uint size { get; set; }
	}
}
