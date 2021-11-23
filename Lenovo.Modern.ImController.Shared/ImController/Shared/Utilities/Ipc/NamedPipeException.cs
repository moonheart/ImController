using System;
using System.Runtime.Serialization;

namespace Lenovo.Modern.ImController.Shared.Utilities.Ipc
{
	// Token: 0x0200003F RID: 63
	public class NamedPipeException : Exception
	{
		// Token: 0x060001C1 RID: 449 RVA: 0x00005A71 File Offset: 0x00003C71
		public NamedPipeException()
		{
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00005A79 File Offset: 0x00003C79
		public NamedPipeException(string message)
			: base(message)
		{
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00005A82 File Offset: 0x00003C82
		public NamedPipeException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00005A8C File Offset: 0x00003C8C
		protected NamedPipeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
