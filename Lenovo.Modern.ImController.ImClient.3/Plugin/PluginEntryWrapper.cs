using System;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.ImClient.Utilities;

namespace Lenovo.Modern.ImController.ImClient.Plugin
{
	// Token: 0x0200000A RID: 10
	public class PluginEntryWrapper : IPlugin
	{
		// Token: 0x06000026 RID: 38 RVA: 0x0000295E File Offset: 0x00000B5E
		public PluginEntryWrapper(CommandMapper commandMapper)
		{
			this._commandMapper = commandMapper;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002970 File Offset: 0x00000B70
		public string HandleAppRequest(string contractRequestXml, Func<string, bool> intermediateResponseFunction, WaitHandle cancelEvent)
		{
			string text = null;
			ManualResetEvent pluginFinishedEvent = new ManualResetEvent(false);
			try
			{
				BrokerResponseTask emptyBrokerResponseTaskForEvent = PluginEntryWrapper.EmptyBrokerResponseTaskForEvent;
				ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "Received app request: " + (contractRequestXml ?? "Null"));
				if (string.IsNullOrWhiteSpace(contractRequestXml))
				{
					throw new ArgumentNullException("Empty data provided");
				}
				ContractRequest contractRequest = Serializer.Deserialize<ContractRequest>(contractRequestXml);
				using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
				{
					if (cancelEvent != null)
					{
						Task.Factory.StartNew(delegate()
						{
							try
							{
								if (WaitHandle.WaitAny(new WaitHandle[] { cancelEvent, pluginFinishedEvent }) == 0)
								{
									cancellationTokenSource.Cancel();
								}
							}
							catch
							{
							}
						}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
					}
					PluginEntryWrapper.PluginEntryRequestFunction pluginEntryRequestFunction = this._commandMapper.ResolveRequestCommandHandler(contractRequest);
					if (pluginEntryRequestFunction == null)
					{
						throw new Exception(string.Format("No action mapped to command {0}", contractRequest.Command.Name));
					}
					Task<BrokerResponseTask> task = PluginEntryWrapper.TryHandleAppRequest(pluginEntryRequestFunction, contractRequest, intermediateResponseFunction, cancellationTokenSource.Token);
					task.Wait();
					if (task.IsCompleted && !task.IsFaulted)
					{
						if (task.Result != null)
						{
							text = Serializer.Serialize<BrokerResponseTask>(task.Result);
						}
					}
					else
					{
						ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Error, string.Format("Plugin handling of app request did not succeed.\r\n{0}", contractRequestXml));
					}
					ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "Returning  app result: " + (text ?? "NULL"));
				}
			}
			catch (Exception ex)
			{
				ExternalLogger.Instance.Log(ex, "Exception while handling contract request.\r\n" + contractRequestXml);
			}
			finally
			{
				pluginFinishedEvent.Set();
				pluginFinishedEvent.Close();
			}
			return text;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002B5C File Offset: 0x00000D5C
		public string HandleEvent(string eventReactionXml)
		{
			string text = null;
			try
			{
				BrokerResponseTask instance = PluginEntryWrapper.EmptyBrokerResponseTaskForEvent;
				ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "Responding to event request: " + (eventReactionXml ?? "Null"));
				if (string.IsNullOrWhiteSpace(eventReactionXml))
				{
					throw new ArgumentNullException("Empty data provided to HandleEvent");
				}
				EventReaction eventReaction = Serializer.Deserialize<EventReaction>(eventReactionXml);
				Task<EventResponse> task = PluginEntryWrapper.TryHandleEventRequest(this._commandMapper.ResolveEventHandler(eventReaction.Monitor, eventReaction.Trigger), eventReaction);
				task.Wait();
				if (task.IsCompleted && !task.IsFaulted)
				{
					EventResponse result = task.Result;
					if (result != null)
					{
						instance = new BrokerResponseTask
						{
							KeepAliveMinutes = result.KeepAliveMinutes,
							EventResponse = result,
							IsComplete = true,
							PercentageComplete = 100
						};
					}
				}
				text = Serializer.Serialize<BrokerResponseTask>(instance);
				ExternalLogger.Instance.Log(ExternalLogger.LogSeverity.Information, "Returning to event result: " + (text ?? "Null"));
			}
			catch (Exception ex)
			{
				ExternalLogger.Instance.Log(ex, "Exception while handling contract event request.\r\n" + (eventReactionXml ?? "NULL"));
			}
			return text;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002C74 File Offset: 0x00000E74
		private static async Task<BrokerResponseTask> TryHandleAppRequest(PluginEntryWrapper.PluginEntryRequestFunction functionToExecute, ContractRequest contractRequest, Func<string, bool> intermediateResponseFunction, CancellationToken cancelToken)
		{
			if (functionToExecute == null || contractRequest == null || intermediateResponseFunction == null)
			{
				throw new ArgumentException("Cannot provide invalid arguments to TryHandleEventRequest");
			}
			BrokerResponseTask response = null;
			try
			{
				BrokerResponseTask brokerResponseTask = await functionToExecute(contractRequest, intermediateResponseFunction, cancelToken);
				response = brokerResponseTask;
			}
			catch (Exception ex)
			{
				ExternalLogger.Instance.Log(ex, "Exception while invoking contract request. handler\r\n" + ((contractRequest != null) ? contractRequest.Name : null));
			}
			return response;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002CD4 File Offset: 0x00000ED4
		private static async Task<EventResponse> TryHandleEventRequest(PluginEntryWrapper.PluginEntryEventFunction functionToExecute, EventReaction eventReaction)
		{
			if (functionToExecute == null || eventReaction == null)
			{
				throw new ArgumentException("Cannot provide invalid arguments to TryHandleEventRequest");
			}
			EventResponse response = null;
			try
			{
				EventResponse eventResponse = await functionToExecute(eventReaction);
				response = eventResponse;
			}
			catch (Exception ex)
			{
				ExternalLogger.Instance.Log(ex, string.Format("Exception while invoking event handler.\r\n{0}", response.Error));
			}
			return response;
		}

		// Token: 0x0400000B RID: 11
		private static readonly BrokerResponseTask EmptyBrokerResponseTaskForAppRequest = new BrokerResponseTask
		{
			ContractResponse = new ContractResponse
			{
				Response = new ResponseData
				{
					Data = ""
				}
			},
			Error = new FailureData
			{
				ResultDescription = "No response was provided"
			},
			IsComplete = true,
			PercentageComplete = 100
		};

		// Token: 0x0400000C RID: 12
		private static readonly BrokerResponseTask EmptyBrokerResponseTaskForEvent = new BrokerResponseTask
		{
			EventResponse = new EventResponse
			{
				Data = ""
			},
			Error = new FailureData
			{
				ResultDescription = "No response was provided"
			},
			IsComplete = true,
			PercentageComplete = 100
		};

		// Token: 0x0400000D RID: 13
		private CommandMapper _commandMapper;

		// Token: 0x02000044 RID: 68
		// (Invoke) Token: 0x06000155 RID: 341
		public delegate Task<BrokerResponseTask> PluginEntryRequestFunction(ContractRequest request, Func<string, bool> intermediateResponseFunction, CancellationToken cancellationToken);

		// Token: 0x02000045 RID: 69
		// (Invoke) Token: 0x06000159 RID: 345
		public delegate Task<EventResponse> PluginEntryEventFunction(EventReaction eventDetails);
	}
}
