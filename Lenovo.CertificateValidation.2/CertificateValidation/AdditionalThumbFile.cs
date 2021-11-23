using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml;
using Lenovo.Tools.Logging;

namespace Lenovo.CertificateValidation
{
	// Token: 0x02000003 RID: 3
	public class AdditionalThumbFile
	{
		// Token: 0x06000002 RID: 2 RVA: 0x0000205C File Offset: 0x0000025C
		private static XmlDocument GetXmlDocFromCache(string cachingFile)
		{
			XmlDocument xmlDocument = AdditionalThumbFile.GetXmlDocument(cachingFile);
			if (xmlDocument != null)
			{
				Logger.Log(Logger.LogSeverity.Information, "Cache file exist, validate the content.");
				if (new XMLFileValidator().GetTrustStatus(xmlDocument) != TrustStatus.FileTrusted)
				{
					Logger.Log(Logger.LogSeverity.Information, "Cache file is not valid.");
					xmlDocument = null;
				}
			}
			return xmlDocument;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000209C File Offset: 0x0000029C
		private static bool ShouldIUpdate(string eTagFile)
		{
			if (!File.Exists(eTagFile))
			{
				Logger.Log(Logger.LogSeverity.Information, "Etag not exist, should update it.");
				return true;
			}
			try
			{
				HttpWebRequest httpWebRequest = WebRequest.CreateHttp(AdditionalThumbFile.TrustedAdditionalile);
				httpWebRequest.Method = "HEAD";
				foreach (string text in File.ReadLines(eTagFile))
				{
					if (text.Contains("Etag:"))
					{
						string value = text.Substring("Etag:".Length);
						httpWebRequest.Headers.Add(HttpRequestHeader.IfNoneMatch, value);
					}
					if (text.Contains("Last-Modified:"))
					{
						string value = text.Substring("Last-Modified:".Length);
						httpWebRequest.IfModifiedSince = Convert.ToDateTime(value);
					}
				}
				using (httpWebRequest.GetResponse())
				{
					Logger.Log(Logger.LogSeverity.Information, "New version of additional thumbprint file, should update.");
					return true;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Information, "There is no update for additional thumbprint file {0}.", new object[] { ex.Message });
			}
			return false;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000021C4 File Offset: 0x000003C4
		private static XmlDocument Fetch3rdPartyFile(string cachingFile)
		{
			XmlDocument xmlDocument = null;
			try
			{
				bool flag = AdditionalThumbFile.ShouldIUpdate(Path.GetDirectoryName(cachingFile) + "\\etag");
				if (!flag)
				{
					xmlDocument = AdditionalThumbFile.GetXmlDocFromCache(cachingFile);
				}
				if (flag || xmlDocument == null)
				{
					Logger.Log(Logger.LogSeverity.Information, "Download xml file.");
					xmlDocument = AdditionalThumbFile.DownloadFile(cachingFile);
				}
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Error, "Fetch3rdPartyFile exception: {0}", new object[] { ex.Message });
				DebugInfo.Output("Fetch3rdPartyFile exception:" + ex.Message, true);
			}
			return xmlDocument;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002250 File Offset: 0x00000450
		private static XmlDocument DownloadFile(string cachingFile)
		{
			XmlDocument xmlDocument = null;
			try
			{
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)WebRequest.CreateHttp(AdditionalThumbFile.TrustedAdditionalile).GetResponse())
				{
					Logger.Log(Logger.LogSeverity.Information, "Get response, update etag file.");
					string path = Path.GetDirectoryName(cachingFile) + "\\etag";
					string text = "Etag:" + httpWebResponse.GetResponseHeader("ETag");
					string text2 = "Last-Modified:" + httpWebResponse.GetResponseHeader("Last-Modified");
					string[] contents = new string[] { text, text2 };
					if (File.Exists(path))
					{
						File.Delete(path);
					}
					File.WriteAllLines(path, contents);
					using (Stream responseStream = httpWebResponse.GetResponseStream())
					{
						Logger.Log(Logger.LogSeverity.Information, "Get response stream, update caching file.");
						if (File.Exists(cachingFile))
						{
							File.Delete(cachingFile);
						}
						xmlDocument = new XmlDocument
						{
							PreserveWhitespace = true,
							XmlResolver = null
						};
						xmlDocument.Load(responseStream);
						xmlDocument.Save(cachingFile);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Information, "DownloadFile exception: {0}", new object[] { ex.Message });
				DebugInfo.Output("Fetch3rdPartyFile exception:" + ex.Message, true);
			}
			return xmlDocument;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000023A4 File Offset: 0x000005A4
		private static void GetThumbPrints(XmlDocument xmlDoc, out List<string> thumbPirnts)
		{
			thumbPirnts = new List<string>();
			XmlElement xmlElement = ((xmlDoc != null) ? xmlDoc.DocumentElement : null);
			XmlNodeList xmlNodeList = ((xmlElement != null) ? xmlElement.SelectNodes("/LenovoCV/AdditionalThumbPrints/ThumbPrint") : null);
			if (xmlNodeList != null && xmlNodeList.Count > 0)
			{
				foreach (object obj in xmlNodeList)
				{
					XmlNode xmlNode = (XmlNode)obj;
					string innerText = xmlNode.InnerText;
					if (!string.IsNullOrEmpty((innerText != null) ? innerText.Trim() : null))
					{
						Logger.Log(Logger.LogSeverity.Information, "Add additional thumb print {0}", new object[] { xmlNode.InnerText.Trim() });
						thumbPirnts.Add(xmlNode.InnerText.Trim());
					}
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002470 File Offset: 0x00000670
		public static bool GetAdditionalThumbPrints(string cachingFolder)
		{
			XMLFileValidator xmlfileValidator = new XMLFileValidator();
			if (cachingFolder.Equals(Path.GetTempPath(), StringComparison.InvariantCultureIgnoreCase))
			{
				Logger.Log(Logger.LogSeverity.Information, "Fetch 3rd party file fail, cannot cach it into temp folder.");
				throw new CVException("Cannot set cache folder to temp path!");
			}
			Directory.CreateDirectory(cachingFolder);
			string cachingFile = cachingFolder + "\\AddtionalThumbPrints.xml";
			Logger.Log(Logger.LogSeverity.Information, "Fetch 3rd party file.");
			XmlDocument xmlDocument = AdditionalThumbFile.Fetch3rdPartyFile(cachingFile);
			if (xmlDocument == null)
			{
				Logger.Log(Logger.LogSeverity.Information, "Fetch 3rd party file fail, add offline thumbprint.");
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				if (executingAssembly != null)
				{
					string location = executingAssembly.Location;
				}
				using (MemoryStream memoryStream = new MemoryStream(Lenovo_CertificateValidation.trusted3rdparty))
				{
					xmlDocument = AdditionalThumbFile.GetXmlDocument(memoryStream);
				}
			}
			bool result;
			if (xmlDocument != null && xmlfileValidator.GetTrustStatus(xmlDocument) == TrustStatus.FileTrusted)
			{
				Logger.Log(Logger.LogSeverity.Information, "Get trusted xml document, add additional thumbprint.");
				List<string> thumbPrints;
				AdditionalThumbFile.GetThumbPrints(xmlDocument, out thumbPrints);
				CertificateTools.AddValidThumbPrints(thumbPrints);
				result = true;
			}
			else
			{
				Logger.Log(Logger.LogSeverity.Information, "GetAdditionalThumbPrints fail.");
				result = false;
			}
			return result;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002554 File Offset: 0x00000754
		private static XmlDocument GetXmlDocument(string file)
		{
			XmlDocument xmlDocument = null;
			if (File.Exists(file))
			{
				xmlDocument = new XmlDocument
				{
					PreserveWhitespace = true,
					XmlResolver = null
				};
				xmlDocument.Load(file);
			}
			return xmlDocument;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002588 File Offset: 0x00000788
		private static XmlDocument GetXmlDocument(Stream stream)
		{
			XmlDocument xmlDocument = null;
			if (stream != Stream.Null)
			{
				xmlDocument = new XmlDocument
				{
					PreserveWhitespace = true,
					XmlResolver = null
				};
				xmlDocument.Load(stream);
			}
			return xmlDocument;
		}

		// Token: 0x04000001 RID: 1
		private static readonly string TrustedAdditionalile = "https://filedownload.lenovo.com/enm/certificatevalidation/trusted3rdparty.xml";
	}
}
