using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lenovo.Modern.CoreTypes.Events.FileSystem;
using Lenovo.Modern.ImController.ImClient.Models;
using Lenovo.Modern.ImController.Shared.Model.Packages;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Services;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController.EventManager.Services.EventMonitors.FilesSystem
{
	// Token: 0x0200001D RID: 29
	internal class FileSystemEventMonitor : EventMonitorBase
	{
		// Token: 0x0600008E RID: 142 RVA: 0x00005AC1 File Offset: 0x00003CC1
		public FileSystemEventMonitor()
		{
			this._fileSystemSubscriptionMappingList = new ConcurrentBag<EventSubscriptionMapping<FileSystemEventSubscription>>();
			this._fileOrDirectoryPath = new List<string>();
			this._fileDirWatchers = new ConcurrentBag<FileSystemWatcher>();
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00005AEA File Offset: 0x00003CEA
		public override string Version
		{
			get
			{
				return FileSystemMonitorConstants.Version;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00005AF1 File Offset: 0x00003CF1
		public override string Name
		{
			get
			{
				return FileSystemMonitorConstants.MonitorName;
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005AF8 File Offset: 0x00003CF8
		public override void RegisterSubscribedEvent(SubscribedEvent subscribedEvent)
		{
			string parameter = subscribedEvent.Parameter;
			FileSystemEventSubscription fileSystemEventSubscription = Serializer.Deserialize<FileSystemEventSubscription>(subscribedEvent.Parameter);
			if (fileSystemEventSubscription != null)
			{
				if (fileSystemEventSubscription.DirectoryList != null)
				{
					foreach (string text in fileSystemEventSubscription.DirectoryList)
					{
						if (!this._fileOrDirectoryPath.Contains(text))
						{
							this._fileOrDirectoryPath.Add(text);
							this.RegisterDirectoryWatch(text);
						}
					}
				}
				if (fileSystemEventSubscription.FileList != null)
				{
					foreach (string text2 in fileSystemEventSubscription.FileList)
					{
						if (!this._fileOrDirectoryPath.Contains(text2))
						{
							this._fileOrDirectoryPath.Add(text2);
							this.RegisterFileWatch(text2);
						}
					}
				}
			}
			this._fileSystemSubscriptionMappingList.Add(new EventSubscriptionMapping<FileSystemEventSubscription>(fileSystemEventSubscription, subscribedEvent));
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000344B File Offset: 0x0000164B
		public override Task<bool> InitializeAsync(EventHandlerReason reason)
		{
			return Task.FromResult<bool>(true);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00005BB8 File Offset: 0x00003DB8
		public override void Unregister(EventHandlerReason reason)
		{
			while (!this._fileSystemSubscriptionMappingList.IsEmpty)
			{
				EventSubscriptionMapping<FileSystemEventSubscription> eventSubscriptionMapping = null;
				this._fileSystemSubscriptionMappingList.TryTake(out eventSubscriptionMapping);
			}
			while (!this._fileDirWatchers.IsEmpty)
			{
				FileSystemWatcher fileSystemWatcher = null;
				this._fileDirWatchers.TryTake(out fileSystemWatcher);
				fileSystemWatcher.Dispose();
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005C0C File Offset: 0x00003E0C
		private void RegisterDirectoryWatch(string pathToDirectory)
		{
			if (!Utility.SanitizePath(ref pathToDirectory))
			{
				Logger.Log(Logger.LogSeverity.Error, "RegisterDirectoryWatch: Failed to RegisterDirectoryWatch as directory path is invalid. Path - {0}", new object[] { pathToDirectory });
				return;
			}
			FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
			fileSystemWatcher.Path = pathToDirectory;
			fileSystemWatcher.IncludeSubdirectories = true;
			fileSystemWatcher.Changed += this.OnDirectoryChanged;
			fileSystemWatcher.Created += this.OnDirectoryChanged;
			fileSystemWatcher.Deleted += this.OnDirectoryChanged;
			fileSystemWatcher.EnableRaisingEvents = true;
			this._fileDirWatchers.Add(fileSystemWatcher);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005C98 File Offset: 0x00003E98
		private void RegisterFileWatch(string pathToFile)
		{
			if (!Utility.SanitizePath(ref pathToFile))
			{
				Logger.Log(Logger.LogSeverity.Error, "RegisterFileWatch: Failed to RegisterFileWatch as file path is invalid. Path - {0}", new object[] { pathToFile });
				return;
			}
			FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
			fileSystemWatcher.Path = Path.GetDirectoryName(pathToFile);
			fileSystemWatcher.Filter = Path.GetFileName(pathToFile);
			fileSystemWatcher.Changed += this.OnFileChanged;
			fileSystemWatcher.Created += this.OnFileChanged;
			fileSystemWatcher.Deleted += this.OnFileChanged;
			fileSystemWatcher.EnableRaisingEvents = true;
			this._fileDirWatchers.Add(fileSystemWatcher);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005D2C File Offset: 0x00003F2C
		private void OnDirectoryChanged(object source, FileSystemEventArgs e)
		{
			FileSystemEventReaction fileSystemEventReaction = new FileSystemEventReaction();
			fileSystemEventReaction.ModifiedDirectory = e.FullPath;
			IEnumerable<SubscribedEvent> directoryRecepientsForEvent = this.GetDirectoryRecepientsForEvent(fileSystemEventReaction);
			string parameter = Serializer.Serialize<FileSystemEventReaction>(fileSystemEventReaction);
			EventReaction eventReaction = new EventReaction
			{
				Monitor = FileSystemMonitorConstants.MonitorName,
				DataType = FileSystemMonitorConstants.DataType,
				Trigger = FileSystemMonitorConstants.DirectoryTrigger,
				Parameter = parameter
			};
			foreach (SubscribedEvent subscribedEvent in directoryRecepientsForEvent)
			{
				base.NotifyObservers(eventReaction, subscribedEvent);
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005DC4 File Offset: 0x00003FC4
		private void OnFileChanged(object source, FileSystemEventArgs e)
		{
			FileSystemEventReaction fileSystemEventReaction = new FileSystemEventReaction();
			fileSystemEventReaction.ModifiedFile = e.FullPath;
			IEnumerable<SubscribedEvent> fileRecepientsForEvent = this.GetFileRecepientsForEvent(fileSystemEventReaction);
			string parameter = Serializer.Serialize<FileSystemEventReaction>(fileSystemEventReaction);
			EventReaction eventReaction = new EventReaction
			{
				Monitor = FileSystemMonitorConstants.MonitorName,
				DataType = FileSystemMonitorConstants.DataType,
				Trigger = FileSystemMonitorConstants.FileTrigger,
				Parameter = parameter
			};
			foreach (SubscribedEvent subscribedEvent in fileRecepientsForEvent)
			{
				base.NotifyObservers(eventReaction, subscribedEvent);
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005E5C File Offset: 0x0000405C
		private IEnumerable<SubscribedEvent> GetDirectoryRecepientsForEvent(FileSystemEventReaction fileSystemEvent)
		{
			List<SubscribedEvent> list = new List<SubscribedEvent>();
			foreach (EventSubscriptionMapping<FileSystemEventSubscription> eventSubscriptionMapping in this._fileSystemSubscriptionMappingList)
			{
				if (eventSubscriptionMapping.EventSubscriptionData.DirectoryList.Contains(fileSystemEvent.ModifiedDirectory))
				{
					list.Add(eventSubscriptionMapping.SubscribedEvent);
				}
			}
			return list;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00005ED0 File Offset: 0x000040D0
		private IEnumerable<SubscribedEvent> GetFileRecepientsForEvent(FileSystemEventReaction fileSystemEvent)
		{
			List<SubscribedEvent> list = new List<SubscribedEvent>();
			foreach (EventSubscriptionMapping<FileSystemEventSubscription> eventSubscriptionMapping in this._fileSystemSubscriptionMappingList)
			{
				if (eventSubscriptionMapping.EventSubscriptionData.FileList.Contains(fileSystemEvent.ModifiedFile))
				{
					list.Add(eventSubscriptionMapping.SubscribedEvent);
				}
			}
			return list;
		}

		// Token: 0x04000067 RID: 103
		private ConcurrentBag<EventSubscriptionMapping<FileSystemEventSubscription>> _fileSystemSubscriptionMappingList;

		// Token: 0x04000068 RID: 104
		private List<string> _fileOrDirectoryPath;

		// Token: 0x04000069 RID: 105
		private ConcurrentBag<FileSystemWatcher> _fileDirWatchers;
	}
}
