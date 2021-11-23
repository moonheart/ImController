using System;
using System.Xml;

namespace Lenovo.Modern.Utilities.Services.Validation
{
	// Token: 0x02000002 RID: 2
	public interface IImcCertificateValidator
	{
		// Token: 0x06000001 RID: 1
		bool AssertDigitalSignatureIsValid(string filePath);

		// Token: 0x06000002 RID: 2
		bool IsXmlValid(XmlDocument doc);

		// Token: 0x06000003 RID: 3
		bool IsXmlValid(string filePath);
	}
}
