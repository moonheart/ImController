using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.FileSystem
{
	// Token: 0x0200003C RID: 60
	[XmlRoot(ElementName = "FileSystemEventSubscription", Namespace = null)]
	public sealed class FileSystemEventSubscription
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000284 RID: 644 RVA: 0x000051CF File Offset: 0x000033CF
		// (set) Token: 0x06000285 RID: 645 RVA: 0x000051D7 File Offset: 0x000033D7
		[XmlArray("FileLocations")]
		[XmlArrayItem("File")]
		public string[] FileList { get; set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000286 RID: 646 RVA: 0x000051E0 File Offset: 0x000033E0
		// (set) Token: 0x06000287 RID: 647 RVA: 0x000051E8 File Offset: 0x000033E8
		[XmlArray("DirectoryLocations")]
		[XmlArrayItem("Directory")]
		public string[] DirectoryList { get; set; }
	}
}
