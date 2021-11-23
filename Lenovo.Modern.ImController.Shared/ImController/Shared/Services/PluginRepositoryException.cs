using System;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x02000022 RID: 34
	public class PluginRepositoryException : Exception
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00006CD4 File Offset: 0x00004ED4
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00006CDC File Offset: 0x00004EDC
		public int ResponseCode { get; set; }

		// Token: 0x060000CC RID: 204 RVA: 0x00005A79 File Offset: 0x00003C79
		public PluginRepositoryException(string message)
			: base(message)
		{
		}
	}
}
