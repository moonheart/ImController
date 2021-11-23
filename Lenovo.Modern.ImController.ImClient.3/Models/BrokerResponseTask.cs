using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.ImController.ImClient.Models
{
	// Token: 0x02000025 RID: 37
	public sealed class BrokerResponseTask
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x00002344 File Offset: 0x00000544
		public BrokerResponseTask()
		{
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000493B File Offset: 0x00002B3B
		public BrokerResponseTask(string contractResponseXml, bool isComplete, int percentageComplete)
		{
			this.IsComplete = isComplete;
			this.PercentageComplete = percentageComplete;
			this.ContractResponse = new ContractResponse
			{
				Response = new ResponseData
				{
					Data = contractResponseXml
				}
			};
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x0000496E File Offset: 0x00002B6E
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00004976 File Offset: 0x00002B76
		[XmlAttribute(AttributeName = "taskId")]
		public string TaskId { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x0000497F File Offset: 0x00002B7F
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00004987 File Offset: 0x00002B87
		[XmlAttribute(AttributeName = "isComplete")]
		public bool IsComplete { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004990 File Offset: 0x00002B90
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00004998 File Offset: 0x00002B98
		[XmlAttribute(AttributeName = "keepAliveMins")]
		public int KeepAliveMinutes { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000CC RID: 204 RVA: 0x000049A1 File Offset: 0x00002BA1
		// (set) Token: 0x060000CD RID: 205 RVA: 0x000049A9 File Offset: 0x00002BA9
		[XmlAttribute(AttributeName = "percentageComplete")]
		public int PercentageComplete { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000CE RID: 206 RVA: 0x000049B2 File Offset: 0x00002BB2
		// (set) Token: 0x060000CF RID: 207 RVA: 0x000049BA File Offset: 0x00002BBA
		[XmlAttribute(AttributeName = "statusComment")]
		public string StatusComment { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000049C3 File Offset: 0x00002BC3
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x000049CB File Offset: 0x00002BCB
		[XmlElement(ElementName = "ContractResponse")]
		public ContractResponse ContractResponse { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x000049D4 File Offset: 0x00002BD4
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x000049DC File Offset: 0x00002BDC
		[XmlElement(ElementName = "EventResponse")]
		public EventResponse EventResponse { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x000049E5 File Offset: 0x00002BE5
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x000049ED File Offset: 0x00002BED
		[XmlElement(ElementName = "FailureData")]
		public FailureData Error { get; set; }
	}
}
