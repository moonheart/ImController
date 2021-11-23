using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Lenovo.Tools.Logging;
using SBCertValidator;
using SBHTTPCertRetriever;
using SBHTTPCRL;
using SBHTTPOCSPClient;
using SBLDAPCRL;
using SBUtils;
using SBX509;
using SBXMLAdESIntf;
using SBXMLCore;
using SBXMLSec;
using SBXMLSig;

namespace Lenovo.CertificateValidation
{
	// Token: 0x02000011 RID: 17
	public sealed class XMLFileValidator
	{
		// Token: 0x06000065 RID: 101 RVA: 0x000049CC File Offset: 0x00002BCC
		public XMLFileValidator()
		{
			string text = "XMLFileValidator construcor";
			this.ValidationDate = DateTime.MinValue;
			Logger.Setup(Logger.LogSeverity.Information, "CVErrors");
			Logger.LogDirectory = Path.GetTempPath();
			Logger.IsLoggingEnabled = false;
			try
			{
				SBUtils.__Global.SetLicenseKey("BFEA7F45F4DEBCE4965277FFD1222984C3A9FEEE561F241920A0A99E6B22CC44C08FA6B33A78D97D5F8534409FB3860E4D6AA735278C8760B77F2BE9495CCEC07130357EA7B05E82AB23A5D9439688F9A50F95E2768477E9C0369FDEF3931495696F0C4230599A51E690A1CF9D4542AF00D40EA5F6D45478EBC6B2D2508C02D50318DCD9CBE65A40F30C6080A65B189A528CB7D6D70CBCF0C18F15782A9208FAC28ABBFB49829802EEC2232149C8C3CC7213F58D3A2364D6DB797C958286C88B6E1E37CFDFACA7744DB3E9A6443EEA4C5E91C6CA3B945E378428DFB46BD2363DB08CD6EC353D5B4AD99F8A633096C476950243AF64515FF4885C37482B72E098");
				SBHTTPCRL.__Global.RegisterHTTPCRLRetrieverFactory();
				SBLDAPCRL.__Global.RegisterLDAPCRLRetrieverFactory();
				SBHTTPOCSPClient.__Global.RegisterHTTPOCSPClientFactory();
				SBHTTPCertRetriever.__Global.RegisterHTTPCertificateRetrieverFactory();
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Error, "Exception thrown in {0}, Exception = {1}", new object[] { text, ex.Message });
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004A5C File Offset: 0x00002C5C
		public TrustStatus GetTrustStatus(string filename)
		{
			bool flag;
			return this.GetTrustStatus(filename, out flag);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004A74 File Offset: 0x00002C74
		public TrustStatus GetTrustStatus(XmlDocument xmlDoc)
		{
			if (xmlDoc == null)
			{
				return TrustStatus.InvalidXmlFile;
			}
			bool flag;
			return this.GetTrustStatus(xmlDoc, out flag);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004A90 File Offset: 0x00002C90
		public TrustStatus GetTrustStatus(XmlDocument xmlDoc, out bool oldCertificate)
		{
			string text = "GetTrustStatus";
			TrustStatus trustStatus = TrustStatus.UntrustedFile;
			oldCertificate = false;
			TElXMLDOMDocument telXMLDOMDocument = null;
			try
			{
				this.AppendInterCA(xmlDoc);
				telXMLDOMDocument = new TElXMLDOMDocument();
				if (xmlDoc != null)
				{
					MemoryStream memoryStream = new MemoryStream();
					xmlDoc.PreserveWhitespace = true;
					xmlDoc.Save(memoryStream);
					memoryStream.Flush();
					memoryStream.Position = 0L;
					try
					{
						telXMLDOMDocument.LoadFromStream(memoryStream);
					}
					catch
					{
						trustStatus = TrustStatus.InvalidXmlFile;
						telXMLDOMDocument = null;
					}
					if (telXMLDOMDocument != null)
					{
						X509Certificate2 x509Certificate;
						trustStatus = this.VerifyXmlDocument(telXMLDOMDocument, out x509Certificate);
						if (trustStatus == TrustStatus.FileTrusted || trustStatus == TrustStatus.SystemOffline || trustStatus == TrustStatus.ChainOfTrustFailed)
						{
							if (x509Certificate != null)
							{
								trustStatus = this.VerifyCertificateIsLenovos(x509Certificate, out oldCertificate);
							}
							else
							{
								trustStatus = this.VerifyCertificateIsLenovos(xmlDoc, out oldCertificate);
							}
						}
					}
				}
				else
				{
					trustStatus = TrustStatus.InvalidXmlFile;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Error, "Exception thrown in {0}, Exception = {1}", new object[] { text, ex.Message });
				trustStatus = TrustStatus.ExceptionThrown;
			}
			finally
			{
				if (telXMLDOMDocument != null)
				{
					telXMLDOMDocument.Dispose();
				}
			}
			return trustStatus;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004B80 File Offset: 0x00002D80
		public TrustStatus GetTrustStatus(string filename, out bool oldCertificate)
		{
			string text = "GetTrustStatus";
			Logger.Log(Logger.LogSeverity.Information, "Entered {0}", new object[] { text });
			TrustStatus trustStatus = TrustStatus.NotSignedWithTrustedCertificate;
			oldCertificate = false;
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
				{
					xmlDocument.PreserveWhitespace = true;
					xmlDocument.XmlResolver = null;
					try
					{
						xmlDocument.Load(filename);
						trustStatus = this.GetTrustStatus(xmlDocument, out oldCertificate);
					}
					catch
					{
						trustStatus = TrustStatus.InvalidXmlFile;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Error, "Exception thrown in {0}, Exception = {1}", new object[] { text, ex.Message });
				trustStatus = TrustStatus.ExceptionThrown;
			}
			Logger.Log(Logger.LogSeverity.Information, "Exited {0}, trusted = {1}", new object[] { text, trustStatus });
			return trustStatus;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004C4C File Offset: 0x00002E4C
		private void AppendInterCA(XmlDocument xmlDoc)
		{
			XmlElement xmlElement = ((xmlDoc != null) ? xmlDoc.DocumentElement : null);
			XmlNodeList xmlNodeList = ((xmlElement != null) ? xmlElement.GetElementsByTagName("issuerCertificate") : null);
			if (xmlNodeList != null && xmlNodeList.Count > 0)
			{
				foreach (object obj in xmlNodeList)
				{
					string innerText = ((XmlNode)obj).InnerText;
					string text = ((innerText != null) ? innerText.Trim() : null);
					if (!string.IsNullOrEmpty(text))
					{
						X509Certificate2 issuerCert = new X509Certificate2(Encoding.ASCII.GetBytes(text));
						try
						{
							if (!LenovoInterCA.ExtraCertificates.Value.Exists((X509Certificate2 x) => x.Thumbprint.Equals(issuerCert.Thumbprint)))
							{
								LenovoInterCA.ExtraCertificates.Value.Add(issuerCert);
							}
						}
						catch (Exception)
						{
						}
					}
				}
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00004D44 File Offset: 0x00002F44
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00004D4C File Offset: 0x00002F4C
		public DateTime ValidationDate { get; set; }

		// Token: 0x0600006D RID: 109 RVA: 0x00004D58 File Offset: 0x00002F58
		private TrustStatus VerifyCertificateIsLenovos(X509Certificate2 signerCertificate, out bool oldCertificate)
		{
			string text = "VerifyCertificateIsLenovos:SBB Signed Verion";
			Logger.Log(Logger.LogSeverity.Information, "Entered {0}", new object[] { text });
			oldCertificate = false;
			TrustStatus trustStatus = this.ValidateXmlUsingEmbeddedSignature(signerCertificate);
			if (trustStatus != TrustStatus.FileTrusted)
			{
				trustStatus = this.ValidateXmlUsingThumbprintList(signerCertificate);
				oldCertificate = true;
			}
			if (trustStatus == TrustStatus.FileTrusted && CertificateTools.IsDisAllowedThumbprint(signerCertificate))
			{
				trustStatus = TrustStatus.FileNotTrusted;
			}
			Logger.Log(Logger.LogSeverity.Information, "Exited {0}, trusted = {1}, used ThumbprintList = {2}", new object[] { text, trustStatus, oldCertificate });
			return trustStatus;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004DD0 File Offset: 0x00002FD0
		private TrustStatus VerifyCertificateIsLenovos(XmlDocument xmlDoc, out bool oldCertificate)
		{
			string text = "VerifyCertificateIsLenovos:Old Signed Verion";
			Logger.Log(Logger.LogSeverity.Information, "Entered {0}", new object[] { text });
			oldCertificate = false;
			if (xmlDoc == null)
			{
				return TrustStatus.InvalidXmlFile;
			}
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
						trustStatus = this.ValidateXmlUsingEmbeddedSignature(signedXml);
						if (trustStatus != TrustStatus.FileTrusted && trustStatus != TrustStatus.FileTamperedWith)
						{
							trustStatus = this.ValidateXmlUsingThumbprintList(signedXml);
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
			Logger.Log(Logger.LogSeverity.Information, "Exited {0}, trusted = {1}, used ThumbprintList = {2}", new object[] { text, trustStatus, oldCertificate });
			return trustStatus;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004E90 File Offset: 0x00003090
		private RSACryptoServiceProvider CreateRSAKeyFromString(string keyString)
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
					rsacryptoServiceProvider.Dispose();
					rsacryptoServiceProvider = null;
				}
			}
			return rsacryptoServiceProvider;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004ED8 File Offset: 0x000030D8
		private TrustStatus VerifyXmlDocument(TElXMLDOMDocument xmlDoc, out X509Certificate2 signerCertificate)
		{
			string text = "VerifyXmlDocument";
			Logger.Log(Logger.LogSeverity.Information, "Entered {0}", new object[] { text });
			TrustStatus trustStatus = TrustStatus.FileNotTrusted;
			signerCertificate = null;
			try
			{
				if (xmlDoc != null)
				{
					TElXMLDOMElement telXMLDOMElement = null;
					this.RetrieveXmlData(xmlDoc);
					try
					{
						telXMLDOMElement = this.GetXMLSignatureNode(xmlDoc);
					}
					catch
					{
						telXMLDOMElement = null;
						trustStatus = TrustStatus.FileNotSigned;
					}
					if (telXMLDOMElement == null)
					{
						trustStatus = TrustStatus.FileNotSigned;
						goto IL_B9;
					}
					TElXMLVerifier telXMLVerifier = new TElXMLVerifier();
					TElXAdESVerifier telXAdESVerifier = new TElXAdESVerifier();
					try
					{
						telXAdESVerifier.OnBeforeCertificateValidate += this.HandleBeforeCertificateValidate;
						telXMLVerifier.XAdESProcessor = telXAdESVerifier;
						try
						{
							telXMLVerifier.Load(telXMLDOMElement);
						}
						catch
						{
						}
						if (telXAdESVerifier.IsEnabled)
						{
							trustStatus = this.ValidateSignature(telXMLVerifier, telXAdESVerifier, out signerCertificate);
						}
						else
						{
							trustStatus = TrustStatus.FileTrusted;
						}
						goto IL_B9;
					}
					finally
					{
						if (telXMLVerifier.KeyData != null)
						{
							telXMLVerifier.KeyData.Dispose();
						}
						telXMLVerifier.Dispose();
						telXAdESVerifier.Dispose();
					}
				}
				trustStatus = TrustStatus.InvalidXmlFile;
				IL_B9:;
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Error, "Exception thrown in {0}, Exception = {1}", new object[] { text, ex.Message });
				trustStatus = TrustStatus.ExceptionThrown;
			}
			Logger.Log(Logger.LogSeverity.Information, "Exited {0}, trusted = {1}", new object[] { text, trustStatus });
			return trustStatus;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005018 File Offset: 0x00003218
		private TElXMLDOMNode RetrieveXmlData(TElXMLDOMDocument xmlDoc)
		{
			TElXMLDOMNode telXMLDOMNode = null;
			if (xmlDoc != null)
			{
				telXMLDOMNode = xmlDoc.FirstChild;
				while (telXMLDOMNode != null)
				{
					telXMLDOMNode = telXMLDOMNode.NextSibling;
					if (telXMLDOMNode is TElXMLDOMElement)
					{
						break;
					}
				}
			}
			return telXMLDOMNode;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005048 File Offset: 0x00003248
		private TrustStatus ValidateSignature(TElXMLVerifier verifier, TElXAdESVerifier xAdESVerifier, out X509Certificate2 signerCertificate)
		{
			string text = "ValidateSignature";
			Logger.Log(Logger.LogSeverity.Information, "Entered {0}", new object[] { text });
			TrustStatus trustStatus = TrustStatus.FileNotTrusted;
			DateTime validationMoment = DateTime.UtcNow;
			int num = 0;
			TElX509CertificateValidator telX509CertificateValidator = new TElX509CertificateValidator();
			TElXMLReference telXMLReference = null;
			signerCertificate = null;
			try
			{
				if (this.ValidationDate != DateTime.MinValue)
				{
					validationMoment = this.ValidationDate.ToUniversalTime();
				}
				telX509CertificateValidator.InitializeWinStorages();
				telXMLReference = new TElXMLReference();
				if (verifier.ValidateSignature())
				{
					signerCertificate = verifier.SignerCertificate.ToX509Certificate2(false);
					telXMLReference.URINodes = new TElXMLNodeSet(false);
					this.SetSignatureValidationOptions(ref telX509CertificateValidator);
					if (verifier.ValidateReferences())
					{
						if (xAdESVerifier != null && xAdESVerifier.IsEnabled)
						{
							xAdESVerifier.CertificateValidator = telX509CertificateValidator;
							xAdESVerifier.ValidationMoment = validationMoment;
							TSBXAdESValidity tsbxadESValidity = xAdESVerifier.Validate(ref num);
							if (tsbxadESValidity == TSBXAdESValidity.xsvValid)
							{
								trustStatus = TrustStatus.FileTrusted;
							}
							else if (tsbxadESValidity == TSBXAdESValidity.xsvIncomplete && num == 16)
							{
								if (verifier.SignerCertificate != null)
								{
									trustStatus = this.WasFileCountersignedProperly(verifier.SignerCertificate, xAdESVerifier.CertifiedSigningTime);
								}
								else
								{
									trustStatus = TrustStatus.FileNotSigned;
								}
							}
							else if (tsbxadESValidity == TSBXAdESValidity.xsvInvalid && num == 32)
							{
								trustStatus = TrustStatus.ChainOfTrustFailed;
							}
							else if (tsbxadESValidity == TSBXAdESValidity.xsvIncomplete && (num == 2097168 || num == 2097184 || num == 2097152))
							{
								trustStatus = TrustStatus.SystemOffline;
							}
							else if (tsbxadESValidity == TSBXAdESValidity.xsvInvalid && (num == 2097168 || num == 2097184 || num == 2097152))
							{
								trustStatus = TrustStatus.SystemOffline;
							}
							else
							{
								Logger.Log(Logger.LogSeverity.Information, "XAdESValidity = {0}, XAdESReasons = {1}", new object[]
								{
									tsbxadESValidity.ToString(),
									num.ToString()
								});
								trustStatus = TrustStatus.XAdesFailed;
							}
						}
					}
					else
					{
						trustStatus = TrustStatus.FileTamperedWith;
					}
				}
				else
				{
					trustStatus = TrustStatus.SignatureCheckFailed;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Error, "Exception thrown in {0}, Exception = {1}", new object[] { text, ex.Message });
			}
			finally
			{
				if (telX509CertificateValidator != null)
				{
					telX509CertificateValidator.Dispose();
				}
				if (telXMLReference != null)
				{
					telXMLReference.Dispose();
				}
			}
			Logger.Log(Logger.LogSeverity.Information, "Exited {0}, trusted = {1}", new object[] { text, trustStatus });
			return trustStatus;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00005274 File Offset: 0x00003474
		private TrustStatus WasFileCountersignedProperly(TElX509Certificate signerCertificate, DateTime certifiedSigningTime)
		{
			TrustStatus trustStatus = TrustStatus.NotCountersignedProperly;
			string text = "WasFileCountersignedProperly";
			Logger.Log(Logger.LogSeverity.Information, "Entered {0}", new object[] { text });
			try
			{
				if (signerCertificate != null && signerCertificate.ValidFrom <= certifiedSigningTime && certifiedSigningTime <= signerCertificate.ValidTo)
				{
					trustStatus = TrustStatus.FileTrusted;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Error, "Exception thrown in {0}, Exception = {1}", new object[] { text, ex.Message });
			}
			Logger.Log(Logger.LogSeverity.Information, "Exited {0}, trusted = {1}", new object[] { text, trustStatus });
			return trustStatus;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00005314 File Offset: 0x00003514
		private void HandleBeforeCertificateValidate(object Sender, TElX509Certificate Cert, TElX509CertificateValidator CertValidator)
		{
			CertValidator.IgnoreSystemTrust = false;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005320 File Offset: 0x00003520
		private void SetSignatureValidationOptions(ref TElX509CertificateValidator certValidator)
		{
			certValidator.ClearTrustedCertificates();
			certValidator.ClearKnownCertificates();
			certValidator.ClearKnownCRLs();
			certValidator.ClearKnownOCSPResponses();
			certValidator.OfflineMode = false;
			certValidator.MandatoryCRLCheck = true;
			certValidator.MandatoryOCSPCheck = true;
			certValidator.MandatoryRevocationCheck = true;
			certValidator.CheckValidityPeriodForTrusted = true;
			certValidator.IgnoreCAKeyUsage = false;
			certValidator.ValidateInvalidCertificates = false;
			certValidator.ForceCompleteChainValidationForTrusted = true;
			certValidator.ImplicitlyTrustSelfSignedCertificates = false;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00005394 File Offset: 0x00003594
		private TElXMLDOMElement GetXMLSignatureNode(TElXMLDOMDocument xmlDoc)
		{
			TElXMLDOMElement result = null;
			TElXMLNodeSet telXMLNodeSet = null;
			TElXMLNamespaceMap telXMLNamespaceMap = null;
			try
			{
				if (xmlDoc != null && xmlDoc.DocumentElement != null)
				{
					telXMLNamespaceMap = new TElXMLNamespaceMap();
					telXMLNamespaceMap.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
					telXMLNodeSet = xmlDoc.SelectNodes("//ds:Signature", telXMLNamespaceMap);
					if (0 < telXMLNodeSet.Count)
					{
						result = telXMLNodeSet.get_Node(telXMLNodeSet.Count - 1) as TElXMLDOMElement;
					}
				}
			}
			finally
			{
				if (telXMLNodeSet != null)
				{
					telXMLNodeSet.Dispose();
				}
				if (telXMLNamespaceMap != null)
				{
					telXMLNamespaceMap.Dispose();
				}
			}
			return result;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005418 File Offset: 0x00003618
		private TrustStatus ValidateXmlUsingEmbeddedSignature(X509Certificate2 signerCertificate)
		{
			TrustStatus trustStatus = TrustStatus.FileNotTrusted;
			string text = "ValidateXmlUsingEmbeddedSignature";
			Logger.Log(Logger.LogSeverity.Information, "Entered {0}", new object[] { text });
			try
			{
				if (signerCertificate != null)
				{
					Logger.Log(Logger.LogSeverity.Information, "Got a certificate, now checking the signature properties");
					trustStatus = CertificateTools.ValidateCertificateProperties(signerCertificate);
					if (trustStatus == TrustStatus.FileTrusted)
					{
						Logger.Log(Logger.LogSeverity.Information, "Properties check out, now checking the issuer and root thumbprint");
						trustStatus = CertificateTools.ValidateIssuerAndRootCertificate(signerCertificate);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Error, "Exception thrown in {0}, Exception = {1}", new object[] { text, ex.Message });
			}
			Logger.Log(Logger.LogSeverity.Information, "Exited {0}, trusted = {1}", new object[] { text, trustStatus });
			return trustStatus;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000054C0 File Offset: 0x000036C0
		private TrustStatus ValidateXmlUsingEmbeddedSignature(SignedXml signedXml, out X509Certificate2 signerCertificate)
		{
			TrustStatus trustStatus = TrustStatus.FileNotTrusted;
			signerCertificate = null;
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
				if (x509Certificate != null)
				{
					signerCertificate = x509Certificate;
					if (signedXml.CheckSignature(x509Certificate, true))
					{
						trustStatus = CertificateTools.ValidateCertificateProperties(x509Certificate);
						if (trustStatus == TrustStatus.FileTrusted)
						{
							trustStatus = CertificateTools.ValidateIssuerAndRootCertificate(x509Certificate);
						}
						if (trustStatus == TrustStatus.FileTrusted && CertificateTools.IsDisAllowedThumbprint(x509Certificate))
						{
							trustStatus = TrustStatus.FileNotTrusted;
						}
					}
					else
					{
						trustStatus = TrustStatus.FileTamperedWith;
					}
				}
			}
			catch
			{
			}
			return trustStatus;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005594 File Offset: 0x00003794
		private TrustStatus ValidateXmlUsingEmbeddedSignature(SignedXml signedXml)
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
				if (x509Certificate != null)
				{
					if (signedXml.CheckSignature(x509Certificate, true))
					{
						trustStatus = CertificateTools.ValidateCertificateProperties(x509Certificate);
						if (trustStatus == TrustStatus.FileTrusted)
						{
							trustStatus = CertificateTools.ValidateIssuerAndRootCertificate(x509Certificate);
						}
						if (trustStatus == TrustStatus.FileTrusted && CertificateTools.IsDisAllowedThumbprint(x509Certificate))
						{
							trustStatus = TrustStatus.FileNotTrusted;
						}
					}
					else
					{
						trustStatus = TrustStatus.FileTamperedWith;
					}
				}
			}
			catch
			{
			}
			return trustStatus;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00005664 File Offset: 0x00003864
		private TrustStatus ValidateXmlUsingThumbprintList(SignedXml signedXml)
		{
			TrustStatus trustStatus = TrustStatus.NotLenovoCertificate;
			foreach (RSACryptoServiceProvider rsacryptoServiceProvider in this.GetPublicKeys())
			{
				if (rsacryptoServiceProvider != null)
				{
					try
					{
						if (signedXml.CheckSignature(rsacryptoServiceProvider))
						{
							trustStatus = TrustStatus.FileTrusted;
						}
						goto IL_30;
					}
					catch
					{
						trustStatus = TrustStatus.ExceptionThrown;
						goto IL_30;
					}
					goto IL_2E;
				}
				goto IL_2E;
				IL_30:
				if (trustStatus == TrustStatus.FileTrusted)
				{
					break;
				}
				continue;
				IL_2E:
				trustStatus = TrustStatus.InvalidPublicKey;
				goto IL_30;
			}
			return trustStatus;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000056DC File Offset: 0x000038DC
		private TrustStatus ValidateXmlUsingThumbprintList(X509Certificate2 signerCertificate)
		{
			string text = "ValidateXmlUsingThumbprintList";
			Logger.Log(Logger.LogSeverity.Information, "Entered {0}", new object[] { text });
			TrustStatus trustStatus = TrustStatus.NotLenovoCertificate;
			if (signerCertificate != null)
			{
				trustStatus = CertificateTools.ValidateCertificateUsingThumbprintList(signerCertificate);
			}
			Logger.Log(Logger.LogSeverity.Information, "Exited {0}, trusted = {1}", new object[] { text, trustStatus });
			return trustStatus;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00005730 File Offset: 0x00003930
		private List<RSACryptoServiceProvider> GetPublicKeys()
		{
			if (this._publicKeys == null)
			{
				this._publicKeys = new List<RSACryptoServiceProvider>();
				this._publicKeys.Add(this.CreateRSAKeyFromString("<RSAKeyValue><Modulus>twVWMufFrSa68v9h+qMIEPTNZrZcxPzdiOoDfdTuX4A51yhSpbcySu9MbeXsu4Zg8f29nQMYnYsxSVfAUpaed3Sjjnff2k6/WHpSfoGzf5oDWrzY3Nss64VrE2/C3FZ0XVlp/xRAOO51MyRQ14Pz0KVOggw4LXgjQ8Tyy379Fi9CiDdBLmSFBt2rDoGVPfiiu0XPg6H6r/jw4U3sQ2iw1eyFWXa+tKqHm4uJKvWzWZWd5Wsls4iWx5RhHgT7+O3fLRc1FPf8oXl5QdVer/1UHGHN4wRbEJDDJWKhCUCbpzLxCkd1FIxMPQgojbiz9TkBBLi35zYzhHeRABHm1DX/BQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"));
				this._publicKeys.Add(this.CreateRSAKeyFromString("<RSAKeyValue><Modulus>vKn97XWN3F6SXADFjQvheHsuEq5Ri1zHWyrryhm9MkEXXKgm0oh/3MoJp0mB+AeZpjO1QEGFVtT2Cj1guZuZss3xp7Zo43ERG1f8QiMSczsSDaIpH7okbLN2unoFslm7NXHEjRyqqRH8+3Ffbwz4Ge1nH5QrJj1UyR4UoxK4x9gh5r7oh5XsYeWIxHKFPbTm4xcFEcm2MVKoo+Fbb85vTweecBGJcC775jPCIAMeBj0tUYkPnCSsyl/ZDqJWxnseHZdqDllj0dAuOXB4sdQGsJPpuC+siYfezm4pAyvysJO2dgY+L7DehPmI9w8Vyf9ITUoVvdBilpSyHRylpWeloQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"));
				this._publicKeys.Add(this.CreateRSAKeyFromString("<RSAKeyValue><Modulus>vmGNeKZAzsxMT78A7pT1DJqrr2KXgNnu05nXt81j1nmALHEO0BDwOS5ShZJFu07vwKI6U1LOacQQxtRxLGvm73+wiQKSUWqPwLStg2mZAeql0IkH96Pi6o3vNDdb8wuovWdDREwNoi6jAGcxE8tnBKsrfZxt+poaKOJiTEAdCJ2HTDBKO2+o22aT744E8eZlggiQzBElxfQZXbDnbbHGROS3mHMurdg2KNWYzrjxK83D5V97ROrszJ9y2lxNeZeHcCjFxJykH+N16rjmmYuyosnOqT7/RkjnPxNbIgcbj4/kutvScGhSD980gzFhiy+bZtHk4jFmu+AZPsJZVw8pAw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"));
				this._publicKeys.Add(this.CreateRSAKeyFromString("<RSAKeyValue><Modulus>yzBsrsI3SEwirVwsBVr3bTzZHYmPSF1Q5e74HF75AP1VSJfri4ajKE4jWpRAf83IMgyCJBIVVSgCpspvU5SG0GaZGaHcAPn2h36R0PXTq9CElIX8JUKKpoMy9iY8zqFwsBnWaDFRC9ugBk9O+CqGhyGdxGhdbYyPu1SI5TvtSIp1WHYbzMqebclterwplnnpidXZ1r5+UZ66MlXrBHES0PFvwqkB6wjRf2jofFU8ow3fevhCxNh2hN+sS4MuNu6awj/sQS9DcUTILvM3yD3WOnuO9apssZ/NcurH0LaZSCaOK92xL1a+iwqSGc9vYM/cxyrzncrEqun+A6AwQsKzcQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"));
			}
			return this._publicKeys;
		}

		// Token: 0x0400005F RID: 95
		private List<RSACryptoServiceProvider> _publicKeys;
	}
}
