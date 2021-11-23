using System;
using Lenovo.Modern.ImController.ImClient.Models;

namespace Lenovo.Modern.ImController.Shared.Utilities
{
	// Token: 0x02000030 RID: 48
	public static class BrokerResponseFactory
	{
		// Token: 0x06000178 RID: 376 RVA: 0x000076D4 File Offset: 0x000058D4
		public static BrokerResponse CreateNormalBrokerResponse(string result, BrokerResponseTask brokerResponseTask)
		{
			return new BrokerResponse
			{
				Version = Constants.ImControllerVersion,
				Result = result,
				Task = brokerResponseTask,
				Error = new FailureData
				{
					ResultCode = 0,
					ResultCodeGroup = "General",
					ResultDescription = "Success"
				}
			};
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007728 File Offset: 0x00005928
		public static BrokerResponse CreateSuccessfulBrokerResponse(Guid taskId, ContractResponse contractResponse)
		{
			BrokerResponseTask task = new BrokerResponseTask
			{
				ContractResponse = contractResponse,
				IsComplete = true,
				PercentageComplete = 100,
				TaskId = taskId.ToString()
			};
			return new BrokerResponse
			{
				Version = Constants.ImControllerVersion,
				Result = "Success",
				Task = task,
				Error = new FailureData
				{
					ResultCode = 0,
					ResultCodeGroup = "General",
					ResultDescription = "Success"
				}
			};
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000077B0 File Offset: 0x000059B0
		public static BrokerResponse CreateIntermediateBrokerResponse(Guid taskId, ContractResponse contractResponse, int percentageComplete)
		{
			BrokerResponseTask task = new BrokerResponseTask
			{
				ContractResponse = contractResponse,
				IsComplete = false,
				PercentageComplete = percentageComplete,
				TaskId = taskId.ToString()
			};
			return new BrokerResponse
			{
				Version = Constants.ImControllerVersion,
				Result = "Success",
				Task = task,
				Error = new FailureData
				{
					ResultCode = 0,
					ResultCodeGroup = "General",
					ResultDescription = "Success"
				}
			};
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007838 File Offset: 0x00005A38
		public static BrokerResponse CreateErrorBrokerResponse(string responseCodeGroup, int responseCode, string responseDescription)
		{
			return new BrokerResponse
			{
				Version = Constants.ImControllerVersion,
				Result = null,
				Task = new BrokerResponseTask
				{
					IsComplete = false,
					PercentageComplete = 0,
					StatusComment = "Failure, see failure data",
					ContractResponse = null
				},
				Error = new FailureData
				{
					ResultCode = responseCode,
					ResultCodeGroup = responseCodeGroup,
					ResultDescription = responseDescription
				}
			};
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000078A8 File Offset: 0x00005AA8
		public static BrokerResponse GetRequestCancellationErrorMessage(string taskId)
		{
			return new BrokerResponse
			{
				Version = Constants.ImControllerVersion,
				Result = null,
				Task = new BrokerResponseTask
				{
					IsComplete = false,
					PercentageComplete = 0,
					StatusComment = "Failure, see failure data",
					ContractResponse = null
				},
				Error = new FailureData
				{
					ResultCode = 901,
					ResultCodeGroup = "Plugin Manager",
					ResultDescription = "PluginHostWrapper not found for task ID " + taskId
				}
			};
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007929 File Offset: 0x00005B29
		public static BrokerResponse GetServiceModeErrorMessage(string taskId)
		{
			BrokerResponse requestCancellationErrorMessage = BrokerResponseFactory.GetRequestCancellationErrorMessage(taskId);
			requestCancellationErrorMessage.Error.ResultCodeGroup = "ImController request broker";
			requestCancellationErrorMessage.Error.ResultDescription = "Imcontroller is in service mode " + taskId;
			return requestCancellationErrorMessage;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007957 File Offset: 0x00005B57
		public static BrokerResponse GetImcRequestCancellationErrorMessage(string taskId)
		{
			BrokerResponse serviceModeErrorMessage = BrokerResponseFactory.GetServiceModeErrorMessage(taskId);
			serviceModeErrorMessage.Error.ResultDescription = "Imcontroller request is cancelled due to service suspend " + taskId;
			return serviceModeErrorMessage;
		}
	}
}
