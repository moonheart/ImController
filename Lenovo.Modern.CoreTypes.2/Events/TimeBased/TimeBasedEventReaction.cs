using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.TimeBased
{
	// Token: 0x0200002C RID: 44
	[XmlRoot(ElementName = "TimeBasedEventReaction", Namespace = null)]
	public sealed class TimeBasedEventReaction
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000497D File Offset: 0x00002B7D
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x00004985 File Offset: 0x00002B85
		[XmlElement(ElementName = "FriendlyName")]
		public string FriendlyName { get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000498E File Offset: 0x00002B8E
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x00004996 File Offset: 0x00002B96
		[XmlElement(ElementName = "OOBEProximity")]
		public bool OOBEProximity { get; set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000499F File Offset: 0x00002B9F
		// (set) Token: 0x060001CB RID: 459 RVA: 0x000049A7 File Offset: 0x00002BA7
		[XmlIgnore]
		public DateTimeOffset StartDateTime { get; set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060001CC RID: 460 RVA: 0x000049B0 File Offset: 0x00002BB0
		// (set) Token: 0x060001CD RID: 461 RVA: 0x000049EF File Offset: 0x00002BEF
		[XmlElement("StartDateTime", IsNullable = true)]
		public string XmlDateCreatedDoNotUse
		{
			get
			{
				string result = string.Empty;
				if (this.StartDateTime != DateTimeOffset.MinValue)
				{
					result = this.StartDateTime.ToString(Constants.DateTimeFormat, CultureInfo.InvariantCulture);
				}
				return result;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					this.StartDateTime = DateTimeOffset.Parse(value);
				}
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00004A05 File Offset: 0x00002C05
		// (set) Token: 0x060001CF RID: 463 RVA: 0x00004A0D File Offset: 0x00002C0D
		[XmlElement(ElementName = "RepeatIntervalUnit")]
		public RepeatIntervalUnitEnum RepeatIntervalUnit { get; set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00004A16 File Offset: 0x00002C16
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x00004A1E File Offset: 0x00002C1E
		[XmlElement(ElementName = "RepeatInterval")]
		public short RepeatInterval { get; set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00004A27 File Offset: 0x00002C27
		// (set) Token: 0x060001D3 RID: 467 RVA: 0x00004A2F File Offset: 0x00002C2F
		[XmlElement(ElementName = "OffsetForFirstEvent")]
		public short OffsetForFirstEvent { get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x00004A38 File Offset: 0x00002C38
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x00004A40 File Offset: 0x00002C40
		[XmlElement(ElementName = "DayForWeeklyEvent")]
		public DayOfTheWeekEnum DayForWeeklyEvent { get; set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x00004A49 File Offset: 0x00002C49
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x00004A51 File Offset: 0x00002C51
		[XmlArray("DatesForMonthlyEventList")]
		[XmlArrayItem("DatesForMonthlyEvent")]
		public int[] DatesForMonthlyEvent { get; set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x00004A5A File Offset: 0x00002C5A
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x00004A62 File Offset: 0x00002C62
		[XmlElement(ElementName = "RequireNetworkConnection")]
		public bool RequireNetworkConnection { get; set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00004A6B File Offset: 0x00002C6B
		// (set) Token: 0x060001DB RID: 475 RVA: 0x00004A73 File Offset: 0x00002C73
		[XmlElement(ElementName = "RandomDelay")]
		public string RandomDelay { get; set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060001DC RID: 476 RVA: 0x00004A7C File Offset: 0x00002C7C
		// (set) Token: 0x060001DD RID: 477 RVA: 0x00004A84 File Offset: 0x00002C84
		[XmlElement(ElementName = "RepeatTaskInterval")]
		public string RepeatTaskInterval { get; set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00004A8D File Offset: 0x00002C8D
		// (set) Token: 0x060001DF RID: 479 RVA: 0x00004A95 File Offset: 0x00002C95
		[XmlElement(ElementName = "RepeatTaskDuration")]
		public string RepeatTaskDuration { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00004A9E File Offset: 0x00002C9E
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x00004AA6 File Offset: 0x00002CA6
		[XmlElement(ElementName = "StopTaskAtEndDuration")]
		public bool StopTaskAtEndDuration { get; set; }
	}
}
