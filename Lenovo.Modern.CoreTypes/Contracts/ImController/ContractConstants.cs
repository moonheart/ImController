using System;

namespace Lenovo.Modern.CoreTypes.Contracts.ImController
{
	// Token: 0x020000AB RID: 171
	public sealed class ContractConstants
	{
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x00007DDC File Offset: 0x00005FDC
		public static ContractConstants Get
		{
			get
			{
				ContractConstants result;
				if ((result = ContractConstants._contractConstants) == null)
				{
					ContractConstants contractConstants = new ContractConstants();
					contractConstants.ContractName = "ImController";
					contractConstants.ContractVersion = "1.0.0.0";
					contractConstants.CommandNameGetStatus = "Get-Status";
					contractConstants.DataTypeStatusRequest = "StatusRequest";
					contractConstants.DataTypeStatusResponse = "StatusResponse";
					contractConstants.StatusNormal = "Running";
					contractConstants.StatusMaintenanceMode = "MaintenanceMode";
					contractConstants.CommandNameGetPendingUpdates = "Get-PendingUpdates";
					contractConstants.DataTypePendingUpdateRequest = "PendingUpdateRequest";
					contractConstants.DataTypePendingUpdateResponse = "PendingUpdateResponse";
					contractConstants.CommandNameInstallPendingUpdates = "Install-PendingUpdates";
					contractConstants.DataTypeInstallPendingRequest = "InstallPendingRequest";
					contractConstants.DataTypeInstallPendingResponse = "InstallPendingResponse";
					contractConstants.CommandNameRestart = "Request - Restart";
					contractConstants.CommandNameIsEntitled = "Is-Entitled";
					contractConstants.DataTypeEntitledResponse = "EntitledResponse";
					contractConstants.CommandNameGetEntitledApps = "Get-EntitledApps";
					contractConstants.DataTypeEntitledAppsResponse = "EntitledAppsResponse";
					contractConstants.CommandNameInstallEntitledApps = "Install-EntitledApps";
					result = contractConstants;
					ContractConstants._contractConstants = contractConstants;
				}
				return result;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x00007ED1 File Offset: 0x000060D1
		// (set) Token: 0x0600066A RID: 1642 RVA: 0x00007ED9 File Offset: 0x000060D9
		public string ContractName { get; private set; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00007EE2 File Offset: 0x000060E2
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x00007EEA File Offset: 0x000060EA
		public string ContractVersion { get; private set; }

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x00007EF3 File Offset: 0x000060F3
		// (set) Token: 0x0600066E RID: 1646 RVA: 0x00007EFB File Offset: 0x000060FB
		public string CommandNameGetStatus { get; private set; }

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x00007F04 File Offset: 0x00006104
		// (set) Token: 0x06000670 RID: 1648 RVA: 0x00007F0C File Offset: 0x0000610C
		public string DataTypeStatusRequest { get; private set; }

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x00007F15 File Offset: 0x00006115
		// (set) Token: 0x06000672 RID: 1650 RVA: 0x00007F1D File Offset: 0x0000611D
		public string DataTypeStatusResponse { get; private set; }

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x00007F26 File Offset: 0x00006126
		// (set) Token: 0x06000674 RID: 1652 RVA: 0x00007F2E File Offset: 0x0000612E
		public string StatusNormal { get; private set; }

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x00007F37 File Offset: 0x00006137
		// (set) Token: 0x06000676 RID: 1654 RVA: 0x00007F3F File Offset: 0x0000613F
		public string StatusMaintenanceMode { get; private set; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x00007F48 File Offset: 0x00006148
		// (set) Token: 0x06000678 RID: 1656 RVA: 0x00007F50 File Offset: 0x00006150
		public string CommandNameGetPendingUpdates { get; private set; }

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x00007F59 File Offset: 0x00006159
		// (set) Token: 0x0600067A RID: 1658 RVA: 0x00007F61 File Offset: 0x00006161
		public string DataTypePendingUpdateRequest { get; private set; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x00007F6A File Offset: 0x0000616A
		// (set) Token: 0x0600067C RID: 1660 RVA: 0x00007F72 File Offset: 0x00006172
		public string DataTypePendingUpdateResponse { get; private set; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x00007F7B File Offset: 0x0000617B
		// (set) Token: 0x0600067E RID: 1662 RVA: 0x00007F83 File Offset: 0x00006183
		public string CommandNameInstallPendingUpdates { get; private set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x00007F8C File Offset: 0x0000618C
		// (set) Token: 0x06000680 RID: 1664 RVA: 0x00007F94 File Offset: 0x00006194
		public string DataTypeInstallPendingRequest { get; private set; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x00007F9D File Offset: 0x0000619D
		// (set) Token: 0x06000682 RID: 1666 RVA: 0x00007FA5 File Offset: 0x000061A5
		public string DataTypeInstallPendingResponse { get; private set; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x00007FAE File Offset: 0x000061AE
		// (set) Token: 0x06000684 RID: 1668 RVA: 0x00007FB6 File Offset: 0x000061B6
		public string CommandNameRestart { get; private set; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x00007FBF File Offset: 0x000061BF
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x00007FC7 File Offset: 0x000061C7
		public string CommandNameIsEntitled { get; private set; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x00007FD0 File Offset: 0x000061D0
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x00007FD8 File Offset: 0x000061D8
		public string DataTypeEntitledResponse { get; private set; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x00007FE1 File Offset: 0x000061E1
		// (set) Token: 0x0600068A RID: 1674 RVA: 0x00007FE9 File Offset: 0x000061E9
		public string CommandNameGetEntitledApps { get; private set; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x00007FF2 File Offset: 0x000061F2
		// (set) Token: 0x0600068C RID: 1676 RVA: 0x00007FFA File Offset: 0x000061FA
		public string DataTypeEntitledAppsResponse { get; private set; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x00008003 File Offset: 0x00006203
		// (set) Token: 0x0600068E RID: 1678 RVA: 0x0000800B File Offset: 0x0000620B
		public string CommandNameInstallEntitledApps { get; private set; }

		// Token: 0x0400033C RID: 828
		private static ContractConstants _contractConstants;
	}
}
