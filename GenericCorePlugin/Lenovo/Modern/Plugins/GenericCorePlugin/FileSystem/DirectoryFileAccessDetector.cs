using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.FileSystem
{
	// Token: 0x02000015 RID: 21
	public class DirectoryFileAccessDetector
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x000077AC File Offset: 0x000059AC
		public DirectoryFileAccessDetector()
		{
			this._currentUser = WindowsIdentity.GetCurrent();
			this._currentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000077D0 File Offset: 0x000059D0
		public bool HasAccess(DirectoryInfo directory, FileSystemRights right)
		{
			AuthorizationRuleCollection accessRules = directory.GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));
			return this.HasFileOrDirectoryAccess(right, accessRules);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00007800 File Offset: 0x00005A00
		public bool HasAccess(FileInfo file, FileSystemRights right)
		{
			AuthorizationRuleCollection accessRules = file.GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));
			return this.HasFileOrDirectoryAccess(right, accessRules);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00007830 File Offset: 0x00005A30
		private bool HasFileOrDirectoryAccess(FileSystemRights right, AuthorizationRuleCollection acl)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int i = 0; i < acl.Count; i++)
			{
				FileSystemAccessRule fileSystemAccessRule = (FileSystemAccessRule)acl[i];
				if ((this._currentUser != null && null != this._currentUser.User && this._currentUser.User.Equals(fileSystemAccessRule.IdentityReference)) || (this._currentPrincipal != null && this._currentPrincipal.IsInRole((SecurityIdentifier)fileSystemAccessRule.IdentityReference)))
				{
					if (fileSystemAccessRule.AccessControlType.Equals(AccessControlType.Deny))
					{
						if ((fileSystemAccessRule.FileSystemRights & right) == right)
						{
							if (!fileSystemAccessRule.IsInherited)
							{
								return false;
							}
							flag3 = true;
						}
					}
					else if (fileSystemAccessRule.AccessControlType.Equals(AccessControlType.Allow) && (fileSystemAccessRule.FileSystemRights & right) == right)
					{
						if (fileSystemAccessRule.IsInherited)
						{
							flag2 = true;
						}
						else
						{
							flag = true;
						}
					}
				}
			}
			return flag || (flag2 && !flag3);
		}

		// Token: 0x04000043 RID: 67
		private WindowsIdentity _currentUser;

		// Token: 0x04000044 RID: 68
		private WindowsPrincipal _currentPrincipal;
	}
}
