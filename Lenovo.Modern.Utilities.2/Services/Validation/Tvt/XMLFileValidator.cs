using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Lenovo.Modern.Utilities.Services.Validation.Tvt
{
	// Token: 0x02000026 RID: 38
	public static class XMLFileValidator
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x00005030 File Offset: 0x00003230
		public static TrustStatus GetTrustStatus(XmlDocument xmlDoc)
		{
			TrustStatus result;
			if (xmlDoc != null)
			{
				bool flag;
				result = XMLFileValidator.VerifyXmlDocument(xmlDoc, out flag);
			}
			else
			{
				result = TrustStatus.InvalidXmlFile;
			}
			return result;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005054 File Offset: 0x00003254
		public static TrustStatus GetTrustStatus(string filename)
		{
			bool flag;
			return XMLFileValidator.GetTrustStatus(filename, out flag);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000506C File Offset: 0x0000326C
		public static TrustStatus GetTrustStatus(string filename, out bool oldCertificate)
		{
			TrustStatus result = TrustStatus.NotSignedWithTrustedCertificate;
			oldCertificate = false;
			try
			{
				if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.PreserveWhitespace = true;
					try
					{
						xmlDocument.Load(filename);
					}
					catch
					{
						result = TrustStatus.InvalidXmlFile;
						xmlDocument = null;
					}
					if (xmlDocument != null)
					{
						result = XMLFileValidator.VerifyXmlDocument(xmlDocument, out oldCertificate);
					}
				}
				else if (string.IsNullOrEmpty(filename))
				{
					result = TrustStatus.FileNameNullOrEmpty;
				}
				else
				{
					result = TrustStatus.FileNotFound;
				}
			}
			catch
			{
				result = TrustStatus.ExceptionThrown;
			}
			return result;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000050EC File Offset: 0x000032EC
		private static RSACryptoServiceProvider CreateRSAKeyFromString(string keyString)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider = null;
			try
			{
				if (!string.IsNullOrEmpty(keyString))
				{
					rsacryptoServiceProvider = new RSACryptoServiceProvider();
					rsacryptoServiceProvider.FromXmlString(keyString);
				}
			}
			catch
			{
				if (rsacryptoServiceProvider != null)
				{
					((IDisposable)rsacryptoServiceProvider).Dispose();
					rsacryptoServiceProvider = null;
				}
			}
			return rsacryptoServiceProvider;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005134 File Offset: 0x00003334
		private static TrustStatus VerifyXmlDocument(XmlDocument xmlDoc, out bool oldCertificate)
		{
			oldCertificate = false;
			SignedXml signedXml = new SignedXml(xmlDoc);
			XmlNodeList elementsByTagName = xmlDoc.GetElementsByTagName("Signature");
			int count = elementsByTagName.Count;
			TrustStatus trustStatus;
			if (count != 0)
			{
				if (count != 1)
				{
					trustStatus = TrustStatus.FileSignedMultipleTimes;
				}
				else
				{
					XmlNode xmlNode = elementsByTagName[0];
					if (xmlNode != null)
					{
						signedXml.LoadXml((XmlElement)xmlNode);
						trustStatus = XMLFileValidator.ValidateXmlUsingEmbeddedSignature(signedXml);
						if (trustStatus != TrustStatus.FileTrusted)
						{
							trustStatus = XMLFileValidator.ValidateXmlUsingThumbprintList(signedXml);
							oldCertificate = true;
						}
					}
					else
					{
						trustStatus = TrustStatus.FileNotSigned;
					}
				}
			}
			else
			{
				trustStatus = TrustStatus.FileNotSigned;
			}
			return trustStatus;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000051A4 File Offset: 0x000033A4
		private static TrustStatus ValidateXmlUsingEmbeddedSignature(SignedXml signedXml)
		{
			TrustStatus trustStatus = TrustStatus.FileNotTrusted;
			try
			{
				X509Certificate2 x509Certificate = null;
				foreach (object obj in signedXml.KeyInfo)
				{
					KeyInfoClause keyInfoClause = (KeyInfoClause)obj;
					if (keyInfoClause is KeyInfoX509Data && ((KeyInfoX509Data)keyInfoClause).Certificates.Count > 0)
					{
						x509Certificate = (X509Certificate2)((KeyInfoX509Data)keyInfoClause).Certificates[0];
					}
				}
				if (x509Certificate != null && signedXml.CheckSignature(x509Certificate, true))
				{
					trustStatus = CertificateTools.ValidateCertificateProperties(x509Certificate);
					if (trustStatus == TrustStatus.FileTrusted)
					{
						trustStatus = CertificateTools.ValidateIssuingCertificate(x509Certificate);
					}
				}
			}
			catch
			{
			}
			return trustStatus;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005260 File Offset: 0x00003460
		private static TrustStatus ValidateXmlUsingThumbprintList(SignedXml signedXml)
		{
			TrustStatus trustStatus = TrustStatus.FileNotTrusted;
			foreach (RSACryptoServiceProvider rsacryptoServiceProvider in XMLFileValidator.GetPublicKeys())
			{
				if (rsacryptoServiceProvider != null)
				{
					try
					{
						if (signedXml.CheckSignature(rsacryptoServiceProvider))
						{
							trustStatus = TrustStatus.FileTrusted;
						}
						goto IL_2E;
					}
					catch
					{
						trustStatus = TrustStatus.ExceptionThrown;
						goto IL_2E;
					}
					goto IL_2C;
				}
				goto IL_2C;
				IL_2E:
				if (trustStatus == TrustStatus.FileTrusted)
				{
					break;
				}
				continue;
				IL_2C:
				trustStatus = TrustStatus.InvalidPublicKey;
				goto IL_2E;
			}
			return trustStatus;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000052D8 File Offset: 0x000034D8
		private static List<RSACryptoServiceProvider> GetPublicKeys()
		{
			if (XMLFileValidator._publicKeys == null)
			{
				XMLFileValidator._publicKeys = new List<RSACryptoServiceProvider>();
				XMLFileValidator._publicKeys.Add(XMLFileValidator.CreateRSAKeyFromString("<RSAKeyValue><Modulus>wGCU86Mn04SEKGJeVB9S9IB15g6XlOeezVHoQB15fQ17doPHpDlyOHIG30ROS2z1+8KIqSqFrCD6ANghmTUrRHoMfH1OHw4I4hSJOi/NM+uBhpDjju3kb4zr5lQCiqJwMvVzt26B9Qpp7yOn+Z7gUhqhqvslOKMkakC2sZO+7uPL8b9YTXYQ4ET21ZuAoPeOz7+v7ACp6ptTSovtlszqWki8pkKuZ6WB0iUNNoUrV66Dr3NMswFBEGfJE2nKeNU5gvIrYAPKd/Qo9/sWprozN4p/ZZZvVZmRAm0WK8h6+HhCGC9gp9/w7ne1KTV8Tw240wyK2czd5SWPWk2uQXzGcQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"));
				XMLFileValidator._publicKeys.Add(XMLFileValidator.CreateRSAKeyFromString("<RSAKeyValue><Modulus>x1IWZtz+kEtrpe8PPNjoetTc6gJgTWpmCkc5tcKsFIbVgRCplQj0CSCMHUnok6HzM2Wy6Ome6fPySvESKDpL2jr9RCruFJehbngT0uIN5vMIWlhVwYYPN1OjRhr6pwfVPudHfSsTJ0j6L0hMD7NnUboSohvA4oBlbj1LTTZms5fEGlFqcqI8hxBE0X2cLedz4LtozAnyELDPmPPNM6BAfmlvPrCWB2YAZSFMKJKen1jPWDkKCED0E6O+7iN/kSazmp+NbpPw42JJ+Z+1CskBPD/tDZZsdk+lHSf8QJNK9N+ljWq2L1p0BpwOcf40AbRvz60mn7kchFjHwv5GRqVpeQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"));
				XMLFileValidator._publicKeys.Add(XMLFileValidator.CreateRSAKeyFromString("<RSAKeyValue><Modulus>3dVWFEYMA9umKTFggWHQscn+uHUqEjiCClRgoqG52Ko82aT5XLbHZ33IkdkA41hoFTbpv6YZ6Ol85OePD8itirzYof1PiW9eHAR1T7R+Z1lVcpQsZBXIEd7vR3CU4Nfu64wu0qtPUkm5/yrUdcw7e4+N4/yrpaaH/CPkk/iBuQZuo+1CCtJRmjXxAmI5TspOBXCBLtMIU43gzczgBr0oRZBFgom67cinb2u1nWl7iCjNZy7LZYkVkG+xXBFFeECoOzki3cctOCeb96LmX/GZgeDeV83S6Q0l3nQBcZgYRZB6XY2GzTRFSV3LyGaLUauvryAy5yASCGAxwIw2NN3TSQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"));
				XMLFileValidator._publicKeys.Add(XMLFileValidator.CreateRSAKeyFromString("<RSAKeyValue><Modulus>yRuxXgMnYq/33mpI0Jh/+8nTTUmQ5t21bh2Jut5bZj9+8h533O2NcWcahNnq3Ikl1qfD359MrdQ24TQa9outqrYHZquBPTxWw6CAo6HqdMJ5K3nT9uS9oscHF5FCHBzys7Ud4O4mt7z4/VFMLMxLk2TOlvZtObeQLgPhkeidJPizkLEdULt0ijp2o22rNH4UNpOf5WXavFwLyhSzsAQQ7PlpxWuIFfddqHTd7yuhjQlQZVG873q1EBksLo76e5tlx7fCew6K1dC3worFpk6e/J4GVfQiCcm1iv5ZGS0vJ5C2g2hZOKaI8P/Q8UjmPQ23ypZpngmPTzxZ3Vev7izg2Q==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"));
				XMLFileValidator._publicKeys.Add(XMLFileValidator.CreateRSAKeyFromString("<RSAKeyValue><Modulus>yzBsrsI3SEwirVwsBVr3bTzZHYmPSF1Q5e74HF75AP1VSJfri4ajKE4jWpRAf83IMgyCJBIVVSgCpspvU5SG0GaZGaHcAPn2h36R0PXTq9CElIX8JUKKpoMy9iY8zqFwsBnWaDFRC9ugBk9O+CqGhyGdxGhdbYyPu1SI5TvtSIp1WHYbzMqebclterwplnnpidXZ1r5+UZ66MlXrBHES0PFvwqkB6wjRf2jofFU8ow3fevhCxNh2hN+sS4MuNu6awj/sQS9DcUTILvM3yD3WOnuO9apssZ/NcurH0LaZSCaOK92xL1a+iwqSGc9vYM/cxyrzncrEqun+A6AwQsKzcQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"));
				XMLFileValidator._publicKeys.Add(XMLFileValidator.CreateRSAKeyFromString("<RSAKeyValue><Modulus>sUbfJXfRTZo4JScE3Jb0wql4pAI4DMZtdTnHPmd6ey80D3IlHhZpBQcnMu/QU2mjfVhAkW/mg6Ccud5gYA5nyf7dTUJ2otFcVUu08FRGGtgOv1Q/XVsJjJHGvGXX2QyrchtrygQimo2B/MRM863IZAro059fe5eBfTochD9tNu8SeFGHNY+2XizyIWVduKuRHQZKo3w6qmL3hi/tDn3xyg3xD+3QwY1w2Iv2wkwHL83LeekHOD44ubO2cpY6dT3IFgWRmhcDZ28jZVQknp1swOdOpjHknmTYmlitldm9msiRqhIYEY0dpSoQHM4xIQ7H5VdjdsUpgpV6Na33Zu9Hew==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"));
				XMLFileValidator._publicKeys.Add(XMLFileValidator.CreateRSAKeyFromString("<RSAKeyValue><Modulus>twVWMufFrSa68v9h+qMIEPTNZrZcxPzdiOoDfdTuX4A51yhSpbcySu9MbeXsu4Zg8f29nQMYnYsxSVfAUpaed3Sjjnff2k6/WHpSfoGzf5oDWrzY3Nss64VrE2/C3FZ0XVlp/xRAOO51MyRQ14Pz0KVOggw4LXgjQ8Tyy379Fi9CiDdBLmSFBt2rDoGVPfiiu0XPg6H6r/jw4U3sQ2iw1eyFWXa+tKqHm4uJKvWzWZWd5Wsls4iWx5RhHgT7+O3fLRc1FPf8oXl5QdVer/1UHGHN4wRbEJDDJWKhCUCbpzLxCkd1FIxMPQgojbiz9TkBBLi35zYzhHeRABHm1DX/BQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"));
				XMLFileValidator._publicKeys.Add(XMLFileValidator.CreateRSAKeyFromString("<RSAKeyValue><Modulus>vKn97XWN3F6SXADFjQvheHsuEq5Ri1zHWyrryhm9MkEXXKgm0oh/3MoJp0mB+AeZpjO1QEGFVtT2Cj1guZuZss3xp7Zo43ERG1f8QiMSczsSDaIpH7okbLN2unoFslm7NXHEjRyqqRH8+3Ffbwz4Ge1nH5QrJj1UyR4UoxK4x9gh5r7oh5XsYeWIxHKFPbTm4xcFEcm2MVKoo+Fbb85vTweecBGJcC775jPCIAMeBj0tUYkPnCSsyl/ZDqJWxnseHZdqDllj0dAuOXB4sdQGsJPpuC+siYfezm4pAyvysJO2dgY+L7DehPmI9w8Vyf9ITUoVvdBilpSyHRylpWeloQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"));
			}
			return XMLFileValidator._publicKeys;
		}

		// Token: 0x04000043 RID: 67
		private static List<RSACryptoServiceProvider> _publicKeys;
	}
}
