using System;

namespace Lenovo.Modern.CoreTypes.Events.FileSystem
{
	// Token: 0x0200003A RID: 58
	public sealed class FileSystemEventConstants
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000273 RID: 627 RVA: 0x00005100 File Offset: 0x00003300
		public static FileSystemEventConstants Get
		{
			get
			{
				FileSystemEventConstants result;
				if ((result = FileSystemEventConstants._eventConstants) == null)
				{
					FileSystemEventConstants fileSystemEventConstants = new FileSystemEventConstants();
					fileSystemEventConstants.FileSystemEventMonitorName = "FileSystemMonitor";
					fileSystemEventConstants.FileSystemDirectoryEventTrigger = "DirectoryChange";
					fileSystemEventConstants.FileSystemFileEventTrigger = "FileChange";
					fileSystemEventConstants.FileSystemEventVersion = "1.0.0.0";
					fileSystemEventConstants.FileSystemEventDataType = "FileSystemEvent";
					result = fileSystemEventConstants;
					FileSystemEventConstants._eventConstants = fileSystemEventConstants;
				}
				return result;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000274 RID: 628 RVA: 0x00005158 File Offset: 0x00003358
		// (set) Token: 0x06000275 RID: 629 RVA: 0x00005160 File Offset: 0x00003360
		public string FileSystemEventMonitorName { get; private set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000276 RID: 630 RVA: 0x00005169 File Offset: 0x00003369
		// (set) Token: 0x06000277 RID: 631 RVA: 0x00005171 File Offset: 0x00003371
		public string FileSystemDirectoryEventTrigger { get; private set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000517A File Offset: 0x0000337A
		// (set) Token: 0x06000279 RID: 633 RVA: 0x00005182 File Offset: 0x00003382
		public string FileSystemFileEventTrigger { get; private set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0000518B File Offset: 0x0000338B
		// (set) Token: 0x0600027B RID: 635 RVA: 0x00005193 File Offset: 0x00003393
		public string FileSystemEventVersion { get; private set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000519C File Offset: 0x0000339C
		// (set) Token: 0x0600027D RID: 637 RVA: 0x000051A4 File Offset: 0x000033A4
		public string FileSystemEventDataType { get; private set; }

		// Token: 0x0400010D RID: 269
		private static FileSystemEventConstants _eventConstants;
	}
}
