using System;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.SystemEvent
{
	// Token: 0x02000013 RID: 19
	internal static class SystemEventMonitorConstants
	{
		// Token: 0x04000036 RID: 54
		public static readonly string Version = "1.0.0.0";

		// Token: 0x04000037 RID: 55
		public static readonly string MonitorName = "SystemEventMonitor";

		// Token: 0x04000038 RID: 56
		internal static readonly string DataType = "SystemEvent";

		// Token: 0x04000039 RID: 57
		internal static readonly string Trigger = "SystemEventChange";

		// Token: 0x02000032 RID: 50
		public static class Triggers
		{
			// Token: 0x040000EA RID: 234
			public const string UserLogin = "User-Login";
		}
	}
}
