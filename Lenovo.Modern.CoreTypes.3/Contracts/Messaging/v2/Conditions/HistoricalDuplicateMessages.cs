using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Conditions
{
	// Token: 0x02000093 RID: 147
	public sealed class HistoricalDuplicateMessages
	{
		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x000079E0 File Offset: 0x00005BE0
		// (set) Token: 0x060005D7 RID: 1495 RVA: 0x000079E8 File Offset: 0x00005BE8
		[XmlText]
		public string Value { get; set; }
	}
}
