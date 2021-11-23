using System;
using System.Threading;
using Lenovo.Modern.Utilities.Services.Logging;
using UDC.ClientBrokerAgent;

namespace Lenovo.Modern.ImController.ContractBroker.Services
{
	// Token: 0x0200000F RID: 15
	internal sealed class CBAWrapper
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00003860 File Offset: 0x00001A60
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00003868 File Offset: 0x00001A68
		public ClientBrokerAgent Agent { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00003871 File Offset: 0x00001A71
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00003879 File Offset: 0x00001A79
		public bool Connected { get; private set; }

		// Token: 0x06000094 RID: 148 RVA: 0x00003882 File Offset: 0x00001A82
		public CBAWrapper()
		{
			this.Agent = new ClientBrokerAgent();
			this.Connect();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000389C File Offset: 0x00001A9C
		~CBAWrapper()
		{
			if (!this._disposed)
			{
				this.Dispose();
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000038D0 File Offset: 0x00001AD0
		public void Dispose()
		{
			this._disposed = true;
			try
			{
				this.Agent.Stop();
			}
			catch (Exception ex)
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex, "Exception in CBAWrapper.Dispose.Agent.Stop().");
			}
			try
			{
				this.Agent.Dispose();
			}
			catch (Exception ex2)
			{
				Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex2, "Exception in CBAWrapper.Dispose.Agent.Dispose().");
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003934 File Offset: 0x00001B34
		private bool Connect()
		{
			object syncRoot = CBAWrapper._syncRoot;
			lock (syncRoot)
			{
				if (!this.Connected)
				{
					using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(30000))
					{
						try
						{
							this.Connected = this.Agent.ConnectToUDC(cancellationTokenSource.Token, ref this._lastErrorCode);
						}
						catch (Exception ex)
						{
							Lenovo.Modern.Utilities.Services.Logging.Logger.Log(ex, "Exception in CBAWrapper.Connect().");
						}
					}
				}
			}
			return this.Connected;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000039D4 File Offset: 0x00001BD4
		public int GetLastErrorCode()
		{
			return this._lastErrorCode;
		}

		// Token: 0x04000048 RID: 72
		private int _lastErrorCode;

		// Token: 0x04000049 RID: 73
		private bool _disposed;

		// Token: 0x0400004A RID: 74
		private static readonly object _syncRoot = new object();
	}
}
