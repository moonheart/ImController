using System;
using Lenovo.ImController.EventLogging.Model;

namespace Lenovo.ImController.EventLogging.Services.Repositories
{
	// Token: 0x0200000C RID: 12
	internal interface IWriteableEventRepository : IEventRepository
	{
		// Token: 0x0600001B RID: 27
		void LogEvent(StorableEvent userEvent);
	}
}
