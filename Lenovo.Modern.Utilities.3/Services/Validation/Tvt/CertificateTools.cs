using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace Lenovo.Modern.Utilities.Services.Validation.Tvt
{
	// Token: 0x02000021 RID: 33
	internal class CertificateTools
	{
		// Token: 0x060000AF RID: 175 RVA: 0x000038E0 File Offset: 0x00001AE0
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
								if (WinVerifyTrustTools.WinVerifyTrust(IntPtr.Zero, intPtr, intPtr2) == 0U)
								{
									if (unmanagedPointer3 != IntPtr.Zero)
									{
										wintrust_SIGNATURE_SETTINGS = (WinVerifyTrustTools.WINTRUST_SIGNATURE_SETTINGS)Marshal.PtrToStructure(unmanagedPointer3, typeof(WinVerifyTrustTools.WINTRUST_SIGNATURE_SETTINGS));
									}
									uint num = wintrust_SIGNATURE_SETTINGS.cSecondarySigs + 1U;
									for (uint num2 = 0U; num2 < num; num2 += 1U)
									{
										try
										{
											if (unmanagedPointer3 != IntPtr.Zero)
											{
												wintrust_SIGNATURE_SETTINGS.dwIndex = num2;
												wintrust_SIGNATURE_SETTINGS.dwFlags = 1U;
												Marshal.StructureToPtr(wintrust_SIGNATURE_SETTINGS, unmanagedPointer3, false);
											}
											wintrust_DATA.dwStateAction = WinVerifyTrustTools.StateAction.Verify;
											wintrust_DATA.hWVTStateData = IntPtr.Zero;
											Marshal.StructureToPtr(wintrust_DATA, intPtr2, true);
											WinVerifyTrustTools.WinVerifyTrust(IntPtr.Zero, intPtr, intPtr2);
											wintrust_DATA = (WinVerifyTrustTools.WINTRUST_DATA)Marshal.PtrToStructure(intPtr2, typeof(WinVerifyTrustTools.WINTRUST_DATA));
											IntPtr intPtr3 = WinVerifyTrustTools.WTHelperProvDataFromStateData(wintrust_DATA.hWVTStateData);
											WinVerifyTrustTools.CRYPT_PROVIDER_DATA crypt_PROVIDER_DATA = (WinVerifyTrustTools.CRYPT_PROVIDER_DATA)Marshal.PtrToStructure(intPtr3, typeof(WinVerifyTrustTools.CRYPT_PROVIDER_DATA));
											for (uint num3 = 0U; num3 < crypt_PROVIDER_DATA.csSigners; num3 += 1U)
											{
												IntPtr intPtr4 = WinVerifyTrustTools.WTHelperGetProvSignerFromChain(intPtr3, num3, false, 0U);
												WinVerifyTrustTools.CRYPT_PROVIDER_SGNR crypt_PROVIDER_SGNR = (WinVerifyTrustTools.CRYPT_PROVIDER_SGNR)Marshal.PtrToStructure(intPtr4, typeof(WinVerifyTrustTools.CRYPT_PROVIDER_SGNR));
												WinVerifyTrustTools.CRYPT_PROVIDER_CERT crypt_PROVIDER_CERT = (WinVerifyTrustTools.CRYPT_PROVIDER_CERT)Marshal.PtrToStructure(WinVerifyTrustTools.WTHelperGetProvCertFromChain(intPtr4, num3), typeof(WinVerifyTrustTools.CRYPT_PROVIDER_CERT));
												if (crypt_PROVIDER_CERT.cbStruct > 0U)
												{
													X509Certificate2 item = new X509Certificate2(crypt_PROVIDER_CERT.pCert);
													list.Add(item);
												}
											}
										}
										catch
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

		// Token: 0x060000B0 RID: 176 RVA: 0x00003CDC File Offset: 0x00001EDC
		internal static StringBuilder GetChainInfo(string fileName, bool displayCorrectChainsAlso)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			try
			{
				if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
				{
					X509Certificate x509Certificate = X509Certificate.CreateFromSignedFile(fileName);
					if (x509Certificate != null && x509Certificate.Subject.IndexOf("LENOVO", 0, StringComparison.OrdinalIgnoreCase) >= 0)
					{
						stringBuilder.AppendLine(fileName);
						X509Chain x509Chain = new X509Chain();
						X509Certificate2 certificate = new X509Certificate2(x509Certificate);
						x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
						x509Chain.Build(certificate);
						X509ChainElementEnumerator enumerator = x509Chain.ChainElements.GetEnumerator();
						while (enumerator.MoveNext())
						{
							X509ChainElement element = enumerator.Current;
							if (element != null && element.Certificate != null && !string.IsNullOrEmpty(element.Certificate.Thumbprint))
							{
								stringBuilder.Append("\t");
								stringBuilder.Append(element.Certificate.Thumbprint);
								stringBuilder.Append("\t");
								stringBuilder.AppendLine(element.Certificate.Subject);
								if (CertificateTools.ValidThumbprints.FirstOrDefault((string thumbprint) => thumbprint.Equals(element.Certificate.Thumbprint, StringComparison.InvariantCultureIgnoreCase)) == null)
								{
									flag = true;
								}
							}
						}
					}
				}
			}
			catch
			{
			}
			if (!displayCorrectChainsAlso && !flag)
			{
				stringBuilder = null;
			}
			return stringBuilder;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003E48 File Offset: 0x00002048
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

		// Token: 0x060000B2 RID: 178 RVA: 0x00003E8C File Offset: 0x0000208C
		internal static TrustStatus ValidateIssuingCertificate(X509Certificate2 fileCert2)
		{
			TrustStatus trustStatus = TrustStatus.IssuingCertificateNotValid;
			DebugInfo.Output("Entered ValidateIssuingCertificate", true);
			try
			{
				X509Chain x509Chain = new X509Chain();
				x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
				x509Chain.Build(fileCert2);
				if (x509Chain.ChainElements != null && x509Chain.ChainElements.Count > 1)
				{
					DebugInfo.Output(string.Format("Got {0} certificates in the chain", x509Chain.ChainElements.Count), true);
					X509ChainElement element = x509Chain.ChainElements[1];
					if (element != null && element.Certificate != null)
					{
						if (!string.IsNullOrEmpty(element.Certificate.Thumbprint))
						{
							DebugInfo.Output("Looking at thumbprint: " + element.Certificate.Thumbprint, true);
							if (CertificateTools.ValidIssuingThumbprints.FirstOrDefault((string thumbprint) => thumbprint.Equals(element.Certificate.Thumbprint)) != null)
							{
								trustStatus = TrustStatus.FileTrusted;
							}
						}
						else
						{
							DebugInfo.Output("Fatal Error: element.Certificate.Thumbprint is null or empty!", true);
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
			catch
			{
			}
			DebugInfo.Output("Exited ValidateIssuingCertificate: certStatus = " + trustStatus, true);
			return trustStatus;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004000 File Offset: 0x00002200
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

		// Token: 0x060000B4 RID: 180 RVA: 0x000040B8 File Offset: 0x000022B8
		internal static TrustStatus ValidateCertificateUsingThumbprintList(string fileName)
		{
			TrustStatus trustStatus = TrustStatus.UntrustedFile;
			DebugInfo.Output("Entered ValidateCertificateUsingThumbprintList: filename = " + fileName, true);
			try
			{
				TrustStatus trustStatus2 = TrustStatus.FileTrusted;
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
									trustStatus2 = TrustStatus.NotLenovoCertificate;
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
							trustStatus2 = TrustStatus.FileNotSigned;
						}
					}
					else
					{
						trustStatus2 = TrustStatus.FileNotFound;
					}
				}
				else
				{
					trustStatus2 = TrustStatus.InValidFilenamePassed;
				}
				trustStatus = trustStatus2;
			}
			catch
			{
			}
			DebugInfo.Output("Entered ValidateCertificateUsingThumbprintList: certStatus = " + trustStatus, true);
			return trustStatus;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000041CC File Offset: 0x000023CC
		private static TrustStatus ValidateCertificateSubject(X509Certificate2 lastCert)
		{
			TrustStatus trustStatus = TrustStatus.UntrustedFile;
			DebugInfo.Output("Entered ValidateCertificateSubject", true);
			try
			{
				if (lastCert != null && !string.IsNullOrEmpty(lastCert.Subject))
				{
					List<KeyValuePair<string, string>> subjectParts = CertificateTools.ParseDistinguishedName(lastCert.Subject);
					string sPattern = "(G\\d{2})";
					if (CertificateTools.ExpectedSubjectProperties.All((KeyValuePair<string, string> p) => subjectParts.Contains(p)) && subjectParts.Any((KeyValuePair<string, string> p) => Regex.IsMatch(p.Value, sPattern)))
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

		// Token: 0x060000B6 RID: 182 RVA: 0x000042AC File Offset: 0x000024AC
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

		// Token: 0x060000B7 RID: 183 RVA: 0x0000432C File Offset: 0x0000252C
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

		// Token: 0x0400003D RID: 61
		private static List<KeyValuePair<string, string>> ExpectedSubjectProperties = new List<KeyValuePair<string, string>>
		{
			new KeyValuePair<string, string>("CN", "Lenovo"),
			new KeyValuePair<string, string>("O", "Lenovo"),
			new KeyValuePair<string, string>("L", "Morrisville"),
			new KeyValuePair<string, string>("S", "North Carolina"),
			new KeyValuePair<string, string>("C", "US")
		};

		// Token: 0x0400003E RID: 62
		private static DateTime CertValidAfter = new DateTime(2016, 2, 28);

		// Token: 0x0400003F RID: 63
		private static string[] ValidIssuingThumbprints = new string[] { "007790F6561DAD89B0BCD85585762495E358F8A5", "495847A93187CFB8C71F840CB7B41497AD95C64F", "1392E4C7FF25B9517E931077BBE2664DC87EF70D", "ABCFC30C619BDA3651A0820659A14D78F8DFE10D", "92C1588E85AF2201CE7915E8538B492F605B80C6" };

		// Token: 0x04000040 RID: 64
		private static string[] ValidThumbprints = new string[]
		{
			"C6520558FA8AD73E8C72C9AC51EC5471BE1715C3", "CC5EE80524D43ACD5A32AB1F3A9D163CEE924443", "0DB6ED63773CD32463B7B3B5A7392F737DF81D10", "BA600BCDA68DD5B4DAA951D1038C94262CC28C8B", "A908B9800FDA4BF736E73895666A48850506CB09", "94A39259FF5B572B63144485E10078E673E151E6", "007790f6561dad89b0bcd85585762495e358f8a5", "01E88C4B67D0A17C8199DAAAFBB6E4D1C9AE078F", "037F4C3417A255E0D2DC49A2B6AB8F73AAC70566", "087c2825882a917093e485ec0d400c52c4ff7c7a",
			"0D3503BA760D79208664EB242790D908CAD94D70", "0d78c8e935f04e9aaf4b3b483e1dfa60ce5c4602", "12d4872bc3ef019e7e0b6f132480ae29db5b1ca3", "14e9eee313c7c696fda60c03ac1e8615fbcdc1b3", "194BCC554216F9E514A8C76DF4406CA01C95E6AB", "197a4aebdb25f0170079bb8c73cb2d655e0018a4", "29F99406A394B3F570863BA709DBD629681019BE", "2b39235d7e7c18ace9595fdc2cc3b37f3ed0b74e", "2cdac5a156aaeaf328a09a944e130e4c1870e6b4", "48e024a64dd2dd7c3a121bc03b0192d0724c148f",
			"3103B8559B1F54F992E93E7E5E7F8669DD872193", "32F30882622B87CF8856C63DB873DF0853B4DD27", "48E024A64DD2DD7C3A121BC03B0192D0724C148F", "495847a93187cfb8c71f840cb7b41497ad95c64f", "4EA167AFF233D55252BA00E88FF9686001F76C9C", "4eb6d578499b1ccf5f581ead56be3d9b6744a5e5", "4ED906C8DD5648052FB2194F405506B84E9255D7", "4FAC44FB51C81ADEA5EA87413657984F5D127598", "52495394309dd6ed3e41b562abbcc33888a69c9e", "57618828FC5D838BEFC832DD5144337A35AE7421",
			"5c52cfda009e757a2a096f8c9dcb8f214aff794a", "58455389CF1D0CD6A08E3CE216F65ADFF7A86408", "5ea07bca7948aa8f74875f67f8a67d76cd91a3e9", "62801d44abaa627157c0d42b31871e14b4a71514", "630105053d531f3f61e8f36dd7e22ca872d469e0", "742C3192E607E424EB4549542BE1BBC53E6174E2", "7FB04AAAF7A289857356AE1CD5F8FE20AA9881A8", "885926FD4E17BFD8FD3A9105AAC30D532D0CFDCB", "8FBE4D070EF8AB1BCCAF2A9D5CCAE7282A2C66B3", "9549B6A6CC9AD86789710820F5C82CFA6CA70D32",
			"9DC2D09D1C34A7DBB52FD459F06B6DAC949F5385", "\u200e9e8e57304a7e9a3819dc28e7773fffb9d359bfc2", "a1db6393916f17e4185509400415c70240b0ae6b", "a28b821a1d3faecd8f351b6466e050ed10a5ae1a", "B105D14DC20BD133C6F6B1A2C98545FFC80DB63F", "bec66acc61a3e0fd22f70be1da74302ef42b3e84", "bfa5731783161121244034554512adae0aeedcc0", "c818bcdc5fa50807ddc42480adf5c1c1189c75e2", "D566EA75A379B6006AA1D102E9A81A42C9384FAC", "D8FD5454A7201930810FF3E1458884037DB2DF89",
			"da4005c4302d1d3b92e7e7ac6d38af1b935de4f4", "E8DB74E5FC68E47EF02EB8F575ECC9EEF255515E", "fb18d839a78c599c965681fab70c2b044ef7a3f5", "cb33bc2420b3eda2c7fb2d8ce083d5f17d07d640", "e9d8ca5de2194fdd0ee673ef597a5a6d14805350", "61571245bf2e98f8bfc8d3a6563e8af9b69c62be", "09d56e88b2a50fb61d02659c7bfd7cfb3f39b473", "c184781cf29438b4daa1de8eeee5ac3fefe9512a", "582fc1e60d958c53e7b51f7a830ff72c362efaf3", "d2c701b99d9934d78388c7e3c4bf7f90136c7283",
			"\u200e506d1c154ce510d0e15a448d52995e126f1d98f4", "\u200eef87529d16837362803cca37ad301c9921d82c97"
		};
	}
}
