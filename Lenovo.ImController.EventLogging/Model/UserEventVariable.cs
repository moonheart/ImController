using System;

namespace Lenovo.ImController.EventLogging.Model
{
	// Token: 0x02000015 RID: 21
	public sealed class UserEventVariable
	{
		// Token: 0x06000042 RID: 66 RVA: 0x00002500 File Offset: 0x00000700
		public UserEventVariable()
		{
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002BD8 File Offset: 0x00000DD8
		public UserEventVariable(int id, string name, string value)
		{
			this.Id = id.ToString();
			this.Name = name;
			this.Value = value;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002BFB File Offset: 0x00000DFB
		public UserEventVariable(string name, string value)
		{
			this.Id = string.Empty;
			this.Name = name;
			this.Value = value;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002C1C File Offset: 0x00000E1C
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00002C24 File Offset: 0x00000E24
		public string Id { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002C2D File Offset: 0x00000E2D
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00002C35 File Offset: 0x00000E35
		public string Name { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002C3E File Offset: 0x00000E3E
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002C46 File Offset: 0x00000E46
		public string Value { get; set; }

		// Token: 0x0600004B RID: 75 RVA: 0x00002C4F File Offset: 0x00000E4F
		public override string ToString()
		{
			return this.Name + "::" + this.Value;
		}
	}
}
