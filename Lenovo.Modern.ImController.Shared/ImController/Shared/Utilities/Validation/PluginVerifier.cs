using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Lenovo.Modern.CoreTypes.Models;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Model.Plugin;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Validation;

namespace Lenovo.Modern.ImController.Shared.Utilities.Validation
{
	// Token: 0x0200003C RID: 60
	public class PluginVerifier : IPluginVerifier
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x00008410 File Offset: 0x00006610
		public void VerifyAllPluginsInDirectory(string path)
		{
			if (Constants.IsSecurityDisabled)
			{
				Logger.Log(Logger.LogSeverity.Critical, "Warning: Security has been disabled for plugin dir");
				return;
			}
			if (!Utility.SanitizePath(ref path))
			{
				throw new Exception(string.Format("VerifyAllPluginsInDirectory: Exception thrown while verifying all plugins in directory as path is invalid. Path - {0}", path));
			}
			this.packagePath = path;
			this._fileCollection.Clear();
			if (!string.IsNullOrEmpty(path))
			{
				if (this.IsPlatformFolderExists(path))
				{
					this.GetFileList(path);
					if (this._fileCollection.Any<string>())
					{
						string name = new DirectoryInfo(path).Name;
						this.ValidateDll(name);
						this.ValidateManifest(name);
						return;
					}
				}
				throw new PluginRepositoryException("Platform folder does not exists in the" + path)
				{
					ResponseCode = 212
				};
			}
			throw new PluginRepositoryException("File does not exists in the " + path)
			{
				ResponseCode = 205
			};
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000084D4 File Offset: 0x000066D4
		public void VerifyPlugin(string pluginFullPath)
		{
			if (Constants.IsSecurityDisabled)
			{
				Logger.Log(Logger.LogSeverity.Critical, "Warning: Security has been disabled for validating a plugin");
				return;
			}
			if (!Utility.SanitizePath(ref pluginFullPath))
			{
				throw new Exception(string.Format("VerifyPlugin: Exception thrown while verifying plugin as pluginfullpath is invalid. Path - {0}", pluginFullPath));
			}
			if (!File.Exists(pluginFullPath))
			{
				throw new PluginRepositoryException(pluginFullPath + " does not exists")
				{
					ResponseCode = 205
				};
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pluginFullPath);
			if (!pluginFullPath.Split(new char[] { '\\' }).Contains(fileNameWithoutExtension))
			{
				throw new PluginRepositoryException(pluginFullPath + " is not a Plugin")
				{
					ResponseCode = 202
				};
			}
			(new string[1])[0] = pluginFullPath;
			if (!((IImcCertificateValidator)new ImcCertificateValidator()).AssertDigitalSignatureIsValid(pluginFullPath))
			{
				throw new PluginRepositoryException(fileNameWithoutExtension + " is not signed by Lenovo")
				{
					ResponseCode = 206
				};
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000085A4 File Offset: 0x000067A4
		public IDictionary<string, bool> GetPluginInspectionResults(string extractedPath)
		{
			IDictionary<string, bool> result = new Dictionary<string, bool>();
			this.packagePath = extractedPath;
			if (Constants.IsSecurityDisabled)
			{
				return result;
			}
			if (!Utility.SanitizePath(ref extractedPath))
			{
				throw new Exception(string.Format("GetPluginInspectionResults: Exception thrown while getting plugin inspection results as extracted path is invalid. Path - {0}", extractedPath));
			}
			string name = new DirectoryInfo(extractedPath).Name;
			this._fileCollection.Clear();
			this.GetFileList(extractedPath);
			if (this._fileCollection.Any<string>())
			{
				this.GetDllInspectionResults(ref result, name);
				this.GetManifestInspectionResult(ref result, name);
			}
			return result;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008620 File Offset: 0x00006820
		private void GetFileList(string path)
		{
			if (!Utility.SanitizePath(ref path))
			{
				throw new Exception(string.Format("GetFileList: Exception thrown while getting file list as path is invalid. Path - {0}", path));
			}
			foreach (string item in Directory.GetFiles(path))
			{
				this._fileCollection.Add(item);
			}
			foreach (string path2 in Directory.GetDirectories(path))
			{
				this.GetFileList(path2);
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000868C File Offset: 0x0000688C
		private void ValidateDll(string packagename)
		{
			if (Constants.IsSecurityDisabled)
			{
				Logger.Log(Logger.LogSeverity.Critical, "Warning: Security has been disabled for dll");
				return;
			}
			List<string> list = (from x in this._fileCollection
				where x.Contains(Constants.PluginExtension)
				select x).ToList<string>();
			if (!list.Any<string>())
			{
				throw new PluginRepositoryException("Plugin not found in the package: " + packagename)
				{
					ResponseCode = 202
				};
			}
			string[] array = new string[this.GetPlatformFolderCount(this.packagePath)];
			if (!this.AllBinariesAreSignedByLenovo(list))
			{
				throw new PluginRepositoryException("The Binary is not signed by Lenovo.")
				{
					ResponseCode = 206
				};
			}
			if (!this.IsPluginExists(packagename, ref array))
			{
				throw new PluginRepositoryException(string.Format("There is no {0} plugin in the package.", packagename))
				{
					ResponseCode = 202
				};
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00008758 File Offset: 0x00006958
		private bool AllBinariesAreSignedByLenovo(List<string> dllFiles)
		{
			if (Constants.IsSecurityDisabled)
			{
				Logger.Log(Logger.LogSeverity.Critical, "Warning: Security has been disabled for multiple dlls");
				return true;
			}
			bool result = true;
			IImcCertificateValidator imcCertificateValidator = new ImcCertificateValidator();
			foreach (string filePath in dllFiles)
			{
				if (!imcCertificateValidator.AssertDigitalSignatureIsValid(filePath))
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000087CC File Offset: 0x000069CC
		private bool IsPluginExists(string packageName, ref string[] pluginPath)
		{
			bool result = false;
			List<string> list = (from x in this._fileCollection
				where x.EndsWith(packageName + Constants.PluginExtension)
				select x).ToList<string>();
			if (list.Any<string>() && list.Count == pluginPath.Length)
			{
				for (int i = 0; i < list.Count; i++)
				{
					pluginPath[i] = list[i].ToString();
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00008840 File Offset: 0x00006A40
		private void AssertPluginHasPluginEntry(string[] pluginPath)
		{
			Assembly assembly = null;
			for (int i = 0; i < pluginPath.Count<string>(); i++)
			{
				try
				{
					assembly = Assembly.Load(AssemblyName.GetAssemblyName(pluginPath[i]));
				}
				catch (Exception)
				{
					throw new Exception(string.Format("Could not load assembly: {0}", pluginPath[i]));
				}
				if (null != assembly && assembly.GetTypes() != null)
				{
					if (assembly.GetTypes().FirstOrDefault((Type t) => t.IsPublic && t.IsClass && t.Name == "PluginEntry") == null)
					{
						throw new Exception(string.Format("Assembly {0} does not contain a PluginEntry.", pluginPath[i]));
					}
				}
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000088F0 File Offset: 0x00006AF0
		private bool IsPlatformFolderExists(string packagePath)
		{
			bool result = false;
			string text = Utility.SanitizePath1(packagePath);
			if (string.IsNullOrEmpty(text))
			{
				throw new Exception(string.Format("IsPlatformFolderExists: Package path is invalid/tainted. Path : {0}", packagePath));
			}
			string[] directories = Directory.GetDirectories(text, "*", SearchOption.AllDirectories);
			for (int i = 0; i < directories.Length; i++)
			{
				string name = new DirectoryInfo(directories[i]).Name;
				if (name.ToUpper() == Constants.X64folder.ToUpper() || name.ToUpper() == Constants.X86folder.ToUpper())
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000897C File Offset: 0x00006B7C
		private int GetPlatformFolderCount(string packagePath)
		{
			int num = 0;
			string text = Utility.SanitizePath1(packagePath);
			if (string.IsNullOrEmpty(text))
			{
				throw new Exception(string.Format("GetPlatformFolderCount: Package path is invalid/tainted. Path : {0}", packagePath));
			}
			string[] directories = Directory.GetDirectories(text, "*", SearchOption.AllDirectories);
			for (int i = 0; i < directories.Length; i++)
			{
				string name = new DirectoryInfo(directories[i]).Name;
				if (name == Constants.X64folder || name == Constants.X86folder)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000089F4 File Offset: 0x00006BF4
		private void ValidateManifest(string packageName)
		{
			List<string> list = (from x in this._fileCollection
				where x.ToLower().Contains("manifest.xml")
				select x).ToList<string>();
			if (!list.Any<string>())
			{
				throw new PluginRepositoryException(string.Format("No Manifest file in the {0} package.", packageName))
				{
					ResponseCode = 208
				};
			}
			string name = new DirectoryInfo(list[0].ToString()).Name;
			if (!this.IsManifestSignedByLenovo(list[0].ToString()))
			{
				throw new PluginRepositoryException(string.Format("Manifest file {0} is not signed by Lenovo.", name))
				{
					ResponseCode = 209
				};
			}
			if (this.IsValidManifest(list[0].ToString()))
			{
				return;
			}
			throw new PluginRepositoryException(string.Format("Manifest file {0} is not valid .", name))
			{
				ResponseCode = 210
			};
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00008ACC File Offset: 0x00006CCC
		private bool IsManifestSignedByLenovo(string manifestPath)
		{
			bool result = false;
			if (!Utility.SanitizePath(ref manifestPath))
			{
				throw new Exception(string.Format("IsManifestSignedByLenovo: Exception thrown while validating signature of manifest as path is invalid. Path - {0}", manifestPath));
			}
			if (((IImcCertificateValidator)new ImcCertificateValidator()).IsXmlValid(manifestPath))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00008B08 File Offset: 0x00006D08
		public bool IsValidManifest(string manifestPath)
		{
			bool flag = true;
			bool result;
			try
			{
				if (!Utility.SanitizePath(ref manifestPath))
				{
					throw new Exception(string.Format("IsValidManifest: Exception thrown while validating manifest as path is invalid. Path - {0}", manifestPath));
				}
				using (StreamReader streamReader = new StreamReader(manifestPath))
				{
					string xml = streamReader.ReadToEnd();
					streamReader.Close();
					PluginManifest pluginManifest = Serializer.Deserialize<PluginManifest>(xml);
					if (pluginManifest == null || pluginManifest.SubscribedEventList == null || pluginManifest.PluginInformation == null || pluginManifest.ContractMappingList == null || pluginManifest.ApplicabilityFilter == null || pluginManifest.SubscribedEventList.Any<SubscribedEvent>() || pluginManifest.ContractMappingList.Any<ContractMapping>() || pluginManifest.ApplicabilityFilter.Any<Filter>())
					{
						flag = false;
					}
				}
				result = flag;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00008BC8 File Offset: 0x00006DC8
		private void GetDllInspectionResults(ref IDictionary<string, bool> inspectionResult, string packageName)
		{
			List<string> list = (from x in this._fileCollection
				where x.Contains(Constants.PluginExtension)
				select x).ToList<string>();
			if (this.IsPlatformFolderExists(this.packagePath))
			{
				string[] array = new string[this.GetPlatformFolderCount(this.packagePath)];
				inspectionResult.Add(Constants.IsPlatformFolderExists, true);
				if (list.Any<string>())
				{
					if (!this.AllBinariesAreSignedByLenovo(list))
					{
						inspectionResult.Add(Constants.BinariesSigned, false);
					}
					if (this.IsPluginExists(packageName, ref array))
					{
						try
						{
							inspectionResult.Add(Constants.IsPluginExists, true);
							return;
						}
						catch (Exception)
						{
							inspectionResult.Add(Constants.IsPluginValid, false);
							return;
						}
					}
					inspectionResult.Add(Constants.IsPluginExists, false);
					return;
				}
				inspectionResult.Add(Constants.IsPluginExists, false);
				return;
			}
			else
			{
				inspectionResult.Add(Constants.IsPlatformFolderExists, false);
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00008CB8 File Offset: 0x00006EB8
		private void GetManifestInspectionResult(ref IDictionary<string, bool> inspectionResult, string packageName)
		{
			List<string> list = (from x in this._fileCollection
				where x.ToLower().Contains("manifest.xml")
				select x).ToList<string>();
			if (list.Any<string>())
			{
				inspectionResult.Add(Constants.ManifestExists, true);
				inspectionResult.Add(Constants.ManifestSigned, this.IsManifestSignedByLenovo(list[0].ToString()));
				inspectionResult.Add(Constants.ManifestValid, this.IsValidManifest(list[0].ToString()));
				return;
			}
			inspectionResult.Add(Constants.ManifestExists, false);
		}

		// Token: 0x040000E5 RID: 229
		private IList<string> _fileCollection = new List<string>();

		// Token: 0x040000E6 RID: 230
		private string packagePath = "";
	}
}
