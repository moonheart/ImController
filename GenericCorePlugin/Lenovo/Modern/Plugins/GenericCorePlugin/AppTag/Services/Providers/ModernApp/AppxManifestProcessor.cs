using System;
using System.Collections;
using System.IO;
using System.Xml;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.ModernApp
{
	// Token: 0x02000034 RID: 52
	internal class AppxManifestProcessor
	{
		// Token: 0x0600013E RID: 318 RVA: 0x000098F0 File Offset: 0x00007AF0
		public static string GetProtocol(FileInfo appXManifestFile)
		{
			string result = null;
			try
			{
				if (appXManifestFile != null && appXManifestFile.Exists)
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.Load(appXManifestFile.FullName);
					XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Protocol");
					if (elementsByTagName == null || elementsByTagName.Count <= 0)
					{
						goto IL_9B;
					}
					using (IEnumerator enumerator = elementsByTagName.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							XmlElement xmlElement = (XmlElement)obj;
							if (xmlElement != null)
							{
								result = xmlElement.GetAttribute("Name");
							}
						}
						goto IL_9B;
					}
				}
				Logger.Log(Logger.LogSeverity.Warning, "Predicted appxmanifest file does not exist: {0}", new object[] { (appXManifestFile != null) ? appXManifestFile.FullName : "null" });
				IL_9B:;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to get protocol from appxmanifest");
			}
			return result;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000099C4 File Offset: 0x00007BC4
		public static string GetAppUserModelId(FileInfo appXManifestFile, string packageFamilyName)
		{
			string text = null;
			try
			{
				string text2 = null;
				string value = null;
				if (appXManifestFile != null && appXManifestFile.Exists)
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.Load(appXManifestFile.FullName);
					XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Application");
					if (elementsByTagName != null && elementsByTagName.Count > 0)
					{
						foreach (object obj in elementsByTagName)
						{
							XmlElement xmlElement = (XmlElement)obj;
							if (xmlElement != null)
							{
								text2 = xmlElement.GetAttribute("Id");
							}
						}
					}
					XmlNodeList elementsByTagName2 = xmlDocument.GetElementsByTagName("Identity");
					if (elementsByTagName2 != null && elementsByTagName2.Count > 0)
					{
						foreach (object obj2 in elementsByTagName2)
						{
							XmlElement xmlElement2 = (XmlElement)obj2;
							if (xmlElement2 != null)
							{
								value = xmlElement2.GetAttribute("Name");
							}
						}
					}
					if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(text2))
					{
						text = string.Format("{0}!{1}", packageFamilyName, text2);
					}
				}
				else
				{
					Logger.Log(Logger.LogSeverity.Warning, "Predicted appxmanifest file does not exist: {0}", new object[] { (appXManifestFile != null) ? appXManifestFile.FullName : "null" });
				}
				if (string.IsNullOrWhiteSpace(text))
				{
					Logger.Log(Logger.LogSeverity.Warning, "App does not have app user model id : {0}", new object[] { (appXManifestFile == null) ? "" : appXManifestFile.FullName });
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to extract AppUserModelId from AppxManifest");
			}
			return text;
		}

		// Token: 0x02000095 RID: 149
		internal static class Constants
		{
			// Token: 0x0400022F RID: 559
			public const string XmlNamespace = "http://schemas.microsoft.com/appx/2010/manifest";

			// Token: 0x04000230 RID: 560
			public const string AppxManifestFileName = "AppxManifest.xml";

			// Token: 0x04000231 RID: 561
			public const string AppUserModelIdFormat = "{0}!{1}";

			// Token: 0x04000232 RID: 562
			public const string XmlIdentityNodeName = "Identity";

			// Token: 0x04000233 RID: 563
			public const string XmlIdentityNameAttribute = "Name";

			// Token: 0x04000234 RID: 564
			public const string XmlProtocolNodeName = "Protocol";

			// Token: 0x04000235 RID: 565
			public const string XmlProtocolNameAttribute = "Name";

			// Token: 0x04000236 RID: 566
			public const string XmlApplicationNodeName = "Application";

			// Token: 0x04000237 RID: 567
			public const string XmlApplicationIdAttribute = "Id";
		}
	}
}
