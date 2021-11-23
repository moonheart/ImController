using System;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;

namespace Lenovo.Modern.CoreTypes.Models
{
	// Token: 0x0200000B RID: 11
	public interface IBasicEligabilityRequirements
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600004C RID: 76
		// (set) Token: 0x0600004D RID: 77
		BrandType Brand { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600004E RID: 78
		// (set) Token: 0x0600004F RID: 79
		string Country { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000050 RID: 80
		// (set) Token: 0x06000051 RID: 81
		string SubBrand { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000052 RID: 82
		// (set) Token: 0x06000053 RID: 83
		string Language { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000054 RID: 84
		// (set) Token: 0x06000055 RID: 85
		string FriendlyName { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000056 RID: 86
		// (set) Token: 0x06000057 RID: 87
		EnclosureType EnclosureType { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000058 RID: 88
		// (set) Token: 0x06000059 RID: 89
		string AppVersion { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600005A RID: 90
		// (set) Token: 0x0600005B RID: 91
		string OsVersion { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600005C RID: 92
		// (set) Token: 0x0600005D RID: 93
		string OsBitness { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600005E RID: 94
		// (set) Token: 0x0600005F RID: 95
		string Tag { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000060 RID: 96
		// (set) Token: 0x06000061 RID: 97
		string Manufacturer { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000062 RID: 98
		// (set) Token: 0x06000063 RID: 99
		string Mtm { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000064 RID: 100
		// (set) Token: 0x06000065 RID: 101
		string Family { get; set; }
	}
}
