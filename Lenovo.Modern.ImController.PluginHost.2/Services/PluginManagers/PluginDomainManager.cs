using System;
using System.Collections.Concurrent;
using System.Threading;
using Lenovo.Modern.ImController.ImClient.Models;

namespace Lenovo.Modern.ImController.PluginHost.Services.PluginManagers
{
	// Token: 0x0200000E RID: 14
	public class PluginDomainManager
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00003A30 File Offset: 0x00001C30
		public PluginDomainManager()
		{
			this._listOfDomainInstances = new ConcurrentDictionary<string, DomainedPlugin>();
			this._eventTimeoutLength = TimeSpan.FromMinutes(1.0);
			this._appTimeoutLength = TimeSpan.FromMinutes(5.0);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003A83 File Offset: 0x00001C83
		public PluginDomainManager(double eventTimeoutLengthInMinutes, double appTimeoutLengthInMinutes)
		{
			this._listOfDomainInstances = new ConcurrentDictionary<string, DomainedPlugin>();
			this._eventTimeoutLength = TimeSpan.FromMinutes(eventTimeoutLengthInMinutes);
			this._appTimeoutLength = TimeSpan.FromMinutes(appTimeoutLengthInMinutes);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003ABC File Offset: 0x00001CBC
		public DomainedPlugin GetDomainInstance(PluginRequestInformation plugin)
		{
			this._instListSem.Wait();
			DomainedPlugin domainedPlugin;
			if (this._listOfDomainInstances.ContainsKey(plugin.PluginName))
			{
				if (this._listOfDomainInstances[plugin.PluginName].isActive)
				{
					domainedPlugin = this._listOfDomainInstances[plugin.PluginName];
				}
				else
				{
					this._listOfDomainInstances.TryRemove(plugin.PluginName, out domainedPlugin);
					domainedPlugin = new DomainedPlugin(plugin);
					this._listOfDomainInstances.TryAdd(plugin.PluginName, domainedPlugin);
				}
			}
			else
			{
				domainedPlugin = new DomainedPlugin(plugin);
				this._listOfDomainInstances.TryAdd(plugin.PluginName, domainedPlugin);
			}
			this._instListSem.Release();
			return domainedPlugin;
		}

		// Token: 0x04000028 RID: 40
		private ConcurrentDictionary<string, DomainedPlugin> _listOfDomainInstances;

		// Token: 0x04000029 RID: 41
		private TimeSpan _eventTimeoutLength;

		// Token: 0x0400002A RID: 42
		private TimeSpan _appTimeoutLength;

		// Token: 0x0400002B RID: 43
		private readonly SemaphoreSlim _instListSem = new SemaphoreSlim(1, 1);
	}
}
