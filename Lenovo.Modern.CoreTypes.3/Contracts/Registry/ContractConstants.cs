using System;

namespace Lenovo.Modern.CoreTypes.Contracts.Registry
{
	// Token: 0x02000055 RID: 85
	public sealed class ContractConstants
	{
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x00006064 File Offset: 0x00004264
		public static ContractConstants Get
		{
			get
			{
				ContractConstants result;
				if ((result = ContractConstants._contractConstants) == null)
				{
					ContractConstants contractConstants = new ContractConstants();
					contractConstants.ContractName = "SystemUtilities.Registry";
					contractConstants.ContractVersion = "1.0.0.0";
					contractConstants.DataTypeRegistry = "Registry";
					contractConstants.CommandNameGetRegistryKey = "Get-KeyChildren";
					contractConstants.CommandNameSetRegistryKey = "Set-KeyChildren";
					result = contractConstants;
					ContractConstants._contractConstants = contractConstants;
				}
				return result;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060003CA RID: 970 RVA: 0x000060BC File Offset: 0x000042BC
		// (set) Token: 0x060003CB RID: 971 RVA: 0x000060C4 File Offset: 0x000042C4
		public string ContractName { get; private set; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060003CC RID: 972 RVA: 0x000060CD File Offset: 0x000042CD
		// (set) Token: 0x060003CD RID: 973 RVA: 0x000060D5 File Offset: 0x000042D5
		public string ContractVersion { get; private set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060003CE RID: 974 RVA: 0x000060DE File Offset: 0x000042DE
		// (set) Token: 0x060003CF RID: 975 RVA: 0x000060E6 File Offset: 0x000042E6
		public string DataTypeRegistry { get; private set; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x000060EF File Offset: 0x000042EF
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x000060F7 File Offset: 0x000042F7
		public string CommandNameGetRegistryKey { get; private set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00006100 File Offset: 0x00004300
		// (set) Token: 0x060003D3 RID: 979 RVA: 0x00006108 File Offset: 0x00004308
		public string CommandNameSetRegistryKey { get; private set; }

		// Token: 0x04000202 RID: 514
		private static ContractConstants _contractConstants;
	}
}
