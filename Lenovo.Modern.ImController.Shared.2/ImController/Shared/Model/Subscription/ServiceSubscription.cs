using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.Shared.Model.Subscription
{
	// Token: 0x02000029 RID: 41
	[XmlRoot(ElementName = "Service", Namespace = "")]
	[Serializable]
	public class ServiceSubscription
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00007378 File Offset: 0x00005578
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00007380 File Offset: 0x00005580
		[XmlAttribute("id")]
		public string Id { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00007389 File Offset: 0x00005589
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00007391 File Offset: 0x00005591
		[XmlAttribute("version")]
		public string Version { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000739A File Offset: 0x0000559A
		// (set) Token: 0x06000123 RID: 291 RVA: 0x000073A2 File Offset: 0x000055A2
		[XmlAttribute("downloadLocation32")]
		public string DownloadLocation32 { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000124 RID: 292 RVA: 0x000073AB File Offset: 0x000055AB
		// (set) Token: 0x06000125 RID: 293 RVA: 0x000073B3 File Offset: 0x000055B3
		[XmlAttribute("downloadLocation64")]
		public string DownloadLocation64 { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000126 RID: 294 RVA: 0x000073BC File Offset: 0x000055BC
		// (set) Token: 0x06000127 RID: 295 RVA: 0x000073C4 File Offset: 0x000055C4
		[XmlAttribute("downloadLocation")]
		public string DownloadLocation { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000128 RID: 296 RVA: 0x000073CD File Offset: 0x000055CD
		// (set) Token: 0x06000129 RID: 297 RVA: 0x000073D5 File Offset: 0x000055D5
		[XmlAttribute("msVersion")]
		public string MsVersion { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600012A RID: 298 RVA: 0x000073DE File Offset: 0x000055DE
		// (set) Token: 0x0600012B RID: 299 RVA: 0x000073E6 File Offset: 0x000055E6
		[XmlAttribute("msDownloadLocation32")]
		public string MsDownloadLocation32 { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000073EF File Offset: 0x000055EF
		// (set) Token: 0x0600012D RID: 301 RVA: 0x000073F7 File Offset: 0x000055F7
		[XmlAttribute("msDownloadLocation64")]
		public string MsDownloadLocation64 { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00007400 File Offset: 0x00005600
		// (set) Token: 0x0600012F RID: 303 RVA: 0x00007408 File Offset: 0x00005608
		[XmlArray("SettingsList")]
		[XmlArrayItem("Setting")]
		public Setting[] SettingsList { get; set; }
	}
}
