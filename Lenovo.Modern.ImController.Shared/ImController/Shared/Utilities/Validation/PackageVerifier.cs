using System;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Validation;

namespace Lenovo.Modern.ImController.Shared.Utilities.Validation
{
	// Token: 0x0200003B RID: 59
	public class PackageVerifier : IPackageVerifier
	{
		// Token: 0x0600019F RID: 415 RVA: 0x000083DB File Offset: 0x000065DB
		public PackageVerifier()
		{
			this.validator = new ImcCertificateValidator();
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000083EE File Offset: 0x000065EE
		public bool IsPackageValid(string packagePath)
		{
			if (Constants.IsSecurityDisabled)
			{
				Logger.Log(Logger.LogSeverity.Critical, "Warning: Security has been disabled for validating a package");
				return true;
			}
			return this.validator.AssertDigitalSignatureIsValid(packagePath);
		}

		// Token: 0x040000E4 RID: 228
		private IImcCertificateValidator validator;
	}
}
