using System;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation
{
	// Token: 0x02000045 RID: 69
	[XmlRoot(ElementName = "MachineInformation", Namespace = null)]
	public sealed class MachineInformation
	{
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000546D File Offset: 0x0000366D
		// (set) Token: 0x060002BF RID: 703 RVA: 0x00005475 File Offset: 0x00003675
		[XmlElement(ElementName = "BiosVersion")]
		public string BiosVersion { get; set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000547E File Offset: 0x0000367E
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x00005486 File Offset: 0x00003686
		[XmlElement(ElementName = "BiosDate")]
		public string BiosDate { get; set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000548F File Offset: 0x0000368F
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x00005497 File Offset: 0x00003697
		[XmlElement(ElementName = "ECVersion")]
		public string ECVersion { get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x000054A0 File Offset: 0x000036A0
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x000054A8 File Offset: 0x000036A8
		[XmlIgnore]
		public BrandType Brand { get; set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x000054B4 File Offset: 0x000036B4
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x000054DC File Offset: 0x000036DC
		[XmlElement(ElementName = "Brand")]
		public string XmlBrandDoNotUse
		{
			get
			{
				return this.Brand.ToString().ToLowerInvariant();
			}
			set
			{
				BrandType brand = BrandType.None;
				if (value != null && !Enum.TryParse<BrandType>(value.Trim(), true, out brand))
				{
					Enum.TryParse<BrandType>(value.Trim().Replace("-", string.Empty).Replace(" ", string.Empty)
						.Replace("_", string.Empty), true, out brand);
				}
				this.Brand = brand;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x00005541 File Offset: 0x00003741
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x00005549 File Offset: 0x00003749
		[XmlElement(ElementName = "Country")]
		public string CountryCode { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060002CA RID: 714 RVA: 0x00005552 File Offset: 0x00003752
		// (set) Token: 0x060002CB RID: 715 RVA: 0x0000555A File Offset: 0x0000375A
		[XmlIgnore]
		public DateTimeOffset DateCreated { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060002CC RID: 716 RVA: 0x00005564 File Offset: 0x00003764
		// (set) Token: 0x060002CD RID: 717 RVA: 0x000055A4 File Offset: 0x000037A4
		[XmlElement("DateCreated", IsNullable = true)]
		public string XmlDateCreatedDoNotUse
		{
			get
			{
				string result = string.Empty;
				if (this.DateCreated != DateTimeOffset.MinValue)
				{
					result = this.DateCreated.ToString(Constants.DateTimeFormat, CultureInfo.InvariantCulture);
				}
				return result;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					try
					{
						this.DateCreated = DateTimeOffset.ParseExact(value, Constants.DateFormat, CultureInfo.InvariantCulture);
					}
					catch (Exception)
					{
						this.DateCreated = DateTimeOffset.ParseExact(value, Constants.DateTimeFormat, CultureInfo.InvariantCulture);
					}
				}
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060002CE RID: 718 RVA: 0x000055FC File Offset: 0x000037FC
		// (set) Token: 0x060002CF RID: 719 RVA: 0x00005604 File Offset: 0x00003804
		[XmlIgnore]
		public EnclosureType Enclosure { get; set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x00005610 File Offset: 0x00003810
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x00005638 File Offset: 0x00003838
		[XmlElement(ElementName = "EnclosureType")]
		public string XmlDoNotUseEnclosureType
		{
			get
			{
				return this.Enclosure.ToString().ToLowerInvariant();
			}
			set
			{
				EnclosureType enclosure = EnclosureType.None;
				if (value != null && !Enum.TryParse<EnclosureType>(value.Trim(), true, out enclosure))
				{
					Enum.TryParse<EnclosureType>(value.Trim().Replace("-", string.Empty).Replace(" ", string.Empty)
						.Replace("_", string.Empty), true, out enclosure);
				}
				this.Enclosure = enclosure;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000569D File Offset: 0x0000389D
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x000056A5 File Offset: 0x000038A5
		[XmlElement(ElementName = "Family")]
		public string Family { get; set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x000056AE File Offset: 0x000038AE
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x000056B6 File Offset: 0x000038B6
		[XmlIgnore]
		public DateTimeOffset FirstRunDate { get; set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x000056C0 File Offset: 0x000038C0
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x00005700 File Offset: 0x00003900
		[XmlElement("FirstRunDate", IsNullable = true)]
		public string XmlFirstRunDateDoNotUse
		{
			get
			{
				string result = string.Empty;
				if (this.FirstRunDate != DateTimeOffset.MinValue)
				{
					result = this.FirstRunDate.ToString(Constants.DateTimeFormat, CultureInfo.InvariantCulture);
				}
				return result;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					try
					{
						this.FirstRunDate = DateTimeOffset.ParseExact(value, Constants.DateFormat, CultureInfo.InvariantCulture);
					}
					catch (Exception)
					{
						this.FirstRunDate = DateTimeOffset.ParseExact(value, Constants.DateTimeFormat, CultureInfo.InvariantCulture);
					}
				}
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x00005758 File Offset: 0x00003958
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x00005760 File Offset: 0x00003960
		[XmlElement(ElementName = "Locale")]
		public string Locale { get; set; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060002DA RID: 730 RVA: 0x00005769 File Offset: 0x00003969
		// (set) Token: 0x060002DB RID: 731 RVA: 0x00005771 File Offset: 0x00003971
		[XmlElement(ElementName = "MTM")]
		public string MTM { get; set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000577A File Offset: 0x0000397A
		// (set) Token: 0x060002DD RID: 733 RVA: 0x00005782 File Offset: 0x00003982
		[XmlElement(ElementName = "MT")]
		public string MT { get; set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000578B File Offset: 0x0000398B
		// (set) Token: 0x060002DF RID: 735 RVA: 0x00005793 File Offset: 0x00003993
		[XmlElement(ElementName = "Manufacturer")]
		public string Manufacturer { get; set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000579C File Offset: 0x0000399C
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x000057A4 File Offset: 0x000039A4
		[XmlElement(ElementName = "OSBitness")]
		public string OperatingSystemBitness { get; set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x000057AD File Offset: 0x000039AD
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x000057B5 File Offset: 0x000039B5
		[XmlElement(ElementName = "OSVersionString")]
		public string OperatingSystemVerion { get; set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x000057BE File Offset: 0x000039BE
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x000057C6 File Offset: 0x000039C6
		[XmlElement(ElementName = "OSName")]
		public string OSName { get; set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x000057CF File Offset: 0x000039CF
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x000057D7 File Offset: 0x000039D7
		[XmlElement(ElementName = "OS")]
		public string OS { get; set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x000057E0 File Offset: 0x000039E0
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x000057E8 File Offset: 0x000039E8
		[XmlElement(ElementName = "CPUAddressWidth")]
		public string CPUAddressWidth { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060002EA RID: 746 RVA: 0x000057F1 File Offset: 0x000039F1
		// (set) Token: 0x060002EB RID: 747 RVA: 0x000057F9 File Offset: 0x000039F9
		[XmlElement(ElementName = "CPUArchitecture")]
		public string CPUArchitecture { get; set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060002EC RID: 748 RVA: 0x00005802 File Offset: 0x00003A02
		// (set) Token: 0x060002ED RID: 749 RVA: 0x0000580A File Offset: 0x00003A0A
		[XmlArray("PreloadTagList")]
		[XmlArrayItem("PreloadTag")]
		public string[] PreloadTagList { get; set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060002EE RID: 750 RVA: 0x00005813 File Offset: 0x00003A13
		// (set) Token: 0x060002EF RID: 751 RVA: 0x0000581B File Offset: 0x00003A1B
		[XmlElement(ElementName = "Serialnumber")]
		public string SerialNumber { get; set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x00005824 File Offset: 0x00003A24
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x0000582C File Offset: 0x00003A2C
		[XmlElement(ElementName = "SKU")]
		public string SKU { get; set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x00005835 File Offset: 0x00003A35
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x0000583D File Offset: 0x00003A3D
		[XmlElement(ElementName = "SubBrand")]
		public string SubBrand { get; set; }

		// Token: 0x060002F4 RID: 756 RVA: 0x00005848 File Offset: 0x00003A48
		private static bool IsWebServiceValid(string webServiceText)
		{
			bool result = false;
			try
			{
				if (!string.IsNullOrWhiteSpace(webServiceText) && Uri.IsWellFormedUriString(webServiceText, UriKind.RelativeOrAbsolute))
				{
					if (new Uri(webServiceText).Host.Equals("www.Lenovo.com", StringComparison.OrdinalIgnoreCase))
					{
						result = true;
					}
					if (!string.IsNullOrWhiteSpace(Path.GetExtension(webServiceText)))
					{
						result = false;
					}
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x000058AC File Offset: 0x00003AAC
		private static string TrimSerialNumber(string serialNumber)
		{
			if (!string.IsNullOrWhiteSpace(serialNumber) && serialNumber.Length > 10)
			{
				serialNumber = serialNumber.Substring(0, 10);
			}
			return serialNumber;
		}
	}
}
