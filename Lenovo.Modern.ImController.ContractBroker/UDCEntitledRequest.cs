using System;
using System.Runtime.Serialization;
using Lenovo.Modern.ImController.Shared;

namespace Lenovo.Modern.ImController.ContractBroker
{
	// Token: 0x02000008 RID: 8
	[DataContract]
	public class UDCEntitledRequest
	{
		// Token: 0x0600003C RID: 60 RVA: 0x0000233F File Offset: 0x0000053F
		public UDCEntitledRequest(string aName, string aVersion)
		{
			this.appName = aName;
			this.appVersion = aVersion;
			this.imc = Constants.ImControllerVersion;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002360 File Offset: 0x00000560
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002368 File Offset: 0x00000568
		[DataMember(Name = "appName")]
		public string appName { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002371 File Offset: 0x00000571
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002379 File Offset: 0x00000579
		[DataMember(Name = "appVersion")]
		public string appVersion { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002382 File Offset: 0x00000582
		// (set) Token: 0x06000042 RID: 66 RVA: 0x0000238A File Offset: 0x0000058A
		[DataMember(Name = "imc")]
		public string imc { get; set; }
	}
}
