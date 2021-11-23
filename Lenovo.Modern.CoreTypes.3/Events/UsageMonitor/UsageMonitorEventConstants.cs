using System;

namespace Lenovo.Modern.CoreTypes.Events.UsageMonitor
{
	// Token: 0x02000028 RID: 40
	public sealed class UsageMonitorEventConstants
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00004740 File Offset: 0x00002940
		public static UsageMonitorEventConstants Get
		{
			get
			{
				UsageMonitorEventConstants result;
				if ((result = UsageMonitorEventConstants._eventConstants) == null)
				{
					UsageMonitorEventConstants usageMonitorEventConstants = new UsageMonitorEventConstants();
					usageMonitorEventConstants.UsageMonitorEventMonitorName = "UsageMonitor";
					usageMonitorEventConstants.UsageMonitorAppTrigger = "App";
					usageMonitorEventConstants.UsageMonitorEventVersion = "1.0.0.0";
					usageMonitorEventConstants.ActionTypeProcessStart = "0";
					usageMonitorEventConstants.ActionTypeProcessStop = "1";
					usageMonitorEventConstants.ActionTypeServiceStart = "2";
					usageMonitorEventConstants.ActionTypeServiceStop = "3";
					usageMonitorEventConstants.ActionTypeSystemResume = "4";
					usageMonitorEventConstants.ActionTypeSystemSuspend = "5";
					usageMonitorEventConstants.ActionTypeUserLogon = "6";
					usageMonitorEventConstants.ActionTypeUserLogoff = "7";
					usageMonitorEventConstants.ActionTypeUserswitch = "8";
					usageMonitorEventConstants.ActionTypeSessionLock = "9";
					usageMonitorEventConstants.ActionTypeSessionUnlock = "10";
					result = usageMonitorEventConstants;
					UsageMonitorEventConstants._eventConstants = usageMonitorEventConstants;
				}
				return result;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000047FE File Offset: 0x000029FE
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x00004806 File Offset: 0x00002A06
		public string UsageMonitorEventMonitorName { get; private set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000480F File Offset: 0x00002A0F
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x00004817 File Offset: 0x00002A17
		public string UsageMonitorAppTrigger { get; private set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00004820 File Offset: 0x00002A20
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x00004828 File Offset: 0x00002A28
		public string UsageMonitorEventVersion { get; private set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00004831 File Offset: 0x00002A31
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00004839 File Offset: 0x00002A39
		public string ActionTypeProcessStart { get; private set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00004842 File Offset: 0x00002A42
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x0000484A File Offset: 0x00002A4A
		public string ActionTypeProcessStop { get; private set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00004853 File Offset: 0x00002A53
		// (set) Token: 0x060001AA RID: 426 RVA: 0x0000485B File Offset: 0x00002A5B
		public string ActionTypeServiceStart { get; private set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00004864 File Offset: 0x00002A64
		// (set) Token: 0x060001AC RID: 428 RVA: 0x0000486C File Offset: 0x00002A6C
		public string ActionTypeServiceStop { get; private set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00004875 File Offset: 0x00002A75
		// (set) Token: 0x060001AE RID: 430 RVA: 0x0000487D File Offset: 0x00002A7D
		public string ActionTypeSystemResume { get; private set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00004886 File Offset: 0x00002A86
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x0000488E File Offset: 0x00002A8E
		public string ActionTypeSystemSuspend { get; private set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00004897 File Offset: 0x00002A97
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x0000489F File Offset: 0x00002A9F
		public string ActionTypeUserLogon { get; private set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x000048A8 File Offset: 0x00002AA8
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x000048B0 File Offset: 0x00002AB0
		public string ActionTypeUserLogoff { get; private set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x000048B9 File Offset: 0x00002AB9
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x000048C1 File Offset: 0x00002AC1
		public string ActionTypeUserswitch { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x000048CA File Offset: 0x00002ACA
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x000048D2 File Offset: 0x00002AD2
		public string ActionTypeSessionLock { get; private set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x000048DB File Offset: 0x00002ADB
		// (set) Token: 0x060001BA RID: 442 RVA: 0x000048E3 File Offset: 0x00002AE3
		public string ActionTypeSessionUnlock { get; private set; }

		// Token: 0x0400009B RID: 155
		private static UsageMonitorEventConstants _eventConstants;
	}
}
