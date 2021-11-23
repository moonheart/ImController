using System;
using System.Threading;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.EventManager.Services
{
	// Token: 0x02000009 RID: 9
	public class EventReactionSender : IEventReactionSender
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002716 File Offset: 0x00000916
		public EventReactionSender(IPluginManager pluginManager, PluginRepository pluginRepository)
		{
			this._pluginManager = pluginManager;
			this._pluginRepository = pluginRepository;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000272C File Offset: 0x0000092C
		public bool SendEventReaction(EventReaction eventReaction, SubscribedEvent subscription, CancellationToken cancelToken)
		{
			try
			{
				PluginRequestInformation pluginInfo = new PluginRequestInformation
				{
					EventReaction = eventReaction,
					PluginName = subscription.Plugin,
					RunAs = (string.IsNullOrEmpty(subscription.Runas) ? RunAs.User : ((subscription.Runas.ToUpper() == RunAs.System.ToString().ToUpper()) ? RunAs.System : RunAs.User)),
					RequestType = RequestType.Event,
					TaskId = Guid.NewGuid().ToString()
				};
				this._pluginManager.MakePluginRequest(pluginInfo, cancelToken);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to forward events to plugin");
			}
			return true;
		}

		// Token: 0x04000017 RID: 23
		private readonly IPluginManager _pluginManager;

		// Token: 0x04000018 RID: 24
		private readonly PluginRepository _pluginRepository;
	}
}
