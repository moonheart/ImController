using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ContractBroker
{
	// Token: 0x02000007 RID: 7
	public sealed class App
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002251 File Offset: 0x00000451
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002259 File Offset: 0x00000459
		[XmlAttribute("name")]
		public string name { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002262 File Offset: 0x00000462
		// (set) Token: 0x06000022 RID: 34 RVA: 0x0000226A File Offset: 0x0000046A
		[XmlArray("CampaignTagList")]
		[XmlArrayItem("CampaignTag")]
		public string[] CampaignTagList { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002273 File Offset: 0x00000473
		// (set) Token: 0x06000024 RID: 36 RVA: 0x0000227B File Offset: 0x0000047B
		[XmlAttribute("version")]
		public string version { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002284 File Offset: 0x00000484
		// (set) Token: 0x06000026 RID: 38 RVA: 0x0000228C File Offset: 0x0000048C
		[XmlAttribute("appID")]
		public string appID { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002295 File Offset: 0x00000495
		// (set) Token: 0x06000028 RID: 40 RVA: 0x0000229D File Offset: 0x0000049D
		[XmlAttribute("partNum")]
		public string partNum { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000022A6 File Offset: 0x000004A6
		// (set) Token: 0x0600002A RID: 42 RVA: 0x000022AE File Offset: 0x000004AE
		[XmlAttribute("status")]
		public string status { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000022B7 File Offset: 0x000004B7
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000022BF File Offset: 0x000004BF
		[XmlAttribute("progress")]
		public string progress { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000022C8 File Offset: 0x000004C8
		// (set) Token: 0x0600002E RID: 46 RVA: 0x000022D0 File Offset: 0x000004D0
		[XmlAttribute("error")]
		public string error { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000022D9 File Offset: 0x000004D9
		// (set) Token: 0x06000030 RID: 48 RVA: 0x000022E1 File Offset: 0x000004E1
		[XmlAttribute("verbose")]
		public bool verbose { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000022EA File Offset: 0x000004EA
		// (set) Token: 0x06000032 RID: 50 RVA: 0x000022F2 File Offset: 0x000004F2
		[XmlAttribute("activationCode")]
		public string activationCode { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000022FB File Offset: 0x000004FB
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002303 File Offset: 0x00000503
		[XmlAttribute("redemptionURL")]
		public string redemptionURL { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000035 RID: 53 RVA: 0x0000230C File Offset: 0x0000050C
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00002314 File Offset: 0x00000514
		[XmlAttribute("licAgreementURL")]
		public string licAgreementURL { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000037 RID: 55 RVA: 0x0000231D File Offset: 0x0000051D
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002325 File Offset: 0x00000525
		[XmlAttribute("swHomePageURL")]
		public string swHomePageURL { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000039 RID: 57 RVA: 0x0000232E File Offset: 0x0000052E
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00002336 File Offset: 0x00000536
		[XmlAttribute("size")]
		public uint size { get; set; }
	}
}
