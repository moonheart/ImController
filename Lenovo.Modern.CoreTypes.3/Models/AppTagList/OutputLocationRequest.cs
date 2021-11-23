using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.AppTagList
{
	// Token: 0x0200001D RID: 29
	[XmlRoot(ElementName = "OutputLocationRequest", Namespace = null)]
	public sealed class OutputLocationRequest
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000157 RID: 343 RVA: 0x000044B7 File Offset: 0x000026B7
		// (set) Token: 0x06000158 RID: 344 RVA: 0x000044BF File Offset: 0x000026BF
		[XmlAttribute(AttributeName = "Format")]
		public string Format { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000159 RID: 345 RVA: 0x000044C8 File Offset: 0x000026C8
		// (set) Token: 0x0600015A RID: 346 RVA: 0x000044D0 File Offset: 0x000026D0
		[XmlArray("LocationList")]
		[XmlArrayItem("Location")]
		public object[] Location { get; set; }
	}
}
