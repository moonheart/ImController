using System;
using System.Collections.Generic;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation
{
	// Token: 0x0200000A RID: 10
	public class Constants
	{
		// Token: 0x04000024 RID: 36
		public static readonly string WindowsCurrentVersionRegistryKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion";

		// Token: 0x04000025 RID: 37
		public static readonly string WindowsProductNameRegistryValue = "ProductName";

		// Token: 0x04000026 RID: 38
		public static readonly string WindowsBuildVersionRegistryValue = "CurrentVersion";

		// Token: 0x04000027 RID: 39
		public static readonly string WindowsMajorVersionNumberRegistryValue = "CurrentMajorVersionNumber";

		// Token: 0x04000028 RID: 40
		public static readonly string WindowsMinorVersionNumberRegistryValue = "CurrentMinorVersionNumber";

		// Token: 0x04000029 RID: 41
		public static readonly string WindowsBuildNumberRegistryValue = "CurrentBuildNumber";

		// Token: 0x0400002A RID: 42
		public static readonly string WindowsUBRRegistryValue = "UBR";

		// Token: 0x0400002B RID: 43
		public static readonly string WindowsEditionIDRegistryValue = "EditionID";

		// Token: 0x0400002C RID: 44
		public static readonly string WindowsBIOSRegistryKey = "HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\System\\BIOS";

		// Token: 0x0400002D RID: 45
		public static readonly string WindowsBIOSReleaseDateRegistryValue = "BIOSReleaseDate";

		// Token: 0x0400002E RID: 46
		public static readonly string WindowsECFirmwareMajorReleaseRegistryValue = "ECFirmwareMajorRelease";

		// Token: 0x0400002F RID: 47
		public static readonly string WindowsECFirmwareMinorReleaseRegistryValue = "ECFirmwareMinorRelease";

		// Token: 0x04000030 RID: 48
		public static readonly IReadOnlyDictionary<string, string> CPUValueDictionary = new Dictionary<string, string>
		{
			{ "0", "x86" },
			{ "1", "MIPS" },
			{ "2", "Alpha" },
			{ "3", "PowerPC" },
			{ "5", "ARM" },
			{ "6", "Itanium-based systems" },
			{ "9", "x64" }
		};

		// Token: 0x04000031 RID: 49
		public static readonly string WindowsCPURegistryKey = "HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0";

		// Token: 0x04000032 RID: 50
		public static readonly string CPUIdentifierRegistryValue = "Identifier";

		// Token: 0x04000033 RID: 51
		public static readonly string HardwareConfigRegistryKey = "HKEY_LOCAL_MACHINE\\SYSTEM\\HardwareConfig\\Current";

		// Token: 0x02000065 RID: 101
		public static class SkuKeys
		{
			// Token: 0x0400012C RID: 300
			public const string Mt = "MT";

			// Token: 0x0400012D RID: 301
			public const string Family = "FM";

			// Token: 0x0400012E RID: 302
			public const string Brand = "BU";

			// Token: 0x0400012F RID: 303
			public const string Manufacturer = "MFCTR";
		}

		// Token: 0x02000066 RID: 102
		public static class BrandNames
		{
			// Token: 0x04000130 RID: 304
			public static readonly string Think = "Think";

			// Token: 0x04000131 RID: 305
			public static readonly string Idea = "Idea";

			// Token: 0x04000132 RID: 306
			public static readonly string Lenovo = "Lenovo";

			// Token: 0x04000133 RID: 307
			public static readonly string Other = "Other";
		}

		// Token: 0x02000067 RID: 103
		public static class SubBrands
		{
			// Token: 0x04000134 RID: 308
			public const string ThinkPad = "ThinkPad";

			// Token: 0x04000135 RID: 309
			public const string ThinkCentre = "ThinkCentre";

			// Token: 0x04000136 RID: 310
			public const string ThinkCenter = "ThinkCenter";

			// Token: 0x04000137 RID: 311
			public const string ThinkStation = "ThinkStation";

			// Token: 0x04000138 RID: 312
			public const string Erazer = "Erazer";

			// Token: 0x04000139 RID: 313
			public const string IdeaPad = "IdeaPad";

			// Token: 0x0400013A RID: 314
			public const string IdeaCentre = "IdeaCentre";

			// Token: 0x0400013B RID: 315
			public const string IdeaCenter = "IdeaCenter";

			// Token: 0x0400013C RID: 316
			public const string IdeaTab = "IdeaTab";

			// Token: 0x0400013D RID: 317
			public const string EdgeSuffix = "Edge";

			// Token: 0x0400013E RID: 318
			public const string Medion = "Medion";

			// Token: 0x0400013F RID: 319
			public const string Miix = "Miix";

			// Token: 0x04000140 RID: 320
			public const string Yoga = "Yoga";

			// Token: 0x04000141 RID: 321
			public const string Lenovo = "Lenovo";

			// Token: 0x04000142 RID: 322
			public const string Other = "Other";
		}
	}
}
