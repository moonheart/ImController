using System;

namespace Lenovo.Modern.ImController.CommandLineServices
{
	// Token: 0x02000015 RID: 21
	internal static class Constants
	{
		// Token: 0x02000025 RID: 37
		internal static class Command
		{
			// Token: 0x04000088 RID: 136
			public static readonly string InstallPackagesWithReboot = "installpackageswithreboot";

			// Token: 0x04000089 RID: 137
			public static readonly string InstallPackages = "installpackages";

			// Token: 0x0400008A RID: 138
			public static readonly string FixPermissions = "fixpermissions";

			// Token: 0x0400008B RID: 139
			public static readonly string GetStatus = "getstatus";

			// Token: 0x0400008C RID: 140
			public static readonly string InstallSubscription = "installsubscription";

			// Token: 0x0400008D RID: 141
			public static readonly string UnInstallPackages = "uninstallpackages";

			// Token: 0x0400008E RID: 142
			public static readonly string ProtocolEvent = "ProtocolEventPackage";

			// Token: 0x0400008F RID: 143
			public static readonly string ProtocolEventValue = "protocoleventvalue";

			// Token: 0x04000090 RID: 144
			public static readonly string InfUninstallation = "infuninstallation";

			// Token: 0x04000091 RID: 145
			public static readonly string DeleteAllImcScheduledTasks = "deleteallimcscheduledtasks";

			// Token: 0x04000092 RID: 146
			public static readonly string DeleteImcTimebasedScheduledTasks = "deleteimctimebasedscheduledtasks";

			// Token: 0x04000093 RID: 147
			public static readonly string TimeBasedEventTrigger = "timebasedeventtrigger";

			// Token: 0x04000094 RID: 148
			public static readonly string InhibitOldImcInstaller = "verysilent";
		}
	}
}
