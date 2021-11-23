using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.TimeBased
{
	// Token: 0x0200002A RID: 42
	public enum DayOfTheWeekEnum
	{
		// Token: 0x040000B0 RID: 176
		[XmlEnum(Name = "Sunday")]
		Sunday = 1,
		// Token: 0x040000B1 RID: 177
		[XmlEnum(Name = "Monday")]
		Monday,
		// Token: 0x040000B2 RID: 178
		[XmlEnum(Name = "Tuesday")]
		Tuesday = 4,
		// Token: 0x040000B3 RID: 179
		[XmlEnum(Name = "Wednesday")]
		Wednesday = 8,
		// Token: 0x040000B4 RID: 180
		[XmlEnum(Name = "Thursday")]
		Thursday = 16,
		// Token: 0x040000B5 RID: 181
		[XmlEnum(Name = "Friday")]
		Friday = 32,
		// Token: 0x040000B6 RID: 182
		[XmlEnum(Name = "Saturday")]
		Saturday = 64,
		// Token: 0x040000B7 RID: 183
		[XmlEnum(Name = "AllDays")]
		AllDays = 0
	}
}
