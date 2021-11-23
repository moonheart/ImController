using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Lenovo.Modern.ImController.Shared.Utilities.Ipc
{
	// Token: 0x02000042 RID: 66
	[SuppressUnmanagedCodeSecurity]
	internal class Win32
	{
		// Token: 0x060001CC RID: 460
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool GetNamedPipeClientProcessId(SafePipeHandle handle, out int id);

		// Token: 0x060001CD RID: 461
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenProcess(Win32.ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

		// Token: 0x060001CE RID: 462
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, out SafeTokenHandle TokenHandle);

		// Token: 0x060001CF RID: 463
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool CloseHandle(IntPtr handle);

		// Token: 0x060001D0 RID: 464
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern uint QueryFullProcessImageName(IntPtr hProcess, uint flags, [MarshalAs(UnmanagedType.LPStr)] StringBuilder text, out uint size);

		// Token: 0x040000F5 RID: 245
		public static readonly int TOKEN_QUERY = 8;

		// Token: 0x040000F6 RID: 246
		public static readonly int TOKEN_DUPLICATE = 2;

		// Token: 0x0200008F RID: 143
		[Flags]
		public enum ProcessAccessFlags : uint
		{
			// Token: 0x04000251 RID: 593
			All = 2035711U,
			// Token: 0x04000252 RID: 594
			Terminate = 1U,
			// Token: 0x04000253 RID: 595
			CreateThread = 2U,
			// Token: 0x04000254 RID: 596
			VirtualMemoryOperation = 8U,
			// Token: 0x04000255 RID: 597
			VirtualMemoryRead = 16U,
			// Token: 0x04000256 RID: 598
			VirtualMemoryWrite = 32U,
			// Token: 0x04000257 RID: 599
			DuplicateHandle = 64U,
			// Token: 0x04000258 RID: 600
			CreateProcess = 128U,
			// Token: 0x04000259 RID: 601
			SetQuota = 256U,
			// Token: 0x0400025A RID: 602
			SetInformation = 512U,
			// Token: 0x0400025B RID: 603
			QueryInformation = 1024U,
			// Token: 0x0400025C RID: 604
			QueryLimitedInformation = 4096U,
			// Token: 0x0400025D RID: 605
			Synchronize = 1048576U
		}
	}
}
