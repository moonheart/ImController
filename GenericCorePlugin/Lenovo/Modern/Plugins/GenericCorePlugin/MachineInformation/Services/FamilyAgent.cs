using System;
using System.Collections.Generic;
using Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation.Utilities;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation.Services
{
	// Token: 0x02000011 RID: 17
	public class FamilyAgent
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x00007014 File Offset: 0x00005214
		public string GetFamily(string familyFromBios, string skuFromBios)
		{
			Func<string, bool> func = (string value) => !string.IsNullOrWhiteSpace(value) && value.Length > 3 && !string.IsNullOrWhiteSpace(value) && !value.Contains("O.E.M.", StringComparison.OrdinalIgnoreCase) && !value.Contains("OEM", StringComparison.OrdinalIgnoreCase);
			try
			{
				string familyFromSku = this.GetFamilyFromSku(skuFromBios);
				if (func(familyFromSku))
				{
					return familyFromSku;
				}
				if (func(familyFromBios))
				{
					return familyFromBios;
				}
				string text = this.GetFamilyFromRegistry();
				if (func(text))
				{
					return text;
				}
				text = this.GetFamilyFromRegistrySku();
				if (func(text))
				{
					return text;
				}
				Logger.Log(Logger.LogSeverity.Error, "FamilyAgent: Unable to identify a valid family for device");
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "FamilyAgent: Exception thrown getting the family for the machine");
			}
			return string.Empty;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000070BC File Offset: 0x000052BC
		private string GetFamilyFromSku(string sku)
		{
			string result = string.Empty;
			try
			{
				if (!string.IsNullOrWhiteSpace(sku))
				{
					Dictionary<string, string> valuesFromSku = SkuParser.GetValuesFromSku(sku);
					if (valuesFromSku != null && valuesFromSku.ContainsKey("FM"))
					{
						string text = valuesFromSku["FM"];
						if (!string.IsNullOrWhiteSpace(text))
						{
							result = text;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "FamilyAgent: Exception thrown getting the family from sku");
			}
			return result;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00007124 File Offset: 0x00005324
		private string GetFamilyFromRegistry()
		{
			string result = string.Empty;
			try
			{
				IContainer container = new RegistrySystem().LoadContainer(Constants.HardwareConfigRegistryKey);
				if (container != null)
				{
					IContainerValue value = container.GetValue("SystemFamily");
					if (value != null)
					{
						result = value.GetValueAsString();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "FamilyAgent: Exception thrown getting the family from registry");
			}
			return result;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00007180 File Offset: 0x00005380
		private string GetFamilyFromRegistrySku()
		{
			string result = string.Empty;
			try
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
							Dictionary<string, string> valuesFromSku = SkuParser.GetValuesFromSku(valueAsString);
							if (valuesFromSku != null && valuesFromSku.ContainsKey("FM"))
							{
								string text = valuesFromSku["FM"];
								if (!string.IsNullOrWhiteSpace(text))
								{
									result = text;
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "FamilyAgent: Exception thrown getting the family from registry SKU");
			}
			return result;
		}
	}
}
