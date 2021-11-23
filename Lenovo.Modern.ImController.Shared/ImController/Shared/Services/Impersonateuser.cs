using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.SystemContext.Interop;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x0200000B RID: 11
	public class Impersonateuser
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002DF8 File Offset: 0x00000FF8
		private bool ImpersonateToken(IntPtr hUserToken)
		{
			bool result = false;
			try
			{
				using (WindowsIdentity windowsIdentity = new WindowsIdentity(hUserToken))
				{
					this._impersonationContext = windowsIdentity.Impersonate();
					if (this._impersonationContext != null)
					{
						result = true;
					}
				}
			}
			catch (Exception)
			{
				Logger.Log(Logger.LogSeverity.Critical, "ImpersonateToken exception");
			}
			return result;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002E60 File Offset: 0x00001060
		public bool EnterImpersonation()
		{
			bool result = false;
			IntPtr zero = IntPtr.Zero;
			try
			{
				if (Authorization.GetSessionUserToken(ref zero))
				{
					result = this.ImpersonateToken(zero);
				}
			}
			catch (Exception)
			{
			}
			if (zero != IntPtr.Zero)
			{
				Impersonateuser.CloseHandle(zero);
			}
			return result;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002EB0 File Offset: 0x000010B0
		public bool EnterImpersonation(IntPtr hUserToken)
		{
			bool result = false;
			try
			{
				result = this.ImpersonateToken(hUserToken);
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002EE0 File Offset: 0x000010E0
		public void ExitImpersonation()
		{
			if (this._impersonationContext != null)
			{
				this._impersonationContext.Undo();
				this._impersonationContext.Dispose();
				this._impersonationContext = null;
			}
		}

		// Token: 0x0600002E RID: 46
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hSnapshot);

		// Token: 0x0400004C RID: 76
		private WindowsImpersonationContext _impersonationContext;
	}
}
