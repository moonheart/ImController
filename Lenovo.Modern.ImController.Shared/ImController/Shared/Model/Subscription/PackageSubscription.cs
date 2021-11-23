using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;
using Lenovo.Modern.ImController.Shared.Model.Packages;

namespace Lenovo.Modern.ImController.Shared.Model.Subscription
{
	// Token: 0x02000027 RID: 39
	[XmlRoot(ElementName = "ImControllerSubscription", Namespace = "")]
	public class PackageSubscription
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00006F3F File Offset: 0x0000513F
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00006F47 File Offset: 0x00005147
		[XmlAttribute("version")]
		public string Version { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00006F50 File Offset: 0x00005150
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00006F58 File Offset: 0x00005158
		[XmlIgnore]
		public DateTime DateCreated { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00006F64 File Offset: 0x00005164
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00006FA3 File Offset: 0x000051A3
		[XmlAttribute("dateCreated")]
		public string Datecreated
		{
			get
			{
				string result = string.Empty;
				if (this.DateCreated != DateTime.MinValue)
				{
					result = this.DateCreated.ToString(Constants.DateTimeFormat, CultureInfo.InvariantCulture);
				}
				return result;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					this.DateCreated = DateTime.Parse(value);
				}
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00006FB9 File Offset: 0x000051B9
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00006FC1 File Offset: 0x000051C1
		[XmlElement(ElementName = "Service")]
		public ServiceSubscription Service { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00006FCA File Offset: 0x000051CA
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00006FD2 File Offset: 0x000051D2
		[XmlIgnore]
		public DateTime DateChanged { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00006FDC File Offset: 0x000051DC
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00007016 File Offset: 0x00005216
		[XmlElement(ElementName = "datemodified")]
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

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x0000702C File Offset: 0x0000522C
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00007034 File Offset: 0x00005234
		[XmlArray("PackageList")]
		[XmlArrayItem("Package")]
		public List<Package> PackageList { get; set; }

		// Token: 0x060000F7 RID: 247 RVA: 0x00007040 File Offset: 0x00005240
		public sealed override bool Equals(object obj)
		{
			bool result = true;
			PackageSubscription packageSubscription = obj as PackageSubscription;
			if (packageSubscription != null)
			{
				if (!this.Version.Equals(packageSubscription.Version))
				{
					return false;
				}
				if (this.DateChanged < packageSubscription.DateChanged)
				{
					return false;
				}
				if (this.PackageList.Count != packageSubscription.PackageList.Count)
				{
					return false;
				}
				using (List<Package>.Enumerator enumerator = this.PackageList.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						Package package = enumerator.Current;
						return packageSubscription.PackageList.Any((Package x) => x.PackageInformation.Name == package.PackageInformation.Name && x.PackageInformation.Version == package.PackageInformation.Version && x.PackageInformation.DateChanged == package.PackageInformation.DateChanged);
					}
				}
				return result;
			}
			return result;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000710C File Offset: 0x0000530C
		public sealed override int GetHashCode()
		{
			return this.Version.GetHashCode();
		}
	}
}
