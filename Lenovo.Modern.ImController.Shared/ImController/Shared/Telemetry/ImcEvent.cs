using System;
using Lenovo.ImController.EventLogging.Model;

namespace Lenovo.Modern.ImController.Shared.Telemetry
{
	// Token: 0x02000006 RID: 6
	public class ImcEvent
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000025A5 File Offset: 0x000007A5
		internal UserEvent Event { get; }

		// Token: 0x06000011 RID: 17 RVA: 0x000025AD File Offset: 0x000007AD
		internal ImcEvent(UserEvent userEvent)
		{
			this.Event = userEvent;
		}
	}
}
