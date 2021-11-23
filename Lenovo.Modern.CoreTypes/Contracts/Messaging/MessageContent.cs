using System;
using System.Xml;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging
{
	// Token: 0x02000077 RID: 119
	[XmlRoot(ElementName = "MessageContent", Namespace = null)]
	public sealed class MessageContent
	{
		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x00006F69 File Offset: 0x00005169
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x00006F71 File Offset: 0x00005171
		[XmlIgnore]
		public string Tile { get; set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00006F7A File Offset: 0x0000517A
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x00006F8C File Offset: 0x0000518C
		[XmlElement(ElementName = "Tile1")]
		public XmlCDataSection XmlTileDoNotUse
		{
			get
			{
				return new XmlDocument().CreateCDataSection(this.Tile);
			}
			set
			{
				this.Tile = value.Value;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00006F9A File Offset: 0x0000519A
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x00006FA2 File Offset: 0x000051A2
		[XmlIgnore]
		public string Toast { get; set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00006FAB File Offset: 0x000051AB
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x00006FBD File Offset: 0x000051BD
		[XmlElement(ElementName = "Toast1")]
		public XmlCDataSection XmlToastDoNotUse
		{
			get
			{
				return new XmlDocument().CreateCDataSection(this.Toast);
			}
			set
			{
				this.Toast = value.Value;
			}
		}
	}
}
