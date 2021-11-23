using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Lenovo.Modern.Utilities.Services.Validation.Tvt
{
	// Token: 0x02000022 RID: 34
	public sealed class FileValidator
	{
		// Token: 0x060000BA RID: 186 RVA: 0x00004790 File Offset: 0x00002990
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
						if (trustStatus == TrustStatus.FileTrusted)
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

		// Token: 0x060000BB RID: 187 RVA: 0x00004850 File Offset: 0x00002A50
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

		// Token: 0x060000BC RID: 188 RVA: 0x00004884 File Offset: 0x00002A84
		public static AsyncResult BeginGetTrustStatus(string fileName, AsyncCallback callback, object state)
		{
			AsyncResult result = new AsyncResult(state);
			bool oldCertificate;
			uint WinVerifyTrustStatus;
			ThreadPool.QueueUserWorkItem(delegate(object Action)
			{
				TrustStatus trustStatus = FileValidator.GetTrustStatus(fileName, out oldCertificate, out WinVerifyTrustStatus);
				result.Result = trustStatus;
				result.OldCertificateUsedForValidation = oldCertificate;
				result.WinVerifyTrustStatus = WinVerifyTrustStatus;
				result.IsCompleted = true;
				((ManualResetEvent)result.AsyncWaitHandle).Set();
				if (callback != null)
				{
					callback(result);
				}
			});
			return result;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000048BC File Offset: 0x00002ABC
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

		// Token: 0x060000BE RID: 190 RVA: 0x000048E4 File Offset: 0x00002AE4
		private static TrustStatus ValidateALenovoCertificateInChain(string fileName, out bool thumprintListUsed)
		{
			TrustStatus trustStatus = TrustStatus.UntrustedFile;
			DebugInfo.Output("Entered ValidateALenovoCertificateInChain", true);
			thumprintListUsed = false;
			try
			{
				TrustStatus trustStatus2 = TrustStatus.UntrustedFile;
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
									trustStatus2 = FileValidator.ValidateSigningCertificate(signingCert, out thumprintListUsed);
									if (trustStatus2 == TrustStatus.FileTrusted)
									{
										break;
									}
									trustStatus2 = TrustStatus.NotLenovoCertificate;
								}
								goto IL_8F;
							}
						}
						trustStatus2 = TrustStatus.FileNotSigned;
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
				IL_8F:
				trustStatus = trustStatus2;
			}
			catch
			{
			}
			DebugInfo.Output("Exited ValidateCertificateUsingCertProperties: certStatus = " + trustStatus, true);
			return trustStatus;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000049BC File Offset: 0x00002BBC
		private static TrustStatus ValidateSigningCertificate(X509Certificate2 signingCert, out bool thumprintListUsed)
		{
			TrustStatus result = TrustStatus.FileNotTrusted;
			thumprintListUsed = false;
			try
			{
				TrustStatus trustStatus;
				if (CertificateTools.ValidateCertificateProperties(signingCert) == TrustStatus.FileTrusted)
				{
					DebugInfo.Output("Certificate properties OK, now validating issuing certificate", true);
					trustStatus = CertificateTools.ValidateIssuingCertificate(signingCert);
				}
				else
				{
					trustStatus = CertificateTools.ValidateCertificateUsingThumbprintList(signingCert);
					thumprintListUsed = true;
				}
				result = trustStatus;
			}
			catch
			{
			}
			return result;
		}
	}
}
