using System;
using System.Collections.Generic;
using Lenovo.Modern.ImController.EventManager.Services;
using Lenovo.Modern.ImController.EventManager.Services.EventMonitors.AppMonitor;
using Lenovo.Modern.ImController.EventManager.Services.EventMonitors.FilesSystem;
using Lenovo.Modern.ImController.EventManager.Services.EventMonitors.ImController;
using Lenovo.Modern.ImController.EventManager.Services.EventMonitors.Network;
using Lenovo.Modern.ImController.EventManager.Services.EventMonitors.Registry;
using Lenovo.Modern.ImController.EventManager.Services.EventMonitors.SystemEvent;
using Lenovo.Modern.ImController.EventManager.Services.EventMonitors.TimeBased;
using Lenovo.Modern.ImController.EventManager.Services.EventMonitors.UsageMonitor;
using Lenovo.Modern.ImController.EventManager.Services.EventMonitors.WindowMessage;
using Lenovo.Modern.ImController.Shared.Services;
using Lenovo.Modern.ImController.Shared.Services.Contracts.SystemInformation.AppTag;
using Lenovo.Modern.Utilities.Patterns.Ioc;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Network;

namespace Lenovo.Modern.ImController.EventManager
{
	// Token: 0x02000002 RID: 2
	public static class EventManagerFactory
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static EventManager GetEventManagerInstance(IntPtr serviceHandle, ref ServiceControlHandlerDelegate svcCtrlHandlerEx)
		{
			try
			{
				if (EventManagerFactory._eventManagerInstance == null)
				{
					ISubscriptionManager instance = SubscriptionManager.GetInstance(new NetworkAgent());
					IMachineInformationManager instance2 = MachineInformationManager.GetInstance();
					IAppAndTagManager instance3 = AppAndTagManager.GetInstance();
					IPluginManager pluginManager = InstanceContainer.GetInstance().Resolve<IPluginManager>();
					FileSystemEventMonitor item = new FileSystemEventMonitor();
					RegistryMonitor item2 = new RegistryMonitor();
					WindowMessageMonitor item3 = new WindowMessageMonitor(serviceHandle, ref svcCtrlHandlerEx);
					NetworkMonitor item4 = new NetworkMonitor();
					WlanMonitor item5 = new WlanMonitor();
					SystemEventMonitor item6 = new SystemEventMonitor();
					ImControllerServiceEventMonitor item7 = new ImControllerServiceEventMonitor();
					TimeBasedMonitor item8 = new TimeBasedMonitor(instance2);
					HardcodedEventPrioritizer eventPrioritizer = new HardcodedEventPrioritizer();
					AppMonitorMonitor item9 = new AppMonitorMonitor();
					UsageMonitor item10 = new UsageMonitor();
					IEnumerable<IEventMonitor> eventMonitors = new List<IEventMonitor> { item7, item6, item, item2, item3, item4, item5, item8, item9, item10 };
					EventManagerFactory._eventManagerInstance = new EventManager(instance, instance2, instance3, eventMonitors, pluginManager, eventPrioritizer);
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to initialize Event Manager instance");
			}
			return EventManagerFactory._eventManagerInstance;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002164 File Offset: 0x00000364
		public static EventManager GetEventManagerPreviouslyCreatedInstance()
		{
			return EventManagerFactory._eventManagerInstance;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000216B File Offset: 0x0000036B
		public static void DisposeEventManager()
		{
			EventManagerFactory._eventManagerInstance = null;
		}

		// Token: 0x04000001 RID: 1
		private static EventManager _eventManagerInstance;
	}
}
