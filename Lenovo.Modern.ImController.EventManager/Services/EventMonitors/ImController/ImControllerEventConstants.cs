using System;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.ImController
{
	// Token: 0x0200001B RID: 27
	internal static class ImControllerEventConstants
	{
		// Token: 0x04000062 RID: 98
		public static readonly string Version = "1.0.0.0";

		// Token: 0x04000063 RID: 99
		public static readonly string MonitorName = "ImControllerServiceEventMonitor";

		// Token: 0x04000064 RID: 100
		public static readonly string ImControllerEventDataType = "ImControllerServiceEvent";

		// Token: 0x02000038 RID: 56
		public static class Triggers
		{
			// Token: 0x040000F9 RID: 249
			public const string ServiceStartup = "imc-startup";
		}
	}
}
