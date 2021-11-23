using System;

namespace Lenovo.Modern.CoreTypes.Events.ImController
{
	// Token: 0x02000039 RID: 57
	public sealed class ImControllerEventConstants
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000506C File Offset: 0x0000326C
		public static ImControllerEventConstants Get
		{
			get
			{
				ImControllerEventConstants result;
				if ((result = ImControllerEventConstants._eventConstants) == null)
				{
					ImControllerEventConstants imControllerEventConstants = new ImControllerEventConstants();
					imControllerEventConstants.ImControllerEventMonitorName = "ImControllerServiceEventMonitor";
					imControllerEventConstants.ImControllerStartEventTriggerName = "imc-startup";
					imControllerEventConstants.ImControllerEventVersion = "1.0.0.0";
					imControllerEventConstants.ImControllerEventDataType = "ImControllerServiceEvent";
					result = imControllerEventConstants;
					ImControllerEventConstants._eventConstants = imControllerEventConstants;
				}
				return result;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600026A RID: 618 RVA: 0x000050B9 File Offset: 0x000032B9
		// (set) Token: 0x0600026B RID: 619 RVA: 0x000050C1 File Offset: 0x000032C1
		public string ImControllerEventMonitorName { get; private set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600026C RID: 620 RVA: 0x000050CA File Offset: 0x000032CA
		// (set) Token: 0x0600026D RID: 621 RVA: 0x000050D2 File Offset: 0x000032D2
		public string ImControllerStartEventTriggerName { get; private set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600026E RID: 622 RVA: 0x000050DB File Offset: 0x000032DB
		// (set) Token: 0x0600026F RID: 623 RVA: 0x000050E3 File Offset: 0x000032E3
		public string ImControllerEventVersion { get; private set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000270 RID: 624 RVA: 0x000050EC File Offset: 0x000032EC
		// (set) Token: 0x06000271 RID: 625 RVA: 0x000050F4 File Offset: 0x000032F4
		public string ImControllerEventDataType { get; private set; }

		// Token: 0x04000108 RID: 264
		private static ImControllerEventConstants _eventConstants;
	}
}
