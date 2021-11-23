using System;
using System.Threading;

namespace Lenovo.Modern.ImController.PluginHost.AppDomain.Services
{
	// Token: 0x02000005 RID: 5
	public interface IPluginInvoker
	{
		// Token: 0x06000010 RID: 16
		string InvokeAppRequest(string taskId, string requestXml, Func<string, bool> updateReceiver, WaitHandle cancelEvent);

		// Token: 0x06000011 RID: 17
		string InvokeEventRequest(string eventXml);
	}
}
