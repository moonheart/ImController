using System;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation.StorageInformation
{
	// Token: 0x02000048 RID: 72
	public sealed class StorageContractConstants
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000301 RID: 769 RVA: 0x00005910 File Offset: 0x00003B10
		public static StorageContractConstants Get
		{
			get
			{
				StorageContractConstants result;
				if ((result = StorageContractConstants._contractConstants) == null)
				{
					StorageContractConstants storageContractConstants = new StorageContractConstants();
					storageContractConstants.ContractName = "SystemInformation.Storage";
					storageContractConstants.ContractVersion = "1.0.0.0";
					storageContractConstants.CommandNameGetCapability = "Get-Capability";
					storageContractConstants.CommandNameGetStorageInformation = "Get-StorageInformation";
					storageContractConstants.CommandNameWriteStorageInformation = "Write-StorageInformation";
					storageContractConstants.DataTypeCapability = "CapabilityResponse";
					storageContractConstants.DataTypeStorageInformation = "StorageInformationResponse";
					result = storageContractConstants;
					StorageContractConstants._contractConstants = storageContractConstants;
				}
				return result;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0000597E File Offset: 0x00003B7E
		// (set) Token: 0x06000303 RID: 771 RVA: 0x00005986 File Offset: 0x00003B86
		public string ContractName { get; private set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000598F File Offset: 0x00003B8F
		// (set) Token: 0x06000305 RID: 773 RVA: 0x00005997 File Offset: 0x00003B97
		public string ContractVersion { get; private set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000306 RID: 774 RVA: 0x000059A0 File Offset: 0x00003BA0
		// (set) Token: 0x06000307 RID: 775 RVA: 0x000059A8 File Offset: 0x00003BA8
		public string CommandNameGetCapability { get; private set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000308 RID: 776 RVA: 0x000059B1 File Offset: 0x00003BB1
		// (set) Token: 0x06000309 RID: 777 RVA: 0x000059B9 File Offset: 0x00003BB9
		public string CommandNameGetStorageInformation { get; private set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600030A RID: 778 RVA: 0x000059C2 File Offset: 0x00003BC2
		// (set) Token: 0x0600030B RID: 779 RVA: 0x000059CA File Offset: 0x00003BCA
		public string CommandNameWriteStorageInformation { get; private set; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600030C RID: 780 RVA: 0x000059D3 File Offset: 0x00003BD3
		// (set) Token: 0x0600030D RID: 781 RVA: 0x000059DB File Offset: 0x00003BDB
		public string DataTypeCapability { get; private set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600030E RID: 782 RVA: 0x000059E4 File Offset: 0x00003BE4
		// (set) Token: 0x0600030F RID: 783 RVA: 0x000059EC File Offset: 0x00003BEC
		public string DataTypeStorageInformation { get; private set; }

		// Token: 0x040001A3 RID: 419
		private static StorageContractConstants _contractConstants;
	}
}
