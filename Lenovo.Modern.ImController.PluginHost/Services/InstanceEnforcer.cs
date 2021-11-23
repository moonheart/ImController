using System;
using System.Runtime.InteropServices;
using Lenovo.Modern.ImController.Shared.Utilities.Interop;

namespace Lenovo.Modern.ImController.PluginHost.Services
{
	// Token: 0x0200000C RID: 12
	public class InstanceEnforcer : IInstanceEnforcer
	{
		// Token: 0x06000039 RID: 57 RVA: 0x0000366B File Offset: 0x0000186B
		public void AssertOnlyInstance(string name)
		{
			if (InstanceEnforcer.IsAnotherInstanceRunning(name + "_InstanceEnforcerMutex"))
			{
				throw new InstanceEnforcer.MultipleInstanceException("An instance already exists with the same name");
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000368A File Offset: 0x0000188A
		private static bool IsAnotherInstanceRunning(string name)
		{
			Mutex.CreateMutex(IntPtr.Zero, true, name);
			return Marshal.GetLastWin32Error() == 183;
		}

		// Token: 0x02000014 RID: 20
		public class MultipleInstanceException : Exception
		{
			// Token: 0x0600004D RID: 77 RVA: 0x00003C5C File Offset: 0x00001E5C
			public MultipleInstanceException(string message)
				: base(message)
			{
			}
		}
	}
}
