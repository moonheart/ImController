using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IWshRuntimeLibrary
{
	// Token: 0x02000047 RID: 71
	[CompilerGenerated]
	[DefaultMember("FullName")]
	[Guid("F935DC23-1CF0-11D0-ADB9-00C04FD58A0B")]
	[TypeIdentifier]
	[ComImport]
	public interface IWshShortcut
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000189 RID: 393
		[DispId(0)]
		[IndexerName("FullName")]
		string FullName
		{
			[DispId(0)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600018A RID: 394
		// (set) Token: 0x0600018B RID: 395
		[DispId(1000)]
		string Arguments
		{
			[DispId(1000)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(1000)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			[param: MarshalAs(UnmanagedType.BStr)]
			[param: In]
			set;
		}

		// Token: 0x0600018C RID: 396
		void _VtblGap1_7();

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600018D RID: 397
		// (set) Token: 0x0600018E RID: 398
		[DispId(1005)]
		string TargetPath
		{
			[DispId(1005)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(1005)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			[param: MarshalAs(UnmanagedType.BStr)]
			[param: In]
			set;
		}
	}
}
