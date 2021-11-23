using System;
using System.Diagnostics.Eventing;
using Lenovo.ImController.EventLogging.Services.Repositories.Event;

namespace Lenovo.ImController.EventLogging.Services.Provider
{
	// Token: 0x02000011 RID: 17
	public class EventSource : IDisposable
	{
		// Token: 0x06000028 RID: 40 RVA: 0x00002990 File Offset: 0x00000B90
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.m_provider.Dispose();
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000029A0 File Offset: 0x00000BA0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000029B0 File Offset: 0x00000BB0
		public EventSource(WindowsEventLogRepository.EventLogConfig config)
		{
			this.m_provider = new TelemetryEventProvider(config.ProviderId);
			this.SampleEvt_UnicodeString = new EventDescriptor(config.EventConfig.EventId, config.EventConfig.Version, config.EventConfig.Channel, config.EventConfig.Level, config.EventConfig.OpCode, config.EventConfig.Task, config.EventConfig.Keywords);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A2C File Offset: 0x00000C2C
		public bool EventWriteSampleEvt_UnicodeString(string Prop_UnicodeString)
		{
			return this.m_provider.WriteEvent(ref this.SampleEvt_UnicodeString, Prop_UnicodeString);
		}

		// Token: 0x04000014 RID: 20
		internal TelemetryEventProvider m_provider;

		// Token: 0x04000015 RID: 21
		protected EventDescriptor SampleEvt_UnicodeString;
	}
}
