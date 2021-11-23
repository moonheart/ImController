using System;

namespace Lenovo.Modern.ImController.ImClient.Services
{
	// Token: 0x02000011 RID: 17
	public class BrokerRequestAgentException : Exception
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003409 File Offset: 0x00001609
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00003411 File Offset: 0x00001611
		public int ResponseCode { get; set; }

		// Token: 0x06000058 RID: 88 RVA: 0x0000341A File Offset: 0x0000161A
		public BrokerRequestAgentException(string message)
			: base(message)
		{
		}
	}
}
