using System;
using System.Configuration.Install;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading.Tasks;
using Lenovo.Modern.ImController.CommandLineServices;
using Lenovo.Modern.ImController.Shared;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.ImController
{
	// Token: 0x02000010 RID: 16
	internal class Program
	{
		// Token: 0x0600002E RID: 46
		[DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool SetDefaultDllDirectories(uint directoryFlags);

		// Token: 0x0600002F RID: 47 RVA: 0x00003378 File Offset: 0x00001578
		[MTAThread]
		private static void Main(string[] args)
		{
			Program.SetDefaultDllDirectories(2048U);
			if (!Environment.UserInteractive && args.Length == 0)
			{
				ServiceBase.Run(new ServiceBase[]
				{
					new ImControllerService()
				});
				return;
			}
			if (args.Length >= 1)
			{
				try
				{
					bool flag = CommadLineResponder.HandleImcCommandLine(args);
					Logger.Log(Logger.LogSeverity.Information, string.Format("Program: Completed handling of CMD args.  Success?, {0}", flag));
					if (flag)
					{
						return;
					}
					Logger.Log(Logger.LogSeverity.Error, "Program: Completed CMD bug did not return");
				}
				catch (Exception ex)
				{
					Logger.Log(ex, "Critical: Program: Unable to handle IMC Command line args");
				}
			}
			bool flag2 = false;
			if (args.Length != 0)
			{
				string text = args[0].ToLowerInvariant();
				if (!(text == "/i") && !(text == "/install"))
				{
					if (!(text == "/uninstall") && !(text == "/u"))
					{
						if (!(text == "/start"))
						{
							if (!(text == "/stop"))
							{
								Logger.Log(Logger.LogSeverity.Error, "Program: Unknown command '" + text + "' passed");
							}
							else
							{
								Program.StopService();
							}
						}
						else
						{
							Program.StartService();
						}
					}
					else
					{
						Program.UninstallService();
					}
				}
				else
				{
					Program.InstallService();
				}
			}
			else
			{
				flag2 = true;
			}
			if (flag2)
			{
				Logger.Log(Logger.LogSeverity.Warning, "Program: Will run as APP");
				using (ImControllerService imControllerService = new ImControllerService())
				{
					imControllerService.InitSvcForRunasApp().Wait();
					imControllerService.SvcCtrlForRunAsApp(13, 18).Wait();
					Task.Delay(int.MaxValue).Wait();
					imControllerService.Stop();
				}
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000034FC File Offset: 0x000016FC
		private static int InstallService()
		{
			try
			{
				string[] array = new string[1];
				int num = 0;
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				array[num] = ((executingAssembly != null) ? executingAssembly.Location : null);
				ManagedInstallerClass.InstallHelper(array);
			}
			catch (Exception)
			{
			}
			return 0;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003540 File Offset: 0x00001740
		private static int UninstallService()
		{
			try
			{
				string[] array = new string[2];
				array[0] = "/u";
				int num = 1;
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				array[num] = ((executingAssembly != null) ? executingAssembly.Location : null);
				ManagedInstallerClass.InstallHelper(array);
			}
			catch (Exception)
			{
			}
			return 0;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000358C File Offset: 0x0000178C
		private static void StartService()
		{
			using (ServiceController serviceController = new ServiceController())
			{
				serviceController.ServiceName = Lenovo.Modern.ImController.Shared.Constants.ImControllerServiceName;
				try
				{
					serviceController.Refresh();
					if (serviceController.Status.Equals(ServiceControllerStatus.StartPending) || serviceController.Status.Equals(ServiceControllerStatus.Stopped))
					{
						serviceController.Start();
						serviceController.WaitForStatus(ServiceControllerStatus.Running);
					}
				}
				catch (InvalidOperationException)
				{
				}
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003620 File Offset: 0x00001820
		private static void StopService()
		{
			using (ServiceController serviceController = new ServiceController())
			{
				serviceController.ServiceName = Lenovo.Modern.ImController.Shared.Constants.ImControllerServiceName;
				try
				{
					serviceController.Refresh();
					if (serviceController.Status.Equals(ServiceControllerStatus.Running))
					{
						serviceController.Stop();
						serviceController.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(1500L));
					}
				}
				catch (InvalidOperationException)
				{
				}
			}
		}
	}
}
