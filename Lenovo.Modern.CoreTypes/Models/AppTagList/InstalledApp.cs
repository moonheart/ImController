using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Models.AppTagList
{
	// Token: 0x0200001A RID: 26
	public sealed class InstalledApp
	{
		// Token: 0x06000121 RID: 289 RVA: 0x00003409 File Offset: 0x00001609
		public InstalledApp()
		{
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000041DB File Offset: 0x000023DB
		public InstalledApp(AppType type, string key, string displayName, string protocol, DateTimeOffset dateInstalled, string version)
		{
			this.Type = type;
			this.Key = key;
			this.DisplayName = displayName;
			this.Protocol = protocol;
			this.DateInstalled = dateInstalled;
			this.Version = version;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00004210 File Offset: 0x00002410
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00004218 File Offset: 0x00002418
		[XmlAttribute(AttributeName = "type")]
		public AppType Type { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00004221 File Offset: 0x00002421
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00004229 File Offset: 0x00002429
		[XmlAttribute(AttributeName = "key")]
		public string Key { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00004232 File Offset: 0x00002432
		// (set) Token: 0x06000128 RID: 296 RVA: 0x0000423A File Offset: 0x0000243A
		[XmlAttribute(AttributeName = "displayName")]
		public string DisplayName { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00004243 File Offset: 0x00002443
		// (set) Token: 0x0600012A RID: 298 RVA: 0x0000424B File Offset: 0x0000244B
		[XmlAttribute(AttributeName = "protocol")]
		public string Protocol { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00004254 File Offset: 0x00002454
		// (set) Token: 0x0600012C RID: 300 RVA: 0x0000425C File Offset: 0x0000245C
		[XmlIgnore]
		public DateTimeOffset DateInstalled { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00004268 File Offset: 0x00002468
		// (set) Token: 0x0600012E RID: 302 RVA: 0x000042A8 File Offset: 0x000024A8
		[XmlAttribute("dateInstalled")]
		public string XmlDateInstalledDoNotUse
		{
			get
			{
				string result = string.Empty;
				if (this.DateInstalled != DateTimeOffset.MinValue)
				{
					result = this.DateInstalled.ToString(Constants.DateTimeFormat, CultureInfo.InvariantCulture);
				}
				return result;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					try
					{
						this.DateInstalled = DateTimeOffset.ParseExact(value, Constants.DateTimeFormat, CultureInfo.InvariantCulture);
					}
					catch (Exception)
					{
						this.DateInstalled = DateTimeOffset.ParseExact(value, Constants.DateFormat, CultureInfo.InvariantCulture);
					}
				}
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00004300 File Offset: 0x00002500
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00004308 File Offset: 0x00002508
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
	}
}
