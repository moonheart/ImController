using System;
using System.Runtime.Serialization;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x0200001D RID: 29
	public class SubscriptionManagerException : Exception
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00005A60 File Offset: 0x00003C60
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00005A68 File Offset: 0x00003C68
		public string TaskId { get; set; }

		// Token: 0x060000A0 RID: 160 RVA: 0x00005A71 File Offset: 0x00003C71
		public SubscriptionManagerException()
		{
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005A79 File Offset: 0x00003C79
		public SubscriptionManagerException(string message)
			: base(message)
		{
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005A82 File Offset: 0x00003C82
		public SubscriptionManagerException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005A8C File Offset: 0x00003C8C
		protected SubscriptionManagerException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
