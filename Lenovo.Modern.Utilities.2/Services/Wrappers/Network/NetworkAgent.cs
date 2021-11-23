using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.Utilities.Services.Logging;
using NETWORKLIST;

namespace Lenovo.Modern.Utilities.Services.Wrappers.Network
{
	// Token: 0x0200001C RID: 28
	public class NetworkAgent : INetworkAgent
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00003408 File Offset: 0x00001608
		public NetworkAgent()
		{
			ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			NetworkChange.NetworkAvailabilityChanged += this.NetworkChange_NetworkAvailabilityChanged;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003440 File Offset: 0x00001640
		public async Task<Stream> GetHttpResponseAsStreamAsync(Uri url)
		{
			Stream responseStream = new MemoryStream();
			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage httpResponseMessage = await client.GetAsync(url);
				using (HttpResponseMessage response = httpResponseMessage)
				{
					if (response != null && response.IsSuccessStatusCode)
					{
						await(await response.Content.ReadAsStreamAsync()).CopyToAsync(responseStream);
						responseStream.Seek(0L, SeekOrigin.Begin);
					}
				}
				HttpResponseMessage response = null;
			}
			HttpClient client = null;
			return responseStream;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003488 File Offset: 0x00001688
		public async Task<string> GetHttpResponseAsStringAsync(Uri url)
		{
			string responseString = string.Empty;
			Stream stream = await this.GetHttpResponseAsStreamAsync(url);
			using (Stream responseStream = stream)
			{
				if (responseStream != null)
				{
					using (StreamReader reader = new StreamReader(responseStream))
					{
						responseString = await reader.ReadToEndAsync();
					}
					StreamReader reader = null;
				}
			}
			Stream responseStream = null;
			return responseString;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000034D8 File Offset: 0x000016D8
		public async Task<bool> GetHttpPostResponseAsStreamAsync(Uri url, Dictionary<string, string> postData, Stream responseStreamHolder)
		{
			bool didWork = false;
			if (responseStreamHolder == null)
			{
				throw new ArgumentNullException("responseStreamHolder");
			}
			FormUrlEncodedContent content = new FormUrlEncodedContent(postData);
			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage httpResponseMessage;
				if (postData != null)
				{
					httpResponseMessage = await client.PostAsync(url, content);
				}
				else
				{
					httpResponseMessage = await client.PostAsync(url, null);
				}
				using (HttpResponseMessage response = httpResponseMessage)
				{
					if (response != null)
					{
						Stream stream = await response.Content.ReadAsStreamAsync();
						if (responseStreamHolder != null)
						{
							await stream.CopyToAsync(responseStreamHolder);
							responseStreamHolder.Seek(0L, SeekOrigin.Begin);
							didWork = true;
						}
					}
				}
				HttpResponseMessage response = null;
			}
			HttpClient client = null;
			return didWork;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003530 File Offset: 0x00001730
		public async Task<string> GetHttpPostResponseAsStringAsync(Uri url, Dictionary<string, string> postData)
		{
			string responseString = string.Empty;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				TaskAwaiter<bool> taskAwaiter = this.GetHttpPostResponseAsStreamAsync(url, postData, memoryStream).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					using (StreamReader reader = new StreamReader(memoryStream))
					{
						responseString = await reader.ReadToEndAsync();
					}
					StreamReader reader = null;
				}
			}
			MemoryStream memoryStream = null;
			return responseString;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003588 File Offset: 0x00001788
		public async Task<Dictionary<string, IEnumerable<string>>> GetHttpResponseHeadersAsync(Uri url)
		{
			Dictionary<string, IEnumerable<string>> headers = new Dictionary<string, IEnumerable<string>>();
			using (HttpClient client = new HttpClient())
			{
				using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Head, url))
				{
					using (HttpResponseMessage httpResponseMessage = await client.SendAsync(requestMessage))
					{
						if (httpResponseMessage != null && httpResponseMessage.Headers != null && httpResponseMessage.Headers.Any<KeyValuePair<string, IEnumerable<string>>>())
						{
							foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in httpResponseMessage.Headers)
							{
								headers.Add(keyValuePair.Key, keyValuePair.Value);
							}
						}
					}
				}
				HttpRequestMessage requestMessage = null;
			}
			HttpClient client = null;
			return headers;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000035D0 File Offset: 0x000017D0
		public async Task<bool> DownloadToFileAsync(Uri downloadUrl, string targetFilePath, CancellationToken cancelToken)
		{
			NetworkAgent.<>c__DisplayClass7_0 CS$<>8__locals1 = new NetworkAgent.<>c__DisplayClass7_0();
			CS$<>8__locals1.downloadUrl = downloadUrl;
			CS$<>8__locals1.targetFilePath = targetFilePath;
			CS$<>8__locals1.cancelToken = cancelToken;
			CS$<>8__locals1.wasDownloaded = false;
			string expandedPath = Environment.ExpandEnvironmentVariables(CS$<>8__locals1.targetFilePath);
			NetworkAgent.<>c__DisplayClass7_1 CS$<>8__locals2 = new NetworkAgent.<>c__DisplayClass7_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.client = new WebClient();
			try
			{
				NetworkAgent.<>c__DisplayClass7_2 CS$<>8__locals3 = new NetworkAgent.<>c__DisplayClass7_2();
				CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
				CS$<>8__locals3.completeEvent = new ManualResetEventSlim(false);
				try
				{
					NetworkAgent.<>c__DisplayClass7_3 CS$<>8__locals4 = new NetworkAgent.<>c__DisplayClass7_3();
					CS$<>8__locals4.CS$<>8__locals3 = CS$<>8__locals3;
					CS$<>8__locals4.previousBytesDownloaded = -1L;
					CS$<>8__locals4.bytesDownloaded = -1L;
					try
					{
						CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.cancelToken.Register(delegate()
						{
							try
							{
								CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.client.CancelAsync();
							}
							catch (Exception ex5)
							{
								Logger.Log(ex5, "Exception trying to cancel WebClient download");
							}
						});
					}
					catch (Exception ex)
					{
						Logger.Log(ex, "Exception trying to register for cancellation token cancellation");
					}
					CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.client.DownloadProgressChanged += delegate(object sender, DownloadProgressChangedEventArgs e)
					{
						CS$<>8__locals4.bytesDownloaded = e.BytesReceived;
					};
					CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.client.DownloadFileCompleted += delegate(object sender, AsyncCompletedEventArgs e)
					{
						CS$<>8__locals4.CS$<>8__locals3.completeEvent.Set();
						if (e.Error == null)
						{
							CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.wasDownloaded = true;
							Logger.Log(Logger.LogSeverity.Information, "NetworkAgent: Download completed");
							return;
						}
						Logger.Log(Logger.LogSeverity.Information, "NetworkAgent: Error occured while downloading - {0} {1}", new object[]
						{
							e.Error.Message,
							(e.Error.InnerException != null) ? (" - ErrorDetails: " + e.Error.InnerException.Message) : " "
						});
					};
					bool flag = true;
					int numRetries = 0;
					while (flag && numRetries < this._maxRetries)
					{
						try
						{
							CS$<>8__locals4.CS$<>8__locals3.completeEvent.Reset();
							CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.wasDownloaded = false;
							CS$<>8__locals4.previousBytesDownloaded = -1L;
							CS$<>8__locals4.bytesDownloaded = -1L;
							CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.client.DownloadFileAsync(CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.downloadUrl, expandedPath);
							await Task.Factory.StartNew(delegate()
							{
								try
								{
									while (!CS$<>8__locals4.CS$<>8__locals3.completeEvent.Wait(25000, CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.cancelToken))
									{
										if (CS$<>8__locals4.previousBytesDownloaded == CS$<>8__locals4.bytesDownloaded)
										{
											CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.client.CancelAsync();
											Logger.Log(Logger.LogSeverity.Warning, "No progress trying to download package from {0} and save it to {1}", new object[]
											{
												CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.downloadUrl,
												CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.targetFilePath
											});
											break;
										}
										CS$<>8__locals4.previousBytesDownloaded = CS$<>8__locals4.bytesDownloaded;
									}
								}
								catch (Exception ex5)
								{
									Logger.Log(ex5, "Exception in wait code while trying to download update package from {0} and save it to {1}", new object[]
									{
										CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.downloadUrl,
										CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.targetFilePath
									});
								}
							}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
							flag = false;
						}
						catch (ArgumentNullException ex2)
						{
							Logger.Log(ex2, "ArgumentNullException while trying to download update package");
							flag = false;
						}
						catch (InvalidOperationException ex3)
						{
							Logger.Log(ex3, "InvalidOperationException while trying to download update package from {0} and save it to {1}", new object[]
							{
								CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.downloadUrl,
								CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.targetFilePath
							});
							flag = false;
						}
						catch (OperationCanceledException)
						{
							Logger.Log(Logger.LogSeverity.Information, "Download cancelled: {0}", new object[] { CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.downloadUrl });
							flag = false;
						}
						catch (Exception ex4)
						{
							Logger.Log(ex4, "Exception while trying to download update package from {0} and save it to {1}", new object[]
							{
								CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.downloadUrl,
								CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.targetFilePath
							});
							flag = false;
						}
						finally
						{
							numRetries++;
						}
					}
					CS$<>8__locals4 = null;
				}
				finally
				{
					if (CS$<>8__locals3.completeEvent != null)
					{
						((IDisposable)CS$<>8__locals3.completeEvent).Dispose();
					}
				}
				CS$<>8__locals3 = null;
			}
			finally
			{
				if (CS$<>8__locals2.client != null)
				{
					((IDisposable)CS$<>8__locals2.client).Dispose();
				}
			}
			CS$<>8__locals2 = null;
			return CS$<>8__locals1.wasDownloaded;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003630 File Offset: 0x00001830
		public static async Task<bool> DownloadFileAsync(Uri downloadUrl, string Path)
		{
			bool wasDownloaded = false;
			try
			{
				using (HttpClient client = new HttpClient())
				{
					Stream stream = await(await client.GetAsync(downloadUrl)).Content.ReadAsStreamAsync();
					using (FileStream fileStream = File.Create(Path, (int)stream.Length))
					{
						byte[] array = new byte[stream.Length];
						stream.Read(array, 0, array.Length);
						fileStream.Write(array, 0, array.Length);
					}
					wasDownloaded = true;
				}
				HttpClient client = null;
			}
			catch (Exception arg)
			{
				Logger.Log(Logger.LogSeverity.Error, "ConfigAgent : Downlad file failed!" + arg);
				return wasDownloaded;
			}
			return wasDownloaded;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003680 File Offset: 0x00001880
		public NetworkConnectivity GetNetworkConnectivity()
		{
			NetworkConnectivity result = NetworkConnectivity.NotConnected;
			if (NetworkAgent.IsInternetConnected())
			{
				result = NetworkConnectivity.InternetConnected;
			}
			return result;
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000096 RID: 150 RVA: 0x0000369C File Offset: 0x0000189C
		// (remove) Token: 0x06000097 RID: 151 RVA: 0x000036D4 File Offset: 0x000018D4
		public event NetworkAvailabilityChangedEventHandler NetworkChanged;

		// Token: 0x06000098 RID: 152 RVA: 0x0000370C File Offset: 0x0000190C
		private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
		{
			try
			{
				Logger.Log(Logger.LogSeverity.Information, "GetInternetConnectionStatus_1 returned {0}", new object[] { NetworkAgent.IsInternetConnected().ToString() });
				this.NetworkChanged(sender, e);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000375C File Offset: 0x0000195C
		private static bool IsInternetConnected()
		{
			bool result = false;
			try
			{
				result = ((NetworkListManager)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("DCB00C01-570F-4A9B-8D69-199FDBA5723B")))).IsConnectedToInternet;
			}
			catch (Exception ex)
			{
				Logger.Log(Logger.LogSeverity.Information, "IsInternetConnected: Exception {0}", new object[] { ex.Message });
			}
			return result;
		}

		// Token: 0x0600009A RID: 154
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("wininet.dll", SetLastError = true)]
		private static extern bool InternetGetConnectedState(out int lpdwFlags, int dwReserved);

		// Token: 0x0400001D RID: 29
		private readonly int _maxRetries = 2;
	}
}
