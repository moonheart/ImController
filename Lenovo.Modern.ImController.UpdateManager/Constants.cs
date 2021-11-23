using System;

namespace Lenovo.Modern.ImController.UpdateManager
{
	// Token: 0x02000002 RID: 2
	internal static class Constants
	{
		// Token: 0x04000001 RID: 1
		internal static readonly string PackageTempLocation = "%PROGRAMDATA%\\Lenovo\\iMController\\Temp";

		// Token: 0x04000002 RID: 2
		internal static readonly string PendingPackagesTempLocation = "%PROGRAMDATA%\\Lenovo\\iMController\\Temp\\PendingPackages";

		// Token: 0x04000003 RID: 3
		internal static readonly string PackageLocation = "C:\\Windows\\Lenovo\\iMController";

		// Token: 0x04000004 RID: 4
		internal static readonly string PackagesBaseAddress = "https://filedownload.lenovo.com/enm/sift/packages/";

		// Token: 0x04000005 RID: 5
		internal static readonly string RootFolder = "%PROGRAMDATA%";

		// Token: 0x04000006 RID: 6
		internal static readonly string InfInstaller = "ImController.InfInstaller.exe";

		// Token: 0x04000007 RID: 7
		internal static readonly string InfInstallerSource = "x64\\ImController.InfInstaller\\x64_ImController.InfInstaller_ImController.InfInstaller.exe";

		// Token: 0x04000008 RID: 8
		internal static readonly string InfInstallerSourcex86 = "x86\\ImController.InfInstaller\\x86_ImController.InfInstaller_ImController.InfInstaller.exe";

		// Token: 0x04000009 RID: 9
		internal static readonly string PackageLocationSettingKeyName = "Imc.Update.PackageLocation";

		// Token: 0x0400000A RID: 10
		internal static readonly string MsiToInfBridgeSettingKyName = "Imc.Update.InfExe";

		// Token: 0x0400000B RID: 11
		internal static readonly string MsiLocationSettingKeyName = "Imc.Update.Legacy";

		// Token: 0x0400000C RID: 12
		internal static readonly string MsDownloadLocationSettingKeyName = "Imc.Update.MsCab";

		// Token: 0x0400000D RID: 13
		internal static readonly string ImcIsRollBackEnabled = "Imc.IsRollbackEnabled";

		// Token: 0x0400000E RID: 14
		internal static readonly string CachePackageKeyNameFormat = "{0}-{1}";

		// Token: 0x0400000F RID: 15
		internal static readonly int InstallAttemptThreshold = 5;

		// Token: 0x04000010 RID: 16
		internal static string InstallerFilePathPlaceholder = "%PROGRAMDATA%\\Lenovo\\iMController\\Temp\\SystemInterfaceFoundation.msi";

		// Token: 0x04000011 RID: 17
		internal static string MsInstallerFilePathPlaceholder = "%PROGRAMDATA%\\Lenovo\\iMController\\Temp\\SystemInterfaceFoundation.cab";

		// Token: 0x04000012 RID: 18
		internal static string BridgeInstallerFilePathPlaceholder = "%PROGRAMDATA%\\Lenovo\\iMController\\Temp\\SystemInterfaceFoundation.exe";

		// Token: 0x04000013 RID: 19
		internal static readonly int PendingUpdateTimerInterval = 120000;
	}
}
