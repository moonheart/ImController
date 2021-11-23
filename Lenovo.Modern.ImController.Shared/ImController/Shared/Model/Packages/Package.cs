using System;
using System.Xml.Serialization;
using Lenovo.Modern.CoreTypes.Models;
using Lenovo.Modern.CoreTypes.Models.Subscription;

namespace Lenovo.Modern.ImController.Shared.Model.Packages
{
	// Token: 0x0200002C RID: 44
	[XmlRoot("Package")]
	[Serializable]
	public sealed class Package
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000144 RID: 324 RVA: 0x000074BE File Offset: 0x000056BE
		// (set) Token: 0x06000145 RID: 325 RVA: 0x000074C6 File Offset: 0x000056C6
		[XmlElement("PackageInformation")]
		public PackageInformation PackageInformation { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000146 RID: 326 RVA: 0x000074CF File Offset: 0x000056CF
		// (set) Token: 0x06000147 RID: 327 RVA: 0x000074D7 File Offset: 0x000056D7
		[XmlArray("ContractMappingList")]
		[XmlArrayItem("ContractMapping")]
		public ContractMapping[] ContractMappingList { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000148 RID: 328 RVA: 0x000074E0 File Offset: 0x000056E0
		// (set) Token: 0x06000149 RID: 329 RVA: 0x000074E8 File Offset: 0x000056E8
		[XmlArray("ApplicabilityFilter")]
		[XmlArrayItem("Filter")]
		public Filter[] ApplicabilityFilter { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600014A RID: 330 RVA: 0x000074F1 File Offset: 0x000056F1
		// (set) Token: 0x0600014B RID: 331 RVA: 0x000074F9 File Offset: 0x000056F9
		[XmlArray("EventSubscriptionList")]
		[XmlArrayItem("EventSubscription")]
		public SubscribedEvent[] SubscribedEventList { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00007502 File Offset: 0x00005702
		// (set) Token: 0x0600014D RID: 333 RVA: 0x0000750A File Offset: 0x0000570A
		[XmlArray("SettingList")]
		[XmlArrayItem("Setting")]
		public AppSetting[] SettingList { get; set; }
	}
}
