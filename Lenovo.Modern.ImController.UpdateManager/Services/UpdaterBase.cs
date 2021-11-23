using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Network;
using Lenovo.Modern.Utilities.Services.Wrappers.Storage;

namespace Lenovo.Modern.ImController.UpdateManager.Services
{
	// Token: 0x0200000B RID: 11
	public abstract class UpdaterBase : IUpdater
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00003A31 File Offset: 0x00001C31
		private void NetworkChangedHandler(object sender, NetworkAvailabilityEventArgs netArgs)
		{
			if (netArgs != null && netArgs.IsAvailable)
			{
				this.RunNetworkConnectionHandlerUnwaitedTask();
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003A44 File Offset: 0x00001C44
		private void RunNetworkConnectionHandlerUnwaitedTask()
		{
			if (!this._networkConnectionHandled)
			{
				this._startOnNetworkSem.Wait();
				if (!this._networkConnectionHandled)
				{
					this._networkConnectionHandled = true;
					Task.Run(delegate()
					{
						try
						{
							Logger.Log(Logger.LogSeverity.Information, "UpdaterBase: Network connected. Waiting 30 seconds...");
							if (!this._stopEvent.WaitOne(30000))
							{
								Logger.Log(Logger.LogSeverity.Information, "UpdaterBase: Running updater payload");
								this.RunWhenNetworkConnects().Wait();
							}
						}
						catch (Exception ex)
						{
							Logger.Log(ex, "UpdaterBase: Exception in RunWhenNetworkConnects");
						}
					});
				}
				this._startOnNetworkSem.Release();
			}
		}

		// Token: 0x0600003C RID: 60
		protected abstract Task RunWhenNetworkConnects();

		// Token: 0x0600003D RID: 61 RVA: 0x00003A94 File Offset: 0x00001C94
		protected void StartHandlingNetwork()
		{
			if (this._networkAgent.GetNetworkConnectivity() != NetworkConnectivity.InternetConnected)
			{
				Logger.Log(Logger.LogSeverity.Information, "UpdaterBase: Network is not available, will hook to network event");
				if (!this._networkHandlerAdded)
				{
					this._networkHandlerAdded = true;
					this._networkAgent.NetworkChanged += this.NetworkChangedHandler;
					return;
				}
			}
			else
			{
				Logger.Log(Logger.LogSeverity.Information, "UpdaterBase: Network is available, will run updates now");
				this.RunNetworkConnectionHandlerUnwaitedTask();
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003AF2 File Offset: 0x00001CF2
		protected void SetStop()
		{
			this._stopEvent.Set();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003B00 File Offset: 0x00001D00
		private async Task<IDirectory> ClearAndCreateTempPackageFolder()
		{
			return await((IFileSystem)new WinFileSystem()).LoadDirectory(Environment.ExpandEnvironmentVariables(Constants.RootFolder)).CreateDirectoryAsync(Constants.PackageTempLocation, CreationOption.OpenIfExists);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003B3D File Offset: 0x00001D3D
		protected UpdaterBase(INetworkAgent networkAgent)
		{
			this._networkAgent = networkAgent;
			this._startOnNetworkSem = new SemaphoreSlim(1, 1);
			this._stopEvent = new ManualResetEvent(false);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003B68 File Offset: 0x00001D68
		protected async Task<bool> DownloadFileAsync(string url, string destinationPath, CancellationToken cancelToken)
		{
			string path = Environment.ExpandEnvironmentVariables(destinationPath);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			await this.ClearAndCreateTempPackageFolder();
			Uri uri = CacheBuster.MakeUnique(new Uri(url));
			Logger.Log(Logger.LogSeverity.Information, "UpdaterBase: Downloading file from {0} and save it to {1}", new object[] { uri, destinationPath });
			return await this._networkAgent.DownloadToFileAsync(uri, destinationPath, cancelToken);
		}

		// Token: 0x06000042 RID: 66
		public abstract void Start(CancellationToken cancelToken);

		// Token: 0x06000043 RID: 67
		public abstract void StopAndWait();

		// Token: 0x06000044 RID: 68
		public abstract void ApplyPendingUpdates();

		// Token: 0x04000028 RID: 40
		protected static int _startDelayMS = 180000;

		// Token: 0x04000029 RID: 41
		private readonly INetworkAgent _networkAgent;

		// Token: 0x0400002A RID: 42
		protected readonly ManualResetEvent _stopEvent;

		// Token: 0x0400002B RID: 43
		private SemaphoreSlim _startOnNetworkSem;

		// Token: 0x0400002C RID: 44
		private bool _networkConnectionHandled;

		// Token: 0x0400002D RID: 45
		private bool _networkHandlerAdded;
	}
}
