using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation.BatteryInformation
{
	// Token: 0x02000054 RID: 84
	[XmlRoot(ElementName = "BatteryInformationResponse", Namespace = null)]
	public sealed class BatteryInformationResponse
	{
		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x00006052 File Offset: 0x00004252
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x0000605A File Offset: 0x0000425A
		[XmlArray("BatteryList")]
		[XmlArrayItem("BatteryInformation")]
		public BatteryInformation[] BatteryList { get; set; }
	}
}
