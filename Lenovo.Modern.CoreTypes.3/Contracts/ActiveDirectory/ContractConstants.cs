using System;

namespace Lenovo.Modern.CoreTypes.Contracts.ActiveDirectory
{
	// Token: 0x020000CA RID: 202
	public sealed class ContractConstants
	{
		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x00008580 File Offset: 0x00006780
		public static ContractConstants Get
		{
			get
			{
				ContractConstants result;
				if ((result = ContractConstants._contractConstants) == null)
				{
					ContractConstants contractConstants = new ContractConstants();
					contractConstants.ContractName = "SystemUtilities.ActiveDirectory";
					contractConstants.ContractVersion = "1.0.0.0";
					contractConstants.CommandNameGetPolicies = "Get-Policies";
					contractConstants.DataTypeAppPoliciesRequest = "AppPoliciesRequest";
					contractConstants.DataTypeAppPoliciesResponse = "AppPoliciesResponse";
					result = contractConstants;
					ContractConstants._contractConstants = contractConstants;
				}
				return result;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x000085D8 File Offset: 0x000067D8
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x000085E0 File Offset: 0x000067E0
		public string ContractName { get; private set; }

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x000085E9 File Offset: 0x000067E9
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x000085F1 File Offset: 0x000067F1
		public string ContractVersion { get; private set; }

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x000085FA File Offset: 0x000067FA
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x00008602 File Offset: 0x00006802
		public string CommandNameGetPolicies { get; private set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x0000860B File Offset: 0x0000680B
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x00008613 File Offset: 0x00006813
		public string DataTypeAppPoliciesRequest { get; private set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x0000861C File Offset: 0x0000681C
		// (set) Token: 0x0600073A RID: 1850 RVA: 0x00008624 File Offset: 0x00006824
		public string DataTypeAppPoliciesResponse { get; private set; }

		// Token: 0x04000392 RID: 914
		private static ContractConstants _contractConstants;
	}
}
