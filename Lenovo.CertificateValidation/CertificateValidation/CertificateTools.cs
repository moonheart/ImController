using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Lenovo.CertificateValidation
{
	// Token: 0x02000006 RID: 6
	internal class CertificateTools
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002694 File Offset: 0x00000894
		internal static List<X509Certificate2> GetSigningCertificates(string fileName)
		{
			List<X509Certificate2> list = new List<X509Certificate2>();
			try
			{
				Guid guid = new Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}");
				using (WinVerifyTrustTools.WINTRUST_FILE_INFO fileInfo = new WinVerifyTrustTools.WINTRUST_FILE_INFO(fileName, Guid.Empty))
				{
					using (WinVerifyTrustTools.UnmanagedPointer unmanagedPointer = new WinVerifyTrustTools.UnmanagedPointer(Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Guid))), WinVerifyTrustTools.AllocMethod.HGlobal))
					{
						using (WinVerifyTrustTools.UnmanagedPointer unmanagedPointer2 = new WinVerifyTrustTools.UnmanagedPointer(Marshal.AllocHGlobal(Marshal.SizeOf(typeof(WinVerifyTrustTools.WINTRUST_DATA))), WinVerifyTrustTools.AllocMethod.HGlobal))
						{
							using (WinVerifyTrustTools.UnmanagedPointer unmanagedPointer3 = new WinVerifyTrustTools.UnmanagedPointer(Marshal.AllocHGlobal(Marshal.SizeOf(typeof(WinVerifyTrustTools.WINTRUST_SIGNATURE_SETTINGS))), WinVerifyTrustTools.AllocMethod.HGlobal))
							{
								WinVerifyTrustTools.WINTRUST_SIGNATURE_SETTINGS wintrust_SIGNATURE_SETTINGS = new WinVerifyTrustTools.WINTRUST_SIGNATURE_SETTINGS
								{
									cbStruct = (uint)Marshal.SizeOf(typeof(WinVerifyTrustTools.WINTRUST_SIGNATURE_SETTINGS)),
									dwIndex = 0U,
									dwFlags = 2U,
									cSecondarySigs = 0U,
									dwVerifiedSigIndex = 0U,
									pCryptoPolicy = IntPtr.Zero
								};
								if (unmanagedPointer3 != IntPtr.Zero)
								{
									Marshal.StructureToPtr(wintrust_SIGNATURE_SETTINGS, unmanagedPointer3, false);
								}
								WinVerifyTrustTools.WINTRUST_DATA wintrust_DATA = new WinVerifyTrustTools.WINTRUST_DATA(fileInfo, unmanagedPointer3);
								IntPtr intPtr = unmanagedPointer;
								IntPtr intPtr2 = unmanagedPointer2;
								Marshal.StructureToPtr(guid, intPtr, true);
								Marshal.StructureToPtr(wintrust_DATA, intPtr2, true);
								uint num = WinVerifyTrustTools.WinVerifyTrust(IntPtr.Zero, intPtr, intPtr2);
								if (num == 0U || 2148204810U == num)
								{
									if (unmanagedPointer3 != IntPtr.Zero)
									{
										wintrust_SIGNATURE_SETTINGS = (WinVerifyTrustTools.WINTRUST_SIGNATURE_SETTINGS)Marshal.PtrToStructure(unmanagedPointer3, typeof(WinVerifyTrustTools.WINTRUST_SIGNATURE_SETTINGS));
									}
									uint num2 = wintrust_SIGNATURE_SETTINGS.cSecondarySigs + 1U;
									for (uint num3 = 0U; num3 < num2; num3 += 1U)
									{
										try
										{
											if (unmanagedPointer3 != IntPtr.Zero)
											{
												wintrust_SIGNATURE_SETTINGS.dwIndex = num3;
												wintrust_SIGNATURE_SETTINGS.dwFlags = 1U;
												Marshal.StructureToPtr(wintrust_SIGNATURE_SETTINGS, unmanagedPointer3, false);
											}
											wintrust_DATA.dwStateAction = WinVerifyTrustTools.StateAction.Verify;
											wintrust_DATA.hWVTStateData = IntPtr.Zero;
											Marshal.StructureToPtr(wintrust_DATA, intPtr2, true);
											num = WinVerifyTrustTools.WinVerifyTrust(IntPtr.Zero, intPtr, intPtr2);
											wintrust_DATA = (WinVerifyTrustTools.WINTRUST_DATA)Marshal.PtrToStructure(intPtr2, typeof(WinVerifyTrustTools.WINTRUST_DATA));
											IntPtr intPtr3 = WinVerifyTrustTools.WTHelperProvDataFromStateData(wintrust_DATA.hWVTStateData);
											WinVerifyTrustTools.CRYPT_PROVIDER_DATA crypt_PROVIDER_DATA = (WinVerifyTrustTools.CRYPT_PROVIDER_DATA)Marshal.PtrToStructure(intPtr3, typeof(WinVerifyTrustTools.CRYPT_PROVIDER_DATA));
											for (uint num4 = 0U; num4 < crypt_PROVIDER_DATA.csSigners; num4 += 1U)
											{
												IntPtr intPtr4 = WinVerifyTrustTools.WTHelperGetProvSignerFromChain(intPtr3, num4, false, 0U);
												WinVerifyTrustTools.CRYPT_PROVIDER_SGNR crypt_PROVIDER_SGNR = (WinVerifyTrustTools.CRYPT_PROVIDER_SGNR)Marshal.PtrToStructure(intPtr4, typeof(WinVerifyTrustTools.CRYPT_PROVIDER_SGNR));
												WinVerifyTrustTools.CRYPT_PROVIDER_CERT crypt_PROVIDER_CERT = (WinVerifyTrustTools.CRYPT_PROVIDER_CERT)Marshal.PtrToStructure(WinVerifyTrustTools.WTHelperGetProvCertFromChain(intPtr4, num4), typeof(WinVerifyTrustTools.CRYPT_PROVIDER_CERT));
												if (crypt_PROVIDER_CERT.cbStruct > 0U)
												{
													X509Certificate2 item = new X509Certificate2(crypt_PROVIDER_CERT.pCert);
													list.Add(item);
												}
												if (crypt_PROVIDER_SGNR.csCertChain > 1U)
												{
													WinVerifyTrustTools.CRYPT_PROVIDER_CERT crypt_PROVIDER_CERT2 = (WinVerifyTrustTools.CRYPT_PROVIDER_CERT)Marshal.PtrToStructure(WinVerifyTrustTools.WTHelperGetProvCertFromChain(intPtr4, 1U), typeof(WinVerifyTrustTools.CRYPT_PROVIDER_CERT));
													if (crypt_PROVIDER_CERT2.cbStruct > 0U)
													{
														try
														{
															X509Certificate2 issuerCertificate = new X509Certificate2(crypt_PROVIDER_CERT2.pCert);
															if (!LenovoInterCA.ExtraCertificates.Value.Exists((X509Certificate2 x) => x.Thumbprint.Equals(issuerCertificate.Thumbprint)))
															{
																LenovoInterCA.ExtraCertificates.Value.Add(issuerCertificate);
															}
														}
														catch (Exception)
														{
														}
													}
												}
											}
										}
										catch (Exception)
										{
										}
										finally
										{
											wintrust_DATA.dwStateAction = WinVerifyTrustTools.StateAction.Close;
											Marshal.StructureToPtr(wintrust_DATA, intPtr2, true);
											WinVerifyTrustTools.WinVerifyTrust(IntPtr.Zero, intPtr, intPtr2);
										}
									}
								}
							}
						}
					}
				}
				if (list.Count < 1)
				{
					DebugInfo.Output("No Certs found using WinVerifyTrustMethod, attempting to use .Net method", true);
					X509Certificate x509Certificate = X509Certificate.CreateFromSignedFile(fileName);
					if (x509Certificate != null)
					{
						X509Certificate2 item2 = new X509Certificate2(x509Certificate);
						list.Add(item2);
					}
				}
			}
			catch
			{
			}
			return list;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002B4C File Offset: 0x00000D4C
		internal static void AddValidThumbPrints(List<string> thumbPrints)
		{
			if (thumbPrints.Count > 0)
			{
				foreach (string item in thumbPrints)
				{
					CertificateTools.ValidThumbprints.Add(item);
				}
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002BA8 File Offset: 0x00000DA8
		internal static TrustStatus ValidateCertificateProperties(X509Certificate2 lastCert)
		{
			DebugInfo.Output("Entered ValidateCertificateProperties", true);
			TrustStatus trustStatus = CertificateTools.ValidateCertificateSubject(lastCert);
			if (trustStatus == TrustStatus.FileTrusted)
			{
				trustStatus = CertificateTools.ValidateCertificateDate(lastCert);
			}
			DebugInfo.Output("Exited ValidateCertificateProperties. Trusted = " + trustStatus, true);
			return trustStatus;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002BE8 File Offset: 0x00000DE8
		internal static TrustStatus ValidateIssuerAndRootCertificate(X509Certificate2 fileCert2)
		{
			TrustStatus trustStatus = TrustStatus.IssuingCertificateNotValid;
			DebugInfo.Output("Entered ValidateIssuerAndRootCertificate", true);
			try
			{
				X509Chain x509Chain = new X509Chain();
				x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.Offline;
				x509Chain.ChainPolicy.ExtraStore.AddRange(LenovoInterCA.IssuerCerts);
				LenovoInterCA.ExtraCertificates.Value.RemoveAll((X509Certificate2 it) => it == null);
				x509Chain.ChainPolicy.ExtraStore.AddRange(LenovoInterCA.ExtraCertificates.Value.ToArray());
				x509Chain.Build(fileCert2);
				if (x509Chain.ChainElements != null && x509Chain.ChainElements.Count > 1)
				{
					DebugInfo.Output(string.Format("Got {0} certificates in the chain", x509Chain.ChainElements.Count), true);
					X509ChainElement element = x509Chain.ChainElements[1];
					if (element != null && element.Certificate != null)
					{
						if (!string.IsNullOrEmpty(element.Certificate.Thumbprint))
						{
							DebugInfo.Output("Looking at issuer thumbprint: " + element.Certificate.Thumbprint, true);
							if (CertificateTools.ValidIssuingThumbprints.FirstOrDefault((string thumbprint) => thumbprint.Equals(element.Certificate.Thumbprint, StringComparison.InvariantCultureIgnoreCase)) != null)
							{
								trustStatus = TrustStatus.FileTrusted;
							}
						}
						else
						{
							DebugInfo.Output("Fatal Error: element.Certificate.Thumbprint(issuer thumbprint) is null or empty!", true);
						}
					}
					if (trustStatus != TrustStatus.FileTrusted)
					{
						X509ChainElement rootElement = x509Chain.ChainElements[x509Chain.ChainElements.Count - 1];
						if (rootElement != null && rootElement.Certificate != null)
						{
							if (!string.IsNullOrEmpty(rootElement.Certificate.Thumbprint))
							{
								DebugInfo.Output("Looking at root thumbprint: " + rootElement.Certificate.Thumbprint, true);
								if (CertificateTools.ValidRootThumbPrints.FirstOrDefault((string thumbprint) => thumbprint.Equals(rootElement.Certificate.Thumbprint, StringComparison.InvariantCultureIgnoreCase)) != null)
								{
									trustStatus = TrustStatus.FileTrusted;
								}
							}
							else
							{
								DebugInfo.Output("Fatal Error: element.Certificate.Thumbprint(root thumbprint) is null or empty!", true);
							}
						}
					}
				}
				else
				{
					trustStatus = TrustStatus.NotLenovoCertificate;
					if (x509Chain.ChainElements == null)
					{
						DebugInfo.Output("Error: certChain.ChainElements is null", true);
					}
					else
					{
						DebugInfo.Output(string.Format("Eroror: Only Got {0} certificates in the chain", x509Chain.ChainElements.Count), true);
					}
				}
			}
			catch (Exception ex)
			{
				DebugInfo.Output("Exception thrown: " + ex.Message, true);
			}
			DebugInfo.Output("Exited ValidateIssuerAndRootCertificate: certStatus = " + trustStatus, true);
			return trustStatus;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002E74 File Offset: 0x00001074
		internal static bool IsDisAllowedThumbprint(X509Certificate2 fileCert2)
		{
			return CertificateTools.DisAllowedThumbprints.Any((string thumbprint) => thumbprint.Equals(fileCert2.Thumbprint, StringComparison.InvariantCultureIgnoreCase));
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002EA4 File Offset: 0x000010A4
		internal static TrustStatus ValidateCertificateUsingThumbprintList(X509Certificate2 fileCert2)
		{
			TrustStatus trustStatus = TrustStatus.UntrustedFile;
			DebugInfo.Output("Entered ValidateCertificateUsingThumbprintList", true);
			try
			{
				if (fileCert2 != null && !string.IsNullOrEmpty(fileCert2.Thumbprint))
				{
					DebugInfo.Output(string.Format("Using thumprint {0}", fileCert2.Thumbprint), true);
					if (CertificateTools.ValidThumbprints.FirstOrDefault((string thumbprint) => thumbprint.Equals(fileCert2.Thumbprint, StringComparison.InvariantCultureIgnoreCase)) == null)
					{
						trustStatus = TrustStatus.NotLenovoCertificate;
					}
					else
					{
						trustStatus = TrustStatus.FileTrusted;
					}
				}
				else
				{
					DebugInfo.Output("Fatal Error: Thumbprint is null or empty", true);
				}
			}
			catch
			{
			}
			DebugInfo.Output("Entered ValidateCertificateUsingThumbprintList: certStatus = " + trustStatus, true);
			return trustStatus;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002F5C File Offset: 0x0000115C
		internal static TrustStatus ValidateCertificateUsingThumbprintList(string fileName)
		{
			DebugInfo.Output("Entered ValidateCertificateUsingThumbprintList: filename = " + fileName, true);
			TrustStatus trustStatus = TrustStatus.FileTrusted;
			if (!string.IsNullOrEmpty(fileName))
			{
				if (File.Exists(fileName))
				{
					List<X509Certificate2> signingCertificates = CertificateTools.GetSigningCertificates(fileName);
					if (signingCertificates.Count > 0)
					{
						DebugInfo.Output(string.Format("Got {0} signing certificates", signingCertificates.Count), true);
						X509Certificate2 fileCert2 = signingCertificates.Last<X509Certificate2>();
						if (!string.IsNullOrEmpty(fileCert2.Thumbprint))
						{
							DebugInfo.Output(string.Format("Using thumprint {0}", fileCert2.Thumbprint), true);
							if (CertificateTools.ValidThumbprints.FirstOrDefault((string thumbprint) => thumbprint.Equals(fileCert2.Thumbprint, StringComparison.InvariantCultureIgnoreCase)) == null)
							{
								trustStatus = TrustStatus.NotLenovoCertificate;
							}
						}
						else
						{
							DebugInfo.Output("Fatal Error: Thumbprint is null or empty", true);
						}
					}
					else
					{
						DebugInfo.Output("File not signed", true);
						trustStatus = TrustStatus.FileNotSigned;
					}
				}
				else
				{
					trustStatus = TrustStatus.FileNotFound;
				}
			}
			else
			{
				trustStatus = TrustStatus.InValidFilenamePassed;
			}
			TrustStatus trustStatus2 = trustStatus;
			DebugInfo.Output("Entered ValidateCertificateUsingThumbprintList: certStatus = " + trustStatus2, true);
			return trustStatus2;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003058 File Offset: 0x00001258
		private static TrustStatus ValidateCertificateSubject(X509Certificate2 lastCert)
		{
			TrustStatus trustStatus = TrustStatus.UntrustedFile;
			DebugInfo.Output("Entered ValidateCertificateSubject", true);
			try
			{
				if (lastCert != null && !string.IsNullOrEmpty(lastCert.Subject))
				{
					List<KeyValuePair<string, string>> subjectParts = CertificateTools.ParseDistinguishedName(lastCert.Subject.ToString().ToLowerInvariant());
					if (CertificateTools.IsSubjectPartsMatch(subjectParts, CertificateTools.ExpectedSubjectProperties) || CertificateTools.IsSubjectPartsMatch(subjectParts, CertificateTools.ExpectedSubjectProperties2))
					{
						trustStatus = TrustStatus.FileTrusted;
					}
				}
				else if (lastCert == null)
				{
					DebugInfo.Output("FATAL ERROR: lastCert is null", true);
				}
				else
				{
					DebugInfo.Output("FATAL ERROR: lastCert.Subject is null or empty", true);
				}
			}
			catch (Exception ex)
			{
				DebugInfo.Output("Exception thrown: " + ex.Message, true);
			}
			DebugInfo.Output("Exited ValidateCertificateSubject: trusted = " + trustStatus, true);
			return trustStatus;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003114 File Offset: 0x00001314
		private static bool IsSubjectPartsMatch(List<KeyValuePair<string, string>> subjectParts, List<KeyValuePair<string, string>> expectedSubjectParts)
		{
			bool result = true;
			foreach (KeyValuePair<string, string> keyValuePair in expectedSubjectParts)
			{
				bool flag = false;
				foreach (KeyValuePair<string, string> keyValuePair2 in subjectParts)
				{
					if (keyValuePair.Key.Equals(keyValuePair2.Key, StringComparison.InvariantCultureIgnoreCase) && keyValuePair2.Value.ToLowerInvariant().Contains(keyValuePair.Value.ToLowerInvariant()))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000031D8 File Offset: 0x000013D8
		private static TrustStatus ValidateCertificateDate(X509Certificate2 lastCert)
		{
			TrustStatus trustStatus = TrustStatus.CertificateIssuedTooEarly;
			DebugInfo.Output("Entered ValidateCertificateDate", true);
			DebugInfo.Output("CertValidAfter = " + CertificateTools.CertValidAfter, true);
			if (lastCert != null && lastCert.NotBefore >= CertificateTools.CertValidAfter)
			{
				DebugInfo.Output("lastCert NotBefore = " + lastCert.NotBefore, true);
				trustStatus = TrustStatus.FileTrusted;
			}
			DebugInfo.Output("Exited ValidateCertificateDate: trusted = " + trustStatus, true);
			return trustStatus;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00003258 File Offset: 0x00001458
		private static List<KeyValuePair<string, string>> ParseDistinguishedName(string input)
		{
			DebugInfo.Output("Entered ParseDistinguishedName: subject = " + input, true);
			int i = 0;
			int length = 0;
			int length2 = 0;
			char[] array = new char[50];
			char[] array2 = new char[200];
			bool flag = true;
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			string text;
			string text2;
			while (i < input.Length)
			{
				char c = input[i++];
				if (c != ',')
				{
					if (c != '=')
					{
						if (c == '\\')
						{
							array2[length2++] = c;
							array2[length2++] = input[i++];
						}
						else if (flag)
						{
							array[length++] = c;
						}
						else
						{
							array2[length2++] = c;
						}
					}
					else
					{
						flag = false;
					}
				}
				else
				{
					flag = true;
					text = new string(array).Substring(0, length);
					text2 = new string(array2).Substring(0, length2);
					list.Add(new KeyValuePair<string, string>(text.Trim(new char[] { ' ' }), text2.Trim(new char[] { ' ' })));
					length2 = (length = 0);
				}
			}
			text = new string(array).Substring(0, length);
			text2 = new string(array2).Substring(0, length2);
			list.Add(new KeyValuePair<string, string>(text.Trim(new char[] { ' ' }), text2.Trim(new char[] { ' ' })));
			DebugInfo.Output(list, true);
			DebugInfo.Output("Exited ParseDistingushedName", true);
			return list;
		}

		// Token: 0x04000022 RID: 34
		public const uint CERT_E_CHAIN = 2148204810U;

		// Token: 0x04000023 RID: 35
		private static readonly List<KeyValuePair<string, string>> ExpectedSubjectProperties = new List<KeyValuePair<string, string>>
		{
			new KeyValuePair<string, string>("CN", "Lenovo"),
			new KeyValuePair<string, string>("O", "Lenovo"),
			new KeyValuePair<string, string>("L", "Morrisville"),
			new KeyValuePair<string, string>("S", "North Carolina"),
			new KeyValuePair<string, string>("C", "US")
		};

		// Token: 0x04000024 RID: 36
		private static readonly List<KeyValuePair<string, string>> ExpectedNewSubjectProperties = new List<KeyValuePair<string, string>>
		{
			new KeyValuePair<string, string>("CN", "LENOVO (UNITED STATES) INC."),
			new KeyValuePair<string, string>("O", "LENOVO (UNITED STATES) INC."),
			new KeyValuePair<string, string>("L", "Morrisville"),
			new KeyValuePair<string, string>("S", "North Carolina"),
			new KeyValuePair<string, string>("C", "US")
		};

		// Token: 0x04000025 RID: 37
		private static readonly List<KeyValuePair<string, string>> ExpectedSubjectProperties2 = new List<KeyValuePair<string, string>>
		{
			new KeyValuePair<string, string>("CN", "Motorola Mobility LLC"),
			new KeyValuePair<string, string>("O", "Motorola Mobility LLC"),
			new KeyValuePair<string, string>("L", "Chicago"),
			new KeyValuePair<string, string>("S", "Illinois"),
			new KeyValuePair<string, string>("C", "US")
		};

		// Token: 0x04000026 RID: 38
		private static readonly DateTime CertValidAfter = new DateTime(2016, 2, 28);

		// Token: 0x04000027 RID: 39
		private static string[] ValidIssuingThumbprints = new string[] { "007790F6561DAD89B0BCD85585762495E358F8A5", "495847A93187CFB8C71F840CB7B41497AD95C64F", "1392E4C7FF25B9517E931077BBE2664DC87EF70D", "ABCFC30C619BDA3651A0820659A14D78F8DFE10D" };

		// Token: 0x04000028 RID: 40
		private static string[] ValidRootThumbPrints = new string[] { "0563B8630D62D75ABBC8AB1E4BDFB5A899B24D43", "5FB7EE0633E259DBAD0C4C9AE6D38F1A61C7DC25", "A14B48D943EE0A0E40904F3CE0A4C09193515D3F", "7E04DE896A3E666D00E687D33FFAD93BE83D349E", "DDFB16CD4931C973A2037D3FC83A4D7D775D05E4", "843573112A3B319344E5E4ECABC9F26C7CD54D07", "5EEED86FA37C675230642F55C84DDBF67CD33C80" };

		// Token: 0x04000029 RID: 41
		private static readonly string[] ValidThumbprintsArray = new string[]
		{
			"007790f6561dad89b0bcd85585762495e358f8a5", "01E88C4B67D0A17C8199DAAAFBB6E4D1C9AE078F", "037F4C3417A255E0D2DC49A2B6AB8F73AAC70566", "087c2825882a917093e485ec0d400c52c4ff7c7a", "0D3503BA760D79208664EB242790D908CAD94D70", "0d78c8e935f04e9aaf4b3b483e1dfa60ce5c4602", "12d4872bc3ef019e7e0b6f132480ae29db5b1ca3", "14e9eee313c7c696fda60c03ac1e8615fbcdc1b3", "194BCC554216F9E514A8C76DF4406CA01C95E6AB", "197a4aebdb25f0170079bb8c73cb2d655e0018a4",
			"29F99406A394B3F570863BA709DBD629681019BE", "2b39235d7e7c18ace9595fdc2cc3b37f3ed0b74e", "2cdac5a156aaeaf328a09a944e130e4c1870e6b4", "48e024a64dd2dd7c3a121bc03b0192d0724c148f", "3103B8559B1F54F992E93E7E5E7F8669DD872193", "32F30882622B87CF8856C63DB873DF0853B4DD27", "48E024A64DD2DD7C3A121BC03B0192D0724C148F", "495847a93187cfb8c71f840cb7b41497ad95c64f", "4EA167AFF233D55252BA00E88FF9686001F76C9C", "4eb6d578499b1ccf5f581ead56be3d9b6744a5e5",
			"4ED906C8DD5648052FB2194F405506B84E9255D7", "4FAC44FB51C81ADEA5EA87413657984F5D127598", "52495394309dd6ed3e41b562abbcc33888a69c9e", "57618828FC5D838BEFC832DD5144337A35AE7421", "5c52cfda009e757a2a096f8c9dcb8f214aff794a", "58455389CF1D0CD6A08E3CE216F65ADFF7A86408", "5ea07bca7948aa8f74875f67f8a67d76cd91a3e9", "62801d44abaa627157c0d42b31871e14b4a71514", "630105053d531f3f61e8f36dd7e22ca872d469e0", "742C3192E607E424EB4549542BE1BBC53E6174E2",
			"7FB04AAAF7A289857356AE1CD5F8FE20AA9881A8", "885926FD4E17BFD8FD3A9105AAC30D532D0CFDCB", "8FBE4D070EF8AB1BCCAF2A9D5CCAE7282A2C66B3", "9549B6A6CC9AD86789710820F5C82CFA6CA70D32", "9DC2D09D1C34A7DBB52FD459F06B6DAC949F5385", "\u200e9e8e57304a7e9a3819dc28e7773fffb9d359bfc2", "a1db6393916f17e4185509400415c70240b0ae6b", "a28b821a1d3faecd8f351b6466e050ed10a5ae1a", "\u200ea908b9800fda4bf736e73895666a48850506cb09", "B105D14DC20BD133C6F6B1A2C98545FFC80DB63F",
			"bec66acc61a3e0fd22f70be1da74302ef42b3e84", "bfa5731783161121244034554512adae0aeedcc0", "c818bcdc5fa50807ddc42480adf5c1c1189c75e2", "D566EA75A379B6006AA1D102E9A81A42C9384FAC", "D8FD5454A7201930810FF3E1458884037DB2DF89", "da4005c4302d1d3b92e7e7ac6d38af1b935de4f4", "E8DB74E5FC68E47EF02EB8F575ECC9EEF255515E", "fb18d839a78c599c965681fab70c2b044ef7a3f5", "cb33bc2420b3eda2c7fb2d8ce083d5f17d07d640", "e9d8ca5de2194fdd0ee673ef597a5a6d14805350",
			"61571245bf2e98f8bfc8d3a6563e8af9b69c62be", "09d56e88b2a50fb61d02659c7bfd7cfb3f39b473", "c184781cf29438b4daa1de8eeee5ac3fefe9512a", "582fc1e60d958c53e7b51f7a830ff72c362efaf3", "d2c701b99d9934d78388c7e3c4bf7f90136c7283", "\u200e506d1c154ce510d0e15a448d52995e126f1d98f4", "\u200eef87529d16837362803cca37ad301c9921d82c97"
		};

		// Token: 0x0400002A RID: 42
		private static List<string> ValidThumbprints = new List<string>(CertificateTools.ValidThumbprintsArray);

		// Token: 0x0400002B RID: 43
		private static readonly string[] DisAllowedThumbprintsArray = new string[] { "ef80fc5598746377be8126b3f0074f2609b7095b" };

		// Token: 0x0400002C RID: 44
		private static List<string> DisAllowedThumbprints = new List<string>(CertificateTools.DisAllowedThumbprintsArray);
	}
}
