using System;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation.BatteryInformation
{
	// Token: 0x02000052 RID: 82
	public sealed class BatteryContractConstants
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00005D9C File Offset: 0x00003F9C
		public static BatteryContractConstants Get
		{
			get
			{
				BatteryContractConstants result;
				if ((result = BatteryContractConstants._contractConstants) == null)
				{
					BatteryContractConstants batteryContractConstants = new BatteryContractConstants();
					batteryContractConstants.ContractName = "SystemInformation.Battery";
					batteryContractConstants.ContractVersion = "1.0.0.0";
					batteryContractConstants.CommandNameGetCapability = "Get-Capability";
					batteryContractConstants.CommandNameGetBatteryInformation = "Get-BatteryInformation";
					batteryContractConstants.DataTypeCapability = "CapabilityResponse";
					batteryContractConstants.DataTypeBatteryInformation = "BatteryInformationResponse";
					result = batteryContractConstants;
					BatteryContractConstants._contractConstants = batteryContractConstants;
				}
				return result;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00005DFF File Offset: 0x00003FFF
		// (set) Token: 0x0600037F RID: 895 RVA: 0x00005E07 File Offset: 0x00004007
		public string ContractName { get; private set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00005E10 File Offset: 0x00004010
		// (set) Token: 0x06000381 RID: 897 RVA: 0x00005E18 File Offset: 0x00004018
		public string ContractVersion { get; private set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00005E21 File Offset: 0x00004021
		// (set) Token: 0x06000383 RID: 899 RVA: 0x00005E29 File Offset: 0x00004029
		public string CommandNameGetCapability { get; private set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00005E32 File Offset: 0x00004032
		// (set) Token: 0x06000385 RID: 901 RVA: 0x00005E3A File Offset: 0x0000403A
		public string CommandNameGetBatteryInformation { get; private set; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000386 RID: 902 RVA: 0x00005E43 File Offset: 0x00004043
		// (set) Token: 0x06000387 RID: 903 RVA: 0x00005E4B File Offset: 0x0000404B
		public string DataTypeCapability { get; private set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00005E54 File Offset: 0x00004054
		// (set) Token: 0x06000389 RID: 905 RVA: 0x00005E5C File Offset: 0x0000405C
		public string DataTypeBatteryInformation { get; private set; }

		// Token: 0x040001DD RID: 477
		private static BatteryContractConstants _contractConstants;
	}
}
