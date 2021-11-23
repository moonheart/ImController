using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.Subscription
{
	// Token: 0x02000015 RID: 21
	[XmlRoot("FileTransferList")]
	public sealed class FileCData
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00003E8A File Offset: 0x0000208A
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00003E92 File Offset: 0x00002092
		[XmlElement("TransferInfo")]
		public IEnumerable<TransferInfo> FileTransferList { get; set; }
	}
}
