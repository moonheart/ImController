using System;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services
{
	// Token: 0x02000031 RID: 49
	internal static class ProtocolGenerator
	{
		// Token: 0x06000139 RID: 313 RVA: 0x000097B8 File Offset: 0x000079B8
		public static string CreateProtocolCommand(ModernAppInformation modernApp)
		{
			string result = null;
			try
			{
				if (modernApp != null && !string.IsNullOrWhiteSpace(modernApp.Protocol))
				{
					result = modernApp.Protocol + ":";
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to generate protocol for modern app");
			}
			return result;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00009808 File Offset: 0x00007A08
		public static string CreateProtocolCommand(LegacyAppInformation legacyApp)
		{
			string result = null;
			if (legacyApp != null)
			{
				result = legacyApp.LaunchCommand;
			}
			return result;
		}

		// Token: 0x02000093 RID: 147
		public static class Constants
		{
			// Token: 0x04000225 RID: 549
			public const string Params = "PARAM";

			// Token: 0x04000226 RID: 550
			public const string AppParam = "app";

			// Token: 0x04000227 RID: 551
			public const string AumidParam = "aumid";

			// Token: 0x04000228 RID: 552
			public const string ShortcutParameter = "lnk";
		}
	}
}
