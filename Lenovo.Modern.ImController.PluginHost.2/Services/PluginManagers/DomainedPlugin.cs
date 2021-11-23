using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.PluginHost.AppDomain;
using Lenovo.Modern.ImController.PluginHost.AppDomain.Services;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.PluginHost.Services.PluginManagers
{
	// Token: 0x0200000D RID: 13
	public class DomainedPlugin : IPluginInvoker
	{
		// Token: 0x0600003B RID: 59 RVA: 0x000036A8 File Offset: 0x000018A8
		public DomainedPlugin(PluginRequestInformation pluginInfo)
		{
			if (!File.Exists(pluginInfo.PluginLocation))
			{
				throw new AppDomainException("No file found at " + pluginInfo.PluginLocation);
			}
			this._pluginInfo = pluginInfo;
			Logger.Log(Logger.LogSeverity.Information, "Setting up plugin {0}", new object[] { pluginInfo.PluginName });
			AppDomainSetup info = new AppDomainSetup
			{
				ApplicationBase = Path.GetDirectoryName(pluginInfo.PluginLocation)
			};
			this._domain = AppDomain.CreateDomain(this._pluginInfo.PluginName, null, info);
			Type typeFromHandle = typeof(AppDomainProxy);
			try
			{
				this._proxy = (AppDomainProxy)this._domain.CreateInstanceFromAndUnwrap(typeFromHandle.Assembly.CodeBase, typeFromHandle.FullName, true, BindingFlags.Default, null, null, CultureInfo.CurrentCulture, null);
				if (this._proxy == null)
				{
					throw new Exception(string.Format("Unable to create AppDomainProxy for file {0}, type:  {1}", typeFromHandle.Assembly.CodeBase, typeFromHandle.FullName));
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to create app domain for {0}", new object[] { this._pluginInfo.PluginLocation });
			}
			AppDomainProxy.PluginType pluginType = ((this._pluginInfo.PluginType == PluginType.ManagedLibrary) ? AppDomainProxy.PluginType.ManagedLibrary : AppDomainProxy.PluginType.UnmanagedLibrary);
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "Instantiating Assembly. Plugin location: {0}, plugin type: {1}, plugin bitness: {2}", new object[]
				{
					this._pluginInfo.PluginLocation,
					pluginType.ToString(),
					this._pluginInfo.Bitness.ToString()
				});
				if (Environment.Is64BitProcess)
				{
					Logger.Log(Logger.LogSeverity.Information, "DomainedPlugin is running in 64 bit process");
				}
				else
				{
					Logger.Log(Logger.LogSeverity.Information, "DomainedPlugin is running in 32 bit process");
				}
				AppDomainProxy proxy = this._proxy;
				if (proxy != null)
				{
					proxy.InstantiateAssemblyAndInvoker(this._pluginInfo.PluginLocation, pluginType.ToString());
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "Unable to instantiate app proxy for {0}", new object[] { this._pluginInfo.PluginLocation });
			}
			this._pluginRequestMtx = new SemaphoreSlim(1, 1);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600003C RID: 60 RVA: 0x000038A8 File Offset: 0x00001AA8
		public PluginRequestInformation Plugin
		{
			get
			{
				if (this._domain == null)
				{
					throw new AppDomainException("Domain was unloaded");
				}
				return this._pluginInfo;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600003D RID: 61 RVA: 0x000038C3 File Offset: 0x00001AC3
		public bool isActive
		{
			get
			{
				return this._domain != null;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000038D0 File Offset: 0x00001AD0
		public bool UnloadDomain()
		{
			if (this._domain != null)
			{
				AppDomain.Unload(this._domain);
				this._domain = null;
			}
			return true;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000038F0 File Offset: 0x00001AF0
		public string InvokeAppRequest(string taskId, string requestXml, Func<string, bool> updateReceiver, WaitHandle cancelEvent)
		{
			string text = null;
			if (!cancelEvent.WaitOne(0))
			{
				Semaphore semaphore = new Semaphore(0, 1048576);
				ManualResetEvent manualResetEvent = new ManualResetEvent(false);
				this._proxy.InvokeAppRequest(taskId, requestXml, semaphore.SafeWaitHandle.DangerousGetHandle(), manualResetEvent.SafeWaitHandle.DangerousGetHandle(), cancelEvent.SafeWaitHandle.DangerousGetHandle());
				WaitHandle[] waitHandles = new WaitHandle[] { semaphore, manualResetEvent };
				while (text == null)
				{
					if (!this.isActive)
					{
						break;
					}
					int num = WaitHandle.WaitAny(waitHandles);
					if (num != 0)
					{
						if (num == 1)
						{
							if (this.isActive)
							{
								text = this._proxy.GetFinalResponse(taskId);
							}
						}
					}
					else if (this.isActive)
					{
						updateReceiver(this._proxy.GetNextIntermediateResponse(taskId));
					}
				}
			}
			else
			{
				text = Serializer.Serialize<BrokerResponseTask>(new BrokerResponseTask
				{
					ContractResponse = null,
					IsComplete = false,
					PercentageComplete = 0,
					StatusComment = "Error",
					Error = new FailureData
					{
						ResultCode = 701,
						ResultCodeGroup = "Plugin Error",
						ResultDescription = "Request has been cancelled before it started"
					}
				});
			}
			return text;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003A09 File Offset: 0x00001C09
		public string InvokeEventRequest(string eventXml)
		{
			this._pluginRequestMtx.Wait();
			string result = this._proxy.InvokeEventRequest(eventXml);
			this._pluginRequestMtx.Release();
			return result;
		}

		// Token: 0x04000024 RID: 36
		private readonly PluginRequestInformation _pluginInfo;

		// Token: 0x04000025 RID: 37
		private AppDomain _domain;

		// Token: 0x04000026 RID: 38
		private readonly AppDomainProxy _proxy;

		// Token: 0x04000027 RID: 39
		private SemaphoreSlim _pluginRequestMtx;
	}
}
