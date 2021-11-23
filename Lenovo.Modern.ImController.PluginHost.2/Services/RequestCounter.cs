using System;
using System.Diagnostics;

namespace Lenovo.Modern.ImController.PluginHost.Services
{
	// Token: 0x0200000B RID: 11
	public class RequestCounter
	{
		// Token: 0x0600002E RID: 46 RVA: 0x00003555 File Offset: 0x00001755
		public RequestCounter()
		{
			this._timer = new Stopwatch();
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00003568 File Offset: 0x00001768
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00003570 File Offset: 0x00001770
		public int ResponseCount { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00003579 File Offset: 0x00001779
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00003581 File Offset: 0x00001781
		public int DuplicateCount { get; private set; }

		// Token: 0x06000033 RID: 51 RVA: 0x0000358C File Offset: 0x0000178C
		public void IncrementResponseCount()
		{
			if (!this._timer.IsRunning)
			{
				this._timer.Start();
			}
			else
			{
				this.ResetIfNecessary();
			}
			int responseCount = this.ResponseCount;
			this.ResponseCount = responseCount + 1;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000035C9 File Offset: 0x000017C9
		public bool HasSurpassedThreshold()
		{
			return this.ResponseCount > 100;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000035D5 File Offset: 0x000017D5
		public void Reset()
		{
			this._timer.Restart();
			this.ResponseCount = 0;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000035EC File Offset: 0x000017EC
		public void ResetIfNecessary()
		{
			if (this._timer.Elapsed.TotalSeconds > 60.0)
			{
				this.Reset();
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003620 File Offset: 0x00001820
		public bool IsResponseDuplicateOfLastResponse(string thisResponse)
		{
			if (string.IsNullOrWhiteSpace(this._lastResponse) || string.IsNullOrWhiteSpace(thisResponse))
			{
				return false;
			}
			bool flag = this._lastResponse.Equals(thisResponse, StringComparison.InvariantCultureIgnoreCase);
			if (flag)
			{
				int duplicateCount = this.DuplicateCount;
				this.DuplicateCount = duplicateCount + 1;
			}
			this._lastResponse = thisResponse;
			return flag;
		}

		// Token: 0x0400001E RID: 30
		public const int MaxRequestsPerPeriod = 100;

		// Token: 0x0400001F RID: 31
		public const int PeriodSeconds = 60;

		// Token: 0x04000020 RID: 32
		private readonly Stopwatch _timer;

		// Token: 0x04000021 RID: 33
		private string _lastResponse;
	}
}
