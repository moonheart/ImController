using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Lenovo.CertificateValidation
{
	// Token: 0x0200000F RID: 15
	public static class RSATools
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000043CE File Offset: 0x000025CE
		// (set) Token: 0x06000053 RID: 83 RVA: 0x000043D5 File Offset: 0x000025D5
		public static string ErrorMessage { get; set; }

		// Token: 0x06000054 RID: 84 RVA: 0x000043E0 File Offset: 0x000025E0
		public static X509Certificate2 GetX509CertificateFromCertificateFile(string certificateFile)
		{
			X509Certificate2 result = null;
			try
			{
				if (!string.IsNullOrEmpty(certificateFile) && File.Exists(certificateFile))
				{
					result = new X509Certificate2(X509Certificate.CreateFromCertFile(certificateFile));
				}
			}
			catch (Exception ex)
			{
				RSATools.ErrorMessage = ex.Message;
			}
			return result;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000442C File Offset: 0x0000262C
		public static X509Certificate2 GetX509CertificateFromCertificateFile(string certificateFile, string password)
		{
			X509Certificate2 result = null;
			try
			{
				if (string.IsNullOrEmpty(password))
				{
					result = RSATools.GetX509CertificateFromCertificateFile(certificateFile);
				}
				else if (!string.IsNullOrEmpty(certificateFile) && File.Exists(certificateFile))
				{
					result = new X509Certificate2(certificateFile, password);
				}
			}
			catch (Exception ex)
			{
				RSATools.ErrorMessage = ex.Message;
			}
			return result;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004484 File Offset: 0x00002684
		public static X509Certificate2 GetX509CertificateFromCertificateStore(StoreName name, StoreLocation location, string certName)
		{
			X509Certificate2 result = null;
			try
			{
				X509Store x509Store = new X509Store(name, location);
				x509Store.Open(OpenFlags.ReadOnly);
				X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindBySubjectName, certName, false);
				if (x509Certificate2Collection.Count > 0)
				{
					foreach (X509Certificate2 x509Certificate in x509Certificate2Collection)
					{
						if (x509Certificate.Subject.ToLowerInvariant().Contains("cn=lenovo"))
						{
							result = x509Certificate;
							break;
						}
					}
				}
				else
				{
					RSATools.ErrorMessage = string.Format("No certificates found with name {0} in Store Location: {1} and Store Name: {2}", certName, location.ToString(), name.ToString());
				}
			}
			catch (Exception ex)
			{
				RSATools.ErrorMessage = ex.Message;
			}
			return result;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004538 File Offset: 0x00002738
		public static RSACryptoServiceProvider CreateRSAKeyFromString(string keyString)
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
			catch (Exception ex)
			{
				RSATools.ErrorMessage = ex.Message;
			}
			return rsacryptoServiceProvider;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000457C File Offset: 0x0000277C
		public static RSACryptoServiceProvider GetPrivateKeyFromCertificateStore(StoreName name, StoreLocation location, string certName)
		{
			RSACryptoServiceProvider result = null;
			X509Store x509Store = null;
			try
			{
				x509Store = new X509Store(name, location);
				x509Store.Open(OpenFlags.ReadOnly);
				X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindBySubjectName, certName, false);
				if (x509Certificate2Collection.Count == 1)
				{
					result = (RSACryptoServiceProvider)x509Certificate2Collection[0].PrivateKey;
				}
				else if (x509Certificate2Collection.Count > 0)
				{
					foreach (X509Certificate2 x509Certificate in x509Certificate2Collection)
					{
						if (x509Certificate.Subject.ToLowerInvariant().Contains("cn=lenovo"))
						{
							result = (RSACryptoServiceProvider)x509Certificate.PrivateKey;
							break;
						}
					}
				}
				else
				{
					RSATools.ErrorMessage = string.Format("No certificates found with name {0} in Store Location: {1} and Store Name: {2}", certName, location.ToString(), name.ToString());
				}
			}
			catch (Exception ex)
			{
				RSATools.ErrorMessage = ex.Message;
			}
			finally
			{
				if (x509Store != null)
				{
					x509Store.Close();
				}
			}
			return result;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004674 File Offset: 0x00002874
		public static RSACryptoServiceProvider GetPublicKeyFromCertificateStore(StoreName name, StoreLocation location, string certName)
		{
			RSACryptoServiceProvider result = null;
			try
			{
				X509Certificate2Collection x509Certificate2Collection = new X509Store(name, location).Certificates.Find(X509FindType.FindBySubjectName, certName, false);
				if (x509Certificate2Collection.Count > 0)
				{
					result = (RSACryptoServiceProvider)x509Certificate2Collection[0].PublicKey.Key;
				}
				else
				{
					RSATools.ErrorMessage = string.Format("No certificates found with name {0} in Store Location: {1} and Store Name: {2}", certName, location.ToString(), name.ToString());
				}
			}
			catch (Exception ex)
			{
				RSATools.ErrorMessage = ex.Message;
			}
			return result;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004704 File Offset: 0x00002904
		public static RSACryptoServiceProvider GetPublicKeyFromCertificateFile(string certificateFile)
		{
			RSACryptoServiceProvider result = null;
			try
			{
				if (!string.IsNullOrEmpty(certificateFile) && File.Exists(certificateFile))
				{
					result = (RSACryptoServiceProvider)new X509Certificate2(X509Certificate.CreateFromCertFile(certificateFile)).PublicKey.Key;
				}
			}
			catch (Exception ex)
			{
				RSATools.ErrorMessage = ex.Message;
			}
			return result;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004760 File Offset: 0x00002960
		public static RSACryptoServiceProvider GetPublicKeyFromCertificateFile(string certificateFile, string password)
		{
			RSACryptoServiceProvider result = null;
			try
			{
				if (string.IsNullOrEmpty(password))
				{
					result = RSATools.GetPublicKeyFromCertificateFile(certificateFile);
				}
				else if (!string.IsNullOrEmpty(certificateFile) && File.Exists(certificateFile))
				{
					result = (RSACryptoServiceProvider)new X509Certificate2(certificateFile, password).PublicKey.Key;
				}
			}
			catch (Exception ex)
			{
				RSATools.ErrorMessage = ex.Message;
			}
			return result;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000047C8 File Offset: 0x000029C8
		public static RSACryptoServiceProvider GetPrivateKeyFromCertificateFile(string certificateFile)
		{
			RSACryptoServiceProvider result = null;
			try
			{
				if (!string.IsNullOrEmpty(certificateFile) && File.Exists(certificateFile))
				{
					result = (RSACryptoServiceProvider)new X509Certificate2(X509Certificate.CreateFromCertFile(certificateFile)).PrivateKey;
				}
			}
			catch (Exception ex)
			{
				RSATools.ErrorMessage = ex.Message;
			}
			return result;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000481C File Offset: 0x00002A1C
		public static RSACryptoServiceProvider GetPrivateKeyFromCertificateFile(string certificateFile, string password)
		{
			RSACryptoServiceProvider result = null;
			try
			{
				if (string.IsNullOrEmpty(password))
				{
					result = RSATools.GetPrivateKeyFromCertificateFile(certificateFile);
				}
				else if (!string.IsNullOrEmpty(certificateFile) && File.Exists(certificateFile))
				{
					result = (RSACryptoServiceProvider)new X509Certificate2(certificateFile, password).PrivateKey;
				}
			}
			catch (Exception ex)
			{
				RSATools.ErrorMessage = ex.Message;
			}
			return result;
		}
	}
}
