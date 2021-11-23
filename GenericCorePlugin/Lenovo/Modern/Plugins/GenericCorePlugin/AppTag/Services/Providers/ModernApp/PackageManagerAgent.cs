using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Utilities.Services.Logging;
using Windows.ApplicationModel;
using Windows.Management.Deployment;
using Windows.Storage;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.ModernApp
{
	// Token: 0x02000035 RID: 53
	internal class PackageManagerAgent
	{
		// Token: 0x06000141 RID: 321 RVA: 0x00002048 File Offset: 0x00000248
		internal PackageManagerAgent()
		{
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00009B90 File Offset: 0x00007D90
		public Task<IEnumerable<ModernAppInformation>> CollectAppListAsync()
		{
			return this.GetInstalledPackagesAsync();
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00009B98 File Offset: 0x00007D98
		private Task<IEnumerable<ModernAppInformation>> GetInstalledPackagesAsync()
		{
			return Task.Factory.StartNew<IEnumerable<ModernAppInformation>>(delegate()
			{
				List<ModernAppInformation> list = new List<ModernAppInformation>();
				try
				{
					string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages");
					PackageManager packageManager = new PackageManager();
					string text = null;
					IEnumerable<Package> enumerable = null;
					try
					{
						WindowsIdentity current = WindowsIdentity.GetCurrent();
						string text2;
						if (current == null)
						{
							text2 = null;
						}
						else
						{
							SecurityIdentifier user = current.User;
							text2 = ((user != null) ? user.Value : null);
						}
						text = text2;
						PackageManager packageManager2 = packageManager;
						WindowsIdentity current2 = WindowsIdentity.GetCurrent();
						string text3;
						if (current2 == null)
						{
							text3 = null;
						}
						else
						{
							SecurityIdentifier user2 = current2.User;
							text3 = ((user2 != null) ? user2.Value : null);
						}
						enumerable = packageManager2.FindPackagesForUser(text3);
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "Identity issue while getting packages for current user");
					}
					Logger.Log(Logger.LogSeverity.Information, "Collecting Packages for: {0}", new object[] { text ?? "null" });
					if (enumerable != null && enumerable.Any<Package>())
					{
						using (IEnumerator<Package> enumerator = enumerable.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Package package = enumerator.Current;
								try
								{
									ModernAppInformation modernAppInformation = new ModernAppInformation
									{
										Version = new Version((int)package.Id.Version.Major, (int)package.Id.Version.Minor, (int)package.Id.Version.Build, (int)package.Id.Version.Revision).ToString(),
										DataDirectory = Path.Combine(path, package.Id.FamilyName),
										FamilyName = package.Id.FamilyName,
										FullName = package.Id.FullName,
										Name = package.Id.Name,
										Publisher = package.Id.PublisherId
									};
									try
									{
										if (package.InstalledLocation != null)
										{
											modernAppInformation.InstallDirectory = package.InstalledLocation.Path;
											FileInfo manifestFile = PackageManagerAgent.GetManifestFile(package.InstalledLocation);
											modernAppInformation.Protocol = AppxManifestProcessor.GetProtocol(manifestFile);
											modernAppInformation.AppUserModelId = AppxManifestProcessor.GetAppUserModelId(manifestFile, modernAppInformation.FamilyName);
										}
									}
									catch (Exception ex2)
									{
										Logger.Log(Logger.LogSeverity.Warning, "Unable to process package install location details for {0}. Exception: {1}", new object[]
										{
											package.Id.Name,
											ex2.GetType()
										});
									}
									list.Add(modernAppInformation);
								}
								catch (Exception ex3)
								{
									Logger.Log(Logger.LogSeverity.Warning, "Unable to load information for package: ({0}). Exception: {1}", new object[]
									{
										package.Id.FullName,
										ex3.GetType()
									});
								}
							}
							goto IL_251;
						}
					}
					Logger.Log(Logger.LogSeverity.Warning, "No modern packages collected");
					IL_251:;
				}
				catch (Exception ex4)
				{
					Logger.Log(ex4, "Exception while loading list of installed packages");
				}
				return list;
			});
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00009BC4 File Offset: 0x00007DC4
		private static FileInfo GetManifestFile(IStorageFolder folder)
		{
			FileInfo result = null;
			if (folder != null)
			{
				result = new FileInfo(Path.Combine(folder.Path, "AppxManifest.xml"));
			}
			return result;
		}

		// Token: 0x02000096 RID: 150
		public static class Constants
		{
			// Token: 0x04000238 RID: 568
			public const string PackagesDirectory = "Packages";
		}
	}
}
