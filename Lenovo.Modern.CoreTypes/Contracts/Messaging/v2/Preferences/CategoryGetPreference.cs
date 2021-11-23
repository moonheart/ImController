using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Preferences
{
	// Token: 0x02000084 RID: 132
	public sealed class CategoryGetPreference
	{
		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x000076D6 File Offset: 0x000058D6
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x000076DE File Offset: 0x000058DE
		[XmlAttribute(AttributeName = "id")]
		public string CategoryId { get; set; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x000076E7 File Offset: 0x000058E7
		// (set) Token: 0x06000573 RID: 1395 RVA: 0x000076EF File Offset: 0x000058EF
		[XmlAttribute(AttributeName = "settingValue")]
		public Preference Value { get; set; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x000076F8 File Offset: 0x000058F8
		// (set) Token: 0x06000575 RID: 1397 RVA: 0x00007700 File Offset: 0x00005900
		[XmlAttribute(AttributeName = "isPolicyManaged")]
		public bool IsManaged { get; set; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x00007709 File Offset: 0x00005909
		// (set) Token: 0x06000577 RID: 1399 RVA: 0x00007711 File Offset: 0x00005911
		[XmlAttribute(AttributeName = "displayName")]
		public string DisplayName { get; set; }

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0000771A File Offset: 0x0000591A
		// (set) Token: 0x06000579 RID: 1401 RVA: 0x00007722 File Offset: 0x00005922
		[XmlAttribute(AttributeName = "displayDescription")]
		public string DisplayDescription { get; set; }
	}
}
