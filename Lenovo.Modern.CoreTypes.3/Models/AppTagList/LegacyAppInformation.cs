using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.AppTagList
{
	// Token: 0x0200001B RID: 27
	public sealed class LegacyAppInformation
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00004311 File Offset: 0x00002511
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00004319 File Offset: 0x00002519
		[XmlAttribute(AttributeName = "displayName")]
		public string DisplayName { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00004322 File Offset: 0x00002522
		// (set) Token: 0x06000134 RID: 308 RVA: 0x0000432A File Offset: 0x0000252A
		[XmlAttribute(AttributeName = "installDirectory")]
		public string InstallDirectory { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00004333 File Offset: 0x00002533
		// (set) Token: 0x06000136 RID: 310 RVA: 0x0000433B File Offset: 0x0000253B
		[XmlAttribute(AttributeName = "executableName")]
		public string ExecutableName { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00004344 File Offset: 0x00002544
		// (set) Token: 0x06000138 RID: 312 RVA: 0x0000434C File Offset: 0x0000254C
		[XmlAttribute(AttributeName = "launchCommand")]
		public string LaunchCommand { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00004355 File Offset: 0x00002555
		// (set) Token: 0x0600013A RID: 314 RVA: 0x0000435D File Offset: 0x0000255D
		[XmlAttribute(AttributeName = "launchArguments")]
		public string LaunchArguments { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00004366 File Offset: 0x00002566
		// (set) Token: 0x0600013C RID: 316 RVA: 0x0000436E File Offset: 0x0000256E
		[XmlIgnore]
		public DateTimeOffset DateModified { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00004378 File Offset: 0x00002578
		// (set) Token: 0x0600013E RID: 318 RVA: 0x000043B7 File Offset: 0x000025B7
		[XmlAttribute("dateModified")]
		public string XmlDateModifiedDoNotUse
		{
			get
			{
				string result = string.Empty;
				if (this.DateModified != DateTimeOffset.MinValue)
				{
					result = this.DateModified.ToString(Constants.DateTimeFormat, CultureInfo.InvariantCulture);
				}
				return result;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					this.DateModified = DateTimeOffset.Parse(value);
				}
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600013F RID: 319 RVA: 0x000043CD File Offset: 0x000025CD
		// (set) Token: 0x06000140 RID: 320 RVA: 0x000043D5 File Offset: 0x000025D5
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		// Token: 0x06000141 RID: 321 RVA: 0x000043DE File Offset: 0x000025DE
		public override string ToString()
		{
			string result;
			if ((result = this.DisplayName) == null)
			{
				result = this.ExecutableName ?? string.Empty;
			}
			return result;
		}
	}
}
