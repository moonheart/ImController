using System;

namespace Lenovo.Modern.ImController.ImClient.Utilities
{
	// Token: 0x02000005 RID: 5
	internal static class Serializer
	{
		// Token: 0x06000015 RID: 21 RVA: 0x0000234C File Offset: 0x0000054C
		public static T Deserialize<T>(string xml)
		{
			return XmlStringSerializer.Deserialize<T>(xml);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002354 File Offset: 0x00000554
		public static string Serialize<T>(T instance)
		{
			return XmlStringSerializer.Serialize<T>(instance);
		}
	}
}
