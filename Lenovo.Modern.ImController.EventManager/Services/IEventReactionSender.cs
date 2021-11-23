using System;
using System.Threading;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model.Packages;

namespace Lenovo.Modern.ImController.EventManager.Services
{
	// Token: 0x0200000C RID: 12
	internal interface IEventReactionSender
	{
		// Token: 0x06000022 RID: 34
		bool SendEventReaction(EventReaction eventReaction, SubscribedEvent subscription, CancellationToken cancelToken);
	}
}
