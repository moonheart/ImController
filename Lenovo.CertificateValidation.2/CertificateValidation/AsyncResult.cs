using System;
using System.Threading;

namespace Lenovo.CertificateValidation
{
	// Token: 0x02000004 RID: 4
	public sealed class AsyncResult : IAsyncResult
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000025CF File Offset: 0x000007CF
		public AsyncResult(object state)
		{
			this.m_AsyncState = state;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000025DE File Offset: 0x000007DE
		public object AsyncState
		{
			get
			{
				return this.m_AsyncState;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000025E8 File Offset: 0x000007E8
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

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000263F File Offset: 0x0000083F
		// (set) Token: 0x06000010 RID: 16 RVA: 0x00002647 File Offset: 0x00000847
		public bool CompletedSynchronously { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002650 File Offset: 0x00000850
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002658 File Offset: 0x00000858
		public bool IsCompleted { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002661 File Offset: 0x00000861
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002669 File Offset: 0x00000869
		public TrustStatus Result { get; internal set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002672 File Offset: 0x00000872
		// (set) Token: 0x06000016 RID: 22 RVA: 0x0000267A File Offset: 0x0000087A
		public uint WinVerifyTrustStatus { get; internal set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002683 File Offset: 0x00000883
		// (set) Token: 0x06000018 RID: 24 RVA: 0x0000268B File Offset: 0x0000088B
		public bool OldCertificateUsedForValidation { get; internal set; }

		// Token: 0x04000002 RID: 2
		private ManualResetEvent m_AsyncWaitHandle;

		// Token: 0x04000003 RID: 3
		private readonly object m_AsyncState;
	}
}
