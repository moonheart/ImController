using System;
using System.Diagnostics;
using System.IO;

namespace Lenovo.Modern.ImController.Shared.Utilities
{
	// Token: 0x02000035 RID: 53
	public class PackageExtractor
	{
		// Token: 0x0600018A RID: 394 RVA: 0x00007E88 File Offset: 0x00006088
		public static bool UnzipPackage(string sourcePath, string destinationPath)
		{
			bool result = false;
			try
			{
				if (!Utility.SanitizePath(ref sourcePath))
				{
					throw new Exception(string.Format("UnzipPackage: Exception thrown while unzip package as sourcePath is invalid. Path - {0}", sourcePath));
				}
				if (!Utility.SanitizePath(ref destinationPath))
				{
					throw new Exception(string.Format("UnzipPackage: Exception thrown while unzip package as destinationPath is invalid. Path - {0}", destinationPath));
				}
				string fileName = string.Format("{0}\\expand.exe", Environment.SystemDirectory);
				string arguments = string.Format("-F:* {0} {1}", sourcePath, destinationPath);
				ProcessStartInfo processStartInfo = new ProcessStartInfo();
				processStartInfo.FileName = fileName;
				processStartInfo.Arguments = arguments;
				processStartInfo.CreateNoWindow = true;
				processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				processStartInfo.UseShellExecute = false;
				Process process = new Process();
				process.StartInfo = processStartInfo;
				process.EnableRaisingEvents = true;
				if (!Directory.Exists(destinationPath))
				{
					Directory.CreateDirectory(destinationPath);
				}
				process.Start();
				process.WaitForExit();
				result = true;
			}
			catch (Exception)
			{
				throw new Exception("Exception thrown trying to extract package from the" + sourcePath + "path");
			}
			return result;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00007F6C File Offset: 0x0000616C
		public static bool CreatePackage(string sourcePath, string savePath)
		{
			bool result = false;
			try
			{
				if (!Utility.SanitizePath(ref sourcePath))
				{
					throw new Exception(string.Format("CreatePackage: Exception thrown while creating package as sourcePath is invalid. Path - {0}", sourcePath));
				}
				if (!Utility.SanitizePath(ref savePath))
				{
					throw new Exception(string.Format("CreatePackage: Exception thrown while creating package as destinationPath is invalid. Path - {0}", savePath));
				}
				string fileName = string.Format("{0}\\makecab.exe", Environment.SystemDirectory);
				string arguments = string.Format("/D DiskDirectoryTemplate={0} /L {1} {2}", sourcePath, savePath, sourcePath);
				ProcessStartInfo processStartInfo = new ProcessStartInfo();
				processStartInfo.FileName = fileName;
				processStartInfo.Arguments = arguments;
				processStartInfo.RedirectStandardOutput = true;
				processStartInfo.RedirectStandardError = true;
				processStartInfo.UseShellExecute = false;
				processStartInfo.CreateNoWindow = true;
				new Process
				{
					StartInfo = processStartInfo,
					EnableRaisingEvents = true
				}.Start();
				result = true;
			}
			catch (Exception)
			{
				throw new Exception("Exception thrown trying to create package from the" + sourcePath + "path");
			}
			return result;
		}
	}
}
