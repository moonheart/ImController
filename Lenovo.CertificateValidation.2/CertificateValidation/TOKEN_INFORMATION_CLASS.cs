using System;

namespace Lenovo.CertificateValidation
{
	// Token: 0x0200000C RID: 12
	internal enum TOKEN_INFORMATION_CLASS
	{
		// Token: 0x0400003D RID: 61
		TokenUser = 1,
		// Token: 0x0400003E RID: 62
		TokenGroups,
		// Token: 0x0400003F RID: 63
		TokenPrivileges,
		// Token: 0x04000040 RID: 64
		TokenOwner,
		// Token: 0x04000041 RID: 65
		TokenPrimaryGroup,
		// Token: 0x04000042 RID: 66
		TokenDefaultDacl,
		// Token: 0x04000043 RID: 67
		TokenSource,
		// Token: 0x04000044 RID: 68
		TokenType,
		// Token: 0x04000045 RID: 69
		TokenImpersonationLevel,
		// Token: 0x04000046 RID: 70
		TokenStatistics,
		// Token: 0x04000047 RID: 71
		TokenRestrictedSids,
		// Token: 0x04000048 RID: 72
		TokenSessionId,
		// Token: 0x04000049 RID: 73
		TokenGroupsAndPrivileges,
		// Token: 0x0400004A RID: 74
		TokenSessionReference,
		// Token: 0x0400004B RID: 75
		TokenSandBoxInert,
		// Token: 0x0400004C RID: 76
		TokenAuditPolicy,
		// Token: 0x0400004D RID: 77
		TokenOrigin,
		// Token: 0x0400004E RID: 78
		TokenElevationType,
		// Token: 0x0400004F RID: 79
		TokenLinkedToken,
		// Token: 0x04000050 RID: 80
		TokenElevation,
		// Token: 0x04000051 RID: 81
		TokenHasRestrictions,
		// Token: 0x04000052 RID: 82
		TokenAccessInformation,
		// Token: 0x04000053 RID: 83
		TokenVirtualizationAllowed,
		// Token: 0x04000054 RID: 84
		TokenVirtualizationEnabled,
		// Token: 0x04000055 RID: 85
		TokenIntegrityLevel,
		// Token: 0x04000056 RID: 86
		TokenUIAccess,
		// Token: 0x04000057 RID: 87
		TokenMandatoryPolicy,
		// Token: 0x04000058 RID: 88
		TokenLogonSid,
		// Token: 0x04000059 RID: 89
		MaxTokenInfoClass
	}
}
