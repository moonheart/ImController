using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Lenovo.Modern.ImController.ImClient.Utilities;

namespace Lenovo.Modern.ImController.ImClient.Package
{
	// Token: 0x02000007 RID: 7
	public class PackageManifestAgent
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002494 File Offset: 0x00000694
		public KeyValuePair<string, string> GetSetting(string packageName, string settingKey, PackageManifestAgent.SettingsQuery queryType)
		{
			return this.GetSettings(packageName, settingKey, queryType).FirstOrDefault<KeyValuePair<string, string>>();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000024A4 File Offset: 0x000006A4
		public IEnumerable<KeyValuePair<string, string>> GetSettings(string packageName, string settingKey, PackageManifestAgent.SettingsQuery queryType)
		{
			if (string.IsNullOrWhiteSpace(packageName) || string.IsNullOrWhiteSpace(settingKey))
			{
				throw new ArgumentException("Invalid args provided to get manifest settings");
			}
			List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
			IEnumerable<KeyValuePair<string, string>> packageSettings = this.GetPackageSettings(packageName);
			Func<string, bool> func = (string s) => s.Equals(settingKey, StringComparison.OrdinalIgnoreCase);
			Func<string, bool> func2 = (string s) => s.StartsWith(settingKey, StringComparison.OrdinalIgnoreCase);
			if (packageSettings != null || packageSettings.Any<KeyValuePair<string, string>>())
			{
				Func<string, bool> compareFunc = ((queryType == PackageManifestAgent.SettingsQuery.Equals) ? func : func2);
				result = (from s in packageSettings
					where s.Key != null && compareFunc(s.Key)
					select s).ToList<KeyValuePair<string, string>>();
			}
			return result;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002544 File Offset: 0x00000744
		private IEnumerable<KeyValuePair<string, string>> GetPackageSettings(string packageName)
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			XmlNode package = this.GetPackage(packageName);
			if (package != null)
			{
				XmlNodeList xmlNodeList = package.SelectNodes("SettingList/Setting");
				for (int i = 0; i < xmlNodeList.Count; i++)
				{
					XmlNode xmlNode = xmlNodeList[i];
					XmlAttribute xmlAttribute = xmlNode.Attributes["key"];
					string innerText = xmlNode.InnerText;
					if (xmlAttribute != null && !string.IsNullOrWhiteSpace(xmlAttribute.Value))
					{
						list.Add(new KeyValuePair<string, string>(xmlAttribute.Value, innerText));
					}
				}
			}
			return list;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000025C8 File Offset: 0x000007C8
		private XmlNode GetPackage(string packageName)
		{
			XmlNode xmlNode = null;
			XmlDocument subscriptionFile = this.GetSubscriptionFile();
			if (subscriptionFile != null)
			{
				xmlNode = subscriptionFile.SelectSingleNode("//Package[PackageInformation/@name='" + packageName + "']");
				if (xmlNode == null)
				{
					throw new ArgumentException("Unable to locate package named " + packageName + " inside manifest");
				}
			}
			return xmlNode;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002614 File Offset: 0x00000814
		private XmlDocument GetSubscriptionFile()
		{
			if (this._cachedSubscriptionFile == null)
			{
				string text = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Lenovo\\ImController\\ImControllerSubscription.xml";
				if (!File.Exists(text))
				{
					throw new FileNotFoundException("Subscription file not found at " + text);
				}
				if (!ExternalSignatureValidator.Instance.Validate(text))
				{
					throw new InvalidOperationException("The file " + text + " is not signed and trusted");
				}
				XmlDocument xmlDocument = new XmlDocument
				{
					XmlResolver = null
				};
				xmlDocument.Load(text);
				this._cachedSubscriptionFile = xmlDocument;
			}
			return this._cachedSubscriptionFile;
		}

		// Token: 0x04000008 RID: 8
		private XmlDocument _cachedSubscriptionFile;

		// Token: 0x0200003A RID: 58
		public enum SettingsQuery
		{
			// Token: 0x040000C0 RID: 192
			Equals,
			// Token: 0x040000C1 RID: 193
			StartsWith
		}

		// Token: 0x0200003B RID: 59
		private static class PackageConstants
		{
		}
	}
}
