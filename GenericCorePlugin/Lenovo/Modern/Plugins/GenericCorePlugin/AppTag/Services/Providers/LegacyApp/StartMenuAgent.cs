using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using IWshRuntimeLibrary;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Providers.LegacyApp
{
	// Token: 0x02000036 RID: 54
	internal class StartMenuAgent
	{
		// Token: 0x06000145 RID: 325 RVA: 0x00002048 File Offset: 0x00000248
		internal StartMenuAgent()
		{
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00009BF0 File Offset: 0x00007DF0
		public async Task<IEnumerable<LegacyAppInformation>> CollectAppListAsync()
		{
			List<LegacyAppInformation> list = new List<LegacyAppInformation>();
			try
			{
				IEnumerable<LegacyAppInformation> enumerable = await this.CollectAppListAsync(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu)));
				if (enumerable != null)
				{
					list.AddRange(enumerable);
				}
				DirectoryInfo directory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\\Windows\\Start Menu\\Programs"));
				IEnumerable<LegacyAppInformation> enumerable2 = await this.CollectAppListAsync(directory);
				if (enumerable2 != null)
				{
					list.AddRange(enumerable2);
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to collect start menu items");
			}
			return list;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00009C38 File Offset: 0x00007E38
		private Task<IEnumerable<LegacyAppInformation>> CollectAppListAsync(DirectoryInfo directory)
		{
			List<LegacyAppInformation> list = new List<LegacyAppInformation>();
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "Collecting apps inside {0}", new object[] { (directory != null) ? directory.FullName : "null" });
				if (directory != null && directory.Exists)
				{
					List<FileInfo> list2 = new List<FileInfo>();
					StartMenuAgent.GetFilesRecursively(list2, directory, "*.lnk");
					if (list2 == null || !list2.Any<FileInfo>())
					{
						goto IL_243;
					}
					using (List<FileInfo>.Enumerator enumerator = list2.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							FileInfo fileInfo = enumerator.Current;
							try
							{
								IWshShortcut wshShortcut = null;
								try
								{
									wshShortcut = ((WshShell)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")))).CreateShortcut(fileInfo.FullName) as IWshShortcut;
								}
								catch (Exception ex)
								{
									Logger.Log(ex, "Unable to process shortcut for file {0}", new object[] { fileInfo.FullName });
								}
								if (wshShortcut != null && !string.IsNullOrWhiteSpace(wshShortcut.TargetPath))
								{
									Version version = null;
									try
									{
										FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(wshShortcut.TargetPath);
										if (versionInfo != null && !string.IsNullOrWhiteSpace(versionInfo.ProductVersion))
										{
											version = new Version(versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductBuildPart, versionInfo.ProductPrivatePart);
										}
									}
									catch (FileNotFoundException)
									{
									}
									catch (Exception ex2)
									{
										Logger.Log(ex2, "Unable to find product version for file: {0}", new object[] { (wshShortcut != null) ? wshShortcut.TargetPath : "null" });
									}
									list.Add(new LegacyAppInformation
									{
										DisplayName = Path.GetFileNameWithoutExtension(fileInfo.FullName),
										ExecutableName = Path.GetFileName(wshShortcut.TargetPath),
										InstallDirectory = Path.GetDirectoryName(wshShortcut.TargetPath),
										LaunchCommand = fileInfo.FullName,
										LaunchArguments = (wshShortcut.Arguments ?? string.Empty),
										DateModified = fileInfo.LastWriteTime,
										Version = ((version != null) ? version.ToString() : "0.0.0.0")
									});
								}
							}
							catch (Exception ex3)
							{
								Logger.Log(ex3, "Unable to process start menu item {0}, Exception: {1}", new object[]
								{
									fileInfo.FullName,
									ex3.GetType()
								});
							}
						}
						goto IL_243;
					}
				}
				Logger.Log(Logger.LogSeverity.Error, "Start menu directory null or invalid: {0}", new object[] { (directory != null) ? directory.FullName : "null" });
				IL_243:;
			}
			catch (Exception ex4)
			{
				Logger.Log(ex4, "Unable to gather all legacy apps from start menu");
			}
			return Task.FromResult<IEnumerable<LegacyAppInformation>>(list);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00009F30 File Offset: 0x00008130
		private static void GetFilesRecursively(List<FileInfo> files, DirectoryInfo directory, string searchPattern)
		{
			files = files ?? new List<FileInfo>();
			searchPattern = searchPattern ?? "*";
			if (files == null || directory == null || searchPattern == null)
			{
				throw new ArgumentNullException();
			}
			string fullName = directory.FullName;
			if ((File.GetAttributes(fullName) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
			{
				try
				{
					files.AddRange(from filePath in Directory.GetFiles(fullName, searchPattern).AsEnumerable<string>()
						select new FileInfo(filePath));
				}
				catch (UnauthorizedAccessException)
				{
					Logger.Log(Logger.LogSeverity.Warning, "UnauthorizedAccess while getting files from:  {0}", new object[] { fullName });
				}
				foreach (string text in Directory.GetDirectories(fullName))
				{
					try
					{
						StartMenuAgent.GetFilesRecursively(files, new DirectoryInfo(text), searchPattern);
					}
					catch (UnauthorizedAccessException)
					{
						Logger.Log(Logger.LogSeverity.Warning, "UnauthorizedAccess while processing folder:  {0}", new object[] { text });
					}
				}
			}
		}

		// Token: 0x02000098 RID: 152
		internal static class Constants
		{
			// Token: 0x0400023B RID: 571
			public const string ShortcutExtension = ".lnk";
		}
	}
}
