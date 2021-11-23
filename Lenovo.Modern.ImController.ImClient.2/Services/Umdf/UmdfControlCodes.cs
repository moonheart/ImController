using System;

namespace Lenovo.Modern.ImController.ImClient.Services.Umdf
{
	// Token: 0x0200001B RID: 27
	public static class UmdfControlCodes
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00003A86 File Offset: 0x00001C86
		private static uint CreateCtlCode(uint deviceType, uint function, uint access, uint method)
		{
			return (deviceType << 16) | (access << 14) | (function << 2) | method;
		}

		// Token: 0x04000047 RID: 71
		public static readonly uint WaitForNextRequest = UmdfControlCodes.CreateCtlCode(53078U, (uint)ImDriverConstants.FunctionIds.WaitForNextRequest, 3U, 0U);

		// Token: 0x04000048 RID: 72
		public static readonly uint PutBrokerRequest = UmdfControlCodes.CreateCtlCode(53078U, (uint)ImDriverConstants.FunctionIds.PutBrokerRequest, 3U, 0U);

		// Token: 0x04000049 RID: 73
		public static readonly uint PutBrokerResponse = UmdfControlCodes.CreateCtlCode(53078U, (uint)ImDriverConstants.FunctionIds.PutBrokerResponse, 3U, 0U);

		// Token: 0x0400004A RID: 74
		public static readonly uint WaitForResponseV2 = UmdfControlCodes.CreateCtlCode(53078U, (uint)ImDriverConstants.FunctionIds.WaitForResponseV2, 3U, 0U);

		// Token: 0x0400004B RID: 75
		public static readonly uint CloseTask = UmdfControlCodes.CreateCtlCode(53078U, (uint)ImDriverConstants.FunctionIds.CloseTask, 3U, 0U);

		// Token: 0x0200005F RID: 95
		private static class TransferTypes
		{
			// Token: 0x0400014C RID: 332
			public const uint METHOD_BUFFERED = 0U;

			// Token: 0x0400014D RID: 333
			public const uint METHOD_IN_DIRECT = 1U;

			// Token: 0x0400014E RID: 334
			public const uint METHOD_OUT_DIRECT = 2U;

			// Token: 0x0400014F RID: 335
			public const uint METHOD_NEITHER = 3U;
		}

		// Token: 0x02000060 RID: 96
		private static class AccessTypes
		{
			// Token: 0x04000150 RID: 336
			public const uint FILE_ANY_ACCESS = 0U;

			// Token: 0x04000151 RID: 337
			public const uint FILE_SPECIAL_ACCESS = 0U;

			// Token: 0x04000152 RID: 338
			public const uint FILE_READ_ACCESS = 1U;

			// Token: 0x04000153 RID: 339
			public const uint FILE_WRITE_ACCESS = 2U;
		}
	}
}
