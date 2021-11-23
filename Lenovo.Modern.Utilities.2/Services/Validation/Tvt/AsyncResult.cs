using System;
using System.Threading;

namespace Lenovo.Modern.Utilities.Services.Validation.Tvt
{
	// Token: 0x0200001F RID: 31
	public sealed class AsyncResult : IAsyncResult
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x0000381C File Offset: 0x00001A1C
		public AsyncResult(object state)
		{
			this.m_AsyncState = state;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000382B File Offset: 0x00001A2B
		public object AsyncState
		{
			get
			{
				return this.m_AsyncState;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00003834 File Offset: 0x00001A34
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				if (this.m_AsyncWaitHandle == null)
				{
					bool isCompleted = this.IsCompleted;
					ManualResetEvent manualResetEvent = new ManualResetEvent(isCompleted);
					if (Interlocked.CompareExchange<ManualResetEvent>(ref this.m_AsyncWaitHandle, manualResetEvent, null) != null)
					{
						manualResetEvent.Close();
					}
					else if (!isCompleted && this.IsCompleted)
					{
						this.m_AsyncWaitHandle.Set();
					}
				}
				return this.m_AsyncWaitHandle;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x0000388B File Offset: 0x00001A8B
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00003893 File Offset: 0x00001A93
		public bool CompletedSynchronously { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000389C File Offset: 0x00001A9C
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x000038A4 File Offset: 0x00001AA4
		public bool IsCompleted { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000038AD File Offset: 0x00001AAD
		// (set) Token: 0x060000AA RID: 170 RVA: 0x000038B5 File Offset: 0x00001AB5
		public TrustStatus Result { get; internal set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000038BE File Offset: 0x00001ABE
		// (set) Token: 0x060000AC RID: 172 RVA: 0x000038C6 File Offset: 0x00001AC6
		public uint WinVerifyTrustStatus { get; internal set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000038CF File Offset: 0x00001ACF
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000038D7 File Offset: 0x00001AD7
		public bool OldCertificateUsedForValidation { get; internal set; }

		// Token: 0x0400001F RID: 31
		private ManualResetEvent m_AsyncWaitHandle;

		// Token: 0x04000020 RID: 32
		private readonly object m_AsyncState;
	}
}
