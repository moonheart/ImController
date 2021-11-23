using System;

namespace Lenovo.Modern.CoreTypes.Events.WindowMessage
{
	// Token: 0x02000027 RID: 39
	public sealed class WindowMessageEventConstants
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00004674 File Offset: 0x00002874
		public static WindowMessageEventConstants Get
		{
			get
			{
				WindowMessageEventConstants result;
				if ((result = WindowMessageEventConstants._eventConstants) == null)
				{
					WindowMessageEventConstants windowMessageEventConstants = new WindowMessageEventConstants();
					windowMessageEventConstants.WindowMessageEventMonitorName = "WindowMessageMonitor";
					windowMessageEventConstants.WindowMessagePowerTriggerName = "PowerChange";
					windowMessageEventConstants.WindowMessageSessionTriggerName = "SessionChange";
					windowMessageEventConstants.WindowMessageDeviceTriggerName = "DeviceChange";
					windowMessageEventConstants.WindowMessageEventVersion = "1.0.0.0";
					windowMessageEventConstants.WindowMessageEventDataType = "WindowMessageEvent";
					result = windowMessageEventConstants;
					WindowMessageEventConstants._eventConstants = windowMessageEventConstants;
				}
				return result;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000191 RID: 401 RVA: 0x000046D7 File Offset: 0x000028D7
		// (set) Token: 0x06000192 RID: 402 RVA: 0x000046DF File Offset: 0x000028DF
		public string WindowMessageEventMonitorName { get; private set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000193 RID: 403 RVA: 0x000046E8 File Offset: 0x000028E8
		// (set) Token: 0x06000194 RID: 404 RVA: 0x000046F0 File Offset: 0x000028F0
		public string WindowMessagePowerTriggerName { get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000195 RID: 405 RVA: 0x000046F9 File Offset: 0x000028F9
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00004701 File Offset: 0x00002901
		public string WindowMessageSessionTriggerName { get; private set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000470A File Offset: 0x0000290A
		// (set) Token: 0x06000198 RID: 408 RVA: 0x00004712 File Offset: 0x00002912
		public string WindowMessageDeviceTriggerName { get; private set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000471B File Offset: 0x0000291B
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00004723 File Offset: 0x00002923
		public string WindowMessageEventVersion { get; private set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600019B RID: 411 RVA: 0x0000472C File Offset: 0x0000292C
		// (set) Token: 0x0600019C RID: 412 RVA: 0x00004734 File Offset: 0x00002934
		public string WindowMessageEventDataType { get; private set; }

		// Token: 0x04000094 RID: 148
		private static WindowMessageEventConstants _eventConstants;
	}
}
