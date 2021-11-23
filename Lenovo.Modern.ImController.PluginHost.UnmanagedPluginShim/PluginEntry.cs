using System;
using System.Threading;

namespace Lenovo.Modern.ImController.PluginHost.UnmanagedPluginShim
{
	// Token: 0x02000002 RID: 2
	public class PluginEntry : MarshalByRefObject
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public PluginEntry()
		{
			this.unmanagedPluginInvoker = new UnmanagedPluginInvoker();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000205B File Offset: 0x0000025B
		public string HandleEvent(string pluginPath, string eventXml)
		{
			return this.unmanagedPluginInvoker.HandleEvent(pluginPath, eventXml);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000206A File Offset: 0x0000026A
		public string HandleAppRequest(string pluginPath, string contractRequestXml, Func<string, bool> responseFunction, WaitHandle cancelEvent)
		{
			return this.unmanagedPluginInvoker.HandleAppRequest(pluginPath, contractRequestXml, responseFunction, cancelEvent);
		}

		// Token: 0x04000001 RID: 1
		private UnmanagedPluginInvoker unmanagedPluginInvoker;
	}
}
