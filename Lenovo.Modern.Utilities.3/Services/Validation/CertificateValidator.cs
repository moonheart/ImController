using System;
using System.Xml;
using Lenovo.Modern.Utilities.Services.Validation.Tvt;

namespace Lenovo.Modern.Utilities.Services.Validation
{
	// Token: 0x0200001D RID: 29
	public class CertificateValidator : ICertificateValidator
	{
		// Token: 0x0600009C RID: 156 RVA: 0x000037BC File Offset: 0x000019BC
		public bool AssertDigitalSignatureIsValid(string filePath)
		{
			bool result = false;
			if (FileValidator.GetTrustStatus(filePath) == TrustStatus.FileTrusted)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000037D8 File Offset: 0x000019D8
		public bool IsXmlValid(XmlDocument doc)
		{
			bool result = false;
			if (XMLFileValidator.GetTrustStatus(doc) == TrustStatus.FileTrusted)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000037F4 File Offset: 0x000019F4
		public bool IsXmlValid(string filePath)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			xmlDocument.Load(filePath);
			return this.IsXmlValid(xmlDocument);
		}
	}
}
