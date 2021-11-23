using System;
using System.Runtime.Serialization;

namespace Lenovo.Modern.ImController.PluginManager.Services
{
	// Token: 0x02000007 RID: 7
	public class PluginManagerException : Exception
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00003A4D File Offset: 0x00001C4D
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00003A55 File Offset: 0x00001C55
		public string TaskId { get; set; }

		// Token: 0x06000032 RID: 50 RVA: 0x00003A5E File Offset: 0x00001C5E
		public PluginManagerException()
		{
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003A44 File Offset: 0x00001C44
		public PluginManagerException(string message)
			: base(message)
		{
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003A66 File Offset: 0x00001C66
		public PluginManagerException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003A70 File Offset: 0x00001C70
		protected PluginManagerException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
