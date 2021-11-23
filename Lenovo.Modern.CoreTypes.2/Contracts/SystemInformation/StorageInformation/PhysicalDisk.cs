using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation.StorageInformation
{
	// Token: 0x0200004A RID: 74
	public sealed class PhysicalDisk
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000326 RID: 806 RVA: 0x00005A9F File Offset: 0x00003C9F
		// (set) Token: 0x06000327 RID: 807 RVA: 0x00005AA7 File Offset: 0x00003CA7
		[XmlElement("Manufacturer")]
		public string Manufacturer { get; set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000328 RID: 808 RVA: 0x00005AB0 File Offset: 0x00003CB0
		// (set) Token: 0x06000329 RID: 809 RVA: 0x00005AB8 File Offset: 0x00003CB8
		[XmlElement("Model")]
		public string Model { get; set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600032A RID: 810 RVA: 0x00005AC1 File Offset: 0x00003CC1
		// (set) Token: 0x0600032B RID: 811 RVA: 0x00005AC9 File Offset: 0x00003CC9
		[XmlElement("Primary")]
		public string Primary { get; set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600032C RID: 812 RVA: 0x00005AD2 File Offset: 0x00003CD2
		// (set) Token: 0x0600032D RID: 813 RVA: 0x00005ADA File Offset: 0x00003CDA
		[XmlElement("Removable")]
		public string Removable { get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600032E RID: 814 RVA: 0x00005AE3 File Offset: 0x00003CE3
		// (set) Token: 0x0600032F RID: 815 RVA: 0x00005AEB File Offset: 0x00003CEB
		[XmlElement("ReturnCode")]
		public string ReturnCode { get; set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000330 RID: 816 RVA: 0x00005AF4 File Offset: 0x00003CF4
		// (set) Token: 0x06000331 RID: 817 RVA: 0x00005AFC File Offset: 0x00003CFC
		[XmlElement("SerialNumber")]
		public string SerialNumber { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000332 RID: 818 RVA: 0x00005B05 File Offset: 0x00003D05
		// (set) Token: 0x06000333 RID: 819 RVA: 0x00005B0D File Offset: 0x00003D0D
		[XmlElement("Size")]
		public string Size { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00005B16 File Offset: 0x00003D16
		// (set) Token: 0x06000335 RID: 821 RVA: 0x00005B1E File Offset: 0x00003D1E
		[XmlElement("Status")]
		public string Status { get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00005B27 File Offset: 0x00003D27
		// (set) Token: 0x06000337 RID: 823 RVA: 0x00005B2F File Offset: 0x00003D2F
		[XmlElement("Caption")]
		public string Caption { get; set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00005B38 File Offset: 0x00003D38
		// (set) Token: 0x06000339 RID: 825 RVA: 0x00005B40 File Offset: 0x00003D40
		[XmlElement("DeviceID")]
		public string DeviceID { get; set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600033A RID: 826 RVA: 0x00005B49 File Offset: 0x00003D49
		// (set) Token: 0x0600033B RID: 827 RVA: 0x00005B51 File Offset: 0x00003D51
		[XmlElement("FirmwareRevision")]
		public string FirmwareRevision { get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600033C RID: 828 RVA: 0x00005B5A File Offset: 0x00003D5A
		// (set) Token: 0x0600033D RID: 829 RVA: 0x00005B62 File Offset: 0x00003D62
		[XmlElement("FreeSpace")]
		public string FreeSpace { get; set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00005B6B File Offset: 0x00003D6B
		// (set) Token: 0x0600033F RID: 831 RVA: 0x00005B73 File Offset: 0x00003D73
		[XmlElement("PartitionStyle")]
		public string PartitionStyle { get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00005B7C File Offset: 0x00003D7C
		// (set) Token: 0x06000341 RID: 833 RVA: 0x00005B84 File Offset: 0x00003D84
		[XmlElement("VolumeName")]
		public string VolumeName { get; set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000342 RID: 834 RVA: 0x00005B8D File Offset: 0x00003D8D
		// (set) Token: 0x06000343 RID: 835 RVA: 0x00005B95 File Offset: 0x00003D95
		[XmlElement("DriveLetter")]
		public string DriveLetter { get; set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00005B9E File Offset: 0x00003D9E
		// (set) Token: 0x06000345 RID: 837 RVA: 0x00005BA6 File Offset: 0x00003DA6
		[XmlArray("LogicalDiskList")]
		[XmlArrayItem("LogicalDisk")]
		public LogicalDisk[] LogicalDiskList { get; set; }
	}
}
