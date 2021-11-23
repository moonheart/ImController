using System;

namespace Lenovo.Modern.Utilities.Services
{
	// Token: 0x02000004 RID: 4
	public static class Serializer
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002108 File Offset: 0x00000308
		public static T Deserialize<T>(string xml)
		{
			return XmlStringSerializer.Deserialize<T>(xml);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002110 File Offset: 0x00000310
		public static string Serialize<T>(T instance)
		{
			return XmlStringSerializer.Serialize<T>(instance);
		}
	}
}
