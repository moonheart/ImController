using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Lenovo.Modern.Utilities.Services.Wrappers.Network
{
	// Token: 0x0200001A RID: 26
	public interface INetworkAgent
	{
		// Token: 0x06000084 RID: 132
		Task<Stream> GetHttpResponseAsStreamAsync(Uri url);

		// Token: 0x06000085 RID: 133
		Task<string> GetHttpResponseAsStringAsync(Uri url);

		// Token: 0x06000086 RID: 134
		Task<bool> GetHttpPostResponseAsStreamAsync(Uri url, Dictionary<string, string> postData, Stream responseStreamHolder);

		// Token: 0x06000087 RID: 135
		Task<string> GetHttpPostResponseAsStringAsync(Uri url, Dictionary<string, string> postData);

		// Token: 0x06000088 RID: 136
		Task<Dictionary<string, IEnumerable<string>>> GetHttpResponseHeadersAsync(Uri url);

		// Token: 0x06000089 RID: 137
		Task<bool> DownloadToFileAsync(Uri downloadUrl, string targetFileLocation, CancellationToken cancelToken);

		// Token: 0x0600008A RID: 138
		NetworkConnectivity GetNetworkConnectivity();

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600008B RID: 139
		// (remove) Token: 0x0600008C RID: 140
		event NetworkAvailabilityChangedEventHandler NetworkChanged;
	}
}
