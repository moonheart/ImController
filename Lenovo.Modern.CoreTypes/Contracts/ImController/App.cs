using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ImController
{
	// Token: 0x020000A8 RID: 168
	public sealed class App
	{
		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x00007CBB File Offset: 0x00005EBB
		// (set) Token: 0x06000644 RID: 1604 RVA: 0x00007CC3 File Offset: 0x00005EC3
		[XmlAttribute("name")]
		public string name { get; set; }

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x00007CCC File Offset: 0x00005ECC
		// (set) Token: 0x06000646 RID: 1606 RVA: 0x00007CD4 File Offset: 0x00005ED4
		[XmlArray("CampaignTagList")]
		[XmlArrayItem("CampaignTag")]
		public string[] CampaignTagList { get; set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00007CDD File Offset: 0x00005EDD
		// (set) Token: 0x06000648 RID: 1608 RVA: 0x00007CE5 File Offset: 0x00005EE5
		[XmlAttribute("version")]
		public string version { get; set; }

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x00007CEE File Offset: 0x00005EEE
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x00007CF6 File Offset: 0x00005EF6
		[XmlAttribute("appID")]
		public string appID { get; set; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00007CFF File Offset: 0x00005EFF
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x00007D07 File Offset: 0x00005F07
		[XmlAttribute("partNum")]
		public string partNum { get; set; }

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00007D10 File Offset: 0x00005F10
		// (set) Token: 0x0600064E RID: 1614 RVA: 0x00007D18 File Offset: 0x00005F18
		[XmlAttribute("status")]
		public string status { get; set; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00007D21 File Offset: 0x00005F21
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x00007D29 File Offset: 0x00005F29
		[XmlAttribute("progress")]
		public string progress { get; set; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x00007D32 File Offset: 0x00005F32
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x00007D3A File Offset: 0x00005F3A
		[XmlAttribute("error")]
		public string error { get; set; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x00007D43 File Offset: 0x00005F43
		// (set) Token: 0x06000654 RID: 1620 RVA: 0x00007D4B File Offset: 0x00005F4B
		[XmlAttribute("verbose")]
		public bool verbose { get; set; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00007D54 File Offset: 0x00005F54
		// (set) Token: 0x06000656 RID: 1622 RVA: 0x00007D5C File Offset: 0x00005F5C
		[XmlAttribute("activationCode")]
		public string activationCode { get; set; }

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x00007D65 File Offset: 0x00005F65
		// (set) Token: 0x06000658 RID: 1624 RVA: 0x00007D6D File Offset: 0x00005F6D
		[XmlAttribute("redemptionURL")]
		public string redemptionURL { get; set; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00007D76 File Offset: 0x00005F76
		// (set) Token: 0x0600065A RID: 1626 RVA: 0x00007D7E File Offset: 0x00005F7E
		[XmlAttribute("licAgreementURL")]
		public string licAgreementURL { get; set; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x00007D87 File Offset: 0x00005F87
		// (set) Token: 0x0600065C RID: 1628 RVA: 0x00007D8F File Offset: 0x00005F8F
		[XmlAttribute("swHomePageURL")]
		public string swHomePageURL { get; set; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x00007D98 File Offset: 0x00005F98
		// (set) Token: 0x0600065E RID: 1630 RVA: 0x00007DA0 File Offset: 0x00005FA0
		[XmlAttribute("size")]
		public uint size { get; set; }
	}
}
