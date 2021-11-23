using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Preferences
{
	// Token: 0x02000085 RID: 133
	public sealed class CategorySetPreference
	{
		// Token: 0x1700027A RID: 634
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x0000772B File Offset: 0x0000592B
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x00007733 File Offset: 0x00005933
		[XmlAttribute(AttributeName = "id")]
		public string CategoryId { get; set; }

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x0000773C File Offset: 0x0000593C
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x00007744 File Offset: 0x00005944
		[XmlAttribute(AttributeName = "settingValue")]
		public Preference Value { get; set; }
	}
}
