using System;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x0200002A RID: 42
	public static class ErrorCodes
	{
		// Token: 0x02000068 RID: 104
		public enum BrokerResponseAgentError
		{
			// Token: 0x04000197 RID: 407
			EmptyBrokerRequest = 305,
			// Token: 0x04000198 RID: 408
			InvalidBrokerResponse
		}

		// Token: 0x02000069 RID: 105
		public enum BrokerRequestAgentError
		{
			// Token: 0x0400019A RID: 410
			InvalidTaskId = 407,
			// Token: 0x0400019B RID: 411
			InvalidBrokerResponse,
			// Token: 0x0400019C RID: 412
			InvalidBrokerResponseData
		}

		// Token: 0x0200006A RID: 106
		public enum RequestMapperError
		{
			// Token: 0x0400019E RID: 414
			InvalidSubscriptionFile = 510,
			// Token: 0x0400019F RID: 415
			InvalidMachineInformation,
			// Token: 0x040001A0 RID: 416
			MatchingPackageNotFound,
			// Token: 0x040001A1 RID: 417
			MatchingApplciablePackageNotFound,
			// Token: 0x040001A2 RID: 418
			MatchingMappingsNotFound
		}

		// Token: 0x0200006B RID: 107
		public enum AppProvisioningError
		{
			// Token: 0x040001A4 RID: 420
			UDCNotInstalled = 1061,
			// Token: 0x040001A5 RID: 421
			UDCNotRunning,
			// Token: 0x040001A6 RID: 422
			InvalidDeviceId,
			// Token: 0x040001A7 RID: 423
			DeviceNotConfigured,
			// Token: 0x040001A8 RID: 424
			DeviceNotRegistered,
			// Token: 0x040001A9 RID: 425
			APSNotAvailable,
			// Token: 0x040001AA RID: 426
			NoInternetConnection,
			// Token: 0x040001AB RID: 427
			TPMDisable
		}
	}
}
