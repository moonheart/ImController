using System;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Model;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents.CloudTags
{
	// Token: 0x0200002E RID: 46
	public class RegistryAgent : ICloudTagProvider
	{
		// Token: 0x06000134 RID: 308 RVA: 0x000096E0 File Offset: 0x000078E0
		public async Task<Tag> GetCloudTagAsync(TagRule tagRule, string tagName)
		{
			Tag tag = null;
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "RegistryAgent: Tag Rule execution for Tag Group " + tagName + " is begining!");
				await this._registryAgentSemaphore.WaitAsync();
				IContainer container = new RegistrySystem().LoadContainer(tagRule.TargetPath);
				if (container != null)
				{
					IContainerValue containerValue = ((!string.IsNullOrEmpty(tagRule.TargetName)) ? container.GetValue(tagRule.TargetName) : null);
					if (containerValue != null)
					{
						if (tagRule.TargetValueRule == Constants.TagValueRules.strTgtValue.ToString())
						{
							string[] array = tagRule.TargetReqValue.Split(new char[] { '|' });
							string valueAsString = containerValue.GetValueAsString();
							for (int i = 0; i < array.Length; i++)
							{
								if (valueAsString == array[i])
								{
									tag = new Tag(tagName, valueAsString);
								}
							}
						}
						else if (tagRule.TargetValueRule == Constants.TagValueRules.boolDoesMatch.ToString())
						{
							bool? valueAsBool = containerValue.GetValueAsBool();
							bool? flag = null;
							if (tagRule.TargetReqValue == "0" || tagRule.TargetReqValue == "1")
							{
								flag = new bool?(Convert.ToBoolean(Convert.ToInt16(tagRule.TargetReqValue)));
							}
							if (valueAsBool != null && flag != null)
							{
								bool? flag2 = valueAsBool;
								bool? flag3 = flag;
								if ((flag2.GetValueOrDefault() == flag3.GetValueOrDefault()) & (flag2 != null == (flag3 != null)))
								{
									tag = new Tag(tagName, string.Empty);
								}
							}
						}
						else if (tagRule.TargetValueRule == Constants.TagValueRules.onlyTag.ToString())
						{
							tag = new Tag(tagName, string.Empty);
						}
					}
					else
					{
						Logger.Log(Logger.LogSeverity.Information, "RegistryAgent: " + tagRule.TargetPath + " doesn't exist!");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "RegistryAgent: Error caught!!");
			}
			finally
			{
				this._registryAgentSemaphore.Release();
			}
			return tag;
		}

		// Token: 0x04000087 RID: 135
		private readonly SemaphoreSlim _registryAgentSemaphore = new SemaphoreSlim(1);
	}
}
