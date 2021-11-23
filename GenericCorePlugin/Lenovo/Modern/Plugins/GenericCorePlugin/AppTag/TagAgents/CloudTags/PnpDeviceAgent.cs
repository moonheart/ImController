using System;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Model;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents.CloudTags
{
	// Token: 0x0200002D RID: 45
	public class PnpDeviceAgent : ICloudTagProvider
	{
		// Token: 0x06000132 RID: 306 RVA: 0x00009674 File Offset: 0x00007874
		public async Task<Tag> GetCloudTagAsync(TagRule tagRule, string tagName)
		{
			Tag tag = null;
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "PnpDeviceAgent: Tag Rule execution for Tag Group " + tagName + " is begining!");
				await this._pnpDeviceAgentSemaphore.WaitAsync();
				string[] array = tagRule.TargetPath.ToString().Split(new char[] { '|' });
				if (array.Length == 2 && !string.IsNullOrEmpty(tagRule.TargetReqValue))
				{
					using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("\\root\\cimv2", string.Concat(new string[]
					{
						"Select * from ",
						tagRule.TargetName,
						" where DeviceClass='",
						array[0],
						"'"
					})))
					{
						ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
						string[] array2 = tagRule.TargetReqValue.Split(new char[] { '|' });
						foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
						{
							object obj = ((ManagementObject)managementBaseObject)[array[1]];
							if (array2.Length > 1)
							{
								foreach (string value in array2)
								{
									if (obj.ToString().Contains(value))
									{
										tag = new Tag(tagName, string.Empty);
									}
								}
							}
							else if (obj.ToString().Contains(tagRule.TargetReqValue))
							{
								tag = new Tag(tagName, string.Empty);
							}
						}
						goto IL_236;
					}
				}
				Logger.Log(Logger.LogSeverity.Information, "PnpDeviceAgent: Target Path " + tagRule.TargetPath + " is not defined properly");
				IL_236:;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "PnpDeviceAgent: Error caught!!");
			}
			finally
			{
				this._pnpDeviceAgentSemaphore.Release();
			}
			return tag;
		}

		// Token: 0x04000086 RID: 134
		private readonly SemaphoreSlim _pnpDeviceAgentSemaphore = new SemaphoreSlim(1);
	}
}
