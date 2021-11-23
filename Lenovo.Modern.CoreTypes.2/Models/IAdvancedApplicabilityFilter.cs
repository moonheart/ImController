using System;
using Lenovo.Modern.CoreTypes.Contracts.Messaging;

namespace Lenovo.Modern.CoreTypes.Models
{
	// Token: 0x0200000A RID: 10
	public interface IAdvancedApplicabilityFilter
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000045 RID: 69
		NegatableBrand[] BrandList { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000046 RID: 70
		NegatableString[] FamilyList { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000047 RID: 71
		NegatableString[] MtmList { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000048 RID: 72
		NegatableString[] SubBrandList { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000049 RID: 73
		NegatableString[] LangList { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600004A RID: 74
		NegatableString[] CountryList { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600004B RID: 75
		NegatableKeyValuePair[] TagList { get; }
	}
}
