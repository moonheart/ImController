using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Lenovo.Tools.Logging
{
	// Token: 0x02000014 RID: 20
	internal sealed class TextFileLogger : ILogger
	{
		// Token: 0x0600008C RID: 140 RVA: 0x000059E0 File Offset: 0x00003BE0
		public TextFileLogger(string pathToLogDirectory, string logFilePrefix)
		{
			if (!Directory.Exists(pathToLogDirectory))
			{
				Directory.CreateDirectory(pathToLogDirectory);
			}
			string arg = logFilePrefix ?? Process.GetCurrentProcess().ProcessName;
			TextFileLogger.LogFilePath = Path.Combine(pathToLogDirectory, string.Format("Log.{0}.{1}.log", arg, DateTime.Now.ToString("yy_MM_dd-hh-mm_ss")));
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005A3C File Offset: 0x00003C3C
		public void Log(Logger.LogSeverity severity, string message)
		{
			try
			{
				string text = string.Format("{0}\t\t{1}\t\t{2}", severity.ToString(), DateTime.Now.ToString("hh:mm:ss.fff"), message);
				this.Log(text);
			}
			catch
			{
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005A90 File Offset: 0x00003C90
		private void Log(string text)
		{
			if (text != null)
			{
				try
				{
					TextFileLogger._writeSemaphore.WaitOne();
					if (!File.Exists(TextFileLogger.LogFilePath))
					{
						using (StreamWriter streamWriter = File.CreateText(TextFileLogger.LogFilePath))
						{
							streamWriter.WriteLine(text);
							streamWriter.Close();
							goto IL_62;
						}
					}
					using (StreamWriter streamWriter2 = File.AppendText(TextFileLogger.LogFilePath))
					{
						streamWriter2.WriteLine(text);
						streamWriter2.Close();
					}
					IL_62:;
				}
				catch (Exception)
				{
				}
				finally
				{
					TextFileLogger._writeSemaphore.Release();
				}
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00005B44 File Offset: 0x00003D44
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

		// Token: 0x04000069 RID: 105
		private static string LogFilePath = null;

		// Token: 0x0400006A RID: 106
		private static Semaphore _writeSemaphore = new Semaphore(1, 1);
	}
}
