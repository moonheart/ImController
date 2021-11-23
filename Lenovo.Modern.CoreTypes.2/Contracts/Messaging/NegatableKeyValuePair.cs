using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging
{
	// Token: 0x0200007D RID: 125
	public sealed class NegatableKeyValuePair
	{
		// Token: 0x06000519 RID: 1305 RVA: 0x00007354 File Offset: 0x00005554
		public NegatableKeyValuePair()
			: this(false, null, string.Empty)
		{
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00007363 File Offset: 0x00005563
		public NegatableKeyValuePair(bool exclude, string key, string negatableKeyValue)
		{
			this.Exclude = exclude;
			this.Value = negatableKeyValue;
			this.Key = key;
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x00007380 File Offset: 0x00005580
		// (set) Token: 0x0600051C RID: 1308 RVA: 0x00007388 File Offset: 0x00005588
		[XmlAttribute(AttributeName = "exclude")]
		public bool Exclude { get; set; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00007391 File Offset: 0x00005591
		// (set) Token: 0x0600051E RID: 1310 RVA: 0x00007399 File Offset: 0x00005599
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x000073A2 File Offset: 0x000055A2
		// (set) Token: 0x06000520 RID: 1312 RVA: 0x000073AA File Offset: 0x000055AA
		[XmlText]
		public string Key { get; set; }
	}
}
