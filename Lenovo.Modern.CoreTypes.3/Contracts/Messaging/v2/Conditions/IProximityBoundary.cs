using System;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Conditions
{
	// Token: 0x02000097 RID: 151
	public interface IProximityBoundary
	{
		// Token: 0x170002AB RID: 683
		// (get) Token: 0x060005EE RID: 1518
		// (set) Token: 0x060005EF RID: 1519
		string MinTimespan { get; set; }

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x060005F0 RID: 1520
		// (set) Token: 0x060005F1 RID: 1521
		string MaxTimespan { get; set; }
	}
}
