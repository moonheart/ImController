using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Lenovo.Modern.ImController
{
	// Token: 0x0200000B RID: 11
	[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
	[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	internal class SafeServiceHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002ADD File Offset: 0x00000CDD
		internal SafeServiceHandle()
			: base(true)
		{
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002AE6 File Offset: 0x00000CE6
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			return Win32.CloseServiceHandle(this.handle);
		}
	}
}
