using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x0200002E RID: 46
	public sealed class FailureData
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00004CCC File Offset: 0x00002ECC
		// (set) Token: 0x0600011C RID: 284 RVA: 0x00004CD4 File Offset: 0x00002ED4
		[XmlElement(ElementName = "ResultCodeGroup")]
		public string ResultCodeGroup { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00004CDD File Offset: 0x00002EDD
		// (set) Token: 0x0600011E RID: 286 RVA: 0x00004CE5 File Offset: 0x00002EE5
		[XmlElement(ElementName = "ResultCode")]
		public int ResultCode { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00004CEE File Offset: 0x00002EEE
		// (set) Token: 0x06000120 RID: 288 RVA: 0x00004CF6 File Offset: 0x00002EF6
		[XmlElement(ElementName = "ResultDescription")]
		public string ResultDescription { get; set; }
	}
}
