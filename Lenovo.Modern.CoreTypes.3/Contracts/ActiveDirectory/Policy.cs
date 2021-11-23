using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ActiveDirectory
{
	// Token: 0x020000CB RID: 203
	[XmlRoot(ElementName = "Policy", Namespace = null)]
	public sealed class Policy
	{
		// Token: 0x1700033B RID: 827
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0000862D File Offset: 0x0000682D
		// (set) Token: 0x0600073D RID: 1853 RVA: 0x00008635 File Offset: 0x00006835
		[XmlAttribute(AttributeName = "Type")]
		public string Type { get; set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0000863E File Offset: 0x0000683E
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x00008646 File Offset: 0x00006846
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x0000864F File Offset: 0x0000684F
		// (set) Token: 0x06000741 RID: 1857 RVA: 0x00008657 File Offset: 0x00006857
		[XmlAttribute(AttributeName = "Value")]
		public string Value { get; set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x00008660 File Offset: 0x00006860
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x00008668 File Offset: 0x00006868
		[XmlAttribute(AttributeName = "Location")]
		public string Location { get; set; }
	}
}
