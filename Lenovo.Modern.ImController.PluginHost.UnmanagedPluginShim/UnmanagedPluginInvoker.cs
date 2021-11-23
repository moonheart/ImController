using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Lenovo.Modern.ImController.PluginHost.UnmanagedPluginShim
{
	// Token: 0x02000003 RID: 3
	public class UnmanagedPluginInvoker : MarshalByRefObject
	{
		// Token: 0x06000004 RID: 4
		[DllImport("kernel32.dll")]
		private static extern IntPtr LoadLibrary(string path);

		// Token: 0x06000005 RID: 5
		[DllImport("kernel32.dll")]
		private static extern bool FreeLibrary(IntPtr hModule);

		// Token: 0x06000006 RID: 6
		[DllImport("kernel32.dll")]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		// Token: 0x06000007 RID: 7 RVA: 0x0000207C File Offset: 0x0000027C
		private bool ReceiveCallback(string responseXml)
		{
			return UnmanagedPluginInvoker._callbackHandler != null && UnmanagedPluginInvoker._callbackHandler(responseXml);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002094 File Offset: 0x00000294
		public string HandleAppRequest(string pluginPath, string requestXml, Func<string, bool> UpdateReceiver, WaitHandle WaitHandleCancelRequest1)
		{
			UnmanagedPluginInvoker._callbackHandler = UpdateReceiver;
			UnmanagedPluginInvoker.InvokeCallbackDelegate callbackDelegate = new UnmanagedPluginInvoker.InvokeCallbackDelegate(this.ReceiveCallback);
			IntPtr hModule = UnmanagedPluginInvoker.LoadLibrary(pluginPath);
			string result = ((UnmanagedPluginInvoker.InvokeAppContractDelegate)Marshal.GetDelegateForFunctionPointer(UnmanagedPluginInvoker.GetProcAddress(hModule, "HandleAppRequest"), typeof(UnmanagedPluginInvoker.InvokeAppContractDelegate)))(requestXml, callbackDelegate, WaitHandleCancelRequest1.SafeWaitHandle.DangerousGetHandle());
			UnmanagedPluginInvoker.FreeLibrary(hModule);
			return result;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000020F4 File Offset: 0x000002F4
		public string HandleEvent(string pluginPath, string eventXml)
		{
			IntPtr hModule = UnmanagedPluginInvoker.LoadLibrary(pluginPath);
			string result = ((UnmanagedPluginInvoker.InvokeEventContractDelegate)Marshal.GetDelegateForFunctionPointer(UnmanagedPluginInvoker.GetProcAddress(hModule, "HandleEvent"), typeof(UnmanagedPluginInvoker.InvokeEventContractDelegate)))(eventXml);
			UnmanagedPluginInvoker.FreeLibrary(hModule);
			return result;
		}

		// Token: 0x04000002 RID: 2
		private static Func<string, bool> _callbackHandler;

		// Token: 0x02000004 RID: 4
		// (Invoke) Token: 0x0600000C RID: 12
		[return: MarshalAs(UnmanagedType.BStr)]
		public delegate string InvokeEventContractDelegate([MarshalAs(UnmanagedType.BStr)] string requestEventXML);

		// Token: 0x02000005 RID: 5
		// (Invoke) Token: 0x06000010 RID: 16
		[return: MarshalAs(UnmanagedType.BStr)]
		public delegate string InvokeAppContractDelegate([MarshalAs(UnmanagedType.BStr)] string requestXML, UnmanagedPluginInvoker.InvokeCallbackDelegate callbackDelegate, IntPtr eventHandle);

		// Token: 0x02000006 RID: 6
		// (Invoke) Token: 0x06000014 RID: 20
		public delegate bool InvokeCallbackDelegate([MarshalAs(UnmanagedType.BStr)] string responseXML);
	}
}
