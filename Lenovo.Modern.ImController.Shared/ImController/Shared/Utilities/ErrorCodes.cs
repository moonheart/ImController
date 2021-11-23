using System;

namespace Lenovo.Modern.ImController.Shared.Utilities
{
	// Token: 0x02000034 RID: 52
	public static class ErrorCodes
	{
		// Token: 0x02000083 RID: 131
		public enum PluginRepositroyError
		{
			// Token: 0x04000216 RID: 534
			PluginNotFound = 202,
			// Token: 0x04000217 RID: 535
			CannotInstallPlugin,
			// Token: 0x04000218 RID: 536
			CannotUninstallPlugin = 203,
			// Token: 0x04000219 RID: 537
			CannotInstallPackage,
			// Token: 0x0400021A RID: 538
			PluginDirectoryNotFound,
			// Token: 0x0400021B RID: 539
			PluginNotSignedByLenovo,
			// Token: 0x0400021C RID: 540
			PluginEntryNotFound,
			// Token: 0x0400021D RID: 541
			ManifestNotFound,
			// Token: 0x0400021E RID: 542
			ManifestNotSignedByLenovo,
			// Token: 0x0400021F RID: 543
			InValidManifest,
			// Token: 0x04000220 RID: 544
			ProblemInDownLoad,
			// Token: 0x04000221 RID: 545
			PlatformfolderNotFound
		}

		// Token: 0x02000084 RID: 132
		public enum BrokerResponseAgentError
		{
			// Token: 0x04000223 RID: 547
			EmptyBrokerRequest = 305,
			// Token: 0x04000224 RID: 548
			InvalidBrokerResponse,
			// Token: 0x04000225 RID: 549
			NotReady
		}

		// Token: 0x02000085 RID: 133
		public enum BrokerRequestAgentError
		{
			// Token: 0x04000227 RID: 551
			InvalidTaskId = 407,
			// Token: 0x04000228 RID: 552
			InvalidBrokerResponse,
			// Token: 0x04000229 RID: 553
			InvalidBrokerResponseData
		}

		// Token: 0x02000086 RID: 134
		public enum AppProvisioningError
		{
			// Token: 0x0400022B RID: 555
			UDCNotInstalled = 1061,
			// Token: 0x0400022C RID: 556
			UDCNotRunning,
			// Token: 0x0400022D RID: 557
			InvalidDeviceId,
			// Token: 0x0400022E RID: 558
			DeviceNotConfigured,
			// Token: 0x0400022F RID: 559
			DeviceNotRegistered,
			// Token: 0x04000230 RID: 560
			APSNotAvailable,
			// Token: 0x04000231 RID: 561
			NoInternetConnection
		}

		// Token: 0x02000087 RID: 135
		public enum RequestMapperError
		{
			// Token: 0x04000233 RID: 563
			InvalidSubscriptionFile = 510,
			// Token: 0x04000234 RID: 564
			InvalidMachineInformation,
			// Token: 0x04000235 RID: 565
			MatchingPackageNotFound,
			// Token: 0x04000236 RID: 566
			MatchingApplciablePackageNotFound,
			// Token: 0x04000237 RID: 567
			MatchingMappingsNotFound,
			// Token: 0x04000238 RID: 568
			InvalidAppTagsFile
		}

		// Token: 0x02000088 RID: 136
		public enum PluginHostError
		{
			// Token: 0x0400023A RID: 570
			MissingMembersForRequestProcessor = 601,
			// Token: 0x0400023B RID: 571
			PluginNotValid,
			// Token: 0x0400023C RID: 572
			ExceptionThrownInvokingPlugin,
			// Token: 0x0400023D RID: 573
			CancelEventNotFound,
			// Token: 0x0400023E RID: 574
			PluginError,
			// Token: 0x0400023F RID: 575
			PluginHostNotStarted
		}

		// Token: 0x02000089 RID: 137
		public enum PluginError
		{
			// Token: 0x04000241 RID: 577
			ExceptionThrownRunningPlugin = 701,
			// Token: 0x04000242 RID: 578
			PluginRequestCancelled
		}

		// Token: 0x0200008A RID: 138
		public enum EventManagerError
		{
			// Token: 0x04000244 RID: 580
			InvalidSubscriptionFile = 801
		}

		// Token: 0x0200008B RID: 139
		public enum PluginManagerError
		{
			// Token: 0x04000246 RID: 582
			RequestCancellationError = 901
		}

		// Token: 0x0200008C RID: 140
		public enum PluginHostWrapperError
		{
			// Token: 0x04000248 RID: 584
			PluginHostNotStarted = 1001
		}
	}
}
