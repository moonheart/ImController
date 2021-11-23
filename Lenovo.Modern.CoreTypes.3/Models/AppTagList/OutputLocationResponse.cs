using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.AppTagList
{
	// Token: 0x0200001F RID: 31
	[XmlRoot(ElementName = "OutputLocationResponse", Namespace = null)]
	public sealed class OutputLocationResponse
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000163 RID: 355 RVA: 0x0000450C File Offset: 0x0000270C
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00004514 File Offset: 0x00002714
		[XmlAttribute(AttributeName = "Success")]
		public bool Success { get; set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000165 RID: 357 RVA: 0x0000451D File Offset: 0x0000271D
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00004525 File Offset: 0x00002725
		[XmlArray("LocationList")]
		[XmlArrayItem("Location")]
		public object[] Location { get; set; }
	}
}
