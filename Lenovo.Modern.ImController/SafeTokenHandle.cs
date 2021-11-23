using System;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Lenovo.Modern.ImController
{
	// Token: 0x0200000C RID: 12
	[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
	[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	internal class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002ADD File Offset: 0x00000CDD
		private SafeTokenHandle()
			: base(true)
		{
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002AF3 File Offset: 0x00000CF3
		internal SafeTokenHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002B03 File Offset: 0x00000D03
		protected override bool ReleaseHandle()
		{
			return Win32.CloseHandle(this.handle);
		}
	}
}
