using System;

namespace Lenovo.ImController.EventLogging.Utilities
{
	// Token: 0x02000005 RID: 5
	public static class Serializer
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002080 File Offset: 0x00000280
		public static T Deserialize<T>(string xml)
		{
			return DataContractStringSerializer.Deserialize<T>(xml);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002088 File Offset: 0x00000288
		public static string Serialize<T>(T instance)
		{
			return DataContractStringSerializer.Serialize<T>(instance);
		}
	}
}
