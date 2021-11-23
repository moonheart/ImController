using System;
using System.Collections.Generic;
using Lenovo.ImController.EventLogging.Services.Repositories.Event;

namespace Lenovo.ImController.EventLogging.Services.Repositories
{
	// Token: 0x0200000D RID: 13
	internal interface IReadableEventRepository
	{
		// Token: 0x0600001C RID: 28
		List<EventLogItem> GetNextListOfEventsAfterDate(string userSid, DateTime lastTransmittedTimeStamp, string providerName, int numberOfEvents);
	}
}
