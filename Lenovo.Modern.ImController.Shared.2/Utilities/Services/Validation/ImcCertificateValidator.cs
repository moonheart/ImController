using System;
using System.Xml;
using Lenovo.CertificateValidation;

namespace Lenovo.Modern.Utilities.Services.Validation
{
	// Token: 0x02000003 RID: 3
	public class ImcCertificateValidator : IImcCertificateValidator
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002058 File Offset: 0x00000258
		public bool AssertDigitalSignatureIsValid(string filePath)
		{
			return FileValidator.GetTrustStatus(filePath) == TrustStatus.FileTrusted;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002063 File Offset: 0x00000263
		public bool IsXmlValid(XmlDocument doc)
		{
			return new XMLFileValidator().GetTrustStatus(doc) == TrustStatus.FileTrusted;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002073 File Offset: 0x00000273
		public bool IsXmlValid(string filePath)
		{
			return new XMLFileValidator().GetTrustStatus(filePath) == TrustStatus.FileTrusted;
		}
	}
}
