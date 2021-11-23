using System;
using System.Globalization;
using System.Xml.Serialization;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;
using Lenovo.Modern.CoreTypes.Models.AppTagList;
using Lenovo.Modern.CoreTypes.Services;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging
{
	// Token: 0x02000075 RID: 117
	[XmlRoot(ElementName = "LenovoMessage", Namespace = null)]
	public sealed class LenovoMessage
	{
		// Token: 0x0600047D RID: 1149 RVA: 0x000066D1 File Offset: 0x000048D1
		public LenovoMessage()
			: this(Guid.NewGuid())
		{
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000066DE File Offset: 0x000048DE
		public LenovoMessage(Guid id)
		{
			this.Id = id;
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x000066ED File Offset: 0x000048ED
		// (set) Token: 0x06000480 RID: 1152 RVA: 0x000066F5 File Offset: 0x000048F5
		[XmlElement(ElementName = "MessageId", Order = 0)]
		public Guid Id { get; set; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x000066FE File Offset: 0x000048FE
		// (set) Token: 0x06000482 RID: 1154 RVA: 0x00006706 File Offset: 0x00004906
		[XmlElement(ElementName = "FriendlyName", Order = 1, IsNullable = true)]
		public string FriendlyName { get; set; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x0000670F File Offset: 0x0000490F
		// (set) Token: 0x06000484 RID: 1156 RVA: 0x00006717 File Offset: 0x00004917
		[XmlElement(ElementName = "Publisher", Order = 2, IsNullable = true)]
		public string Publisher { get; set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x00006720 File Offset: 0x00004920
		// (set) Token: 0x06000486 RID: 1158 RVA: 0x00006728 File Offset: 0x00004928
		[XmlIgnore]
		public DateTimeOffset DateCreated { get; set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x00006734 File Offset: 0x00004934
		// (set) Token: 0x06000488 RID: 1160 RVA: 0x0000676E File Offset: 0x0000496E
		[XmlElement(ElementName = "DateCreated", Order = 3, IsNullable = true)]
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

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x00006784 File Offset: 0x00004984
		// (set) Token: 0x0600048A RID: 1162 RVA: 0x0000678C File Offset: 0x0000498C
		[XmlIgnore]
		public DateTimeOffset DateExpired { get; set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x00006798 File Offset: 0x00004998
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x000067D2 File Offset: 0x000049D2
		[XmlElement(ElementName = "DateExpired", Order = 4, IsNullable = true)]
		public string XmlDateExpiredDoNotUse
		{
			get
			{
				string result = string.Empty;
				if (this.DateExpired != DateTimeOffset.MinValue)
				{
					result = this.DateExpired.ToString(Constants.DateFormat);
				}
				return result;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					this.DateExpired = DateTimeOffset.Parse(value);
				}
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x000067E8 File Offset: 0x000049E8
		// (set) Token: 0x0600048E RID: 1166 RVA: 0x000067F0 File Offset: 0x000049F0
		[XmlElement(ElementName = "MessageCategory", Order = 5)]
		public MessageType MessageCategory { get; set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x000067F9 File Offset: 0x000049F9
		// (set) Token: 0x06000490 RID: 1168 RVA: 0x00006801 File Offset: 0x00004A01
		[XmlElement(ElementName = "Priority", Order = 6)]
		public MessagePriority Priority { get; set; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x0000680A File Offset: 0x00004A0A
		// (set) Token: 0x06000492 RID: 1170 RVA: 0x00006812 File Offset: 0x00004A12
		[XmlElement(ElementName = "MessageFilter", Order = 7)]
		public MessageFilter MessageFilter { get; set; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0000681B File Offset: 0x00004A1B
		// (set) Token: 0x06000494 RID: 1172 RVA: 0x00006823 File Offset: 0x00004A23
		[XmlArray("MessageActionList", Order = 8)]
		[XmlArrayItem("MessageAction")]
		public MessageAction[] MessageActionList { get; set; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0000682C File Offset: 0x00004A2C
		// (set) Token: 0x06000496 RID: 1174 RVA: 0x00006834 File Offset: 0x00004A34
		[XmlElement(ElementName = "MessageContent", Order = 9)]
		public MessageContent MessageContent { get; set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x0000683D File Offset: 0x00004A3D
		// (set) Token: 0x06000498 RID: 1176 RVA: 0x00006845 File Offset: 0x00004A45
		[XmlElement(ElementName = "Interval", Order = 10)]
		public int Interval { get; set; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0000684E File Offset: 0x00004A4E
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x00006856 File Offset: 0x00004A56
		[XmlElement(ElementName = "Dates", Order = 11)]
		public Dates Dates { get; set; }

		// Token: 0x0600049B RID: 1179 RVA: 0x0000685F File Offset: 0x00004A5F
		public bool IsValid()
		{
			return this.DateExpired != DateTimeOffset.MinValue && this.DateCreated != DateTimeOffset.MinValue && this.MessageContent != null;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00006890 File Offset: 0x00004A90
		public static bool IsApplicable(LenovoMessage message, MachineInformation machineInformation, AppAndTagCollection appTagCollection, MessageHistory messageHistory)
		{
			if (message == null || machineInformation == null)
			{
				throw new ArgumentNullException("Cannot provide null instances of message or machineInformation");
			}
			if (message.DateExpired == DateTimeOffset.MinValue || message.DateCreated == DateTimeOffset.MinValue || message.Id == Guid.Empty)
			{
				throw new NotSupportedException("Item does not meet basic inspection requirements");
			}
			DateTime dateTime = message.DateCreated.DateTime;
			DateTime dateTime2 = message.DateExpired.DateTime;
			if (dateTime > DateTime.Now || dateTime2 < DateTime.Now)
			{
				throw new NotSupportedException(string.Format("Item does not meet basic date reqs.  DateCreated: {0}, DateExpired: {1}, Now: {2}", message.DateCreated, message.DateExpired, DateTime.Now));
			}
			LenovoMessage.AssertMessageNotBlockedByIntervalHistory(message, messageHistory);
			LenovoMessage.AssertMessageNotBlockedByOobeProximity(message, machineInformation);
			return EligibilityFilter.IsApplicableAdvanced(message.MessageFilter, machineInformation, appTagCollection);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00006974 File Offset: 0x00004B74
		public static void AssertMessageNotBlockedByOobeProximity(LenovoMessage message, MachineInformation machineInformation)
		{
			if (message.Dates != null && (message.Dates.OobeProximityDaysMin != null || message.Dates.OobeProximityDaysMax != null))
			{
				if (machineInformation.FirstRunDate == DateTimeOffset.MinValue)
				{
					throw new NotSupportedException("Item requires OOBE Proximity but MachineInformation doesn't have a value");
				}
				DateTimeOffset firstRunDate = machineInformation.FirstRunDate;
				int num = (int)Math.Floor(DateTimeOffset.Now.Subtract(firstRunDate).TotalDays);
				if (message.Dates.OobeProximityDaysMin != null)
				{
					int num2 = num;
					int? num3 = message.Dates.OobeProximityDaysMin;
					if ((num2 < num3.GetValueOrDefault()) & (num3 != null))
					{
						throw new NotSupportedException(string.Format("Item requires {0} minimum days after oobe, but actual is {1}", message.Dates.OobeProximityDaysMin, num));
					}
				}
				if (message.Dates.OobeProximityDaysMax != null)
				{
					int num4 = num;
					int? num3 = message.Dates.OobeProximityDaysMax;
					if ((num4 > num3.GetValueOrDefault()) & (num3 != null))
					{
						throw new NotSupportedException(string.Format("Item requires {0} maximum days after oobe, but actual is {1}", message.Dates.OobeProximityDaysMax, num));
					}
				}
			}
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00006AB4 File Offset: 0x00004CB4
		public static bool AssertMessageNotBlockedByIntervalHistory(LenovoMessage message, MessageHistory history)
		{
			bool result = false;
			if (message != null && history != null)
			{
				if (message.Interval < 0)
				{
					if (history.DateRemoved.Date != DateTime.MinValue.Date || history.TimesRemoved > 0 || history.TimesClicked > 0)
					{
						throw new NotSupportedException(string.Format("Item interval is {0}, yet has already been acted on {1} times and removed  {2} times", message.Interval, history.TimesClicked, history.TimesRemoved));
					}
				}
				else if (message.Interval == 0)
				{
					result = false;
				}
				else if (message.Interval > 0)
				{
					if (history.DateRemoved.Date != DateTimeOffset.MinValue.Date)
					{
						double totalDays = DateTimeOffset.Now.Subtract(history.DateRemoved).TotalDays;
						if (totalDays < (double)message.Interval)
						{
							throw new NotSupportedException(string.Format("Item interval is {0}, yet item removed {1} days ago", message.Interval, totalDays));
						}
					}
					if (history.DateLastClicked.Date != DateTimeOffset.MinValue.Date)
					{
						double totalDays2 = DateTimeOffset.Now.Subtract(history.DateLastClicked).TotalDays;
						if (totalDays2 < (double)message.Interval)
						{
							throw new NotSupportedException(string.Format("Item interval is {0}, yet item was clicked on {1} days ago", message.Interval, totalDays2));
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00006C3C File Offset: 0x00004E3C
		internal static DateTime ConvertStringToDate(string dateStamp)
		{
			DateTime dateTime = DateTime.MinValue;
			if (!string.IsNullOrWhiteSpace(dateStamp))
			{
				try
				{
					dateTime = DateTime.ParseExact(dateStamp, Constants.DateFormat, CultureInfo.InvariantCulture);
				}
				catch (Exception)
				{
				}
				if (dateTime == DateTime.MinValue)
				{
					try
					{
						dateTime = DateTime.ParseExact(dateStamp, Constants.DateTimeFormat, CultureInfo.InvariantCulture);
					}
					catch (Exception)
					{
					}
				}
			}
			return dateTime;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00006CB0 File Offset: 0x00004EB0
		public sealed override bool Equals(object obj)
		{
			bool result = false;
			LenovoMessage lenovoMessage = obj as LenovoMessage;
			if (lenovoMessage != null)
			{
				result = this.Id.Equals(lenovoMessage.Id);
			}
			return result;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00006CE0 File Offset: 0x00004EE0
		public sealed override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00006D01 File Offset: 0x00004F01
		public sealed override string ToString()
		{
			if (!string.IsNullOrWhiteSpace(this.FriendlyName))
			{
				return this.FriendlyName;
			}
			return "[NULL]";
		}
	}
}
