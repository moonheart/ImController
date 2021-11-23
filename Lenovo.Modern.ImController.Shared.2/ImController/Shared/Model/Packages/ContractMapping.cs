using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.Shared.Model.Packages
{
	// Token: 0x0200002B RID: 43
	[Serializable]
	public class ContractMapping
	{
		// Token: 0x0600013A RID: 314 RVA: 0x00002050 File Offset: 0x00000250
		public ContractMapping()
		{
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00007455 File Offset: 0x00005655
		public ContractMapping(string name, string version, string runAs, string plugin)
		{
			this.Name = name;
			this.Version = version;
			this.Runas = runAs;
			this.Plugin = plugin;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000747A File Offset: 0x0000567A
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00007482 File Offset: 0x00005682
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000748B File Offset: 0x0000568B
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00007493 File Offset: 0x00005693
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000140 RID: 320 RVA: 0x0000749C File Offset: 0x0000569C
		// (set) Token: 0x06000141 RID: 321 RVA: 0x000074A4 File Offset: 0x000056A4
		[XmlAttribute(AttributeName = "runas")]
		public string Runas { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000142 RID: 322 RVA: 0x000074AD File Offset: 0x000056AD
		// (set) Token: 0x06000143 RID: 323 RVA: 0x000074B5 File Offset: 0x000056B5
		[XmlAttribute(AttributeName = "plugin")]
		public string Plugin { get; set; }
	}
}
