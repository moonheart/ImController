using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Lenovo.Modern.Utilities.Services.Validation.Tvt
{
	// Token: 0x02000024 RID: 36
	public static class RSATools
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004A21 File Offset: 0x00002C21
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00004A28 File Offset: 0x00002C28
		public static string ErrorMessage { get; set; }

		// Token: 0x060000C8 RID: 200 RVA: 0x00004A30 File Offset: 0x00002C30
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

		// Token: 0x060000C9 RID: 201 RVA: 0x00004A7C File Offset: 0x00002C7C
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

		// Token: 0x060000CA RID: 202 RVA: 0x00004AD4 File Offset: 0x00002CD4
		public static X509Certificate2 GetX509CertificateFromCertificateStore(StoreName name, StoreLocation location, string certName)
		{
			X509Certificate2 result = null;
			try
			{
				X509Store x509Store = new X509Store(name, location);
				x509Store.Open(OpenFlags.ReadOnly);
				X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindBySubjectName, certName, false);
				if (x509Certificate2Collection.Count == 1)
				{
					result = x509Certificate2Collection[0];
				}
				else if (x509Certificate2Collection.Count > 0)
				{
					foreach (X509Certificate2 x509Certificate in x509Certificate2Collection)
					{
						if (x509Certificate.Subject.ToLower().Contains("cn=lenovo"))
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

		// Token: 0x060000CB RID: 203 RVA: 0x00004B9C File Offset: 0x00002D9C
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

		// Token: 0x060000CC RID: 204 RVA: 0x00004BE0 File Offset: 0x00002DE0
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
						if (x509Certificate.Subject.ToLower().Contains("cn=lenovo"))
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

		// Token: 0x060000CD RID: 205 RVA: 0x00004CD8 File Offset: 0x00002ED8
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

		// Token: 0x060000CE RID: 206 RVA: 0x00004D68 File Offset: 0x00002F68
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

		// Token: 0x060000CF RID: 207 RVA: 0x00004DC4 File Offset: 0x00002FC4
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

		// Token: 0x060000D0 RID: 208 RVA: 0x00004E2C File Offset: 0x0000302C
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

		// Token: 0x060000D1 RID: 209 RVA: 0x00004E80 File Offset: 0x00003080
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
