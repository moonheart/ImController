using System;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x02000030 RID: 48
	public sealed class PluginRequestInformation
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00004D61 File Offset: 0x00002F61
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00004D69 File Offset: 0x00002F69
		public string PluginName { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00004D72 File Offset: 0x00002F72
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00004D7A File Offset: 0x00002F7A
		public string PluginLocation { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00004D83 File Offset: 0x00002F83
		// (set) Token: 0x0600012D RID: 301 RVA: 0x00004D8B File Offset: 0x00002F8B
		public string TaskId { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00004D94 File Offset: 0x00002F94
		// (set) Token: 0x0600012F RID: 303 RVA: 0x00004D9C File Offset: 0x00002F9C
		public PluginType PluginType { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00004DA5 File Offset: 0x00002FA5
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00004DAD File Offset: 0x00002FAD
		public RequestType RequestType { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00004DB6 File Offset: 0x00002FB6
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00004DBE File Offset: 0x00002FBE
		public Bitness Bitness { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00004DC7 File Offset: 0x00002FC7
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00004DCF File Offset: 0x00002FCF
		public RunAs RunAs { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00004DD8 File Offset: 0x00002FD8
		// (set) Token: 0x06000137 RID: 311 RVA: 0x00004DE0 File Offset: 0x00002FE0
		public ContractRequest ContractRequest { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00004DE9 File Offset: 0x00002FE9
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00004DF1 File Offset: 0x00002FF1
		public EventReaction EventReaction { get; set; }
	}
}
