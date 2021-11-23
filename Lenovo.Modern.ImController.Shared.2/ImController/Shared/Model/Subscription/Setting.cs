using System;
using System.Xml.Serialization;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;
using Lenovo.Modern.CoreTypes.Models;

namespace Lenovo.Modern.ImController.Shared.Model.Subscription
{
	// Token: 0x02000028 RID: 40
	public sealed class Setting : IBasicEligabilityRequirements
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00007119 File Offset: 0x00005319
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00007121 File Offset: 0x00005321
		[XmlAttribute(AttributeName = "key")]
		public string Key { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000FB RID: 251 RVA: 0x0000712A File Offset: 0x0000532A
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00007132 File Offset: 0x00005332
		[XmlIgnore]
		public BrandType Brand { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000FD RID: 253 RVA: 0x0000713C File Offset: 0x0000533C
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00007160 File Offset: 0x00005360
		[XmlAttribute(AttributeName = "brand")]
		public string XmlBrandDoNotUse
		{
			get
			{
				return this.Brand.ToString();
			}
			set
			{
				BrandType brandType = BrandType.None;
				Enum.TryParse<BrandType>(value, true, out brandType);
				this.Brand = BrandType.None;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00007180 File Offset: 0x00005380
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00007188 File Offset: 0x00005388
		[XmlAttribute(AttributeName = "country")]
		public string Country { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00007191 File Offset: 0x00005391
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00007199 File Offset: 0x00005399
		[XmlAttribute(AttributeName = "subbrand")]
		public string SubBrand { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000103 RID: 259 RVA: 0x000071A2 File Offset: 0x000053A2
		// (set) Token: 0x06000104 RID: 260 RVA: 0x000071AA File Offset: 0x000053AA
		[XmlAttribute(AttributeName = "language")]
		public string Language { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000071B3 File Offset: 0x000053B3
		// (set) Token: 0x06000106 RID: 262 RVA: 0x000071BB File Offset: 0x000053BB
		[XmlAttribute(AttributeName = "friendlyName")]
		public string FriendlyName { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000071C4 File Offset: 0x000053C4
		// (set) Token: 0x06000108 RID: 264 RVA: 0x000071CC File Offset: 0x000053CC
		[XmlIgnore]
		public EnclosureType EnclosureType { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000109 RID: 265 RVA: 0x000071D8 File Offset: 0x000053D8
		// (set) Token: 0x0600010A RID: 266 RVA: 0x000071FC File Offset: 0x000053FC
		[XmlAttribute(AttributeName = "enclosureType")]
		public string XmlDoNotUseEnclosureType
		{
			get
			{
				return this.EnclosureType.ToString();
			}
			set
			{
				EnclosureType enclosureType = EnclosureType.None;
				Enum.TryParse<EnclosureType>(value, true, out enclosureType);
				this.EnclosureType = EnclosureType.None;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600010B RID: 267 RVA: 0x0000721C File Offset: 0x0000541C
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00007224 File Offset: 0x00005424
		[XmlAttribute(AttributeName = "appVersion")]
		public string AppVersion { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600010D RID: 269 RVA: 0x0000722D File Offset: 0x0000542D
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00007235 File Offset: 0x00005435
		[XmlAttribute(AttributeName = "osVersion")]
		public string OsVersion { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600010F RID: 271 RVA: 0x0000723E File Offset: 0x0000543E
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00007246 File Offset: 0x00005446
		[XmlAttribute(AttributeName = "osBitness")]
		public string OsBitness { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000724F File Offset: 0x0000544F
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00007257 File Offset: 0x00005457
		[XmlAttribute(AttributeName = "tag")]
		public string Tag { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00007260 File Offset: 0x00005460
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00007268 File Offset: 0x00005468
		[XmlText]
		public string Value { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00007271 File Offset: 0x00005471
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00007279 File Offset: 0x00005479
		[XmlAttribute(AttributeName = "manufacturer")]
		public string Manufacturer { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00007282 File Offset: 0x00005482
		// (set) Token: 0x06000118 RID: 280 RVA: 0x0000728A File Offset: 0x0000548A
		[XmlAttribute(AttributeName = "family")]
		public string Family { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00007293 File Offset: 0x00005493
		// (set) Token: 0x0600011A RID: 282 RVA: 0x0000729B File Offset: 0x0000549B
		[XmlAttribute(AttributeName = "mtm")]
		public string Mtm { get; set; }

		// Token: 0x0600011B RID: 283 RVA: 0x000072A4 File Offset: 0x000054A4
		public bool? GetValueAsBool()
		{
			bool? result = null;
			try
			{
				if (!string.IsNullOrWhiteSpace(this.Value))
				{
					string text = this.Value.Trim();
					bool value;
					if (bool.TryParse(text, out value))
					{
						result = new bool?(value);
					}
					else if (text == "0")
					{
						result = new bool?(false);
					}
					else if (text == "1")
					{
						result = new bool?(true);
					}
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00007328 File Offset: 0x00005528
		public int? GetValueAsInt()
		{
			int? result = null;
			try
			{
				int value;
				if (this.Value != null && int.TryParse(this.Value.Trim(), out value))
				{
					result = new int?(value);
				}
			}
			catch (Exception)
			{
			}
			return result;
		}
	}
}
