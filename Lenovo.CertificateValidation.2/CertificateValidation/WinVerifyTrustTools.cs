using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Lenovo.CertificateValidation
{
	// Token: 0x02000010 RID: 16
	internal class WinVerifyTrustTools
	{
		// Token: 0x0600005E RID: 94
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("Wintrust.dll")]
		internal static extern uint WinVerifyTrust(IntPtr hWnd, IntPtr pgActionID, IntPtr pWinTrustData);

		// Token: 0x0600005F RID: 95
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("Wintrust.dll")]
		internal static extern IntPtr WTHelperProvDataFromStateData(IntPtr pStateData);

		// Token: 0x06000060 RID: 96
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("Wintrust.dll")]
		internal static extern IntPtr WTHelperGetProvSignerFromChain(IntPtr pProvData, uint idxSigner, bool fCounterSigner, uint idxCounterSigner);

		// Token: 0x06000061 RID: 97
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("Wintrust.dll")]
		internal static extern IntPtr WTHelperGetProvCertFromChain(IntPtr pProvData, uint idxSigner);

		// Token: 0x06000062 RID: 98 RVA: 0x00004880 File Offset: 0x00002A80
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

		// Token: 0x0200001F RID: 31
		internal struct WINTRUST_FILE_INFO : IDisposable
		{
			// Token: 0x060000A9 RID: 169 RVA: 0x00005CF4 File Offset: 0x00003EF4
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

			// Token: 0x060000AA RID: 170 RVA: 0x00005D6E File Offset: 0x00003F6E
			public void Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x060000AB RID: 171 RVA: 0x00005D77 File Offset: 0x00003F77
			private void Dispose(bool disposing)
			{
				if (this.pgKnownSubject != IntPtr.Zero)
				{
					Marshal.DestroyStructure(this.pgKnownSubject, typeof(Guid));
					Marshal.FreeHGlobal(this.pgKnownSubject);
				}
			}

			// Token: 0x0400007A RID: 122
			private readonly uint cbStruct;

			// Token: 0x0400007B RID: 123
			[MarshalAs(UnmanagedType.LPTStr)]
			private readonly string pcwszFilePath;

			// Token: 0x0400007C RID: 124
			private readonly IntPtr hFile;

			// Token: 0x0400007D RID: 125
			private readonly IntPtr pgKnownSubject;
		}

		// Token: 0x02000020 RID: 32
		internal enum SignatureSettingsFlags : uint
		{
			// Token: 0x0400007F RID: 127
			VerySpecific = 1U,
			// Token: 0x04000080 RID: 128
			GetSecondarySigCount
		}

		// Token: 0x02000021 RID: 33
		internal enum AllocMethod
		{
			// Token: 0x04000082 RID: 130
			HGlobal,
			// Token: 0x04000083 RID: 131
			CoTaskMem
		}

		// Token: 0x02000022 RID: 34
		internal enum UnionChoice
		{
			// Token: 0x04000085 RID: 133
			File = 1,
			// Token: 0x04000086 RID: 134
			Catalog,
			// Token: 0x04000087 RID: 135
			Blob,
			// Token: 0x04000088 RID: 136
			Signer,
			// Token: 0x04000089 RID: 137
			Cert
		}

		// Token: 0x02000023 RID: 35
		internal enum UiChoice
		{
			// Token: 0x0400008B RID: 139
			All = 1,
			// Token: 0x0400008C RID: 140
			NoUI,
			// Token: 0x0400008D RID: 141
			NoBad,
			// Token: 0x0400008E RID: 142
			NoGood
		}

		// Token: 0x02000024 RID: 36
		internal enum RevocationCheckFlags
		{
			// Token: 0x04000090 RID: 144
			None,
			// Token: 0x04000091 RID: 145
			WholeChain
		}

		// Token: 0x02000025 RID: 37
		internal enum StateAction
		{
			// Token: 0x04000093 RID: 147
			Ignore,
			// Token: 0x04000094 RID: 148
			Verify,
			// Token: 0x04000095 RID: 149
			Close,
			// Token: 0x04000096 RID: 150
			AutoCache,
			// Token: 0x04000097 RID: 151
			AutoCacheFlush
		}

		// Token: 0x02000026 RID: 38
		[Flags]
		internal enum TrustProviderFlags
		{
			// Token: 0x04000099 RID: 153
			UseIE4Trust = 1,
			// Token: 0x0400009A RID: 154
			NoIE4Chain = 2,
			// Token: 0x0400009B RID: 155
			NoPolicyUsage = 4,
			// Token: 0x0400009C RID: 156
			RevocationCheckNone = 16,
			// Token: 0x0400009D RID: 157
			RevocationCheckEndCert = 32,
			// Token: 0x0400009E RID: 158
			RevocationCheckChain = 64,
			// Token: 0x0400009F RID: 159
			RecovationCheckChainExcludeRoot = 128,
			// Token: 0x040000A0 RID: 160
			Safer = 256,
			// Token: 0x040000A1 RID: 161
			HashOnly = 512,
			// Token: 0x040000A2 RID: 162
			UseDefaultOSVerCheck = 1024,
			// Token: 0x040000A3 RID: 163
			LifetimeSigning = 2048,
			// Token: 0x040000A4 RID: 164
			CacheOnlyUrlRetrieval = 4096
		}

		// Token: 0x02000027 RID: 39
		internal enum UIContext
		{
			// Token: 0x040000A6 RID: 166
			Execute,
			// Token: 0x040000A7 RID: 167
			Install
		}

		// Token: 0x02000028 RID: 40
		internal struct WINTRUST_SIGNATURE_SETTINGS
		{
			// Token: 0x060000AC RID: 172 RVA: 0x00005DAB File Offset: 0x00003FAB
			public WINTRUST_SIGNATURE_SETTINGS(uint flags)
			{
				this.cbStruct = (uint)Marshal.SizeOf(typeof(WinVerifyTrustTools.WINTRUST_SIGNATURE_SETTINGS));
				this.dwIndex = 0U;
				this.dwFlags = flags;
				this.cSecondarySigs = 0U;
				this.dwVerifiedSigIndex = 0U;
				this.pCryptoPolicy = IntPtr.Zero;
			}

			// Token: 0x040000A8 RID: 168
			internal uint cbStruct;

			// Token: 0x040000A9 RID: 169
			internal uint dwIndex;

			// Token: 0x040000AA RID: 170
			internal uint dwFlags;

			// Token: 0x040000AB RID: 171
			internal uint cSecondarySigs;

			// Token: 0x040000AC RID: 172
			internal uint dwVerifiedSigIndex;

			// Token: 0x040000AD RID: 173
			internal IntPtr pCryptoPolicy;
		}

		// Token: 0x02000029 RID: 41
		internal struct WINTRUST_DATA : IDisposable
		{
			// Token: 0x060000AD RID: 173 RVA: 0x00005DEC File Offset: 0x00003FEC
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
				this.dwProvFlags = WinVerifyTrustTools.TrustProviderFlags.RevocationCheckChain | WinVerifyTrustTools.TrustProviderFlags.CacheOnlyUrlRetrieval;
				this.dwUIContext = WinVerifyTrustTools.UIContext.Execute;
				this.pSignatureSettings = pSignatureInfo;
			}

			// Token: 0x060000AE RID: 174 RVA: 0x00005E9B File Offset: 0x0000409B
			public void Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x060000AF RID: 175 RVA: 0x00005EA4 File Offset: 0x000040A4
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

			// Token: 0x040000AE RID: 174
			internal uint cbStruct;

			// Token: 0x040000AF RID: 175
			internal IntPtr pPolicyCallbackData;

			// Token: 0x040000B0 RID: 176
			internal IntPtr pSIPCallbackData;

			// Token: 0x040000B1 RID: 177
			internal WinVerifyTrustTools.UiChoice dwUIChoice;

			// Token: 0x040000B2 RID: 178
			internal WinVerifyTrustTools.RevocationCheckFlags fdwRevocationChecks;

			// Token: 0x040000B3 RID: 179
			internal WinVerifyTrustTools.UnionChoice dwUnionChoice;

			// Token: 0x040000B4 RID: 180
			internal IntPtr pInfoStruct;

			// Token: 0x040000B5 RID: 181
			internal WinVerifyTrustTools.StateAction dwStateAction;

			// Token: 0x040000B6 RID: 182
			internal IntPtr hWVTStateData;

			// Token: 0x040000B7 RID: 183
			internal IntPtr pwszURLReference;

			// Token: 0x040000B8 RID: 184
			internal WinVerifyTrustTools.TrustProviderFlags dwProvFlags;

			// Token: 0x040000B9 RID: 185
			internal WinVerifyTrustTools.UIContext dwUIContext;

			// Token: 0x040000BA RID: 186
			internal IntPtr pSignatureSettings;
		}

		// Token: 0x0200002A RID: 42
		internal struct CRYPT_PROVIDER_DATA
		{
			// Token: 0x040000BB RID: 187
			internal uint cbStruct;

			// Token: 0x040000BC RID: 188
			internal IntPtr pWintrustData;

			// Token: 0x040000BD RID: 189
			internal bool fOpenedFile;

			// Token: 0x040000BE RID: 190
			internal IntPtr hWndParent;

			// Token: 0x040000BF RID: 191
			internal IntPtr pgActionID;

			// Token: 0x040000C0 RID: 192
			internal IntPtr hProv;

			// Token: 0x040000C1 RID: 193
			internal uint dwError;

			// Token: 0x040000C2 RID: 194
			internal uint dwRegSecuritySettings;

			// Token: 0x040000C3 RID: 195
			internal uint dwRegPolicySettings;

			// Token: 0x040000C4 RID: 196
			internal IntPtr psPfns;

			// Token: 0x040000C5 RID: 197
			internal uint cdwTrustStepErrors;

			// Token: 0x040000C6 RID: 198
			internal IntPtr padwTrustStepErrors;

			// Token: 0x040000C7 RID: 199
			internal uint chStores;

			// Token: 0x040000C8 RID: 200
			internal IntPtr pahStores;

			// Token: 0x040000C9 RID: 201
			internal uint dwEncoding;

			// Token: 0x040000CA RID: 202
			internal IntPtr hMsg;

			// Token: 0x040000CB RID: 203
			internal uint csSigners;

			// Token: 0x040000CC RID: 204
			internal IntPtr pasSigners;

			// Token: 0x040000CD RID: 205
			internal uint csProvPrivData;

			// Token: 0x040000CE RID: 206
			internal IntPtr pasProvPrivData;

			// Token: 0x040000CF RID: 207
			internal uint dwSubjectChoice;

			// Token: 0x040000D0 RID: 208
			internal IntPtr pPDSip;

			// Token: 0x040000D1 RID: 209
			internal IntPtr pszUsageOID;

			// Token: 0x040000D2 RID: 210
			internal bool fRecallWithState;

			// Token: 0x040000D3 RID: 211
			internal System.Runtime.InteropServices.ComTypes.FILETIME sftSystemTime;

			// Token: 0x040000D4 RID: 212
			internal IntPtr pszCTLSignerUsageOID;

			// Token: 0x040000D5 RID: 213
			internal uint dwProvFlags;

			// Token: 0x040000D6 RID: 214
			internal uint dwFinalError;

			// Token: 0x040000D7 RID: 215
			internal IntPtr pRequestUsage;

			// Token: 0x040000D8 RID: 216
			internal uint dwTrustPubSettings;

			// Token: 0x040000D9 RID: 217
			internal uint dwUIStateFlags;
		}

		// Token: 0x0200002B RID: 43
		internal struct CRYPT_PROVIDER_SGNR
		{
			// Token: 0x040000DA RID: 218
			internal uint cbStruct;

			// Token: 0x040000DB RID: 219
			internal System.Runtime.InteropServices.ComTypes.FILETIME sftVerifyAsOf;

			// Token: 0x040000DC RID: 220
			internal uint csCertChain;

			// Token: 0x040000DD RID: 221
			internal IntPtr pasCertChain;

			// Token: 0x040000DE RID: 222
			internal uint dwSignerType;

			// Token: 0x040000DF RID: 223
			internal IntPtr psSigner;

			// Token: 0x040000E0 RID: 224
			internal uint dwError;

			// Token: 0x040000E1 RID: 225
			internal uint csCounterSigners;

			// Token: 0x040000E2 RID: 226
			internal IntPtr pasCounterSigners;

			// Token: 0x040000E3 RID: 227
			internal IntPtr pChainContext;
		}

		// Token: 0x0200002C RID: 44
		internal struct CRYPT_INTEGER_BLOB
		{
			// Token: 0x040000E4 RID: 228
			internal int cbData;

			// Token: 0x040000E5 RID: 229
			internal IntPtr pbData;
		}

		// Token: 0x0200002D RID: 45
		internal struct CRYPT_ALGORITHM_IDENTIFIER
		{
			// Token: 0x040000E6 RID: 230
			internal IntPtr pszObjId;

			// Token: 0x040000E7 RID: 231
			internal WinVerifyTrustTools.CRYPT_INTEGER_BLOB Parameters;
		}

		// Token: 0x0200002E RID: 46
		internal struct CRYPT_ATTRIBUTES
		{
			// Token: 0x040000E8 RID: 232
			internal int cAttr;

			// Token: 0x040000E9 RID: 233
			internal IntPtr rgAttr;
		}

		// Token: 0x0200002F RID: 47
		internal struct CMSG_SIGNER_INFO
		{
			// Token: 0x040000EA RID: 234
			internal int dwVersion;

			// Token: 0x040000EB RID: 235
			internal WinVerifyTrustTools.CRYPT_INTEGER_BLOB Issuer;

			// Token: 0x040000EC RID: 236
			internal WinVerifyTrustTools.CRYPT_INTEGER_BLOB SerialNumber;

			// Token: 0x040000ED RID: 237
			internal WinVerifyTrustTools.CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;

			// Token: 0x040000EE RID: 238
			internal WinVerifyTrustTools.CRYPT_ALGORITHM_IDENTIFIER HashEncryptionAlgorithm;

			// Token: 0x040000EF RID: 239
			internal WinVerifyTrustTools.CRYPT_INTEGER_BLOB EncryptedHash;

			// Token: 0x040000F0 RID: 240
			internal WinVerifyTrustTools.CRYPT_ATTRIBUTES AuthAttrs;

			// Token: 0x040000F1 RID: 241
			internal WinVerifyTrustTools.CRYPT_ATTRIBUTES UnauthAttrs;
		}

		// Token: 0x02000030 RID: 48
		internal struct CRYPT_PROVIDER_CERT
		{
			// Token: 0x040000F2 RID: 242
			internal uint cbStruct;

			// Token: 0x040000F3 RID: 243
			internal IntPtr pCert;

			// Token: 0x040000F4 RID: 244
			internal bool fCommercial;

			// Token: 0x040000F5 RID: 245
			internal bool fTrustedRoot;

			// Token: 0x040000F6 RID: 246
			internal bool fSelfSigned;

			// Token: 0x040000F7 RID: 247
			internal bool fTestCert;

			// Token: 0x040000F8 RID: 248
			internal uint dwRevokedReason;

			// Token: 0x040000F9 RID: 249
			internal uint dwConfidence;

			// Token: 0x040000FA RID: 250
			internal uint dwError;

			// Token: 0x040000FB RID: 251
			internal IntPtr pTrustListContext;

			// Token: 0x040000FC RID: 252
			internal bool fTrustListSignerCert;

			// Token: 0x040000FD RID: 253
			internal IntPtr pCtlContext;

			// Token: 0x040000FE RID: 254
			internal uint dwCtlError;

			// Token: 0x040000FF RID: 255
			internal bool fIsCyclic;

			// Token: 0x04000100 RID: 256
			internal IntPtr pChainElement;
		}

		// Token: 0x02000031 RID: 49
		internal sealed class UnmanagedPointer : IDisposable
		{
			// Token: 0x060000B0 RID: 176 RVA: 0x00005EFA File Offset: 0x000040FA
			internal UnmanagedPointer(IntPtr ptr, WinVerifyTrustTools.AllocMethod method)
			{
				this.m_meth = method;
				this.m_ptr = ptr;
			}

			// Token: 0x060000B1 RID: 177 RVA: 0x00005F10 File Offset: 0x00004110
			~UnmanagedPointer()
			{
				this.Dispose(false);
			}

			// Token: 0x060000B2 RID: 178 RVA: 0x00005F40 File Offset: 0x00004140
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

			// Token: 0x060000B3 RID: 179 RVA: 0x00005F9C File Offset: 0x0000419C
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x060000B4 RID: 180 RVA: 0x00005FAB File Offset: 0x000041AB
			public static implicit operator IntPtr(WinVerifyTrustTools.UnmanagedPointer ptr)
			{
				return ptr.m_ptr;
			}

			// Token: 0x04000101 RID: 257
			private IntPtr m_ptr;

			// Token: 0x04000102 RID: 258
			private readonly WinVerifyTrustTools.AllocMethod m_meth;
		}
	}
}
