using System;
using System.Xml.Serialization;
using Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Conditions;
using Lenovo.Modern.CoreTypes.Models;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2
{
	// Token: 0x02000081 RID: 129
	[XmlRoot(ElementName = "MessageConditions", Namespace = null)]
	public sealed class MessageConditions
	{
		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x0000762C File Offset: 0x0000582C
		// (set) Token: 0x0600055A RID: 1370 RVA: 0x00007634 File Offset: 0x00005834
		[XmlArray("ApplicabilityFilter")]
		[XmlArrayItem("Filter")]
		public Filter[] ApplicabilityFilter { get; set; }

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x0000763D File Offset: 0x0000583D
		// (set) Token: 0x0600055C RID: 1372 RVA: 0x00007645 File Offset: 0x00005845
		[XmlArray("CustomConditions")]
		[XmlArrayItem("CustomCondition")]
		public CustomCondition[] CustomConditionList { get; set; }

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x0000764E File Offset: 0x0000584E
		// (set) Token: 0x0600055E RID: 1374 RVA: 0x00007656 File Offset: 0x00005856
		[XmlArray("DynamicConditions")]
		[XmlArrayItem("DynamicCondition")]
		public DynamicCondition[] DynamicConditionList { get; set; }

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x0000765F File Offset: 0x0000585F
		// (set) Token: 0x06000560 RID: 1376 RVA: 0x00007667 File Offset: 0x00005867
		[XmlElement(ElementName = "TimeBounds", IsNullable = true)]
		public TimeBoundaryCondition TimeBounds { get; set; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x00007670 File Offset: 0x00005870
		// (set) Token: 0x06000562 RID: 1378 RVA: 0x00007678 File Offset: 0x00005878
		[XmlElement(ElementName = "HistoricalConditions", IsNullable = true)]
		public HistoricalCondition HistoricalConditions { get; set; }
	}
}
