using System;
using System.Runtime.InteropServices;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.Network
{
	// Token: 0x02000018 RID: 24
	internal static class NetworkMonitorConstants
	{
		// Token: 0x0600007A RID: 122
		[DllImport("wininet.dll", SetLastError = true)]
		public static extern bool InternetGetConnectedState(out int lpdwFlags, int dwReserved);

		// Token: 0x04000059 RID: 89
		public static readonly string Version = "1.0.0.0";

		// Token: 0x0400005A RID: 90
		public static readonly string MonitorName = "NetworkMonitor";

		// Token: 0x0400005B RID: 91
		internal static readonly string DataType = "NetworkEvent";

		// Token: 0x0400005C RID: 92
		internal static readonly string TriggerAddressChanged = "AddressChange";

		// Token: 0x0400005D RID: 93
		internal static readonly string TriggerNetworkStatus = "NetworkStatusChange";
	}
}
