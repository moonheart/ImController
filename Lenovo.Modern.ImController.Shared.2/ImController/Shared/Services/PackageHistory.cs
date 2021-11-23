using System;
using System.Reflection;
using Lenovo.Modern.ImController.Shared.Model;
using Lenovo.Modern.ImController.Shared.Utilities;
using Lenovo.Modern.Utilities.Services.Logging;
using Lenovo.Modern.Utilities.Services.Wrappers.Settings;

namespace Lenovo.Modern.ImController.Shared.Services
{
	// Token: 0x02000014 RID: 20
	public class PackageHistory : IPackageHistory
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00003B14 File Offset: 0x00001D14
		public bool CachePackageInformation(CacheInformation packageInfo, bool updateCurrentInstalledVersion = true)
		{
			bool result = false;
			if (packageInfo != null)
			{
				IContainer container = new RegistrySystem().LoadContainer("HKEY_LOCAL_MACHINE\\SOFTWARE\\Lenovo");
				if (container != null)
				{
					container.CreateSubContainer("ImController\\Packages");
					IContainer subContainer = container.GetSubContainer("ImController\\Packages");
					if (subContainer != null)
					{
						string name = packageInfo.Name;
						IContainer subContainer2 = subContainer.GetSubContainer(name);
						if (subContainer2 == null)
						{
							if (subContainer.CreateSubContainer(name))
							{
								IContainer subContainer3 = subContainer.GetSubContainer(name);
								if (subContainer3 != null)
								{
									if (updateCurrentInstalledVersion)
									{
										subContainer3.SetValue("CurrentInstalledVersion", packageInfo.Version);
									}
									if (subContainer3.CreateSubContainer(packageInfo.Version))
									{
										IContainer subContainer4 = subContainer3.GetSubContainer(packageInfo.Version);
										if (subContainer4 != null)
										{
											subContainer4.SetValue("DateLastModified", packageInfo.DateLastModified);
											subContainer4.SetValue("Name", packageInfo.Name);
											subContainer4.SetValue("Location", packageInfo.Location);
											subContainer4.SetValue("Version", packageInfo.Version);
											subContainer4.SetValue("NumberOfInstallAttempts", packageInfo.NumberOfInstallAttempts.ToString());
											IContainer container2 = subContainer4;
											string valueName = "ImControllerVersion";
											Assembly executingAssembly = Assembly.GetExecutingAssembly();
											string value;
											if (executingAssembly == null)
											{
												value = null;
											}
											else
											{
												AssemblyName name2 = executingAssembly.GetName();
												if (name2 == null)
												{
													value = null;
												}
												else
												{
													Version version = name2.Version;
													value = ((version != null) ? version.ToString() : null);
												}
											}
											container2.SetValue(valueName, value);
											result = true;
										}
									}
								}
							}
						}
						else if (subContainer2 != null)
						{
							IContainer subContainer5 = subContainer2.GetSubContainer(packageInfo.Version);
							if (subContainer5 == null)
							{
								if (subContainer2.CreateSubContainer(packageInfo.Version))
								{
									IContainer subContainer6 = subContainer2.GetSubContainer(packageInfo.Version);
									if (subContainer6 != null)
									{
										subContainer6.SetValue("DateLastModified", packageInfo.DateLastModified);
										subContainer6.SetValue("Name", packageInfo.Name);
										subContainer6.SetValue("Location", packageInfo.Location);
										subContainer6.SetValue("Version", packageInfo.Version);
										subContainer6.SetValue("NumberOfInstallAttempts", packageInfo.NumberOfInstallAttempts.ToString());
										IContainer container3 = subContainer6;
										string valueName2 = "ImControllerVersion";
										Assembly executingAssembly2 = Assembly.GetExecutingAssembly();
										string value2;
										if (executingAssembly2 == null)
										{
											value2 = null;
										}
										else
										{
											AssemblyName name3 = executingAssembly2.GetName();
											if (name3 == null)
											{
												value2 = null;
											}
											else
											{
												Version version2 = name3.Version;
												value2 = ((version2 != null) ? version2.ToString() : null);
											}
										}
										container3.SetValue(valueName2, value2);
										result = true;
									}
								}
							}
							else
							{
								subContainer5.SetValue("NumberOfInstallAttempts", packageInfo.NumberOfInstallAttempts.ToString());
								IContainer container4 = subContainer5;
								string valueName3 = "ImControllerVersion";
								Assembly executingAssembly3 = Assembly.GetExecutingAssembly();
								string value3;
								if (executingAssembly3 == null)
								{
									value3 = null;
								}
								else
								{
									AssemblyName name4 = executingAssembly3.GetName();
									if (name4 == null)
									{
										value3 = null;
									}
									else
									{
										Version version3 = name4.Version;
										value3 = ((version3 != null) ? version3.ToString() : null);
									}
								}
								container4.SetValue(valueName3, value3);
								result = true;
							}
							if (updateCurrentInstalledVersion)
							{
								subContainer2.SetValue("CurrentInstalledVersion", packageInfo.Version);
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003DC8 File Offset: 0x00001FC8
		public CacheInformation GetPackageInformationFromCache(string name, string version)
		{
			CacheInformation cacheInformation = null;
			try
			{
				if (!string.IsNullOrWhiteSpace(name))
				{
					string path = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Lenovo\\ImController\\Packages\\" + name;
					IContainer container = ((IContainerSystem)new RegistrySystem()).LoadContainer(path);
					if (container != null && !Utility.SanitizeVersion(ref version))
					{
						IContainer subContainer = container.GetSubContainer(version);
						if (subContainer != null)
						{
							cacheInformation = new CacheInformation
							{
								DateLastModified = subContainer.GetValue("DateLastModified").GetValueAsString(),
								Location = subContainer.GetValue("Location").GetValueAsString(),
								Name = subContainer.GetValue("Name").GetValueAsString(),
								NumberOfInstallAttempts = subContainer.GetValue("NumberOfInstallAttempts").GetValueAsInt().GetValueOrDefault(),
								Version = subContainer.GetValue("Version").GetValueAsString()
							};
							IContainerValue value = subContainer.GetValue("ImControllerVersion");
							Version version2 = new Version("0.0.0.0");
							if (value != null)
							{
								Version.TryParse(value.GetValueAsString(), out version2);
							}
							if (version2 == null || (null != Assembly.GetExecutingAssembly() && Assembly.GetExecutingAssembly().GetName().Version.CompareTo(version2) != 0))
							{
								cacheInformation.NumberOfInstallAttempts = 0;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get package information from cache");
			}
			return cacheInformation;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003F2C File Offset: 0x0000212C
		public Version GetCurrentInstalledVersionOfPackage(string name)
		{
			Version result = null;
			try
			{
				if (!string.IsNullOrWhiteSpace(name))
				{
					string path = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Lenovo\\ImController\\Packages\\" + name;
					IContainer container = ((IContainerSystem)new RegistrySystem()).LoadContainer(path);
					if (container != null)
					{
						result = new Version(container.GetValue("CurrentInstalledVersion").GetValueAsString());
					}
					else
					{
						result = null;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex, "Exception thrown trying to get package information from cache. Default null version will be used");
				result = null;
			}
			return result;
		}
	}
}
