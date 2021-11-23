using System;

namespace Lenovo.Modern.CoreTypes.Events.AppMonitor
{
	// Token: 0x0200003D RID: 61
	public sealed class AppMonitorEventConstants
	{
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000289 RID: 649 RVA: 0x000051F4 File Offset: 0x000033F4
		public static AppMonitorEventConstants Get
		{
			get
			{
				AppMonitorEventConstants result;
				if ((result = AppMonitorEventConstants._eventConstants) == null)
				{
					AppMonitorEventConstants appMonitorEventConstants = new AppMonitorEventConstants();
					appMonitorEventConstants.AppMonitorEventMonitorName = "AppMonitor";
					appMonitorEventConstants.AppMonitorAppOpenedEventTrigger = "AppOpened";
					appMonitorEventConstants.AppMonitorInstallUninstallTrigger = "InstallUninstall";
					appMonitorEventConstants.AppMonitorEventVersion = "1.0.0.0";
					appMonitorEventConstants.AppMonitorEventDataType = "AppMonitor";
					appMonitorEventConstants.AppMonitorEventOpen = "Open";
					appMonitorEventConstants.AppMonitorEventInstall = "Install";
					appMonitorEventConstants.AppMonitorEventUninstall = "Uninstall";
					result = appMonitorEventConstants;
					AppMonitorEventConstants._eventConstants = appMonitorEventConstants;
				}
				return result;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000526D File Offset: 0x0000346D
		// (set) Token: 0x0600028B RID: 651 RVA: 0x00005275 File Offset: 0x00003475
		public string AppMonitorEventMonitorName { get; private set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000527E File Offset: 0x0000347E
		// (set) Token: 0x0600028D RID: 653 RVA: 0x00005286 File Offset: 0x00003486
		public string AppMonitorAppOpenedEventTrigger { get; private set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600028E RID: 654 RVA: 0x0000528F File Offset: 0x0000348F
		// (set) Token: 0x0600028F RID: 655 RVA: 0x00005297 File Offset: 0x00003497
		public string AppMonitorInstallUninstallTrigger { get; private set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000290 RID: 656 RVA: 0x000052A0 File Offset: 0x000034A0
		// (set) Token: 0x06000291 RID: 657 RVA: 0x000052A8 File Offset: 0x000034A8
		public string AppMonitorEventVersion { get; private set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000292 RID: 658 RVA: 0x000052B1 File Offset: 0x000034B1
		// (set) Token: 0x06000293 RID: 659 RVA: 0x000052B9 File Offset: 0x000034B9
		public string AppMonitorEventDataType { get; private set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000294 RID: 660 RVA: 0x000052C2 File Offset: 0x000034C2
		// (set) Token: 0x06000295 RID: 661 RVA: 0x000052CA File Offset: 0x000034CA
		public string AppMonitorEventInstall { get; private set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000296 RID: 662 RVA: 0x000052D3 File Offset: 0x000034D3
		// (set) Token: 0x06000297 RID: 663 RVA: 0x000052DB File Offset: 0x000034DB
		public string AppMonitorEventUninstall { get; private set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000298 RID: 664 RVA: 0x000052E4 File Offset: 0x000034E4
		// (set) Token: 0x06000299 RID: 665 RVA: 0x000052EC File Offset: 0x000034EC
		public string AppMonitorEventOpen { get; private set; }

		// Token: 0x04000117 RID: 279
		private static AppMonitorEventConstants _eventConstants;
	}
}
