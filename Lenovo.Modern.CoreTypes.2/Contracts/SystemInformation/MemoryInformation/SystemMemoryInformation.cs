using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.SystemInformation.MemoryInformation
{
	// Token: 0x02000051 RID: 81
	public sealed class SystemMemoryInformation
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000376 RID: 886 RVA: 0x00005D66 File Offset: 0x00003F66
		// (set) Token: 0x06000377 RID: 887 RVA: 0x00005D6E File Offset: 0x00003F6E
		[XmlElement(ElementName = "NumberOfSlots")]
		public string NumberOfSlots { get; set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000378 RID: 888 RVA: 0x00005D77 File Offset: 0x00003F77
		// (set) Token: 0x06000379 RID: 889 RVA: 0x00005D7F File Offset: 0x00003F7F
		[XmlElement(ElementName = "MaxCapacity")]
		public string MaxCapacity { get; set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600037A RID: 890 RVA: 0x00005D88 File Offset: 0x00003F88
		// (set) Token: 0x0600037B RID: 891 RVA: 0x00005D90 File Offset: 0x00003F90
		[XmlElement(ElementName = "InstalledMemory")]
		public string InstalledMemory { get; set; }
	}
}
