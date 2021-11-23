using System;
using System.Collections.Generic;
using System.Threading;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents.CloudTags
{
	// Token: 0x0200002A RID: 42
	public class CloudTagRegistry
	{
		// Token: 0x06000129 RID: 297 RVA: 0x00009449 File Offset: 0x00007649
		public static CloudTagRegistry GetInstance()
		{
			CloudTagRegistry result;
			if ((result = CloudTagRegistry._instance) == null)
			{
				result = (CloudTagRegistry._instance = new CloudTagRegistry());
			}
			return result;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000945F File Offset: 0x0000765F
		private CloudTagRegistry()
		{
			this.LoadContainer();
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000947C File Offset: 0x0000767C
		private bool LoadContainer()
		{
			bool result;
			try
			{
				this._regSystem = new RegistrySystem();
				this._genericCoreContainer = this._regSystem.LoadContainer("HKEY_CURRENT_USER\\SOFTWARE\\Lenovo\\ImController\\PluginData\\GenericCorePlugin");
				if (this._genericCoreContainer == null)
				{
					IDictionary<string, string> containerValues = new Dictionary<string, string>();
					this._pluginContainer = this._regSystem.LoadContainer(Constants.PluginRegistryKey);
					if (this._pluginContainer != null)
					{
						this._pluginContainer.CreateSubContainer("GenericCorePlugin", containerValues);
					}
					this._genericCoreContainer = this._regSystem.LoadContainer("HKEY_CURRENT_USER\\SOFTWARE\\Lenovo\\ImController\\PluginData\\GenericCorePlugin");
					if (this._genericCoreContainer == null)
					{
						Logger.Log(Logger.LogSeverity.Information, "CloudTagRegistry: Unable to create the container HKEY_CURRENT_USER\\SOFTWARE\\Lenovo\\ImController\\PluginData\\GenericCorePlugin");
						return false;
					}
				}
				result = true;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "CloudTagRegistry: Error caught in LoadContainer()!!");
				throw ex;
			}
			return result;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00009538 File Offset: 0x00007738
		public void SetLastEtag(string eTagValue)
		{
			if (this._genericCoreContainer != null)
			{
				this._genericCoreContainer.SetValue("Etag", eTagValue);
				Logger.Log(Logger.LogSeverity.Information, "CloudTagRegistry : Successfully set eTag value.");
				return;
			}
			Logger.Log(Logger.LogSeverity.Information, "CloudTagRegistry : genericMessagingContainer is null so can't set eTag value for Etag.");
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000956B File Offset: 0x0000776B
		public string GetLastEtag()
		{
			if (this._genericCoreContainer != null)
			{
				IContainerValue value = this._genericCoreContainer.GetValue("Etag");
				return ((value != null) ? value.GetValueAsString() : null) ?? string.Empty;
			}
			return null;
		}

		// Token: 0x0400007E RID: 126
		private const string ETAG = "Etag";

		// Token: 0x0400007F RID: 127
		private RegistrySystem _regSystem;

		// Token: 0x04000080 RID: 128
		private IContainer _pluginContainer;

		// Token: 0x04000081 RID: 129
		private IContainer _genericCoreContainer;

		// Token: 0x04000082 RID: 130
		private SemaphoreSlim _updateSemaphore = new SemaphoreSlim(1);

		// Token: 0x04000083 RID: 131
		private static CloudTagRegistry _instance;
	}
}
