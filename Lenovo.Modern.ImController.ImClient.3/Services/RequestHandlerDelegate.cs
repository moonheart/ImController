using System;
using System.Threading;
using Lenovo.Modern.ImController.ImClient.Models;

namespace Lenovo.Modern.ImController.ImClient.Services
{
	// Token: 0x02000016 RID: 22
	// (Invoke) Token: 0x0600006A RID: 106
	public delegate bool RequestHandlerDelegate(Guid taskId, BrokerRequest request, CancellationToken cancelToken);
}
