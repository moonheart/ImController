using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using Lenovo.Modern.ImController.Shared;

namespace Lenovo.Modern.ImController
{
	// Token: 0x02000011 RID: 17
	[RunInstaller(true)]
	public class ProjectInstaller : Installer
	{
		// Token: 0x06000035 RID: 53 RVA: 0x000036A4 File Offset: 0x000018A4
		public ProjectInstaller()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000036B4 File Offset: 0x000018B4
		private void ProjectInstaller_Committed(object sender, InstallEventArgs e)
		{
			List<SC_ACTION> list = new List<SC_ACTION>();
			list.Add(new SC_ACTION
			{
				Type = 1,
				Delay = 60
			});
			list.Add(new SC_ACTION
			{
				Type = 1,
				Delay = 60
			});
			list.Add(new SC_ACTION
			{
				Type = 1,
				Delay = 60
			});
			try
			{
				ServiceRecoveryProperty.ChangeRecoveryProperty(Constants.ImControllerServiceName, list, 604800, "", false, "");
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003758 File Offset: 0x00001958
		protected override void Dispose(bool disposing)
		{
			if (disposing && this._components != null)
			{
				this._components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003778 File Offset: 0x00001978
		private void InitializeComponent()
		{
			this._imControllerServiceProcessInstaller = new ServiceProcessInstaller();
			this._imControllerInstaller = new ServiceInstaller();
			this._imControllerServiceProcessInstaller.Account = ServiceAccount.LocalSystem;
			this._imControllerServiceProcessInstaller.Password = null;
			this._imControllerServiceProcessInstaller.Username = null;
			this._imControllerServiceProcessInstaller.Committed += this.ProjectInstaller_Committed;
			this._imControllerInstaller.DisplayName = Constants.ImControllerServiceDisplayName;
			this._imControllerInstaller.ServiceName = Constants.ImControllerServiceName;
			this._imControllerInstaller.StartType = ServiceStartMode.Automatic;
			this._imControllerInstaller.Description = Constants.ImControllerServiceDescription;
			base.Installers.AddRange(new Installer[] { this._imControllerServiceProcessInstaller, this._imControllerInstaller });
		}

		// Token: 0x04000038 RID: 56
		private IContainer _components;

		// Token: 0x04000039 RID: 57
		private ServiceProcessInstaller _imControllerServiceProcessInstaller;

		// Token: 0x0400003A RID: 58
		private ServiceInstaller _imControllerInstaller;
	}
}
