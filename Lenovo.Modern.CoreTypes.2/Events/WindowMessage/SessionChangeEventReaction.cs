using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Events.WindowMessage
{
	// Token: 0x02000025 RID: 37
	[XmlRoot(ElementName = "SessionChangeEventReaction", Namespace = null)]
	public sealed class SessionChangeEventReaction
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0000463F File Offset: 0x0000283F
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00004647 File Offset: 0x00002847
		[XmlElement(ElementName = "WtsSessionValue")]
		public int WtsSessionValue { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00004650 File Offset: 0x00002850
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00004658 File Offset: 0x00002858
		[XmlElement(ElementName = "SessionId")]
		public uint SessionId { get; set; }
	}
}
