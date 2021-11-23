using System;
using System.Xml.Serialization;
using Lenovo.Modern.CoreTypes.Models;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging
{
	// Token: 0x02000078 RID: 120
	[XmlRoot(ElementName = "MessageFilter", Namespace = null)]
	public sealed class MessageFilter : IAdvancedApplicabilityFilter
	{
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x00006FCB File Offset: 0x000051CB
		// (set) Token: 0x060004D7 RID: 1239 RVA: 0x00006FD3 File Offset: 0x000051D3
		[XmlArray("BrandList")]
		[XmlArrayItem("Brand")]
		public NegatableBrand[] BrandList { get; set; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00006FDC File Offset: 0x000051DC
		// (set) Token: 0x060004D9 RID: 1241 RVA: 0x00006FE4 File Offset: 0x000051E4
		[XmlArray("FamilyList")]
		[XmlArrayItem("Family")]
		public NegatableString[] FamilyList { get; set; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x00006FED File Offset: 0x000051ED
		// (set) Token: 0x060004DB RID: 1243 RVA: 0x00006FF5 File Offset: 0x000051F5
		[XmlArray("MTMList")]
		[XmlArrayItem("MTM")]
		public NegatableString[] MtmList { get; set; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00006FFE File Offset: 0x000051FE
		// (set) Token: 0x060004DD RID: 1245 RVA: 0x00007006 File Offset: 0x00005206
		[XmlArray("SubBrandList")]
		[XmlArrayItem("SubBrand")]
		public NegatableString[] SubBrandList { get; set; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x0000700F File Offset: 0x0000520F
		// (set) Token: 0x060004DF RID: 1247 RVA: 0x00007017 File Offset: 0x00005217
		[XmlArray("LangList")]
		[XmlArrayItem("Language")]
		public NegatableString[] LangList { get; set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x00007020 File Offset: 0x00005220
		// (set) Token: 0x060004E1 RID: 1249 RVA: 0x00007028 File Offset: 0x00005228
		[XmlArray("CountryList")]
		[XmlArrayItem("Country")]
		public NegatableString[] CountryList { get; set; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00007031 File Offset: 0x00005231
		// (set) Token: 0x060004E3 RID: 1251 RVA: 0x00007039 File Offset: 0x00005239
		[XmlArray("TagList")]
		[XmlArrayItem("Tag")]
		public NegatableKeyValuePair[] TagList { get; set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x00007042 File Offset: 0x00005242
		// (set) Token: 0x060004E5 RID: 1253 RVA: 0x0000704A File Offset: 0x0000524A
		[XmlElement(ElementName = "Target", IsNullable = true)]
		public string Target { get; set; }
	}
}
