using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Lenovo.Modern.ImController.ImClient.Interop
{
	// Token: 0x0200000D RID: 13
	internal class DeviceIo
	{
		// Token: 0x06000039 RID: 57
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool DeviceIoControl(IntPtr hDevice, uint IoControlCode, IntPtr InBuffer, uint nInBufferSize, [Out] IntPtr OutBuffer, uint nOutBufferSize, out uint pBytesReturned, ref System.Threading.NativeOverlapped Overlapped);

		// Token: 0x0600003A RID: 58
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool GetOverlappedResult(IntPtr hFile, [In] ref System.Threading.NativeOverlapped lpOverlapped, out uint lpNumberOfBytesTransferred, bool bWait);

		// Token: 0x0600003B RID: 59
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CancelIo(IntPtr hFile);

		// Token: 0x04000016 RID: 22
		public const int ErrorIOPending = 997;

		// Token: 0x0200004E RID: 78
		[Flags]
		public enum IOMethod : uint
		{
			// Token: 0x040000F5 RID: 245
			Buffered = 0U,
			// Token: 0x040000F6 RID: 246
			InDirect = 1U,
			// Token: 0x040000F7 RID: 247
			OutDirect = 2U,
			// Token: 0x040000F8 RID: 248
			Neither = 3U
		}

		// Token: 0x0200004F RID: 79
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public struct NativeOverlapped
		{
			// Token: 0x040000F9 RID: 249
			private IntPtr InternalLow;

			// Token: 0x040000FA RID: 250
			private IntPtr InternalHigh;

			// Token: 0x040000FB RID: 251
			public long Offset;

			// Token: 0x040000FC RID: 252
			public IntPtr EventHandle;
		}
	}
}
