using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Model
{
	// Token: 0x02000039 RID: 57
	[XmlRoot(ElementName = "CloudTagGroups", Namespace = "")]
	public class CloudTagGroups
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000A233 File Offset: 0x00008433
		// (set) Token: 0x06000153 RID: 339 RVA: 0x0000A23B File Offset: 0x0000843B
		[XmlElement(ElementName = "TagGroup")]
		public TagGroup[] TagGroup { get; set; }
	}
}
