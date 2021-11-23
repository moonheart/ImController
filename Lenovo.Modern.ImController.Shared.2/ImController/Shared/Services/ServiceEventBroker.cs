using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x02000018 RID: 24
	public class ServiceEventBroker
	{
		// Token: 0x0600006F RID: 111 RVA: 0x00004B20 File Offset: 0x00002D20
		public static ServiceEventBroker GetInstance()
		{
			ServiceEventBroker result;
			if ((result = ServiceEventBroker._instance) == null)
			{
				result = (ServiceEventBroker._instance = new ServiceEventBroker());
			}
			return result;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004B36 File Offset: 0x00002D36
		private ServiceEventBroker()
		{
			this._eventHandlerList = new List<IServiceEventHandler>();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004B4C File Offset: 0x00002D4C
		public void AddEventHandler(IServiceEventHandler handler)
		{
			if (this._eventHandlerList.FirstOrDefault((IServiceEventHandler kvp) => kvp == handler) != null)
			{
				throw new Exception(string.Format("Handler already added", new object[0]));
			}
			this._eventHandlerList.Add(handler);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004BA8 File Offset: 0x00002DA8
		public async Task<bool> HandleInitializeAsync()
		{
			bool didAnyFail = false;
			foreach (IServiceEventHandler serviceEventHandler in this._eventHandlerList)
			{
				try
				{
					TaskAwaiter<bool> taskAwaiter = serviceEventHandler.HandleInitializeAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (!taskAwaiter.GetResult())
					{
						didAnyFail = true;
					}
				}
				catch (Exception)
				{
					didAnyFail = true;
				}
			}
			List<IServiceEventHandler>.Enumerator enumerator = default(List<IServiceEventHandler>.Enumerator);
			return !didAnyFail;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004BF0 File Offset: 0x00002DF0
		public async Task<bool> HandleSuspendAsync(EventHandlerReason reason)
		{
			bool didAnyFail = false;
			foreach (IServiceEventHandler serviceEventHandler in this._eventHandlerList)
			{
				try
				{
					TaskAwaiter<bool> taskAwaiter = serviceEventHandler.HandleSuspendAsync(reason).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (!taskAwaiter.GetResult())
					{
						didAnyFail = true;
					}
				}
				catch (Exception)
				{
					didAnyFail = true;
				}
			}
			List<IServiceEventHandler>.Enumerator enumerator = default(List<IServiceEventHandler>.Enumerator);
			return !didAnyFail;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004C40 File Offset: 0x00002E40
		public async Task<bool> HandleResumeAsync(EventHandlerReason reason)
		{
			bool didAnyFail = false;
			foreach (IServiceEventHandler serviceEventHandler in this._eventHandlerList)
			{
				try
				{
					TaskAwaiter<bool> taskAwaiter = serviceEventHandler.HandleResumeAsync(reason).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (!taskAwaiter.GetResult())
					{
						didAnyFail = true;
					}
				}
				catch (Exception)
				{
					didAnyFail = true;
				}
			}
			List<IServiceEventHandler>.Enumerator enumerator = default(List<IServiceEventHandler>.Enumerator);
			return !didAnyFail;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004C90 File Offset: 0x00002E90
		public async Task<bool> HandleUninitializeAsync()
		{
			bool didAnyFail = false;
			foreach (IServiceEventHandler serviceEventHandler in this._eventHandlerList)
			{
				try
				{
					TaskAwaiter<bool> taskAwaiter = serviceEventHandler.HandleUninitializeAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (!taskAwaiter.GetResult())
					{
						didAnyFail = true;
					}
				}
				catch (Exception)
				{
					didAnyFail = true;
				}
			}
			List<IServiceEventHandler>.Enumerator enumerator = default(List<IServiceEventHandler>.Enumerator);
			return !didAnyFail;
		}

		// Token: 0x04000064 RID: 100
		private static ServiceEventBroker _instance;

		// Token: 0x04000065 RID: 101
		private readonly List<IServiceEventHandler> _eventHandlerList;

		// Token: 0x02000059 RID: 89
		public class EventOrder
		{
			// Token: 0x06000210 RID: 528 RVA: 0x0000AA10 File Offset: 0x00008C10
			public EventOrder(int initOrder, int suspendOrder, int resumeOrder)
			{
				this.InitOrderIndex = initOrder;
				this.SuspendOrderIndex = suspendOrder;
				this.ResumeOrderIndex = resumeOrder;
			}

			// Token: 0x17000057 RID: 87
			// (get) Token: 0x06000211 RID: 529 RVA: 0x0000AA2D File Offset: 0x00008C2D
			// (set) Token: 0x06000212 RID: 530 RVA: 0x0000AA35 File Offset: 0x00008C35
			public int InitOrderIndex { get; private set; }

			// Token: 0x17000058 RID: 88
			// (get) Token: 0x06000213 RID: 531 RVA: 0x0000AA3E File Offset: 0x00008C3E
			// (set) Token: 0x06000214 RID: 532 RVA: 0x0000AA46 File Offset: 0x00008C46
			public int SuspendOrderIndex { get; private set; }

			// Token: 0x17000059 RID: 89
			// (get) Token: 0x06000215 RID: 533 RVA: 0x0000AA4F File Offset: 0x00008C4F
			// (set) Token: 0x06000216 RID: 534 RVA: 0x0000AA57 File Offset: 0x00008C57
			public int ResumeOrderIndex { get; private set; }
		}
	}
}
