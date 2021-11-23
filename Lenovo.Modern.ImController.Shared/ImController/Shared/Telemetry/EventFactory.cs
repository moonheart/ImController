using System;
using System.Collections.Generic;
using Lenovo.ImController.EventLogging.Model;

namespace Lenovo.Modern.ImController.Shared.Telemetry
{
	// Token: 0x02000005 RID: 5
	public class EventFactory
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002444 File Offset: 0x00000644
		public static ImcEvent CreateImcUpdateEvent(EventFactory.Constants.UpdateType updateType, EventFactory.Constants.UpdateAction updateAction, EventFactory.Constants.UpdateResult result, string componentName)
		{
			UserEvent userEvent = new UserEvent("Imc-Update-Action", DataClassification.Device, DateTime.Now)
			{
				Variables = new List<UserEventVariable>
				{
					new UserEventVariable("UpdateType", updateType.ToString()),
					new UserEventVariable("UpdateAction", updateAction.ToString()),
					new UserEventVariable("UpdateResult", result.ToString())
				}
			};
			if (!string.IsNullOrWhiteSpace(componentName))
			{
				userEvent.Variables.Add(new UserEventVariable("componentName", componentName));
			}
			return new ImcEvent(userEvent);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000024E8 File Offset: 0x000006E8
		public static ImcEvent CreateImcLifecycleEvent(EventFactory.Constants.LifcycleActivity activityType)
		{
			return new ImcEvent(new UserEvent("Imc-Lifecycle-Action", DataClassification.Device, DateTime.Now)
			{
				Variables = new List<UserEventVariable>
				{
					new UserEventVariable("LifcycleActivity", activityType.ToString())
				}
			});
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002528 File Offset: 0x00000728
		public static ImcEvent CreatePluginActivityEvent(EventFactory.Constants.PluginActivityType activityType, string activityName, string pluginName, string pluginVersion)
		{
			return new ImcEvent(new UserEvent("Imc-Plugin-Activity", DataClassification.Device, DateTime.Now)
			{
				Variables = new List<UserEventVariable>
				{
					new UserEventVariable("PluginActivityType", activityType.ToString()),
					new UserEventVariable("activityName", activityName),
					new UserEventVariable("pluginName", pluginName),
					new UserEventVariable("pluginVersion", pluginVersion)
				}
			});
		}

		// Token: 0x02000047 RID: 71
		public static class Constants
		{
			// Token: 0x02000091 RID: 145
			public static class Events
			{
				// Token: 0x0400026B RID: 619
				public const string UpdateAction = "Imc-Update-Action";

				// Token: 0x0400026C RID: 620
				public const string LifecycleAction = "Imc-Lifecycle-Action";

				// Token: 0x0400026D RID: 621
				public const string PluginActivity = "Imc-Plugin-Activity";
			}

			// Token: 0x02000092 RID: 146
			public static class Variables
			{
				// Token: 0x0400026E RID: 622
				public const string ImcVersion = "ImcVersion";

				// Token: 0x0400026F RID: 623
				public const string DriverVersion = "DriverVersion";

				// Token: 0x04000270 RID: 624
				public const string MinutesSinceServiceStart = "MinutesSinceServiceStart";

				// Token: 0x04000271 RID: 625
				public const string MinutesSinceSessionStart = "MinutesSinceSessionStart";

				// Token: 0x04000272 RID: 626
				public const string WindowsSessionID = "WindowsSessionID";
			}

			// Token: 0x02000093 RID: 147
			public enum UpdateType
			{
				// Token: 0x04000274 RID: 628
				Service,
				// Token: 0x04000275 RID: 629
				Subscription,
				// Token: 0x04000276 RID: 630
				Package
			}

			// Token: 0x02000094 RID: 148
			public enum UpdateAction
			{
				// Token: 0x04000278 RID: 632
				Download,
				// Token: 0x04000279 RID: 633
				Install,
				// Token: 0x0400027A RID: 634
				Remove
			}

			// Token: 0x02000095 RID: 149
			public enum UpdateResult
			{
				// Token: 0x0400027C RID: 636
				Success,
				// Token: 0x0400027D RID: 637
				Fail,
				// Token: 0x0400027E RID: 638
				Corrupt
			}

			// Token: 0x02000096 RID: 150
			public enum LifcycleActivity
			{
				// Token: 0x04000280 RID: 640
				Start,
				// Token: 0x04000281 RID: 641
				Stop,
				// Token: 0x04000282 RID: 642
				Resume,
				// Token: 0x04000283 RID: 643
				Suspend
			}

			// Token: 0x02000097 RID: 151
			public enum PluginActivityType
			{
				// Token: 0x04000285 RID: 645
				Contract,
				// Token: 0x04000286 RID: 646
				Event
			}
		}
	}
}
