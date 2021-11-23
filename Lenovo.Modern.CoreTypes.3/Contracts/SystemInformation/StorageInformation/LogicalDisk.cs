using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation.StorageInformation
{
	// Token: 0x02000049 RID: 73
	public sealed class LogicalDisk
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000311 RID: 785 RVA: 0x000059F5 File Offset: 0x00003BF5
		// (set) Token: 0x06000312 RID: 786 RVA: 0x000059FD File Offset: 0x00003BFD
		[XmlElement("Alert")]
		public string Alert { get; set; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00005A06 File Offset: 0x00003C06
		// (set) Token: 0x06000314 RID: 788 RVA: 0x00005A0E File Offset: 0x00003C0E
		[XmlElement("BootVolume")]
		public string BootVolume { get; set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00005A17 File Offset: 0x00003C17
		// (set) Token: 0x06000316 RID: 790 RVA: 0x00005A1F File Offset: 0x00003C1F
		[XmlElement("Caption")]
		public string Caption { get; set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000317 RID: 791 RVA: 0x00005A28 File Offset: 0x00003C28
		// (set) Token: 0x06000318 RID: 792 RVA: 0x00005A30 File Offset: 0x00003C30
		[XmlElement("DeviceID")]
		public string DeviceID { get; set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00005A39 File Offset: 0x00003C39
		// (set) Token: 0x0600031A RID: 794 RVA: 0x00005A41 File Offset: 0x00003C41
		[XmlElement("Dsk_DeviceID")]
		public string Dsk_DeviceID { get; set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600031B RID: 795 RVA: 0x00005A4A File Offset: 0x00003C4A
		// (set) Token: 0x0600031C RID: 796 RVA: 0x00005A52 File Offset: 0x00003C52
		[XmlElement("FileSystem")]
		public string FileSystem { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00005A5B File Offset: 0x00003C5B
		// (set) Token: 0x0600031E RID: 798 RVA: 0x00005A63 File Offset: 0x00003C63
		[XmlElement("FreeSpace")]
		public string FreeSpace { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00005A6C File Offset: 0x00003C6C
		// (set) Token: 0x06000320 RID: 800 RVA: 0x00005A74 File Offset: 0x00003C74
		[XmlElement("Size")]
		public string Size { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000321 RID: 801 RVA: 0x00005A7D File Offset: 0x00003C7D
		// (set) Token: 0x06000322 RID: 802 RVA: 0x00005A85 File Offset: 0x00003C85
		[XmlElement("UsedSpace")]
		public string UsedSpace { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000323 RID: 803 RVA: 0x00005A8E File Offset: 0x00003C8E
		// (set) Token: 0x06000324 RID: 804 RVA: 0x00005A96 File Offset: 0x00003C96
		[XmlElement("VolumeName")]
		public string VolumeName { get; set; }
	}
}
