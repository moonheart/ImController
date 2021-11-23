using System;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation
{
	// Token: 0x0200000C RID: 12
	public class SMBiosInformation
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00006BEA File Offset: 0x00004DEA
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00006BF2 File Offset: 0x00004DF2
		public string BiosVersion { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00006BFB File Offset: 0x00004DFB
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00006C03 File Offset: 0x00004E03
		public string BiosDate { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00006C0C File Offset: 0x00004E0C
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00006C14 File Offset: 0x00004E14
		public string ECVersion { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00006C1D File Offset: 0x00004E1D
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00006C25 File Offset: 0x00004E25
		public string Manufacturer { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00006C2E File Offset: 0x00004E2E
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00006C36 File Offset: 0x00004E36
		public string MTM { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00006C3F File Offset: 0x00004E3F
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00006C47 File Offset: 0x00004E47
		public string SerialNumber { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00006C50 File Offset: 0x00004E50
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00006C58 File Offset: 0x00004E58
		public string IdeaSerialNumber { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00006C61 File Offset: 0x00004E61
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00006C69 File Offset: 0x00004E69
		public string Version { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00006C72 File Offset: 0x00004E72
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00006C7A File Offset: 0x00004E7A
		public string SKU { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00006C83 File Offset: 0x00004E83
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00006C8B File Offset: 0x00004E8B
		public string Family { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00006C94 File Offset: 0x00004E94
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00006C9C File Offset: 0x00004E9C
		public string EnclosureType { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00006CA5 File Offset: 0x00004EA5
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00006CAD File Offset: 0x00004EAD
		public string AssetTagNumber { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00006CB6 File Offset: 0x00004EB6
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00006CBE File Offset: 0x00004EBE
		public string ProductName { get; set; }
	}
}
