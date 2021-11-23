using System;
using System.Reflection;

namespace Lenovo.ImController.EventLogging
{
	// Token: 0x02000003 RID: 3
	public static class PublicConstants
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static string SdkVersion
		{
			get
			{
				if (PublicConstants._sdkVersion == null)
				{
					PublicConstants._sdkVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
				}
				return PublicConstants._sdkVersion ?? "0.0.0.0";
			}
		}

		// Token: 0x04000004 RID: 4
		private static string _sdkVersion;
	}
}
