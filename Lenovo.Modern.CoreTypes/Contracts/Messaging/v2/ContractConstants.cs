using System;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging.v2
{
	// Token: 0x0200007E RID: 126
	public sealed class ContractConstants
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x000073B4 File Offset: 0x000055B4
		public static ContractConstants Get
		{
			get
			{
				ContractConstants result;
				if ((result = ContractConstants._contractConstants) == null)
				{
					ContractConstants contractConstants = new ContractConstants();
					contractConstants.ContractName = "SystemManagement.Messaging";
					contractConstants.ContractVersion = "1.0.0.0";
					contractConstants.CommandNameGetMessagingPreference = "Get-MessagingPreference";
					contractConstants.CommandNameSetMessagingPreference = "Set-MessagingPreference";
					contractConstants.CommandNameForwardMessageInvocation = "Forward-MessageInvocation";
					contractConstants.CommandNameReceiveMessageAction = "Receive-MessageAction";
					contractConstants.CommandNameQueueMessageAction = "Queue-Message";
					contractConstants.CommandNameMessageRemovalAction = "Remove-Message";
					contractConstants.DataTypeMessagingPreferenceGetRequest = "MessagingPreferenceGetRequest";
					contractConstants.DataTypeMessagingPreferenceGetResponse = "MessagingPreferenceGetResponse";
					contractConstants.DataTypeMessagingPreferenceSetRequest = "MessagingPreferenceSetRequest";
					contractConstants.DataTypeMessageRemovalRequest = "MessageRemovalRequest";
					contractConstants.DataTypeMessageQueueRequest = "MessageQueueRequest";
					contractConstants.DataTypeMessageRequestResult = "MessageRequestResult";
					result = contractConstants;
					ContractConstants._contractConstants = contractConstants;
				}
				return result;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x00007472 File Offset: 0x00005672
		// (set) Token: 0x06000523 RID: 1315 RVA: 0x0000747A File Offset: 0x0000567A
		public string ContractName { get; private set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00007483 File Offset: 0x00005683
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x0000748B File Offset: 0x0000568B
		public string ContractVersion { get; private set; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00007494 File Offset: 0x00005694
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x0000749C File Offset: 0x0000569C
		public string CommandNameGetMessagingPreference { get; private set; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x000074A5 File Offset: 0x000056A5
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x000074AD File Offset: 0x000056AD
		public string CommandNameSetMessagingPreference { get; private set; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x000074B6 File Offset: 0x000056B6
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x000074BE File Offset: 0x000056BE
		public string CommandNameForwardMessageInvocation { get; private set; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x000074C7 File Offset: 0x000056C7
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x000074CF File Offset: 0x000056CF
		public string CommandNameReceiveMessageAction { get; private set; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x000074D8 File Offset: 0x000056D8
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x000074E0 File Offset: 0x000056E0
		public string DataTypeMessagingPreferenceGetRequest { get; private set; }

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x000074E9 File Offset: 0x000056E9
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x000074F1 File Offset: 0x000056F1
		public string DataTypeMessagingPreferenceGetResponse { get; private set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x000074FA File Offset: 0x000056FA
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x00007502 File Offset: 0x00005702
		public string DataTypeMessagingPreferenceSetRequest { get; private set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x0000750B File Offset: 0x0000570B
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x00007513 File Offset: 0x00005713
		public string DataTypeMessageRemovalRequest { get; private set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x0000751C File Offset: 0x0000571C
		// (set) Token: 0x06000537 RID: 1335 RVA: 0x00007524 File Offset: 0x00005724
		public string DataTypeMessageQueueRequest { get; private set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0000752D File Offset: 0x0000572D
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x00007535 File Offset: 0x00005735
		public string DataTypeMessageRequestResult { get; private set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x0000753E File Offset: 0x0000573E
		// (set) Token: 0x0600053B RID: 1339 RVA: 0x00007546 File Offset: 0x00005746
		public string CommandNameQueueMessageAction { get; set; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0000754F File Offset: 0x0000574F
		// (set) Token: 0x0600053D RID: 1341 RVA: 0x00007557 File Offset: 0x00005757
		public string CommandNameMessageRemovalAction { get; set; }

		// Token: 0x040002A9 RID: 681
		private static ContractConstants _contractConstants;
	}
}
