using System;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.ImClient.Services;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x0200002F RID: 47
	public sealed class ImClientRequestTask
	{
		// Token: 0x06000122 RID: 290 RVA: 0x00004CFF File Offset: 0x00002EFF
		internal ImClientRequestTask(Guid taskId, Task<BrokerResponse> brokerResponseTask, IBrokerRequestAgent brokerRequestAgent, Func<BrokerResponseTask, bool> responseReceived)
		{
			if (brokerResponseTask == null)
			{
				throw new ArgumentNullException();
			}
			this._taskId = taskId;
			this._brokerResponseTask = brokerResponseTask;
			this._brokerRequestAgent = brokerRequestAgent;
			this.ResponseReceivedFunction = responseReceived;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00004D2D File Offset: 0x00002F2D
		public Guid TaskId
		{
			get
			{
				return this._taskId;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00004D35 File Offset: 0x00002F35
		public Task<BrokerResponse> ResponseRetreivalTask
		{
			get
			{
				return this._brokerResponseTask;
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00004D3D File Offset: 0x00002F3D
		public Task<bool> Cancel()
		{
			return this._brokerRequestAgent.CancelRequestAsync(this.TaskId);
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00004D50 File Offset: 0x00002F50
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00004D58 File Offset: 0x00002F58
		public Func<BrokerResponseTask, bool> ResponseReceivedFunction { get; private set; }

		// Token: 0x04000094 RID: 148
		private readonly Guid _taskId;

		// Token: 0x04000095 RID: 149
		private readonly Task<BrokerResponse> _brokerResponseTask;

		// Token: 0x04000096 RID: 150
		private readonly IBrokerRequestAgent _brokerRequestAgent;
	}
}
