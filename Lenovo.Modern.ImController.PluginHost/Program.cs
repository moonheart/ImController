using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using Lenovo.Modern.ImController.PluginHost.Services;
using Lenovo.Modern.ImController.PluginHost.Utilities;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.PluginHost
{
	// Token: 0x02000003 RID: 3
	internal class Program
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		private static void Main(string[] args)
		{
			string fileNameEnding = null;
			try
			{
				if (Environment.CommandLine != null)
				{
					InputArguments inputArguments = new InputArguments(Environment.CommandLine);
					if (inputArguments != null && inputArguments.Contains("pluginName"))
					{
						fileNameEnding = inputArguments["pluginName"];
					}
				}
			}
			catch (Exception)
			{
			}
			Logger.Setup(new Logger.Configuration
			{
				LogIdentifier = "ImController.PluginHost",
				FileNameEnding = fileNameEnding,
				FileSizeRollOverKb = 5120,
				IsEnabled = new bool?(true)
			});
			string text = "Unknown";
			string text2 = "Unknown";
			try
			{
				text = new ProcessPrivilegeDetector().GetCurrentProcessPrivilege().ToString();
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				string text3;
				if (executingAssembly == null)
				{
					text3 = null;
				}
				else
				{
					AssemblyName name = executingAssembly.GetName();
					text3 = ((name != null) ? name.CodeBase : null);
				}
				text2 = text3;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unhandled exception while getting privilege and name of current process");
			}
			Logger.Log(Logger.LogSeverity.Information, "Full command line: " + Environment.CommandLine);
			Logger.Log(Logger.LogSeverity.Information, "Bitness: {0}, running as: {1}", new object[]
			{
				Environment.Is64BitProcess ? "64 bit" : "32 bit",
				text
			});
			Logger.Log(Logger.LogSeverity.Information, "PluginHostLocation: {0}", new object[] { text2 });
			try
			{
				InputArguments inputArguments2 = null;
				if (Environment.CommandLine != null)
				{
					inputArguments2 = new InputArguments(Environment.CommandLine);
				}
				if (inputArguments2 == null)
				{
					throw new ArgumentNullException("No arguments supplied");
				}
				Logger.Log(Logger.LogSeverity.Information, "Supplied " + inputArguments2.Count() + " arguments");
				foreach (string message in args)
				{
					Logger.Log(Logger.LogSeverity.Information, message);
				}
				if (inputArguments2.Count() != 0)
				{
					if (string.IsNullOrWhiteSpace(inputArguments2["name"]))
					{
						throw new ArgumentNullException("Name parameter must be specified");
					}
					string text4 = inputArguments2["name"];
					InstanceEnforcer instanceEnforcer = new InstanceEnforcer();
					NamedPipeResponder ipcResponder = new NamedPipeResponder();
					HostRunner hostRunner = new HostRunner(instanceEnforcer, ipcResponder);
					Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs eventArgs)
					{
						Logger.Log(Logger.LogSeverity.Information, "Cancel command activated.  Exiting...");
						eventArgs.Cancel = true;
						hostRunner.Close();
					};
					hostRunner.RunAsync(inputArguments2);
					Logger.Log(Logger.LogSeverity.Information, "Plugin Host is up and running. Pipe name: " + text4);
					Program.WaitForExitEventFromManager(text4);
					hostRunner.Close();
					Logger.Log(Logger.LogSeverity.Information, "Plugin Host has exited");
				}
			}
			catch (Exception ex2)
			{
				Logger.Log(ex2, "Unhandled exception while initializing host");
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000022D0 File Offset: 0x000004D0
		private static void WaitForExitEventFromManager(string name)
		{
			EventWaitHandleAccessRule rule = new EventWaitHandleAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), EventWaitHandleRights.FullControl, AccessControlType.Allow);
			EventWaitHandleSecurity eventWaitHandleSecurity = new EventWaitHandleSecurity();
			eventWaitHandleSecurity.AddAccessRule(rule);
			string text = "Global\\evt_" + name;
			Logger.Log(Logger.LogSeverity.Information, "Waiting for Plugin Host Wrapper to set our exit event, event name: " + text);
			bool flag = false;
			using (EventWaitHandle eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, text, ref flag, eventWaitHandleSecurity))
			{
				while (!eventWaitHandle.WaitOne(30000))
				{
					if (Process.GetCurrentProcess().PrivateMemorySize64 > 524288000L)
					{
						Logger.Log(Logger.LogSeverity.Information, "Plugin Host memory ({0}) reached the maximum limit. Exiting", new object[] { Process.GetCurrentProcess().PrivateMemorySize64 });
						break;
					}
				}
				Logger.Log(Logger.LogSeverity.Information, "Plugin Host exit event was set, exiting. Event name: " + text);
			}
		}

		// Token: 0x04000001 RID: 1
		private const int PROCESS_MEMORY_MONITORING_DELAY_MS = 30000;

		// Token: 0x04000002 RID: 2
		private const int MAX_ALLOWED_MEMORY_SIZE = 524288000;
	}
}
