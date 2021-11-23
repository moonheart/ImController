using System;
using System.Collections.Generic;
using System.Linq;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation.Services
{
	// Token: 0x02000014 RID: 20
	public class SubBrandAgent
	{
		// Token: 0x060000BE RID: 190 RVA: 0x00007518 File Offset: 0x00005718
		public string GetSubBrand(string skuFromBios, string family, string brand)
		{
			string text = this.GetSubBrandFromSku(family);
			if (string.IsNullOrWhiteSpace(text))
			{
				text = this.GetSubBrandFromBU(brand);
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				text = this.GetSubBrandFromSku(skuFromBios);
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				IContainer container = new RegistrySystem().LoadContainer(Constants.HardwareConfigRegistryKey);
				if (container != null)
				{
					IContainerValue value = container.GetValue("SystemSKU");
					if (value != null)
					{
						string valueAsString = value.GetValueAsString();
						if (!string.IsNullOrWhiteSpace(valueAsString))
						{
							text = this.GetSubBrandFromSku(valueAsString);
						}
					}
				}
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				text = this.GetSubBrandForYogaSystems(skuFromBios);
			}
			return text;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000075A0 File Offset: 0x000057A0
		private string GetSubBrandFromSku(string sku)
		{
			string text = string.Empty;
			try
			{
				if (!string.IsNullOrWhiteSpace(sku))
				{
					text = SubBrandAgent.knownSubBrands.FirstOrDefault((string knownSubBrand) => sku.Contains(knownSubBrand, StringComparison.OrdinalIgnoreCase));
					if (!string.IsNullOrWhiteSpace(text) && sku.Contains("Edge", StringComparison.OrdinalIgnoreCase))
					{
						text += " Edge";
					}
					if (!string.IsNullOrWhiteSpace(text) && text == "ThinkCenter")
					{
						text = "ThinkCentre";
					}
					if (!string.IsNullOrWhiteSpace(text) && text == "IdeaCenter")
					{
						text = "IdeaCentre";
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown getting the system sub-brand");
			}
			return text;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00007660 File Offset: 0x00005860
		private string GetSubBrandFromBU(string brand)
		{
			string result = string.Empty;
			try
			{
				if (!string.IsNullOrWhiteSpace(brand))
				{
					result = SubBrandAgent.knownSubBrands.FirstOrDefault((string knownSubBrand) => brand.Contains(knownSubBrand, StringComparison.OrdinalIgnoreCase));
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown getting the system sub-brand from BU");
			}
			return result;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000076C4 File Offset: 0x000058C4
		private string GetSubBrandForYogaSystems(string sku)
		{
			string result = string.Empty;
			string text = sku.ToLower();
			if (!string.IsNullOrWhiteSpace(sku) && text.Contains("yoga"))
			{
				if (text.Contains("idea"))
				{
					result = "IdeaPad";
				}
				else if (text.Contains("think"))
				{
					result = "ThinkPad";
				}
			}
			return result;
		}

		// Token: 0x04000042 RID: 66
		private static List<string> knownSubBrands = new List<string>
		{
			"ThinkPad", "ThinkCentre", "ThinkCenter", "ThinkStation", "IdeaPad", "IdeaCentre", "IdeaCenter", "IdeaTab", "Erazer", "Medion",
			"Lenovo"
		};
	}
}
