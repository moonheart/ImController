using System;
using System.Runtime.InteropServices;

namespace Lenovo.Modern.ImController.Shared.Utilities.Interop
{
	// Token: 0x02000044 RID: 68
	public static class Mutex
	{
		// Token: 0x060001E4 RID: 484
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr CreateMutex(IntPtr lpMutexAttributes, bool bInitialOwner, string lpName);

		// Token: 0x060001E5 RID: 485
		[DllImport("kernel32.dll")]
		public static extern int GetLastError();

		// Token: 0x060001E6 RID: 486
		[DllImport("kernel32.dll")]
		private static extern bool ReleaseMutex(IntPtr hMutex);

		// Token: 0x04000103 RID: 259
		public const int ERROR_ALREADY_EXISTS = 183;
	}
}
