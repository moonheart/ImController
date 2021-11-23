using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.Subscription
{
	// Token: 0x0200000F RID: 15
	[XmlRoot(ElementName = "LenovoMetroSubscription", Namespace = null)]
	public sealed class ModernAppSubscription
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00003D25 File Offset: 0x00001F25
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00003D2D File Offset: 0x00001F2D
		[XmlElement(ElementName = "AppVersion")]
		public AppVersion AppVersion { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00003D36 File Offset: 0x00001F36
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00003D3E File Offset: 0x00001F3E
		[XmlArray("AppDependencyList")]
		[XmlArrayItem("AppDependency")]
		public AppDependency[] AppDependencyList { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00003D47 File Offset: 0x00001F47
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00003D4F File Offset: 0x00001F4F
		[XmlArray("ChannelList")]
		[XmlArrayItem("Channel")]
		public SubscriptionChannel[] ChannelList { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00003D58 File Offset: 0x00001F58
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00003D60 File Offset: 0x00001F60
		[XmlArray("AppSettingList")]
		[XmlArrayItem("AppSetting")]
		public AppSetting[] AppSettingList { get; set; }
	}
}
