using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Principal;
using Lenovo.Modern.Utilities.Services.SystemContext.Interop;
using Lenovo.Modern.Utilities.Services.SystemContext.Settings;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.Utilities.Services.SystemContext.Shared
{
	// Token: 0x0200002D RID: 45
	public class UserInformationProvider : IUserInformationProvider
	{
		// Token: 0x06000118 RID: 280 RVA: 0x00002100 File Offset: 0x00000300
		private UserInformationProvider()
		{
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00005F07 File Offset: 0x00004107
		public static IUserInformationProvider Instance
		{
			get
			{
				if (UserInformationProvider.instance == null)
				{
					UserInformationProvider.instance = new UserInformationProvider();
				}
				return UserInformationProvider.instance;
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005F20 File Offset: 0x00004120
		UserInformation IUserInformationProvider.GetUserInformation()
		{
			Tuple<string, string> loggedInUserNameAndHostName = UserInformationProvider.GetLoggedInUserNameAndHostName();
			return new UserInformation
			{
				SID = UserInformationProvider.GetLoggedInUserSID(),
				UserProfileFolder = this.GetUserProfileFolder(),
				UserName = loggedInUserNameAndHostName.Item1,
				HostName = loggedInUserNameAndHostName.Item2
			};
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005F68 File Offset: 0x00004168
		public static string GetLoggedInUserSID()
		{
			string text = string.Empty;
			string loggedInUsernameBySessionId = Authorization.GetLoggedInUsernameBySessionId(true);
			if (!string.IsNullOrEmpty(loggedInUsernameBySessionId))
			{
				try
				{
					text = ((SecurityIdentifier)new NTAccount(loggedInUsernameBySessionId).Translate(typeof(SecurityIdentifier))).ToString();
				}
				catch (Exception)
				{
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_Process where Description = 'explorer.exe'"))
				{
					if (managementObjectSearcher != null)
					{
						using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
						{
							if (managementObjectCollection.Count != 0)
							{
								string[] array = new string[1];
								try
								{
									ManagementObject managementObject = managementObjectCollection.Cast<ManagementObject>().First<ManagementObject>();
									string methodName = "GetOwnerSid";
									object[] args = array;
									managementObject.InvokeMethod(methodName, args);
									text = array[0];
								}
								catch (Exception)
								{
								}
							}
						}
					}
				}
			}
			return text;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00006050 File Offset: 0x00004250
		private static Tuple<string, string> GetLoggedInUserNameAndHostName()
		{
			string loggedInUsernameBySessionId = Authorization.GetLoggedInUsernameBySessionId(false);
			if (!string.IsNullOrEmpty(loggedInUsernameBySessionId))
			{
				return Tuple.Create<string, string>(loggedInUsernameBySessionId, Environment.MachineName);
			}
			try
			{
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_Process where Description = 'explorer.exe'"))
				{
					if (managementObjectSearcher != null)
					{
						using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
						{
							if (managementObjectCollection.Count != 0)
							{
								string[] array = new string[2];
								ManagementObject managementObject = managementObjectCollection.Cast<ManagementObject>().First<ManagementObject>();
								string methodName = "GetOwner";
								object[] args = array;
								managementObject.InvokeMethod(methodName, args);
								return Tuple.Create<string, string>(array[0], array[1]);
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return Tuple.Create<string, string>(Environment.UserName, Environment.MachineName);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00006120 File Offset: 0x00004320
		private string GetUserProfileFolder()
		{
			string result = string.Empty;
			IContainer container = new SystemContextRegistrySystem().LoadContainer("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\ProfileList\\" + UserInformationProvider.GetLoggedInUserSID());
			if (container != null)
			{
				result = new DirectoryInfo(container.GetValue("ProfileImagePath").GetValueAsString()).FullName;
			}
			return result;
		}

		// Token: 0x04000050 RID: 80
		private static IUserInformationProvider instance;

		// Token: 0x04000051 RID: 81
		private const string ProfileImagePath = "ProfileImagePath";

		// Token: 0x04000052 RID: 82
		private const string HKLM_ProfileList = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\ProfileList\\";
	}
}
