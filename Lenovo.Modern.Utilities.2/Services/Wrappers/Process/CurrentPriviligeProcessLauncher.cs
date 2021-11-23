using System;
using System.Diagnostics;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Utilities.Services.Wrappers.Process
{
	// Token: 0x02000018 RID: 24
	public class CurrentPriviligeProcessLauncher : IProcessLauncher
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000334E File Offset: 0x0000154E
		public static IProcessLauncher Instance
		{
			get
			{
				if (CurrentPriviligeProcessLauncher.instance == null)
				{
					CurrentPriviligeProcessLauncher.instance = new CurrentPriviligeProcessLauncher();
				}
				return CurrentPriviligeProcessLauncher.instance;
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003368 File Offset: 0x00001568
		int? IProcessLauncher.LaunchUserProcess(string appPath, string cmdLine, string workDir, bool visible)
		{
			int? result = null;
			try
			{
				Process process = Process.Start(appPath, cmdLine);
				process.WaitForInputIdle();
				result = new int?(process.Id);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Failed to launch the process");
			}
			return result;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000033B8 File Offset: 0x000015B8
		int? IProcessLauncher.LaunchSystemProcessInUserSession(string appPath, string cmdLine, string workDir, bool visible)
		{
			int? result = null;
			try
			{
				Process process = Process.Start(appPath, cmdLine);
				process.WaitForInputIdle();
				result = new int?(process.Id);
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Failed to launch the process");
			}
			return result;
		}

		// Token: 0x04000019 RID: 25
		private static IProcessLauncher instance;
	}
}
