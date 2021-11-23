using System;

namespace Lenovo.Modern.CoreTypes.Events.Network
{
	// Token: 0x02000034 RID: 52
	public sealed class NetworkEventConstants
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000232 RID: 562 RVA: 0x00004DF4 File Offset: 0x00002FF4
		public static NetworkEventConstants Get
		{
			get
			{
				NetworkEventConstants result;
				if ((result = NetworkEventConstants._eventConstants) == null)
				{
					NetworkEventConstants networkEventConstants = new NetworkEventConstants();
					networkEventConstants.NetworkEventMonitorName = "NetworkMonitor";
					networkEventConstants.NetworkAddressChangedTrigger = "AddressChange";
					networkEventConstants.NetworkStatusChangedTrigger = "NetworkStatusChange";
					networkEventConstants.NetworkEventVersion = "1.0.0.0";
					networkEventConstants.NetworkEventDataType = "NetworkEvent";
					result = networkEventConstants;
					NetworkEventConstants._eventConstants = networkEventConstants;
				}
				return result;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00004E4C File Offset: 0x0000304C
		// (set) Token: 0x06000234 RID: 564 RVA: 0x00004E54 File Offset: 0x00003054
		public string NetworkEventMonitorName { get; private set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00004E5D File Offset: 0x0000305D
		// (set) Token: 0x06000236 RID: 566 RVA: 0x00004E65 File Offset: 0x00003065
		public string NetworkAddressChangedTrigger { get; private set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00004E6E File Offset: 0x0000306E
		// (set) Token: 0x06000238 RID: 568 RVA: 0x00004E76 File Offset: 0x00003076
		public string NetworkStatusChangedTrigger { get; private set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00004E7F File Offset: 0x0000307F
		// (set) Token: 0x0600023A RID: 570 RVA: 0x00004E87 File Offset: 0x00003087
		public string NetworkEventVersion { get; private set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00004E90 File Offset: 0x00003090
		// (set) Token: 0x0600023C RID: 572 RVA: 0x00004E98 File Offset: 0x00003098
		public string NetworkEventDataType { get; private set; }

		// Token: 0x040000EE RID: 238
		private static NetworkEventConstants _eventConstants;
	}
}
