using System;
using System.IO;

namespace Lenovo.Modern.ImController.ImClient.Utilities
{
	// Token: 0x02000003 RID: 3
	public class ExternalSignatureValidator
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002184 File Offset: 0x00000384
		public static void Configure(Func<FileInfo, bool> verifyBinaryFile, Func<FileInfo, bool> verifyXmlFile)
		{
			if (ExternalSignatureValidator.IsConfigured())
			{
				throw new InvalidOperationException("Has already been configured");
			}
			ExternalSignatureValidator._configuredInstance = new ExternalSignatureValidator(verifyBinaryFile, verifyXmlFile);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021A4 File Offset: 0x000003A4
		public static bool IsConfigured()
		{
			return ExternalSignatureValidator._configuredInstance != null;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021AE File Offset: 0x000003AE
		private ExternalSignatureValidator(Func<FileInfo, bool> verifyBinaryFile, Func<FileInfo, bool> verifyXmlFile)
		{
			if (verifyBinaryFile == null || verifyXmlFile == null)
			{
				throw new ArgumentNullException("Must provide functions to validate files");
			}
			this._verifyBinaryFile = verifyBinaryFile;
			this._verifyXmlFile = verifyXmlFile;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021D5 File Offset: 0x000003D5
		private ExternalSignatureValidator()
		{
			this._verifyBinaryFile = null;
			this._verifyXmlFile = null;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000021EB File Offset: 0x000003EB
		internal static ExternalSignatureValidator Instance
		{
			get
			{
				if (ExternalSignatureValidator._configuredInstance != null)
				{
					return ExternalSignatureValidator._configuredInstance;
				}
				ExternalSignatureValidator result;
				if ((result = ExternalSignatureValidator._notConfiguredInstance) == null)
				{
					result = (ExternalSignatureValidator._notConfiguredInstance = new ExternalSignatureValidator());
				}
				return result;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000220E File Offset: 0x0000040E
		internal bool Validate(string pathtoFile)
		{
			return this.Validate(new FileInfo(pathtoFile));
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000221C File Offset: 0x0000041C
		internal bool Validate(FileInfo file)
		{
			if (this._verifyBinaryFile == null || this._verifyXmlFile == null)
			{
				throw new InvalidOperationException("Must be setup with Configure() before using");
			}
			if (file == null || !file.Exists)
			{
				throw new ArgumentNullException("Provided file not valid");
			}
			if (!string.IsNullOrWhiteSpace(file.Extension) && file.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
			{
				return this._verifyXmlFile(file);
			}
			return this._verifyBinaryFile(file);
		}

		// Token: 0x04000004 RID: 4
		private readonly Func<FileInfo, bool> _verifyBinaryFile;

		// Token: 0x04000005 RID: 5
		private readonly Func<FileInfo, bool> _verifyXmlFile;

		// Token: 0x04000006 RID: 6
		private static ExternalSignatureValidator _configuredInstance;

		// Token: 0x04000007 RID: 7
		private static ExternalSignatureValidator _notConfiguredInstance;
	}
}
