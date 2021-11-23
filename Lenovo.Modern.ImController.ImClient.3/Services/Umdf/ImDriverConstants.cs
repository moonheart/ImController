using System;

namespace Lenovo.Modern.ImController.ImClient.Services.Umdf
{
	// Token: 0x02000019 RID: 25
	internal static class ImDriverConstants
	{
		// Token: 0x04000044 RID: 68
		public const ushort DeviceType = 53078;

		// Token: 0x04000045 RID: 69
		public static readonly Guid DeviceInterfaceGuid = new Guid("8466bd23-0fd3-4ffd-8ecb-2f3dbe34262b");

		// Token: 0x0200005E RID: 94
		public static class FunctionIds
		{
			// Token: 0x04000146 RID: 326
			public static readonly ushort WaitForNextRequest = 2049;

			// Token: 0x04000147 RID: 327
			public static readonly ushort PutBrokerRequest = 2050;

			// Token: 0x04000148 RID: 328
			public static readonly ushort PutBrokerResponse = 2051;

			// Token: 0x04000149 RID: 329
			public static readonly ushort WaitForResponse = 2052;

			// Token: 0x0400014A RID: 330
			public static readonly ushort WaitForResponseV2 = 2068;

			// Token: 0x0400014B RID: 331
			public static readonly ushort CloseTask = 2053;
		}
	}
}
