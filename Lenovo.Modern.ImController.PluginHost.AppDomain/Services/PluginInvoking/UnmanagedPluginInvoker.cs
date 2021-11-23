using System;
using System.Reflection;
using System.Threading;
using Lenovo.Modern.ImController.PluginHost.UnmanagedPluginShim;

namespace Lenovo.Modern.ImController.PluginHost.AppDomain.Services.PluginInvoking
{
	// Token: 0x02000007 RID: 7
	public class UnmanagedPluginInvoker : IPluginInvoker
	{
		// Token: 0x06000015 RID: 21 RVA: 0x000024AC File Offset: 0x000006AC
		public UnmanagedPluginInvoker(Assembly assembly, string pluginLocation)
		{
			this._assembly = assembly;
			this._pluginLocation = pluginLocation;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000024D0 File Offset: 0x000006D0
		public string InvokeAppRequest(string taskId, string requestXml, Func<string, bool> updateReceiver, WaitHandle cancelEvent)
		{
			Type type = this._assembly.GetType("Lenovo.Modern.ImController.PluginHost.UnmanagedPluginShim.PluginEntry");
			string result = "";
			if (null != type)
			{
				object target = Activator.CreateInstance(type);
				result = Convert.ToString(type.InvokeMember(Constants.IPlugin.HandleAppRequest, BindingFlags.InvokeMethod, null, target, new object[] { this._pluginLocation, requestXml, updateReceiver, cancelEvent }));
			}
			return result;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000253C File Offset: 0x0000073C
		public string InvokeEventRequest(string eventXml)
		{
			Type type = this._assembly.GetType("Lenovo.Modern.ImController.PluginHost.UnmanagedPluginShim.PluginEntry");
			string result = "";
			if (null != type)
			{
				object target = Activator.CreateInstance(type);
				result = Convert.ToString(type.InvokeMember(Constants.IPlugin.HandleEvent, BindingFlags.InvokeMethod, null, target, new object[] { this._pluginLocation, eventXml }));
			}
			return result;
		}

		// Token: 0x04000006 RID: 6
		private Assembly _assembly;

		// Token: 0x04000007 RID: 7
		private string _pluginLocation;

		// Token: 0x04000008 RID: 8
		private UnmanagedPluginInvoker unmanagedPluginInvoker = new UnmanagedPluginInvoker();
	}
}
