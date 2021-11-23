using System;
using System.Runtime.InteropServices;

namespace Lenovo.Modern.ImController
{
	// Token: 0x02000007 RID: 7
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct SERVICE_FAILURE_ACTIONS
	{
		// Token: 0x04000015 RID: 21
		public int dwResetPeriod;

		// Token: 0x04000016 RID: 22
		public string lpRebootMsg;

		// Token: 0x04000017 RID: 23
		public string lpCommand;

		// Token: 0x04000018 RID: 24
		public int cActions;

		// Token: 0x04000019 RID: 25
		public IntPtr lpsaActions;
	}
}
