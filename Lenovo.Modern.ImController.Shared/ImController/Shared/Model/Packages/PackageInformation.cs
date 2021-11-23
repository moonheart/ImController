using System;
using System.Xml.Serialization;
using Lenovo.Modern.ImController.ImClient.Models;

namespace Lenovo.Modern.ImController.Shared.Model.Packages
{
	// Token: 0x0200002E RID: 46
	[XmlRoot("PackageInformation")]
	[Serializable]
	public sealed class PackageInformation
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00007513 File Offset: 0x00005713
		// (set) Token: 0x06000150 RID: 336 RVA: 0x0000751B File Offset: 0x0000571B
		[XmlAttribute("id")]
		public string Id { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00007524 File Offset: 0x00005724
		// (set) Token: 0x06000152 RID: 338 RVA: 0x0000752C File Offset: 0x0000572C
		[XmlAttribute("name")]
		public string Name { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00007535 File Offset: 0x00005735
		// (set) Token: 0x06000154 RID: 340 RVA: 0x0000753D File Offset: 0x0000573D
		[XmlAttribute("version")]
		public string Version { get; set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00007546 File Offset: 0x00005746
		// (set) Token: 0x06000156 RID: 342 RVA: 0x0000754E File Offset: 0x0000574E
		[XmlAttribute("location")]
		public string Location { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00007557 File Offset: 0x00005757
		// (set) Token: 0x06000158 RID: 344 RVA: 0x0000755F File Offset: 0x0000575F
		[XmlAttribute("msVersion")]
		public string MsVersion { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00007568 File Offset: 0x00005768
		// (set) Token: 0x0600015A RID: 346 RVA: 0x00007570 File Offset: 0x00005770
		[XmlAttribute("msLocation")]
		public string MsLocation { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00007579 File Offset: 0x00005779
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00007581 File Offset: 0x00005781
		[XmlAttribute("customEventMonitor")]
		public string CustomEventMonitor { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600015D RID: 349 RVA: 0x0000758A File Offset: 0x0000578A
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00007592 File Offset: 0x00005792
		[XmlAttribute("packageType")]
		public PackageType Packagetype { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600015F RID: 351 RVA: 0x0000759B File Offset: 0x0000579B
		// (set) Token: 0x06000160 RID: 352 RVA: 0x000075A3 File Offset: 0x000057A3
		[XmlIgnore]
		public DateTime DateChanged { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000161 RID: 353 RVA: 0x000075AC File Offset: 0x000057AC
		// (set) Token: 0x06000162 RID: 354 RVA: 0x000075E6 File Offset: 0x000057E6
		[XmlAttribute("datemodified")]
		public string DateModified
		{
			get
			{
				string result = string.Empty;
				if (this.DateChanged != DateTime.MinValue)
				{
					result = this.DateChanged.ToString(Constants.DateFormat);
				}
				return result;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					this.DateChanged = DateTime.Parse(value);
				}
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000163 RID: 355 RVA: 0x000075FC File Offset: 0x000057FC
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00007604 File Offset: 0x00005804
		[XmlAttribute("memoryManagementType")]
		public PluginType MemoryManagementType { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000165 RID: 357 RVA: 0x0000760D File Offset: 0x0000580D
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00007615 File Offset: 0x00005815
		public int NumberOfInstallAttempts { get; set; }

		// Token: 0x040000D5 RID: 213
		[XmlAttribute("pluginPrivileges")]
		public PluginPrivilege[] Privileges;
	}
}
