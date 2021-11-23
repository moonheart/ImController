using System;
using System.Runtime.Serialization;

namespace Lenovo.Modern.Utilities.Services.Wrappers.Storage
{
	// Token: 0x02000007 RID: 7
	public class Exceptions
	{
		// Token: 0x02000042 RID: 66
		[Serializable]
		public class FileAlreadyExistsException : Exception
		{
			// Token: 0x06000199 RID: 409 RVA: 0x00007C9C File Offset: 0x00005E9C
			public FileAlreadyExistsException(string message)
				: base(message)
			{
			}

			// Token: 0x0600019A RID: 410 RVA: 0x00007CA5 File Offset: 0x00005EA5
			protected FileAlreadyExistsException(SerializationInfo info, StreamingContext context)
				: base(info, context)
			{
			}
		}

		// Token: 0x02000043 RID: 67
		[Serializable]
		public class DirectoryAlreadyExistsException : Exception
		{
			// Token: 0x0600019B RID: 411 RVA: 0x00007C9C File Offset: 0x00005E9C
			public DirectoryAlreadyExistsException(string message)
				: base(message)
			{
			}

			// Token: 0x0600019C RID: 412 RVA: 0x00007CA5 File Offset: 0x00005EA5
			protected DirectoryAlreadyExistsException(SerializationInfo info, StreamingContext context)
				: base(info, context)
			{
			}
		}
	}
}
