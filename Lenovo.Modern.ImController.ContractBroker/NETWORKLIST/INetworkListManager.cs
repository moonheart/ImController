using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NETWORKLIST
{
	// Token: 0x02000016 RID: 22
	[CompilerGenerated]
	[Guid("DCB00000-570F-4A9B-8D69-199FDBA5723B")]
	[TypeIdentifier]
	[ComImport]
	public interface INetworkListManager
	{
		// Token: 0x060000B3 RID: 179
		void _VtblGap1_4();

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000B4 RID: 180
		[DispId(5)]
		bool IsConnectedToInternet
		{
			[DispId(5)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
