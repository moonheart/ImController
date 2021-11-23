using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging
{
	// Token: 0x0200007A RID: 122
	public sealed class MessageHistory
	{
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x000070D8 File Offset: 0x000052D8
		// (set) Token: 0x060004F1 RID: 1265 RVA: 0x000070E0 File Offset: 0x000052E0
		[XmlAttribute(AttributeName = "TimesClicked")]
		public int TimesClicked { get; set; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x000070E9 File Offset: 0x000052E9
		// (set) Token: 0x060004F3 RID: 1267 RVA: 0x000070F1 File Offset: 0x000052F1
		[XmlAttribute(AttributeName = "TimesToasted")]
		public int TimesToasted { get; set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x000070FA File Offset: 0x000052FA
		// (set) Token: 0x060004F5 RID: 1269 RVA: 0x00007102 File Offset: 0x00005302
		[XmlAttribute(AttributeName = "TimesAdded")]
		public int TimesAdded { get; set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x0000710B File Offset: 0x0000530B
		// (set) Token: 0x060004F7 RID: 1271 RVA: 0x00007113 File Offset: 0x00005313
		[XmlAttribute(AttributeName = "TimesDisplayed")]
		public int TimesDisplayed { get; set; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x0000711C File Offset: 0x0000531C
		// (set) Token: 0x060004F9 RID: 1273 RVA: 0x00007124 File Offset: 0x00005324
		[XmlAttribute(AttributeName = "TimesRemoved")]
		public int TimesRemoved { get; set; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x0000712D File Offset: 0x0000532D
		// (set) Token: 0x060004FB RID: 1275 RVA: 0x00007135 File Offset: 0x00005335
		[XmlAttribute(AttributeName = "HasBeenDisplayed")]
		public bool HasBeenDisplayed { get; set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x0000713E File Offset: 0x0000533E
		// (set) Token: 0x060004FD RID: 1277 RVA: 0x00007146 File Offset: 0x00005346
		[XmlIgnore]
		public DateTimeOffset DateAdded { get; set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x00007150 File Offset: 0x00005350
		// (set) Token: 0x060004FF RID: 1279 RVA: 0x0000718A File Offset: 0x0000538A
		[XmlElement("DateAdded", IsNullable = true)]
		public string XmlDateAddedDoNotUse
		{
			get
			{
				string result = string.Empty;
				if (this.DateAdded != DateTimeOffset.MinValue)
				{
					result = this.DateAdded.ToString(Constants.DateFormat);
				}
				return result;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					this.DateAdded = DateTimeOffset.Parse(value);
				}
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x000071A0 File Offset: 0x000053A0
		// (set) Token: 0x06000501 RID: 1281 RVA: 0x000071A8 File Offset: 0x000053A8
		[XmlIgnore]
		public DateTimeOffset DateRemoved { get; set; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x000071B4 File Offset: 0x000053B4
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x000071EE File Offset: 0x000053EE
		[XmlElement("DateRemoved", IsNullable = true)]
		public string XmlDateRemovedDoNotUse
		{
			get
			{
				string result = string.Empty;
				if (this.DateRemoved != DateTimeOffset.MinValue)
				{
					result = this.DateRemoved.ToString(Constants.DateFormat);
				}
				return result;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					this.DateRemoved = DateTimeOffset.Parse(value);
				}
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x00007204 File Offset: 0x00005404
		// (set) Token: 0x06000505 RID: 1285 RVA: 0x0000720C File Offset: 0x0000540C
		[XmlIgnore]
		public DateTimeOffset DateLastToasted { get; set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00007218 File Offset: 0x00005418
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x00007252 File Offset: 0x00005452
		[XmlElement("DateLastToasted", IsNullable = true)]
		public string XmlDateLastToastedDoNotUse
		{
			get
			{
				string result = string.Empty;
				if (this.DateLastToasted != DateTimeOffset.MinValue)
				{
					result = this.DateLastToasted.ToString(Constants.DateFormat);
				}
				return result;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					this.DateLastToasted = DateTimeOffset.Parse(value);
				}
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00007268 File Offset: 0x00005468
		// (set) Token: 0x06000509 RID: 1289 RVA: 0x00007270 File Offset: 0x00005470
		[XmlIgnore]
		public DateTimeOffset DateLastClicked { get; set; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x0000727C File Offset: 0x0000547C
		// (set) Token: 0x0600050B RID: 1291 RVA: 0x000072B6 File Offset: 0x000054B6
		[XmlElement("DateLastClicked", IsNullable = true)]
		public string XmlDateLastClickedDoNotUse
		{
			get
			{
				string result = string.Empty;
				if (this.DateLastClicked != DateTimeOffset.MinValue)
				{
					result = this.DateLastClicked.ToString(Constants.DateFormat);
				}
				return result;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					this.DateLastClicked = DateTimeOffset.Parse(value);
				}
			}
		}
	}
}
