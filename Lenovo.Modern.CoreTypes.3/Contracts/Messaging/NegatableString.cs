using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging
{
	// Token: 0x0200007C RID: 124
	public sealed class NegatableString
	{
		// Token: 0x06000513 RID: 1299 RVA: 0x0000730E File Offset: 0x0000550E
		public NegatableString()
			: this(false, string.Empty)
		{
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0000731C File Offset: 0x0000551C
		public NegatableString(bool exclude, string stringValue)
		{
			this.Exclude = exclude;
			this.Value = stringValue;
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x00007332 File Offset: 0x00005532
		// (set) Token: 0x06000516 RID: 1302 RVA: 0x0000733A File Offset: 0x0000553A
		[XmlAttribute(AttributeName = "exclude")]
		public bool Exclude { get; set; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x00007343 File Offset: 0x00005543
		// (set) Token: 0x06000518 RID: 1304 RVA: 0x0000734B File Offset: 0x0000554B
		[XmlText]
		public string Value { get; set; }
	}
}
