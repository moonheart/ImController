using System;
using System.Xml;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.Shared.Model.Packages
{
	// Token: 0x0200002F RID: 47
	[Serializable]
	public class SubscribedEvent
	{
		// Token: 0x06000168 RID: 360 RVA: 0x00002050 File Offset: 0x00000250
		public SubscribedEvent()
		{
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000761E File Offset: 0x0000581E
		public SubscribedEvent(string monitor, string version, string runas, string plugin, string trigger)
		{
			this.Monitor = monitor;
			this.Version = version;
			this.Runas = runas;
			this.Plugin = plugin;
			this.Trigger = trigger;
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600016A RID: 362 RVA: 0x0000764B File Offset: 0x0000584B
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00007653 File Offset: 0x00005853
		[XmlAttribute(AttributeName = "monitor")]
		public string Monitor { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600016C RID: 364 RVA: 0x0000765C File Offset: 0x0000585C
		// (set) Token: 0x0600016D RID: 365 RVA: 0x00007664 File Offset: 0x00005864
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600016E RID: 366 RVA: 0x0000766D File Offset: 0x0000586D
		// (set) Token: 0x0600016F RID: 367 RVA: 0x00007675 File Offset: 0x00005875
		[XmlAttribute(AttributeName = "runas")]
		public string Runas { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000170 RID: 368 RVA: 0x0000767E File Offset: 0x0000587E
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00007686 File Offset: 0x00005886
		[XmlAttribute(AttributeName = "plugin")]
		public string Plugin { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000172 RID: 370 RVA: 0x0000768F File Offset: 0x0000588F
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00007697 File Offset: 0x00005897
		[XmlAttribute(AttributeName = "trigger")]
		public string Trigger { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000174 RID: 372 RVA: 0x000076A0 File Offset: 0x000058A0
		// (set) Token: 0x06000175 RID: 373 RVA: 0x000076A8 File Offset: 0x000058A8
		[XmlIgnore]
		public string Parameter { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000176 RID: 374 RVA: 0x000076B1 File Offset: 0x000058B1
		// (set) Token: 0x06000177 RID: 375 RVA: 0x000076C3 File Offset: 0x000058C3
		[XmlElement(ElementName = "Parameter")]
		public XmlCDataSection XmlIgnoreEventParameter
		{
			get
			{
				return new XmlDocument().CreateCDataSection(this.Parameter);
			}
			set
			{
				this.Parameter = value.Value;
			}
		}
	}
}
