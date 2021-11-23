using System;
using Lenovo.ImController.EventLogging.Model;
using Lenovo.ImController.EventLogging.Services.Repositories;
using Lenovo.ImController.EventLogging.Services.Repositories.Event;
using Lenovo.ImController.EventLogging.Utilities;

namespace Lenovo.ImController.EventLogging.Services
{
	// Token: 0x0200000A RID: 10
	public class TelemetryLogger
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002607 File Offset: 0x00000807
		// (set) Token: 0x06000015 RID: 21 RVA: 0x00002620 File Offset: 0x00000820
		public static TelemetryLogger Instance
		{
			get
			{
				if (TelemetryLogger._instance == null)
				{
					throw new ArgumentNullException("Instance must be manually set");
				}
				return TelemetryLogger._instance;
			}
			set
			{
				if (TelemetryLogger._instance != null)
				{
					throw new InvalidOperationException("Instance is already set");
				}
				TelemetryLogger._instance = value;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000263A File Offset: 0x0000083A
		public TelemetryLogger(KnownConstants.AppChannel channel, string productName, string productVersion)
			: this(new TelemetryLogger.Configuration(channel, productName, productVersion))
		{
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000264C File Offset: 0x0000084C
		public TelemetryLogger(TelemetryLogger.Configuration config)
		{
			if (config == null)
			{
				throw new ArgumentException("Config is null or invalid");
			}
			if (config == null || config.Repository == null || !(config.Repository is IWriteableEventRepository))
			{
				throw new ArgumentNullException("Repository is null or invalid");
			}
			if (string.IsNullOrWhiteSpace(config.ProductName) || string.IsNullOrWhiteSpace(config.ProductVersion))
			{
				throw new ArgumentNullException("Configuration not complete: product details");
			}
			this._config = config;
			this._repository = config.Repository as IWriteableEventRepository;
			if (this._repository == null)
			{
				throw new InvalidOperationException("Unable to locate writeable event repository");
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000026E0 File Offset: 0x000008E0
		public bool LogEvent(UserEvent userEvent)
		{
			if (userEvent == null)
			{
				throw new ArgumentNullException("userEvent");
			}
			userEvent.UserSID = this.GetCachedCurrentUserId();
			userEvent.ProductName = this._config.ProductName;
			userEvent.ProductVersion = this._config.ProductVersion;
			UserEvent.AssertValidity(userEvent);
			StorableEvent userEvent2 = StorableEvent.FromUserEvent(userEvent);
			this._repository.LogEvent(userEvent2);
			return true;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002743 File Offset: 0x00000943
		public void ClearUserDataCache()
		{
			this._userId = null;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000274C File Offset: 0x0000094C
		private string GetCachedCurrentUserId()
		{
			if (this._userId == null)
			{
				string loggedInUserSID = UserAgent.GetLoggedInUserSID();
				if (string.IsNullOrWhiteSpace(loggedInUserSID))
				{
					return "s-1-5-18";
				}
				this._userId = loggedInUserSID;
			}
			return this._userId;
		}

		// Token: 0x04000007 RID: 7
		private static TelemetryLogger _instance;

		// Token: 0x04000008 RID: 8
		private readonly TelemetryLogger.Configuration _config;

		// Token: 0x04000009 RID: 9
		private readonly IWriteableEventRepository _repository;

		// Token: 0x0400000A RID: 10
		private string _userId;

		// Token: 0x0400000B RID: 11
		private const string LOCAL_SYSTEM_SID = "s-1-5-18";

		// Token: 0x0200001F RID: 31
		public class Configuration
		{
			// Token: 0x0600006E RID: 110 RVA: 0x00002F70 File Offset: 0x00001170
			public Configuration(KnownConstants.AppChannel channel, string productName, string productVersion)
			{
				if (string.IsNullOrWhiteSpace(productVersion) || string.IsNullOrWhiteSpace(productVersion))
				{
					throw new ArgumentNullException("Invalid arguments for Configuration");
				}
				WindowsEventLogRepository windowsEventLogRepository = new WindowsEventLogRepository(EventChannelFactory.Create(channel, EventChannelFactory.ChannelType.Operational));
				if (windowsEventLogRepository == null)
				{
					throw new ArgumentNullException("Desired Repository not found");
				}
				this.Repository = windowsEventLogRepository;
				this.ProductName = productName;
				this.ProductVersion = productVersion;
			}

			// Token: 0x0600006F RID: 111 RVA: 0x00002FD0 File Offset: 0x000011D0
			public Configuration(IEventRepository repository, string productName, string productVersion)
			{
				if (repository == null || string.IsNullOrWhiteSpace(productVersion) || string.IsNullOrWhiteSpace(productVersion))
				{
					throw new ArgumentNullException("Invalid arguments for Configuration");
				}
				this.Repository = repository;
				this.ProductName = productName;
				this.ProductVersion = productVersion;
			}

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x06000070 RID: 112 RVA: 0x0000300B File Offset: 0x0000120B
			public IEventRepository Repository { get; }

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x06000071 RID: 113 RVA: 0x00003013 File Offset: 0x00001213
			public string ProductName { get; }

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x06000072 RID: 114 RVA: 0x0000301B File Offset: 0x0000121B
			public string ProductVersion { get; }
		}
	}
}
