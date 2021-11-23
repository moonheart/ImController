using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Lenovo.CertificateValidation
{
	// Token: 0x0200000D RID: 13
	internal static class ProcessTool
	{
		// Token: 0x06000049 RID: 73
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

		// Token: 0x0600004A RID: 74
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool GetTokenInformation(IntPtr TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass, IntPtr TokenInformation, uint TokenInformationLength, out uint ReturnLength);

		// Token: 0x0600004B RID: 75
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool CloseHandle(IntPtr hHandle);

		// Token: 0x0600004C RID: 76 RVA: 0x000042AC File Offset: 0x000024AC
		internal static bool IsCurrentProcessElevated()
		{
			bool result = false;
			IntPtr zero = IntPtr.Zero;
			uint num = 0U;
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				if (ProcessTool.OpenProcessToken(Process.GetCurrentProcess().Handle, 8U, out zero))
				{
					num = (uint)Marshal.SizeOf(typeof(TOKEN_ELEVATION));
					intPtr = Marshal.AllocHGlobal((int)num);
					if (intPtr != IntPtr.Zero && ProcessTool.GetTokenInformation(zero, TOKEN_INFORMATION_CLASS.TokenElevation, intPtr, num, out num))
					{
						result = ((TOKEN_ELEVATION)Marshal.PtrToStructure(intPtr, typeof(TOKEN_ELEVATION))).TokenIsElevated > 0U;
					}
				}
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					ProcessTool.CloseHandle(zero);
					zero = IntPtr.Zero;
				}
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
					intPtr = IntPtr.Zero;
					num = 0U;
				}
			}
			return result;
		}

		// Token: 0x0400005A RID: 90
		private const uint TOKEN_QUERY = 8U;
	}
}
