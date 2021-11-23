using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppTag.Model
{
	// Token: 0x0200003A RID: 58
	public class TagGroup
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000A244 File Offset: 0x00008444
		// (set) Token: 0x06000156 RID: 342 RVA: 0x0000A24C File Offset: 0x0000844C
		[XmlElement(ElementName = "TagRule")]
		public TagRule[] TagRule { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000A255 File Offset: 0x00008455
		// (set) Token: 0x06000158 RID: 344 RVA: 0x0000A25D File Offset: 0x0000845D
		[XmlAttribute(AttributeName = "logic")]
		public string Logic { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000159 RID: 345 RVA: 0x0000A266 File Offset: 0x00008466
		// (set) Token: 0x0600015A RID: 346 RVA: 0x0000A26E File Offset: 0x0000846E
		[XmlAttribute(AttributeName = "tagName")]
		public string TagName { get; set; }
	}
}
