using System;
using System.Collections.Concurrent;
using Lenovo.Modern.ImController.ImClient.Models;

namespace Lenovo.Modern.ImController.ImClient.Services
{
	// Token: 0x02000013 RID: 19
	public interface IBrokerResponseAgent
	{
		// Token: 0x0600005B RID: 91
		BlockingCollection<RequestTask> GetRequestQueue();

		// Token: 0x0600005C RID: 92
		BlockingCollection<ResponseTask> GetResponseQueue();

		// Token: 0x0600005D RID: 93
		void Start();

		// Token: 0x0600005E RID: 94
		void Stop();
	}
}
