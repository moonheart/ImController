using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation.Services
{
	// Token: 0x02000010 RID: 16
	public class CountryCodeAgent
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00006ECC File Offset: 0x000050CC
		public string GetCountryCode()
		{
			string text = string.Empty;
			try
			{
				text = this.GetCountryCodePinvoke();
				if (string.IsNullOrWhiteSpace(text))
				{
					text = this.GetCountryCodeFromLocaleName();
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown when trying to open the registry key for the country code");
			}
			return text;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00006F14 File Offset: 0x00005114
		private string GetCountryCodePinvoke()
		{
			string text = string.Empty;
			try
			{
				IContainer container = new RegistrySystem().LoadContainer("HKEY_CURRENT_USER\\Control Panel\\International\\Geo");
				if (container != null)
				{
					int? valueAsInt = container.GetValue("Nation").GetValueAsInt();
					if (valueAsInt != null)
					{
						text = new string(' ', 256);
						int geoInfo = CountryCodeAgent.GetGeoInfo(valueAsInt.Value, CountryCodeAgent.SYSGEOTYPE.GEO_ISO2, text, 256, 0);
						if (geoInfo <= 0)
						{
							return null;
						}
						text = text.Substring(0, geoInfo - 1);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the country code using the pinvoke method");
			}
			return text;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00006FAC File Offset: 0x000051AC
		private string GetCountryCodeFromLocaleName()
		{
			string result = string.Empty;
			try
			{
				IContainer container = new RegistrySystem().LoadContainer("HKEY_CURRENT_USER\\Control Panel\\International");
				if (container != null)
				{
					IContainerValue value = container.GetValue("LocaleName");
					if (value != null)
					{
						result = new RegionInfo(value.GetValueAsString()).TwoLetterISORegionName;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get the country code using the region info method");
			}
			return result;
		}

		// Token: 0x060000AF RID: 175
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetGeoInfo(int geoId, CountryCodeAgent.SYSGEOTYPE geoType, string lpGeoData, int cchData, int language);

		// Token: 0x02000068 RID: 104
		public enum SYSGEOTYPE
		{
			// Token: 0x04000144 RID: 324
			GEO_NATION = 1,
			// Token: 0x04000145 RID: 325
			GEO_LATITUDE,
			// Token: 0x04000146 RID: 326
			GEO_LONGITUDE,
			// Token: 0x04000147 RID: 327
			GEO_ISO2,
			// Token: 0x04000148 RID: 328
			GEO_ISO3,
			// Token: 0x04000149 RID: 329
			GEO_RFC1766,
			// Token: 0x0400014A RID: 330
			GEO_LCID,
			// Token: 0x0400014B RID: 331
			GEO_FRIENDLYNAME,
			// Token: 0x0400014C RID: 332
			GEO_OFFICIALNAME,
			// Token: 0x0400014D RID: 333
			GEO_TIMEZONES,
			// Token: 0x0400014E RID: 334
			GEO_OFFICIALLANGUAGES
		}
	}
}
