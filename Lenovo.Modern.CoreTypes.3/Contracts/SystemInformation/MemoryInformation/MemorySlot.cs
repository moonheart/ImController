using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation.MemoryInformation
{
	// Token: 0x02000050 RID: 80
	public sealed class MemorySlot
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600035F RID: 863 RVA: 0x00005CAB File Offset: 0x00003EAB
		// (set) Token: 0x06000360 RID: 864 RVA: 0x00005CB3 File Offset: 0x00003EB3
		[XmlElement(ElementName = "SlotIndex")]
		public string SlotIndex { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00005CBC File Offset: 0x00003EBC
		// (set) Token: 0x06000362 RID: 866 RVA: 0x00005CC4 File Offset: 0x00003EC4
		[XmlElement(ElementName = "BankLabel")]
		public string BankLabel { get; set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00005CCD File Offset: 0x00003ECD
		// (set) Token: 0x06000364 RID: 868 RVA: 0x00005CD5 File Offset: 0x00003ED5
		[XmlElement(ElementName = "InstalledMemory")]
		public string InstalledMemory { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000365 RID: 869 RVA: 0x00005CDE File Offset: 0x00003EDE
		// (set) Token: 0x06000366 RID: 870 RVA: 0x00005CE6 File Offset: 0x00003EE6
		[XmlElement(ElementName = "DeviceLocator")]
		public string DeviceLocator { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000367 RID: 871 RVA: 0x00005CEF File Offset: 0x00003EEF
		// (set) Token: 0x06000368 RID: 872 RVA: 0x00005CF7 File Offset: 0x00003EF7
		[XmlElement(ElementName = "Manufacturer")]
		public string Manufacturer { get; set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00005D00 File Offset: 0x00003F00
		// (set) Token: 0x0600036A RID: 874 RVA: 0x00005D08 File Offset: 0x00003F08
		[XmlElement(ElementName = "MaxCapacity")]
		public string MaxCapacity { get; set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00005D11 File Offset: 0x00003F11
		// (set) Token: 0x0600036C RID: 876 RVA: 0x00005D19 File Offset: 0x00003F19
		[XmlElement(ElementName = "MemoryType")]
		public string MemoryType { get; set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00005D22 File Offset: 0x00003F22
		// (set) Token: 0x0600036E RID: 878 RVA: 0x00005D2A File Offset: 0x00003F2A
		[XmlElement(ElementName = "Onboard")]
		public string Onboard { get; set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00005D33 File Offset: 0x00003F33
		// (set) Token: 0x06000370 RID: 880 RVA: 0x00005D3B File Offset: 0x00003F3B
		[XmlElement(ElementName = "ReturnCode")]
		public string ReturnCode { get; set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00005D44 File Offset: 0x00003F44
		// (set) Token: 0x06000372 RID: 882 RVA: 0x00005D4C File Offset: 0x00003F4C
		[XmlElement(ElementName = "SerialNumber")]
		public string SerialNumber { get; set; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00005D55 File Offset: 0x00003F55
		// (set) Token: 0x06000374 RID: 884 RVA: 0x00005D5D File Offset: 0x00003F5D
		[XmlElement(ElementName = "Speed")]
		public string Speed { get; set; }
	}
}
