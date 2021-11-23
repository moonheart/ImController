using System;
using System.Xml.Serialization;
using Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Preferences;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Commands
{
	// Token: 0x020000A3 RID: 163
	[XmlRoot(ElementName = "MessagingPreferenceSetRequest", Namespace = null)]
	public sealed class MessagingPreferenceSetRequest
	{
		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x00007C22 File Offset: 0x00005E22
		// (set) Token: 0x0600062D RID: 1581 RVA: 0x00007C2A File Offset: 0x00005E2A
		[XmlAttribute(AttributeName = "appId")]
		public string AppId { get; set; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x00007C33 File Offset: 0x00005E33
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x00007C3B File Offset: 0x00005E3B
		[XmlArray(ElementName = "CategoryList")]
		[XmlArrayItem(ElementName = "Category")]
		public CategorySetPreference[] CategoryList { get; set; }
	}
}
