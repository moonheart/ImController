using System;
using System.Runtime.InteropServices;

namespace Lenovo.Modern.ImController
{
	// Token: 0x0200000A RID: 10
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct TOKEN_PRIVILEGES
	{
		// Token: 0x0400001D RID: 29
		public int PrivilegeCount;

		// Token: 0x0400001E RID: 30
		public LUID_AND_ATTRIBUTES Privileges;
	}
}
