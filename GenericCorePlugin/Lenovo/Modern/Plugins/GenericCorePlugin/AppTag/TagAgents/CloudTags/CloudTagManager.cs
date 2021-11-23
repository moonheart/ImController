using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Model;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.Tags;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Validation;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents.CloudTags
{
	// Token: 0x02000028 RID: 40
	internal class CloudTagManager : ITagAgent
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00009129 File Offset: 0x00007329
		// (set) Token: 0x0600011E RID: 286 RVA: 0x00009131 File Offset: 0x00007331
		private CloudTagGroups cloudTagGroups { get; set; }

		// Token: 0x0600011F RID: 287 RVA: 0x0000913A File Offset: 0x0000733A
		public CloudTagManager()
		{
			this._registry = CloudTagRegistry.GetInstance();
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00009158 File Offset: 0x00007358
		public async Task<IEnumerable<Tag>> CollectTagsAsync()
		{
			CloudTagGroups cloudTagGroups = await this.ReadCloudConfigurationFileAsync();
			this.cloudTagGroups = cloudTagGroups;
			if (this.cloudTagGroups != null)
			{
				await this.ParseTagRules();
			}
			return this.lstTags;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000091A0 File Offset: 0x000073A0
		private async Task ParseTagRules()
		{
			if (this.cloudTagGroups != null && this.cloudTagGroups.TagGroup.Any<TagGroup>())
			{
				foreach (TagGroup tagGroup in this.cloudTagGroups.TagGroup)
				{
					if (tagGroup.Logic.ToLowerInvariant() == Constants.TagGroupLogic.or.ToString())
					{
						List<Tag> source = await this.ParseTags(tagGroup);
						if (source.Count<Tag>() > 0)
						{
							this.lstTags.Add(source.FirstOrDefault<Tag>());
						}
					}
					else if (tagGroup.Logic.ToLowerInvariant() == Constants.TagGroupLogic.and.ToString())
					{
						List<Tag> list = await this.ParseTags(tagGroup);
						if (list.Count == tagGroup.TagRule.Count<TagRule>())
						{
							this.lstTags.Add(list.FirstOrDefault<Tag>());
						}
					}
					tagGroup = null;
				}
				TagGroup[] array = null;
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000091E8 File Offset: 0x000073E8
		private async Task<List<Tag>> ParseTags(TagGroup tagGroup)
		{
			List<Tag> lstParsedTags = new List<Tag>();
			foreach (TagRule tagRule in tagGroup.TagRule)
			{
				if (tagGroup.Logic == Constants.TagGroupLogic.or.ToString() && lstParsedTags.Any<Tag>())
				{
					return lstParsedTags;
				}
				if (tagRule.TargetType.ToLowerInvariant() == Constants.TagRule.RegKeyExists.ToString().ToLowerInvariant())
				{
					this.cloudTagFactory = new RegistryAgent();
					Tag tag = await this.cloudTagFactory.GetCloudTagAsync(tagRule, tagGroup.TagName);
					if (tag != null)
					{
						lstParsedTags.Add(tag);
					}
				}
				else if (tagRule.TargetType.ToLowerInvariant() == Constants.TagRule.FileContains.ToString().ToLowerInvariant())
				{
					this.cloudTagFactory = new LocalFileAgent();
					Tag tag2 = await this.cloudTagFactory.GetCloudTagAsync(tagRule, tagGroup.TagName);
					if (tag2 != null)
					{
						lstParsedTags.Add(tag2);
					}
				}
				else if (tagRule.TargetType.ToLowerInvariant() == Constants.TagRule.FileXpath.ToString().ToLowerInvariant())
				{
					this.cloudTagFactory = new FileXPathAgent();
					Tag tag3 = await this.cloudTagFactory.GetCloudTagAsync(tagRule, tagGroup.TagName);
					if (tag3 != null)
					{
						lstParsedTags.Add(tag3);
					}
				}
				else if (tagRule.TargetType.ToLowerInvariant() == Constants.TagRule.AppxInstalled.ToString().ToLowerInvariant())
				{
					this.cloudTagFactory = new AppXAgent();
					Tag tag4 = await this.cloudTagFactory.GetCloudTagAsync(tagRule, tagGroup.TagName);
					if (tag4 != null)
					{
						lstParsedTags.Add(tag4);
					}
				}
				else if (tagRule.TargetType.ToLowerInvariant() == Constants.TagRule.PnpDevice.ToString().ToLowerInvariant())
				{
					this.cloudTagFactory = new PnpDeviceAgent();
					Tag tag5 = await this.cloudTagFactory.GetCloudTagAsync(tagRule, tagGroup.TagName);
					if (tag5 != null)
					{
						lstParsedTags.Add(tag5);
					}
				}
				else if (tagRule.TargetType.ToLowerInvariant() == Constants.TagRule.WmiProperty.ToString().ToLowerInvariant())
				{
					this.cloudTagFactory = new WmiPropertyAgent();
					Tag tag6 = await this.cloudTagFactory.GetCloudTagAsync(tagRule, tagGroup.TagName);
					if (tag6 != null)
					{
						lstParsedTags.Add(tag6);
					}
				}
			}
			TagRule[] array = null;
			return lstParsedTags;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00009238 File Offset: 0x00007438
		private async Task<CloudTagGroups> ReadCloudConfigurationFileAsync()
		{
			CloudTagGroups _cloudTagGroups = null;
			bool registryTagMatch = false;
			HttpResponseMessage response = null;
			try
			{
				string previousTag = this._registry.GetLastEtag();
				if (!string.IsNullOrWhiteSpace(previousTag))
				{
					string value = await this.GetHttpResponseHeadersAsync();
					if (!string.IsNullOrWhiteSpace(value) && previousTag.Equals(value, StringComparison.OrdinalIgnoreCase))
					{
						Logger.Log(Logger.LogSeverity.Information, "CloudTagManager: Online AppsAndTags xml file Will not be evaluated because xml has not changed https://filedownload.lenovo.com/enm/companion/content/tags/v1/cloudtags.xml");
						registryTagMatch = true;
					}
				}
				string content = string.Empty;
				using (HttpClient client = new HttpClient())
				{
					response = await client.GetAsync("https://filedownload.lenovo.com/enm/companion/content/tags/v1/cloudtags.xml");
					if (response != null && HttpStatusCode.OK == response.StatusCode && response.Content != null)
					{
						content = await response.Content.ReadAsStringAsync();
					}
				}
				HttpClient client = null;
				bool flag = false;
				if (string.IsNullOrEmpty(content))
				{
					content = this.ReadFromDisk();
					flag = true;
				}
				if (!string.IsNullOrWhiteSpace(content))
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.PreserveWhitespace = true;
					xmlDocument.LoadXml(content);
					bool flag2 = new CertificateValidator().IsXmlValid(xmlDocument);
					Logger.Log(Logger.LogSeverity.Information, "CloudTagManager: AppsAndTags Signature Verification Status: " + flag2.ToString());
					if (flag2)
					{
						if (!flag && !registryTagMatch)
						{
							HttpResponseMessage httpResponseMessage = response;
							object obj;
							if (httpResponseMessage == null)
							{
								obj = null;
							}
							else
							{
								obj = httpResponseMessage.Headers.FirstOrDefault((KeyValuePair<string, IEnumerable<string>> h) => h.Key.Equals("etag", StringComparison.OrdinalIgnoreCase)).Value;
							}
							object obj2 = obj;
							string text = ((obj2 != null) ? obj2.FirstOrDefault<string>() : null);
							if (!string.IsNullOrWhiteSpace(text))
							{
								this._registry.SetLastEtag(text);
							}
						}
						else
						{
							if (flag)
							{
								Logger.Log(Logger.LogSeverity.Information, "Cloud Tags are being Deserialzed from the offline AppsAndTags.xml file");
							}
							if (registryTagMatch)
							{
								Logger.Log(Logger.LogSeverity.Information, "The registry indicates that the Cloud file is not changed hence the offline file is used to generate AppsAndTags.xml file");
							}
						}
						if (!flag)
						{
							this.SaveToDisk(content);
						}
						_cloudTagGroups = Serializer.Deserialize<CloudTagGroups>(content);
					}
					else
					{
						Logger.Log(Logger.LogSeverity.Error, "cloudtagmanager: online appsandtags file not trusted, so generating from the offline file");
					}
				}
				else
				{
					Logger.Log(Logger.LogSeverity.Warning, "CloudTagManager: Online AppsAndTags file request response content as string was empty");
				}
				previousTag = null;
				content = null;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "CloudTagManager: Error caught in ReadCloudConfigurationFileAsync()!!");
			}
			return _cloudTagGroups;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00009280 File Offset: 0x00007480
		public async Task<string> GetHttpResponseHeadersAsync()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Head, new Uri("https://filedownload.lenovo.com/enm/companion/content/tags/v1/cloudtags.xml")))
					{
						using (HttpResponseMessage httpResponseMessage = await client.SendAsync(requestMessage))
						{
							if (httpResponseMessage != null && httpResponseMessage.Headers != null && httpResponseMessage.Headers.Any<KeyValuePair<string, IEnumerable<string>>>())
							{
								return httpResponseMessage.Headers.ETag.Tag;
							}
						}
					}
					HttpRequestMessage requestMessage = null;
				}
				HttpClient client = null;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "CloudTagManager: Error caught in GetHttpResponseHeadersAsync()!!");
			}
			return null;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000092C0 File Offset: 0x000074C0
		private bool SaveToDisk(string appTagCollectionAsXml)
		{
			bool result = false;
			try
			{
				string text = this.ExpandEnvironmentalVariable(Constants.LocalTagsFilePath);
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				string path = Path.Combine(text, Constants.AppsAndTagsFileName);
				if (File.Exists(path))
				{
					File.WriteAllText(path, appTagCollectionAsXml);
					Logger.Log(Logger.LogSeverity.Information, "CloudTagManager: AppsAndTags.xml file has been updated and cached!");
				}
				else
				{
					File.Create(path).Dispose();
					using (TextWriter textWriter = new StreamWriter(path))
					{
						textWriter.WriteLine(appTagCollectionAsXml);
						Logger.Log(Logger.LogSeverity.Information, "CloudTagManager: AppsAndTags.xml file has been cached!");
					}
				}
				result = true;
			}
			catch (Exception)
			{
				Logger.Log(Logger.LogSeverity.Error, "CloudTagManager: Fail to save AppsAndTags file.");
			}
			return result;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00009374 File Offset: 0x00007574
		private string ReadFromDisk()
		{
			string result = string.Empty;
			try
			{
				string path = Path.Combine(this.ExpandEnvironmentalVariable(Constants.LocalTagsFilePath), Constants.AppsAndTagsFileName);
				if (File.Exists(path))
				{
					result = File.ReadAllText(path);
				}
			}
			catch (Exception)
			{
				Logger.Log(Logger.LogSeverity.Error, "CaF AppTagProvider: Fail to save New AppAndTagCollection.");
			}
			return result;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000093D0 File Offset: 0x000075D0
		private string ExpandEnvironmentalVariable(string environmentVariable)
		{
			string newValue = string.Empty;
			Match match = new Regex("%[A-Za-z0-9\\(\\)]*%").Match(environmentVariable);
			if (match.Length > 0)
			{
				string text = match.Groups[0].ToString();
				if (!string.IsNullOrEmpty(text))
				{
					newValue = Environment.ExpandEnvironmentVariables(text);
				}
				environmentVariable = environmentVariable.Replace(text, newValue);
			}
			return environmentVariable;
		}

		// Token: 0x04000074 RID: 116
		private List<Tag> lstTags = new List<Tag>();

		// Token: 0x04000075 RID: 117
		private ICloudTagProvider cloudTagFactory;

		// Token: 0x04000076 RID: 118
		private CloudTagRegistry _registry;
	}
}
