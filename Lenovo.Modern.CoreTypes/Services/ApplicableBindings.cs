using System;
using System.Collections.Generic;

namespace Lenovo.Modern.CoreTypes.Services
{
	// Token: 0x02000008 RID: 8
	public sealed class ApplicableBindings
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000033E7 File Offset: 0x000015E7
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000033EF File Offset: 0x000015EF
		public IEnumerable<Uri> ImageValues { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000033F8 File Offset: 0x000015F8
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00003400 File Offset: 0x00001600
		public IEnumerable<string> TextValues { get; set; }
	}
}
