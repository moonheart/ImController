using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Lenovo.Modern.Utilities.Services.Logging
{
	// Token: 0x02000034 RID: 52
	public class TextFileLogger : ILogger
	{
		// Token: 0x0600014B RID: 331 RVA: 0x00006B94 File Offset: 0x00004D94
		public TextFileLogger(string pathToLogDirectory, string logFilePrefix, int maxSizeKb)
		{
			if (!Directory.Exists(pathToLogDirectory))
			{
				Directory.CreateDirectory(pathToLogDirectory);
			}
			this._pathToLogDirectory = pathToLogDirectory;
			this._logFilePrefix = logFilePrefix ?? Process.GetCurrentProcess().ProcessName;
			if (maxSizeKb <= 1)
			{
				maxSizeKb = 10240;
			}
			this._logRolloverKb = maxSizeKb;
			this._logCreationTime = DateTime.Now.ToString("yy-MM-dd_hh-mm-ss-fff", CultureInfo.InvariantCulture);
			this.LogFilePath = this.GenerateLogFilepath();
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00006C1C File Offset: 0x00004E1C
		public void Log(Logger.LogSeverity severity, string message)
		{
			try
			{
				string text = string.Format("{0}\t\t{1}\t\t{2}", severity.ToString(), DateTime.Now.ToString("hh:mm:ss.fff"), message);
				this.Log(text);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00006C70 File Offset: 0x00004E70
		private void Log(string text)
		{
			if (text != null)
			{
				try
				{
					this._writeSemaphore.WaitOne();
					FileInfo fileInfo = new FileInfo(this.LogFilePath);
					if (fileInfo.Exists && fileInfo.Length > (long)(this._logRolloverKb * 1024))
					{
						this._rolloverCount++;
						this.LogFilePath = this.GenerateLogFilepath();
					}
					using (StreamWriter streamWriter = File.AppendText(this.LogFilePath))
					{
						streamWriter.WriteLine(text);
						streamWriter.Close();
					}
				}
				catch (Exception)
				{
				}
				finally
				{
					this._writeSemaphore.Release();
				}
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00006D34 File Offset: 0x00004F34
		public void Log(Exception ex, string message)
		{
			string text = string.Format("{0}\t\t{1}\t\t{2}\t\t{3}\r\n\t\t\t\t\t\t\t\t{4}", new object[]
			{
				"Exception",
				DateTime.Now.ToString("hh:mm:ss"),
				ex.GetType(),
				message,
				ex.Message
			});
			this.Log(text);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00006D8C File Offset: 0x00004F8C
		private string GenerateLogFilepath()
		{
			string arg = ((this._rolloverCount > 0) ? ("(" + (this._rolloverCount + 1) + " )") : string.Empty);
			return Path.Combine(this._pathToLogDirectory, string.Format("Log.{0}_{1}{2}.log", this._logFilePrefix, this._logCreationTime, arg));
		}

		// Token: 0x0400005D RID: 93
		private string LogFilePath;

		// Token: 0x0400005E RID: 94
		private readonly string _pathToLogDirectory;

		// Token: 0x0400005F RID: 95
		private readonly string _logFilePrefix;

		// Token: 0x04000060 RID: 96
		private readonly string _logCreationTime;

		// Token: 0x04000061 RID: 97
		private readonly int _logRolloverKb;

		// Token: 0x04000062 RID: 98
		private int _rolloverCount;

		// Token: 0x04000063 RID: 99
		private Semaphore _writeSemaphore = new Semaphore(1, 1);
	}
}
