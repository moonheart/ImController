using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NETWORKLIST
{
	// Token: 0x0200003F RID: 63
	[CompilerGenerated]
	[Guid("DCB00000-570F-4A9B-8D69-199FDBA5723B")]
	[TypeIdentifier]
	[ComImport]
	public interface INetworkListManager
	{
		// Token: 0x06000197 RID: 407
		void _VtblGap1_4();

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000198 RID: 408
		[DispId(5)]
		bool IsConnectedToInternet
		{
			[DispId(5)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
