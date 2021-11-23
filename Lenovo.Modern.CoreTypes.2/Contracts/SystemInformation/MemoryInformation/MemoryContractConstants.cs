using System;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation.MemoryInformation
{
	// Token: 0x0200004D RID: 77
	public sealed class MemoryContractConstants
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600034B RID: 843 RVA: 0x00005BC0 File Offset: 0x00003DC0
		public static MemoryContractConstants Get
		{
			get
			{
				MemoryContractConstants result;
				if ((result = MemoryContractConstants._contractConstants) == null)
				{
					MemoryContractConstants memoryContractConstants = new MemoryContractConstants();
					memoryContractConstants.ContractName = "SystemInformation.Memory";
					memoryContractConstants.ContractVersion = "1.0.0.0";
					memoryContractConstants.CommandNameGetCapability = "Get-Capability";
					memoryContractConstants.CommandNameGetMemoryInformation = "Get-MemoryInformation";
					memoryContractConstants.DataTypeCapability = "CapabilityResponse";
					memoryContractConstants.DataTypeMemoryInformation = "MemoryInformationResponse";
					result = memoryContractConstants;
					MemoryContractConstants._contractConstants = memoryContractConstants;
				}
				return result;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600034C RID: 844 RVA: 0x00005C23 File Offset: 0x00003E23
		// (set) Token: 0x0600034D RID: 845 RVA: 0x00005C2B File Offset: 0x00003E2B
		public string ContractName { get; private set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00005C34 File Offset: 0x00003E34
		// (set) Token: 0x0600034F RID: 847 RVA: 0x00005C3C File Offset: 0x00003E3C
		public string ContractVersion { get; private set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000350 RID: 848 RVA: 0x00005C45 File Offset: 0x00003E45
		// (set) Token: 0x06000351 RID: 849 RVA: 0x00005C4D File Offset: 0x00003E4D
		public string CommandNameGetMemoryInformation { get; private set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000352 RID: 850 RVA: 0x00005C56 File Offset: 0x00003E56
		// (set) Token: 0x06000353 RID: 851 RVA: 0x00005C5E File Offset: 0x00003E5E
		public string CommandNameGetCapability { get; private set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000354 RID: 852 RVA: 0x00005C67 File Offset: 0x00003E67
		// (set) Token: 0x06000355 RID: 853 RVA: 0x00005C6F File Offset: 0x00003E6F
		public string DataTypeMemoryInformation { get; private set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000356 RID: 854 RVA: 0x00005C78 File Offset: 0x00003E78
		// (set) Token: 0x06000357 RID: 855 RVA: 0x00005C80 File Offset: 0x00003E80
		public string DataTypeCapability { get; private set; }

		// Token: 0x040001C6 RID: 454
		private static MemoryContractConstants _contractConstants;
	}
}
