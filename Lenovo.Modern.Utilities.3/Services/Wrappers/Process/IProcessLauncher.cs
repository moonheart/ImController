using System;

namespace Lenovo.Modern.Utilities.Services.Wrappers.Process
{
	// Token: 0x02000019 RID: 25
	public interface IProcessLauncher
	{
		// Token: 0x06000082 RID: 130
		int? LaunchUserProcess(string appPath, string cmdLine, string workDir, bool visible);

		// Token: 0x06000083 RID: 131
		int? LaunchSystemProcessInUserSession(string appPath, string cmdLine, string workDir, bool visible);
	}
}
