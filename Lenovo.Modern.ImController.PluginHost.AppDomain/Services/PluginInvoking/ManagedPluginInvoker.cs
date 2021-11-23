using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Lenovo.Modern.ImController.PluginHost.AppDomain.Services.PluginInvoking
{
	// Token: 0x02000006 RID: 6
	public class ManagedPluginInvoker : IPluginInvoker
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002368 File Offset: 0x00000568
		public ManagedPluginInvoker(Assembly assembly)
		{
			this._assembly = assembly;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002378 File Offset: 0x00000578
		public string InvokeAppRequest(string taskId, string requestXml, Func<string, bool> updateReceiver, WaitHandle cancelEvent)
		{
			Assembly assembly = this._assembly;
			object obj;
			if (assembly == null)
			{
				obj = null;
			}
			else
			{
				Type[] types = assembly.GetTypes();
				if (types == null)
				{
					obj = null;
				}
				else
				{
					obj = types.FirstOrDefault((Type t) => t.IsPublic && t.IsClass && t.Name == "PluginEntry");
				}
			}
			object obj2 = obj;
			if (obj2 == null)
			{
				throw new Exception("Unable to find PluginEntry point");
			}
			object target = Activator.CreateInstance(obj2);
			object obj3 = obj2.InvokeMember(Constants.IPlugin.HandleAppRequest, BindingFlags.InvokeMethod, null, target, new object[] { requestXml, updateReceiver, cancelEvent });
			string text = Convert.ToString(obj3);
			if (obj3 == null || string.IsNullOrEmpty(text))
			{
				text = null;
			}
			return text;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002418 File Offset: 0x00000618
		public string InvokeEventRequest(string eventXml)
		{
			Assembly assembly = this._assembly;
			object obj;
			if (assembly == null)
			{
				obj = null;
			}
			else
			{
				Type[] types = assembly.GetTypes();
				if (types == null)
				{
					obj = null;
				}
				else
				{
					obj = types.FirstOrDefault((Type t) => t.IsPublic && t.IsClass && t.Name == "PluginEntry");
				}
			}
			object obj2 = obj;
			if (obj2 == null)
			{
				throw new Exception("Unable to find PluginEntry point");
			}
			object target = Activator.CreateInstance(obj2);
			object obj3 = obj2.InvokeMember(Constants.IPlugin.HandleEvent, BindingFlags.InvokeMethod, null, target, new object[] { eventXml });
			string text = Convert.ToString(obj3);
			if (obj3 == null || string.IsNullOrEmpty(text))
			{
				text = null;
			}
			return text;
		}

		// Token: 0x04000005 RID: 5
		private Assembly _assembly;
	}
}
