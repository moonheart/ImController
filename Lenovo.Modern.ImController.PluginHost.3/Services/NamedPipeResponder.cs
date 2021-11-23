using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.PluginHost.Services.PluginManagers;
using Lenovo.Modern.ImController.Shared.Utilities.Ipc;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Microsoft.Win32;

namespace Lenovo.Modern.ImController.PluginHost.Services
{
	// Token: 0x02000009 RID: 9
	public class NamedPipeResponder : IIpcResponder
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002827 File Offset: 0x00000A27
		public NamedPipeResponder()
		{
			this._pluginDomainManager = new PluginDomainManager();
			this._requestProcessorMappings = new ConcurrentDictionary<string, RequestProcessor>();
			SystemEvents.PowerModeChanged += this.OnPowerChange;
			this._ipcEnabled = false;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000285D File Offset: 0x00000A5D
		public void BeginWaitingForRequests(string pipeName)
		{
			this._ipcEnabled = true;
			this.InitializePipeServer(pipeName);
			this.InitializePipeClient(pipeName);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002874 File Offset: 0x00000A74
		public void Close()
		{
			this._ipcEnabled = false;
			this._pipeServer.Cancel();
			this._pipeServer.Dispose();
			this._pipeClient.Close();
			this._pipeClient.Dispose();
			this._cancellationTokenSource.Cancel();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000028B4 File Offset: 0x00000AB4
		private void InitializePipeServer(string pipeName)
		{
			this._pipeServer = new NamedPipeServer(pipeName, false);
			this._pipeServer.PipeMessageHandler += delegate(string command)
			{
				this.RespondToPipeMessage(command);
			};
			this._pipeServer.Listen();
			Logger.Log(Logger.LogSeverity.Information, "PluginHost is listening. Pipe name: {0}", new object[] { pipeName });
			this._cancellationTokenSource = new CancellationTokenSource();
			this._cancellationToken = this._cancellationTokenSource.Token;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002921 File Offset: 0x00000B21
		private void InitializePipeClient(string pipeName)
		{
			this._pipeClient = new NamedPipeClient("return" + pipeName);
			this._pipeClient.Connect(CancellationToken.None);
			Logger.Log(Logger.LogSeverity.Information, "PluginHost client is connected to return pipe. Main pipe name: {0}", new object[] { pipeName });
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002960 File Offset: 0x00000B60
		private void OnPowerChange(object s, PowerModeChangedEventArgs e)
		{
			PowerModes mode = e.Mode;
			if (mode == PowerModes.Resume)
			{
				Logger.Log(Logger.LogSeverity.Information, "System is resumed. Enabling IPC.");
				this._ipcEnabled = true;
				return;
			}
			if (mode != PowerModes.Suspend)
			{
				return;
			}
			Logger.Log(Logger.LogSeverity.Information, "System is suspended. Disabling IPC.");
			this._ipcEnabled = false;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000029A4 File Offset: 0x00000BA4
		private bool RespondToPipeMessage(string command)
		{
			this._requestCounter += 1U;
			Logger.Log(Logger.LogSeverity.Information, "New request received (total requests: {0}) by PluginHost (NamedPipeResponder): {1}", new object[] { this._requestCounter, command });
			bool flag = false;
			if (this._ipcEnabled)
			{
				try
				{
					if (!string.IsNullOrWhiteSpace(command))
					{
						PluginRequestInformation request = Serializer.Deserialize<PluginRequestInformation>(command);
						if (request.RequestType == RequestType.Cancel)
						{
							Logger.Log(Logger.LogSeverity.Information, "Pipe received cancelation request for task {0}", new object[] { request.TaskId });
							if (this._requestProcessorMappings.ContainsKey(request.TaskId))
							{
								flag = this._requestProcessorMappings[request.TaskId].CancelRequest();
								if (flag)
								{
									this.SendSuccessfulCancelRequestMessage(request.TaskId);
								}
								else
								{
									Logger.Log(Logger.LogSeverity.Error, "Pipe failed while canceling request {0}", new object[] { request.TaskId });
								}
								RequestProcessor requestProcessor;
								this._requestProcessorMappings.TryRemove(request.TaskId, out requestProcessor);
							}
							else
							{
								string text = string.Join(", ", from m in this._requestProcessorMappings
									select m.Key);
								Logger.Log(Logger.LogSeverity.Error, "Matching request processor not found for requested cancellation with task ID {0}. These exist though: {1}", new object[] { request.TaskId, text });
							}
						}
						else if (!string.IsNullOrWhiteSpace(request.PluginName) && !string.IsNullOrWhiteSpace(request.PluginLocation))
						{
							RequestProcessor processor = new RequestProcessor(this._pluginDomainManager, this._pipeClient);
							this._requestProcessorMappings.TryAdd(request.TaskId, processor);
							Task.Factory.StartNew(delegate()
							{
								processor.ProcessRequest(request);
								this._responseCounter += 1U;
								Logger.Log(Logger.LogSeverity.Information, "Total requests completed: {0}", new object[] { this._responseCounter });
								RequestProcessor requestProcessor2;
								this._requestProcessorMappings.TryRemove(request.TaskId, out requestProcessor2);
							}, this._cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
							flag = true;
						}
						else
						{
							Logger.Log(Logger.LogSeverity.Warning, "IPC received but null or empty data: {0}", new object[] { command });
						}
					}
					return flag;
				}
				catch (AggregateException)
				{
					Logger.Log(Logger.LogSeverity.Information, "Aborting response to IPC due to cancellation request");
					return flag;
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "Unable to respond to IPC request");
					return flag;
				}
			}
			Logger.Log(Logger.LogSeverity.Information, "System is being suspended, IPC is disabled and this response will be discarded");
			flag = true;
			return flag;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002C34 File Offset: 0x00000E34
		private void SendSuccessfulCancelRequestMessage(string taskId)
		{
			string sendStr = Serializer.Serialize<BrokerResponseTask>(new BrokerResponseTask
			{
				ContractResponse = null,
				IsComplete = true,
				PercentageComplete = 100,
				StatusComment = "Success"
			});
			if (this._pipeClient != null)
			{
				this._pipeClient.Send(sendStr);
			}
		}

		// Token: 0x04000009 RID: 9
		private const bool VERIFY_PARENT_PROCESS = false;

		// Token: 0x0400000A RID: 10
		private NamedPipeServer _pipeServer;

		// Token: 0x0400000B RID: 11
		private NamedPipeClient _pipeClient;

		// Token: 0x0400000C RID: 12
		private CancellationTokenSource _cancellationTokenSource;

		// Token: 0x0400000D RID: 13
		private CancellationToken _cancellationToken;

		// Token: 0x0400000E RID: 14
		private PluginDomainManager _pluginDomainManager;

		// Token: 0x0400000F RID: 15
		private ConcurrentDictionary<string, RequestProcessor> _requestProcessorMappings;

		// Token: 0x04000010 RID: 16
		private uint _requestCounter;

		// Token: 0x04000011 RID: 17
		private uint _responseCounter;

		// Token: 0x04000012 RID: 18
		private bool _ipcEnabled;
	}
}
