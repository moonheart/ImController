using System;
using System.Xml.Serialization;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;

namespace Lenovo.Modern.CoreTypes.Contracts.Messaging
{
	// Token: 0x0200007B RID: 123
	public sealed class NegatableBrand
	{
		// Token: 0x0600050D RID: 1293 RVA: 0x000072CC File Offset: 0x000054CC
		public NegatableBrand()
			: this(false, BrandType.Other)
		{
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000072D6 File Offset: 0x000054D6
		public NegatableBrand(bool exclude, BrandType brand)
		{
			this.Exclude = exclude;
			this.Value = brand;
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x000072EC File Offset: 0x000054EC
		// (set) Token: 0x06000510 RID: 1296 RVA: 0x000072F4 File Offset: 0x000054F4
		[XmlAttribute(AttributeName = "exclude")]
		public bool Exclude { get; set; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x000072FD File Offset: 0x000054FD
		// (set) Token: 0x06000512 RID: 1298 RVA: 0x00007305 File Offset: 0x00005505
		[XmlText]
		public BrandType Value { get; set; }
	}
}
