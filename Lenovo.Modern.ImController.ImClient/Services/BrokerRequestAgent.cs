using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.ImClient.Services.Umdf;
using Lenovo.Modern.ImController.ImClient.Utilities;

namespace Lenovo.Modern.ImController.ImClient.Services
{
	// Token: 0x02000010 RID: 16
	public class BrokerRequestAgent : IBrokerRequestAgent
	{
		// Token: 0x0600004D RID: 77 RVA: 0x000030E0 File Offset: 0x000012E0
		public BrokerRequestAgent()
		{
			try
			{
				this._deviceAgent = DeviceDriverAgent.GetInstance();
			}
			catch (Exception)
			{
				throw new BrokerRequestAgentException(string.Format("Error occured while communicating with iMDriver. Please install iMController.", new object[0]));
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003128 File Offset: 0x00001328
		public async Task<ImClientRequestTask> MakeRequestAsync(ContractRequest request, Func<BrokerResponseTask, bool> responseReceived, CancellationToken cancelToken)
		{
			if (request == null)
			{
				throw new ArgumentNullException("Cannot provide NULL to broker requester");
			}
			string brokerRequest = Serializer.Serialize<BrokerRequest>(BrokerRequestAgent.GenerateBrokerRequest(request));
			Guid taskId2 = await this._deviceAgent.PutRequestAsync(brokerRequest);
			Guid taskId = taskId2;
			if (taskId == Guid.Empty)
			{
				throw new BrokerRequestAgentException(string.Format("Invalid task ID created {0}", taskId));
			}
			Task<BrokerResponse> brokerResponseTask = Task.Run<BrokerResponse>(() => this.GetFinalContractResponseAsync(taskId, responseReceived, cancelToken));
			return new ImClientRequestTask(taskId, brokerResponseTask, this, responseReceived);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003188 File Offset: 0x00001388
		public async Task<bool> CancelRequestAsync(Guid taskId)
		{
			bool requestSuccessfullyCanceled = false;
			ContractRequest contractRequest = BrokerRequestAgent.GenerateCancellationRequest(taskId);
			using (new CancellationTokenSource(5000))
			{
				string brokerRequest = Serializer.Serialize<BrokerRequest>(BrokerRequestAgent.GenerateBrokerRequest(contractRequest));
				Guid guid = await this._deviceAgent.PutRequestAsync(brokerRequest);
				Guid cancellationtaskId = guid;
				Task<bool> task = this.PutCloseResponse(taskId);
				await Task.WhenAny(new Task[]
				{
					task,
					Task.Delay(1000)
				});
				requestSuccessfullyCanceled = ((task.IsCompleted && task.Result) || !task.IsCompleted) && cancellationtaskId != Guid.Empty;
				if (requestSuccessfullyCanceled)
				{
					Task.Delay(2000).Wait();
				}
				await this._deviceAgent.CloseTaskAsync(taskId);
				await this._deviceAgent.CloseTaskAsync(cancellationtaskId);
				cancellationtaskId = default(Guid);
				task = null;
			}
			CancellationTokenSource cts = null;
			return requestSuccessfullyCanceled;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000031D8 File Offset: 0x000013D8
		private Task<bool> PutCloseResponse(Guid taskId)
		{
			BrokerRequestAgent.<>c__DisplayClass4_0 CS$<>8__locals1 = new BrokerRequestAgent.<>c__DisplayClass4_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.taskId = taskId;
			BrokerResponse instance = new BrokerResponse
			{
				Version = "1.0.0.0",
				Result = null,
				Task = new BrokerResponseTask
				{
					IsComplete = false,
					PercentageComplete = 99,
					StatusComment = "Success",
					ContractResponse = null
				}
			};
			CS$<>8__locals1.responseXml = Serializer.Serialize<BrokerResponse>(instance);
			return Task.Run<bool>(delegate()
			{
				BrokerRequestAgent.<>c__DisplayClass4_0.<<PutCloseResponse>b__0>d <<PutCloseResponse>b__0>d;
				<<PutCloseResponse>b__0>d.<>4__this = CS$<>8__locals1;
				<<PutCloseResponse>b__0>d.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
				<<PutCloseResponse>b__0>d.<>1__state = -1;
				AsyncTaskMethodBuilder<bool> <>t__builder = <<PutCloseResponse>b__0>d.<>t__builder;
				<>t__builder.Start<BrokerRequestAgent.<>c__DisplayClass4_0.<<PutCloseResponse>b__0>d>(ref <<PutCloseResponse>b__0>d);
				return <<PutCloseResponse>b__0>d.<>t__builder.Task;
			});
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003258 File Offset: 0x00001458
		private async Task<BrokerResponse> GetResponseAsync(Guid taskId, Func<BrokerResponseTask, bool> responseReceivedHandler, CancellationToken cancelToken)
		{
			BrokerResponse brokerResponse = null;
			if (taskId == Guid.Empty)
			{
				throw new ArgumentException(string.Format("Invalid task ID provided: {0}", taskId));
			}
			Tuple<Guid, string> tuple = await this._deviceAgent.WaitForResponseAsync(taskId, cancelToken);
			if (tuple == null && cancelToken.IsCancellationRequested)
			{
				throw new BrokerRequestAgentException(string.Format("CancelToken is canceled while waiting for response for task {0}: response pair is null", taskId))
				{
					ResponseCode = 408
				};
			}
			if (tuple == null)
			{
				throw new BrokerRequestAgentException(string.Format("Invalid/Empty Broker Response provided for task {0}: response pair is null", taskId))
				{
					ResponseCode = 408
				};
			}
			if (string.IsNullOrWhiteSpace(tuple.Item2))
			{
				throw new BrokerRequestAgentException(string.Format("Invalid/Empty Broker Response XML provided for task {0}", taskId))
				{
					ResponseCode = 408
				};
			}
			try
			{
				brokerResponse = Serializer.Deserialize<BrokerResponse>(tuple.Item2);
			}
			catch (Exception)
			{
				Console.WriteLine("GetFinalContractResponseAsync exception on response deserialization. responsePair.Item2 = " + tuple.Item2.Length);
			}
			if (brokerResponse == null || brokerResponse.Task == null)
			{
				throw new BrokerRequestAgentException("Invalid broker response data")
				{
					ResponseCode = 409
				};
			}
			return brokerResponse;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000032B0 File Offset: 0x000014B0
		private async Task<BrokerResponse> GetFinalContractResponseAsync(Guid taskId, Func<BrokerResponseTask, bool> responseReceivedHandler, CancellationToken cancelToken)
		{
			BrokerResponse brokerResponse = null;
			bool final = false;
			while (!final)
			{
				BrokerResponse brokerResponse2 = await this.GetResponseAsync(taskId, responseReceivedHandler, cancelToken);
				brokerResponse = brokerResponse2;
				bool flag = BrokerRequestAgent.SafelyBroadcastUpdate(brokerResponse.Task, responseReceivedHandler);
				if (brokerResponse.Error != null && brokerResponse.Error.ResultCode != 0)
				{
					flag = false;
				}
				if (!flag || brokerResponse.Task.IsComplete)
				{
					final = true;
				}
			}
			await this._deviceAgent.CloseTaskAsync(taskId);
			return brokerResponse;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003310 File Offset: 0x00001510
		private static bool SafelyBroadcastUpdate(BrokerResponseTask brokerResponseTask, Func<BrokerResponseTask, bool> responseReceivedHandler)
		{
			bool result = false;
			if (brokerResponseTask != null && responseReceivedHandler != null)
			{
				try
				{
					result = responseReceivedHandler(brokerResponseTask);
				}
				catch (Exception)
				{
				}
			}
			return result;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003344 File Offset: 0x00001544
		public static BrokerRequest GenerateBrokerRequest(ContractRequest contractRequest)
		{
			if (contractRequest == null)
			{
				throw new ArgumentNullException("Cannot provide NULL to broker requester");
			}
			return new BrokerRequest
			{
				ContractRequest = contractRequest,
				Authentication = new BrokerAuthentication
				{
					Token = DateTime.Now.ToString(CultureInfo.InvariantCulture)
				},
				BrokerRequirements = new BrokerRequirements
				{
					MaxVersion = null,
					MinVersion = "1.0.0.0"
				},
				Version = "1.0.0.0"
			};
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000033B8 File Offset: 0x000015B8
		public static ContractRequest GenerateCancellationRequest(Guid taskId)
		{
			return new ContractRequest
			{
				Name = "CancelContractBrokerRequest",
				Command = new ContractCommandRequest
				{
					Name = "CancelContractBrokerRequest",
					Parameter = taskId.ToString(),
					RequestType = "Cancel"
				}
			};
		}

		// Token: 0x04000039 RID: 57
		private readonly IInterProcessRequester _deviceAgent;
	}
}
