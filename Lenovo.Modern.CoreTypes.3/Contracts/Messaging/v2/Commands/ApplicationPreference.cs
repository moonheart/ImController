using System;
using System.Xml.Serialization;
using Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Preferences;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2.Commands
{
	// Token: 0x020000A2 RID: 162
	[XmlRoot(ElementName = "ApplicationPreference", Namespace = null)]
	public sealed class ApplicationPreference
	{
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x00007C00 File Offset: 0x00005E00
		// (set) Token: 0x06000628 RID: 1576 RVA: 0x00007C08 File Offset: 0x00005E08
		[XmlAttribute(AttributeName = "appId")]
		public string AppId { get; set; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x00007C11 File Offset: 0x00005E11
		// (set) Token: 0x0600062A RID: 1578 RVA: 0x00007C19 File Offset: 0x00005E19
		[XmlArray(ElementName = "CategoryList")]
		[XmlArrayItem(ElementName = "Category")]
		public CategoryGetPreference[] CategoryList { get; set; }
	}
}
