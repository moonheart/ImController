using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Lenovo.ImController.EventLogging.Model;
using Lenovo.ImController.EventLogging.Services.Provider;
using Lenovo.ImController.EventLogging.Utilities;

namespace Lenovo.ImController.EventLogging.Services.Repositories.Event
{
	// Token: 0x02000010 RID: 16
	public class WindowsEventLogRepository : IEventRepository, IWriteableEventRepository, IReadableEventRepository
	{
		// Token: 0x06000023 RID: 35 RVA: 0x000027E6 File Offset: 0x000009E6
		public WindowsEventLogRepository(WindowsEventLogRepository.EventLogConfig config)
		{
			this.Configuration = config;
			this._mEventSource = new EventSource(this.Configuration);
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000024 RID: 36 RVA: 0x0000280D File Offset: 0x00000A0D
		private WindowsEventLogRepository.EventLogConfig Configuration { get; }

		// Token: 0x06000025 RID: 37 RVA: 0x00002818 File Offset: 0x00000A18
		void IWriteableEventRepository.LogEvent(StorableEvent userEvent)
		{
			string prop_UnicodeString = Serializer.Serialize<StorableEvent>(userEvent);
			this._mEventSource.EventWriteSampleEvt_UnicodeString(prop_UnicodeString);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002839 File Offset: 0x00000A39
		List<EventLogItem> IReadableEventRepository.GetNextListOfEventsAfterDate(string userSid, DateTime lastTrasmittedTimeStamp, string providerName, int numberOfEvents)
		{
			return this.GetListOfEventRecordsForUser(userSid, lastTrasmittedTimeStamp, this.level, providerName, numberOfEvents);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000284C File Offset: 0x00000A4C
		private List<EventLogItem> GetListOfEventRecordsForUser(string userSid, DateTime lastTrasmittedTimeStamp, int level, string providerName, int maxNumberOfEvents)
		{
			List<EventLogItem> list = new List<EventLogItem>();
			if (string.IsNullOrWhiteSpace(userSid))
			{
				return list;
			}
			string path = providerName + "/Operational";
			string query = string.Format("*[System[(Level = {0}) and Provider[@Name = '{1}'] and TimeCreated[@SystemTime > '{2}']]]", level, providerName, lastTrasmittedTimeStamp.ToUniversalTime().ToString("o"));
			using (EventLogReader eventLogReader = new EventLogReader(new EventLogQuery(path, PathType.LogName, query)))
			{
				int i = 0;
				while (i < maxNumberOfEvents)
				{
					try
					{
						using (EventRecord eventRecord = eventLogReader.ReadEvent())
						{
							if (eventRecord == null || eventRecord.Properties == null || !eventRecord.Properties.Any<EventProperty>())
							{
								break;
							}
							EventProperty eventProperty = eventRecord.Properties[0];
							string xml;
							if (eventProperty == null)
							{
								xml = null;
							}
							else
							{
								object value = eventProperty.Value;
								xml = ((value != null) ? value.ToString() : null);
							}
							StorableEvent storableEvent = Serializer.Deserialize<StorableEvent>(xml);
							if (storableEvent != null && userSid.Equals(storableEvent.UserSID, StringComparison.InvariantCultureIgnoreCase))
							{
								list.Add(new EventLogItem(eventRecord.TimeCreated.GetValueOrDefault(), storableEvent));
								i++;
							}
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return list;
		}

		// Token: 0x04000010 RID: 16
		private EventSource _mEventSource;

		// Token: 0x04000011 RID: 17
		private readonly int level = 4;

		// Token: 0x04000012 RID: 18
		private const string EVENt_LOG_QUERY = "*[System[(Level = {0}) and Provider[@Name = '{1}'] and TimeCreated[@SystemTime > '{2}']]]";

		// Token: 0x02000020 RID: 32
		public class EventLogConfig
		{
			// Token: 0x06000073 RID: 115 RVA: 0x00003023 File Offset: 0x00001223
			public EventLogConfig(Guid providerId, WindowsEventLogRepository.EventConfig eventConfig)
			{
				if (providerId == Guid.Empty || eventConfig == null)
				{
					throw new ArgumentNullException("Invalid args for EventConfig");
				}
				this.ProviderId = providerId;
				this.EventConfig = eventConfig;
			}

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x06000074 RID: 116 RVA: 0x00003054 File Offset: 0x00001254
			public Guid ProviderId { get; }

			// Token: 0x17000024 RID: 36
			// (get) Token: 0x06000075 RID: 117 RVA: 0x0000305C File Offset: 0x0000125C
			public WindowsEventLogRepository.EventConfig EventConfig { get; }
		}

		// Token: 0x02000021 RID: 33
		public class EventConfig
		{
			// Token: 0x06000076 RID: 118 RVA: 0x00003064 File Offset: 0x00001264
			public EventConfig(int eventId, byte channel, byte version, byte level, byte opCode, int task, long keywords)
			{
				this.EventId = eventId;
				this.Channel = channel;
				this.Version = version;
				this.Level = level;
				this.OpCode = opCode;
				this.Task = task;
				this.Keywords = keywords;
			}

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x06000077 RID: 119 RVA: 0x000030A1 File Offset: 0x000012A1
			public int EventId { get; }

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x06000078 RID: 120 RVA: 0x000030A9 File Offset: 0x000012A9
			public byte Channel { get; }

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x06000079 RID: 121 RVA: 0x000030B1 File Offset: 0x000012B1
			public byte Version { get; }

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x0600007A RID: 122 RVA: 0x000030B9 File Offset: 0x000012B9
			public byte Level { get; }

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x0600007B RID: 123 RVA: 0x000030C1 File Offset: 0x000012C1
			public byte OpCode { get; }

			// Token: 0x1700002A RID: 42
			// (get) Token: 0x0600007C RID: 124 RVA: 0x000030C9 File Offset: 0x000012C9
			public int Task { get; }

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x0600007D RID: 125 RVA: 0x000030D1 File Offset: 0x000012D1
			public long Keywords { get; }
		}
	}
}
