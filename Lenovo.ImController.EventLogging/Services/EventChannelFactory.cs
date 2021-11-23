using System;
using Lenovo.ImController.EventLogging.Services.Repositories.Event;

namespace Lenovo.ImController.EventLogging.Services
{
	// Token: 0x02000009 RID: 9
	public static class EventChannelFactory
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002514 File Offset: 0x00000714
		public static WindowsEventLogRepository.EventLogConfig Create(KnownConstants.AppChannel app, EventChannelFactory.ChannelType type)
		{
			return new WindowsEventLogRepository.EventLogConfig(EventChannelFactory.GetProviderId(app), new WindowsEventLogRepository.EventConfig(KnownConstants.EventLog.EventIds.Default, EventChannelFactory.GetEventChannelType(app, type), KnownConstants.EventLog.Versions.Default, KnownConstants.EventLog.Levels.Default, KnownConstants.EventLog.OpCodes.Default, KnownConstants.EventLog.Tasks.Default, KnownConstants.EventLog.Keywords.Default));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000254C File Offset: 0x0000074C
		private static byte GetEventChannelType(KnownConstants.AppChannel channel, EventChannelFactory.ChannelType type)
		{
			if (type != EventChannelFactory.ChannelType.Operational)
			{
				throw new NotSupportedException("Currently only Operational is supported");
			}
			byte result;
			switch (channel)
			{
			case KnownConstants.AppChannel.Companion:
				result = KnownConstants.EventLog.Channels.CompanionAppOperational;
				break;
			case KnownConstants.AppChannel.Settings:
				result = KnownConstants.EventLog.Channels.SettingsAppOperational;
				break;
			case KnownConstants.AppChannel.Device:
				result = KnownConstants.EventLog.Channels.DeviceOperational;
				break;
			case KnownConstants.AppChannel.Core:
				result = KnownConstants.EventLog.Channels.CoreOperational;
				break;
			default:
				throw new InvalidOperationException("No channel type for channel");
			}
			return result;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000025B0 File Offset: 0x000007B0
		private static Guid GetProviderId(KnownConstants.AppChannel channel)
		{
			Guid result = Guid.Empty;
			switch (channel)
			{
			case KnownConstants.AppChannel.Companion:
				result = KnownConstants.EventLog.Providers.CompanionId;
				break;
			case KnownConstants.AppChannel.Settings:
				result = KnownConstants.EventLog.Providers.SettingsId;
				break;
			case KnownConstants.AppChannel.Device:
				result = KnownConstants.EventLog.Providers.DeviceId;
				break;
			case KnownConstants.AppChannel.Core:
				result = KnownConstants.EventLog.Providers.CoreId;
				break;
			default:
				throw new InvalidOperationException("No provider ID for channel");
			}
			return result;
		}

		// Token: 0x0200001E RID: 30
		public enum ChannelType
		{
			// Token: 0x04000057 RID: 87
			Operational
		}
	}
}
