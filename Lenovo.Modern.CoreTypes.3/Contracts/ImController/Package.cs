using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ImController
{
	// Token: 0x020000B0 RID: 176
	public sealed class Package
	{
		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x00008069 File Offset: 0x00006269
		// (set) Token: 0x0600069F RID: 1695 RVA: 0x00008071 File Offset: 0x00006271
		[XmlAttribute("name")]
		public string name { get; set; }

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x0000807A File Offset: 0x0000627A
		// (set) Token: 0x060006A1 RID: 1697 RVA: 0x00008082 File Offset: 0x00006282
		[XmlAttribute("version")]
		public string version { get; set; }

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x0000808B File Offset: 0x0000628B
		// (set) Token: 0x060006A3 RID: 1699 RVA: 0x00008093 File Offset: 0x00006293
		[XmlAttribute("lastModified")]
		public string lastModified { get; set; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x0000809C File Offset: 0x0000629C
		// (set) Token: 0x060006A5 RID: 1701 RVA: 0x000080A4 File Offset: 0x000062A4
		[XmlAttribute("rebootRequired")]
		public bool rebootRequired { get; set; }

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x000080AD File Offset: 0x000062AD
		// (set) Token: 0x060006A7 RID: 1703 RVA: 0x000080B5 File Offset: 0x000062B5
		[XmlAttribute("status")]
		public string status { get; set; }

		// Token: 0x060006A8 RID: 1704 RVA: 0x000080BE File Offset: 0x000062BE
		public Package()
		{
			this.status = "pending";
		}
	}
}
