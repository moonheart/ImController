using System;

namespace Lenovo.Modern.ImController.Shared.Utilities
{
	// Token: 0x02000033 RID: 51
	public static class CacheBuster
	{
		// Token: 0x06000188 RID: 392 RVA: 0x00007D7C File Offset: 0x00005F7C
		public static Uri MakeUnique(Uri location)
		{
			Uri result = location;
			if (location != null && !string.IsNullOrWhiteSpace(location.OriginalString))
			{
				result = new Uri(CacheBuster.AppendCacheBustToEnd(location.OriginalString));
			}
			return result;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00007DB4 File Offset: 0x00005FB4
		private static string AppendCacheBustToEnd(string address)
		{
			string arg = string.Format("unique={0}{1}{2}{3}{4}", new object[]
			{
				DateTime.Now.DayOfYear,
				DateTime.Now.Hour,
				DateTime.Now.Minute,
				DateTime.Now.Second,
				DateTime.Now.Millisecond
			});
			string result = address;
			if (!string.IsNullOrWhiteSpace(address))
			{
				if (address.Contains("?"))
				{
					address += string.Format("{0}{1}", "&", arg);
				}
				else
				{
					address += string.Format("{0}{1}", "?", arg);
				}
				result = address;
			}
			return result;
		}
	}
}
