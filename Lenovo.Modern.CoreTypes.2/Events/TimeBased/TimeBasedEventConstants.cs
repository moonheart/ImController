using System;

namespace Lenovo.Modern.CoreTypes.Events.TimeBased
{
	// Token: 0x0200002B RID: 43
	public sealed class TimeBasedEventConstants
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060001BC RID: 444 RVA: 0x000048EC File Offset: 0x00002AEC
		public static TimeBasedEventConstants Get
		{
			get
			{
				TimeBasedEventConstants result;
				if ((result = TimeBasedEventConstants._eventConstants) == null)
				{
					TimeBasedEventConstants timeBasedEventConstants = new TimeBasedEventConstants();
					timeBasedEventConstants.TimeBasedEventMonitorName = "TimeBasedMonitor";
					timeBasedEventConstants.TimeBasedEventTriggerName = "TimeBaseChange";
					timeBasedEventConstants.TimeBasedEventVersion = "1.0.0.2";
					timeBasedEventConstants.TimeBasedEventDataType = "TimeBasedEvent";
					result = timeBasedEventConstants;
					TimeBasedEventConstants._eventConstants = timeBasedEventConstants;
				}
				return result;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00004939 File Offset: 0x00002B39
		// (set) Token: 0x060001BE RID: 446 RVA: 0x00004941 File Offset: 0x00002B41
		public string TimeBasedEventMonitorName { get; private set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000494A File Offset: 0x00002B4A
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x00004952 File Offset: 0x00002B52
		public string TimeBasedEventTriggerName { get; private set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000495B File Offset: 0x00002B5B
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x00004963 File Offset: 0x00002B63
		public string TimeBasedEventVersion { get; private set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000496C File Offset: 0x00002B6C
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00004974 File Offset: 0x00002B74
		public string TimeBasedEventDataType { get; private set; }

		// Token: 0x040000B8 RID: 184
		private static TimeBasedEventConstants _eventConstants;
	}
}
