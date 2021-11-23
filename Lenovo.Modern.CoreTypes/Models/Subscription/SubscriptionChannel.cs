using System;
using System.Xml.Serialization;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;

namespace Lenovo.Modern.CoreTypes.Models.Subscription
{
	// Token: 0x02000017 RID: 23
	public sealed class SubscriptionChannel : IBasicEligabilityRequirements
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00003EF0 File Offset: 0x000020F0
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00003EF8 File Offset: 0x000020F8
		[XmlAttribute(AttributeName = "id")]
		public Guid Id { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00003F01 File Offset: 0x00002101
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00003F09 File Offset: 0x00002109
		[XmlText]
		public string BaseUrl { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00003F12 File Offset: 0x00002112
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00003F1A File Offset: 0x0000211A
		[XmlAttribute(AttributeName = "friendlyname")]
		public string FriendlyName { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00003F23 File Offset: 0x00002123
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00003F2B File Offset: 0x0000212B
		[XmlIgnore]
		public BrandType Brand { get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00003F34 File Offset: 0x00002134
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00003F5C File Offset: 0x0000215C
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

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00003FC1 File Offset: 0x000021C1
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00003FC9 File Offset: 0x000021C9
		[XmlAttribute(AttributeName = "country")]
		public string Country { get; set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00003FD2 File Offset: 0x000021D2
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00003FDA File Offset: 0x000021DA
		[XmlAttribute(AttributeName = "language")]
		public string Language { get; set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00003FE3 File Offset: 0x000021E3
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00003FEB File Offset: 0x000021EB
		[XmlAttribute(AttributeName = "subbrand")]
		public string SubBrand { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00003FF4 File Offset: 0x000021F4
		// (set) Token: 0x06000109 RID: 265 RVA: 0x00003FFC File Offset: 0x000021FC
		[XmlIgnore]
		public EnclosureType EnclosureType { get; set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00004008 File Offset: 0x00002208
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00004030 File Offset: 0x00002230
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

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00004095 File Offset: 0x00002295
		// (set) Token: 0x0600010D RID: 269 RVA: 0x0000409D File Offset: 0x0000229D
		[XmlAttribute(AttributeName = "appversion")]
		public string AppVersion { get; set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000040A6 File Offset: 0x000022A6
		// (set) Token: 0x0600010F RID: 271 RVA: 0x000040AE File Offset: 0x000022AE
		[XmlAttribute(AttributeName = "osversion")]
		public string OsVersion { get; set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000040B7 File Offset: 0x000022B7
		// (set) Token: 0x06000111 RID: 273 RVA: 0x000040BF File Offset: 0x000022BF
		[XmlAttribute(AttributeName = "osbitness")]
		public string OsBitness { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000112 RID: 274 RVA: 0x000040C8 File Offset: 0x000022C8
		// (set) Token: 0x06000113 RID: 275 RVA: 0x000040D0 File Offset: 0x000022D0
		[XmlAttribute(AttributeName = "tag")]
		public string Tag { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000040D9 File Offset: 0x000022D9
		// (set) Token: 0x06000115 RID: 277 RVA: 0x000040E1 File Offset: 0x000022E1
		[XmlAttribute(AttributeName = "manufacturer")]
		public string Manufacturer { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000116 RID: 278 RVA: 0x000040EA File Offset: 0x000022EA
		// (set) Token: 0x06000117 RID: 279 RVA: 0x000040F2 File Offset: 0x000022F2
		[XmlAttribute(AttributeName = "family")]
		public string Family { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000118 RID: 280 RVA: 0x000040FB File Offset: 0x000022FB
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00004103 File Offset: 0x00002303
		[XmlAttribute(AttributeName = "mtm")]
		public string Mtm { get; set; }
	}
}
