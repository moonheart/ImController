using System;
using System.Threading.Tasks;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x02000010 RID: 16
	public interface IServiceEventHandler
	{
		// Token: 0x0600003C RID: 60
		Task<bool> HandleInitializeAsync();

		// Token: 0x0600003D RID: 61
		Task<bool> HandleSuspendAsync(EventHandlerReason reason);

		// Token: 0x0600003E RID: 62
		Task<bool> HandleResumeAsync(EventHandlerReason reason);

		// Token: 0x0600003F RID: 63
		Task<bool> HandleUninitializeAsync();
	}
}
