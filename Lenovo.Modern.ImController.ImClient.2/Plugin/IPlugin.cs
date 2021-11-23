using System;
using System.Threading;

namespace Lenovo.Modern.ImController.ImClient.Plugin
{
	// Token: 0x02000009 RID: 9
	public interface IPlugin
	{
		// Token: 0x06000024 RID: 36
		string HandleAppRequest(string contractRequestXml, Func<string, bool> responseFunction, WaitHandle cancelEvent);

		// Token: 0x06000025 RID: 37
		string HandleEvent(string eventXml);
	}
}
