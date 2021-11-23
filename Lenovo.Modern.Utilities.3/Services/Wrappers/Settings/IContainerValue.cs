using System;

namespace Lenovo.Modern.Utilities.Services.Wrappers.Settings
{
	// Token: 0x02000013 RID: 19
	public interface IContainerValue
	{
		// Token: 0x06000062 RID: 98
		string GetName();

		// Token: 0x06000063 RID: 99
		string GetValueAsString();

		// Token: 0x06000064 RID: 100
		int? GetValueAsInt();

		// Token: 0x06000065 RID: 101
		bool? GetValueAsBool();
	}
}
