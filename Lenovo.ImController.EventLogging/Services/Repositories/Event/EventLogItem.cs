using System;
using Lenovo.ImController.EventLogging.Model;

namespace Lenovo.ImController.EventLogging.Services.Repositories.Event
{
	// Token: 0x0200000F RID: 15
	internal class EventLogItem
	{
		// Token: 0x0600001E RID: 30 RVA: 0x0000278B File Offset: 0x0000098B
		public EventLogItem(DateTime dateLogged, StorableEvent userEvent)
		{
			if (dateLogged == DateTime.MinValue || dateLogged == DateTime.MaxValue)
			{
				throw new ArgumentException("Null or invalid arguments to create StoredEvent");
			}
			this.DateLogged = dateLogged;
			this.Event = userEvent;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000027C6 File Offset: 0x000009C6
		public StorableEvent Event { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000027CE File Offset: 0x000009CE
		public int EventCategory { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000027D6 File Offset: 0x000009D6
		public int EventId { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000027DE File Offset: 0x000009DE
		public DateTime DateLogged { get; }
	}
}
