using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging
{
	// Token: 0x02000079 RID: 121
	public sealed class MessageHeaderType
	{
		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x00007053 File Offset: 0x00005253
		// (set) Token: 0x060004E8 RID: 1256 RVA: 0x0000705B File Offset: 0x0000525B
		[XmlAttribute(AttributeName = "MessageId")]
		public Guid MessageId { get; set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00007064 File Offset: 0x00005264
		// (set) Token: 0x060004EA RID: 1258 RVA: 0x0000706C File Offset: 0x0000526C
		[XmlAttribute(AttributeName = "FriendlyName")]
		public string FriendlyName { get; set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00007075 File Offset: 0x00005275
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x0000707D File Offset: 0x0000527D
		[XmlIgnore]
		public DateTimeOffset DateCreated { get; set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x00007088 File Offset: 0x00005288
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x000070C2 File Offset: 0x000052C2
		[XmlElement("DateCreated", IsNullable = true)]
		public string XmlDateCreatedDoNotUse
		{
			get
			{
				string result = string.Empty;
				if (this.DateCreated != DateTimeOffset.MinValue)
				{
					result = this.DateCreated.ToString(Constants.DateFormat);
				}
				return result;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					this.DateCreated = DateTimeOffset.Parse(value);
				}
			}
		}
	}
}
