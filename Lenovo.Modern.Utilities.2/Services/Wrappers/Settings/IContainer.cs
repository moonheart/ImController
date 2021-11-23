using System;
using System.Collections.Generic;

namespace Lenovo.Modern.Utilities.Services.Wrappers.Settings
{
	// Token: 0x02000011 RID: 17
	public interface IContainer
	{
		// Token: 0x06000056 RID: 86
		IEnumerable<IContainerValue> GetValues(bool IsADSetting = true);

		// Token: 0x06000057 RID: 87
		string GetPath();

		// Token: 0x06000058 RID: 88
		IEnumerable<string> GetSubContainerNames();

		// Token: 0x06000059 RID: 89
		IContainer GetSubContainer(string containerName);

		// Token: 0x0600005A RID: 90
		bool CreateSubContainer(string containerName);

		// Token: 0x0600005B RID: 91
		bool CreateSubContainer(string containerName, IDictionary<string, string> containerValues);

		// Token: 0x0600005C RID: 92
		bool DeleteSubContainer(string containerName);

		// Token: 0x0600005D RID: 93
		bool SetValue(string valueName, string value);

		// Token: 0x0600005E RID: 94
		bool SetValue(string valueName, object value, RegistryKind registryKind);

		// Token: 0x0600005F RID: 95
		bool DeleteValue(string valueName);

		// Token: 0x06000060 RID: 96
		IContainerValue GetValue(string valueName);
	}
}
