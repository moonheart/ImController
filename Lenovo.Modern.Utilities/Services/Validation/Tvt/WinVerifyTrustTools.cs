using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Lenovo.Modern.Utilities.Services.Validation.Tvt
{
	// Token: 0x02000025 RID: 37
	internal class WinVerifyTrustTools
	{
		// Token: 0x060000D2 RID: 210
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("Wintrust.dll")]
		internal static extern uint WinVerifyTrust(IntPtr hWnd, IntPtr pgActionID, IntPtr pWinTrustData);

		// Token: 0x060000D3 RID: 211
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("Wintrust.dll")]
		internal static extern IntPtr WTHelperProvDataFromStateData(IntPtr pStateData);

		// Token: 0x060000D4 RID: 212
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("Wintrust.dll")]
		internal static extern IntPtr WTHelperGetProvSignerFromChain(IntPtr pProvData, uint idxSigner, bool fCounterSigner, uint idxCounterSigner);

		// Token: 0x060000D5 RID: 213
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("Wintrust.dll")]
		internal static extern IntPtr WTHelperGetProvCertFromChain(IntPtr pProvData, uint idxSigner);

		// Token: 0x060000D6 RID: 214 RVA: 0x00004EE4 File Offset: 0x000030E4
		internal static TrustStatus WinVerifyTrust(string fileName, out uint result)
		{
			DebugInfo.Output("Entered WinVerifyTrust", true);
			TrustStatus trustStatus = TrustStatus.WinVerifyTrustFailed;
			Guid guid = new Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}");
			result = 0U;
			using (WinVerifyTrustTools.WINTRUST_FILE_INFO fileInfo = new WinVerifyTrustTools.WINTRUST_FILE_INFO(fileName, Guid.Empty))
			{
				using (WinVerifyTrustTools.UnmanagedPointer unmanagedPointer = new WinVerifyTrustTools.UnmanagedPointer(Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Guid))), WinVerifyTrustTools.AllocMethod.HGlobal))
				{
					using (WinVerifyTrustTools.UnmanagedPointer unmanagedPointer2 = new WinVerifyTrustTools.UnmanagedPointer(Marshal.AllocHGlobal(Marshal.SizeOf(typeof(WinVerifyTrustTools.WINTRUST_DATA))), WinVerifyTrustTools.AllocMethod.HGlobal))
					{
						WinVerifyTrustTools.WINTRUST_DATA wintrust_DATA = new WinVerifyTrustTools.WINTRUST_DATA(fileInfo, IntPtr.Zero);
						IntPtr intPtr = unmanagedPointer;
						IntPtr intPtr2 = unmanagedPointer2;
						Marshal.StructureToPtr(guid, intPtr, true);
						Marshal.StructureToPtr(wintrust_DATA, intPtr2, true);
						result = WinVerifyTrustTools.WinVerifyTrust(IntPtr.Zero, intPtr, intPtr2);
						if (result == 0U)
						{
							trustStatus = TrustStatus.FileTrusted;
						}
					}
				}
			}
			DebugInfo.Output("Exited WinVerifyTrust: WinVerifyTrust result = " + string.Format("0x{0:X}", result), true);
			DebugInfo.Output("Exited WinVerifyTrust: retValue = " + string.Format("0x{0:X}", trustStatus), true);
			return trustStatus;
		}

		// Token: 0x02000061 RID: 97
		internal struct WINTRUST_FILE_INFO : IDisposable
		{
			// Token: 0x060001DC RID: 476 RVA: 0x00009784 File Offset: 0x00007984
			public WINTRUST_FILE_INFO(string fileName, Guid subject)
			{
				this.cbStruct = (uint)Marshal.SizeOf(typeof(WinVerifyTrustTools.WINTRUST_FILE_INFO));
				this.pcwszFilePath = fileName;
				if (subject != Guid.Empty)
				{
					this.pgKnownSubject = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Guid)));
					Marshal.StructureToPtr(subject, this.pgKnownSubject, true);
				}
				else
				{
					this.pgKnownSubject = IntPtr.Zero;
				}
				this.hFile = IntPtr.Zero;
			}

			// Token: 0x060001DD RID: 477 RVA: 0x000097FE File Offset: 0x000079FE
			public void Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x060001DE RID: 478 RVA: 0x00009807 File Offset: 0x00007A07
			private void Dispose(bool disposing)
			{
				if (this.pgKnownSubject != IntPtr.Zero)
				{
					Marshal.DestroyStructure(this.pgKnownSubject, typeof(Guid));
					Marshal.FreeHGlobal(this.pgKnownSubject);
				}
			}

			// Token: 0x040000FF RID: 255
			private uint cbStruct;

			// Token: 0x04000100 RID: 256
			[MarshalAs(UnmanagedType.LPTStr)]
			private string pcwszFilePath;

			// Token: 0x04000101 RID: 257
			private IntPtr hFile;

			// Token: 0x04000102 RID: 258
			private IntPtr pgKnownSubject;
		}

		// Token: 0x02000062 RID: 98
		internal enum SignatureSettingsFlags : uint
		{
			// Token: 0x04000104 RID: 260
			VerySpecific = 1U,
			// Token: 0x04000105 RID: 261
			GetSecondarySigCount
		}

		// Token: 0x02000063 RID: 99
		internal enum AllocMethod
		{
			// Token: 0x04000107 RID: 263
			HGlobal,
			// Token: 0x04000108 RID: 264
			CoTaskMem
		}

		// Token: 0x02000064 RID: 100
		internal enum UnionChoice
		{
			// Token: 0x0400010A RID: 266
			File = 1,
			// Token: 0x0400010B RID: 267
			Catalog,
			// Token: 0x0400010C RID: 268
			Blob,
			// Token: 0x0400010D RID: 269
			Signer,
			// Token: 0x0400010E RID: 270
			Cert
		}

		// Token: 0x02000065 RID: 101
		internal enum UiChoice
		{
			// Token: 0x04000110 RID: 272
			All = 1,
			// Token: 0x04000111 RID: 273
			NoUI,
			// Token: 0x04000112 RID: 274
			NoBad,
			// Token: 0x04000113 RID: 275
			NoGood
		}

		// Token: 0x02000066 RID: 102
		internal enum RevocationCheckFlags
		{
			// Token: 0x04000115 RID: 277
			None,
			// Token: 0x04000116 RID: 278
			WholeChain
		}

		// Token: 0x02000067 RID: 103
		internal enum StateAction
		{
			// Token: 0x04000118 RID: 280
			Ignore,
			// Token: 0x04000119 RID: 281
			Verify,
			// Token: 0x0400011A RID: 282
			Close,
			// Token: 0x0400011B RID: 283
			AutoCache,
			// Token: 0x0400011C RID: 284
			AutoCacheFlush
		}

		// Token: 0x02000068 RID: 104
		internal enum TrustProviderFlags
		{
			// Token: 0x0400011E RID: 286
			UseIE4Trust = 1,
			// Token: 0x0400011F RID: 287
			NoIE4Chain,
			// Token: 0x04000120 RID: 288
			NoPolicyUsage = 4,
			// Token: 0x04000121 RID: 289
			RevocationCheckNone = 16,
			// Token: 0x04000122 RID: 290
			RevocationCheckEndCert = 32,
			// Token: 0x04000123 RID: 291
			RevocationCheckChain = 64,
			// Token: 0x04000124 RID: 292
			RecovationCheckChainExcludeRoot = 128,
			// Token: 0x04000125 RID: 293
			Safer = 256,
			// Token: 0x04000126 RID: 294
			HashOnly = 512,
			// Token: 0x04000127 RID: 295
			UseDefaultOSVerCheck = 1024,
			// Token: 0x04000128 RID: 296
			LifetimeSigning = 2048,
			// Token: 0x04000129 RID: 297
			CacheOnlyUrlRetrieval = 4096
		}

		// Token: 0x02000069 RID: 105
		internal enum UIContext
		{
			// Token: 0x0400012B RID: 299
			Execute,
			// Token: 0x0400012C RID: 300
			Install
		}

		// Token: 0x0200006A RID: 106
		internal struct WINTRUST_SIGNATURE_SETTINGS
		{
			// Token: 0x060001DF RID: 479 RVA: 0x0000983B File Offset: 0x00007A3B
			public WINTRUST_SIGNATURE_SETTINGS(uint flags)
			{
				this.cbStruct = (uint)Marshal.SizeOf(typeof(WinVerifyTrustTools.WINTRUST_SIGNATURE_SETTINGS));
				this.dwIndex = 0U;
				this.dwFlags = flags;
				this.cSecondarySigs = 0U;
				this.dwVerifiedSigIndex = 0U;
				this.pCryptoPolicy = IntPtr.Zero;
			}

			// Token: 0x0400012D RID: 301
			internal uint cbStruct;

			// Token: 0x0400012E RID: 302
			internal uint dwIndex;

			// Token: 0x0400012F RID: 303
			internal uint dwFlags;

			// Token: 0x04000130 RID: 304
			internal uint cSecondarySigs;

			// Token: 0x04000131 RID: 305
			internal uint dwVerifiedSigIndex;

			// Token: 0x04000132 RID: 306
			internal IntPtr pCryptoPolicy;
		}

		// Token: 0x0200006B RID: 107
		internal struct WINTRUST_DATA : IDisposable
		{
			// Token: 0x060001E0 RID: 480 RVA: 0x0000987C File Offset: 0x00007A7C
			public WINTRUST_DATA(WinVerifyTrustTools.WINTRUST_FILE_INFO fileInfo, IntPtr pSignatureInfo)
			{
				this.cbStruct = (uint)Marshal.SizeOf(typeof(WinVerifyTrustTools.WINTRUST_DATA));
				this.pInfoStruct = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(WinVerifyTrustTools.WINTRUST_FILE_INFO)));
				Marshal.StructureToPtr(fileInfo, this.pInfoStruct, false);
				this.dwUnionChoice = WinVerifyTrustTools.UnionChoice.File;
				this.pPolicyCallbackData = IntPtr.Zero;
				this.pSIPCallbackData = IntPtr.Zero;
				this.dwUIChoice = WinVerifyTrustTools.UiChoice.NoUI;
				this.fdwRevocationChecks = WinVerifyTrustTools.RevocationCheckFlags.WholeChain;
				this.dwStateAction = WinVerifyTrustTools.StateAction.Ignore;
				this.hWVTStateData = IntPtr.Zero;
				this.pwszURLReference = IntPtr.Zero;
				this.dwProvFlags = WinVerifyTrustTools.TrustProviderFlags.RevocationCheckChain;
				this.dwUIContext = WinVerifyTrustTools.UIContext.Execute;
				this.pSignatureSettings = pSignatureInfo;
			}

			// Token: 0x060001E1 RID: 481 RVA: 0x00009928 File Offset: 0x00007B28
			public void Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x060001E2 RID: 482 RVA: 0x00009934 File Offset: 0x00007B34
			private void Dispose(bool disposing)
			{
				if (this.dwUnionChoice == WinVerifyTrustTools.UnionChoice.File)
				{
					WinVerifyTrustTools.WINTRUST_FILE_INFO wintrust_FILE_INFO = default(WinVerifyTrustTools.WINTRUST_FILE_INFO);
					Marshal.PtrToStructure(this.pInfoStruct, wintrust_FILE_INFO);
					wintrust_FILE_INFO.Dispose();
					Marshal.DestroyStructure(this.pInfoStruct, typeof(WinVerifyTrustTools.WINTRUST_FILE_INFO));
				}
				Marshal.FreeHGlobal(this.pInfoStruct);
			}

			// Token: 0x04000133 RID: 307
			internal uint cbStruct;

			// Token: 0x04000134 RID: 308
			internal IntPtr pPolicyCallbackData;

			// Token: 0x04000135 RID: 309
			internal IntPtr pSIPCallbackData;

			// Token: 0x04000136 RID: 310
			internal WinVerifyTrustTools.UiChoice dwUIChoice;

			// Token: 0x04000137 RID: 311
			internal WinVerifyTrustTools.RevocationCheckFlags fdwRevocationChecks;

			// Token: 0x04000138 RID: 312
			internal WinVerifyTrustTools.UnionChoice dwUnionChoice;

			// Token: 0x04000139 RID: 313
			internal IntPtr pInfoStruct;

			// Token: 0x0400013A RID: 314
			internal WinVerifyTrustTools.StateAction dwStateAction;

			// Token: 0x0400013B RID: 315
			internal IntPtr hWVTStateData;

			// Token: 0x0400013C RID: 316
			internal IntPtr pwszURLReference;

			// Token: 0x0400013D RID: 317
			internal WinVerifyTrustTools.TrustProviderFlags dwProvFlags;

			// Token: 0x0400013E RID: 318
			internal WinVerifyTrustTools.UIContext dwUIContext;

			// Token: 0x0400013F RID: 319
			internal IntPtr pSignatureSettings;
		}

		// Token: 0x0200006C RID: 108
		internal struct CRYPT_PROVIDER_DATA
		{
			// Token: 0x04000140 RID: 320
			internal uint cbStruct;

			// Token: 0x04000141 RID: 321
			internal IntPtr pWintrustData;

			// Token: 0x04000142 RID: 322
			internal bool fOpenedFile;

			// Token: 0x04000143 RID: 323
			internal IntPtr hWndParent;

			// Token: 0x04000144 RID: 324
			internal IntPtr pgActionID;

			// Token: 0x04000145 RID: 325
			internal IntPtr hProv;

			// Token: 0x04000146 RID: 326
			internal uint dwError;

			// Token: 0x04000147 RID: 327
			internal uint dwRegSecuritySettings;

			// Token: 0x04000148 RID: 328
			internal uint dwRegPolicySettings;

			// Token: 0x04000149 RID: 329
			internal IntPtr psPfns;

			// Token: 0x0400014A RID: 330
			internal uint cdwTrustStepErrors;

			// Token: 0x0400014B RID: 331
			internal IntPtr padwTrustStepErrors;

			// Token: 0x0400014C RID: 332
			internal uint chStores;

			// Token: 0x0400014D RID: 333
			internal IntPtr pahStores;

			// Token: 0x0400014E RID: 334
			internal uint dwEncoding;

			// Token: 0x0400014F RID: 335
			internal IntPtr hMsg;

			// Token: 0x04000150 RID: 336
			internal uint csSigners;

			// Token: 0x04000151 RID: 337
			internal IntPtr pasSigners;

			// Token: 0x04000152 RID: 338
			internal uint csProvPrivData;

			// Token: 0x04000153 RID: 339
			internal IntPtr pasProvPrivData;

			// Token: 0x04000154 RID: 340
			internal uint dwSubjectChoice;

			// Token: 0x04000155 RID: 341
			internal IntPtr pPDSip;

			// Token: 0x04000156 RID: 342
			internal IntPtr pszUsageOID;

			// Token: 0x04000157 RID: 343
			internal bool fRecallWithState;

			// Token: 0x04000158 RID: 344
			internal System.Runtime.InteropServices.ComTypes.FILETIME sftSystemTime;

			// Token: 0x04000159 RID: 345
			internal IntPtr pszCTLSignerUsageOID;

			// Token: 0x0400015A RID: 346
			internal uint dwProvFlags;

			// Token: 0x0400015B RID: 347
			internal uint dwFinalError;

			// Token: 0x0400015C RID: 348
			internal IntPtr pRequestUsage;

			// Token: 0x0400015D RID: 349
			internal uint dwTrustPubSettings;

			// Token: 0x0400015E RID: 350
			internal uint dwUIStateFlags;
		}

		// Token: 0x0200006D RID: 109
		internal struct CRYPT_PROVIDER_SGNR
		{
			// Token: 0x0400015F RID: 351
			internal uint cbStruct;

			// Token: 0x04000160 RID: 352
			internal System.Runtime.InteropServices.ComTypes.FILETIME sftVerifyAsOf;

			// Token: 0x04000161 RID: 353
			internal uint csCertChain;

			// Token: 0x04000162 RID: 354
			internal IntPtr pasCertChain;

			// Token: 0x04000163 RID: 355
			internal uint dwSignerType;

			// Token: 0x04000164 RID: 356
			internal IntPtr psSigner;

			// Token: 0x04000165 RID: 357
			internal uint dwError;

			// Token: 0x04000166 RID: 358
			internal uint csCounterSigners;

			// Token: 0x04000167 RID: 359
			internal IntPtr pasCounterSigners;

			// Token: 0x04000168 RID: 360
			internal IntPtr pChainContext;
		}

		// Token: 0x0200006E RID: 110
		internal struct CRYPT_INTEGER_BLOB
		{
			// Token: 0x04000169 RID: 361
			internal int cbData;

			// Token: 0x0400016A RID: 362
			internal IntPtr pbData;
		}

		// Token: 0x0200006F RID: 111
		internal struct CRYPT_ALGORITHM_IDENTIFIER
		{
			// Token: 0x0400016B RID: 363
			internal IntPtr pszObjId;

			// Token: 0x0400016C RID: 364
			internal WinVerifyTrustTools.CRYPT_INTEGER_BLOB Parameters;
		}

		// Token: 0x02000070 RID: 112
		internal struct CRYPT_ATTRIBUTES
		{
			// Token: 0x0400016D RID: 365
			internal int cAttr;

			// Token: 0x0400016E RID: 366
			internal IntPtr rgAttr;
		}

		// Token: 0x02000071 RID: 113
		internal struct CMSG_SIGNER_INFO
		{
			// Token: 0x0400016F RID: 367
			internal int dwVersion;

			// Token: 0x04000170 RID: 368
			internal WinVerifyTrustTools.CRYPT_INTEGER_BLOB Issuer;

			// Token: 0x04000171 RID: 369
			internal WinVerifyTrustTools.CRYPT_INTEGER_BLOB SerialNumber;

			// Token: 0x04000172 RID: 370
			internal WinVerifyTrustTools.CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;

			// Token: 0x04000173 RID: 371
			internal WinVerifyTrustTools.CRYPT_ALGORITHM_IDENTIFIER HashEncryptionAlgorithm;

			// Token: 0x04000174 RID: 372
			internal WinVerifyTrustTools.CRYPT_INTEGER_BLOB EncryptedHash;

			// Token: 0x04000175 RID: 373
			internal WinVerifyTrustTools.CRYPT_ATTRIBUTES AuthAttrs;

			// Token: 0x04000176 RID: 374
			internal WinVerifyTrustTools.CRYPT_ATTRIBUTES UnauthAttrs;
		}

		// Token: 0x02000072 RID: 114
		internal struct CRYPT_PROVIDER_CERT
		{
			// Token: 0x04000177 RID: 375
			internal uint cbStruct;

			// Token: 0x04000178 RID: 376
			internal IntPtr pCert;

			// Token: 0x04000179 RID: 377
			internal bool fCommercial;

			// Token: 0x0400017A RID: 378
			internal bool fTrustedRoot;

			// Token: 0x0400017B RID: 379
			internal bool fSelfSigned;

			// Token: 0x0400017C RID: 380
			internal bool fTestCert;

			// Token: 0x0400017D RID: 381
			internal uint dwRevokedReason;

			// Token: 0x0400017E RID: 382
			internal uint dwConfidence;

			// Token: 0x0400017F RID: 383
			internal uint dwError;

			// Token: 0x04000180 RID: 384
			internal IntPtr pTrustListContext;

			// Token: 0x04000181 RID: 385
			internal bool fTrustListSignerCert;

			// Token: 0x04000182 RID: 386
			internal IntPtr pCtlContext;

			// Token: 0x04000183 RID: 387
			internal uint dwCtlError;

			// Token: 0x04000184 RID: 388
			internal bool fIsCyclic;

			// Token: 0x04000185 RID: 389
			internal IntPtr pChainElement;
		}

		// Token: 0x02000073 RID: 115
		internal sealed class UnmanagedPointer : IDisposable
		{
			// Token: 0x060001E3 RID: 483 RVA: 0x0000998A File Offset: 0x00007B8A
			internal UnmanagedPointer(IntPtr ptr, WinVerifyTrustTools.AllocMethod method)
			{
				this.m_meth = method;
				this.m_ptr = ptr;
			}

			// Token: 0x060001E4 RID: 484 RVA: 0x000099A0 File Offset: 0x00007BA0
			~UnmanagedPointer()
			{
				this.Dispose(false);
			}

			// Token: 0x060001E5 RID: 485 RVA: 0x000099D0 File Offset: 0x00007BD0
			private void Dispose(bool disposing)
			{
				if (this.m_ptr != IntPtr.Zero)
				{
					if (this.m_meth == WinVerifyTrustTools.AllocMethod.HGlobal)
					{
						Marshal.FreeHGlobal(this.m_ptr);
					}
					else if (this.m_meth == WinVerifyTrustTools.AllocMethod.CoTaskMem)
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

			// Token: 0x060001E6 RID: 486 RVA: 0x00009A2C File Offset: 0x00007C2C
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x060001E7 RID: 487 RVA: 0x00009A3B File Offset: 0x00007C3B
			public static implicit operator IntPtr(WinVerifyTrustTools.UnmanagedPointer ptr)
			{
				return ptr.m_ptr;
			}

			// Token: 0x04000186 RID: 390
			private IntPtr m_ptr;

			// Token: 0x04000187 RID: 391
			private WinVerifyTrustTools.AllocMethod m_meth;
		}
	}
}
