using System;

namespace Lenovo.Modern.Utilities.Services.SystemContext.Shared
{
	// Token: 0x0200002C RID: 44
	public class UserInformation
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00005EC3 File Offset: 0x000040C3
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00005ECB File Offset: 0x000040CB
		public string SID { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00005ED4 File Offset: 0x000040D4
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00005EDC File Offset: 0x000040DC
		public string UserProfileFolder { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00005EE5 File Offset: 0x000040E5
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00005EED File Offset: 0x000040ED
		public string UserName { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00005EF6 File Offset: 0x000040F6
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00005EFE File Offset: 0x000040FE
		public string HostName { get; set; }
	}
}
