using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.FileSystem
{
	// Token: 0x0200003B RID: 59
	[XmlRoot(ElementName = "FileSystemEventReaction", Namespace = null)]
	public sealed class FileSystemEventReaction
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600027F RID: 639 RVA: 0x000051AD File Offset: 0x000033AD
		// (set) Token: 0x06000280 RID: 640 RVA: 0x000051B5 File Offset: 0x000033B5
		[XmlElement("ModifiedFileLocations")]
		public string ModifiedFile { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000281 RID: 641 RVA: 0x000051BE File Offset: 0x000033BE
		// (set) Token: 0x06000282 RID: 642 RVA: 0x000051C6 File Offset: 0x000033C6
		[XmlElement("ModifiedDirectoryLocations")]
		public string ModifiedDirectory { get; set; }
	}
}
