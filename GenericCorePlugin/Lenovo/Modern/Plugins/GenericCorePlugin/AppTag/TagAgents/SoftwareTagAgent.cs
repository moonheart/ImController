using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.Tags;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents
{
	// Token: 0x02000023 RID: 35
	internal class SoftwareTagAgent : ITagAgent
	{
		// Token: 0x06000105 RID: 261 RVA: 0x0000859C File Offset: 0x0000679C
		public Task<IEnumerable<Tag>> CollectTagsAsync()
		{
			List<Tag> list = new List<Tag>();
			string text = string.Empty;
			try
			{
				text = this.GetOperatingSystemBuildNumber();
				if (!string.IsNullOrWhiteSpace(text))
				{
					list.Add(new Tag
					{
						Key = "System.OsVersion",
						Value = text
					});
					list.Add(new Tag
					{
						Key = "System.OsVersion." + text
					});
				}
				else
				{
					Logger.Log(Logger.LogSeverity.Information, "No operating system build number detected via registry key");
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown detecting OS build number in the SoftwareTagAgent");
			}
			Tuple<bool, string> tuple = SoftwareTagAgent.CheckPreloadSignatureExistence();
			if (tuple.Item1)
			{
				list.Add(new Tag
				{
					Key = "System.Preload.Signature",
					Value = tuple.Item2
				});
			}
			return Task.FromResult<IEnumerable<Tag>>(list);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00008660 File Offset: 0x00006860
		public string GetOperatingSystemBuildNumber()
		{
			string result = string.Empty;
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = string.Empty;
			string text4 = string.Empty;
			try
			{
				IContainer container = new RegistrySystem().LoadContainer("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion");
				if (container != null)
				{
					IContainerValue value = container.GetValue("CurrentMajorVersionNumber");
					if (value != null)
					{
						text = value.GetValueAsInt().ToString();
					}
					IContainerValue value2 = container.GetValue("CurrentMinorVersionNumber");
					if (value2 != null)
					{
						text2 = value2.GetValueAsInt().ToString();
					}
					IContainerValue value3 = container.GetValue("CurrentBuildNumber");
					if (value3 != null)
					{
						text3 = value3.GetValueAsString();
					}
					IContainerValue value4 = container.GetValue("UBR");
					if (value4 != null)
					{
						text4 = value4.GetValueAsInt().ToString();
					}
				}
				if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(text2) && !string.IsNullOrWhiteSpace(text3) && !string.IsNullOrWhiteSpace(text4))
				{
					result = string.Format("{0}.{1}.{2}.{3}", new object[] { text, text2, text3, text4 });
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown getting the operating system build number");
			}
			return result;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000879C File Offset: 0x0000699C
		private static Tuple<bool, string> CheckPreloadSignatureExistence()
		{
			bool item = false;
			string item2 = string.Empty;
			try
			{
				RegistrySystem registrySystem = new RegistrySystem();
				IContainer container = registrySystem.LoadContainer("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\Signature");
				if (container != null)
				{
					item = true;
					IContainerValue value = container.GetValue("ImageType");
					if (value != null)
					{
						item2 = value.GetValueAsString();
					}
				}
				else
				{
					IContainer container2 = registrySystem.LoadContainer("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\Signature");
					if (container2 != null)
					{
						item = true;
						IContainerValue value2 = container2.GetValue("ImageType");
						if (value2 != null)
						{
							item2 = value2.GetValueAsString();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to check the preload signature registry key");
			}
			return new Tuple<bool, string>(item, item2);
		}

		// Token: 0x04000064 RID: 100
		public const string WindowsCurrentVersionRegistryKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion";

		// Token: 0x04000065 RID: 101
		public const string WindowsBuildVersionRegistryValue = "CurrentVersion";

		// Token: 0x04000066 RID: 102
		public const string WindowsBuildNumberRegistryValue = "CurrentBuildNumber";

		// Token: 0x04000067 RID: 103
		public const string MicrosoftSignatureRegistryKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\Signature";

		// Token: 0x04000068 RID: 104
		public const string MicrosoftSignatureRegistryKey32 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\Signature";

		// Token: 0x04000069 RID: 105
		public const string MicrosoftSignatureValueName = "ImageType";

		// Token: 0x0400006A RID: 106
		public const string WindowsMajorVersionNumberRegistryValue = "CurrentMajorVersionNumber";

		// Token: 0x0400006B RID: 107
		public const string WindowsMinorVersionNumberRegistryValue = "CurrentMinorVersionNumber";

		// Token: 0x0400006C RID: 108
		public const string WindowsUBRRegistryValue = "UBR";
	}
}
