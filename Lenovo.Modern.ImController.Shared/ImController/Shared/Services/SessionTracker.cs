using System;
using Lenovo.Modern.ImController.Shared.Telemetry;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x0200001A RID: 26
	public class SessionTracker
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00004D47 File Offset: 0x00002F47
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00004D4F File Offset: 0x00002F4F
		private DateTime? ProcessStartTime { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00004D58 File Offset: 0x00002F58
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00004D60 File Offset: 0x00002F60
		private DateTime? SessionStartTime { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00004D69 File Offset: 0x00002F69
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00004D71 File Offset: 0x00002F71
		private Guid SessionId { get; set; }

		// Token: 0x0600007F RID: 127 RVA: 0x00002050 File Offset: 0x00000250
		private SessionTracker()
		{
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004D7A File Offset: 0x00002F7A
		public static SessionTracker GetInstance()
		{
			SessionTracker result;
			if ((result = SessionTracker._instance) == null)
			{
				result = (SessionTracker._instance = new SessionTracker());
			}
			return result;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004D90 File Offset: 0x00002F90
		public void StartProcess()
		{
			this.ProcessStartTime = new DateTime?(DateTime.Now);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003010 File Offset: 0x00001210
		public void StopProcess()
		{
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004DA2 File Offset: 0x00002FA2
		public void StartSession()
		{
			this.SessionStartTime = new DateTime?(DateTime.Now);
			this.SessionId = Guid.NewGuid();
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004DC0 File Offset: 0x00002FC0
		public void StopSession()
		{
			this.SessionStartTime = null;
			this.SessionId = Guid.Empty;
			EventLogger.GetInstance().CleanupData();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004DF4 File Offset: 0x00002FF4
		public int GetMinutesSinceProcessStart()
		{
			TimeSpan? timeSpan = DateTime.Now - this.ProcessStartTime;
			if (timeSpan == null)
			{
				return 0;
			}
			return Convert.ToInt32(Math.Round(timeSpan.Value.TotalMinutes));
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004E5C File Offset: 0x0000305C
		public int GetMinutesSinceSessionStart()
		{
			TimeSpan? timeSpan = DateTime.Now - this.SessionStartTime;
			if (timeSpan == null)
			{
				return 0;
			}
			return Convert.ToInt32(Math.Round(timeSpan.Value.TotalMinutes));
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004EC2 File Offset: 0x000030C2
		public Guid GetSessionId()
		{
			return this.SessionId;
		}

		// Token: 0x04000066 RID: 102
		private static SessionTracker _instance;
	}
}
