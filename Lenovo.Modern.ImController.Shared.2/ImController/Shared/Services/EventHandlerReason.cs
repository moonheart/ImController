using System;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x0200000F RID: 15
	public enum EventHandlerReason
	{
		// Token: 0x0400004F RID: 79
		SystemStart,
		// Token: 0x04000050 RID: 80
		SystemSuspend,
		// Token: 0x04000051 RID: 81
		SystemResume,
		// Token: 0x04000052 RID: 82
		UserLogon,
		// Token: 0x04000053 RID: 83
		UserLogoff,
		// Token: 0x04000054 RID: 84
		UserSwitch,
		// Token: 0x04000055 RID: 85
		SystemShutdown,
		// Token: 0x04000056 RID: 86
		SessionLock,
		// Token: 0x04000057 RID: 87
		SessionUnlock
	}
}
