using System;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.Plugins.GenericCorePlugin.MachineInformation.Services
{
	// Token: 0x02000013 RID: 19
	public class OperatingSystemAgent
	{
		// Token: 0x060000B9 RID: 185 RVA: 0x000072A0 File Offset: 0x000054A0
		public string GetOperatingSystemProductName()
		{
			string result = string.Empty;
			try
			{
				IContainer container = new RegistrySystem().LoadContainer(Constants.WindowsCurrentVersionRegistryKey);
				if (container != null)
				{
					IContainerValue value = container.GetValue(Constants.WindowsProductNameRegistryValue);
					if (value != null)
					{
						result = value.GetValueAsString();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown getting the operating system product name");
			}
			return result;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000072FC File Offset: 0x000054FC
		public string GetOperatingSystemProductEditionID()
		{
			string result = string.Empty;
			try
			{
				IContainer container = new RegistrySystem().LoadContainer(Constants.WindowsCurrentVersionRegistryKey);
				if (container != null)
				{
					IContainerValue value = container.GetValue(Constants.WindowsEditionIDRegistryValue);
					if (value != null)
					{
						result = value.GetValueAsString();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown getting the operating system product edition ID");
			}
			return result;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00007358 File Offset: 0x00005558
		public string GetOperatingSystemBuildNumber()
		{
			string result = string.Empty;
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = string.Empty;
			string text4 = string.Empty;
			try
			{
				IContainer container = new RegistrySystem().LoadContainer(Constants.WindowsCurrentVersionRegistryKey);
				if (container != null)
				{
					IContainerValue value = container.GetValue(Constants.WindowsMajorVersionNumberRegistryValue);
					if (value != null)
					{
						text = value.GetValueAsInt().ToString();
					}
					IContainerValue value2 = container.GetValue(Constants.WindowsMinorVersionNumberRegistryValue);
					if (value2 != null)
					{
						text2 = value2.GetValueAsInt().ToString();
					}
					IContainerValue value3 = container.GetValue(Constants.WindowsBuildNumberRegistryValue);
					if (value3 != null)
					{
						text3 = value3.GetValueAsString();
					}
					IContainerValue value4 = container.GetValue(Constants.WindowsUBRRegistryValue);
					if (value4 != null)
					{
						text4 = value4.GetValueAsInt().ToString();
					}
				}
				if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(text2) && !string.IsNullOrWhiteSpace(text3) && !string.IsNullOrWhiteSpace(text4))
				{
					result = string.Format("{0}.{1}.{2}.{3}", new object[] { text, text2, text3, text4 });
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown getting the operating system build number");
			}
			return result;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00007494 File Offset: 0x00005694
		public string GetOS()
		{
			string result = string.Empty;
			string operatingSystemProductName = this.GetOperatingSystemProductName();
			double num = 0.0;
			try
			{
				string[] array = operatingSystemProductName.Split(new char[0]);
				int num2 = 0;
				while (!double.TryParse(array[num2], out num))
				{
					num2++;
				}
				result = operatingSystemProductName.Substring(0, 3).ToUpperInvariant() + num.ToString();
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown getting the OS");
			}
			return result;
		}
	}
}
