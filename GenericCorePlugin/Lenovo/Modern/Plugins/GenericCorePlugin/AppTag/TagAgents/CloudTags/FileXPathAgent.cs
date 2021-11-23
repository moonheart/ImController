using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.XPath;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Model;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents.CloudTags
{
	// Token: 0x0200002B RID: 43
	public class FileXPathAgent : ICloudTagProvider
	{
		// Token: 0x0600012E RID: 302 RVA: 0x0000959C File Offset: 0x0000779C
		public async Task<Tag> GetCloudTagAsync(TagRule tagRule, string tagName)
		{
			Tag tag = null;
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "FileXPathAgent: Tag Rule execution for Tag Group " + tagName + " is begining!");
				await this._fileXPathSemaphore.WaitAsync();
				string text = tagRule.TargetPath;
				string newValue = string.Empty;
				Match match = new Regex("%[A-Za-z0-9\\(\\)]*%").Match(text);
				string text2 = string.Empty;
				if (match.Length > 0)
				{
					text2 = match.Groups[0].ToString();
					if (!string.IsNullOrEmpty(text2))
					{
						newValue = Environment.ExpandEnvironmentVariables(text2);
					}
					text = text.Replace(text2, newValue);
				}
				if (File.Exists(text) && !string.IsNullOrEmpty(tagRule.TargetName))
				{
					string s = File.ReadAllText(text);
					XPathDocument xpathDocument = new XPathDocument(new MemoryStream(Encoding.Unicode.GetBytes(s)));
					if (xpathDocument != null)
					{
						XPathNavigator xpathNavigator = xpathDocument.CreateNavigator();
						if (xpathNavigator != null)
						{
							XPathNodeIterator xpathNodeIterator = xpathNavigator.Select(tagRule.TargetName);
							if (xpathNodeIterator != null)
							{
								using (IEnumerator enumerator = xpathNodeIterator.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										if (enumerator.Current.ToString() == tagRule.TargetReqValue)
										{
											tag = new Tag(tagName, string.Empty);
										}
									}
									goto IL_1F3;
								}
							}
							Logger.Log(Logger.LogSeverity.Information, "FileXPathAgent: " + tagRule.TargetPath + " doesn't contain data!");
						}
					}
				}
				IL_1F3:;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "FileXPathAgent: Error caught!!");
			}
			finally
			{
				this._fileXPathSemaphore.Release();
			}
			return tag;
		}

		// Token: 0x04000084 RID: 132
		private readonly SemaphoreSlim _fileXPathSemaphore = new SemaphoreSlim(1);
	}
}
