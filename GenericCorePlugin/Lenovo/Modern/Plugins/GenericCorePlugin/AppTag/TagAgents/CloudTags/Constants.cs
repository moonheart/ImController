using System;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents.CloudTags
{
	// Token: 0x02000029 RID: 41
	public static class Constants
	{
		// Token: 0x04000078 RID: 120
		public const string PackagesDirectory = "Packages";

		// Token: 0x04000079 RID: 121
		public const string OnlineTagsUrl = "https://filedownload.lenovo.com/enm/companion/content/tags/v1/cloudtags.xml";

		// Token: 0x0400007A RID: 122
		public const string GenericCoreRegistryKey = "HKEY_CURRENT_USER\\SOFTWARE\\Lenovo\\ImController\\PluginData\\GenericCorePlugin";

		// Token: 0x0400007B RID: 123
		public static string PluginRegistryKey = "HKEY_CURRENT_USER\\Software\\Lenovo\\ImController\\PluginData";

		// Token: 0x0400007C RID: 124
		public static string LocalTagsFilePath = "%localappdata%\\Lenovo\\ImController\\PluginData\\GenericCorePlugin";

		// Token: 0x0400007D RID: 125
		public static string AppsAndTagsFileName = "AppsAndTags.xml";

		// Token: 0x02000089 RID: 137
		public enum TagRule
		{
			// Token: 0x040001EC RID: 492
			RegKeyValue,
			// Token: 0x040001ED RID: 493
			RegKeyExists,
			// Token: 0x040001EE RID: 494
			FileContains,
			// Token: 0x040001EF RID: 495
			FileXpath,
			// Token: 0x040001F0 RID: 496
			AppxInstalled,
			// Token: 0x040001F1 RID: 497
			PnpDevice,
			// Token: 0x040001F2 RID: 498
			WmiProperty
		}

		// Token: 0x0200008A RID: 138
		public enum TagValueRules
		{
			// Token: 0x040001F4 RID: 500
			strTgtValue,
			// Token: 0x040001F5 RID: 501
			boolDoesMatch,
			// Token: 0x040001F6 RID: 502
			onlyTag,
			// Token: 0x040001F7 RID: 503
			condition
		}

		// Token: 0x0200008B RID: 139
		public enum TagValueConditions
		{
			// Token: 0x040001F9 RID: 505
			greaterThanOrEqual,
			// Token: 0x040001FA RID: 506
			lessThanOrEqual,
			// Token: 0x040001FB RID: 507
			equal
		}

		// Token: 0x0200008C RID: 140
		public enum TagTargetPath
		{
			// Token: 0x040001FD RID: 509
			ClassGuid,
			// Token: 0x040001FE RID: 510
			hardwareId
		}

		// Token: 0x0200008D RID: 141
		public enum TagGroupLogic
		{
			// Token: 0x04000200 RID: 512
			and,
			// Token: 0x04000201 RID: 513
			or
		}
	}
}
