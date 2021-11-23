using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Model;
using Lenovo.Modern.Utilities.Services.Logging;
using Windows.ApplicationModel;
using Windows.Management.Deployment;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.TagAgents.CloudTags
{
	// Token: 0x02000027 RID: 39
	public class AppXAgent : ICloudTagProvider
	{
		// Token: 0x0600011B RID: 283 RVA: 0x000090C0 File Offset: 0x000072C0
		public async Task<Tag> GetCloudTagAsync(TagRule tagRule, string tagName)
		{
			Tag tag = null;
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "AppXAgent: Tag Rule execution for Tag Group " + tagName + " is begining!");
				if (!string.IsNullOrEmpty(tagRule.TargetName))
				{
					await this._appXAgentSemaphore.WaitAsync();
					Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages");
					PackageManager packageManager = new PackageManager();
					IEnumerable<Package> enumerable = null;
					try
					{
						WindowsIdentity current = WindowsIdentity.GetCurrent();
						string text;
						if (current == null)
						{
							text = null;
						}
						else
						{
							SecurityIdentifier user = current.User;
							text = ((user != null) ? user.Value : null);
						}
						string text2 = text;
						if (!string.IsNullOrEmpty(text2))
						{
							enumerable = packageManager.FindPackagesForUser(text2, tagRule.TargetName);
						}
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "AppXAgent: Identity issue while getting " + tagRule.TargetName + " package for current user");
					}
					if (enumerable != null && enumerable.Any<Package>())
					{
						Version version = ((!string.IsNullOrEmpty(tagRule.TargetReqValue.ToString())) ? new Version(tagRule.TargetReqValue.ToString()) : null);
						using (IEnumerator<Package> enumerator = enumerable.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Package package = enumerator.Current;
								try
								{
									string text3 = tagRule.TargetValueRule.ToString();
									if (!string.IsNullOrEmpty(text3) && text3 == Constants.TagValueRules.condition.ToString())
									{
										Version v = new Version((int)package.Id.Version.Major, (int)package.Id.Version.Minor, (int)package.Id.Version.Build, (int)package.Id.Version.Revision);
										if (v != null && version != null)
										{
											string a = tagRule.TargetValueCondition.ToString();
											if (a == Constants.TagValueConditions.greaterThanOrEqual.ToString())
											{
												if (v >= version)
												{
													tag = new Tag(tagName, string.Empty);
												}
											}
											else if (a == Constants.TagValueConditions.lessThanOrEqual.ToString())
											{
												if (v <= version)
												{
													tag = new Tag(tagName, string.Empty);
												}
											}
											else if (a == Constants.TagValueConditions.equal.ToString() && v == version)
											{
												tag = new Tag(tagName, string.Empty);
											}
										}
									}
									else if (!string.IsNullOrEmpty(text3) && text3 == Constants.TagValueRules.onlyTag.ToString() && (package.Id.FamilyName == tagRule.TargetName.ToString() || package.Id.Name == tagRule.TargetName.ToString()))
									{
										tag = new Tag(tagName, string.Empty);
									}
								}
								catch (Exception ex2)
								{
									Logger.Log(Logger.LogSeverity.Warning, string.Format("AppXAgent: Unable to load information for package: ({0}). Exception: {1}", package.Id.FullName, ex2.GetType()));
								}
								if (tag != null)
								{
									return tag;
								}
							}
							goto IL_3DD;
						}
					}
					Logger.Log(Logger.LogSeverity.Warning, "AppXAgent: No modern packages collected");
				}
				IL_3DD:;
			}
			catch (Exception ex3)
			{
				Logger.Log(ex3, "AppXAgent: Exception caught!!");
			}
			finally
			{
				this._appXAgentSemaphore.Release();
			}
			return tag;
		}

		// Token: 0x04000073 RID: 115
		private readonly SemaphoreSlim _appXAgentSemaphore = new SemaphoreSlim(1);
	}
}
