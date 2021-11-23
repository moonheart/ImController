using System;
using System.Xml.Serialization;
using Lenovo.Modern.CoreTypes.Models;
using Lenovo.Modern.ImController.Shared.Model.Packages;

namespace Lenovo.Modern.ImController.Shared.Model.Plugin
{
	// Token: 0x0200002A RID: 42
	[XmlRoot(ElementName = "PluginManifest", Namespace = null)]
	public sealed class PluginManifest
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00007411 File Offset: 0x00005611
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00007419 File Offset: 0x00005619
		[XmlElement(ElementName = "PluginInformation")]
		public PackageInformation PluginInformation { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00007422 File Offset: 0x00005622
		// (set) Token: 0x06000134 RID: 308 RVA: 0x0000742A File Offset: 0x0000562A
		[XmlArray("ApplicabilityFilter")]
		[XmlArrayItem("Filter")]
		public Filter[] ApplicabilityFilter { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00007433 File Offset: 0x00005633
		// (set) Token: 0x06000136 RID: 310 RVA: 0x0000743B File Offset: 0x0000563B
		[XmlArray("ContractMappingList")]
		[XmlArrayItem("ContractMapping")]
		public ContractMapping[] ContractMappingList { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00007444 File Offset: 0x00005644
		// (set) Token: 0x06000138 RID: 312 RVA: 0x0000744C File Offset: 0x0000564C
		[XmlArray("SubscribedEventList")]
		[XmlArrayItem("SubscribedEvent")]
		public SubscribedEvent[] SubscribedEventList { get; set; }
	}
}
