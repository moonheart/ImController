using System;

namespace Lenovo.Modern.Utilities.Services.Logging
{
	// Token: 0x02000035 RID: 53
	public interface ILogger
	{
		// Token: 0x06000150 RID: 336
		void Log(Logger.LogSeverity severity, string message);

		// Token: 0x06000151 RID: 337
		void Log(Exception ex, string message);
	}
}
