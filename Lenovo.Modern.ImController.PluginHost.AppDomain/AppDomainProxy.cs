using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.PluginHost.AppDomain.Services;
using Lenovo.Modern.ImController.PluginHost.AppDomain.Services.PluginInvoking;
using Lenovo.Modern.ImController.PluginHost.UnmanagedPluginShim;
using Microsoft.Win32.SafeHandles;

namespace Lenovo.Modern.ImController.PluginHost.AppDomain
{
	// Token: 0x02000003 RID: 3
	public class AppDomainProxy : MarshalByRefObject
	{
		// Token: 0x06000005 RID: 5 RVA: 0x0000206D File Offset: 0x0000026D
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000209C File Offset: 0x0000029C
		public void InstantiateAssemblyAndInvoker(string pluginLocation, string pluginTypeText)
		{
			AppDomainProxy.PluginType pluginType = AppDomainProxy.PluginType.ManagedLibrary;
			Enum.TryParse<AppDomainProxy.PluginType>(pluginTypeText, out pluginType);
			try
			{
				if (pluginType != AppDomainProxy.PluginType.ManagedLibrary)
				{
					if (pluginType != AppDomainProxy.PluginType.UnmanagedLibrary)
					{
						throw new AppDomainException("No PluginType (unmanaged or managed) provided to AppDomainProxy");
					}
					Assembly assembly = Assembly.GetAssembly(typeof(PluginEntry));
					string fileName = Path.GetFileName((assembly != null) ? assembly.CodeBase : null);
					Assembly callingAssembly = Assembly.GetCallingAssembly();
					Assembly assembly2 = Assembly.Load(AssemblyName.GetAssemblyName(Path.GetDirectoryName((callingAssembly != null) ? callingAssembly.Location : null) + "\\" + fileName));
					this._pluginInvoker = new Lenovo.Modern.ImController.PluginHost.AppDomain.Services.PluginInvoking.UnmanagedPluginInvoker(assembly2, pluginLocation);
				}
				else
				{
					Assembly assembly3 = Assembly.Load(AssemblyName.GetAssemblyName(pluginLocation));
					this._pluginInvoker = new ManagedPluginInvoker(assembly3);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002154 File Offset: 0x00000354
		public void InvokeAppRequest(string taskId, string requestXml, IntPtr intermediateResponseWaitHandle, IntPtr finalResponseWaitHandle, IntPtr cancelEventPtr)
		{
			this._nextIntermediateResponse[taskId] = new List<string>();
			Task.Factory.StartNew(delegate()
			{
				using (ManualResetEvent manualResetEvent = new ManualResetEvent(false))
				{
					manualResetEvent.SafeWaitHandle = new SafeWaitHandle(cancelEventPtr, false);
					using (Semaphore intermediateEvent = new Semaphore(0, 1048576))
					{
						intermediateEvent.SafeWaitHandle = new SafeWaitHandle(intermediateResponseWaitHandle, false);
						using (ManualResetEvent manualResetEvent2 = new ManualResetEvent(false))
						{
							manualResetEvent2.SafeWaitHandle = new SafeWaitHandle(finalResponseWaitHandle, false);
							try
							{
								string responseXml = this.InvokeAppRequest(taskId, requestXml, delegate(string s)
								{
									this.SetNextIntermdiateREsponse(taskId, intermediateEvent, s);
									return true;
								}, manualResetEvent);
								this.SetFinalResponse(taskId, manualResetEvent2, responseXml);
							}
							catch (Exception ex)
							{
								this.SetFinalResponse(taskId, manualResetEvent2, this.CreateErrorResponse("Plugin crashed with exception: " + ex.GetType().ToString()));
							}
						}
					}
				}
			}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021CB File Offset: 0x000003CB
		private string CreateErrorResponse(string errorText)
		{
			return string.Format("<BrokerResponseTask isComplete=\"true\" percentageComplete=\"0\"><FailureData><ResultCodeGroup>Plugin Host</ResultCodeGroup><ResultCode>605</ResultCode><ResultDescription>Error while running domain: {0}</ResultDescription></FailureData></BrokerResponseTask>", errorText);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021D8 File Offset: 0x000003D8
		public void SetNextIntermdiateREsponse(string taskId, Semaphore e, string responseXml)
		{
			object nirLock = this._nirLock;
			lock (nirLock)
			{
				if (!string.IsNullOrWhiteSpace(responseXml))
				{
					this._nextIntermediateResponse[taskId].Add(responseXml);
					e.Release();
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002234 File Offset: 0x00000434
		public void SetFinalResponse(string taskId, ManualResetEvent e, string responseXml)
		{
			if (string.IsNullOrWhiteSpace(responseXml))
			{
				this._finalResponseXml[taskId] = this.CreateErrorResponse("Empty final response was received from plugin.");
			}
			else
			{
				this._finalResponseXml[taskId] = responseXml;
			}
			try
			{
				e.Set();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000228C File Offset: 0x0000048C
		public string GetNextIntermediateResponse(string taskId)
		{
			object nirLock = this._nirLock;
			string result;
			lock (nirLock)
			{
				string text = this._nextIntermediateResponse[taskId].First<string>();
				this._nextIntermediateResponse[taskId].RemoveAt(0);
				result = text;
			}
			return result;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022EC File Offset: 0x000004EC
		public string GetFinalResponse(string taskId)
		{
			string result = "";
			try
			{
				this._finalResponseXml.TryRemove(taskId, out result);
			}
			catch (Exception)
			{
			}
			try
			{
				List<string> list;
				this._nextIntermediateResponse.TryRemove(taskId, out list);
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002348 File Offset: 0x00000548
		public string InvokeAppRequest(string taskId, string requestXml, Func<string, bool> updateReceiver, WaitHandle cancelEvent)
		{
			return this._pluginInvoker.InvokeAppRequest(taskId, requestXml, updateReceiver, cancelEvent);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000235A File Offset: 0x0000055A
		public string InvokeEventRequest(string eventXml)
		{
			return this._pluginInvoker.InvokeEventRequest(eventXml);
		}

		// Token: 0x04000001 RID: 1
		private IPluginInvoker _pluginInvoker;

		// Token: 0x04000002 RID: 2
		private ConcurrentDictionary<string, List<string>> _nextIntermediateResponse = new ConcurrentDictionary<string, List<string>>();

		// Token: 0x04000003 RID: 3
		private object _nirLock = new object();

		// Token: 0x04000004 RID: 4
		private ConcurrentDictionary<string, string> _finalResponseXml = new ConcurrentDictionary<string, string>();

		// Token: 0x02000008 RID: 8
		[Serializable]
		public enum PluginType
		{
			// Token: 0x0400000A RID: 10
			ManagedLibrary,
			// Token: 0x0400000B RID: 11
			UnmanagedLibrary
		}
	}
}
