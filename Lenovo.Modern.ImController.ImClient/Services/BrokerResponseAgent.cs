using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.ImClient.Services.Umdf;
using Lenovo.Modern.ImController.ImClient.Utilities;

namespace Lenovo.Modern.ImController.ImClient.Services
{
	// Token: 0x02000017 RID: 23
	public class BrokerResponseAgent : IBrokerResponseAgent
	{
		// Token: 0x0600006D RID: 109 RVA: 0x00003424 File Offset: 0x00001624
		private BrokerResponseAgent()
		{
			this._serviceStoppedSource = new CancellationTokenSource();
			this._cancellationToken = this._serviceStoppedSource.Token;
			this._deviceAgent = DeviceDriverAgent.GetInstance();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003474 File Offset: 0x00001674
		public static BrokerResponseAgent GetInstance()
		{
			if (BrokerResponseAgent._instance == null)
			{
				BrokerResponseAgent._instance = new BrokerResponseAgent();
			}
			return BrokerResponseAgent._instance;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000348C File Offset: 0x0000168C
		private void WaitForRequestThread()
		{
			while (!this._cancellationToken.IsCancellationRequested)
			{
				RequestTask requestTask = new RequestTask();
				Tuple<Guid, string> result = this._deviceAgent.WaitForNextRequestAsync(this._cancellationToken).Result;
				if (!this._cancellationToken.IsCancellationRequested && result != null)
				{
					requestTask.TaskId = result.Item1;
					requestTask.Request = result.Item2;
					this._requestTaskQueue.Add(requestTask);
				}
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000034FC File Offset: 0x000016FC
		private void SendResponseThread()
		{
			try
			{
				while (!this._cancellationToken.IsCancellationRequested)
				{
					BrokerResponseAgent.<>c__DisplayClass11_0 CS$<>8__locals1 = new BrokerResponseAgent.<>c__DisplayClass11_0();
					CS$<>8__locals1.<>4__this = this;
					CS$<>8__locals1.response = this._responseTaskQueue.Take(this._cancellationToken);
					if (CS$<>8__locals1.response.Response == null)
					{
						throw new ArgumentNullException("Cannot provide a null Broker Response");
					}
					Guid taskId = CS$<>8__locals1.response.TaskId;
					CS$<>8__locals1.responseXml = Serializer.Serialize<BrokerResponse>(CS$<>8__locals1.response.Response);
					Task.Factory.StartNew<Task>(delegate()
					{
						BrokerResponseAgent.<>c__DisplayClass11_0.<<SendResponseThread>b__0>d <<SendResponseThread>b__0>d;
						<<SendResponseThread>b__0>d.<>4__this = CS$<>8__locals1;
						<<SendResponseThread>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<SendResponseThread>b__0>d.<>1__state = -1;
						AsyncTaskMethodBuilder <>t__builder = <<SendResponseThread>b__0>d.<>t__builder;
						<>t__builder.Start<BrokerResponseAgent.<>c__DisplayClass11_0.<<SendResponseThread>b__0>d>(ref <<SendResponseThread>b__0>d);
						return <<SendResponseThread>b__0>d.<>t__builder.Task;
					}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
				}
			}
			catch (OperationCanceledException)
			{
				ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "BrokerResponseAgent was cancelled");
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000035C8 File Offset: 0x000017C8
		public BlockingCollection<RequestTask> GetRequestQueue()
		{
			return this._requestTaskQueue;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000035D0 File Offset: 0x000017D0
		public BlockingCollection<ResponseTask> GetResponseQueue()
		{
			return this._responseTaskQueue;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000035D8 File Offset: 0x000017D8
		public void Start()
		{
			this._requestWaitThread = Task.Factory.StartNew(delegate()
			{
				this.WaitForRequestThread();
			}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
			this._sendResponseThread = Task.Factory.StartNew(delegate()
			{
				this.SendResponseThread();
			}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003633 File Offset: 0x00001833
		public void Stop()
		{
			this._serviceStoppedSource.Cancel();
			if (this._requestWaitThread != null)
			{
				this._requestWaitThread.Wait();
			}
			if (this._sendResponseThread != null)
			{
				this._sendResponseThread.Wait();
			}
		}

		// Token: 0x0400003B RID: 59
		private readonly DeviceDriverAgent _deviceAgent;

		// Token: 0x0400003C RID: 60
		private CancellationTokenSource _serviceStoppedSource;

		// Token: 0x0400003D RID: 61
		private CancellationToken _cancellationToken;

		// Token: 0x0400003E RID: 62
		private Task _requestWaitThread;

		// Token: 0x0400003F RID: 63
		private Task _sendResponseThread;

		// Token: 0x04000040 RID: 64
		private static BrokerResponseAgent _instance;

		// Token: 0x04000041 RID: 65
		private BlockingCollection<RequestTask> _requestTaskQueue = new BlockingCollection<RequestTask>();

		// Token: 0x04000042 RID: 66
		private BlockingCollection<ResponseTask> _responseTaskQueue = new BlockingCollection<ResponseTask>();
	}
}
