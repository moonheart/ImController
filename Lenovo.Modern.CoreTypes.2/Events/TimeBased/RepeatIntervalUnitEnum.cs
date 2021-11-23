using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.TimeBased
{
	// Token: 0x02000029 RID: 41
	public enum RepeatIntervalUnitEnum
	{
		// Token: 0x040000AB RID: 171
		[XmlEnum(Name = "Hourly")]
		Hourly,
		// Token: 0x040000AC RID: 172
		[XmlEnum(Name = "Daily")]
		Daily,
		// Token: 0x040000AD RID: 173
		[XmlEnum(Name = "Weekly")]
		Weekly,
		// Token: 0x040000AE RID: 174
		[XmlEnum(Name = "Monthly")]
		Monthly
	}
}
