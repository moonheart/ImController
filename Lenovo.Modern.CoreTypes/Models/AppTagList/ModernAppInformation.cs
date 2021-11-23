using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.AppTagList
{
	// Token: 0x0200001C RID: 28
	public sealed class ModernAppInformation
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000143 RID: 323 RVA: 0x000043F9 File Offset: 0x000025F9
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00004401 File Offset: 0x00002601
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000145 RID: 325 RVA: 0x0000440A File Offset: 0x0000260A
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00004412 File Offset: 0x00002612
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000147 RID: 327 RVA: 0x0000441B File Offset: 0x0000261B
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00004423 File Offset: 0x00002623
		[XmlAttribute(AttributeName = "installDirectory")]
		public string InstallDirectory { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000149 RID: 329 RVA: 0x0000442C File Offset: 0x0000262C
		// (set) Token: 0x0600014A RID: 330 RVA: 0x00004434 File Offset: 0x00002634
		[XmlAttribute(AttributeName = "dataDirectory")]
		public string DataDirectory { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000443D File Offset: 0x0000263D
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00004445 File Offset: 0x00002645
		[XmlAttribute(AttributeName = "familyName")]
		public string FamilyName { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600014D RID: 333 RVA: 0x0000444E File Offset: 0x0000264E
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00004456 File Offset: 0x00002656
		[XmlAttribute(AttributeName = "fullName")]
		public string FullName { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600014F RID: 335 RVA: 0x0000445F File Offset: 0x0000265F
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00004467 File Offset: 0x00002667
		[XmlAttribute(AttributeName = "publisher")]
		public string Publisher { get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00004470 File Offset: 0x00002670
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00004478 File Offset: 0x00002678
		[XmlAttribute(AttributeName = "protocol")]
		public string Protocol { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00004481 File Offset: 0x00002681
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00004489 File Offset: 0x00002689
		[XmlAttribute(AttributeName = "appUserModelId")]
		public string AppUserModelId { get; set; }

		// Token: 0x06000155 RID: 341 RVA: 0x00004492 File Offset: 0x00002692
		public override string ToString()
		{
			string result;
			if ((result = this.FullName) == null && (result = this.FamilyName) == null)
			{
				result = this.Name ?? string.Empty;
			}
			return result;
		}
	}
}
