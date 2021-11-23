using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Lenovo.CertificateValidation
{
	// Token: 0x0200000E RID: 14
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Lenovo_CertificateValidation
	{
		// Token: 0x0600004D RID: 77 RVA: 0x000025BB File Offset: 0x000007BB
		internal Lenovo_CertificateValidation()
		{
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00004378 File Offset: 0x00002578
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Lenovo_CertificateValidation.resourceMan == null)
				{
					Lenovo_CertificateValidation.resourceMan = new ResourceManager("Lenovo.CertificateValidation.Lenovo.CertificateValidation", typeof(Lenovo_CertificateValidation).Assembly);
				}
				return Lenovo_CertificateValidation.resourceMan;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004F RID: 79 RVA: 0x000043A4 File Offset: 0x000025A4
		// (set) Token: 0x06000050 RID: 80 RVA: 0x000043AB File Offset: 0x000025AB
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Lenovo_CertificateValidation.resourceCulture;
			}
			set
			{
				Lenovo_CertificateValidation.resourceCulture = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000043B3 File Offset: 0x000025B3
		internal static byte[] trusted3rdparty
		{
			get
			{
				return (byte[])Lenovo_CertificateValidation.ResourceManager.GetObject("trusted3rdparty", Lenovo_CertificateValidation.resourceCulture);
			}
		}

		// Token: 0x0400005B RID: 91
		private static ResourceManager resourceMan;

		// Token: 0x0400005C RID: 92
		private static CultureInfo resourceCulture;
	}
}
