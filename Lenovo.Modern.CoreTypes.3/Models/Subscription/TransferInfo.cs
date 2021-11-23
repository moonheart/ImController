using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.Subscription
{
	// Token: 0x02000016 RID: 22
	[XmlRoot("TransferInfo")]
	public sealed class TransferInfo
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00003E9B File Offset: 0x0000209B
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00003EA3 File Offset: 0x000020A3
		[XmlAttribute("Application")]
		public string Application { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00003EAC File Offset: 0x000020AC
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00003EB4 File Offset: 0x000020B4
		[XmlAttribute("Brand")]
		public string Brand { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00003EBD File Offset: 0x000020BD
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00003EC5 File Offset: 0x000020C5
		[XmlAttribute("UserFolder")]
		public string UserFolder { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00003ECE File Offset: 0x000020CE
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00003ED6 File Offset: 0x000020D6
		[XmlAttribute("Source")]
		public string Source { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00003EDF File Offset: 0x000020DF
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00003EE7 File Offset: 0x000020E7
		[XmlAttribute("Destination")]
		public string Destination { get; set; }
	}
}
