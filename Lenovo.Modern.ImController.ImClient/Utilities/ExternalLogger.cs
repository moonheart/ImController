using System;

namespace Lenovo.Modern.ImController.ImClient.Utilities
{
	// Token: 0x02000002 RID: 2
	public class ExternalLogger
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void Configure(Action<string> logAction)
		{
			if (ExternalLogger.IsConfigured())
			{
				throw new InvalidOperationException("Can only be configured once");
			}
			ExternalLogger._configuredInstance = new ExternalLogger(logAction);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000206F File Offset: 0x0000026F
		public static bool IsConfigured()
		{
			return ExternalLogger._configuredInstance != null;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002079 File Offset: 0x00000279
		private ExternalLogger(Action<string> logAction)
		{
			if (logAction == null)
			{
				throw new ArgumentNullException("Must provide a log action");
			}
			this._logAction = logAction;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002096 File Offset: 0x00000296
		private ExternalLogger()
		{
			this._logAction = null;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020A5 File Offset: 0x000002A5
		internal static ExternalLogger Instance
		{
			get
			{
				if (ExternalLogger._configuredInstance != null)
				{
					return ExternalLogger._configuredInstance;
				}
				ExternalLogger result;
				if ((result = ExternalLogger._voidLogger) == null)
				{
					result = (ExternalLogger._voidLogger = new ExternalLogger());
				}
				return result;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020C8 File Offset: 0x000002C8
		internal void Log(ExternalLogger.LogSeverity severity, string text)
		{
			if (this._logAction != null)
			{
				try
				{
					this._logAction(severity.ToString() + "   " + text);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002118 File Offset: 0x00000318
		internal void Log(ExternalLogger.LogSeverity severity, string format, params object[] args)
		{
			this.Log(severity, string.Format(format, args));
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002128 File Offset: 0x00000328
		internal void Log(Exception ex, string format, params object[] args)
		{
			this.Log(ex, string.Format(format, args));
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002138 File Offset: 0x00000338
		internal void Log(Exception ex, string text)
		{
			if (this._logAction != null)
			{
				try
				{
					this._logAction("Exception  " + ex.Message + "  " + text);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x04000001 RID: 1
		private readonly Action<string> _logAction;

		// Token: 0x04000002 RID: 2
		private static ExternalLogger _configuredInstance;

		// Token: 0x04000003 RID: 3
		private static ExternalLogger _voidLogger;

		// Token: 0x02000038 RID: 56
		public enum LogSeverity
		{
			// Token: 0x040000B7 RID: 183
			Information,
			// Token: 0x040000B8 RID: 184
			Warning,
			// Token: 0x040000B9 RID: 185
			Error
		}
	}
}
