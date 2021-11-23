using System;

namespace Lenovo.Modern.CoreTypes.Contracts.AppLauncher
{
	// Token: 0x020000BF RID: 191
	public sealed class ContractConstants
	{
		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x00008338 File Offset: 0x00006538
		public static ContractConstants Get
		{
			get
			{
				ContractConstants result;
				if ((result = ContractConstants._contractConstants) == null)
				{
					ContractConstants contractConstants = new ContractConstants();
					contractConstants.ContractName = "SystemUtilities.AppLauncher";
					contractConstants.ContractVersion = "1.0.0.0";
					contractConstants.CommandNameLaunchDesktopApp = "Launch-DesktopApp";
					contractConstants.CommandNameLaunchUniversalApp = "Launch-UniversalApp";
					contractConstants.CommandNameLaunchControlPanelApp = "Launch-ControlPanelItem";
					contractConstants.CommandNameLaunchDocument = "Launch-Document";
					contractConstants.DataTypeDesktopAppLaunchRequest = "DesktopAppLaunchRequest";
					contractConstants.DataTypeUniversalAppLaunchRequest = "UniversalAppLaunchRequest";
					contractConstants.DataTypeDocumentLaunchRequest = "DocumentLaunchRequest";
					contractConstants.DataTypeAppLauncherResponse = "AppLauncherResponse";
					contractConstants.ControlPanelItemRequest = "ControlPanelItemRequest";
					result = contractConstants;
					ContractConstants._contractConstants = contractConstants;
				}
				return result;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x000083D5 File Offset: 0x000065D5
		// (set) Token: 0x060006F4 RID: 1780 RVA: 0x000083DD File Offset: 0x000065DD
		public string ContractName { get; private set; }

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x000083E6 File Offset: 0x000065E6
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x000083EE File Offset: 0x000065EE
		public string ContractVersion { get; private set; }

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x000083F7 File Offset: 0x000065F7
		// (set) Token: 0x060006F8 RID: 1784 RVA: 0x000083FF File Offset: 0x000065FF
		public string CommandNameLaunchDesktopApp { get; private set; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00008408 File Offset: 0x00006608
		// (set) Token: 0x060006FA RID: 1786 RVA: 0x00008410 File Offset: 0x00006610
		public string CommandNameLaunchUniversalApp { get; private set; }

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x00008419 File Offset: 0x00006619
		// (set) Token: 0x060006FC RID: 1788 RVA: 0x00008421 File Offset: 0x00006621
		public string CommandNameLaunchControlPanelApp { get; private set; }

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x0000842A File Offset: 0x0000662A
		// (set) Token: 0x060006FE RID: 1790 RVA: 0x00008432 File Offset: 0x00006632
		public string CommandNameLaunchDocument { get; private set; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x0000843B File Offset: 0x0000663B
		// (set) Token: 0x06000700 RID: 1792 RVA: 0x00008443 File Offset: 0x00006643
		public string DataTypeDesktopAppLaunchRequest { get; private set; }

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0000844C File Offset: 0x0000664C
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x00008454 File Offset: 0x00006654
		public string DataTypeUniversalAppLaunchRequest { get; private set; }

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x0000845D File Offset: 0x0000665D
		// (set) Token: 0x06000704 RID: 1796 RVA: 0x00008465 File Offset: 0x00006665
		public string DataTypeDocumentLaunchRequest { get; private set; }

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0000846E File Offset: 0x0000666E
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x00008476 File Offset: 0x00006676
		public string ControlPanelItemRequest { get; private set; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0000847F File Offset: 0x0000667F
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x00008487 File Offset: 0x00006687
		public string DataTypeAppLauncherResponse { get; private set; }

		// Token: 0x04000378 RID: 888
		private static ContractConstants _contractConstants;
	}
}
