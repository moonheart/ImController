using System;

namespace Lenovo.ImController.EventLogging
{
	// Token: 0x02000004 RID: 4
	public static class KnownConstants
	{
		// Token: 0x02000018 RID: 24
		public enum AppChannel
		{
			// Token: 0x04000030 RID: 48
			Companion,
			// Token: 0x04000031 RID: 49
			Settings,
			// Token: 0x04000032 RID: 50
			Device,
			// Token: 0x04000033 RID: 51
			Core
		}

		// Token: 0x02000019 RID: 25
		public static class EventLog
		{
			// Token: 0x02000025 RID: 37
			public static class Configurations
			{
			}

			// Token: 0x02000026 RID: 38
			public static class Providers
			{
				// Token: 0x0400006B RID: 107
				public static readonly Guid CompanionId = new Guid("71a9201e-73b0-43fe-9821-7e159a59bc71");

				// Token: 0x0400006C RID: 108
				public static readonly Guid SettingsId = new Guid("34a9201e-73b0-43fe-9821-7e159a59bc71");

				// Token: 0x0400006D RID: 109
				public static readonly Guid DeviceId = new Guid("809c33b3-ed18-4d42-8b60-19eb0049e77c");

				// Token: 0x0400006E RID: 110
				public static readonly Guid CoreId = new Guid("f0a12ecd-d276-4642-a90e-d7618eeeacdd");
			}

			// Token: 0x02000027 RID: 39
			public static class EventIds
			{
				// Token: 0x0400006F RID: 111
				public static readonly int Default = 104;
			}

			// Token: 0x02000028 RID: 40
			public static class Channels
			{
				// Token: 0x04000070 RID: 112
				public static readonly byte CompanionAppOperational = 16;

				// Token: 0x04000071 RID: 113
				public static readonly byte SettingsAppOperational = 2;

				// Token: 0x04000072 RID: 114
				public static readonly byte DeviceOperational = 3;

				// Token: 0x04000073 RID: 115
				public static readonly byte CoreOperational = 4;
			}

			// Token: 0x02000029 RID: 41
			public static class Versions
			{
				// Token: 0x04000074 RID: 116
				public static readonly byte Default;
			}

			// Token: 0x0200002A RID: 42
			public static class Levels
			{
				// Token: 0x04000075 RID: 117
				public static readonly byte Default = 4;
			}

			// Token: 0x0200002B RID: 43
			public static class OpCodes
			{
				// Token: 0x04000076 RID: 118
				public static readonly byte Default;
			}

			// Token: 0x0200002C RID: 44
			public static class Tasks
			{
				// Token: 0x04000077 RID: 119
				public static readonly int Default;
			}

			// Token: 0x0200002D RID: 45
			public static class Keywords
			{
				// Token: 0x04000078 RID: 120
				public static readonly long Default = 4611686018427387904L;
			}
		}
	}
}
