using System;
using System.Xml.Serialization;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;

namespace Lenovo.Modern.CoreTypes.Models.Subscription
{
	// Token: 0x0200000D RID: 13
	[XmlRoot(ElementName = "AppSetting", Namespace = null)]
	public sealed class AppSetting : IBasicEligabilityRequirements
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003ABF File Offset: 0x00001CBF
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00003AC7 File Offset: 0x00001CC7
		[XmlAttribute(AttributeName = "key")]
		public string Key { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00003AD0 File Offset: 0x00001CD0
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00003AD8 File Offset: 0x00001CD8
		[XmlText]
		public string Value { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00003AE1 File Offset: 0x00001CE1
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00003AE9 File Offset: 0x00001CE9
		[XmlIgnore]
		public BrandType Brand { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003AF4 File Offset: 0x00001CF4
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00003B1C File Offset: 0x00001D1C
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00003B81 File Offset: 0x00001D81
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00003B89 File Offset: 0x00001D89
		[XmlAttribute(AttributeName = "country")]
		public string Country { get; set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00003B92 File Offset: 0x00001D92
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00003B9A File Offset: 0x00001D9A
		[XmlAttribute(AttributeName = "language")]
		public string Language { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00003BA3 File Offset: 0x00001DA3
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00003BAB File Offset: 0x00001DAB
		[XmlAttribute(AttributeName = "subbrand")]
		public string SubBrand { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00003BB4 File Offset: 0x00001DB4
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00003BBC File Offset: 0x00001DBC
		[XmlAttribute(AttributeName = "friendlyname")]
		public string FriendlyName { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00003BC5 File Offset: 0x00001DC5
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00003BCD File Offset: 0x00001DCD
		[XmlIgnore]
		public EnclosureType EnclosureType { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00003BD8 File Offset: 0x00001DD8
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003C00 File Offset: 0x00001E00
		[XmlAttribute(AttributeName = "enclosuretype")]
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003C65 File Offset: 0x00001E65
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003C6D File Offset: 0x00001E6D
		[XmlAttribute(AttributeName = "appversion")]
		public string AppVersion { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003C76 File Offset: 0x00001E76
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00003C7E File Offset: 0x00001E7E
		[XmlAttribute(AttributeName = "osversion")]
		public string OsVersion { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00003C87 File Offset: 0x00001E87
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00003C8F File Offset: 0x00001E8F
		[XmlAttribute(AttributeName = "osbitness")]
		public string OsBitness { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003C98 File Offset: 0x00001E98
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00003CA0 File Offset: 0x00001EA0
		[XmlAttribute(AttributeName = "tag")]
		public string Tag { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003CA9 File Offset: 0x00001EA9
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00003CB1 File Offset: 0x00001EB1
		[XmlAttribute(AttributeName = "manufacturer")]
		public string Manufacturer { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00003CBA File Offset: 0x00001EBA
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00003CC2 File Offset: 0x00001EC2
		[XmlAttribute(AttributeName = "family")]
		public string Family { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00003CCB File Offset: 0x00001ECB
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00003CD3 File Offset: 0x00001ED3
		[XmlAttribute(AttributeName = "mtm")]
		public string Mtm { get; set; }
	}
}
