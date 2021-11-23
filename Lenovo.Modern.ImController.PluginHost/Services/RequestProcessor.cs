using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Remoting;
using System.Threading;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.PluginHost.Services.PluginManagers;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Utilities.Ipc;
using Lenovo.Modern.ImController.Shared.Utilities.Validation;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.PluginHost.Services
{
	// Token: 0x0200000A RID: 10
	public class RequestProcessor : MarshalByRefObject, IRequestProcessor
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002C8B File Offset: 0x00000E8B
		public RequestProcessor(PluginDomainManager pluginDomainManager, NamedPipeClient namedPipeClient)
		{
			this._requestCounter = new RequestCounter();
			this._pluginDomainManager = pluginDomainManager;
			this._pluginVerifier = new PluginVerifier();
			this._namedPipeClient = namedPipeClient;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002CB8 File Offset: 0x00000EB8
		public string ProcessRequest(PluginRequestInformation pluginRequestInfo)
		{
			string text = string.Empty;
			try
			{
				if (this._pluginDomainManager == null || pluginRequestInfo == null || string.IsNullOrWhiteSpace(pluginRequestInfo.PluginName) || string.IsNullOrWhiteSpace(pluginRequestInfo.PluginLocation))
				{
					RequestType requestType = RequestType.Event;
					if (pluginRequestInfo != null)
					{
						requestType = pluginRequestInfo.RequestType;
					}
					text = this.GetErrorMessage(requestType, 601, "Plugin Host", "Missing members for RequestProcessor");
				}
				else
				{
					this._taskId = pluginRequestInfo.TaskId;
					this._pluginRequestInfo = pluginRequestInfo;
					bool flag = true;
					try
					{
						if (!this.IsValidPluginLocation(pluginRequestInfo.PluginLocation))
						{
							throw new PluginRepositoryException("Invalid plugin loctaion");
						}
						if (!RequestProcessor._verifiedPlugins.ContainsKey(this._pluginRequestInfo.PluginLocation))
						{
							this._fs = new FileStream(this._pluginRequestInfo.PluginLocation, FileMode.Open, FileAccess.Read, FileShare.Read);
							this._pluginVerifier.VerifyPlugin(this._pluginRequestInfo.PluginLocation);
						}
						RequestProcessor._verifiedPlugins.TryAdd(this._pluginRequestInfo.PluginLocation, this._pluginRequestInfo.PluginName);
					}
					catch (PluginRepositoryException ex)
					{
						flag = false;
						Logger.Log(Logger.LogSeverity.Error, "Plugin is not valid: {0}", new object[] { this._pluginRequestInfo.PluginLocation });
						text = this.GetErrorMessage(this._pluginRequestInfo.RequestType, 602, "Plugin Host", "Plugin not valid: " + ex.Message);
					}
					catch (Exception ex2)
					{
						flag = false;
						Logger.Log(Logger.LogSeverity.Error, "Plugin could not be locked: {0}", new object[] { this._pluginRequestInfo.PluginLocation });
						text = this.GetErrorMessage(this._pluginRequestInfo.RequestType, 602, "Plugin Host", "Plugin could not be locked: " + ex2.Message);
					}
					if (flag)
					{
						bool flag2 = true;
						int num = 0;
						while (flag2)
						{
							try
							{
								Logger.Log(Logger.LogSeverity.Information, "RequestProcessor: Get domain instance for plugin {0}. Plugin path: {1}", new object[]
								{
									this._pluginRequestInfo.PluginName,
									this._pluginRequestInfo.PluginLocation
								});
								this._plugin = this._pluginDomainManager.GetDomainInstance(this._pluginRequestInfo);
								switch (this._pluginRequestInfo.RequestType)
								{
								case RequestType.Application:
								case RequestType.Internal:
								{
									Logger.Log(Logger.LogSeverity.Information, "RequestProcessor: Handling app request.");
									if (this._plugin == null || this._pluginRequestInfo.ContractRequest == null)
									{
										throw new MissingMemberException("Plugin or ContractRequest not found");
									}
									string text2 = Serializer.Serialize<ContractRequest>(this._pluginRequestInfo.ContractRequest);
									this._cancellationEvent = new ManualResetEvent(false);
									Logger.Log(Logger.LogSeverity.Information, "RequestProcessor will send request with taskId [{0}] to plugin: {1}", new object[]
									{
										this._taskId.ToString(),
										text2
									});
									text = this._plugin.InvokeAppRequest(this._taskId, text2, new Func<string, bool>(this.ReceiveResponseFromPlugin), this._cancellationEvent);
									Logger.Log(Logger.LogSeverity.Information, "RequestProcessor InvokeAppRequest for taskId [{0}] returned: {1}", new object[]
									{
										this._taskId.ToString(),
										text
									});
									if (string.IsNullOrEmpty(text))
									{
										text = this.GetErrorMessage(this._pluginRequestInfo.RequestType, 603, "Plugin Host", "App response is not valid");
									}
									this._cancellationEvent.Dispose();
									goto IL_36E;
								}
								}
								Logger.Log(Logger.LogSeverity.Information, "RequestProcessor: Handling event request.");
								if (this._plugin == null || this._pluginRequestInfo.EventReaction == null)
								{
									throw new MissingMemberException("Plugin or EventReaction not found");
								}
								string eventXml = Serializer.Serialize<EventReaction>(this._pluginRequestInfo.EventReaction);
								text = this._plugin.InvokeEventRequest(eventXml);
								if (string.IsNullOrEmpty(text))
								{
									text = this.GetErrorMessage(this._pluginRequestInfo.RequestType, 603, "Plugin Host", "Event response is not valid");
								}
								IL_36E:
								flag2 = false;
							}
							catch (RemotingException)
							{
								GC.Collect();
								Thread.Sleep(1000);
								if (num < 1)
								{
									Logger.Log(Logger.LogSeverity.Information, "RequestProcessor: Domain instance for plugin {0} thrown RemotingException. Will retry.", new object[] { this._pluginRequestInfo.PluginName });
									try
									{
										if (this._plugin.isActive)
										{
											this._plugin.UnloadDomain();
										}
									}
									catch (Exception)
									{
									}
									num++;
								}
								else
								{
									text = this.GetErrorMessage(this._pluginRequestInfo.RequestType, 603, "Plugin Host", "System.Runtime.Remoting.RemotingException in PluginHost: Object has been disconnected or does not exist at the server.");
									Logger.Log(Logger.LogSeverity.Error, "RequestProcessor: Exception thrown invoking " + this._pluginRequestInfo.PluginName);
									Logger.Log(Logger.LogSeverity.Error, "RequestProcessor: System.Runtime.Remoting.RemotingException in PluginHost: Object has been disconnected or does not exist at the server.");
									flag2 = false;
								}
							}
						}
					}
				}
			}
			catch (Exception ex3)
			{
				text = this.GetErrorMessage(this._pluginRequestInfo.RequestType, 603, "Plugin Host", "Exception thrown invoking plugin: " + ex3.Message);
				Logger.Log(Logger.LogSeverity.Error, "Exception thrown invoking " + this._pluginRequestInfo.PluginName, new object[] { ex3 });
				Logger.Log(ex3, "RequestProcessor: Exception in RequestProcessor");
			}
			try
			{
				if (this._namedPipeClient != null)
				{
					this.ReceiveResponseFromPlugin(text);
				}
			}
			catch (Exception ex4)
			{
				Logger.Log(ex4, "RequestProcessor: Exception thrown trying to send response to IMC");
			}
			return text;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003204 File Offset: 0x00001404
		public bool CancelRequest()
		{
			bool result = false;
			if (this._cancellationEvent != null)
			{
				Logger.Log(Logger.LogSeverity.Information, "RequestProcessor: Task {0} will be cancelled", new object[] { this._taskId });
				try
				{
					this._cancellationEvent.Set();
					this._cancellationEvent.Dispose();
				}
				catch (Exception)
				{
					Logger.Log(Logger.LogSeverity.Error, "RequestProcessor: Exception while cancelling task {0}", new object[] { this._taskId });
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003280 File Offset: 0x00001480
		private bool ReceiveResponseFromPlugin(string brokerResponseTaskXml)
		{
			bool result = false;
			this._requestCounter.IncrementResponseCount();
			if (this._requestCounter.HasSurpassedThreshold())
			{
				Logger.Log(Logger.LogSeverity.Warning, "RequestProcessor: This plugin sent too many responses {0} and will be killed", new object[] { this._requestCounter.ResponseCount });
				this.CancelRequest();
				try
				{
					if (this._plugin.isActive)
					{
						this._plugin.UnloadDomain();
					}
				}
				catch (Exception)
				{
				}
				if (this._namedPipeClient != null)
				{
					this._namedPipeClient.Send(this.GetErrorMessage(this._pluginRequestInfo.RequestType, 605, "Plugin Host", "Plugin sent too many responses and was forcefully unloaded"));
				}
				return false;
			}
			if (this._requestCounter.IsResponseDuplicateOfLastResponse(brokerResponseTaskXml))
			{
				Logger.Log(Logger.LogSeverity.Warning, "RequestProcessor: {0} identical responses were discarded before receiving this response", new object[] { this._requestCounter.DuplicateCount });
				return true;
			}
			Logger.Log(Logger.LogSeverity.Information, "RequestProcessor: Plugin response received for taskid {0}: {1}", new object[]
			{
				this._taskId.ToString(),
				brokerResponseTaskXml
			});
			string sendStr = null;
			try
			{
				BrokerResponseTask brokerResponseTask = Serializer.Deserialize<BrokerResponseTask>(brokerResponseTaskXml);
				brokerResponseTask.TaskId = this._taskId;
				sendStr = Serializer.Serialize<BrokerResponseTask>(brokerResponseTask);
			}
			catch (InvalidOperationException)
			{
				sendStr = this.GetErrorMessage(this._pluginRequestInfo.RequestType, 603, "Plugin Host", "Plugin sent an invalid response");
			}
			if (this._namedPipeClient != null)
			{
				this._namedPipeClient.Send(sendStr);
			}
			return result;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000033F0 File Offset: 0x000015F0
		private string GetErrorMessage(RequestType requestType, int resultCode, string resultCodeGroup, string resultDescription)
		{
			string empty = string.Empty;
			switch (requestType)
			{
			case RequestType.Application:
			case RequestType.Cancel:
				return Serializer.Serialize<BrokerResponseTask>(new BrokerResponseTask
				{
					ContractResponse = null,
					EventResponse = null,
					IsComplete = false,
					PercentageComplete = 0,
					StatusComment = "Error",
					TaskId = this._taskId,
					Error = new FailureData
					{
						ResultCode = resultCode,
						ResultCodeGroup = resultCodeGroup,
						ResultDescription = resultDescription
					}
				});
			}
			return Serializer.Serialize<BrokerResponseTask>(new BrokerResponseTask
			{
				ContractResponse = null,
				EventResponse = null,
				IsComplete = false,
				PercentageComplete = 0,
				StatusComment = "Error",
				TaskId = this._taskId,
				Error = new FailureData
				{
					ResultCode = resultCode,
					ResultCodeGroup = resultCodeGroup,
					ResultDescription = resultDescription
				}
			});
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000034D8 File Offset: 0x000016D8
		private bool IsValidPluginLocation(string path)
		{
			bool result = false;
			string directoryName = Path.GetDirectoryName(path);
			directoryName = Path.GetDirectoryName(directoryName);
			directoryName = Path.GetDirectoryName(directoryName);
			if (string.Compare(directoryName, RequestProcessor.GetPluginInstallationLocation(), StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				result = true;
			}
			else
			{
				Logger.Log(Logger.LogSeverity.Warning, "Invalid plugin location {0} {1}", new object[]
				{
					directoryName,
					RequestProcessor.GetPluginInstallationLocation()
				});
			}
			return result;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000352C File Offset: 0x0000172C
		private static string GetPluginInstallationLocation()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), RequestProcessor.PluginsInstallationRelativePath);
		}

		// Token: 0x04000013 RID: 19
		private PluginDomainManager _pluginDomainManager;

		// Token: 0x04000014 RID: 20
		private NamedPipeClient _namedPipeClient;

		// Token: 0x04000015 RID: 21
		private PluginRequestInformation _pluginRequestInfo;

		// Token: 0x04000016 RID: 22
		private PluginVerifier _pluginVerifier;

		// Token: 0x04000017 RID: 23
		private ManualResetEvent _cancellationEvent;

		// Token: 0x04000018 RID: 24
		private DomainedPlugin _plugin;

		// Token: 0x04000019 RID: 25
		private string _taskId;

		// Token: 0x0400001A RID: 26
		private readonly RequestCounter _requestCounter;

		// Token: 0x0400001B RID: 27
		private FileStream _fs;

		// Token: 0x0400001C RID: 28
		private static ConcurrentDictionary<string, string> _verifiedPlugins = new ConcurrentDictionary<string, string>();

		// Token: 0x0400001D RID: 29
		internal static readonly string PluginsInstallationRelativePath = "Lenovo\\iMController\\Plugins";
	}
}
