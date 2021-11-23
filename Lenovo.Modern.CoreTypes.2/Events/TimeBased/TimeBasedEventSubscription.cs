using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.TimeBased
{
	// Token: 0x0200002D RID: 45
	[XmlRoot(ElementName = "TimeBasedEventSubscription", Namespace = null)]
	public sealed class TimeBasedEventSubscription
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00004AAF File Offset: 0x00002CAF
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x00004AB7 File Offset: 0x00002CB7
		[XmlElement(ElementName = "FriendlyName")]
		public string FriendlyName { get; set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00004AC0 File Offset: 0x00002CC0
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x00004AC8 File Offset: 0x00002CC8
		[XmlElement(ElementName = "OOBEProximity")]
		public bool OOBEProximity { get; set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00004AD1 File Offset: 0x00002CD1
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00004AD9 File Offset: 0x00002CD9
		[XmlIgnore]
		public DateTimeOffset StartDateTime { get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00004AE4 File Offset: 0x00002CE4
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00004B23 File Offset: 0x00002D23
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

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00004B39 File Offset: 0x00002D39
		// (set) Token: 0x060001EC RID: 492 RVA: 0x00004B41 File Offset: 0x00002D41
		[XmlElement(ElementName = "RepeatIntervalUnit")]
		public RepeatIntervalUnitEnum RepeatIntervalUnit { get; set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00004B4A File Offset: 0x00002D4A
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00004B52 File Offset: 0x00002D52
		[XmlElement(ElementName = "RepeatInterval")]
		public short RepeatInterval { get; set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00004B5B File Offset: 0x00002D5B
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x00004B63 File Offset: 0x00002D63
		[XmlElement(ElementName = "OffsetForFirstEvent")]
		public short OffsetForFirstEvent { get; set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00004B6C File Offset: 0x00002D6C
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x00004B74 File Offset: 0x00002D74
		[XmlElement(ElementName = "DayForWeeklyEvent")]
		public DayOfTheWeekEnum DayForWeeklyEvent { get; set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00004B7D File Offset: 0x00002D7D
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x00004B85 File Offset: 0x00002D85
		[XmlArray("DatesForMonthlyEventList")]
		[XmlArrayItem("DatesForMonthlyEvent")]
		public int[] DatesForMonthlyEvent { get; set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00004B8E File Offset: 0x00002D8E
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x00004B96 File Offset: 0x00002D96
		[XmlElement(ElementName = "RequireNetworkConnection")]
		public bool RequireNetworkConnection { get; set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x00004B9F File Offset: 0x00002D9F
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x00004BA7 File Offset: 0x00002DA7
		[XmlElement(ElementName = "RandomDelay")]
		public string RandomDelay { get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00004BB0 File Offset: 0x00002DB0
		// (set) Token: 0x060001FA RID: 506 RVA: 0x00004BB8 File Offset: 0x00002DB8
		[XmlElement(ElementName = "RepeatTaskInterval")]
		public string RepeatTaskInterval { get; set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060001FB RID: 507 RVA: 0x00004BC1 File Offset: 0x00002DC1
		// (set) Token: 0x060001FC RID: 508 RVA: 0x00004BC9 File Offset: 0x00002DC9
		[XmlElement(ElementName = "RepeatTaskDuration")]
		public string RepeatTaskDuration { get; set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00004BD2 File Offset: 0x00002DD2
		// (set) Token: 0x060001FE RID: 510 RVA: 0x00004BDA File Offset: 0x00002DDA
		[XmlElement(ElementName = "StopTaskAtEndDuration")]
		public bool StopTaskAtEndDuration { get; set; }
	}
}
