using System;
using System.IO;
using System.Text.RegularExpressions;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.SystemContext.Shared;

namespace Lenovo.Modern.Utilities.Services.SystemContext
{
	// Token: 0x02000027 RID: 39
	public class SystemPathMapper : ISystemPathMapper
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x0000539E File Offset: 0x0000359E
		private SystemPathMapper()
		{
			this._userInformation = UserInformationProvider.Instance.GetUserInformation();
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x000053B6 File Offset: 0x000035B6
		public static ISystemPathMapper Instance
		{
			get
			{
				if (SystemPathMapper.instance == null)
				{
					SystemPathMapper.instance = new SystemPathMapper();
				}
				return SystemPathMapper.instance;
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000053D0 File Offset: 0x000035D0
		string ISystemPathMapper.GetUserContextFolder(string path)
		{
			try
			{
				Match match = new Regex("%(.*)%").Match(path);
				string text = string.Empty;
				if (match.Length > 0)
				{
					text = match.Groups[1].ToString();
					if (!string.IsNullOrEmpty(text))
					{
						string text2 = text.ToUpper();
						uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
						if (num <= 832146132U)
						{
							if (num != 68203685U)
							{
								if (num != 196715663U)
								{
									if (num == 832146132U)
									{
										if (text2 == "APPDATA")
										{
											path = path.Replace("%" + text + "%", this._userInformation.UserProfileFolder + Path.DirectorySeparatorChar.ToString() + "AppData\\Roaming");
										}
									}
								}
								else if (text2 == "USERPROFILE")
								{
									path = path.Replace("%" + text + "%", this._userInformation.UserProfileFolder);
								}
							}
							else if (text2 == "HOMEPATH")
							{
								path = path.Replace("%" + text + "%", this._userInformation.UserProfileFolder.Substring(4));
							}
						}
						else if (num <= 3273820103U)
						{
							if (num != 2777987627U)
							{
								if (num == 3273820103U)
								{
									if (text2 == "TEMP")
									{
										path = path.Replace("%" + text + "%", this._userInformation.UserProfileFolder + Path.DirectorySeparatorChar.ToString() + "AppData\\Local\\Temp");
									}
								}
							}
							else if (text2 == "LOCALAPPDATA")
							{
								path = path.Replace("%" + text + "%", this._userInformation.UserProfileFolder + Path.DirectorySeparatorChar.ToString() + "AppData\\Local");
							}
						}
						else if (num != 3569741241U)
						{
							if (num == 4115772078U)
							{
								if (text2 == "TMP")
								{
									path = path.Replace("%" + text + "%", this._userInformation.UserProfileFolder + Path.DirectorySeparatorChar.ToString() + "AppData\\Local\\Temp");
								}
							}
						}
						else if (text2 == "USERNAME")
						{
							path = path.Replace("%" + text + "%", this._userInformation.UserName);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Unable to map user context folder");
			}
			return path;
		}

		// Token: 0x04000044 RID: 68
		private static ISystemPathMapper instance;

		// Token: 0x04000045 RID: 69
		private UserInformation _userInformation;
	}
}
