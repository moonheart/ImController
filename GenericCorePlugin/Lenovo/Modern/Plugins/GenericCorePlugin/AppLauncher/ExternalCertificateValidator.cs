using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppLauncher
{
	// Token: 0x02000041 RID: 65
	internal class ExternalCertificateValidator
	{
		// Token: 0x0600017B RID: 379 RVA: 0x0000A9AD File Offset: 0x00008BAD
		public bool AssertDigitalSignatureIsValid(string filePath)
		{
			return ExternalCertificateValidator.GetTrustStatus(filePath) == ExternalCertificateValidator.TrustStatus.FileTrusted;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000A9BC File Offset: 0x00008BBC
		private static ExternalCertificateValidator.TrustStatus GetTrustStatus(string fileName)
		{
			ExternalCertificateValidator.TrustStatus trustStatus = ExternalCertificateValidator.TrustStatus.UntrustedFile;
			try
			{
				if (!string.IsNullOrEmpty(fileName))
				{
					if (File.Exists(fileName))
					{
						trustStatus = ExternalCertificateValidator.WinVerifyTrust(fileName);
						if (trustStatus == ExternalCertificateValidator.TrustStatus.FileTrusted)
						{
							trustStatus = ExternalCertificateValidator.GetValidCertificateStatus(fileName);
						}
					}
					else
					{
						trustStatus = ExternalCertificateValidator.TrustStatus.FileNotFound;
					}
				}
				else
				{
					trustStatus = ExternalCertificateValidator.TrustStatus.InValidFilenamePassed;
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception in FinaryFileValidator.GetTrustStatus");
			}
			return trustStatus;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000AA14 File Offset: 0x00008C14
		private static ExternalCertificateValidator.TrustStatus GetValidCertificateStatus(string fileName)
		{
			ExternalCertificateValidator.TrustStatus result = ExternalCertificateValidator.TrustStatus.UntrustedFile;
			try
			{
				ExternalCertificateValidator.TrustStatus trustStatus = ExternalCertificateValidator.TrustStatus.FileTrusted;
				List<string> validThumprints = ExternalCertificateValidator.GetValidThumprints();
				if (!string.IsNullOrEmpty(fileName))
				{
					if (File.Exists(fileName))
					{
						X509Certificate x509Certificate = X509Certificate.CreateFromSignedFile(fileName);
						if (x509Certificate != null)
						{
							X509Certificate2 fileCert2 = new X509Certificate2(x509Certificate);
							if (!string.IsNullOrEmpty(fileCert2.Thumbprint) && validThumprints.FirstOrDefault((string thumbprint) => thumbprint.Equals(fileCert2.Thumbprint, StringComparison.InvariantCultureIgnoreCase)) == null)
							{
								trustStatus = ExternalCertificateValidator.TrustStatus.NotExternalTruestedCertificate;
								Logger.Log(Logger.LogSeverity.Warning, "Unable to verify the thumbprint for " + fileName);
							}
						}
						else
						{
							trustStatus = ExternalCertificateValidator.TrustStatus.FileNotSigned;
						}
					}
					else
					{
						trustStatus = ExternalCertificateValidator.TrustStatus.FileNotFound;
					}
				}
				else
				{
					trustStatus = ExternalCertificateValidator.TrustStatus.InValidFilenamePassed;
				}
				result = trustStatus;
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception in BinaryFileValidator.LenovoCertificateStatus");
			}
			return result;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000AAC4 File Offset: 0x00008CC4
		private static ExternalCertificateValidator.TrustStatus WinVerifyTrust(string fileName)
		{
			ExternalCertificateValidator.TrustStatus result = ExternalCertificateValidator.TrustStatus.WinVerifyTrustFailed;
			Guid structure = new Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}");
			using (ExternalCertificateValidator.WINTRUST_FILE_INFO fileInfo = new ExternalCertificateValidator.WINTRUST_FILE_INFO(fileName, Guid.Empty))
			{
				using (ExternalCertificateValidator.UnmanagedPointer unmanagedPointer = new ExternalCertificateValidator.UnmanagedPointer(Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Guid))), ExternalCertificateValidator.AllocMethod.HGlobal))
				{
					using (ExternalCertificateValidator.UnmanagedPointer unmanagedPointer2 = new ExternalCertificateValidator.UnmanagedPointer(Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ExternalCertificateValidator.WINTRUST_DATA))), ExternalCertificateValidator.AllocMethod.HGlobal))
					{
						ExternalCertificateValidator.WINTRUST_DATA structure2 = new ExternalCertificateValidator.WINTRUST_DATA(fileInfo);
						IntPtr intPtr = unmanagedPointer;
						IntPtr intPtr2 = unmanagedPointer2;
						Marshal.StructureToPtr<Guid>(structure, intPtr, true);
						Marshal.StructureToPtr<ExternalCertificateValidator.WINTRUST_DATA>(structure2, intPtr2, true);
						if (ExternalCertificateValidator.WinVerifyTrust(IntPtr.Zero, intPtr, intPtr2) == 0U)
						{
							result = ExternalCertificateValidator.TrustStatus.FileTrusted;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000ABAC File Offset: 0x00008DAC
		private static List<string> GetValidThumprints()
		{
			return new List<string> { "\u200e61B09D4DB366700790FBA72F47ED0032F56B97FB", "\u200e73faeba4094658a83811cdc2e7934078aa41b363" };
		}

		// Token: 0x06000180 RID: 384
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("Wintrust.dll")]
		private static extern uint WinVerifyTrust(IntPtr hWnd, IntPtr pgActionID, IntPtr pWinTrustData);

		// Token: 0x020000A3 RID: 163
		private enum TrustStatus
		{
			// Token: 0x04000269 RID: 617
			FileTrusted,
			// Token: 0x0400026A RID: 618
			NotExternalTruestedCertificate,
			// Token: 0x0400026B RID: 619
			FileNotFound,
			// Token: 0x0400026C RID: 620
			InValidFilenamePassed,
			// Token: 0x0400026D RID: 621
			CertificateContentsInvalid,
			// Token: 0x0400026E RID: 622
			FileNotSigned,
			// Token: 0x0400026F RID: 623
			WinVerifyTrustFailed,
			// Token: 0x04000270 RID: 624
			UntrustedFile
		}

		// Token: 0x020000A4 RID: 164
		internal struct WINTRUST_FILE_INFO : IDisposable
		{
			// Token: 0x06000236 RID: 566 RVA: 0x00011E34 File Offset: 0x00010034
			public WINTRUST_FILE_INFO(string fileName, Guid subject)
			{
				this.cbStruct = (uint)Marshal.SizeOf(typeof(ExternalCertificateValidator.WINTRUST_FILE_INFO));
				this.pcwszFilePath = fileName;
				if (subject != Guid.Empty)
				{
					this.pgKnownSubject = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Guid)));
					Marshal.StructureToPtr<Guid>(subject, this.pgKnownSubject, true);
				}
				else
				{
					this.pgKnownSubject = IntPtr.Zero;
				}
				this.hFile = IntPtr.Zero;
			}

			// Token: 0x06000237 RID: 567 RVA: 0x00011EA9 File Offset: 0x000100A9
			public void Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x06000238 RID: 568 RVA: 0x00011EB2 File Offset: 0x000100B2
			private void Dispose(bool disposing)
			{
				if (this.pgKnownSubject != IntPtr.Zero)
				{
					Marshal.DestroyStructure(this.pgKnownSubject, typeof(Guid));
					Marshal.FreeHGlobal(this.pgKnownSubject);
				}
			}

			// Token: 0x04000271 RID: 625
			private uint cbStruct;

			// Token: 0x04000272 RID: 626
			[MarshalAs(UnmanagedType.LPTStr)]
			private string pcwszFilePath;

			// Token: 0x04000273 RID: 627
			private IntPtr hFile;

			// Token: 0x04000274 RID: 628
			private IntPtr pgKnownSubject;
		}

		// Token: 0x020000A5 RID: 165
		internal enum AllocMethod
		{
			// Token: 0x04000276 RID: 630
			HGlobal,
			// Token: 0x04000277 RID: 631
			CoTaskMem
		}

		// Token: 0x020000A6 RID: 166
		private enum UnionChoice
		{
			// Token: 0x04000279 RID: 633
			File = 1,
			// Token: 0x0400027A RID: 634
			Catalog,
			// Token: 0x0400027B RID: 635
			Blob,
			// Token: 0x0400027C RID: 636
			Signer,
			// Token: 0x0400027D RID: 637
			Cert
		}

		// Token: 0x020000A7 RID: 167
		private enum UiChoice
		{
			// Token: 0x0400027F RID: 639
			All = 1,
			// Token: 0x04000280 RID: 640
			NoUI,
			// Token: 0x04000281 RID: 641
			NoBad,
			// Token: 0x04000282 RID: 642
			NoGood
		}

		// Token: 0x020000A8 RID: 168
		private enum RevocationCheckFlags
		{
			// Token: 0x04000284 RID: 644
			None,
			// Token: 0x04000285 RID: 645
			WholeChain
		}

		// Token: 0x020000A9 RID: 169
		private enum StateAction
		{
			// Token: 0x04000287 RID: 647
			Ignore,
			// Token: 0x04000288 RID: 648
			Verify,
			// Token: 0x04000289 RID: 649
			Close,
			// Token: 0x0400028A RID: 650
			AutoCache,
			// Token: 0x0400028B RID: 651
			AutoCacheFlush
		}

		// Token: 0x020000AA RID: 170
		private enum TrustProviderFlags
		{
			// Token: 0x0400028D RID: 653
			UseIE4Trust = 1,
			// Token: 0x0400028E RID: 654
			NoIE4Chain,
			// Token: 0x0400028F RID: 655
			NoPolicyUsage = 4,
			// Token: 0x04000290 RID: 656
			RevocationCheckNone = 16,
			// Token: 0x04000291 RID: 657
			RevocationCheckEndCert = 32,
			// Token: 0x04000292 RID: 658
			RevocationCheckChain = 64,
			// Token: 0x04000293 RID: 659
			RecovationCheckChainExcludeRoot = 128,
			// Token: 0x04000294 RID: 660
			Safer = 256,
			// Token: 0x04000295 RID: 661
			HashOnly = 512,
			// Token: 0x04000296 RID: 662
			UseDefaultOSVerCheck = 1024,
			// Token: 0x04000297 RID: 663
			LifetimeSigning = 2048
		}

		// Token: 0x020000AB RID: 171
		private enum UIContext
		{
			// Token: 0x04000299 RID: 665
			Execute,
			// Token: 0x0400029A RID: 666
			Install
		}

		// Token: 0x020000AC RID: 172
		internal struct WINTRUST_DATA : IDisposable
		{
			// Token: 0x06000239 RID: 569 RVA: 0x00011EE8 File Offset: 0x000100E8
			public WINTRUST_DATA(ExternalCertificateValidator.WINTRUST_FILE_INFO fileInfo)
			{
				this.cbStruct = (uint)Marshal.SizeOf(typeof(ExternalCertificateValidator.WINTRUST_DATA));
				this.pInfoStruct = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ExternalCertificateValidator.WINTRUST_FILE_INFO)));
				Marshal.StructureToPtr<ExternalCertificateValidator.WINTRUST_FILE_INFO>(fileInfo, this.pInfoStruct, false);
				this.dwUnionChoice = ExternalCertificateValidator.UnionChoice.File;
				this.pPolicyCallbackData = IntPtr.Zero;
				this.pSIPCallbackData = IntPtr.Zero;
				this.dwUIChoice = ExternalCertificateValidator.UiChoice.NoUI;
				this.fdwRevocationChecks = ExternalCertificateValidator.RevocationCheckFlags.None;
				this.dwStateAction = ExternalCertificateValidator.StateAction.Ignore;
				this.hWVTStateData = IntPtr.Zero;
				this.pwszURLReference = IntPtr.Zero;
				this.dwProvFlags = ExternalCertificateValidator.TrustProviderFlags.RevocationCheckNone;
				this.dwUIContext = ExternalCertificateValidator.UIContext.Execute;
			}

			// Token: 0x0600023A RID: 570 RVA: 0x00011F88 File Offset: 0x00010188
			public void Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x0600023B RID: 571 RVA: 0x00011F94 File Offset: 0x00010194
			private void Dispose(bool disposing)
			{
				if (this.dwUnionChoice == ExternalCertificateValidator.UnionChoice.File)
				{
					ExternalCertificateValidator.WINTRUST_FILE_INFO structure = default(ExternalCertificateValidator.WINTRUST_FILE_INFO);
					Marshal.PtrToStructure<ExternalCertificateValidator.WINTRUST_FILE_INFO>(this.pInfoStruct, structure);
					structure.Dispose();
					Marshal.DestroyStructure(this.pInfoStruct, typeof(ExternalCertificateValidator.WINTRUST_FILE_INFO));
				}
				Marshal.FreeHGlobal(this.pInfoStruct);
			}

			// Token: 0x0400029B RID: 667
			private uint cbStruct;

			// Token: 0x0400029C RID: 668
			private IntPtr pPolicyCallbackData;

			// Token: 0x0400029D RID: 669
			private IntPtr pSIPCallbackData;

			// Token: 0x0400029E RID: 670
			private ExternalCertificateValidator.UiChoice dwUIChoice;

			// Token: 0x0400029F RID: 671
			private ExternalCertificateValidator.RevocationCheckFlags fdwRevocationChecks;

			// Token: 0x040002A0 RID: 672
			private ExternalCertificateValidator.UnionChoice dwUnionChoice;

			// Token: 0x040002A1 RID: 673
			private IntPtr pInfoStruct;

			// Token: 0x040002A2 RID: 674
			private ExternalCertificateValidator.StateAction dwStateAction;

			// Token: 0x040002A3 RID: 675
			private IntPtr hWVTStateData;

			// Token: 0x040002A4 RID: 676
			private IntPtr pwszURLReference;

			// Token: 0x040002A5 RID: 677
			private ExternalCertificateValidator.TrustProviderFlags dwProvFlags;

			// Token: 0x040002A6 RID: 678
			private ExternalCertificateValidator.UIContext dwUIContext;
		}

		// Token: 0x020000AD RID: 173
		internal sealed class UnmanagedPointer : IDisposable
		{
			// Token: 0x0600023C RID: 572 RVA: 0x00011FE5 File Offset: 0x000101E5
			internal UnmanagedPointer(IntPtr ptr, ExternalCertificateValidator.AllocMethod method)
			{
				this.m_meth = method;
				this.m_ptr = ptr;
			}

			// Token: 0x0600023D RID: 573 RVA: 0x00011FFC File Offset: 0x000101FC
			~UnmanagedPointer()
			{
				this.Dispose(false);
			}

			// Token: 0x0600023E RID: 574 RVA: 0x0001202C File Offset: 0x0001022C
			private void Dispose(bool disposing)
			{
				if (this.m_ptr != IntPtr.Zero)
				{
					if (this.m_meth == ExternalCertificateValidator.AllocMethod.HGlobal)
					{
						Marshal.FreeHGlobal(this.m_ptr);
					}
					else if (this.m_meth == ExternalCertificateValidator.AllocMethod.CoTaskMem)
					{
						Marshal.FreeCoTaskMem(this.m_ptr);
					}
					this.m_ptr = IntPtr.Zero;
				}
				if (disposing)
				{
					GC.SuppressFinalize(this);
				}
			}

			// Token: 0x0600023F RID: 575 RVA: 0x00012088 File Offset: 0x00010288
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06000240 RID: 576 RVA: 0x00012097 File Offset: 0x00010297
			public static implicit operator IntPtr(ExternalCertificateValidator.UnmanagedPointer ptr)
			{
				return ptr.m_ptr;
			}

			// Token: 0x040002A7 RID: 679
			private IntPtr m_ptr;

			// Token: 0x040002A8 RID: 680
			private ExternalCertificateValidator.AllocMethod m_meth;
		}
	}
}
