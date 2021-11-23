using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x02000021 RID: 33
	public sealed class BrokerAuthentication
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x0000486F File Offset: 0x00002A6F
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00004877 File Offset: 0x00002A77
		[XmlElement(ElementName = "Token", IsNullable = true)]
		public string Token { get; set; }
	}
}
