using System;

namespace Lenovo.Modern.ImController.PluginHost.Services
{
	// Token: 0x02000007 RID: 7
	public interface IIpcResponder
	{
		// Token: 0x0600001A RID: 26
		void BeginWaitingForRequests(string pipeName);

		// Token: 0x0600001B RID: 27
		void Close();
	}
}
