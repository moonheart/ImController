using System;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using Lenovo.Modern.ImController.ImClient.Interop;

namespace Lenovo.Modern.ImController.ImClient.Services.Umdf
{
	// Token: 0x0200001A RID: 26
	public static class DeviceDriver
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003698 File Offset: 0x00001898
		public static string DriverPath
		{
			get
			{
				if (string.IsNullOrEmpty(DeviceDriver._driverPath))
				{
					DeviceDriver._driverPath = DeviceDriver.GetDriverPath();
				}
				return DeviceDriver._driverPath;
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000036B5 File Offset: 0x000018B5
		public static IntPtr GetDeviceDriverHandle()
		{
			return Win32fileOp.CreateFile(DeviceDriver.DriverPath, 3221225472U, 3U, IntPtr.Zero, 3U, 1073741952U, IntPtr.Zero);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000036D7 File Offset: 0x000018D7
		public static bool CloseDeviceDriverHandle(IntPtr nHandle)
		{
			return nHandle != (IntPtr)(-1L) && Win32fileOp.CloseHandle(nHandle);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000036F0 File Offset: 0x000018F0
		public static bool IsUmdfDriverInstalled()
		{
			return DeviceDriver.DriverPath.Length > 0;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003700 File Offset: 0x00001900
		private static string GetDriverPath()
		{
			string result = string.Empty;
			SetupApi.SP_DEVICE_INTERFACE_DATA sp_DEVICE_INTERFACE_DATA = default(SetupApi.SP_DEVICE_INTERFACE_DATA);
			Guid deviceInterfaceGuid = ImDriverConstants.DeviceInterfaceGuid;
			IntPtr intPtr = SetupApi.SetupDiGetClassDevs(ref deviceInterfaceGuid, IntPtr.Zero, IntPtr.Zero, 16U);
			if (intPtr == (IntPtr)(-1L))
			{
				throw new Exception(string.Format("Generation of Device Information set failed with error code -{0}", Marshal.GetLastWin32Error()));
			}
			uint num = 0U;
			uint num2;
			for (;;)
			{
				sp_DEVICE_INTERFACE_DATA.cbSize = Marshal.SizeOf(typeof(SetupApi.SP_DEVICE_INTERFACE_DATA));
				if (!SetupApi.SetupDiEnumDeviceInterfaces(intPtr, IntPtr.Zero, ref deviceInterfaceGuid, num, ref sp_DEVICE_INTERFACE_DATA))
				{
					goto IL_15A;
				}
				SetupApi.SetupDiGetDeviceInterfaceDetail(intPtr, ref sp_DEVICE_INTERFACE_DATA, IntPtr.Zero, 0U, out num2, IntPtr.Zero);
				if (num2 != 0U)
				{
					break;
				}
				num += 1U;
			}
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)num2);
			Marshal.WriteInt32(intPtr2, (IntPtr.Size == 4) ? (4 + Marshal.SystemDefaultCharSize) : 8);
			uint deviceInterfaceDetailDataSize = num2;
			if (SetupApi.SetupDiGetDeviceInterfaceDetail(intPtr, ref sp_DEVICE_INTERFACE_DATA, intPtr2, deviceInterfaceDetailDataSize, out num2, IntPtr.Zero))
			{
				byte[] array = new byte[num2];
				Marshal.Copy(intPtr2, array, 0, (int)num2);
				byte[] array2 = new byte[(ulong)num2 - (ulong)((long)Marshal.SizeOf(typeof(int)))];
				Array.Copy(array, (long)Marshal.SizeOf(typeof(int)), array2, 0L, (long)((ulong)num2 - (ulong)((long)Marshal.SizeOf(typeof(int)))));
				result = Encoding.Unicode.GetString(array2);
			}
			Marshal.FreeHGlobal(intPtr2);
			IL_15A:
			SetupApi.SetupDiDestroyDeviceInfoList(intPtr);
			return result;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003870 File Offset: 0x00001A70
		public static uint GetDevicedriveStatus()
		{
			uint result = 1U;
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select HardwareId,ConfigManagerErrorCode from Win32_PnPEntity where Description='System Interface Foundation Device'"))
			{
				if (managementObjectSearcher != null)
				{
					using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
					{
						foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
						{
							string[] array = (string[])managementBaseObject.GetPropertyValue("HardwareId");
							if (((array != null) ? array[0] : "NO HWDID").Contains("iMDriver"))
							{
								result = Convert.ToUInt32(managementBaseObject.GetPropertyValue("ConfigManagerErrorCode").ToString());
								break;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003940 File Offset: 0x00001B40
		private static string GetStringPropertyForDevice(IntPtr infoSet, ref SetupApi.SP_DEVINFO_DATA devInfo, int property)
		{
			uint num;
			uint num2;
			if (!SetupApi.SetupDiGetDeviceRegistryProperty(infoSet, ref devInfo, (uint)property, out num, null, 0U, out num2))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 13)
				{
					return string.Empty;
				}
				if (lastWin32Error != 122)
				{
				}
			}
			byte[] array = new byte[num2];
			SetupApi.SetupDiGetDeviceRegistryProperty(infoSet, ref devInfo, (uint)property, out num, array, (uint)array.Length, out num2);
			return Encoding.Unicode.GetString(array);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003998 File Offset: 0x00001B98
		public static void EnableDisableDevice(bool bEnable)
		{
			Guid empty = Guid.Empty;
			IntPtr intPtr = SetupApi.SetupDiGetClassDevs(ref empty, IntPtr.Zero, IntPtr.Zero, 4U);
			SetupApi.SP_DEVINFO_DATA sp_DEVINFO_DATA = default(SetupApi.SP_DEVINFO_DATA);
			sp_DEVINFO_DATA.cbSize = (uint)Marshal.SizeOf(sp_DEVINFO_DATA);
			uint num = 0U;
			for (;;)
			{
				if (SetupApi.SetupDiEnumDeviceInfo(intPtr, num, ref sp_DEVINFO_DATA))
				{
					string stringPropertyForDevice = DeviceDriver.GetStringPropertyForDevice(intPtr, ref sp_DEVINFO_DATA, 1);
					if (!string.IsNullOrEmpty(stringPropertyForDevice) && stringPropertyForDevice.Contains("iMDriver"))
					{
						break;
					}
				}
				num += 1U;
			}
			SetupApi.SP_CLASSINSTALL_HEADER sp_CLASSINSTALL_HEADER = default(SetupApi.SP_CLASSINSTALL_HEADER);
			sp_CLASSINSTALL_HEADER.cbSize = (uint)Marshal.SizeOf(sp_CLASSINSTALL_HEADER);
			sp_CLASSINSTALL_HEADER.InstallFunction = 18U;
			SetupApi.SP_PROPCHANGE_PARAMS sp_PROPCHANGE_PARAMS = new SetupApi.SP_PROPCHANGE_PARAMS
			{
				ClassInstallHeader = sp_CLASSINSTALL_HEADER,
				StateChange = (bEnable ? 1 : 2),
				Scope = 1,
				HwProfile = 0U
			};
			SetupApi.SetupDiSetClassInstallParams(intPtr, ref sp_DEVINFO_DATA, ref sp_PROPCHANGE_PARAMS, Marshal.SizeOf(sp_PROPCHANGE_PARAMS));
			SetupApi.SetupDiChangeState(intPtr, ref sp_DEVINFO_DATA);
		}

		// Token: 0x04000046 RID: 70
		private static string _driverPath;
	}
}
