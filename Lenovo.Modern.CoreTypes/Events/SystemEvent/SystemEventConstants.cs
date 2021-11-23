using System;

namespace Lenovo.Modern.CoreTypes.Events.SystemEvent
{
	// Token: 0x0200002E RID: 46
	public sealed class SystemEventConstants
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00004BE4 File Offset: 0x00002DE4
		public static SystemEventConstants Get
		{
			get
			{
				SystemEventConstants result;
				if ((result = SystemEventConstants._eventConstants) == null)
				{
					SystemEventConstants systemEventConstants = new SystemEventConstants();
					systemEventConstants.SystemEventMonitorName = "SystemEventMonitor";
					systemEventConstants.SystemEventTriggerName = "SystemEventChange";
					systemEventConstants.SystemEventVersion = "1.0.0.0";
					systemEventConstants.SystemEventDataType = "SystemEvent";
					systemEventConstants.UserLoginTriggerName = "User-Login";
					result = systemEventConstants;
					SystemEventConstants._eventConstants = systemEventConstants;
				}
				return result;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00004C3C File Offset: 0x00002E3C
		// (set) Token: 0x06000202 RID: 514 RVA: 0x00004C44 File Offset: 0x00002E44
		public string SystemEventMonitorName { get; private set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00004C4D File Offset: 0x00002E4D
		// (set) Token: 0x06000204 RID: 516 RVA: 0x00004C55 File Offset: 0x00002E55
		public string SystemEventTriggerName { get; private set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00004C5E File Offset: 0x00002E5E
		// (set) Token: 0x06000206 RID: 518 RVA: 0x00004C66 File Offset: 0x00002E66
		public string SystemEventVersion { get; private set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00004C6F File Offset: 0x00002E6F
		// (set) Token: 0x06000208 RID: 520 RVA: 0x00004C77 File Offset: 0x00002E77
		public string SystemEventDataType { get; private set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00004C80 File Offset: 0x00002E80
		// (set) Token: 0x0600020A RID: 522 RVA: 0x00004C88 File Offset: 0x00002E88
		public string UserLoginTriggerName { get; private set; }

		// Token: 0x040000D7 RID: 215
		private static SystemEventConstants _eventConstants;
	}
}
