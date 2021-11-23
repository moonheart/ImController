using System;
using System.Globalization;
using Lenovo.Modern.CoreTypes.Events.UsageMonitor;
using Lenovo.Modern.ImController.Shared.Services;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.UsageMonitor.AppUsageMonitor
{
	// Token: 0x02000022 RID: 34
	internal sealed class AppUsageRecord
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00006D39 File Offset: 0x00004F39
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00006D41 File Offset: 0x00004F41
		public string AppName { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00006D4A File Offset: 0x00004F4A
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00006D52 File Offset: 0x00004F52
		public string PID { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00006D5B File Offset: 0x00004F5B
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00006D63 File Offset: 0x00004F63
		public string ActionType { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00006D6C File Offset: 0x00004F6C
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00006D74 File Offset: 0x00004F74
		public string DateTime { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00006D7D File Offset: 0x00004F7D
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00006D85 File Offset: 0x00004F85
		public string YogaMode { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00006D8E File Offset: 0x00004F8E
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00006D96 File Offset: 0x00004F96
		public string Sid { get; set; }

		// Token: 0x060000CA RID: 202 RVA: 0x00006DA0 File Offset: 0x00004FA0
		public AppUsageRecord(string appName, string pid, bool isStarted, string sid)
		{
			this.AppName = appName;
			this.PID = pid;
			this.ActionType = (isStarted ? UsageMonitorEventConstants.Get.ActionTypeProcessStart : UsageMonitorEventConstants.Get.ActionTypeProcessStop);
			this.DateTime = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
			this.Sid = sid;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00006E08 File Offset: 0x00005008
		public AppUsageRecord(string appName, string pid, EventHandlerReason reason, string sid)
		{
			this.AppName = appName;
			this.PID = pid;
			switch (reason)
			{
			case EventHandlerReason.SystemStart:
				this.ActionType = UsageMonitorEventConstants.Get.ActionTypeServiceStart;
				break;
			case EventHandlerReason.SystemSuspend:
				this.ActionType = UsageMonitorEventConstants.Get.ActionTypeSystemSuspend;
				break;
			case EventHandlerReason.SystemResume:
				this.ActionType = UsageMonitorEventConstants.Get.ActionTypeSystemResume;
				break;
			case EventHandlerReason.UserLogon:
				this.ActionType = UsageMonitorEventConstants.Get.ActionTypeUserLogon;
				break;
			case EventHandlerReason.UserLogoff:
				this.ActionType = UsageMonitorEventConstants.Get.ActionTypeUserLogoff;
				break;
			case EventHandlerReason.UserSwitch:
				this.ActionType = UsageMonitorEventConstants.Get.ActionTypeUserswitch;
				break;
			case EventHandlerReason.SystemShutdown:
				this.ActionType = UsageMonitorEventConstants.Get.ActionTypeServiceStop;
				break;
			case EventHandlerReason.SessionLock:
				this.ActionType = UsageMonitorEventConstants.Get.ActionTypeSessionLock;
				break;
			case EventHandlerReason.SessionUnlock:
				this.ActionType = UsageMonitorEventConstants.Get.ActionTypeSessionUnlock;
				break;
			}
			this.DateTime = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
			this.Sid = sid;
		}
	}
}
