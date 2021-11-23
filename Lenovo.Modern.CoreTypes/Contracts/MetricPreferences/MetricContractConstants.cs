using System;

namespace Lenovo.Modern.CoreTypes.Contracts.MetricPreferences
{
	// Token: 0x02000064 RID: 100
	public sealed class MetricContractConstants
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x000062BC File Offset: 0x000044BC
		public static MetricContractConstants Get
		{
			get
			{
				MetricContractConstants result;
				if ((result = MetricContractConstants._contractConstants) == null)
				{
					MetricContractConstants metricContractConstants = new MetricContractConstants();
					metricContractConstants.ContractName = "MetricPreferences";
					metricContractConstants.ContractVersion = "1.0.0.0";
					metricContractConstants.CommandNameGetAppMetricCollectionSetting = "Get-AppMetricCollectionSetting";
					metricContractConstants.CommandNameGetAllAppMetricCollectionSettings = "Get-AllAppMetricCollectionSettings";
					metricContractConstants.CommandNameSetAppMetricCollectionSettings = "Set-AppMetricCollectionSettings";
					metricContractConstants.DataTypeAppIdentifer = "AppIdentifier";
					metricContractConstants.DataTypeAppMetricCollectionSettingRequest = "AppMetricCollectionSettingRequest";
					metricContractConstants.DatatypeAllAppsMetricCollectionSettingResponse = "AllAppsMetricCollectionSettingResponse";
					metricContractConstants.DataTypeAppMetricCollectionSettingResponse = "AppMetricCollectionResponse";
					result = metricContractConstants;
					MetricContractConstants._contractConstants = metricContractConstants;
				}
				return result;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00006340 File Offset: 0x00004540
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x00006348 File Offset: 0x00004548
		public string ContractName { get; private set; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x00006351 File Offset: 0x00004551
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x00006359 File Offset: 0x00004559
		public string ContractVersion { get; private set; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00006362 File Offset: 0x00004562
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x0000636A File Offset: 0x0000456A
		public string CommandNameGetAppMetricCollectionSetting { get; private set; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00006373 File Offset: 0x00004573
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x0000637B File Offset: 0x0000457B
		public string CommandNameGetAllAppMetricCollectionSettings { get; private set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00006384 File Offset: 0x00004584
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x0000638C File Offset: 0x0000458C
		public string CommandNameSetAppMetricCollectionSettings { get; private set; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x00006395 File Offset: 0x00004595
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x0000639D File Offset: 0x0000459D
		public string DataTypeAppMetricCollectionSettingRequest { get; private set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x000063A6 File Offset: 0x000045A6
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x000063AE File Offset: 0x000045AE
		public string DataTypeAppMetricCollectionSettingResponse { get; private set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x000063B7 File Offset: 0x000045B7
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x000063BF File Offset: 0x000045BF
		public string DataTypeAppIdentifer { get; private set; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x000063C8 File Offset: 0x000045C8
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x000063D0 File Offset: 0x000045D0
		public string DatatypeAllAppsMetricCollectionSettingResponse { get; private set; }

		// Token: 0x04000224 RID: 548
		private static MetricContractConstants _contractConstants;
	}
}
