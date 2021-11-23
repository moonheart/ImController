using System;
using System.Runtime.InteropServices;
using System.Security;
using Lenovo.Modern.CoreTypes.Events.Network;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.Network
{
	// Token: 0x0200001A RID: 26
	[SuppressUnmanagedCodeSecurity]
	public static class WlanMonitorConstants
	{
		// Token: 0x02000037 RID: 55
		public class Win32
		{
			// Token: 0x06000105 RID: 261
			[DllImport("Wlanapi.dll")]
			public static extern int WlanOpenHandle(uint dwClientVersion, IntPtr pReserved, ref uint pdwNegotiatedVersion, ref IntPtr ClientHandle);

			// Token: 0x06000106 RID: 262
			[DllImport("Wlanapi.dll")]
			public static extern int WlanCloseHandle(IntPtr hClientHandle, IntPtr pReserved);

			// Token: 0x06000107 RID: 263
			[DllImport("Wlanapi.dll")]
			public static extern int WlanRegisterNotification(IntPtr hClientHandle, int dwNotifSource, bool bIgnoreDuplicate, WlanMonitorConstants.Win32.WLAN_NOTIFICATION_CALLBACK funcCallback, IntPtr pCallbackContext, IntPtr pReserved, ref WlanEventConstants.Win32.WLAN_NOTIFICATION_SOURCE pdwPrevNotifSource);

			// Token: 0x040000F8 RID: 248
			public const int ERROR_SUCCESS = 0;

			// Token: 0x0200004C RID: 76
			public struct WLAN_NOTIFICATION_DATA
			{
				// Token: 0x04000148 RID: 328
				public int notificationSource;

				// Token: 0x04000149 RID: 329
				public int notificationCode;

				// Token: 0x0400014A RID: 330
				public Guid InterfaceGuid;

				// Token: 0x0400014B RID: 331
				public int dataSize;

				// Token: 0x0400014C RID: 332
				public IntPtr data;
			}

			// Token: 0x0200004D RID: 77
			public struct WLAN_NOTIFICATION_DATA_PARTIAL
			{
				// Token: 0x0400014D RID: 333
				public int notificationSource;

				// Token: 0x0400014E RID: 334
				public int notificationCode;

				// Token: 0x0400014F RID: 335
				public Guid InterfaceGuid;

				// Token: 0x04000150 RID: 336
				public int dataSize;
			}

			// Token: 0x0200004E RID: 78
			// (Invoke) Token: 0x06000122 RID: 290
			public delegate void WLAN_NOTIFICATION_CALLBACK(ref WlanMonitorConstants.Win32.WLAN_NOTIFICATION_DATA notificationData, IntPtr context);
		}
	}
}
