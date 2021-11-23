using System;
using System.Collections.Generic;
using Lenovo.Modern.CoreTypes.Contracts.SystemInformation;
using Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation.Utilities;
using Lenovo.Modern.Utilities.Services.Logging;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation.Services
{
	// Token: 0x0200000F RID: 15
	public class BrandAgent
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00006D64 File Offset: 0x00004F64
		public BrandType GetBrand(string skuFromBios, string familyFromBios, string manufacturerFromBios)
		{
			BrandType brandType = BrandType.None;
			try
			{
				brandType = this.GetBrandFromSku(skuFromBios);
				if (brandType == BrandType.None)
				{
					brandType = this.GetBrandFromFamily(familyFromBios);
					if (brandType == BrandType.None)
					{
						brandType = this.GetBrandFromManufacturer(manufacturerFromBios);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown getting the system brand");
			}
			return brandType;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00006DB0 File Offset: 0x00004FB0
		private BrandType GetBrandFromSku(string sku)
		{
			BrandType result = BrandType.None;
			if (!string.IsNullOrWhiteSpace(sku))
			{
				Dictionary<string, string> valuesFromSku = SkuParser.GetValuesFromSku(sku);
				if (valuesFromSku != null && valuesFromSku.ContainsKey("BU"))
				{
					string text = valuesFromSku["BU"];
					if (!string.IsNullOrWhiteSpace(text))
					{
						if (text.Contains(Constants.BrandNames.Think, StringComparison.OrdinalIgnoreCase))
						{
							result = BrandType.Think;
						}
						else if (text.Contains(Constants.BrandNames.Idea, StringComparison.OrdinalIgnoreCase))
						{
							result = BrandType.Idea;
						}
						else if (text.Contains(Constants.BrandNames.Lenovo, StringComparison.OrdinalIgnoreCase))
						{
							result = BrandType.Lenovo;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00006E28 File Offset: 0x00005028
		private BrandType GetBrandFromFamily(string family)
		{
			BrandType result = BrandType.None;
			if (!string.IsNullOrWhiteSpace(family))
			{
				string text = family.ToLowerInvariant();
				if (text.Contains("think"))
				{
					result = BrandType.Think;
				}
				else if (text.Contains("idea"))
				{
					if (text.Contains("ideatab"))
					{
						result = BrandType.IdeaTab;
					}
					else
					{
						result = BrandType.Idea;
					}
				}
				else if (text.Contains("lenovo"))
				{
					result = BrandType.Lenovo;
				}
				else if (text.Contains("medion"))
				{
					result = BrandType.Medion;
				}
			}
			return result;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00006E9C File Offset: 0x0000509C
		private BrandType GetBrandFromManufacturer(string manufacturer)
		{
			BrandType result = BrandType.None;
			if (!string.IsNullOrWhiteSpace(manufacturer))
			{
				if (manufacturer.ToLowerInvariant().Contains("lenovo"))
				{
					result = BrandType.Lenovo;
				}
				else
				{
					result = BrandType.Other;
				}
			}
			return result;
		}
	}
}
