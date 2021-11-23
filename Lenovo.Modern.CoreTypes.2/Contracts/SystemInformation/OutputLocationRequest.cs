using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation
{
	// Token: 0x02000046 RID: 70
	[XmlRoot(ElementName = "OutputLocationRequest")]
	public sealed class OutputLocationRequest
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x000058CC File Offset: 0x00003ACC
		// (set) Token: 0x060002F8 RID: 760 RVA: 0x000058D4 File Offset: 0x00003AD4
		[XmlAttribute(AttributeName = "Format")]
		public string Format { get; set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x000058DD File Offset: 0x00003ADD
		// (set) Token: 0x060002FA RID: 762 RVA: 0x000058E5 File Offset: 0x00003AE5
		[XmlAttribute(AttributeName = "Location")]
		public string Location { get; set; }
	}
}
