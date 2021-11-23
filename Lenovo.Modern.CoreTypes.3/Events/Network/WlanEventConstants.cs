using System;

namespace Lenovo.Modern.CoreTypes.Events.Network
{
	// Token: 0x02000037 RID: 55
	public class WlanEventConstants
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00004EF8 File Offset: 0x000030F8
		public static WlanEventConstants Get
		{
			get
			{
				WlanEventConstants result;
				if ((result = WlanEventConstants._eventConstants) == null)
				{
					WlanEventConstants wlanEventConstants = new WlanEventConstants();
					wlanEventConstants.WlanEventMonitorName = "WlanMonitor";
					wlanEventConstants.WlanAcmEventTriggerName = "acm-event";
					wlanEventConstants.WlanMsmAssociatedTriggerName = "msm-Associated";
					wlanEventConstants.WlanMsmConnectedTriggerName = "msm-Connected";
					wlanEventConstants.WlanMsmDisconnectedTriggerName = "msm-Disconnected";
					wlanEventConstants.WlanMsmAssociatingTriggerName = "msm-Associating";
					wlanEventConstants.WlanMsmAuthenticatingTriggerName = "msm-Authenticating";
					wlanEventConstants.WlanEventVersion = "1.0.0.0";
					wlanEventConstants.WlanAcmEventDataType = "WlanAcmEventData";
					result = wlanEventConstants;
					WlanEventConstants._eventConstants = wlanEventConstants;
				}
				return result;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600024B RID: 587 RVA: 0x00004F7C File Offset: 0x0000317C
		// (set) Token: 0x0600024C RID: 588 RVA: 0x00004F84 File Offset: 0x00003184
		public string WlanEventMonitorName { get; private set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00004F8D File Offset: 0x0000318D
		// (set) Token: 0x0600024E RID: 590 RVA: 0x00004F95 File Offset: 0x00003195
		public string WlanAcmEventTriggerName { get; private set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00004F9E File Offset: 0x0000319E
		// (set) Token: 0x06000250 RID: 592 RVA: 0x00004FA6 File Offset: 0x000031A6
		public string WlanEventVersion { get; private set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00004FAF File Offset: 0x000031AF
		// (set) Token: 0x06000252 RID: 594 RVA: 0x00004FB7 File Offset: 0x000031B7
		public string WlanAcmEventDataType { get; private set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000253 RID: 595 RVA: 0x00004FC0 File Offset: 0x000031C0
		// (set) Token: 0x06000254 RID: 596 RVA: 0x00004FC8 File Offset: 0x000031C8
		public string WlanMsmAssociatedTriggerName { get; private set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000255 RID: 597 RVA: 0x00004FD1 File Offset: 0x000031D1
		// (set) Token: 0x06000256 RID: 598 RVA: 0x00004FD9 File Offset: 0x000031D9
		public string WlanMsmConnectedTriggerName { get; private set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000257 RID: 599 RVA: 0x00004FE2 File Offset: 0x000031E2
		// (set) Token: 0x06000258 RID: 600 RVA: 0x00004FEA File Offset: 0x000031EA
		public string WlanMsmDisconnectedTriggerName { get; private set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000259 RID: 601 RVA: 0x00004FF3 File Offset: 0x000031F3
		// (set) Token: 0x0600025A RID: 602 RVA: 0x00004FFB File Offset: 0x000031FB
		public string WlanMsmAssociatingTriggerName { get; private set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600025B RID: 603 RVA: 0x00005004 File Offset: 0x00003204
		// (set) Token: 0x0600025C RID: 604 RVA: 0x0000500C File Offset: 0x0000320C
		public string WlanMsmAuthenticatingTriggerName { get; private set; }

		// Token: 0x040000F9 RID: 249
		private static WlanEventConstants _eventConstants;

		// Token: 0x020000DE RID: 222
		public class Win32
		{
			// Token: 0x040003C9 RID: 969
			public const int ERROR_SUCCESS = 0;

			// Token: 0x020000E0 RID: 224
			[Flags]
			public enum WLAN_NOTIFICATION_SOURCE
			{
				// Token: 0x040003CC RID: 972
				None = 0,
				// Token: 0x040003CD RID: 973
				All = 65535,
				// Token: 0x040003CE RID: 974
				ACM = 8,
				// Token: 0x040003CF RID: 975
				MSM = 16,
				// Token: 0x040003D0 RID: 976
				Security = 32,
				// Token: 0x040003D1 RID: 977
				IHV = 64,
				// Token: 0x040003D2 RID: 978
				HNWK = 128
			}
		}
	}
}
