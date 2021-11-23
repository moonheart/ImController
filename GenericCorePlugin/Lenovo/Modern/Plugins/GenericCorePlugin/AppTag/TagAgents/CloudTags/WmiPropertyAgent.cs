using System;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Model;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents.CloudTags
{
	// Token: 0x0200002F RID: 47
	public class WmiPropertyAgent : ICloudTagProvider
	{
		// Token: 0x06000136 RID: 310 RVA: 0x0000974C File Offset: 0x0000794C
		public async Task<Tag> GetCloudTagAsync(TagRule tagRule, string tagName)
		{
			Tag tag = null;
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "WmiPropertyAgent: Tag Rule execution for Tag Group " + tagName + " is begining!");
				await this._wmiPropertySemaphore.WaitAsync();
				string targetReqValue = tagRule.TargetReqValue;
				if (!string.IsNullOrEmpty(targetReqValue))
				{
					using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from " + tagRule.TargetPath.ToString()))
					{
						ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
						if (managementObjectCollection != null && managementObjectCollection.Count > 0)
						{
							using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectCollection.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									if (((ManagementObject)enumerator.Current)[tagRule.TargetName].ToString() == targetReqValue)
									{
										tag = new Tag(tagName, string.Empty);
									}
								}
								goto IL_159;
							}
						}
						Logger.Log(Logger.LogSeverity.Information, "WmiPropertyAgent: Query returned empty results");
						IL_159:
						goto IL_176;
					}
				}
				Logger.Log(Logger.LogSeverity.Information, "WmiPropertyAgent: TargetReqValue is empty");
				IL_176:;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "WmiPropertyAgent: Error caught!!");
			}
			finally
			{
				this._wmiPropertySemaphore.Release();
			}
			return tag;
		}

		// Token: 0x04000088 RID: 136
		private readonly SemaphoreSlim _wmiPropertySemaphore = new SemaphoreSlim(1);
	}
}
