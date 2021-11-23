using System;
using System.Runtime.Serialization;

namespace Lenovo.Modern.ImController.PluginHost.AppDomain
{
	// Token: 0x02000002 RID: 2
	[Serializable]
	public class AppDomainException : Exception
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public AppDomainException()
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000250
		public AppDomainException(string message)
			: base(message)
		{
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002059 File Offset: 0x00000259
		public AppDomainException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002063 File Offset: 0x00000263
		protected AppDomainException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
