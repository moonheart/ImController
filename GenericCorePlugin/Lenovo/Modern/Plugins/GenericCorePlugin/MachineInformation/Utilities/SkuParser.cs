using System;
using System.Collections.Generic;
using System.Linq;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation.Utilities
{
	// Token: 0x0200000E RID: 14
	public static class SkuParser
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00006CD8 File Offset: 0x00004ED8
		public static Dictionary<string, string> GetValuesFromSku(string sku)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (!string.IsNullOrWhiteSpace(sku))
			{
				sku = "MFCTR_" + sku;
				List<string> list = sku.Split(new char[] { '_' }).ToList<string>();
				for (int i = 0; i < list.Count<string>(); i++)
				{
					if (list.Count >= i + 2)
					{
						string text = list[i];
						string value = list[++i];
						if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
						{
							dictionary.Add(text, value);
						}
					}
				}
			}
			return dictionary;
		}
	}
}
