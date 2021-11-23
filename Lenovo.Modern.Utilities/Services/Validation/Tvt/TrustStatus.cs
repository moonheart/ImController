using System;

namespace Lenovo.Modern.Utilities.Services.Validation.Tvt
{
	// Token: 0x02000020 RID: 32
	public enum TrustStatus
	{
		// Token: 0x04000027 RID: 39
		FileTrusted,
		// Token: 0x04000028 RID: 40
		FileNotTrusted,
		// Token: 0x04000029 RID: 41
		FileNotSigned,
		// Token: 0x0400002A RID: 42
		FileNotFound,
		// Token: 0x0400002B RID: 43
		FileSignedMultipleTimes,
		// Token: 0x0400002C RID: 44
		InvalidXmlFile,
		// Token: 0x0400002D RID: 45
		InvalidPublicKey,
		// Token: 0x0400002E RID: 46
		ExceptionThrown,
		// Token: 0x0400002F RID: 47
		FileNameNullOrEmpty,
		// Token: 0x04000030 RID: 48
		NotSignedWithTrustedCertificate,
		// Token: 0x04000031 RID: 49
		UntrustedFile,
		// Token: 0x04000032 RID: 50
		InValidFilenamePassed,
		// Token: 0x04000033 RID: 51
		NotLenovoCertificate,
		// Token: 0x04000034 RID: 52
		IssuingCertificateNotValid,
		// Token: 0x04000035 RID: 53
		CertificateIssuedTooEarly,
		// Token: 0x04000036 RID: 54
		WinVerifyTrustFailed,
		// Token: 0x04000037 RID: 55
		XAdesFailed,
		// Token: 0x04000038 RID: 56
		ChainOfTrustFailed,
		// Token: 0x04000039 RID: 57
		SignatureCheckFailed,
		// Token: 0x0400003A RID: 58
		FileTamperedWith,
		// Token: 0x0400003B RID: 59
		NotCountersignedProperly,
		// Token: 0x0400003C RID: 60
		SystemOffline
	}
}
