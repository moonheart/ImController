using System;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation
{
	// Token: 0x0200000D RID: 13
	public static class StringExtensions
	{
		// Token: 0x060000A5 RID: 165 RVA: 0x00006CC7 File Offset: 0x00004EC7
		public static bool Contains(this string source, string toCheck, StringComparison comp)
		{
			return source.IndexOf(toCheck, comp) >= 0;
		}
	}
}
