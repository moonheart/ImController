using System;

namespace Lenovo.Modern.ImController.ImClient.Services
{
	// Token: 0x02000018 RID: 24
	public class BrokerResponseAgentException : Exception
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003676 File Offset: 0x00001876
		// (set) Token: 0x06000078 RID: 120 RVA: 0x0000367E File Offset: 0x0000187E
		public int ResponseCode { get; set; }

		// Token: 0x06000079 RID: 121 RVA: 0x0000341A File Offset: 0x0000161A
		public BrokerResponseAgentException(string message)
			: base(message)
		{
		}
	}
}
