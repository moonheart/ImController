using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.CoreTypes.Utilities;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Services.Output
{
	// Token: 0x02000038 RID: 56
	internal class OutputWriter
	{
		// Token: 0x06000150 RID: 336 RVA: 0x0000A1FC File Offset: 0x000083FC
		public Task<bool> WriteFileAsync(AppAndTagCollection installedItemsAndTags, DirectoryInfo outputDirectory)
		{
			return Task.Factory.StartNew<bool>(delegate()
			{
				bool result = false;
				if (installedItemsAndTags != null && outputDirectory != null)
				{
					if (!outputDirectory.Exists)
					{
						Logger.Log(Logger.LogSeverity.Information, "Output location did not exist, will attempt to create.");
					}
					try
					{
						string contents = Serializer.Serialize<AppAndTagCollection>(installedItemsAndTags);
						File.WriteAllText(Path.Combine(outputDirectory.FullName, "AppsAndTags.xml"), contents, new UTF8Encoding(false));
						result = true;
						Logger.Log(Logger.LogSeverity.Information, "{0} Was written to {1}", new object[] { "AppsAndTags.xml", outputDirectory.FullName });
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "Exception while serializing/writing {0} to {1}", new object[] { "AppsAndTags.xml", outputDirectory.FullName });
						result = false;
					}
				}
				return result;
			});
		}
	}
}
