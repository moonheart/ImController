using System;
using System.Runtime.InteropServices;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppLauncher
{
	// Token: 0x0200003F RID: 63
	[Guid("2e941141-7f97-4756-ba1d-9decde894a3d")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IApplicationActivationManager
	{
		// Token: 0x06000178 RID: 376
		IntPtr ActivateApplication([In] string appUserModelId, [In] string arguments, [In] ActivateOptions options, out uint processId);

		// Token: 0x06000179 RID: 377
		IntPtr ActivateForFile([In] string appUserModelId, [In] IntPtr itemArray, [In] string verb, out uint processId);

		// Token: 0x0600017A RID: 378
		IntPtr ActivateForProtocol([In] string appUserModelId, [In] IntPtr itemArray, out uint processId);
	}
}
