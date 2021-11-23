using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.AppLauncher
{
	// Token: 0x0200003E RID: 62
	[Guid("45BA127D-10A8-46EA-8AB7-56EA9078943C")]
	[ComImport]
	public class ApplicationActivationManager : IApplicationActivationManager
	{
		// Token: 0x06000174 RID: 372
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern IntPtr ActivateApplication([In] string appUserModelId, [In] string arguments, [In] ActivateOptions options, out uint processId);

		// Token: 0x06000175 RID: 373
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern IntPtr ActivateForFile([In] string appUserModelId, [In] IntPtr itemArray, [In] string verb, out uint processId);

		// Token: 0x06000176 RID: 374
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern IntPtr ActivateForProtocol([In] string appUserModelId, [In] IntPtr itemArray, out uint processId);

		// Token: 0x06000177 RID: 375
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ApplicationActivationManager();
	}
}
