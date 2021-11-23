using System;

namespace Lenovo.Modern.CoreTypes.Contracts.FileSystem
{
	// Token: 0x020000B5 RID: 181
	public sealed class ContractConstants
	{
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x00008148 File Offset: 0x00006348
		public static ContractConstants Get
		{
			get
			{
				ContractConstants result;
				if ((result = ContractConstants._contractConstants) == null)
				{
					ContractConstants contractConstants = new ContractConstants();
					contractConstants.ContractName = "SystemUtilities.Filesystem";
					contractConstants.ContractVersion = "1.0.0.0";
					contractConstants.CommandNameGetDirectoryListing = "Get-DirectoryListing";
					contractConstants.CommandNamePerformActionItems = "Perform-ActionOnItems";
					contractConstants.DataTypeDirectoryListingRequest = "DirectoryListingRequest";
					contractConstants.DataTypeDirectoryListingResponse = "DirectoryListingResponse";
					contractConstants.DataTypeItemActionRequest = "ItemActionRequest";
					contractConstants.ActionTypeCopy = "COPY";
					result = contractConstants;
					ContractConstants._contractConstants = contractConstants;
				}
				return result;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x000081C1 File Offset: 0x000063C1
		// (set) Token: 0x060006BD RID: 1725 RVA: 0x000081C9 File Offset: 0x000063C9
		public string ActionTypeCopy { get; private set; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x000081D2 File Offset: 0x000063D2
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x000081DA File Offset: 0x000063DA
		public string ContractName { get; private set; }

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x000081E3 File Offset: 0x000063E3
		// (set) Token: 0x060006C1 RID: 1729 RVA: 0x000081EB File Offset: 0x000063EB
		public string ContractVersion { get; private set; }

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x000081F4 File Offset: 0x000063F4
		// (set) Token: 0x060006C3 RID: 1731 RVA: 0x000081FC File Offset: 0x000063FC
		public string CommandNameGetDirectoryListing { get; private set; }

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x00008205 File Offset: 0x00006405
		// (set) Token: 0x060006C5 RID: 1733 RVA: 0x0000820D File Offset: 0x0000640D
		public string CommandNamePerformActionItems { get; private set; }

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x00008216 File Offset: 0x00006416
		// (set) Token: 0x060006C7 RID: 1735 RVA: 0x0000821E File Offset: 0x0000641E
		public string DataTypeDirectoryListingRequest { get; private set; }

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x00008227 File Offset: 0x00006427
		// (set) Token: 0x060006C9 RID: 1737 RVA: 0x0000822F File Offset: 0x0000642F
		public string DataTypeDirectoryListingResponse { get; private set; }

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x00008238 File Offset: 0x00006438
		// (set) Token: 0x060006CB RID: 1739 RVA: 0x00008240 File Offset: 0x00006440
		public string DataTypeItemActionRequest { get; private set; }

		// Token: 0x04000361 RID: 865
		private static ContractConstants _contractConstants;
	}
}
