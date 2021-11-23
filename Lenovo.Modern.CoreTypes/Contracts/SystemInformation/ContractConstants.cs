using System;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation
{
	// Token: 0x02000041 RID: 65
	public sealed class ContractConstants
	{
		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000536C File Offset: 0x0000356C
		public static ContractConstants Get
		{
			get
			{
				ContractConstants result;
				if ((result = ContractConstants._contractConstants) == null)
				{
					ContractConstants contractConstants = new ContractConstants();
					contractConstants.ContractName = "SystemInformation.Machine";
					contractConstants.ContractVersion = "1.0.0.0";
					contractConstants.CommandNameGetMachineInformation = "Get-MachineInformation";
					contractConstants.CommandNameWriteMachineInformation = "Write-MachineInformation";
					contractConstants.DataTypeMachineInformation = "MachineInformation";
					contractConstants.DataTypeOutputLocationRequest = "OutputLocationRequest";
					contractConstants.DataTypeOutputLocationResponse = "OutputLocationResponse";
					contractConstants.FileNameMachineInformation = "MachineInformation.xml";
					result = contractConstants;
					ContractConstants._contractConstants = contractConstants;
				}
				return result;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060002AD RID: 685 RVA: 0x000053E5 File Offset: 0x000035E5
		// (set) Token: 0x060002AE RID: 686 RVA: 0x000053ED File Offset: 0x000035ED
		public string ContractName { get; private set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060002AF RID: 687 RVA: 0x000053F6 File Offset: 0x000035F6
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x000053FE File Offset: 0x000035FE
		public string ContractVersion { get; private set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x00005407 File Offset: 0x00003607
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x0000540F File Offset: 0x0000360F
		public string CommandNameGetMachineInformation { get; private set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x00005418 File Offset: 0x00003618
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x00005420 File Offset: 0x00003620
		public string CommandNameWriteMachineInformation { get; private set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x00005429 File Offset: 0x00003629
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x00005431 File Offset: 0x00003631
		public string DataTypeMachineInformation { get; private set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000543A File Offset: 0x0000363A
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x00005442 File Offset: 0x00003642
		public string DataTypeOutputLocationRequest { get; private set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000544B File Offset: 0x0000364B
		// (set) Token: 0x060002BA RID: 698 RVA: 0x00005453 File Offset: 0x00003653
		public string DataTypeOutputLocationResponse { get; private set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000545C File Offset: 0x0000365C
		// (set) Token: 0x060002BC RID: 700 RVA: 0x00005464 File Offset: 0x00003664
		public string FileNameMachineInformation { get; private set; }

		// Token: 0x04000127 RID: 295
		private static ContractConstants _contractConstants;
	}
}
