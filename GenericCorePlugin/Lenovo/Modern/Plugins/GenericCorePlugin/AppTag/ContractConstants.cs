using System;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag
{
	// Token: 0x0200001B RID: 27
	public sealed class ContractConstants
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00007BE4 File Offset: 0x00005DE4
		public static ContractConstants Get
		{
			get
			{
				ContractConstants result;
				if ((result = ContractConstants._contractConstants) == null)
				{
					ContractConstants contractConstants = new ContractConstants();
					contractConstants.ContractName = "SystemInformation.AppTag";
					contractConstants.ContractVersion = "1.0.0.0";
					contractConstants.CommandNameGetAppAndTags = "Get-AppsAndTags";
					contractConstants.CommandNameWriteAppAndTags = "Write-AppsAndTags";
					contractConstants.DataTypeAppAndTagsRequest = "AppAndTagsRequest";
					contractConstants.DataTypeAppAndTagsResponse = "AppAndTagsResponse";
					result = contractConstants;
					ContractConstants._contractConstants = contractConstants;
				}
				return result;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00007C47 File Offset: 0x00005E47
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00007C4F File Offset: 0x00005E4F
		public string ContractName { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00007C58 File Offset: 0x00005E58
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00007C60 File Offset: 0x00005E60
		public string ContractVersion { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00007C69 File Offset: 0x00005E69
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00007C71 File Offset: 0x00005E71
		public string CommandNameGetAppAndTags { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00007C7A File Offset: 0x00005E7A
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00007C82 File Offset: 0x00005E82
		public string CommandNameWriteAppAndTags { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00007C8B File Offset: 0x00005E8B
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00007C93 File Offset: 0x00005E93
		public string DataTypeAppAndTagsRequest { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00007C9C File Offset: 0x00005E9C
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00007CA4 File Offset: 0x00005EA4
		public string DataTypeAppAndTagsResponse { get; private set; }

		// Token: 0x0400004C RID: 76
		private static ContractConstants _contractConstants;
	}
}
