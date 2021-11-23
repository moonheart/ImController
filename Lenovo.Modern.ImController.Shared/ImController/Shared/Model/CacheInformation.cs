using System;

namespace Lenovo.Modern.ImController.Shared.Model
{
	// Token: 0x02000025 RID: 37
	public class CacheInformation
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00006EC8 File Offset: 0x000050C8
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00006ED0 File Offset: 0x000050D0
		public string Name { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00006ED9 File Offset: 0x000050D9
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00006EE1 File Offset: 0x000050E1
		public string DateLastModified { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00006EEA File Offset: 0x000050EA
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00006EF2 File Offset: 0x000050F2
		public string Location { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00006EFB File Offset: 0x000050FB
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00006F03 File Offset: 0x00005103
		public string Version { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00006F0C File Offset: 0x0000510C
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00006F14 File Offset: 0x00005114
		public int NumberOfInstallAttempts { get; set; }
	}
}
