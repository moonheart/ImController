using System;
using System.IO;
using Lenovo.Modern.ImController.ImClient.Models;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x02000019 RID: 25
	public class InstallationLocator
	{
		// Token: 0x06000076 RID: 118 RVA: 0x00004CD5 File Offset: 0x00002ED5
		public static string GetPluginInstallationLocation()
		{
			return Path.Combine(Environment.ExpandEnvironmentVariables(Constants.RootFolder), Constants.PluginsInstallationRelativePath);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004CEC File Offset: 0x00002EEC
		public static string GetPluginHostInstallationLocation(Bitness bit)
		{
			string result = string.Empty;
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
			if (bit == Bitness.X64)
			{
				string text = Path.Combine(folderPath, Constants.PluginHostInstallationRelativePath, Constants.PluginHostFileName);
				if (File.Exists(text))
				{
					result = text;
				}
			}
			else if (bit == Bitness.X86)
			{
				string text2 = Path.Combine(folderPath, Constants.PluginHost86InstallationRelativePath, Constants.PluginHostFileName);
				if (File.Exists(text2))
				{
					result = text2;
				}
			}
			return result;
		}
	}
}
