using System;
using Lenovo.Modern.ImController.ImClient.Models;

namespace Lenovo.Modern.ImController.PluginHost.Services
{
	// Token: 0x02000008 RID: 8
	public interface IRequestProcessor
	{
		// Token: 0x0600001C RID: 28
		string ProcessRequest(PluginRequestInformation pluginRequestInfo);
	}
}
