using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;
using Lenovo.Modern.CoreTypes.Models;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging
{
	// Token: 0x02000076 RID: 118
	public sealed class MessageAction : IBasicEligabilityRequirements
	{
		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x00006D1C File Offset: 0x00004F1C
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x00006D24 File Offset: 0x00004F24
		[XmlAttribute(AttributeName = "type")]
		public MessageActionType Type { get; set; }

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x00006D2D File Offset: 0x00004F2D
		// (set) Token: 0x060004A6 RID: 1190 RVA: 0x00006D35 File Offset: 0x00004F35
		[XmlAttribute(AttributeName = "country")]
		public string Country { get; set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00006D3E File Offset: 0x00004F3E
		// (set) Token: 0x060004A8 RID: 1192 RVA: 0x00006D46 File Offset: 0x00004F46
		[XmlIgnore]
		public BrandType Brand { get; set; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x00006D50 File Offset: 0x00004F50
		// (set) Token: 0x060004AA RID: 1194 RVA: 0x00006D78 File Offset: 0x00004F78
		[XmlAttribute(AttributeName = "brand")]
		public string XmlBrandDoNotUse
		{
			get
			{
				return this.Brand.ToString().ToLowerInvariant();
			}
			set
			{
				BrandType brand = BrandType.None;
				if (value != null && !Enum.TryParse<BrandType>(value.Trim(), true, out brand))
				{
					Enum.TryParse<BrandType>(value.Trim().Replace("-", string.Empty).Replace(" ", string.Empty)
						.Replace("_", string.Empty), true, out brand);
				}
				this.Brand = brand;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x00006DDD File Offset: 0x00004FDD
		// (set) Token: 0x060004AC RID: 1196 RVA: 0x00006DE5 File Offset: 0x00004FE5
		[XmlAttribute(AttributeName = "language")]
		public string Language { get; set; }

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00006DEE File Offset: 0x00004FEE
		// (set) Token: 0x060004AE RID: 1198 RVA: 0x00006DF6 File Offset: 0x00004FF6
		[XmlAttribute(AttributeName = "subBrand")]
		public string SubBrand { get; set; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x00006DFF File Offset: 0x00004FFF
		// (set) Token: 0x060004B0 RID: 1200 RVA: 0x00006E07 File Offset: 0x00005007
		[XmlAttribute(AttributeName = "friendlyName")]
		public string FriendlyName { get; set; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00006E10 File Offset: 0x00005010
		// (set) Token: 0x060004B2 RID: 1202 RVA: 0x00006E18 File Offset: 0x00005018
		[XmlAttribute(AttributeName = "enclosureType")]
		public EnclosureType EnclosureType { get; set; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00006E21 File Offset: 0x00005021
		// (set) Token: 0x060004B4 RID: 1204 RVA: 0x00006E29 File Offset: 0x00005029
		[XmlAttribute(AttributeName = "appVersion")]
		public string AppVersion { get; set; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00006E32 File Offset: 0x00005032
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x00006E3A File Offset: 0x0000503A
		[XmlText]
		public string Command { get; set; }

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00006E43 File Offset: 0x00005043
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x00006E4B File Offset: 0x0000504B
		[XmlAttribute(AttributeName = "toastInterval")]
		public int ToastInterval { get; set; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00006E54 File Offset: 0x00005054
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x00006E5C File Offset: 0x0000505C
		[XmlAttribute(AttributeName = "toastRepeat")]
		public int ToastRepeat { get; set; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00006E65 File Offset: 0x00005065
		// (set) Token: 0x060004BC RID: 1212 RVA: 0x00006E6D File Offset: 0x0000506D
		[XmlAttribute(AttributeName = "force")]
		public bool Force { get; set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00006E76 File Offset: 0x00005076
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x00006E7E File Offset: 0x0000507E
		[XmlAttribute(AttributeName = "osVersion")]
		public string OsVersion { get; set; }

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x00006E87 File Offset: 0x00005087
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x00006E8F File Offset: 0x0000508F
		[XmlAttribute(AttributeName = "osBitness")]
		public string OsBitness { get; set; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00006E98 File Offset: 0x00005098
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x00006EA0 File Offset: 0x000050A0
		[XmlAttribute(AttributeName = "tag")]
		public string Tag { get; set; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00006EA9 File Offset: 0x000050A9
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x00006EB1 File Offset: 0x000050B1
		[XmlAttribute(AttributeName = "manufacturer")]
		public string Manufacturer { get; set; }

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00006EBA File Offset: 0x000050BA
		// (set) Token: 0x060004C6 RID: 1222 RVA: 0x00006EC2 File Offset: 0x000050C2
		[XmlAttribute(AttributeName = "family")]
		public string Family { get; set; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00006ECB File Offset: 0x000050CB
		// (set) Token: 0x060004C8 RID: 1224 RVA: 0x00006ED3 File Offset: 0x000050D3
		[XmlAttribute(AttributeName = "mtm")]
		public string Mtm { get; set; }

		// Token: 0x060004C9 RID: 1225 RVA: 0x00006EDC File Offset: 0x000050DC
		public MessageAction()
			: this(MessageActionType.Other, string.Empty, string.Empty)
		{
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00006EEF File Offset: 0x000050EF
		public MessageAction(MessageActionType type, string country, string command)
		{
			this.Type = type;
			this.Country = country;
			this.Command = command;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00006F0C File Offset: 0x0000510C
		public MessageAction(MessageActionType type, string command)
		{
			this.Type = type;
			this.Country = string.Empty;
			this.Command = command;
		}

		// Token: 0x0400028A RID: 650
		private static readonly Dictionary<MessageActionType, string> MessageActionTypeValues = new Dictionary<MessageActionType, string>
		{
			{
				MessageActionType.Other,
				"other"
			},
			{
				MessageActionType.Protocol,
				"protocol"
			},
			{
				MessageActionType.WebAddress,
				"webaddress"
			},
			{
				MessageActionType.InstalledItem,
				"installeditem"
			}
		};
	}
}
