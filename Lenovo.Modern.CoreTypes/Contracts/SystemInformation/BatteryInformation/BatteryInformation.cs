using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation.BatteryInformation
{
	// Token: 0x02000053 RID: 83
	public sealed class BatteryInformation
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00005E65 File Offset: 0x00004065
		// (set) Token: 0x0600038C RID: 908 RVA: 0x00005E6D File Offset: 0x0000406D
		[XmlElement(ElementName = "AlertCode")]
		public string AlertCode { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00005E76 File Offset: 0x00004076
		// (set) Token: 0x0600038E RID: 910 RVA: 0x00005E7E File Offset: 0x0000407E
		[XmlElement(ElementName = "BarCodingNumber")]
		public string BarCodingNumber { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00005E87 File Offset: 0x00004087
		// (set) Token: 0x06000390 RID: 912 RVA: 0x00005E8F File Offset: 0x0000408F
		[XmlElement(ElementName = "BatterySlotNumber")]
		public string BatterySlotNumber { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00005E98 File Offset: 0x00004098
		// (set) Token: 0x06000392 RID: 914 RVA: 0x00005EA0 File Offset: 0x000040A0
		[XmlElement(ElementName = "BatterySoftwareInstalled")]
		public string BatterySoftwareInstalled { get; set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00005EA9 File Offset: 0x000040A9
		// (set) Token: 0x06000394 RID: 916 RVA: 0x00005EB1 File Offset: 0x000040B1
		[XmlElement(ElementName = "BatteryWarrantyCode")]
		public string BatteryWarrantyCode { get; set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00005EBA File Offset: 0x000040BA
		// (set) Token: 0x06000396 RID: 918 RVA: 0x00005EC2 File Offset: 0x000040C2
		[XmlElement(ElementName = "ChargeStatus")]
		public string ChargeStatus { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00005ECB File Offset: 0x000040CB
		// (set) Token: 0x06000398 RID: 920 RVA: 0x00005ED3 File Offset: 0x000040D3
		[XmlElement(ElementName = "Condition")]
		public string Condition { get; set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00005EDC File Offset: 0x000040DC
		// (set) Token: 0x0600039A RID: 922 RVA: 0x00005EE4 File Offset: 0x000040E4
		[XmlElement(ElementName = "Current")]
		public string Current { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00005EED File Offset: 0x000040ED
		// (set) Token: 0x0600039C RID: 924 RVA: 0x00005EF5 File Offset: 0x000040F5
		[XmlElement(ElementName = "CycleCount")]
		public string CycleCount { get; set; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00005EFE File Offset: 0x000040FE
		// (set) Token: 0x0600039E RID: 926 RVA: 0x00005F06 File Offset: 0x00004106
		[XmlElement(ElementName = "FirstUsedDate")]
		public string FirstUsedDate { get; set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00005F0F File Offset: 0x0000410F
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x00005F17 File Offset: 0x00004117
		[XmlElement(ElementName = "FullChargeCapacity")]
		public string FullChargeCapacity { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00005F20 File Offset: 0x00004120
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x00005F28 File Offset: 0x00004128
		[XmlElement(ElementName = "ID")]
		public string ID { get; set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00005F31 File Offset: 0x00004131
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x00005F39 File Offset: 0x00004139
		[XmlElement(ElementName = "IsLenovoBattery")]
		public string IsLenovoBattery { get; set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00005F42 File Offset: 0x00004142
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x00005F4A File Offset: 0x0000414A
		[XmlElement(ElementName = "IsRemovable")]
		public string IsRemovable { get; set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00005F53 File Offset: 0x00004153
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x00005F5B File Offset: 0x0000415B
		[XmlElement(ElementName = "ManufactureDate")]
		public string ManufactureDate { get; set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x00005F64 File Offset: 0x00004164
		// (set) Token: 0x060003AA RID: 938 RVA: 0x00005F6C File Offset: 0x0000416C
		[XmlElement(ElementName = "ManufactureName")]
		public string ManufactureName { get; set; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00005F75 File Offset: 0x00004175
		// (set) Token: 0x060003AC RID: 940 RVA: 0x00005F7D File Offset: 0x0000417D
		[XmlElement(ElementName = "OriginalBattery")]
		public string OriginalBattery { get; set; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00005F86 File Offset: 0x00004186
		// (set) Token: 0x060003AE RID: 942 RVA: 0x00005F8E File Offset: 0x0000418E
		[XmlElement(ElementName = "PWMCondition")]
		public string PWMCondition { get; set; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060003AF RID: 943 RVA: 0x00005F97 File Offset: 0x00004197
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x00005F9F File Offset: 0x0000419F
		[XmlElement(ElementName = "RemainingCapacity")]
		public string RemainingCapacity { get; set; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x00005FA8 File Offset: 0x000041A8
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x00005FB0 File Offset: 0x000041B0
		[XmlElement(ElementName = "RemainingPercent")]
		public string RemainingPercent { get; set; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00005FB9 File Offset: 0x000041B9
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x00005FC1 File Offset: 0x000041C1
		[XmlElement(ElementName = "RemainingTime")]
		public string RemainingTime { get; set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x00005FCA File Offset: 0x000041CA
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x00005FD2 File Offset: 0x000041D2
		[XmlElement(ElementName = "ReturnCode")]
		public string ReturnCode { get; set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00005FDB File Offset: 0x000041DB
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x00005FE3 File Offset: 0x000041E3
		[XmlElement(ElementName = "SerialNumber")]
		public string SerialNumber { get; set; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00005FEC File Offset: 0x000041EC
		// (set) Token: 0x060003BA RID: 954 RVA: 0x00005FF4 File Offset: 0x000041F4
		[XmlElement(ElementName = "SoftwarePath")]
		public string SoftwarePath { get; set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00005FFD File Offset: 0x000041FD
		// (set) Token: 0x060003BC RID: 956 RVA: 0x00006005 File Offset: 0x00004205
		[XmlElement(ElementName = "Temperature")]
		public string Temperature { get; set; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0000600E File Offset: 0x0000420E
		// (set) Token: 0x060003BE RID: 958 RVA: 0x00006016 File Offset: 0x00004216
		[XmlElement(ElementName = "ValidWarranty")]
		public string ValidWarranty { get; set; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060003BF RID: 959 RVA: 0x0000601F File Offset: 0x0000421F
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x00006027 File Offset: 0x00004227
		[XmlElement(ElementName = "Voltage")]
		public string Voltage { get; set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00006030 File Offset: 0x00004230
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x00006038 File Offset: 0x00004238
		[XmlElement(ElementName = "Warranty")]
		public string Warranty { get; set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00006041 File Offset: 0x00004241
		// (set) Token: 0x060003C4 RID: 964 RVA: 0x00006049 File Offset: 0x00004249
		[XmlElement(ElementName = "Wattage")]
		public string Wattage { get; set; }
	}
}
