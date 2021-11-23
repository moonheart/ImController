using System;

namespace Lenovo.Modern.ImController.EventManager.Services
{
	// Token: 0x02000008 RID: 8
	public class EventManagerException : Exception
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000026FC File Offset: 0x000008FC
		// (set) Token: 0x06000013 RID: 19 RVA: 0x00002704 File Offset: 0x00000904
		public int ResponseCode { get; set; }

		// Token: 0x06000014 RID: 20 RVA: 0x0000270D File Offset: 0x0000090D
		public EventManagerException(string message)
			: base(message)
		{
		}
	}
}
