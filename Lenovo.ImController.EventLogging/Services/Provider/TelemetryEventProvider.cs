using System;
using System.Diagnostics.Eventing;
using System.Runtime.InteropServices;

namespace Lenovo.ImController.EventLogging.Services.Provider
{
	// Token: 0x02000012 RID: 18
	internal class TelemetryEventProvider : EventProvider
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002A40 File Offset: 0x00000C40
		internal TelemetryEventProvider(Guid id)
			: base(id)
		{
		}

		// Token: 0x02000022 RID: 34
		[StructLayout(LayoutKind.Explicit, Size = 16)]
		private struct EventData
		{
			// Token: 0x04000064 RID: 100
			[FieldOffset(0)]
			internal ulong DataPointer;

			// Token: 0x04000065 RID: 101
			[FieldOffset(8)]
			internal uint Size;

			// Token: 0x04000066 RID: 102
			[FieldOffset(12)]
			internal int Reserved;
		}
	}
}
