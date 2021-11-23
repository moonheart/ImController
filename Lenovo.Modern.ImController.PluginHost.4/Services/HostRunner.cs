using System;
using Lenovo.Modern.ImController.PluginHost.Utilities;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.PluginHost.Services
{
	// Token: 0x02000005 RID: 5
	public class HostRunner
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000026BA File Offset: 0x000008BA
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000026C2 File Offset: 0x000008C2
		internal string Name { get; private set; }

		// Token: 0x06000015 RID: 21 RVA: 0x000026CB File Offset: 0x000008CB
		public HostRunner(IInstanceEnforcer instanceEnforcer, IIpcResponder ipcResponder)
		{
			this._instanceEnforcer = instanceEnforcer;
			this._ipcResponder = ipcResponder;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000026E1 File Offset: 0x000008E1
		public void Close()
		{
			Logger.Log(Logger.LogSeverity.Information, "Closing Host");
			this._ipcResponder.Close();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026FC File Offset: 0x000008FC
		public bool RunAsync(InputArguments arguments)
		{
			bool result = false;
			if (this._instanceEnforcer == null || this._ipcResponder == null)
			{
				throw new MissingMemberException("Missing member for HostRunner:Run");
			}
			if (arguments == null || !arguments.Contains("name"))
			{
				throw new ArgumentNullException("Missing arguments to run");
			}
			this.Name = arguments["name"];
			Logger.Log(Logger.LogSeverity.Information, "Running as: {0}", new object[] { this.Name });
			try
			{
				this._instanceEnforcer.AssertOnlyInstance(this.Name);
				this.ListenAndRespondToIpcCommandsAsync();
			}
			catch (InstanceEnforcer.MultipleInstanceException)
			{
				Logger.Log(Logger.LogSeverity.Error, "An instance is already running with the same name");
			}
			catch (AggregateException ex)
			{
				Logger.Log(Logger.LogSeverity.Warning, "Host primary task canceled, {0}, {1}", new object[]
				{
					ex.GetType(),
					ex.Message
				});
			}
			catch (Exception ex2)
			{
				result = true;
				Logger.Log(ex2, "Unhandeled exception for primary task. {0}, {1}", new object[]
				{
					ex2.GetType(),
					ex2.Message
				});
			}
			return result;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000280C File Offset: 0x00000A0C
		private void ListenAndRespondToIpcCommandsAsync()
		{
			if (this._ipcResponder != null)
			{
				this._ipcResponder.BeginWaitingForRequests(this.Name);
			}
		}

		// Token: 0x04000006 RID: 6
		private readonly IInstanceEnforcer _instanceEnforcer;

		// Token: 0x04000007 RID: 7
		private readonly IIpcResponder _ipcResponder;
	}
}
