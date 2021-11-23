using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Model;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents.CloudTags
{
	// Token: 0x0200002C RID: 44
	public class LocalFileAgent : ICloudTagProvider
	{
		// Token: 0x06000130 RID: 304 RVA: 0x00009608 File Offset: 0x00007808
		public async Task<Tag> GetCloudTagAsync(TagRule tagRule, string tagName)
		{
			Tag tag = null;
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "LocalFileAgent: Tag Rule execution for Tag Group " + tagName + " is begining!");
				await this._localFileAgentSemaphore.WaitAsync();
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
				if (string.IsNullOrEmpty(tagRule.TargetReqValue))
				{
					Logger.Log(Logger.LogSeverity.Information, "LocalFileAgent: TargetReqValue: \"" + tagRule.TargetReqValue + "\" is empty!");
					return tag;
				}
				if (File.Exists(text))
				{
					string text3 = File.ReadAllText(text);
					if (!string.IsNullOrEmpty(text3))
					{
						if (text3.Contains(tagRule.TargetReqValue))
						{
							tag = new Tag(tagName, string.Empty);
						}
					}
					else
					{
						Logger.Log(Logger.LogSeverity.Information, "LocalFileAgent: TargetPath: \"" + tagRule.TargetPath + "\" is empty!");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "LocalFileAgent: Error caught!!");
			}
			finally
			{
				this._localFileAgentSemaphore.Release();
			}
			return tag;
		}

		// Token: 0x04000085 RID: 133
		private readonly SemaphoreSlim _localFileAgentSemaphore = new SemaphoreSlim(1);
	}
}
