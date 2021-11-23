using System;

namespace Lenovo.CertificateValidation
{
	// Token: 0x02000005 RID: 5
	public enum TrustStatus
	{
		// Token: 0x0400000A RID: 10
		FileTrusted,
		// Token: 0x0400000B RID: 11
		FileNotTrusted,
		// Token: 0x0400000C RID: 12
		FileNotSigned,
		// Token: 0x0400000D RID: 13
		FileNotFound,
		// Token: 0x0400000E RID: 14
		FileSignedMultipleTimes,
		// Token: 0x0400000F RID: 15
		InvalidXmlFile,
		// Token: 0x04000010 RID: 16
		InvalidPublicKey,
		// Token: 0x04000011 RID: 17
		ExceptionThrown,
		// Token: 0x04000012 RID: 18
		FileNameNullOrEmpty,
		// Token: 0x04000013 RID: 19
		NotSignedWithTrustedCertificate,
		// Token: 0x04000014 RID: 20
		UntrustedFile,
		// Token: 0x04000015 RID: 21
		InValidFilenamePassed,
		// Token: 0x04000016 RID: 22
		NotLenovoCertificate,
		// Token: 0x04000017 RID: 23
		IssuingCertificateNotValid,
		// Token: 0x04000018 RID: 24
		CertificateIssuedTooEarly,
		// Token: 0x04000019 RID: 25
		WinVerifyTrustFailed,
		// Token: 0x0400001A RID: 26
		XAdesFailed,
		// Token: 0x0400001B RID: 27
		ChainOfTrustFailed,
		// Token: 0x0400001C RID: 28
		SignatureCheckFailed,
		// Token: 0x0400001D RID: 29
		FileTamperedWith,
		// Token: 0x0400001E RID: 30
		NotCountersignedProperly,
		// Token: 0x0400001F RID: 31
		SystemOffline,
		// Token: 0x04000020 RID: 32
		InvalidCertificate,
		// Token: 0x04000021 RID: 33
		RootCertificateNotValid
	}
}
