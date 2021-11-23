using System;

namespace Lenovo.Modern.CoreTypes.Events.Registry
{
	// Token: 0x02000031 RID: 49
	public sealed class RegistryEventConstants
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00004CD8 File Offset: 0x00002ED8
		public static RegistryEventConstants Get
		{
			get
			{
				RegistryEventConstants result;
				if ((result = RegistryEventConstants._eventConstants) == null)
				{
					RegistryEventConstants registryEventConstants = new RegistryEventConstants();
					registryEventConstants.RegistryEventMonitorName = "RegistryMonitor";
					registryEventConstants.RegistryEventTriggerName = "RegistryChange";
					registryEventConstants.RegistryEventVersion = "1.0.0.0";
					registryEventConstants.RegistryEventDataType = "RegistryEvent";
					result = registryEventConstants;
					RegistryEventConstants._eventConstants = registryEventConstants;
				}
				return result;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00004D25 File Offset: 0x00002F25
		// (set) Token: 0x06000218 RID: 536 RVA: 0x00004D2D File Offset: 0x00002F2D
		public string RegistryEventMonitorName { get; private set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00004D36 File Offset: 0x00002F36
		// (set) Token: 0x0600021A RID: 538 RVA: 0x00004D3E File Offset: 0x00002F3E
		public string RegistryEventTriggerName { get; private set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00004D47 File Offset: 0x00002F47
		// (set) Token: 0x0600021C RID: 540 RVA: 0x00004D4F File Offset: 0x00002F4F
		public string RegistryEventVersion { get; private set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00004D58 File Offset: 0x00002F58
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00004D60 File Offset: 0x00002F60
		public string RegistryEventDataType { get; private set; }

		// Token: 0x040000E1 RID: 225
		private static RegistryEventConstants _eventConstants;
	}
}
