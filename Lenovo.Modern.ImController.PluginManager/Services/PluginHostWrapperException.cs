using System;

namespace Lenovo.Modern.ImController.PluginManager.Services
{
	// Token: 0x02000006 RID: 6
	public class PluginHostWrapperException : Exception
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00003A33 File Offset: 0x00001C33
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00003A3B File Offset: 0x00001C3B
		public int ResponseCode { get; set; }

		// Token: 0x0600002F RID: 47 RVA: 0x00003A44 File Offset: 0x00001C44
		public PluginHostWrapperException(string message)
			: base(message)
		{
		}
	}
}
