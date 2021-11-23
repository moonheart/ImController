using System;

namespace Lenovo.Modern.ImController.PluginHost.Services
{
	// Token: 0x02000006 RID: 6
	public interface IInstanceEnforcer
	{
		// Token: 0x06000019 RID: 25
		void AssertOnlyInstance(string name);
	}
}
