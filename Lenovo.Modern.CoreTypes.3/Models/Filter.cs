using System;
using System.Xml.Serialization;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;

namespace Lenovo.Modern.CoreTypes.Models
{
	// Token: 0x02000009 RID: 9
	public sealed class Filter : IBasicEligabilityRequirements
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00003411 File Offset: 0x00001611
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00003419 File Offset: 0x00001619
		[XmlIgnore]
		public BrandType Brand { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00003424 File Offset: 0x00001624
		// (set) Token: 0x06000020 RID: 32 RVA: 0x0000344C File Offset: 0x0000164C
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

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000034B1 File Offset: 0x000016B1
		// (set) Token: 0x06000022 RID: 34 RVA: 0x000034B9 File Offset: 0x000016B9
		[XmlAttribute(AttributeName = "country")]
		public string Country { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000034C2 File Offset: 0x000016C2
		// (set) Token: 0x06000024 RID: 36 RVA: 0x000034CA File Offset: 0x000016CA
		[XmlAttribute(AttributeName = "subbrand")]
		public string SubBrand { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000034D3 File Offset: 0x000016D3
		// (set) Token: 0x06000026 RID: 38 RVA: 0x000034DB File Offset: 0x000016DB
		[XmlAttribute(AttributeName = "language")]
		public string Language { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000034E4 File Offset: 0x000016E4
		// (set) Token: 0x06000028 RID: 40 RVA: 0x000034EC File Offset: 0x000016EC
		[XmlAttribute(AttributeName = "friendlyName")]
		public string FriendlyName { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000034F5 File Offset: 0x000016F5
		// (set) Token: 0x0600002A RID: 42 RVA: 0x000034FD File Offset: 0x000016FD
		[XmlIgnore]
		public EnclosureType EnclosureType { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00003508 File Offset: 0x00001708
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00003530 File Offset: 0x00001730
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

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00003595 File Offset: 0x00001795
		// (set) Token: 0x0600002E RID: 46 RVA: 0x0000359D File Offset: 0x0000179D
		[XmlAttribute(AttributeName = "appVersion")]
		public string AppVersion { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000035A6 File Offset: 0x000017A6
		// (set) Token: 0x06000030 RID: 48 RVA: 0x000035AE File Offset: 0x000017AE
		[XmlAttribute(AttributeName = "osVersion")]
		public string OsVersion { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000035B7 File Offset: 0x000017B7
		// (set) Token: 0x06000032 RID: 50 RVA: 0x000035BF File Offset: 0x000017BF
		[XmlAttribute(AttributeName = "osBitness")]
		public string OsBitness { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000035C8 File Offset: 0x000017C8
		// (set) Token: 0x06000034 RID: 52 RVA: 0x000035D0 File Offset: 0x000017D0
		[XmlAttribute(AttributeName = "tag")]
		public string Tag { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000035D9 File Offset: 0x000017D9
		// (set) Token: 0x06000036 RID: 54 RVA: 0x000035E1 File Offset: 0x000017E1
		[XmlText]
		public string Value { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000035EA File Offset: 0x000017EA
		// (set) Token: 0x06000038 RID: 56 RVA: 0x000035F2 File Offset: 0x000017F2
		[XmlAttribute(AttributeName = "manufacturer")]
		public string Manufacturer { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000035FB File Offset: 0x000017FB
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00003603 File Offset: 0x00001803
		[XmlAttribute(AttributeName = "family")]
		public string Family { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003B RID: 59 RVA: 0x0000360C File Offset: 0x0000180C
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00003614 File Offset: 0x00001814
		[XmlAttribute(AttributeName = "mtm")]
		public string Mtm { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003D RID: 61 RVA: 0x0000361D File Offset: 0x0000181D
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00003625 File Offset: 0x00001825
		[XmlAttribute(AttributeName = "upeTag")]
		public string UpeTag { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600003F RID: 63 RVA: 0x0000362E File Offset: 0x0000182E
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00003636 File Offset: 0x00001836
		[XmlAttribute(AttributeName = "audienceTest")]
		public string AudienceTest { get; set; }

		// Token: 0x06000041 RID: 65 RVA: 0x00003640 File Offset: 0x00001840
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

		// Token: 0x06000042 RID: 66 RVA: 0x000036C4 File Offset: 0x000018C4
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

		// Token: 0x06000043 RID: 67 RVA: 0x00003714 File Offset: 0x00001914
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Filter filter = obj as Filter;
			return filter != null && (this.AppVersion == filter.AppVersion && this.Country == filter.Country && this.Family == filter.Family && this.FriendlyName == filter.FriendlyName && this.Language == filter.Language && this.Manufacturer == filter.Manufacturer && this.Mtm == filter.Mtm && this.OsBitness == filter.OsBitness && this.OsVersion == filter.OsVersion && this.SubBrand == filter.SubBrand && this.Tag == filter.Tag && this.UpeTag == filter.UpeTag && this.Brand == filter.Brand) && this.EnclosureType == filter.EnclosureType;
		}
	}
}
