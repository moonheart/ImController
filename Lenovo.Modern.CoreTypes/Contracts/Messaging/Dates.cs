using System;
using System.Xml.Serialization;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging
{
	// Token: 0x0200006F RID: 111
	public sealed class Dates
	{
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x0000660A File Offset: 0x0000480A
		// (set) Token: 0x06000475 RID: 1141 RVA: 0x00006612 File Offset: 0x00004812
		[XmlIgnore]
		public int? OobeProximityDaysMin { get; set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x0000661B File Offset: 0x0000481B
		// (set) Token: 0x06000477 RID: 1143 RVA: 0x00006623 File Offset: 0x00004823
		[XmlIgnore]
		public int? OobeProximityDaysMax { get; set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x0000662C File Offset: 0x0000482C
		// (set) Token: 0x06000479 RID: 1145 RVA: 0x00006662 File Offset: 0x00004862
		[XmlAttribute("oobeProximityDaysMin")]
		public string XmlOobeProximityDaysMinDoNotUse
		{
			get
			{
				string text = string.Empty;
				if (this.OobeProximityDaysMin != null)
				{
					text += this.OobeProximityDaysMin;
				}
				return text;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					this.OobeProximityDaysMin = new int?(Convert.ToInt32(value));
				}
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x00006680 File Offset: 0x00004880
		// (set) Token: 0x0600047B RID: 1147 RVA: 0x000066B6 File Offset: 0x000048B6
		[XmlAttribute("oobeProximityDaysMax")]
		public string XmlOobeProximityDaysMaxDoNotUse
		{
			get
			{
				string text = string.Empty;
				if (this.OobeProximityDaysMax != null)
				{
					text += this.OobeProximityDaysMax;
				}
				return text;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					this.OobeProximityDaysMax = new int?(Convert.ToInt32(value));
				}
			}
		}
	}
}
