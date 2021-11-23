using System;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Lenovo.Modern.ImController.Shared.Utilities.Ipc
{
	// Token: 0x02000041 RID: 65
	[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
	[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	internal class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060001C9 RID: 457 RVA: 0x0000923D File Offset: 0x0000743D
		private SafeTokenHandle()
			: base(true)
		{
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00009246 File Offset: 0x00007446
		internal SafeTokenHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00009256 File Offset: 0x00007456
		protected override bool ReleaseHandle()
		{
			return Win32.CloseHandle(this.handle);
		}
	}
}
