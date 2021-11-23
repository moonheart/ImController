using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Lenovo.CertificateValidation
{
	// Token: 0x02000007 RID: 7
	public sealed class FileValidator
	{
		// Token: 0x06000027 RID: 39 RVA: 0x000037E4 File Offset: 0x000019E4
		public static TrustStatus GetTrustStatus(string fileName, out bool oldCertificate, out uint WinVerifyTrustStatus)
		{
			TrustStatus trustStatus = TrustStatus.UntrustedFile;
			oldCertificate = false;
			WinVerifyTrustStatus = 0U;
			DebugInfo.Enabled = true;
			DebugInfo.Output("=====================================================================", true);
			DebugInfo.Output("Entered GetTrustStatus: " + fileName, true);
			try
			{
				if (!string.IsNullOrEmpty(fileName))
				{
					if (File.Exists(fileName))
					{
						int num = 6;
						do
						{
							trustStatus = WinVerifyTrustTools.WinVerifyTrust(fileName, out WinVerifyTrustStatus);
							if (2148204810U == WinVerifyTrustStatus)
							{
								num--;
								Thread.Sleep(750);
							}
						}
						while (WinVerifyTrustStatus == 2148204810U && num != 0);
						if (trustStatus == TrustStatus.FileTrusted || WinVerifyTrustStatus == 2148204810U)
						{
							trustStatus = FileValidator.ValidateALenovoCertificateInChain(fileName, out oldCertificate);
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
			}
			catch
			{
			}
			DebugInfo.Output("Exited GetTrustStatus. trusted = " + trustStatus, true);
			DebugInfo.Output("=====================================================================", true);
			return trustStatus;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000038B0 File Offset: 0x00001AB0
		public static TrustStatus GetTrustStatus(string fileName)
		{
			TrustStatus result = TrustStatus.UntrustedFile;
			try
			{
				bool flag;
				uint num;
				result = FileValidator.GetTrustStatus(fileName, out flag, out num);
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000038E4 File Offset: 0x00001AE4
		public static bool FetchAdditionalThumbPrint(string cachingFolder)
		{
			return AdditionalThumbFile.GetAdditionalThumbPrints(cachingFolder);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000038EC File Offset: 0x00001AEC
		public static TrustStatus GetCertTrustStatus(string fileName)
		{
			bool flag;
			return FileValidator.GetCertTrustStatus(fileName, out flag);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003904 File Offset: 0x00001B04
		public static TrustStatus GetCertTrustStatus(string fileName, out bool oldCertificate)
		{
			TrustStatus result = TrustStatus.UntrustedFile;
			oldCertificate = false;
			try
			{
				if (string.IsNullOrEmpty(fileName))
				{
					return TrustStatus.InValidFilenamePassed;
				}
				if (File.Exists(fileName))
				{
					return TrustStatus.FileNotFound;
				}
				bool flag;
				result = FileValidator.ValidateSigningCertificate(new X509Certificate2(fileName), out flag);
				oldCertificate = !flag;
			}
			catch
			{
				result = TrustStatus.InvalidCertificate;
			}
			return result;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003960 File Offset: 0x00001B60
		public static AsyncResult BeginGetTrustStatus(string fileName, AsyncCallback callback, object state)
		{
			AsyncResult result = new AsyncResult(state);
			ThreadPool.QueueUserWorkItem(delegate(object Action)
			{
				bool oldCertificateUsedForValidation;
				uint winVerifyTrustStatus;
				TrustStatus trustStatus = FileValidator.GetTrustStatus(fileName, out oldCertificateUsedForValidation, out winVerifyTrustStatus);
				result.Result = trustStatus;
				result.OldCertificateUsedForValidation = oldCertificateUsedForValidation;
				result.WinVerifyTrustStatus = winVerifyTrustStatus;
				result.IsCompleted = true;
				((ManualResetEvent)result.AsyncWaitHandle).Set();
				if (callback != null)
				{
					callback(result);
				}
			});
			return result;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003998 File Offset: 0x00001B98
		public static TrustStatus EndGetTrustStatus(AsyncResult result)
		{
			TrustStatus result2 = TrustStatus.UntrustedFile;
			if (result != null)
			{
				result.AsyncWaitHandle.WaitOne();
				result2 = result.Result;
			}
			return result2;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000039C0 File Offset: 0x00001BC0
		private static TrustStatus ValidateALenovoCertificateInChain(string fileName, out bool thumprintListUsed)
		{
			DebugInfo.Output("Entered ValidateALenovoCertificateInChain", true);
			thumprintListUsed = false;
			TrustStatus trustStatus = TrustStatus.UntrustedFile;
			if (!string.IsNullOrEmpty(fileName))
			{
				if (File.Exists(fileName))
				{
					List<X509Certificate2> signingCertificates = CertificateTools.GetSigningCertificates(fileName);
					if (signingCertificates.Count > 0)
					{
						DebugInfo.Output(string.Format("Got {0} signing certs", signingCertificates.Count), true);
						using (IEnumerator<X509Certificate2> enumerator = signingCertificates.Reverse<X509Certificate2>().GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								X509Certificate2 signingCert = enumerator.Current;
								trustStatus = FileValidator.ValidateSigningCertificate(signingCert, out thumprintListUsed);
								if (trustStatus == TrustStatus.FileTrusted)
								{
									break;
								}
								trustStatus = TrustStatus.NotLenovoCertificate;
							}
							goto IL_8E;
						}
					}
					trustStatus = TrustStatus.FileNotSigned;
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
			IL_8E:
			TrustStatus trustStatus2 = trustStatus;
			DebugInfo.Output("Exited ValidateCertificateUsingCertProperties: certStatus = " + trustStatus2, true);
			return trustStatus2;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003A84 File Offset: 0x00001C84
		private static TrustStatus ValidateSigningCertificate(X509Certificate2 signingCert, out bool thumprintListUsed)
		{
			thumprintListUsed = false;
			TrustStatus trustStatus;
			if (CertificateTools.ValidateCertificateProperties(signingCert) == TrustStatus.FileTrusted)
			{
				DebugInfo.Output("Certificate properties OK, now validating issuing certificate", true);
				trustStatus = CertificateTools.ValidateIssuerAndRootCertificate(signingCert);
				if (trustStatus == TrustStatus.FileTrusted && CertificateTools.IsDisAllowedThumbprint(signingCert))
				{
					trustStatus = TrustStatus.FileNotTrusted;
				}
			}
			else
			{
				trustStatus = CertificateTools.ValidateCertificateUsingThumbprintList(signingCert);
				thumprintListUsed = true;
			}
			return trustStatus;
		}
	}
}
