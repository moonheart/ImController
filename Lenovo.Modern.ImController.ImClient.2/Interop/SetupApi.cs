using System;
using System.Runtime.InteropServices;

namespace Lenovo.Modern.ImController.ImClient.Interop
{
	// Token: 0x0200000E RID: 14
	public class SetupApi
	{
		// Token: 0x0600003D RID: 61
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, uint Flags);

		// Token: 0x0600003E RID: 62
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, uint memberIndex, ref SetupApi.SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

		// Token: 0x0600003F RID: 63
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetupDiSetClassInstallParams(IntPtr DeviceInfoSet, [In] ref SetupApi.SP_DEVINFO_DATA deviceInfoData, [In] ref SetupApi.SP_PROPCHANGE_PARAMS classInstallParams, int ClassInstallParamsSize);

		// Token: 0x06000040 RID: 64
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("setupapi.dll", CharSet = CharSet.Auto)]
		public static extern bool SetupDiCallClassInstaller(uint InstallFunction, IntPtr DeviceInfoSet, IntPtr DeviceInfoData);

		// Token: 0x06000041 RID: 65
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SetupApi.SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, uint deviceInterfaceDetailDataSize, out uint requiredSize, ref SetupApi.SP_DEVINFO_DATA deviceInfoData);

		// Token: 0x06000042 RID: 66
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern bool SetupDiChangeState(IntPtr deviceInfoSet, [In] [Out] ref SetupApi.SP_DEVINFO_DATA DeviceInfoData);

		// Token: 0x06000043 RID: 67
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SetupApi.SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, uint deviceInterfaceDetailDataSize, out uint requiredSize, IntPtr deviceInfoData);

		// Token: 0x06000044 RID: 68
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint MemberIndex, ref SetupApi.SP_DEVINFO_DATA DeviceInfoData);

		// Token: 0x06000045 RID: 69
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetupDiGetDeviceRegistryProperty(IntPtr deviceInfoSet, ref SetupApi.SP_DEVINFO_DATA deviceInfoData, uint property, out uint propertyRegDataType, byte[] propertyBuffer, uint propertyBufferSize, out uint requiredSize);

		// Token: 0x06000046 RID: 70
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("setupapi.dll", SetLastError = true)]
		public static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

		// Token: 0x04000017 RID: 23
		public const int ERROR_INVALID_DATA = 13;

		// Token: 0x04000018 RID: 24
		public const int ERROR_INSUFFICIENT_BUFFER = 122;

		// Token: 0x04000019 RID: 25
		public const uint DIGCF_ALLCLASSES = 4U;

		// Token: 0x0400001A RID: 26
		public const uint DIGCF_DEVICEINTERFACE = 16U;

		// Token: 0x0400001B RID: 27
		public const long INVALID_HANDLE_VALUE = -1L;

		// Token: 0x0400001C RID: 28
		public const int DIF_PROPERTYCHANGE = 18;

		// Token: 0x0400001D RID: 29
		public const int DICS_FLAG_GLOBAL = 1;

		// Token: 0x0400001E RID: 30
		public const int DICS_FLAG_CONFIGSPECIFIC = 2;

		// Token: 0x0400001F RID: 31
		public const int DICS_ENABLE = 1;

		// Token: 0x04000020 RID: 32
		public const int DICS_DISABLE = 2;

		// Token: 0x04000021 RID: 33
		public const int HARDWAREID = 1;

		// Token: 0x04000022 RID: 34
		public const uint DEVICE_OK = 0U;

		// Token: 0x04000023 RID: 35
		public const uint DEVICE_DISABLED = 22U;

		// Token: 0x04000024 RID: 36
		public const uint DEVICE_HAS_PROBLEM = 1U;

		// Token: 0x02000050 RID: 80
		public struct SP_DEVICE_INTERFACE_DATA
		{
			// Token: 0x040000FD RID: 253
			public int cbSize;

			// Token: 0x040000FE RID: 254
			public Guid interfaceClassGuid;

			// Token: 0x040000FF RID: 255
			public int flags;

			// Token: 0x04000100 RID: 256
			private UIntPtr reserved;
		}

		// Token: 0x02000051 RID: 81
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct SP_DEVICE_INTERFACE_DETAIL_DATA
		{
			// Token: 0x04000101 RID: 257
			public int cbSize;

			// Token: 0x04000102 RID: 258
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
			public string DevicePath;
		}

		// Token: 0x02000052 RID: 82
		public struct SP_DEVINFO_DATA
		{
			// Token: 0x04000103 RID: 259
			public uint cbSize;

			// Token: 0x04000104 RID: 260
			public Guid ClassGuid;

			// Token: 0x04000105 RID: 261
			public uint DevInst;

			// Token: 0x04000106 RID: 262
			public IntPtr Reserved;
		}

		// Token: 0x02000053 RID: 83
		public struct SP_CLASSINSTALL_HEADER
		{
			// Token: 0x04000107 RID: 263
			public uint cbSize;

			// Token: 0x04000108 RID: 264
			public uint InstallFunction;
		}

		// Token: 0x02000054 RID: 84
		public struct SP_PROPCHANGE_PARAMS
		{
			// Token: 0x04000109 RID: 265
			public SetupApi.SP_CLASSINSTALL_HEADER ClassInstallHeader;

			// Token: 0x0400010A RID: 266
			public int StateChange;

			// Token: 0x0400010B RID: 267
			public int Scope;

			// Token: 0x0400010C RID: 268
			public uint HwProfile;
		}
	}
}
