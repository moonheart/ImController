using System;
using System.Collections.Generic;

namespace Lenovo.Modern.Utilities.Services.Validation.Tvt
{
	// Token: 0x02000023 RID: 35
	internal class DebugInfo
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00004A10 File Offset: 0x00002C10
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00004A17 File Offset: 0x00002C17
		internal static bool Enabled { get; set; }

		// Token: 0x060000C3 RID: 195 RVA: 0x00004A1F File Offset: 0x00002C1F
		internal static void Output(List<KeyValuePair<string, string>> items, bool append = true)
		{
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004A1F File Offset: 0x00002C1F
		internal static void Output(string line, bool append = true)
		{
		}
	}
}
