using System;
using System.Xml;

namespace Lenovo.Modern.Utilities.Services.Validation
{
	// Token: 0x0200001E RID: 30
	public interface ICertificateValidator
	{
		// Token: 0x0600009F RID: 159
		bool AssertDigitalSignatureIsValid(string filePath);

		// Token: 0x060000A0 RID: 160
		bool IsXmlValid(XmlDocument doc);

		// Token: 0x060000A1 RID: 161
		bool IsXmlValid(string filePath);
	}
}
