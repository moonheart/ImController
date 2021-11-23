using System;

namespace Lenovo.Modern.ImController.ContractBroker.Services
{
	// Token: 0x02000012 RID: 18
	public class RequestMapperException : Exception
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00003C88 File Offset: 0x00001E88
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003C90 File Offset: 0x00001E90
		public int ResponseCode { get; set; }

		// Token: 0x060000A3 RID: 163 RVA: 0x00003C99 File Offset: 0x00001E99
		public RequestMapperException(string message)
			: base(message)
		{
		}
	}
}
