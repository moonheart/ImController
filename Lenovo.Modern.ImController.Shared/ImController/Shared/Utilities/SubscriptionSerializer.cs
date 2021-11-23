using System;
using System.Reflection;
using Lenovo.Modern.ImController.Shared.Model.Subscription;

namespace Lenovo.Modern.ImController.Shared.Utilities
{
	// Token: 0x02000032 RID: 50
	public static class SubscriptionSerializer
	{
		// Token: 0x06000186 RID: 390 RVA: 0x00007D38 File Offset: 0x00005F38
		public static PackageSubscription Deserialize(string str)
		{
			return null;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00007D3C File Offset: 0x00005F3C
		public static string Serialize(PackageSubscription obj)
		{
			string text = "";
			foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
			{
				text += propertyInfo.Name;
			}
			return text;
		}
	}
}
