using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.WindowMessage
{
	// Token: 0x02000010 RID: 16
	[SuppressUnmanagedCodeSecurity]
	public static class WindowMessageMonitorConstants
	{
		// Token: 0x04000025 RID: 37
		public static readonly string Version = "1.0.0.0";

		// Token: 0x04000026 RID: 38
		public static readonly string MonitorName = "WindowMessageMonitor";

		// Token: 0x04000027 RID: 39
		internal static readonly string DataType = "WindowMessageEvent";

		// Token: 0x04000028 RID: 40
		internal static readonly string PowerTrigger = "PowerChange";

		// Token: 0x04000029 RID: 41
		internal static readonly string SessionTrigger = "SessionChange";

		// Token: 0x0400002A RID: 42
		internal static readonly string DeviceTrigger = "DeviceChange";

		// Token: 0x0200002C RID: 44
		public class Win32
		{
			// Token: 0x060000F0 RID: 240
			[DllImport("User32", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
			public static extern IntPtr RegisterPowerSettingNotification(IntPtr hRecipient, ref Guid PowerSettingGuid, int Flags);

			// Token: 0x060000F1 RID: 241
			[DllImport("User32", CallingConvention = CallingConvention.StdCall)]
			private static extern bool UnregisterPowerSettingNotification(IntPtr handle);

			// Token: 0x060000F2 RID: 242
			[DllImport("user32.dll", SetLastError = true)]
			public static extern IntPtr RegisterDeviceNotificationW(IntPtr IntPtr, IntPtr NotificationFilter, uint Flags);

			// Token: 0x060000F3 RID: 243
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			public static extern uint UnregisterDeviceNotification(IntPtr hHandle);

			// Token: 0x040000B3 RID: 179
			public const int NO_ERROR = 0;

			// Token: 0x040000B4 RID: 180
			public const int DEVICE_NOTIFY_SERVICE_HANDLE = 1;

			// Token: 0x040000B5 RID: 181
			public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;

			// Token: 0x040000B6 RID: 182
			public const int DBT_DEVTYP_DEVICEINTERFACE = 5;

			// Token: 0x040000B7 RID: 183
			public const int DBT_DEVTYP_HANDLE = 6;

			// Token: 0x040000B8 RID: 184
			public const int DBT_DEVICEARRIVAL = 32768;

			// Token: 0x040000B9 RID: 185
			public const int DBT_DEVICEQUERYREMOVE = 32769;

			// Token: 0x040000BA RID: 186
			public const int DBT_DEVICEREMOVECOMPLETE = 32772;

			// Token: 0x040000BB RID: 187
			public const int SERVICE_CONTROL_STOP = 1;

			// Token: 0x040000BC RID: 188
			public const int SERVICE_CONTROL_DEVICEEVENT = 11;

			// Token: 0x040000BD RID: 189
			public const int SERVICE_CONTROL_SHUTDOWN = 5;

			// Token: 0x040000BE RID: 190
			public const int SERVICE_CONTROL_POWEREVENT = 13;

			// Token: 0x040000BF RID: 191
			public const int SERVICE_CONTROL_SESSIONCHANGE = 14;

			// Token: 0x040000C0 RID: 192
			public const int SERVICE_START = 10001;

			// Token: 0x040000C1 RID: 193
			public const int SERVICE_SHUTDOWN = 1001;

			// Token: 0x040000C2 RID: 194
			public const int PBT_APMQUERYSUSPEND = 0;

			// Token: 0x040000C3 RID: 195
			public const int PBT_APMQUERYSTANDBY = 1;

			// Token: 0x040000C4 RID: 196
			public const int PBT_APMQUERYSUSPENDFAILED = 2;

			// Token: 0x040000C5 RID: 197
			public const int PBT_APMQUERYSTANDBYFAILED = 3;

			// Token: 0x040000C6 RID: 198
			public const int PBT_APMSUSPEND = 4;

			// Token: 0x040000C7 RID: 199
			public const int PBT_APMSTANDBY = 5;

			// Token: 0x040000C8 RID: 200
			public const int PBT_APMRESUMECRITICAL = 6;

			// Token: 0x040000C9 RID: 201
			public const int PBT_APMRESUMESUSPEND = 7;

			// Token: 0x040000CA RID: 202
			public const int PBT_APMRESUMESTANDBY = 8;

			// Token: 0x040000CB RID: 203
			public const int PBT_APMBATTERYLOW = 9;

			// Token: 0x040000CC RID: 204
			public const int PBT_APMPOWERSTATUSCHANGE = 10;

			// Token: 0x040000CD RID: 205
			public const int PBT_APMOEMEVENT = 11;

			// Token: 0x040000CE RID: 206
			public const int PBT_APMRESUMEAUTOMATIC = 18;

			// Token: 0x040000CF RID: 207
			public const int PBT_POWERSETTINGCHANGE = 32787;

			// Token: 0x040000D0 RID: 208
			public const int WTS_CONSOLE_CONNECT = 1;

			// Token: 0x040000D1 RID: 209
			public const int WTS_CONSOLE_DISCONNECT = 2;

			// Token: 0x040000D2 RID: 210
			public const int WTS_REMOTE_CONNECT = 3;

			// Token: 0x040000D3 RID: 211
			public const int WTS_REMOTE_DISCONNECT = 4;

			// Token: 0x040000D4 RID: 212
			public const int WTS_SESSION_LOGON = 5;

			// Token: 0x040000D5 RID: 213
			public const int WTS_SESSION_LOGOFF = 6;

			// Token: 0x040000D6 RID: 214
			public const int WTS_SESSION_LOCK = 7;

			// Token: 0x040000D7 RID: 215
			public const int WTS_SESSION_UNLOCK = 8;

			// Token: 0x040000D8 RID: 216
			public const int WTS_SESSION_REMOTE_CONTROL = 9;

			// Token: 0x02000046 RID: 70
			[StructLayout(LayoutKind.Sequential, Pack = 4)]
			public struct POWERBROADCAST_SETTING_PARTIAL
			{
				// Token: 0x04000135 RID: 309
				public Guid PowerSetting;

				// Token: 0x04000136 RID: 310
				public int DataLength;
			}

			// Token: 0x02000047 RID: 71
			[StructLayout(LayoutKind.Sequential, Pack = 4)]
			public struct POWERBROADCAST_SETTING
			{
				// Token: 0x04000137 RID: 311
				public Guid PowerSetting;

				// Token: 0x04000138 RID: 312
				public int DataLength;

				// Token: 0x04000139 RID: 313
				public byte[] Data;
			}

			// Token: 0x02000048 RID: 72
			[StructLayout(LayoutKind.Sequential, Pack = 4)]
			public struct WTSSESSION_NOTIFICATION
			{
				// Token: 0x0400013A RID: 314
				public int cbSize;

				// Token: 0x0400013B RID: 315
				public uint dwSessionId;
			}

			// Token: 0x02000049 RID: 73
			public struct DEV_BROADCAST_HDR
			{
				// Token: 0x0400013C RID: 316
				public uint dbch_Size;

				// Token: 0x0400013D RID: 317
				public uint dbch_DeviceType;

				// Token: 0x0400013E RID: 318
				public uint dbch_Reserved;
			}

			// Token: 0x0200004A RID: 74
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
			public struct DEV_BROADCAST_DEVICEINTERFACE
			{
				// Token: 0x0400013F RID: 319
				public int dbcc_size;

				// Token: 0x04000140 RID: 320
				public int dbcc_devicetype;

				// Token: 0x04000141 RID: 321
				public int dbcc_reserved;

				// Token: 0x04000142 RID: 322
				public Guid dbcc_classguid;

				// Token: 0x04000143 RID: 323
				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
				public string dbcc_name;
			}
		}
	}
}
