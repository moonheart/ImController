using System;
using System.Collections.Generic;

namespace Lenovo.CertificateValidation
{
	// Token: 0x02000008 RID: 8
	internal class DebugInfo
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00003ACA File Offset: 0x00001CCA
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00003AD1 File Offset: 0x00001CD1
		internal static bool Enabled { get; set; }

		// Token: 0x06000033 RID: 51 RVA: 0x000037DF File Offset: 0x000019DF
		internal static void Output(List<KeyValuePair<string, string>> items, bool append = true)
		{
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000037DF File Offset: 0x000019DF
		internal static void Output(string line, bool append = true)
		{
		}
	}
}
