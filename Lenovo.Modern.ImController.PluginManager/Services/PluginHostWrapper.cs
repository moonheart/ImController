using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Telemetry;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.ImController.Shared.Utilities.Ipc;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.SystemContext.ProcessLauncher;
using Lenovo.Modern.Utilities.Services.Wrappers.Process;

namespace Lenovo.Modern.ImController.PluginManager.Services
{
	// Token: 0x02000004 RID: 4
	public class PluginHostWrapper
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020A3 File Offset: 0x000002A3
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000020AB File Offset: 0x000002AB
		public Bitness Bit { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020B4 File Offset: 0x000002B4
		// (set) Token: 0x06000006 RID: 6 RVA: 0x000020BC File Offset: 0x000002BC
		public RunAs Context { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020C5 File Offset: 0x000002C5
		// (set) Token: 0x06000008 RID: 8 RVA: 0x000020CD File Offset: 0x000002CD
		public string PluginName { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000020D6 File Offset: 0x000002D6
		// (set) Token: 0x0600000A RID: 10 RVA: 0x000020DE File Offset: 0x000002DE
		public string PluginVersion { get; private set; }

		// Token: 0x0600000B RID: 11 RVA: 0x000020E8 File Offset: 0x000002E8
		public PluginHostWrapper(string pluginName, string pluginVersion, RunAs context, Bitness bitness, Func<Guid, BrokerResponse, bool> rspHandler, bool deviceAssociation, string associatedUapId)
			: this(context)
		{
			this.Context = context;
			this.Bit = bitness;
			this._responseHandler = rspHandler;
			this.PluginName = pluginName;
			this.PluginVersion = pluginVersion;
			this._finalResponses = new ConcurrentDictionary<string, string>();
			this._retryEvent = new ManualResetEventSlim(false);
			this._deviceAssociation = deviceAssociation;
			if (!string.IsNullOrWhiteSpace(associatedUapId))
			{
				this._uapAssociation = true;
				this._associatedUapId = associatedUapId;
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002158 File Offset: 0x00000358
		public PluginHostWrapper(RunAs context)
		{
			this.Context = context;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002168 File Offset: 0x00000368
		public void CleanupData()
		{
			if (this._exitEvent != null)
			{
				this._exitEvent.Dispose();
			}
			if (this._pluginHostProcess != null)
			{
				this._pluginHostProcess.Dispose();
			}
			if (this._pipeClient != null)
			{
				this._pipeClient.Dispose();
			}
			if (this._pipeServer != null)
			{
				this._pipeServer.Dispose();
			}
			if (this._retryEvent != null)
			{
				this._retryEvent.Dispose();
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021D4 File Offset: 0x000003D4
		public bool StartPluginHost(CancellationToken cancelToken)
		{
			this._pipeName = Guid.NewGuid().ToString();
			Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Start Plugin Host as {2}.", new object[]
			{
				this.PluginName,
				this._pipeName,
				this.Context.ToString()
			});
			EventWaitHandleAccessRule rule = new EventWaitHandleAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), EventWaitHandleRights.FullControl, AccessControlType.Allow);
			EventWaitHandleSecurity eventWaitHandleSecurity = new EventWaitHandleSecurity();
			eventWaitHandleSecurity.AddAccessRule(rule);
			string text = "Global\\evt_" + this._pipeName;
			Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}:Waiting for Plugin Host Wrapper to set our exit event, event name: {2}", new object[] { this.PluginName, this._pipeName, text });
			bool flag = false;
			this._exitEvent = new EventWaitHandle(false, EventResetMode.ManualReset, text, ref flag, eventWaitHandleSecurity);
			this._exitEvent.Reset();
			Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Plugin Host exit event was reset, event name: {2}", new object[] { this.PluginName, this._pipeName, text });
			bool flag2;
			if (this.Context == RunAs.User)
			{
				flag2 = this.StartPluginHostAsUser();
			}
			else
			{
				flag2 = this.StartPluginHostAsSystem();
			}
			if (flag2)
			{
				this._pipeClient = new NamedPipeClient(this._pipeName);
				this._pipeClient.CriticalErrorHandler += delegate()
				{
					if (!cancelToken.IsCancellationRequested)
					{
						try
						{
							this.StopPluginHost();
							if (this._pluginHostProcess != null)
							{
								int id = this._pluginHostProcess.Id;
								Logger.Log(Logger.LogSeverity.Warning, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Plugin Host (PID: {2}) does not respond and will be killed", new object[] { this.PluginName, this._pipeName, id });
								this._pluginHostProcess.Kill();
								Logger.Log(Logger.LogSeverity.Warning, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Plugin Host (PID: {2}) was killed successfully", new object[] { this.PluginName, this._pipeName, id });
							}
						}
						catch (Exception ex)
						{
							Logger.Log(ex, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Exception while killing PluginHost", new object[] { this.PluginName, this._pipeName });
						}
					}
					if (!cancelToken.IsCancellationRequested)
					{
						try
						{
							this.StartPluginHost(cancelToken);
							this._retryEvent.Set();
						}
						catch (Exception ex2)
						{
							Logger.Log(ex2, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Exception while restarting PluginHost", new object[] { this.PluginName, this._pipeName });
						}
					}
				};
				this._pipeServer = new NamedPipeServer("return" + this._pipeName, false);
				this._pipeServer.PipeMessageHandler += delegate(string response)
				{
					this.HandlePluginResponse(response);
				};
				this._pipeServer.Listen();
				Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Client on service side is waiting for Plugin Host to go live", new object[] { this.PluginName, this._pipeName });
				this.WaitForServerLive(cancelToken);
				Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Host on pipe is live, trying to connect...", new object[] { this.PluginName, this._pipeName });
				this._pipeClient.Connect(cancelToken);
			}
			return flag2;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000023D8 File Offset: 0x000005D8
		public void StopPluginHost()
		{
			Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: StopPluginHost called", new object[] { this.PluginName, this._pipeName });
			if (this._exitEvent != null)
			{
				Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Stopping plugin host... ", new object[] { this.PluginName, this._pipeName });
				this._exitEvent.Set();
			}
			if (this._pipeClient != null)
			{
				this._pipeClient.Close();
			}
			if (this._pipeServer != null)
			{
				this._pipeServer.Cancel();
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002468 File Offset: 0x00000668
		public bool IsPluginHostStopped()
		{
			bool flag = false;
			try
			{
				if (this._pluginHostProcess != null)
				{
					flag = this._pluginHostProcess.HasExited;
					if (flag)
					{
						Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Plugin Host as {2} was closed.", new object[]
						{
							this.PluginName,
							this._pipeName,
							this.Context.ToString()
						});
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PluginHostWrapper IsPluginHostStopped:{0}, Pipe:{1}: Cannot stop plugin host", new object[] { this.PluginName, this._pipeName });
			}
			return flag;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002500 File Offset: 0x00000700
		public bool WaitForPluginHostToStop(int timeout = 0)
		{
			bool flag = false;
			try
			{
				if (this._pluginHostProcess != null)
				{
					Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Waiting for plugin host process to exit...", new object[] { this.PluginName, this._pipeName });
					if (timeout <= 0)
					{
						flag = this._pluginHostProcess.WaitForExit(PluginHostWrapper._pluginHostExitTimeoutMS);
					}
					else
					{
						flag = this._pluginHostProcess.WaitForExit(timeout);
					}
					if (flag)
					{
						Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Plugin Host as {2} was closed.", new object[]
						{
							this.PluginName,
							this._pipeName,
							this.Context.ToString()
						});
						this.CleanupData();
						this._pluginHostProcess = null;
					}
					else
					{
						Logger.Log(Logger.LogSeverity.Warning, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Plugin Host as {0} did not close within timeout", new object[]
						{
							this.PluginName,
							this._pipeName,
							this.Context.ToString()
						});
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Cannot stop plugin host", new object[] { this.PluginName, this._pipeName });
			}
			return flag;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002620 File Offset: 0x00000820
		public void Kill()
		{
			try
			{
				if (this._pluginHostProcess != null)
				{
					int id = this._pluginHostProcess.Id;
					Logger.Log(Logger.LogSeverity.Warning, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Plugin Host (PID: {2}) did not exit properly and will be killed", new object[] { this.PluginName, this._pipeName, id });
					this._pluginHostProcess.Kill();
					this.CleanupData();
					this._pluginHostProcess = null;
					Logger.Log(Logger.LogSeverity.Warning, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Plugin Host (PID: {0}) was killed successfully", new object[] { this.PluginName, this._pipeName, id });
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Exception in PluginHostWrapper.WaitForPluginHostToStop", new object[] { this.PluginName, this._pipeName });
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000026E8 File Offset: 0x000008E8
		public static void KillZombie()
		{
			try
			{
				string pluginHostFileNameLowerCaseFriendly = Constants.PluginHostFileNameLowerCaseFriendly;
				foreach (Process process in Process.GetProcesses())
				{
					if ((process.ProcessName.Contains("lenovo.modern") || process.ProcessName.Contains("Lenovo.Modern")) && process.ProcessName.ToLower().Contains(pluginHostFileNameLowerCaseFriendly))
					{
						Logger.Log(Logger.LogSeverity.Warning, "PluginHostWrapper: A zombie PluginHost process was found and will be killed. PID: {0}", new object[] { process.Id });
						try
						{
							process.Kill();
						}
						catch (Exception ex)
						{
							Logger.Log(ex, "PluginHostWrapper: Exception while killing zombie PluginHost process with pid {0}", new object[] { process.Id });
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "PluginHostWrapper: Exception while killing zombie PluginHost processes");
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000027C4 File Offset: 0x000009C4
		private void WaitForServerLive(CancellationToken waitCancelToken)
		{
			bool flag = false;
			try
			{
				if (this._pipeClient.WaitForServerLive(waitCancelToken))
				{
					flag = true;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Exception in PluginHostWrapper.WaitForServerLive", new object[] { this.PluginName, this._pipeName });
			}
			if (!flag)
			{
				throw new PluginHostWrapperException("Exception thrown trying to wait for plugin host")
				{
					ResponseCode = 1001
				};
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002834 File Offset: 0x00000A34
		public async Task<BrokerResponseTask> MakePluginRequest(PluginRequestInformation pluginInfo, CancellationToken cancelToken)
		{
			BrokerResponseTask response = null;
			if (pluginInfo != null)
			{
				Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: MakePluginRequest called. Context: {2}, TaskId: {3}", new object[]
				{
					this.PluginName,
					this._pipeName,
					this.Context.ToString(),
					pluginInfo.TaskId
				});
				if (pluginInfo.RequestType == RequestType.Application || pluginInfo.RequestType == RequestType.Cancel || pluginInfo.RequestType == RequestType.Internal)
				{
					EventFactory.Constants.PluginActivityType activityType = EventFactory.Constants.PluginActivityType.Contract;
					ContractRequest contractRequest = pluginInfo.ContractRequest;
					string str = ((contractRequest != null) ? contractRequest.Name : null);
					string str2 = "::";
					ContractRequest contractRequest2 = pluginInfo.ContractRequest;
					string str3;
					if (contractRequest2 == null)
					{
						str3 = null;
					}
					else
					{
						ContractCommandRequest command = contractRequest2.Command;
						str3 = ((command != null) ? command.Name : null);
					}
					ImcEvent userEvent = EventFactory.CreatePluginActivityEvent(activityType, str + str2 + str3, this.PluginName, this.PluginVersion);
					EventLogger.GetInstance().LogEvent(userEvent);
				}
				else if (pluginInfo.RequestType == RequestType.Event)
				{
					EventFactory.Constants.PluginActivityType activityType2 = EventFactory.Constants.PluginActivityType.Event;
					EventReaction eventReaction = pluginInfo.EventReaction;
					string str4 = ((eventReaction != null) ? eventReaction.Monitor : null);
					string str5 = "::";
					EventReaction eventReaction2 = pluginInfo.EventReaction;
					ImcEvent userEvent2 = EventFactory.CreatePluginActivityEvent(activityType2, str4 + str5 + ((eventReaction2 != null) ? eventReaction2.Trigger : null), this.PluginName, this.PluginVersion);
					EventLogger.GetInstance().LogEvent(userEvent2);
				}
			}
			if (cancelToken.IsCancellationRequested)
			{
				Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1} TaskId{2}: MakePluginRequest: Cancellation was requested", new object[]
				{
					this.PluginName,
					this._pipeName,
					(pluginInfo != null) ? pluginInfo.TaskId : "NULL"
				});
			}
			else
			{
				bool flag = pluginInfo != null && pluginInfo.ContractRequest != null && pluginInfo.ContractRequest.Command != null && pluginInfo.ContractRequest.Command.Name != null;
				bool flag2 = pluginInfo != null && pluginInfo.EventReaction != null && pluginInfo.EventReaction.Trigger != null;
				if (pluginInfo == null || pluginInfo.PluginName == null || pluginInfo.PluginLocation == null || (!flag && !flag2))
				{
					response = Serializer.Deserialize<BrokerResponseTask>("<BrokerResponseTask isComplete=\"false\" percentageComplete=\"0\"><FailureData><ResultCodeGroup>Plugin Host</ResultCodeGroup><ResultCode>606</ResultCode><ResultDescription>Error processing request: Bad parameters.</ResultDescription></FailureData></BrokerResponseTask>");
				}
				else
				{
					if (flag)
					{
						Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: MakePluginRequest called with CONTRACT request. Context: {2}, command: {3}", new object[]
						{
							this.PluginName,
							this._pipeName,
							this.Context.ToString(),
							this._pipeName,
							pluginInfo.PluginName,
							pluginInfo.ContractRequest.Command.Name
						});
					}
					else
					{
						Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: MakePluginRequest called with EVENT request. Context: {2},  trigger: {3}", new object[]
						{
							this.PluginName,
							this._pipeName,
							this.Context.ToString(),
							pluginInfo.EventReaction.Trigger
						});
					}
					try
					{
						string requestXml = Serializer.Serialize<PluginRequestInformation>(pluginInfo);
						string text = await this.SendPluginRequest(pluginInfo.TaskId, requestXml, cancelToken);
						if (pluginInfo.RequestType == RequestType.Cancel)
						{
							response = Serializer.Deserialize<BrokerResponseTask>("<BrokerResponseTask isComplete=\"false\" percentageComplete=\"0\"><FailureData><ResultCodeGroup>Plugin Error</ResultCodeGroup><ResultCode>702</ResultCode><ResultDescription>Request cancelled</ResultDescription></FailureData></BrokerResponseTask>");
						}
						else if (text != null)
						{
							response = Serializer.Deserialize<BrokerResponseTask>(text);
						}
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "PluginHostWrapper Plugin:{0}, Pipe:{1} TaskId:{3} : Exception while trying to send request. Plugin Path: {2}", new object[] { this.PluginName, this._pipeName, pluginInfo.PluginLocation, pluginInfo.TaskId });
						response = Serializer.Deserialize<BrokerResponseTask>("<BrokerResponseTask isComplete=\"false\" percentageComplete=\"0\"><FailureData><ResultCodeGroup>Plugin Host</ResultCodeGroup><ResultCode>606</ResultCode><ResultDescription>Error processing request: Exception while trying to send request.</ResultDescription></FailureData></BrokerResponseTask>");
					}
				}
			}
			return response;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000288C File Offset: 0x00000A8C
		private async Task<string> SendPluginRequest(string taskId, string requestXml, CancellationToken cancelToken)
		{
			this._finalResponses.TryAdd(taskId, string.Empty);
			try
			{
				bool retried = false;
				int retryCounter = 0;
				this._retryEvent.Reset();
				this._pipeClient.Send(requestXml);
				while (!cancelToken.IsCancellationRequested && !retried && string.IsNullOrEmpty(this._finalResponses[taskId]))
				{
					if (!retried && this._retryEvent.Wait(0))
					{
						retried = true;
						int num = retryCounter;
						retryCounter = num + 1;
						this._retryEvent.Reset();
						this._pipeClient.Send(requestXml);
					}
					else
					{
						await Task.Delay(100);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PluginHostWrapper Plugin:{0}, Pipe:{1}, taskId:{2}: Exception in SendPluginRequest while waiting for final response", new object[] { this.PluginName, this._pipeName, taskId });
			}
			string result;
			this._finalResponses.TryRemove(taskId, out result);
			return result;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000028EC File Offset: 0x00000AEC
		private void HandlePluginResponse(string responseXml)
		{
			if (Logger.IsLoggingEnabledForSeverity(Logger.LogSeverity.Information))
			{
				Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Received response from PluginHost (TRUNCATED): {2}", new object[]
				{
					this.PluginName,
					this._pipeName,
					responseXml.Substring(0, (responseXml.Length > 300) ? 300 : responseXml.Length) + "......"
				});
			}
			try
			{
				BrokerResponseTask brokerResponseTask = Serializer.Deserialize<BrokerResponseTask>(responseXml);
				BrokerResponse arg = BrokerResponseFactory.CreateNormalBrokerResponse(brokerResponseTask.StatusComment, brokerResponseTask);
				Guid arg2 = new Guid(brokerResponseTask.TaskId);
				if (this._responseHandler != null)
				{
					Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Calling response handler", new object[] { this.PluginName, this._pipeName });
					this._responseHandler(arg2, arg);
					Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Response handler returned", new object[] { this.PluginName, this._pipeName });
				}
				else
				{
					Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Response handler is null, will not invoke", new object[] { this.PluginName, this._pipeName });
				}
				if (brokerResponseTask.IsComplete || brokerResponseTask.Error != null)
				{
					Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Final broker response or error was received.", new object[] { this.PluginName, this._pipeName });
					this._finalResponses[brokerResponseTask.TaskId] = responseXml;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PluginHostWrapper Plugin:{0}, Pipe:{1}: Unable to handle response from plugin", new object[] { this.PluginName, this._pipeName });
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002A70 File Offset: 0x00000C70
		private bool StartPluginHostByProcessStart(string filePath, string cmdLine)
		{
			bool result;
			try
			{
				if (!Utility.SanitizePath(ref filePath))
				{
					Logger.Log(Logger.LogSeverity.Error, "StartPluginHostByProcessStart: Filepath to start pluginhost process is invalid: Path : {0} with args : {1}.", new object[] { filePath, cmdLine });
					return false;
				}
				if (!Utility.SanitizeArg(ref cmdLine))
				{
					Logger.Log(Logger.LogSeverity.Error, "StartPluginHostByProcessStart: Argument to start pluginhost process is invalid: Arguments : {0}.", new object[] { cmdLine });
				}
				Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper: Plugin: {0}. Starting PluginHost by Process.Start({1}, {2});", new object[] { this.PluginName, filePath, cmdLine });
				this._pluginHostProcess = Process.Start(filePath, cmdLine);
				Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper: Plugin: {0}. Start Plugin Host as System successfully. Process.hasExited={1}", new object[]
				{
					this.PluginName,
					this._pluginHostProcess.HasExited
				});
				result = true;
			}
			catch (ObjectDisposedException ex)
			{
				Logger.Log(ex, "PluginHostWrapper: Plugin: {0}. ObjectDisposedException while running Plugin Host: The process object has already been disposed.", new object[] { this.PluginName });
				result = false;
			}
			catch (InvalidOperationException ex2)
			{
				Logger.Log(ex2, "PluginHostWrapper: Plugin: {0}. InvalidOperationException while running Plugin Host: The fileName or arguments parameter is null.", new object[] { this.PluginName });
				result = false;
			}
			catch (Win32Exception ex3)
			{
				Logger.Log(ex3, "PluginHostWrapper: Plugin: {0}. Win32Exception while running Plugin Host", new object[] { this.PluginName });
				result = false;
			}
			catch (FileNotFoundException ex4)
			{
				Logger.Log(ex4, "PluginHostWrapper: Plugin: {0}. FileNotFoundException while running Plugin Host", new object[] { this.PluginName });
				result = false;
			}
			return result;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002BD0 File Offset: 0x00000DD0
		private bool StartPluginHostAsSystem()
		{
			Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper: Plugin: {0}. Starting Plugin Host as System. Pipe name: {1}", new object[] { this.PluginName, this._pipeName });
			Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper: Plugin: {0}. Plugin Host bitness: {1}", new object[]
			{
				this.PluginName,
				this.Bit.ToString()
			});
			bool result = false;
			string cmdLine = string.Concat(new string[] { "-name ", this._pipeName, " -runas SYSTEM -pluginName ", this.PluginName, " -pluginVersion ", this.PluginVersion });
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper: Plugin: {0}. Retrieving Plugin Host path...", new object[] { this.PluginName });
				string text = InstallationLocator.GetPluginHostInstallationLocation(this.Bit);
				Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper: Plugin: {0}. Plugin Host path retrieved as: {1}", new object[] { this.PluginName, text });
				text = this.GetAssociatedPluginHostFile(text);
				result = this.StartPluginHostByProcessStart(text, cmdLine);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PluginHostWrapper: Plugin: {0}. Cannot Start Plugin Host as System", new object[] { this.PluginName });
			}
			return result;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002CF8 File Offset: 0x00000EF8
		private bool StartPluginHostAsUser()
		{
			Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper: Plugin: {0}. Starting Plugin Host as User. Pipe name: {1}", new object[] { this.PluginName, this._pipeName });
			Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper: Plugin: {0}. Plugin Host bitness: {1}", new object[]
			{
				this.PluginName,
				this.Bit.ToString()
			});
			bool result = false;
			string text = string.Concat(new string[] { "-name ", this._pipeName, " -runas -pluginName ", this.PluginName, " -pluginVersion ", this.PluginVersion });
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper: Plugin: {0}. Retrieving Plugin Host path...", new object[] { this.PluginName });
				string text2 = InstallationLocator.GetPluginHostInstallationLocation(this.Bit);
				text2 = this.GetAssociatedPluginHostFile(text2);
				Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper: Plugin: {0}. Plugin Host path retrieved as: {1}", new object[] { this.PluginName, text2 });
				Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper: Plugin: {0}. Starting PluginHost by _systemProcessLauncher.LaunchUserProcess({1}, {2});", new object[] { this.PluginName, text2, text });
				this._systemProcessLauncher = SystemProcessLauncher.Instance;
				int? num = this._systemProcessLauncher.LaunchUserProcess(text2, text, ".", false);
				if (num != null)
				{
					Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper: Plugin: {0}. Plugin Host process ID: {1}", new object[] { this.PluginName, num.Value });
					try
					{
						this._pluginHostProcess = Process.GetProcessById(num.Value);
						if (this._pluginHostProcess != null)
						{
							result = true;
						}
						Logger.Log(Logger.LogSeverity.Information, "PluginHostWrapper: Plugin: {0}. Start Plugin Host as User successfully, pipe name: {1}", new object[] { this.PluginName, this._pipeName });
					}
					catch (ArgumentException)
					{
						Logger.Log(Logger.LogSeverity.Error, "PluginHostWrapper: Plugin: {0}. ArgumentException while running Plugin Host: The process specified by the processId parameter is not running. The identifier might be expired.", new object[] { this.PluginName });
						result = false;
					}
					catch (InvalidOperationException)
					{
						Logger.Log(Logger.LogSeverity.Error, "PluginHostWrapper: Plugin: {0}. InvalidOperationException while running Plugin Host: The process was not started by this object.", new object[] { this.PluginName });
						result = false;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PluginHostWrapper: Plugin: {0}. Cannot Start Plugin Host as User", new object[] { this.PluginName });
				result = false;
			}
			return result;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002F48 File Offset: 0x00001148
		private string GetAssociatedPluginHostFile(string pluginHostPath)
		{
			string text = pluginHostPath;
			try
			{
				string text2 = "";
				if (this._deviceAssociation)
				{
					text2 = Constants.DeviceAssociationSuffix;
				}
				else if (this._uapAssociation)
				{
					if (this._associatedUapId.ToLower().Contains(Constants.SettingAssociationUapIdPartial))
					{
						text2 = Constants.SettingsAssociationSuffix;
					}
					else if (this._associatedUapId.ToLower().Contains(Constants.CompanionAssociationUapIdPartial))
					{
						text2 = Constants.CompanionAssociationSuffix;
					}
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text = pluginHostPath.Remove(pluginHostPath.Length - 4);
					text = text + "." + text2 + ".exe";
				}
				if (!File.Exists(text))
				{
					text = pluginHostPath;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "GetPluginHostFileForMappedAssociation: Exception occured. Falling back to use original executable");
				text = pluginHostPath;
			}
			return text;
		}

		// Token: 0x0400000C RID: 12
		private string _pipeName;

		// Token: 0x0400000D RID: 13
		private EventWaitHandle _exitEvent;

		// Token: 0x0400000E RID: 14
		private Process _pluginHostProcess;

		// Token: 0x0400000F RID: 15
		private Func<Guid, BrokerResponse, bool> _responseHandler;

		// Token: 0x04000010 RID: 16
		private static readonly int _pluginHostExitTimeoutMS = 15000;

		// Token: 0x04000011 RID: 17
		private NamedPipeClient _pipeClient;

		// Token: 0x04000012 RID: 18
		private NamedPipeServer _pipeServer;

		// Token: 0x04000013 RID: 19
		private ConcurrentDictionary<string, string> _finalResponses;

		// Token: 0x04000014 RID: 20
		private ManualResetEventSlim _retryEvent;

		// Token: 0x04000015 RID: 21
		private bool _deviceAssociation;

		// Token: 0x04000016 RID: 22
		private bool _uapAssociation;

		// Token: 0x04000017 RID: 23
		private string _associatedUapId;

		// Token: 0x04000018 RID: 24
		private IProcessLauncher _systemProcessLauncher;
	}
}
