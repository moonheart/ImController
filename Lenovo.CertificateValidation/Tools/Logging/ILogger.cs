using System;

namespace Lenovo.Tools.Logging
{
	// Token: 0x02000012 RID: 18
	internal interface ILogger
	{
		// Token: 0x0600007D RID: 125
		void Log(Logger.LogSeverity severity, string message);

		// Token: 0x0600007E RID: 126
		void Log(Exception ex, string message);
	}
}
