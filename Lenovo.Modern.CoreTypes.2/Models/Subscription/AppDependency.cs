using System;
using System.Xml.Serialization;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;

namespace Lenovo.Modern.CoreTypes.Models.Subscription
{
	// Token: 0x0200000C RID: 12
	[XmlRoot(ElementName = "AppDependency", Namespace = null)]
	public sealed class AppDependency : IBasicEligabilityRequirements
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003846 File Offset: 0x00001A46
		// (set) Token: 0x06000067 RID: 103 RVA: 0x0000384E File Offset: 0x00001A4E
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003857 File Offset: 0x00001A57
		// (set) Token: 0x06000069 RID: 105 RVA: 0x0000385F File Offset: 0x00001A5F
		[XmlAttribute(AttributeName = "force")]
		public bool ForceUpgrade { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00003868 File Offset: 0x00001A68
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00003870 File Offset: 0x00001A70
		[XmlIgnore]
		public Uri DependencyLocation { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600006C RID: 108 RVA: 0x0000387C File Offset: 0x00001A7C
		// (set) Token: 0x0600006D RID: 109 RVA: 0x000038A6 File Offset: 0x00001AA6
		[XmlText]
		public string XmlDependencyLocationDoNotUse
		{
			get
			{
				string result = null;
				if (this.DependencyLocation != null)
				{
					result = this.DependencyLocation.ToString();
				}
				return result;
			}
			set
			{
				this.DependencyLocation = new Uri(value);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000038B4 File Offset: 0x00001AB4
		// (set) Token: 0x0600006F RID: 111 RVA: 0x000038BC File Offset: 0x00001ABC
		[XmlAttribute(AttributeName = "id")]
		public Guid Id { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000070 RID: 112 RVA: 0x000038C5 File Offset: 0x00001AC5
		// (set) Token: 0x06000071 RID: 113 RVA: 0x000038CD File Offset: 0x00001ACD
		[XmlAttribute(AttributeName = "friendlyname")]
		public string FriendlyName { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000072 RID: 114 RVA: 0x000038D6 File Offset: 0x00001AD6
		// (set) Token: 0x06000073 RID: 115 RVA: 0x000038DE File Offset: 0x00001ADE
		[XmlIgnore]
		public BrandType Brand { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000038E8 File Offset: 0x00001AE8
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00003910 File Offset: 0x00001B10
		[XmlAttribute(AttributeName = "brand")]
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

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00003975 File Offset: 0x00001B75
		// (set) Token: 0x06000077 RID: 119 RVA: 0x0000397D File Offset: 0x00001B7D
		[XmlIgnore]
		public EnclosureType EnclosureType { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003988 File Offset: 0x00001B88
		// (set) Token: 0x06000079 RID: 121 RVA: 0x000039B0 File Offset: 0x00001BB0
		[XmlAttribute(AttributeName = "enclosureType")]
		public string XmlDoNotUseEnclosureType
		{
			get
			{
				return this.EnclosureType.ToString().ToLowerInvariant();
			}
			set
			{
				EnclosureType enclosureType = EnclosureType.None;
				if (value != null && !Enum.TryParse<EnclosureType>(value.Trim(), true, out enclosureType))
				{
					Enum.TryParse<EnclosureType>(value.Trim().Replace("-", string.Empty).Replace(" ", string.Empty)
						.Replace("_", string.Empty), true, out enclosureType);
				}
				this.EnclosureType = enclosureType;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003A15 File Offset: 0x00001C15
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00003A1D File Offset: 0x00001C1D
		[XmlAttribute(AttributeName = "country")]
		public string Country { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003A26 File Offset: 0x00001C26
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00003A2E File Offset: 0x00001C2E
		[XmlAttribute(AttributeName = "language")]
		public string Language { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00003A37 File Offset: 0x00001C37
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00003A3F File Offset: 0x00001C3F
		[XmlAttribute(AttributeName = "subbrand")]
		public string SubBrand { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003A48 File Offset: 0x00001C48
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00003A50 File Offset: 0x00001C50
		[XmlAttribute(AttributeName = "appversion")]
		public string AppVersion { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003A59 File Offset: 0x00001C59
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00003A61 File Offset: 0x00001C61
		[XmlAttribute(AttributeName = "osversion")]
		public string OsVersion { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00003A6A File Offset: 0x00001C6A
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00003A72 File Offset: 0x00001C72
		[XmlAttribute(AttributeName = "osbitness")]
		public string OsBitness { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003A7B File Offset: 0x00001C7B
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00003A83 File Offset: 0x00001C83
		[XmlAttribute(AttributeName = "tag")]
		public string Tag { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003A8C File Offset: 0x00001C8C
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00003A94 File Offset: 0x00001C94
		[XmlAttribute(AttributeName = "manufacturer")]
		public string Manufacturer { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003A9D File Offset: 0x00001C9D
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00003AA5 File Offset: 0x00001CA5
		[XmlAttribute(AttributeName = "family")]
		public string Family { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003AAE File Offset: 0x00001CAE
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00003AB6 File Offset: 0x00001CB6
		[XmlAttribute(AttributeName = "mtm")]
		public string Mtm { get; set; }
	}
}
