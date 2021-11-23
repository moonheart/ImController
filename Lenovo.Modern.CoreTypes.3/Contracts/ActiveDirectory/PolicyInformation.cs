using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.ActiveDirectory
{
	// Token: 0x020000CC RID: 204
	[XmlRoot(ElementName = "PolicyInformation", Namespace = null)]
	public sealed class PolicyInformation
	{
		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x00008671 File Offset: 0x00006871
		// (set) Token: 0x06000746 RID: 1862 RVA: 0x00008679 File Offset: 0x00006879
		[XmlAttribute(AttributeName = "AppName")]
		public string AppName { get; set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x00008682 File Offset: 0x00006882
		// (set) Token: 0x06000748 RID: 1864 RVA: 0x0000868A File Offset: 0x0000688A
		[XmlAttribute(AttributeName = "OutputPath")]
		public string OutputPath { get; set; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x00008693 File Offset: 0x00006893
		// (set) Token: 0x0600074A RID: 1866 RVA: 0x0000869B File Offset: 0x0000689B
		[XmlElement("PolicyList")]
		public PolicyList[] PolicyList { get; set; }
	}
}
